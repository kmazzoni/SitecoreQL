using System;
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
        public ItemQuery(IReadOnlyRepository<GraphQLSearchResultItem> repository, IQueryArgumentsBuilder argumentsBuilder, IArgumentToExpressionConverter argumentsConverter)
        {
            Name = "Query";

            var arguments = argumentsBuilder.GetQueryArguments<ItemType>();
            arguments.AddRange(new[] { new QueryArgument(typeof(IntGraphType)) { Name = "first", DefaultValue = 0 }, new QueryArgument(typeof(IntGraphType)) { Name = "offset", DefaultValue = 0 } });

            Field<SearchQueryType>("search", arguments: arguments, resolve: context =>
            {
                var first = context.GetArgument<int>("first");
                var offset = context.GetArgument<int>("offset");

                var expression = argumentsConverter.ConvertArguments(context.Arguments);
                if (expression != null)
                {
                    return repository.GetMany(expression, first, offset);
                }
                
                return repository.GetAll();
            });

            Field<ItemType>("item", 
                arguments: new QueryArguments(new QueryArgument(typeof(StringGraphType)) {Name = "id"}), 
                resolve: context => repository.GetById(new Guid(context.GetArgument<string>("id"))));
        }

        public class GraphQLSearchResultItem : SearchResultItem
        {
            [IndexField("_latestversion")]
            public bool IsLatestVersion { get; set; }
        }
    }
}