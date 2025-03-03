namespace GenAIChat.Domain.Common
{
    public class Paged<T> where T : class
    {
        public PaginationOptions Page { get; set; } = new PaginationOptions();
        public IEnumerable<T> Data { get; set; } = [];

        public Paged() { }
        public Paged(PaginationOptions options, IEnumerable<T>? data)
        {
            Data = data ?? [];
            Page = new PaginationOptions(options);
        }
    }
}
