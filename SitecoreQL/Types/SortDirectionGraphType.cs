using GraphQL.Types;

namespace SitecoreQL.Types
{
    public class SortDirectionGraphType : EnumerationGraphType
    {
        public SortDirectionGraphType()
        {
            Name = "SortDirection";
            Description = "Direction to sort results (asc/desc)";
            AddValue("ASC", "Ascending", 1);
            AddValue("DESC", "Descending", 2);
        }
    }

    public enum SortDirection
    {
        ASC = 1,
        DESC = 2
    }
}