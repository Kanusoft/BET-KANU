using BetKanu.Data;
using BetKanu.Models;
using BetKanu.Models.Common;
using BetKanu.Models.DTOs.Collaboration;
using BET_KANU.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BetKanu.Data.Repositories.Collaboration;

public sealed class CollaborationMediaService : ICollaborationMediaService
{
    private readonly BKdbContext _db;
    private readonly IBlobStorageService _blob;
    private readonly ILogger<CollaborationMediaService> _logger;
    private readonly CollaborationUploadOptions _uploadOptions;

    public CollaborationMediaService(
        BKdbContext db,
        IBlobStorageService blob,
        IOptions<CollaborationUploadOptions> uploadOptions,
        ILogger<CollaborationMediaService> logger)
    {
        _db = db;
        _blob = blob;
        _logger = logger;
        _uploadOptions = uploadOptions.Value;
    }

    public async Task<CollaborationMediaDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.CollaborationMedias.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        return entity is null ? null : MapDto(entity);
    }

    public async Task<IReadOnlyList<CollaborationMediaDto>> GetByPartnerContributorIdAsync(
        int partnerContributorId,
        CancellationToken ct = default)
    {
        var rows = await _db.CollaborationMedias.AsNoTracking()
            .Where(x => x.PartnerContributorId == partnerContributorId)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);

        return rows.Select(MapDto).ToList();
    }

    public async Task<IReadOnlyList<CollaborationMediaDto>> GetByCollaborationProjectIdAsync(
        int collaborationProjectId,
        CancellationToken ct = default)
    {
        var rows = await _db.CollaborationMedias.AsNoTracking()
            .Where(x => x.CollaborationProjectId == collaborationProjectId)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);

        return rows.Select(MapDto).ToList();
    }

    public async Task<IReadOnlyList<CollaborationMediaDto>> GetActiveForPublicAsync(
        int? partnerContributorId = null,
        int? collaborationProjectId = null,
        CollaborationMediaType? mediaType = null,
        CancellationToken ct = default)
    {
        var query = _db.CollaborationMedias.AsNoTracking().Where(x => x.IsActive);

        if (partnerContributorId.HasValue)
            query = query.Where(x => x.PartnerContributorId == partnerContributorId.Value);

        if (collaborationProjectId.HasValue)
            query = query.Where(x => x.CollaborationProjectId == collaborationProjectId.Value);

        if (mediaType.HasValue)
            query = query.Where(x => x.MediaType == mediaType.Value);

        var rows = await query
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Id)
            .ToListAsync(ct);

        return rows.Select(MapDto).ToList();
    }

    public async Task<ServiceResult<CollaborationMediaDto>> UploadAsync(
        CollaborationMediaUploadDto dto,
        IFormFile file,
        CancellationToken ct = default)
    {
        var parentValidation = await ValidateParentAsync(dto.PartnerContributorId, dto.CollaborationProjectId, ct);
        if (!parentValidation.Succeeded)
            return ServiceResult<CollaborationMediaDto>.From(parentValidation);

        var fileValidation = CollaborationFileValidator.Validate(file, _uploadOptions);
        if (!fileValidation.IsValid)
            return ServiceResult<CollaborationMediaDto>.Failure(fileValidation.Error!);

        var blobName = CollaborationValidation.BuildBlobPath("media", fileValidation.SanitizedFileName);
        string? uploadedBlob = null;

        try
        {
            await using var stream = file.OpenReadStream();
            var upload = await _blob.UploadAsync(stream, blobName, file.ContentType, ct);
            uploadedBlob = upload.BlobName;

            var entity = new CollaborationMedia
            {
                PartnerContributorId = dto.PartnerContributorId,
                CollaborationProjectId = dto.CollaborationProjectId,
                FileName = fileValidation.SanitizedFileName,
                FileUrl = upload.Url,
                BlobName = upload.BlobName,
                ContentType = file.ContentType,
                FileSizeBytes = file.Length,
                MediaType = fileValidation.MediaType,
                Title = CollaborationValidation.NormalizeOptional(dto.Title),
                Caption = CollaborationValidation.NormalizeOptional(dto.Caption),
                Description = CollaborationValidation.NormalizeOptional(dto.Description),
                DisplayOrder = dto.DisplayOrder,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _db.CollaborationMedias.Add(entity);
            await _db.SaveChangesAsync(ct);
            return ServiceResult<CollaborationMediaDto>.Success(MapDto(entity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Collaboration media upload failed");
            if (!string.IsNullOrWhiteSpace(uploadedBlob))
            {
                try { await _blob.DeleteDocumentAsync(uploadedBlob); }
                catch (Exception cleanupEx)
                {
                    _logger.LogError(cleanupEx, "Failed cleaning orphaned media blob {Blob}", uploadedBlob);
                }
            }

            return ServiceResult<CollaborationMediaDto>.Failure("Media upload failed.");
        }
    }

    public async Task<ServiceResult<IReadOnlyList<CollaborationMediaDto>>> UploadManyAsync(
        CollaborationMediaUploadDto dto,
        IReadOnlyList<IFormFile> files,
        CancellationToken ct = default)
    {
        if (files == null || files.Count == 0)
            return ServiceResult<IReadOnlyList<CollaborationMediaDto>>.Failure("At least one file is required.");

        var results = new List<CollaborationMediaDto>();
        var errors = new List<string>();

        foreach (var file in files)
        {
            var result = await UploadAsync(dto, file, ct);
            if (result.Succeeded && result.Data != null)
                results.Add(result.Data);
            else
                errors.Add($"{file.FileName}: {result.Error}");
        }

        if (results.Count == 0)
            return ServiceResult<IReadOnlyList<CollaborationMediaDto>>.Failure(errors);

        if (errors.Count > 0)
        {
            return new ServiceResult<IReadOnlyList<CollaborationMediaDto>>
            {
                Succeeded = true,
                Data = results,
                Errors = errors
            };
        }

        return ServiceResult<IReadOnlyList<CollaborationMediaDto>>.Success(results);
    }

    public async Task<ServiceResult<CollaborationMediaDto>> UpdateAsync(
        int id,
        CollaborationMediaUpdateDto dto,
        CancellationToken ct = default)
    {
        var entity = await _db.CollaborationMedias.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult<CollaborationMediaDto>.Failure("Media was not found.");

        if (!string.IsNullOrWhiteSpace(dto.Title) && dto.Title.Length > 250)
            return ServiceResult<CollaborationMediaDto>.Failure("Title cannot exceed 250 characters.");

        if (!string.IsNullOrWhiteSpace(dto.Caption) && dto.Caption.Length > 1000)
            return ServiceResult<CollaborationMediaDto>.Failure("Caption cannot exceed 1000 characters.");

        if (!string.IsNullOrWhiteSpace(dto.ThumbnailUrl) && dto.ThumbnailUrl.Length > 1500)
            return ServiceResult<CollaborationMediaDto>.Failure("Thumbnail URL cannot exceed 1500 characters.");

        if (dto.DisplayOrder < 0)
            return ServiceResult<CollaborationMediaDto>.Failure("Display order must be zero or greater.");

        entity.Title = CollaborationValidation.NormalizeOptional(dto.Title);
        entity.Caption = CollaborationValidation.NormalizeOptional(dto.Caption);
        entity.Description = CollaborationValidation.NormalizeOptional(dto.Description);
        entity.ThumbnailUrl = CollaborationValidation.NormalizeOptional(dto.ThumbnailUrl);
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsActive = dto.IsActive;

        await _db.SaveChangesAsync(ct);
        return ServiceResult<CollaborationMediaDto>.Success(MapDto(entity));
    }

    public async Task<ServiceResult> UpdateDisplayOrderAsync(int id, int displayOrder, CancellationToken ct = default)
    {
        if (displayOrder < 0)
            return ServiceResult.Failure("Display order must be zero or greater.");

        var entity = await _db.CollaborationMedias.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult.Failure("Media was not found.");

        entity.DisplayOrder = displayOrder;
        await _db.SaveChangesAsync(ct);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> SetActiveAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await _db.CollaborationMedias.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult.Failure("Media was not found.");

        entity.IsActive = isActive;
        await _db.SaveChangesAsync(ct);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.CollaborationMedias.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult.Failure("Media was not found.");

        var blobName = entity.BlobName
                       ?? CollaborationValidation.TryExtractBlobNameFromUrl(entity.FileUrl);

        _db.CollaborationMedias.Remove(entity);
        await _db.SaveChangesAsync(ct);

        await TryDeleteBlobIfUnreferencedAsync(blobName, excludedMediaId: id);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult<string>> GetDownloadUrlAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.CollaborationMedias.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return ServiceResult<string>.Failure("Media was not found.");

        if (!string.IsNullOrWhiteSpace(entity.FileUrl))
            return ServiceResult<string>.Success(entity.FileUrl);

        if (!string.IsNullOrWhiteSpace(entity.BlobName))
            return ServiceResult<string>.Success(_blob.GetPublicUrl(entity.BlobName));

        return ServiceResult<string>.Failure("No download URL is available for this media item.");
    }

    internal static CollaborationMediaDto MapDto(CollaborationMedia x) => new()
    {
        Id = x.Id,
        PartnerContributorId = x.PartnerContributorId,
        CollaborationProjectId = x.CollaborationProjectId,
        FileName = x.FileName,
        FileUrl = x.FileUrl,
        BlobName = x.BlobName,
        ContentType = x.ContentType,
        FileSizeBytes = x.FileSizeBytes,
        MediaType = x.MediaType,
        Title = x.Title,
        Caption = x.Caption,
        Description = x.Description,
        ThumbnailUrl = x.ThumbnailUrl,
        DisplayOrder = x.DisplayOrder,
        IsActive = x.IsActive,
        CreatedAt = x.CreatedAt
    };

    private async Task<ServiceResult> ValidateParentAsync(
        int? partnerContributorId,
        int? collaborationProjectId,
        CancellationToken ct)
    {
        var hasPartner = partnerContributorId.HasValue && partnerContributorId.Value > 0;
        var hasProject = collaborationProjectId.HasValue && collaborationProjectId.Value > 0;

        if (hasPartner == hasProject)
        {
            return ServiceResult.Failure(
                "Media must belong to exactly one parent: either PartnerContributorId or CollaborationProjectId.");
        }

        if (hasPartner)
        {
            var exists = await _db.PartnerContributors.AsNoTracking()
                .AnyAsync(x => x.Id == partnerContributorId!.Value, ct);
            if (!exists)
                return ServiceResult.Failure("PartnerContributorId does not reference an existing organization.");
        }

        if (hasProject)
        {
            var exists = await _db.CollaborationProjects.AsNoTracking()
                .AnyAsync(x => x.Id == collaborationProjectId!.Value, ct);
            if (!exists)
                return ServiceResult.Failure("CollaborationProjectId does not reference an existing project.");
        }

        return ServiceResult.Success();
    }

    private async Task TryDeleteBlobIfUnreferencedAsync(string? blobName, int excludedMediaId)
    {
        if (!CollaborationValidation.IsManagedCollaborationBlob(blobName))
            return;

        var stillUsedByMedia = await _db.CollaborationMedias.AsNoTracking()
            .AnyAsync(x => x.BlobName == blobName && x.Id != excludedMediaId);

        if (stillUsedByMedia)
            return;

        var url = _blob.GetPublicUrl(blobName!);
        var usedAsLogo =
            await _db.PartnerContributors.AsNoTracking()
                .AnyAsync(x => x.LogoUrl == url || x.LogoUrl == blobName) ||
            await _db.CollaborationProjects.AsNoTracking()
                .AnyAsync(x => x.LogoUrl == url || x.LogoUrl == blobName);

        if (usedAsLogo)
            return;

        try
        {
            await _blob.DeleteDocumentAsync(blobName!);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not delete media blob {Blob}", blobName);
        }
    }
}
