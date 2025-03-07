using TendersITAssistant.Domain.Filter;

namespace TendersITAssistant.Application.Specifications
{
    public static class FilterExtensions
    {
        public static string ToAzureFilterString(this IFilter filter)
        {
            switch (filter)
            {
                case PropertyEqualsFilter propertyEqualsFilter:
                    return ConvertPropertyEqualsFilterAsString(propertyEqualsFilter);
                case AndFilter andFilter:
                    return ConvertAndFilterAsString(andFilter);
            }

            throw new NotSupportedException($"The filter of type '{filter.GetType().Name}' is unknow");
        }

        private static string ConvertAndFilterAsString(AndFilter andFilter)
        {
            var left = andFilter.Left.ToAzureFilterString();
            var right = andFilter.Right.ToAzureFilterString();
            return $"({left} and {right})";
        }

        private static string ConvertPropertyEqualsFilterAsString(PropertyEqualsFilter propertyEqualsFilter)
        {
            return propertyEqualsFilter.Value switch
            {
                string stringValue => $"{propertyEqualsFilter.PropertyName} eq '{stringValue}'",
                int or double or long or float or decimal => $"{propertyEqualsFilter.PropertyName} eq {propertyEqualsFilter.Value}",
                DateTime => $"{propertyEqualsFilter.PropertyName} eq datetime'{((DateTime)propertyEqualsFilter.Value).ToString("yyyy-MM-ddTHH:mm:ss")}'",
                _ => throw new NotSupportedException($"Type de propriété non pris en charge : {propertyEqualsFilter.Value.GetType()}")
            };
        }
    }
}