namespace GenAIChat.Domain.Common
{
    public class PaginationOptions
    {
        public static PaginationOptions Default => new();
        public static PaginationOptions All => new(DefaultOffset, null);
        public static PaginationOptions AllAfter(int offset) => new(offset, null);

        public const int DefaultOffset = 0;
        public const int MaxLimit = 1000;
        public const int DefaultLimit = 10;

        public int Offset { get; set; } = DefaultOffset;
        public int? Limit { get; set; } = DefaultLimit;

        public PaginationOptions() { }
        public PaginationOptions(int offset, int? limit)
        {
            Offset = offset;

            Limit = !limit.HasValue
                ? null
                : limit.Value > MaxLimit ? MaxLimit : limit.Value;
        }
        public PaginationOptions(PaginationOptions options)
        {
            Offset = options.Offset;
            Limit = options.Limit;
        }
    }
}
