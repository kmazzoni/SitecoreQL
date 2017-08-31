using System.Linq;
using GraphQL.Types;
using Sitecore.ContentSearch.Linq;
using SitecoreQL.Query;

namespace SitecoreQL.Types
{
    public class SearchQueryType : ObjectGraphType<SearchResults<ItemQuery.GraphQLSearchResultItem>>
    {
        public SearchQueryType()
        {
            Name = "SearchQueryResult";
            Description = "Search results.";

            Field(x => x.TotalSearchResults).Name("totalCount").Description("Total number of items matching the search criteria.");
            Field<ListGraphType<ItemType>>("items", "The items returned from the search.", resolve: context =>
            {
                return context.Source.Select(x => x.Document);
            });
            Field<ListGraphType<FacetsGraphType>>("facets", "The set of requested facets and their values/counts.", resolve: context => context.Source.Facets.Categories);
        }
    }
}