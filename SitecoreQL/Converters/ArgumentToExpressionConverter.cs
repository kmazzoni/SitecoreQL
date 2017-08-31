using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using GraphQL;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using SitecoreQL.Query;
using SitecoreQL.Types;

namespace SitecoreQL.Converters
{
    public class ArgumentToExpressionConverter : IArgumentToExpressionConverter
    {
        public Expression<Func<ItemQuery.GraphQLSearchResultItem, bool>> ConvertToFilter(IDictionary<string, object> arguments)
        {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(ItemQuery.GraphQLSearchResultItem), "item");

            var predicate = PredicateBuilder.True<ItemQuery.GraphQLSearchResultItem>();
            foreach (var argument in arguments.Where(a => a.Value != null))
            {
                var propertyInfo = GetProperty<ItemQuery.GraphQLSearchResultItem>(argument.Key);

                if(propertyInfo == null) continue;

                var @where = GetWhereClause(parameterExpression, propertyInfo, argument.Value);

                predicate = predicate.And(@where);
            }

            return predicate;
        }

        public Func<IQueryable<ItemQuery.GraphQLSearchResultItem>, IOrderedQueryable<ItemQuery.GraphQLSearchResultItem>> ConvertToOrderBy(IDictionary<string, object> arguments)
        {
            SortOption sortOption = arguments.ToObject<SortOption>();

            return q =>
            {
                Expression<Func<ItemQuery.GraphQLSearchResultItem, IComparable>> expression = GenerateMemberExpression<IComparable>(sortOption.Field);

                if (expression != null)
                {
                    return sortOption.Dir == SortDirection.DESC ? q.OrderByDescending(expression) : q.OrderBy(expression);
                }

                return null;
            };
        }

        private Expression<Func<ItemQuery.GraphQLSearchResultItem, TK>> GenerateMemberExpression<TK>(string propertyName)
        {
            var propertyInfo = GetProperty<ItemQuery.GraphQLSearchResultItem>(propertyName);

            if (propertyInfo == null) return null;

            var entityParam = Expression.Parameter(typeof(ItemQuery.GraphQLSearchResultItem), "e");

            var columnExpr = GetPropertyExpression(entityParam, propertyInfo);

            return Expression.Lambda<Func<ItemQuery.GraphQLSearchResultItem, TK>>(columnExpr, entityParam);
        }

        public Expression<Func<ItemQuery.GraphQLSearchResultItem, bool>> GetWhereClause(ParameterExpression parameterExpression, PropertyInfo propertyInfo,
            object argumentValue)
        {
            var fieldExpression = GetPropertyExpression(parameterExpression, propertyInfo);
            var valueExpression = GetValueExpression(propertyInfo, argumentValue);

            Expression binaryExpression = Expression.Equal(fieldExpression, valueExpression);
            if (propertyInfo.PropertyType == typeof(IEnumerable<>))
            {
                binaryExpression = Expression.Call(fieldExpression, "Contains", null, valueExpression);
            }
            
            return Expression.Lambda<Func<ItemQuery.GraphQLSearchResultItem, bool>>(binaryExpression,
                parameterExpression);
        }

        public Expression GetPropertyExpression(ParameterExpression parameterExpression, PropertyInfo propertyInfo)
        {
            return Expression.Property(parameterExpression, parameterExpression.Type, propertyInfo.Name);
        }

        public Expression GetValueExpression(PropertyInfo propertyInfo, object value)
        {
            if (propertyInfo.PropertyType == typeof(string))
            {
                return Expression.Constant((string)value);
            }

            if (propertyInfo.PropertyType == typeof(Guid))
            {
                Guid guid = Guid.Parse(value.ToString());

                return Expression.Constant(guid);
            }

            if (propertyInfo.PropertyType == typeof(ID))
            {
                Guid guid = Guid.Parse(value.ToString());

                return Expression.Constant(new ID(guid));
            }

            if (propertyInfo.PropertyType == typeof(int))
            {
                int i = int.Parse(value.ToString());

                return Expression.Constant(i);
            }

            if (propertyInfo.PropertyType == typeof(DateTime))
            {
                DateTime date = DateTime.Parse(value.ToString());

                return Expression.Constant(date);
            }

            if (propertyInfo.PropertyType == typeof(bool))
            {
                bool boolean = bool.Parse(value.ToString());

                return Expression.Constant(boolean);
            }

            return Expression.Constant((string) value);
        }

        public PropertyInfo GetProperty<T>(string fieldName) where T : SearchResultItem
        {
            var propertyInfo = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(IndexFieldAttribute), false).Any())
                .FirstOrDefault(p => p.Name.Equals(fieldName, StringComparison.InvariantCultureIgnoreCase));

            // custom case
            if (propertyInfo == null && fieldName.Equals("id", StringComparison.InvariantCultureIgnoreCase))
            {
                propertyInfo = typeof(T).GetProperties()
                    .FirstOrDefault(p => p.Name.Equals("itemid", StringComparison.InvariantCultureIgnoreCase));
            }

            return propertyInfo;
        }
    }

    public class SortOption
    {
        public string Field { get; set; }
        public SortDirection Dir { get; set; }
    }
}