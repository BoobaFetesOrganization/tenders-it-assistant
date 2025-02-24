using GenAIChat.Domain.Filter;

namespace GenAIChat.Application.Specifications
{
    public static class FilterExtensions
    {
        public static string ToAzureFilterString(this IFilter filter)
        {
            if (filter is PropertyEqualsFilter propertyEqualsFilter)
            {
                // Vérification du type de la propriété pour une gestion appropriée dans la chaîne de filtre
                if (propertyEqualsFilter.Value is string)
                {
                    return $"{propertyEqualsFilter.PropertyName} eq '{propertyEqualsFilter.Value}'";
                }
                else if (propertyEqualsFilter.Value is int || propertyEqualsFilter.Value is double || propertyEqualsFilter.Value is long || propertyEqualsFilter.Value is float || propertyEqualsFilter.Value is decimal)
                {
                    return $"{propertyEqualsFilter.PropertyName} eq {propertyEqualsFilter.Value}";
                }
                else if (propertyEqualsFilter.Value is DateTime)
                {
                    return $"{propertyEqualsFilter.PropertyName} eq datetime'{((DateTime)propertyEqualsFilter.Value).ToString("yyyy-MM-ddTHH:mm:ss")}'"; // Format ISO 8601
                }
                // Gestion des autres types (booléen, Guid, etc.) selon vos besoins
                else
                {
                    throw new NotSupportedException($"Type de propriété non pris en charge : {propertyEqualsFilter.Value.GetType()}");
                }
            }

            throw new NotSupportedException($"The filter of type '{filter.GetType().Name}' is unknow");
        }
    }
}