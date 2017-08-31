using GraphQL.Types;

namespace SitecoreQL.Types
{
    public class SortGraphType : InputObjectGraphType
    {
        public SortGraphType()
        {
            Name = "Sort";
            Field<StringGraphType>("field");
            Field<StringGraphType>("dir");
        }
    }
}