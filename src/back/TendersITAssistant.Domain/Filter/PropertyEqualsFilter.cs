namespace TendersITAssistant.Domain.Filter
{
    public class PropertyEqualsFilter : IFilter
    {
        public readonly string PropertyName;

        public readonly object Value;

        public readonly bool Not;

        public PropertyEqualsFilter(string propertyName, object value, bool not = false)
        {
            PropertyName = propertyName;
            Value = value;
            Not = not;
        }
    }
}
