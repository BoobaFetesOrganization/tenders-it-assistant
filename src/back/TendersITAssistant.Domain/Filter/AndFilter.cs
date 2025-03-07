namespace TendersITAssistant.Domain.Filter
{
    public class AndFilter : IFilter
    {
        public readonly IFilter Left;

        public readonly IFilter Right;

        public AndFilter(IFilter left, IFilter right)
        {
            Left = left;
            Right = right;
        }
    }
}
