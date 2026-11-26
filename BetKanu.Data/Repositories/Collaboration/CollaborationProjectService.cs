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

public sealed class CollaborationProjectService : ICollaborationProjectService
{
    private readonly BKdbContext _db;
    private readonly IBlobStorageService _blob;
    private readonly ILogger<CollaborationProjectService> _logger;
    private readonly CollaborationUploadOptions _uploadOptions;

    public CollaborationProjectService(
        BKdbContext db,
        IBlobStorageService blob,
        IOptions<CollaborationUploadOptions> uploadOptions,
        ILogger<CollaborationProjectService> logger)
    {
        _db = db;
        _blob = blob;
        _logger = logger;
        _uploadOptions = uploadOptions.Value;
    }

    public async Task<IReadOnlyList<CollaborationProjectListDto>> GetAllAsync(CancellationToken ct = default)
    {
        var rows = await BaseQuery()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Title)
            .ToListAsync(ct);

        var (books, products) = await CollaborationContentLookup.LoadAsync(_db, rows, ct);
        return rows.Select(x => MapList(x, books, products)).ToList();
    }

    public async Task<CollaborationProjectDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.CollaborationProjects
            .AsNoTracking()
            .Include(x => x.PartnerContributor)
            .Include(x => x.Media)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return null;

        var (books, products) = await CollaborationContentLookup.LoadAsync(_db, new[] { entity }, ct);
        return MapDetails(entity, books, products);
    }

    public async Task<IReadOnlyList<CollaborationProjectListDto>> GetByPartnerContributorIdAsync(
        int partnerContributorId,
        CancellationToken ct = default)
    {
        var rows = await BaseQuery()
            .Where(x => x.PartnerContributorId == partnerContributorId)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Title)
            .ToListAsync(ct);

        var (books, products) = await CollaborationContentLookup.LoadAsync(_db, rows, ct);
        return rows.Select(x => MapList(x, books, products)).ToList();
    }

    public async Task<IReadOnlyList<CollaborationProjectListDto>> GetActiveByPartnerContributorIdAsync(
        int partnerContributorId,
        CancellationToken ct = default)
    {
        var rows = await BaseQuery()
            .Where(x => x.PartnerContributorId == partnerContributorId && x.IsActive)
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Title)
            .ToListAsync(ct);

        var (books, products) = await CollaborationContentLookup.LoadAsync(_db, rows, ct);
        return rows.Select(x => MapList(x, books, products)).ToList();
    }

    public async Task<PagedResult<CollaborationProjectListDto>> GetAdminPagedAsync(
        CollaborationProjectFilterDto filter,
        CancellationToken ct = default)
    {
        filter ??= new CollaborationProjectFilterDto();
        var (page, pageSize) = CollaborationValidation.NormalizePaging(filter.Page, filter.PageSize);
        var query = BaseQuery();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(x => x.Title.Contains(term));
        }

        if (filter.PartnerContributorId.HasValue)
            query = query.Where(x => x.PartnerContributorId == filter.PartnerContributorId.Value);

        if (filter.Year.HasValue)
            query = query.Where(x => x.Year == filter.Year.Value);

        if (!string.IsNullOrWhiteSpace(filter.ProjectType))
        {
            var type = filter.ProjectType.Trim();
            query = query.Where(x => x.ProjectType == type);
        }

        if (filter.IsActive.HasValue)
            query = query.Where(x => x.IsActive == filter.IsActive.Value);

        if (filter.ContentType.HasValue && filter.ContentId.HasValue)
        {
            query = query.Where(x =>
                x.ContentType == filter.ContentType.Value &&
                x.ContentId == filter.ContentId.Value);
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        var (books, products) = await CollaborationContentLookup.LoadAsync(_db, items, ct);
        return new PagedResult<CollaborationProjectListDto>
        {
            Items = items.Select(x => MapList(x, books, products)).ToList(),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ServiceResult<CollaborationProjectDetailsDto>> CreateAsync(
        CollaborationProjectCreateDto dto,
        CancellationToken ct = default)
    {
        var validation = await ValidateAsync(
            dto.PartnerContributorId,
            dto.Title,
            dto.ProjectLink,
            dto.ProjectType,
            dto.ShortDescription,
            dto.Year,
            dto.DisplayOrder,
            dto.ContentType,
            dto.ContentId,
            dto.ContentPageId,
            ct);

        if (!validation.Succeeded)
            return ServiceResult<CollaborationProjectDetailsDto>.From(validation);

        var entity = new CollaborationProject
        {
            PartnerContributorId = dto.PartnerContributorId,
            Title = dto.Title.Trim(),
            ShortDescription = CollaborationValidation.NormalizeOptional(dto.ShortDescription),
            LongDescription = CollaborationValidation.NormalizeOptional(dto.LongDescription),
            ProjectLink = CollaborationValidation.NormalizeOptional(dto.ProjectLink),
            ProjectType = CollaborationValidation.NormalizeOptional(dto.ProjectType),
            ContentType = dto.ContentType,
            ContentId = dto.ContentId,
            Year = dto.Year,
            ContentPageId = dto.ContentPageId,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        _db.CollaborationProjects.Add(entity);
        await _db.SaveChangesAsync(ct);

        var created = await GetByIdAsync(entity.Id, ct);
        return ServiceResult<CollaborationProjectDetailsDto>.Success(created!);
    }

    public async Task<ServiceResult<CollaborationProjectDetailsDto>> UpdateAsync(
        int id,
        CollaborationProjectUpdateDto dto,
        CancellationToken ct = default)
    {
        var validation = await ValidateAsync(
            dto.PartnerContributorId,
            dto.Title,
            dto.ProjectLink,
            dto.ProjectType,
            dto.ShortDescription,
            dto.Year,
            dto.DisplayOrder,
            dto.ContentType,
            dto.ContentId,
            dto.ContentPageId,
            ct);

        if (!validation.Succeeded)
            return ServiceResult<CollaborationProjectDetailsDto>.From(validation);

        var entity = await _db.CollaborationProjects
            .Include(x => x.PartnerContributor)
            .Include(x => x.Media)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return ServiceResult<CollaborationProjectDetailsDto>.Failure("Project was not found.");

        entity.PartnerContributorId = dto.PartnerContributorId;
        entity.Title = dto.Title.Trim();
        entity.ShortDescription = CollaborationValidation.NormalizeOptional(dto.ShortDescription);
        entity.LongDescription = CollaborationValidation.NormalizeOptional(dto.LongDescription);
        entity.ProjectLink = CollaborationValidation.NormalizeOptional(dto.ProjectLink);
        entity.ProjectType = CollaborationValidation.NormalizeOptional(dto.ProjectType);
        entity.ContentType = dto.ContentType;
        entity.ContentId = dto.ContentId;
        entity.Year = dto.Year;
        entity.ContentPageId = dto.ContentPageId;
        entity.IsActive = dto.IsActive;
        entity.DisplayOrder = dto.DisplayOrder;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        var (books, products) = await CollaborationContentLookup.LoadAsync(_db, new[] { entity }, ct);
        return ServiceResult<CollaborationProjectDetailsDto>.Success(MapDetails(entity, books, products));
    }

    public async Task<ServiceResult> SetActiveAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await _db.CollaborationProjects.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult.Failure("Project was not found.");

        entity.IsActive = isActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> UpdateDisplayOrderAsync(int id, int displayOrder, CancellationToken ct = default)
    {
        if (displayOrder < 0)
            return ServiceResult.Failure("Display order must be zero or greater.");

        var entity = await _db.CollaborationProjects.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult.Failure("Project was not found.");

        entity.DisplayOrder = displayOrder;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.CollaborationProjects
            .Include(x => x.Media)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return ServiceResult.Failure("Project was not found.");

        if (entity.Media.Count > 0)
        {
            return ServiceResult.Failure(
                "Cannot delete this project while media records still exist. Deactivate it instead, or remove media first.");
        }

        var logoBlob = CollaborationValidation.TryExtractBlobNameFromUrl(entity.LogoUrl);
        _db.CollaborationProjects.Remove(entity);
        await _db.SaveChangesAsync(ct);
        await TryDeleteOwnedBlobAsync(logoBlob);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult<string>> UploadLogoAsync(
        int id,
        IFormFile file,
        CancellationToken ct = default)
    {
        var entity = await _db.CollaborationProjects.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult<string>.Failure("Project was not found.");

        var validation = CollaborationFileValidator.Validate(file, _uploadOptions, imagesOnly: true);
        if (!validation.IsValid)
            return ServiceResult<string>.Failure(validation.Error!);

        var blobName = CollaborationValidation.BuildBlobPath("project-logos", validation.SanitizedFileName);
        string? uploadedBlob = null;
        try
        {
            await using var stream = file.OpenReadStream();
            var upload = await _blob.UploadAsync(stream, blobName, file.ContentType, ct);
            uploadedBlob = upload.BlobName;

            var previous = entity.LogoUrl;
            entity.LogoUrl = upload.Url;
            entity.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);

            await TryDeleteOwnedBlobAsync(CollaborationValidation.TryExtractBlobNameFromUrl(previous));
            return ServiceResult<string>.Success(upload.Url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload project logo for id {Id}", id);
            if (!string.IsNullOrWhiteSpace(uploadedBlob))
            {
                try { await _blob.DeleteDocumentAsync(uploadedBlob); }
                catch (Exception cleanupEx)
                {
                    _logger.LogError(cleanupEx, "Failed cleaning orphaned project logo blob {Blob}", uploadedBlob);
                }
            }

            return ServiceResult<string>.Failure("Logo upload failed.");
        }
    }

    private async Task<ServiceResult> ValidateAsync(
        int partnerContributorId,
        string title,
        string? projectLink,
        string? projectType,
        string? shortDescription,
        int? year,
        int displayOrder,
        CollaborationContentType? contentType,
        int? contentId,
        int? contentPageId,
        CancellationToken ct)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(title))
            errors.Add("Title is required.");
        else if (title.Trim().Length > 250)
            errors.Add("Title cannot exceed 250 characters.");

        if (partnerContributorId <= 0 ||
            !await _db.PartnerContributors.AsNoTracking().AnyAsync(x => x.Id == partnerContributorId, ct))
        {
            errors.Add("PartnerContributorId must reference an existing organization.");
        }

        if (!CollaborationValidation.IsValidHttpUrl(projectLink))
            errors.Add("Project link must be a valid HTTP or HTTPS address.");

        if (!string.IsNullOrWhiteSpace(projectType) && projectType.Length > 150)
            errors.Add("Project type cannot exceed 150 characters.");

        if (!string.IsNullOrWhiteSpace(shortDescription) && shortDescription.Length > 1000)
            errors.Add("Short description cannot exceed 1000 characters.");

        if (year.HasValue && (year.Value < 1900 || year.Value > 2100))
            errors.Add("Year must be between 1900 and 2100.");

        if (displayOrder < 0)
            errors.Add("Display order must be zero or greater.");

        var contentHasType = contentType.HasValue;
        var contentHasId = contentId.HasValue;

        if (contentHasType != contentHasId)
        {
            errors.Add("The selected content is invalid.");
        }
        else if (contentHasType && contentHasId)
        {
            if (contentId!.Value <= 0)
            {
                errors.Add("The selected content is invalid.");
            }
            else if (!Enum.IsDefined(typeof(CollaborationContentType), contentType!.Value))
            {
                errors.Add("The selected content type is not supported.");
            }
            else if (contentType == CollaborationContentType.Product)
            {
                if (!await _db.Products.AsNoTracking().AnyAsync(x => x.Id == contentId.Value, ct))
                    errors.Add("The selected product was not found.");
            }
            else if (contentType == CollaborationContentType.ReaderBook)
            {
                if (!await _db.Books.AsNoTracking().AnyAsync(x => x.Id == contentId.Value, ct))
                    errors.Add("The selected BK Reader book was not found.");
            }
            else
            {
                errors.Add("The selected content type is not supported.");
            }
        }

        if (contentPageId.HasValue && contentPageId.Value <= 0)
            errors.Add("ContentPageId must be a positive value when provided.");

        return errors.Count == 0 ? ServiceResult.Success() : ServiceResult.Failure(errors);
    }

    private IQueryable<CollaborationProject> BaseQuery() =>
        _db.CollaborationProjects
            .AsNoTracking()
            .Include(x => x.PartnerContributor)
            .Include(x => x.Media);

    private async Task TryDeleteOwnedBlobAsync(string? blobName)
    {
        if (!CollaborationValidation.IsManagedCollaborationBlob(blobName))
            return;

        var referenced = await _db.CollaborationMedias.AsNoTracking()
            .AnyAsync(x => x.BlobName == blobName);

        if (referenced)
            return;

        var url = _blob.GetPublicUrl(blobName!);
        var logoReferenced =
            await _db.PartnerContributors.AsNoTracking().AnyAsync(x => x.LogoUrl == url || x.LogoUrl == blobName) ||
            await _db.CollaborationProjects.AsNoTracking().AnyAsync(x => x.LogoUrl == url || x.LogoUrl == blobName);

        if (logoReferenced)
            return;

        try
        {
            await _blob.DeleteDocumentAsync(blobName!);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not delete project logo blob {Blob}", blobName);
        }
    }

    private static CollaborationProjectListDto MapList(
        CollaborationProject x,
        IReadOnlyDictionary<int, Book>? books = null,
        IReadOnlyDictionary<int, Product>? products = null)
    {
        var content = CollaborationContentLookup.Resolve(x.ContentType, x.ContentId, books, products);

        return new()
        {
            Id = x.Id,
            PartnerContributorId = x.PartnerContributorId,
            PartnerContributorName = x.PartnerContributor?.Name ?? string.Empty,
            Title = x.Title,
            LogoUrl = x.LogoUrl,
            ShortDescription = x.ShortDescription,
            LongDescription = x.LongDescription,
            ProjectLink = x.ProjectLink,
            ProjectType = x.ProjectType,
            ContentType = x.ContentType,
            ContentId = x.ContentId,
            ContentTitle = content?.Title,
            ContentGroup = content?.Group,
            ContentSubtitle = content?.Subtitle,
            ContentImageUrl = content?.ImageUrl,
            ContentUrl = content?.Url,
            Year = x.Year,
            IsActive = x.IsActive,
            DisplayOrder = x.DisplayOrder,
            MediaCount = x.Media?.Count ?? 0,
            Media = (x.Media ?? Enumerable.Empty<CollaborationMedia>())
                .OrderBy(m => m.DisplayOrder)
                .ThenBy(m => m.Id)
                .Select(CollaborationMediaService.MapDto)
                .ToList()
        };
    }

    private static CollaborationProjectDetailsDto MapDetails(
        CollaborationProject x,
        IReadOnlyDictionary<int, Book>? books = null,
        IReadOnlyDictionary<int, Product>? products = null)
    {
        var content = CollaborationContentLookup.Resolve(x.ContentType, x.ContentId, books, products);

        return new()
        {
            Id = x.Id,
            PartnerContributorId = x.PartnerContributorId,
            PartnerContributorName = x.PartnerContributor?.Name ?? string.Empty,
            Title = x.Title,
            LogoUrl = x.LogoUrl,
            ShortDescription = x.ShortDescription,
            LongDescription = x.LongDescription,
            ProjectLink = x.ProjectLink,
            ProjectType = x.ProjectType,
            ContentType = x.ContentType,
            ContentId = x.ContentId,
            ContentTitle = content?.Title,
            ContentGroup = content?.Group,
            Year = x.Year,
            ContentPageId = x.ContentPageId,
            IsActive = x.IsActive,
            DisplayOrder = x.DisplayOrder,
            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            Media = (x.Media ?? Enumerable.Empty<CollaborationMedia>())
                .OrderBy(m => m.DisplayOrder)
                .ThenBy(m => m.Id)
                .Select(CollaborationMediaService.MapDto)
                .ToList()
        };
    }
}
