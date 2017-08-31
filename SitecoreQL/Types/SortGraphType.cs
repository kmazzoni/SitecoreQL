using GraphQL.Types;
using Sitecore.ContentSearch.Linq;

namespace SitecoreQL.Types
{
    public class SortGraphType : InputObjectGraphType
    {
        public SortGraphType()
        {
            Name = "Sort";
            Description = "Specify the field to sort on and direction.";
            Field<StringGraphType>("field");
            Field<SortDirectionGraphType>("dir");
        }
    }

    public class FacetsGraphType : ObjectGraphType<FacetCategory>
    {
        public FacetsGraphType()
        {
            Name = "Facets";

            Field(x => x.Name);
            Field(x => x.Values, type:typeof(ListGraphType<FacetValueGraphType>));
        }
    }

    public class FacetValueGraphType : ObjectGraphType<FacetValue>
    {
        public FacetValueGraphType()
        {
            Name = "FacetValues";

            Field(x => x.Name);
            Field(x => x.AggregateCount).Name("count");
        }
    }
}