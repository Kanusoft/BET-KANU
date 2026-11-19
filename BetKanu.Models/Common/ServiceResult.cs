namespace BetKanu.Models.Common
{
    public class ServiceResult
    {
        public bool Succeeded { get; init; }
        public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();

        public string? Error => Errors.Count > 0 ? Errors[0] : null;

        public static ServiceResult Success() => new() { Succeeded = true };

        public static ServiceResult Failure(params string[] errors) => new()
        {
            Succeeded = false,
            Errors = errors?.Where(e => !string.IsNullOrWhiteSpace(e)).ToArray()
                ?? new[] { "An unexpected error occurred." }
        };

        public static ServiceResult Failure(IEnumerable<string> errors) =>
            Failure(errors.ToArray());
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; init; }

        public static ServiceResult<T> Success(T data) => new()
        {
            Succeeded = true,
            Data = data
        };

        public new static ServiceResult<T> Failure(params string[] errors) => new()
        {
            Succeeded = false,
            Errors = errors?.Where(e => !string.IsNullOrWhiteSpace(e)).ToArray()
                ?? new[] { "An unexpected error occurred." }
        };

        public new static ServiceResult<T> Failure(IEnumerable<string> errors) =>
            Failure(errors.ToArray());

        public static ServiceResult<T> From(ServiceResult result) => new()
        {
            Succeeded = result.Succeeded,
            Errors = result.Errors
        };
    }

    public sealed class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
