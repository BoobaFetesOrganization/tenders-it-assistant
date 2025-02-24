namespace GenAIChat.Domain.Filter
{
    public class PropertyEqualsFilter : IFilter
    {
        public readonly string PropertyName;

        public readonly object Value;

        public readonly bool Insensitive;

        public PropertyEqualsFilter(string propertyName, object value, bool insensitive = false)
        {
            PropertyName = propertyName;
            Value = value;
            Insensitive = insensitive;
        }
    }
}
