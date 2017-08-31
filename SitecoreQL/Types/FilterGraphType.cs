using System.Linq;
using GraphQL;
using GraphQL.Types;
using SitecoreQL.Query;

namespace SitecoreQL.Types
{
    public class FilterGraphType : InputObjectGraphType
    {
        public FilterGraphType()
        {
            Name = "Filter";

            Field<GraphQL.Types.IdGraphType>("id");
            Field<StringGraphType>("name");
            Field<StringGraphType>("language");
            Field<StringGraphType>("databaseName");
            Field<StringGraphType>("templateId");
            Field<StringGraphType>("parentId");
            Field<StringGraphType>("createdBy");
            Field<StringGraphType>("updatedBy");
            Field<BooleanGraphType>("isLatestVersion");
            Field<StringGraphType>("site");
        }
    }
}