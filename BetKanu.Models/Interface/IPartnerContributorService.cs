using BetKanu.Models.Common;
using BetKanu.Models.DTOs.Collaboration;
using Microsoft.AspNetCore.Http;

namespace BET_KANU.Services;

public interface IPartnerContributorService
{
    Task<IReadOnlyList<PartnerContributorListDto>> GetAllAsync(CancellationToken ct = default);

    Task<PartnerContributorDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<PagedResult<PartnerContributorListDto>> GetAdminPagedAsync(
        PartnerContributorFilterDto filter,
        CancellationToken ct = default);

    Task<IReadOnlyList<PartnerContributorListDto>> GetActivePartnersAsync(CancellationToken ct = default);

    Task<IReadOnlyList<PartnerContributorListDto>> GetActiveContributorsAsync(CancellationToken ct = default);

    Task<IReadOnlyList<PartnerContributorListDto>> GetActiveContributorsByYearAsync(
        int year,
        CancellationToken ct = default);

    Task<IReadOnlyList<PartnerContributorListDto>> SearchByNameAsync(
        string name,
        CancellationToken ct = default);

    Task<ServiceResult<PartnerContributorDetailsDto>> CreateAsync(
        PartnerContributorCreateDto dto,
        CancellationToken ct = default);

    Task<ServiceResult<PartnerContributorDetailsDto>> UpdateAsync(
        int id,
        PartnerContributorUpdateDto dto,
        CancellationToken ct = default);

    Task<ServiceResult> SetActiveAsync(int id, bool isActive, CancellationToken ct = default);

    Task<ServiceResult> UpdateDisplayOrderAsync(int id, int displayOrder, CancellationToken ct = default);

    Task<ServiceResult> DeleteAsync(int id, CancellationToken ct = default);

    Task<ServiceResult<string>> UploadLogoAsync(
        int id,
        IFormFile file,
        CancellationToken ct = default);
}
