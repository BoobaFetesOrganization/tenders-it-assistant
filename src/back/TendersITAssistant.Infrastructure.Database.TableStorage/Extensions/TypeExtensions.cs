using System.Collections;
using System.Reflection;

namespace TendersITAssistant.Infrastructure.Database.TableStorage.Extensions
{
    internal static class TypeExtensions
    {
        public static IEnumerable<string> DetectMembersButProperties(this object obj)
        {
            obj.DetectMembers(out _, out var collections, out var references);
            return [.. collections, .. references];
        }
        public static void DetectMembers(this object obj, out List<string> properties, out List<string> collections, out List<string> references)
        {
            ArgumentNullException.ThrowIfNull(obj);
           
            properties = [];
            collections = [];
            references = [];

            Type type = obj.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in propertyInfos)
            {
                Type propertyType = property.PropertyType;

                if (propertyType.IsCollection()) collections.Add(property.Name);
                else if (!propertyType.IsBaseType() && propertyType.IsInstantiable()) references.Add(property.Name);
                else properties.Add(property.Name);
            }
        }
        
        private static bool IsCollection(this Type type)
        {
            if (type == typeof(string))
                return false;

            return typeof(IEnumerable).IsAssignableFrom(type) ||
                   (type.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
        }
        private static bool IsBaseType(this Type type)
        {
            return type.IsPrimitive || type == typeof(string) || type == typeof(decimal);
        }
        private static bool IsInstantiable(this Type type)
        {
            return type.IsClass && type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}
