namespace GenAIChat.Domain.Filter
{
    public class PropertyEqualsFilter : IFilter
    {
        public readonly string PropertyName;

        public readonly object Value;

        public PropertyEqualsFilter(string propertyName, object value)
        {
            PropertyName = propertyName;
            Value = value;
        }
    }
}
