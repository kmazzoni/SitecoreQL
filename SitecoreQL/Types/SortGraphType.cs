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
            Field<StringGraphType>("field", "The Sitecore field to sort all results by.");
            Field<SortDirectionGraphType>("dir", "The direction the results should be ordered by (ASC/DESC).");
        }
    }

    public class FacetsGraphType : ObjectGraphType<FacetCategory>
    {
        public FacetsGraphType()
        {
            Name = "Facets";

            Field(x => x.Name).Description("The facet's name.");
            Field(x => x.Values, type:typeof(ListGraphType<FacetValueGraphType>)).Description("The valid values for this facet.");
        }
    }

    public class FacetValueGraphType : ObjectGraphType<FacetValue>
    {
        public FacetValueGraphType()
        {
            Name = "FacetValues";

            Field(x => x.Name).Description("The facet value's name.");
            Field(x => x.AggregateCount).Name("count").Description("The number of item's whose field value matches this facet value's name.");
        }
    }
}