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
            Description = "Query for Sitecore items";

            var queryArguments = new QueryArguments
            {
                new QueryArgument<FilterGraphType> {Name = "filter", DefaultValue = new Dictionary<string, object>()},
                new QueryArgument<IntGraphType> {Name = "first", DefaultValue = 0},
                new QueryArgument<IntGraphType> {Name = "offset", DefaultValue = 0},
                new QueryArgument<SortGraphType> {Name = "sort", DefaultValue = new Dictionary<string, object>()},
                new QueryArgument<ListGraphType<StringGraphType>> { Name = "facets", DefaultValue = new string[] { } }
            };

            Field<SearchQueryType>("search", "Search against all Sitecore items by providing various filter/sort/faceting options.",
                queryArguments,
                context =>
            {
                var filter = context.GetArgument<Dictionary<string, object>>("filter");
                var first = context.GetArgument<int>("first");
                var offset = context.GetArgument<int>("offset");
                var sort = context.GetArgument<Dictionary<string, object>>("sort");
                var facets = context.GetArgument<IList<string>>("facets");

                var filterExpression = argumentsConverter.ConvertToFilter(filter);
                var orderByExpression = argumentsConverter.ConvertToOrderBy(sort);
                var facetOnExpression = argumentsConverter.ConvertToFacets(facets);

                return repository.GetMany(filterExpression, orderByExpression, facetOnExpression, first, offset);
            });

            Field<SearchItemType>("item", "Lookup a single Sitecore item by it's ID.",
                new QueryArguments(new QueryArgument(typeof(StringGraphType)) { Name = "id" }),
                context => repository.GetById(new Guid(context.GetArgument<string>("id"))));
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