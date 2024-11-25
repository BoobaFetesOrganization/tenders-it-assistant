namespace GenAIChat.Domain.Common
{
    public class Paged<T> where T : class
    {
        public PaginationOptions Page { get; set; } = new PaginationOptions();
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

        public Paged() { }
        public Paged(PaginationOptions options, IEnumerable<T>? data)
        {
            Data = data ?? Enumerable.Empty<T>();
            Page = new PaginationOptions(options);
        }
        public Paged(PaginationOptions options, int count, IEnumerable<T>? data) : this(options, data)
        {
            Page.SetCount(count);
        }
    }
}
