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
            Field(x => x.TotalSearchResults).Name("totalCount");
            Field<ListGraphType<ItemType>>("items", resolve: context =>
            {
                return context.Source.Select(x => x.Document);
            });
            //Field(x => x.Facets);
        }
    }
}