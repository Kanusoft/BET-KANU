using BetKanu.Models;
using BetKanu.Models.Common;

namespace BET_KANU.Services;

public interface ISupporterService
{
    Task<List<Supporter>> GetPublishedAsync(
        int currentYear,
        CancellationToken ct = default);

    Task<List<Supporter>> SearchAdminAsync(
        string? query,
        SupporterType? type,
        int? year,
        bool? published,
        CancellationToken ct = default,
        int? period = null);

    Task<Supporter?> GetAsync(
        int id,
        CancellationToken ct = default);

    Task<int> CreateAsync(
        Supporter item,
        CancellationToken ct = default);

    Task<bool> UpdateAsync(
        int id,
        Supporter updated,
        CancellationToken ct = default);

    Task<bool> SetPublishedAsync(
        int id,
        bool published,
        CancellationToken ct = default);

    Task<bool> DeleteAsync(
        int id,
        CancellationToken ct = default);
}