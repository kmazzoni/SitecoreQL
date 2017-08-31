using System;
using System.Collections.Generic;
using GraphQL.Types;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using SitecoreQL.Converters;
using SitecoreQL.Repositories;
using SitecoreQL.Types;

namespace SitecoreQL.Query
{
    public class ItemQuery : ObjectGraphType
    {
        public ItemQuery(IReadOnlyRepository<GraphQLSearchResultItem> repository, IArgumentToExpressionConverter argumentsConverter)
        {
            Name = "Query";

            var queryArguments = new QueryArguments
            {
                new QueryArgument<FilterGraphType> {Name = "filter", DefaultValue = new Dictionary<string, object>()},
                new QueryArgument<IntGraphType> {Name = "first", DefaultValue = 0},
                new QueryArgument<IntGraphType> {Name = "offset", DefaultValue = 0},
                new QueryArgument<SortGraphType> {Name = "sort", DefaultValue = new Dictionary<string, object>()}
            };

            Field<SearchQueryType>("search", arguments: queryArguments, resolve: context =>
            {
                var filter = context.GetArgument<Dictionary<string,object>>("filter");
                var first = context.GetArgument<int>("first");
                var offset = context.GetArgument<int>("offset");
                var sort = context.GetArgument<Dictionary<string, object>>("sort");

                var filterExpression = argumentsConverter.ConvertToFilter(filter);
                var orderByExpression = argumentsConverter.ConvertToOrderBy(sort);
                 
                return repository.GetMany(filterExpression, orderByExpression, first, offset);
            });

            Field<ItemType>("item", 
                arguments: new QueryArguments(new QueryArgument(typeof(StringGraphType)) {Name = "id"}), 
                resolve: context => repository.GetById(new Guid(context.GetArgument<string>("id"))));
        }

        public class GraphQLSearchResultItem : SearchResultItem
        {
            [IndexField("_latestversion")]
            public bool IsLatestVersion { get; set; }

            [IndexField("_group")]
            public virtual Guid Id { get; set; }

            [IndexField("_parent")]
            public virtual Guid ParentId { get; set; }

            [IndexField("site")]
            public virtual IEnumerable<string> Site { get; set; }
        }
    }
}