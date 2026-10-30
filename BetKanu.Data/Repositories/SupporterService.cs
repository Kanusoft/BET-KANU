
using BetKanu.Data;
using BetKanu.Models;
using BetKanu.Models.Common;
using Microsoft.EntityFrameworkCore;

namespace BET_KANU.Services;

public sealed class SupporterService : ISupporterService
{
    private readonly BKdbContext _context;

    public SupporterService(BKdbContext context) => _context = context;

    public Task<List<Supporter>> GetPublishedAsync(int currentYear, CancellationToken ct = default)
        => _context.Supporters.AsNoTracking()
            // All published supporters are public, including upcoming years (shown in their own year tab).
            // RecognitionYear 0 = legacy unknown year; still listed.
            .Where(x => x.IsPublished)
            .OrderBy(x => x.Period)
            .ThenBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);

    public Task<List<Supporter>> SearchAdminAsync(
        string? query,
        SupporterType? type,
        int? year,
        bool? published,
        CancellationToken ct = default,
        int? period = null)
    {
        var q = _context.Supporters
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            string search = query.Trim();
            q = q.Where(x => x.Name.Contains(search));
        }

        if (type.HasValue)
            q = q.Where(x => x.Type == type.Value);

        if (year.HasValue)
            q = q.Where(x => x.RecognitionYear == year.Value);

        if (published.HasValue)
            q = q.Where(x => x.IsPublished == published.Value);

        if (period.HasValue)
            q = q.Where(x => x.Period == period.Value);

        return q
            .OrderBy(x => x.Period)
            .ThenByDescending(x => x.RecognitionYear)
            .ThenBy(x => x.Type)
            .ThenBy(x => x.DisplayOrder)
            .ThenBy(x => x.Name)
            .ToListAsync(ct);
    }

    public Task<Supporter?> GetAsync(int id, CancellationToken ct = default)
        => _context.Supporters.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<int> CreateAsync(Supporter item, CancellationToken ct = default)
    {
        item.Name = item.Name.Trim();
        item.DisplayLabel = string.IsNullOrWhiteSpace(item.DisplayLabel) ? null : item.DisplayLabel.Trim();
        item.CreatedAtUtc = DateTime.UtcNow;
        item.UpdatedAtUtc = null;
        _context.Supporters.Add(item);
        await _context.SaveChangesAsync(ct);
        return item.Id;
    }

    public async Task<bool> UpdateAsync(
        int id,
        Supporter updated,
        CancellationToken ct = default)
    {
        var current = await _context.Supporters
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (current is null)
            return false;

        current.Name = updated.Name.Trim();
        current.Type = updated.Type;
        current.Period = updated.Period;
        current.RecognitionYear = updated.RecognitionYear;
        current.DisplayLabel = string.IsNullOrWhiteSpace(updated.DisplayLabel)
            ? null
            : updated.DisplayLabel.Trim();
        current.DisplayOrder = updated.DisplayOrder;
        current.IsPublished = updated.IsPublished;
        current.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> SetPublishedAsync(int id, bool published, CancellationToken ct = default)
    {
        var current = await _context.Supporters.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (current is null) return false;
        current.IsPublished = published;
        current.UpdatedAtUtc = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var current = await _context.Supporters.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (current is null) return false;
        _context.Supporters.Remove(current);
        await _context.SaveChangesAsync(ct);
        return true;
    }
}
