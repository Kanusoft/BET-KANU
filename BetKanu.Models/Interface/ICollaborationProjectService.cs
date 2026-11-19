using BetKanu.Models.Common;
using BetKanu.Models.DTOs.Collaboration;
using Microsoft.AspNetCore.Http;

namespace BET_KANU.Services;

public interface ICollaborationProjectService
{
    Task<IReadOnlyList<CollaborationProjectListDto>> GetAllAsync(CancellationToken ct = default);

    Task<CollaborationProjectDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<CollaborationProjectListDto>> GetByPartnerContributorIdAsync(
        int partnerContributorId,
        CancellationToken ct = default);

    Task<IReadOnlyList<CollaborationProjectListDto>> GetActiveByPartnerContributorIdAsync(
        int partnerContributorId,
        CancellationToken ct = default);

    Task<PagedResult<CollaborationProjectListDto>> GetAdminPagedAsync(
        CollaborationProjectFilterDto filter,
        CancellationToken ct = default);

    Task<ServiceResult<CollaborationProjectDetailsDto>> CreateAsync(
        CollaborationProjectCreateDto dto,
        CancellationToken ct = default);

    Task<ServiceResult<CollaborationProjectDetailsDto>> UpdateAsync(
        int id,
        CollaborationProjectUpdateDto dto,
        CancellationToken ct = default);

    Task<ServiceResult> SetActiveAsync(int id, bool isActive, CancellationToken ct = default);

    Task<ServiceResult> UpdateDisplayOrderAsync(int id, int displayOrder, CancellationToken ct = default);

    Task<ServiceResult> DeleteAsync(int id, CancellationToken ct = default);

    Task<ServiceResult<string>> UploadLogoAsync(
        int id,
        IFormFile file,
        CancellationToken ct = default);
}
