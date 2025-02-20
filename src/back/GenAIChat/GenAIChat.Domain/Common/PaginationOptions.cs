namespace GenAIChat.Domain.Common
{
    public class PaginationOptions
    {
        public const int DefaultOffset = 0;
        public const int MaxLimit = 1000;
        public const int DefaultLimit = 10;

        public int Offset { get; set; } = DefaultOffset;
        public int Limit { get; set; } = DefaultLimit;

        public int? Count { get; set; } = null;

        public PaginationOptions() { }
        public PaginationOptions(int offset, int limit)
        {
            Offset = offset;
            Limit = limit > MaxLimit ? MaxLimit : limit;
        }
        public PaginationOptions(PaginationOptions options, int? count = null) : this(options.Offset, options.Limit)
        {
            Count = count;
        }

        public void SetCount(int count) => Count = count;
    }
}
