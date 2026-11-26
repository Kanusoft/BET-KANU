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

public sealed class PartnerContributorService : IPartnerContributorService
{
    private readonly BKdbContext _db;
    private readonly IBlobStorageService _blob;
    private readonly ILogger<PartnerContributorService> _logger;
    private readonly CollaborationUploadOptions _uploadOptions;

    public PartnerContributorService(
        BKdbContext db,
        IBlobStorageService blob,
        IOptions<CollaborationUploadOptions> uploadOptions,
        ILogger<PartnerContributorService> logger)
    {
        _db = db;
        _blob = blob;
        _logger = logger;
        _uploadOptions = uploadOptions.Value;
    }

    public async Task<IReadOnlyList<PartnerContributorListDto>> GetAllAsync(CancellationToken ct = default)
    {
        var rows = await BaseQuery()
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);

        return rows.Select(MapList).ToList();
    }

    public async Task<PartnerContributorDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.PartnerContributors
            .AsNoTracking()
            .Include(x => x.Projects)
                .ThenInclude(p => p.Media)
            .Include(x => x.Media)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return null;

        var (books, products) = await CollaborationContentLookup.LoadAsync(_db, entity.Projects, ct);
        return MapDetails(entity, books, products);
    }

    public async Task<PagedResult<PartnerContributorListDto>> GetAdminPagedAsync(
        PartnerContributorFilterDto filter,
        CancellationToken ct = default)
    {
        filter ??= new PartnerContributorFilterDto();
        var (page, pageSize) = CollaborationValidation.NormalizePaging(filter.Page, filter.PageSize);

        var query = BaseQuery();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var term = filter.Search.Trim();
            query = query.Where(x => x.Name.Contains(term));
        }

        if (filter.Category.HasValue)
        {
            var category = filter.Category.Value;
            query = category switch
            {
                CollaborationCategory.Partner =>
                    query.Where(x => x.Category == CollaborationCategory.Partner
                                     || x.Category == CollaborationCategory.Both),
                CollaborationCategory.Contributor =>
                    query.Where(x => x.Category == CollaborationCategory.Contributor
                                     || x.Category == CollaborationCategory.Both),
                _ => query.Where(x => x.Category == category)
            };
        }

        if (filter.IsActive.HasValue)
            query = query.Where(x => x.IsActive == filter.IsActive.Value);

        if (filter.Year.HasValue)
            query = query.Where(x => x.Year == filter.Year.Value);

        var total = await query.CountAsync(ct);
        var items = await query
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<PartnerContributorListDto>
        {
            Items = items.Select(MapList).ToList(),
            TotalCount = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public Task<IReadOnlyList<PartnerContributorListDto>> GetActivePartnersAsync(CancellationToken ct = default)
        => GetActiveByCategoryAsync(CollaborationCategory.Partner, year: null, ct);

    public Task<IReadOnlyList<PartnerContributorListDto>> GetActiveContributorsAsync(CancellationToken ct = default)
        => GetActiveByCategoryAsync(CollaborationCategory.Contributor, year: null, ct);

    public Task<IReadOnlyList<PartnerContributorListDto>> GetActiveContributorsByYearAsync(
        int year,
        CancellationToken ct = default)
        => GetActiveByCategoryAsync(CollaborationCategory.Contributor, year, ct);

    public async Task<IReadOnlyList<PartnerContributorListDto>> SearchByNameAsync(
        string name,
        CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Array.Empty<PartnerContributorListDto>();

        var term = name.Trim();
        var rows = await BaseQuery()
            .Where(x => x.Name.Contains(term))
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);

        return rows.Select(MapList).ToList();
    }

    public async Task<ServiceResult<PartnerContributorDetailsDto>> CreateAsync(
        PartnerContributorCreateDto dto,
        CancellationToken ct = default)
    {
        var validation = ValidateCreateOrUpdate(dto.Name, dto.Category, dto.WebsiteUrl, dto.Year, dto.DisplayOrder,
            dto.ShortDescription, dto.PartnerType);
        if (!validation.Succeeded)
            return ServiceResult<PartnerContributorDetailsDto>.From(validation);

        var entity = new PartnerContributor
        {
            Name = dto.Name.Trim(),
            ShortDescription = CollaborationValidation.NormalizeOptional(dto.ShortDescription),
            LongDescription = CollaborationValidation.NormalizeOptional(dto.LongDescription),
            WebsiteUrl = CollaborationValidation.NormalizeOptional(dto.WebsiteUrl),
            Category = dto.Category,
            PartnerType = CollaborationValidation.NormalizeOptional(dto.PartnerType),
            Year = dto.Year,
            IsActive = dto.IsActive,
            DisplayOrder = dto.DisplayOrder,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        _db.PartnerContributors.Add(entity);
        await _db.SaveChangesAsync(ct);

        return ServiceResult<PartnerContributorDetailsDto>.Success(MapDetails(entity));
    }

    public async Task<ServiceResult<PartnerContributorDetailsDto>> UpdateAsync(
        int id,
        PartnerContributorUpdateDto dto,
        CancellationToken ct = default)
    {
        var validation = ValidateCreateOrUpdate(dto.Name, dto.Category, dto.WebsiteUrl, dto.Year, dto.DisplayOrder,
            dto.ShortDescription, dto.PartnerType);
        if (!validation.Succeeded)
            return ServiceResult<PartnerContributorDetailsDto>.From(validation);

        var entity = await _db.PartnerContributors
            .Include(x => x.Projects)
            .Include(x => x.Media)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return ServiceResult<PartnerContributorDetailsDto>.Failure("Partner/Contributor was not found.");

        entity.Name = dto.Name.Trim();
        entity.ShortDescription = CollaborationValidation.NormalizeOptional(dto.ShortDescription);
        entity.LongDescription = CollaborationValidation.NormalizeOptional(dto.LongDescription);
        entity.WebsiteUrl = CollaborationValidation.NormalizeOptional(dto.WebsiteUrl);
        entity.Category = dto.Category;
        entity.PartnerType = CollaborationValidation.NormalizeOptional(dto.PartnerType);
        entity.Year = dto.Year;
        entity.IsActive = dto.IsActive;
        entity.DisplayOrder = dto.DisplayOrder;
        entity.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        var (books, products) = await CollaborationContentLookup.LoadAsync(_db, entity.Projects, ct);
        return ServiceResult<PartnerContributorDetailsDto>.Success(MapDetails(entity, books, products));
    }

    public async Task<ServiceResult> SetActiveAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await _db.PartnerContributors.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult.Failure("Partner/Contributor was not found.");

        entity.IsActive = isActive;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> UpdateDisplayOrderAsync(int id, int displayOrder, CancellationToken ct = default)
    {
        if (displayOrder < 0)
            return ServiceResult.Failure("Display order must be zero or greater.");

        var entity = await _db.PartnerContributors.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult.Failure("Partner/Contributor was not found.");

        entity.DisplayOrder = displayOrder;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await _db.PartnerContributors
            .Include(x => x.Projects)
            .Include(x => x.Media)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (entity is null)
            return ServiceResult.Failure("Partner/Contributor was not found.");

        if (entity.Projects.Count > 0 || entity.Media.Count > 0)
        {
            return ServiceResult.Failure(
                "Cannot delete this organization while projects or media still exist. Deactivate it instead, or remove dependents first.");
        }

        var logoBlob = CollaborationValidation.TryExtractBlobNameFromUrl(entity.LogoUrl);
        _db.PartnerContributors.Remove(entity);
        await _db.SaveChangesAsync(ct);

        await TryDeleteOwnedBlobAsync(logoBlob, excludeMediaId: null, excludeLogoUrls: Array.Empty<string>());
        return ServiceResult.Success();
    }

    public async Task<ServiceResult<string>> UploadLogoAsync(
        int id,
        IFormFile file,
        CancellationToken ct = default)
    {
        var entity = await _db.PartnerContributors.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (entity is null)
            return ServiceResult<string>.Failure("Partner/Contributor was not found.");

        var validation = CollaborationFileValidator.Validate(file, _uploadOptions, imagesOnly: true);
        if (!validation.IsValid)
            return ServiceResult<string>.Failure(validation.Error!);

        var blobName = CollaborationValidation.BuildBlobPath("logos", validation.SanitizedFileName);
        string? uploadedBlob = null;
        try
        {
            await using var stream = file.OpenReadStream();
            var upload = await _blob.UploadAsync(stream, blobName, file.ContentType, ct);
            uploadedBlob = upload.BlobName;

            var previousLogo = entity.LogoUrl;
            entity.LogoUrl = upload.Url;
            entity.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync(ct);

            var previousBlob = CollaborationValidation.TryExtractBlobNameFromUrl(previousLogo);
            await TryDeleteOwnedBlobAsync(previousBlob, excludeMediaId: null, excludeLogoUrls: new[] { entity.LogoUrl });

            return ServiceResult<string>.Success(upload.Url);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload partner logo for id {Id}", id);
            if (!string.IsNullOrWhiteSpace(uploadedBlob))
            {
                try { await _blob.DeleteDocumentAsync(uploadedBlob); }
                catch (Exception cleanupEx)
                {
                    _logger.LogError(cleanupEx, "Failed cleaning orphaned logo blob {Blob}", uploadedBlob);
                }
            }

            return ServiceResult<string>.Failure("Logo upload failed.");
        }
    }

    private async Task<IReadOnlyList<PartnerContributorListDto>> GetActiveByCategoryAsync(
        CollaborationCategory category,
        int? year,
        CancellationToken ct)
    {
        var query = BaseQuery().Where(x => x.IsActive);

        query = category switch
        {
            CollaborationCategory.Partner =>
                query.Where(x => x.Category == CollaborationCategory.Partner
                                 || x.Category == CollaborationCategory.Both),
            CollaborationCategory.Contributor =>
                query.Where(x => x.Category == CollaborationCategory.Contributor
                                 || x.Category == CollaborationCategory.Both),
            _ => query.Where(x => x.Category == category)
        };

        if (year.HasValue)
            query = query.Where(x => x.Year == year.Value);

        var rows = await query
            .OrderBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);

        return rows.Select(MapList).ToList();
    }

    private IQueryable<PartnerContributor> BaseQuery() =>
        _db.PartnerContributors
            .AsNoTracking()
            .Include(x => x.Projects)
            .Include(x => x.Media);

    private static ServiceResult ValidateCreateOrUpdate(
        string name,
        CollaborationCategory category,
        string? websiteUrl,
        int? year,
        int displayOrder,
        string? shortDescription,
        string? partnerType)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
            errors.Add("Name is required.");
        else if (name.Trim().Length > 200)
            errors.Add("Name cannot exceed 200 characters.");

        if (!Enum.IsDefined(typeof(CollaborationCategory), category))
            errors.Add("Category is invalid.");

        if (!CollaborationValidation.IsValidHttpUrl(websiteUrl))
            errors.Add("Website URL must be a valid HTTP or HTTPS address.");

        if (!string.IsNullOrWhiteSpace(shortDescription) && shortDescription.Length > 1000)
            errors.Add("Short description cannot exceed 1000 characters.");

        if (!string.IsNullOrWhiteSpace(partnerType) && partnerType.Length > 150)
            errors.Add("Partner type cannot exceed 150 characters.");

        if (year.HasValue && (year.Value < 1900 || year.Value > 2100))
            errors.Add("Year must be between 1900 and 2100.");

        if (displayOrder < 0)
            errors.Add("Display order must be zero or greater.");

        return errors.Count == 0 ? ServiceResult.Success() : ServiceResult.Failure(errors);
    }

    private async Task TryDeleteOwnedBlobAsync(
        string? blobName,
        int? excludeMediaId,
        IEnumerable<string?> excludeLogoUrls)
    {
        if (!CollaborationValidation.IsManagedCollaborationBlob(blobName))
            return;

        var stillReferencedByMedia = await _db.CollaborationMedias
            .AsNoTracking()
            .AnyAsync(x =>
                x.BlobName == blobName &&
                (!excludeMediaId.HasValue || x.Id != excludeMediaId.Value));

        if (stillReferencedByMedia)
            return;

        var publicUrl = _blob.GetPublicUrl(blobName!);
        var excluded = new HashSet<string>(
            excludeLogoUrls.Where(x => !string.IsNullOrWhiteSpace(x))!,
            StringComparer.OrdinalIgnoreCase);

        var stillReferencedByLogo =
            await _db.PartnerContributors.AsNoTracking()
                .AnyAsync(x => x.LogoUrl == publicUrl || x.LogoUrl == blobName) ||
            await _db.CollaborationProjects.AsNoTracking()
                .AnyAsync(x => x.LogoUrl == publicUrl || x.LogoUrl == blobName);

        if (stillReferencedByLogo && !excluded.Contains(publicUrl) && !excluded.Contains(blobName!))
            return;

        if (stillReferencedByLogo)
            return;

        try
        {
            await _blob.DeleteDocumentAsync(blobName!);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not delete collaboration blob {Blob}", blobName);
        }
    }

    private static PartnerContributorListDto MapList(PartnerContributor x) => new()
    {
        Id = x.Id,
        Name = x.Name,
        LogoUrl = x.LogoUrl,
        ShortDescription = x.ShortDescription,
        Category = x.Category,
        PartnerType = x.PartnerType,
        Year = x.Year,
        IsActive = x.IsActive,
        DisplayOrder = x.DisplayOrder,
        ProjectCount = x.Projects?.Count ?? 0,
        MediaCount = x.Media?.Count ?? 0
    };

    private static PartnerContributorDetailsDto MapDetails(
        PartnerContributor x,
        IReadOnlyDictionary<int, Book>? books = null,
        IReadOnlyDictionary<int, Product>? products = null) => new()
    {
        Id = x.Id,
        Name = x.Name,
        LogoUrl = x.LogoUrl,
        ShortDescription = x.ShortDescription,
        LongDescription = x.LongDescription,
        WebsiteUrl = x.WebsiteUrl,
        Category = x.Category,
        PartnerType = x.PartnerType,
        Year = x.Year,
        IsActive = x.IsActive,
        DisplayOrder = x.DisplayOrder,
        CreatedAt = x.CreatedAt,
        UpdatedAt = x.UpdatedAt,
        Projects = (x.Projects ?? Enumerable.Empty<CollaborationProject>())
            .OrderBy(p => p.DisplayOrder)
            .ThenBy(p => p.Title)
            .Select(p =>
            {
                var content = CollaborationContentLookup.Resolve(
                    p.ContentType,
                    p.ContentId,
                    books,
                    products);

                return new CollaborationProjectListDto
                {
                    Id = p.Id,
                    PartnerContributorId = p.PartnerContributorId,
                    PartnerContributorName = x.Name,
                    Title = p.Title,
                    LogoUrl = p.LogoUrl,
                    ShortDescription = p.ShortDescription,
                    LongDescription = p.LongDescription,
                    ProjectLink = p.ProjectLink,
                    ProjectType = p.ProjectType,
                    ContentType = p.ContentType,
                    ContentId = p.ContentId,
                    ContentTitle = content?.Title,
                    ContentGroup = content?.Group,
                    ContentSubtitle = content?.Subtitle,
                    ContentImageUrl = content?.ImageUrl,
                    ContentUrl = content?.Url,
                    Year = p.Year,
                    IsActive = p.IsActive,
                    DisplayOrder = p.DisplayOrder,
                    MediaCount = p.Media?.Count ?? 0,
                    Media = (p.Media ?? Enumerable.Empty<CollaborationMedia>())
                        .OrderBy(m => m.DisplayOrder)
                        .ThenBy(m => m.Id)
                        .Select(CollaborationMediaService.MapDto)
                        .ToList()
                };
            }).ToList(),
        Media = (x.Media ?? Enumerable.Empty<CollaborationMedia>())
            .OrderBy(m => m.DisplayOrder)
            .ThenBy(m => m.Id)
            .Select(CollaborationMediaService.MapDto)
            .ToList()
    };
}
