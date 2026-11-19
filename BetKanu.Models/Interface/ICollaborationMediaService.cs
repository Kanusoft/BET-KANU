using BetKanu.Models.Common;
using BetKanu.Models.DTOs.Collaboration;
using Microsoft.AspNetCore.Http;

namespace BET_KANU.Services;

public interface ICollaborationMediaService
{
    Task<CollaborationMediaDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<CollaborationMediaDto>> GetByPartnerContributorIdAsync(
        int partnerContributorId,
        CancellationToken ct = default);

    Task<IReadOnlyList<CollaborationMediaDto>> GetByCollaborationProjectIdAsync(
        int collaborationProjectId,
        CancellationToken ct = default);

    Task<IReadOnlyList<CollaborationMediaDto>> GetActiveForPublicAsync(
        int? partnerContributorId = null,
        int? collaborationProjectId = null,
        CollaborationMediaType? mediaType = null,
        CancellationToken ct = default);

    Task<ServiceResult<CollaborationMediaDto>> UploadAsync(
        CollaborationMediaUploadDto dto,
        IFormFile file,
        CancellationToken ct = default);

    Task<ServiceResult<IReadOnlyList<CollaborationMediaDto>>> UploadManyAsync(
        CollaborationMediaUploadDto dto,
        IReadOnlyList<IFormFile> files,
        CancellationToken ct = default);

    Task<ServiceResult<CollaborationMediaDto>> UpdateAsync(
        int id,
        CollaborationMediaUpdateDto dto,
        CancellationToken ct = default);

    Task<ServiceResult> UpdateDisplayOrderAsync(int id, int displayOrder, CancellationToken ct = default);

    Task<ServiceResult> SetActiveAsync(int id, bool isActive, CancellationToken ct = default);

    Task<ServiceResult> DeleteAsync(int id, CancellationToken ct = default);

    Task<ServiceResult<string>> GetDownloadUrlAsync(int id, CancellationToken ct = default);
}
