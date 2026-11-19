using System.Text.RegularExpressions;

namespace BetKanu.Data.Repositories.Collaboration;

internal static class CollaborationValidation
{
    private static readonly Regex HttpUrlRegex = new(
        @"^https?:\/\/[^\s/$.?#].[^\s]*$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public static bool IsValidHttpUrl(string? url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;

        if (url.Length > 1000)
            return false;

        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return false;

        return (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
               && HttpUrlRegex.IsMatch(url);
    }

    public static string? NormalizeOptional(string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public static (int page, int pageSize) NormalizePaging(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 200) pageSize = 200;
        return (page, pageSize);
    }

    public static string BuildBlobPath(string folder, string sanitizedFileName)
        => $"collaborations/{folder}/{Guid.NewGuid():N}-{sanitizedFileName}";

    public static string? TryExtractBlobNameFromUrl(string? urlOrBlobName)
    {
        if (string.IsNullOrWhiteSpace(urlOrBlobName))
            return null;

        var value = urlOrBlobName.Trim();
        if (!value.Contains("://", StringComparison.Ordinal))
            return value.TrimStart('/');

        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            return null;

        // Expected: https://{account}.blob.core.windows.net/{container}/{blobPath}
        var segments = uri.AbsolutePath.Trim('/').Split('/', 2, StringSplitOptions.RemoveEmptyEntries);
        return segments.Length >= 2 ? segments[1] : null;
    }

    public static bool IsManagedCollaborationBlob(string? blobName)
        => !string.IsNullOrWhiteSpace(blobName)
           && blobName.StartsWith("collaborations/", StringComparison.OrdinalIgnoreCase);
}
