namespace GenAIChat.Domain.Common
{
    public class PaginationOptions
    {
        public const int DefaultOffset = 0;
        public const int MaxLimit = 1000;
        public const int DefaultLimit = 10;

        public int Offset { get; set; } = DefaultOffset;
        public int Limit { get; set; } = DefaultLimit;

        public int Count { get; set; } = 0;

        public PaginationOptions() { }
        public PaginationOptions(int? offset, int? limit)
        {
            if (offset.HasValue)
                Offset = offset.Value;
            if (limit.HasValue)
                Limit = limit.Value > MaxLimit ? MaxLimit : limit.Value;
        }
        public PaginationOptions(PaginationOptions options, int count)
        {
            Offset = options.Offset;
            Limit = options.Limit;
            Count = count;
        }
    }
}
