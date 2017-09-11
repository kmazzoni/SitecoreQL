using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Sitecore.Data.Items;

namespace SitecoreQL.Types
{
    public class ItemType : ObjectGraphType<Item>
    {
        public ItemType()
        {
            Name = "SitecoreItem";
            Description = "Sitecore item.";

            Field<GraphQL.Types.IdGraphType>("id", "The item's unique ID.", resolve: context => context.Source.ID.Guid.ToString());
            Field(x => x.Name, true).Description("The name of the item.");
            /*
            Field(x => x.Language, true).Description("The language the item was created in.");
            Field(x => x.DatabaseName, true).Description("The name of the database the item was searched from.");
            Field(x => x.Path, true).Description("The Sitecore item path.");
            Field(x => x.Url, true).Description("The item's relative URL.");
            Field(x => x.CreatedDate, true).Description("The date the item was created.");
            Field(x => x.CreatedBy, true).Description("The username of the person who created the item.");
            Field(x => x.Updated, true).Description("The date the item was last updated.");
            Field(x => x.UpdatedBy, true).Description("The username of the person who last updated the item.");
            Field(x => x.Version, true).Description("The version number of the item");
            */
            Field<ListGraphType<KeyValuePairType>>("fields", "The item's custom fields.", resolve: context =>
            {
                return context.Source?.Fields?.Select(f => new KeyValuePair<string, string>(f.Key, f.Value as string)) ?? Enumerable.Empty<KeyValuePair<string, string>>();
            });
            Field<SearchItemType>("template", "The template of the item.", resolve: context => context.Source.Template.InnerItem);
            Field<SearchItemType>("parent", "The item's parent.", resolve: context => context.Source.Parent);
            Field<ListGraphType<SearchItemType>>("children", "The direction children of the item.", resolve: context => context.Source.Children.ToArray());
        }
    }
}