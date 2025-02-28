using GenAIChat.Domain.Common;
using GenAIChat.Domain.Filter;
using System.Linq.Expressions;

namespace GenAIChat.Infrastructure.Database.Sqlite.Extensions
{
    public static class FilterExtensions
    {
        public static Expression<Func<TDomain, bool>> ToQueryableExpression<TDomain>(this IFilter filter) where TDomain : class, IEntityDomain
        {
            return filter switch
            {
                PropertyEqualsFilter propertyEqualsFilter => ConvertPropertyEqualsFilter<TDomain>(propertyEqualsFilter),
                AndFilter andFilter => ConvertAndFilter<TDomain>(andFilter),
                _ => throw new NotSupportedException($"The filter of type '{filter.GetType().Name}' is unknown"),
            };

            throw new NotSupportedException($"The filter of type '{filter.GetType().Name}' is unknown");
        }

        private static Expression<Func<TDomain, bool>> ConvertAndFilter<TDomain>(AndFilter andFilter) where TDomain : class, IEntityDomain
        {
            var left = andFilter.Left.ToQueryableExpression<TDomain>();
            var right = andFilter.Right.ToQueryableExpression<TDomain>();

            var body = Expression.AndAlso(left.Body, right.Body);
            return Expression.Lambda<Func<TDomain, bool>>(body, left.Parameters);
        }

        private static Expression<Func<TDomain, bool>> ConvertPropertyEqualsFilter<TDomain>(PropertyEqualsFilter propertyEqualsFilter) where TDomain : class, IEntityDomain
        {
            var parameter = Expression.Parameter(typeof(TDomain), "x");
            var property = Expression.Property(parameter, propertyEqualsFilter.PropertyName);
            var constant = Expression.Constant(propertyEqualsFilter.Value);

            var body = Expression.Equal(property, constant);
            return Expression.Lambda<Func<TDomain, bool>>(body, parameter);
        }
    }
}