using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SitecoreQL.Query;

namespace SitecoreQL.Converters
{
    public interface IArgumentToExpressionConverter
    {
        Expression<Func<ItemQuery.GraphQLSearchResultItem, bool>> ConvertToFilter(IDictionary<string, object> arguments);
        Func<IQueryable<ItemQuery.GraphQLSearchResultItem>, IOrderedQueryable<ItemQuery.GraphQLSearchResultItem>> ConvertToOrderBy(IDictionary<string, object> arguments);
        IEnumerable<Expression<Func<ItemQuery.GraphQLSearchResultItem, object>>> ConvertToFacets(IEnumerable<string> arguments);
    }
}
