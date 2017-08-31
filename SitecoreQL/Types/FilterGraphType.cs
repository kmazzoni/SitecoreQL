using GraphQL.Types;

namespace SitecoreQL.Types
{
    public class FilterGraphType : InputObjectGraphType
    {
        public FilterGraphType()
        {
            Name = "Filter";

            Field<GraphQL.Types.IdGraphType>("id", "The item's unique ID.");
            Field<StringGraphType>("name", "The name of the item.");
            Field<StringGraphType>("language", "The language the item was created in.");
            Field<StringGraphType>("databaseName", "The name of the database the item was searched from.");
            Field<StringGraphType>("templateId", "The ID of the item's template.");
            Field<StringGraphType>("templateName", "The name of the item's template.");
            Field<StringGraphType>("parentId", "The ID of the item's parent.");
            Field<StringGraphType>("createdBy", "The username of the person who created the item.");
            Field<StringGraphType>("updatedBy", "The username of the person who last updated the item.");
            Field<BooleanGraphType>("isLatestVersion", "True/False whether this is the latest version of the item.");
            Field<StringGraphType>("site", "The name of the site the item belongs to.");
        }
    }
}