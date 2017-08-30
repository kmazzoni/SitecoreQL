using System;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using SitecoreQL.Types;

namespace SitecoreQL.Query
{
    public interface IQueryArgumentsBuilder
    {
        QueryArguments GetQueryArguments<T>() where T : ObjectGraphType<ItemQuery.GraphQLSearchResultItem>;
    }

    public class QueryArgumentsBuilder : IQueryArgumentsBuilder
    {
        private readonly ItemType _itemType;

        public QueryArgumentsBuilder(ItemType itemType)
        {
            _itemType = itemType;
        }

        public QueryArguments GetQueryArguments<T>() where T : ObjectGraphType<ItemQuery.GraphQLSearchResultItem>
        {
            return new QueryArguments(_itemType.Fields.Where(f => f.Type.IsSubclassOf(typeof(ScalarGraphType))).Select(GetQueryArgument));
        }

        protected QueryArgument GetQueryArgument(FieldType field)
        {
            return new QueryArgument(field.Type) { Name = field.Name.ToCamelCase() };
        }
    }
}