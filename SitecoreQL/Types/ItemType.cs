using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using Sitecore.Collections;
using Sitecore.Data.Items;
using Sitecore.Links;

namespace SitecoreQL.Types
{
    public class ItemType : ObjectGraphType<Item>
    {
        public ItemType()
        {
            Name = "SitecoreItem";
            Description = "Sitecore item.";

            Field<GraphQL.Types.IdGraphType>("id", "The item's unique ID.", resolve: context => context.Source.ID.Guid.ToString());
            Field(x => x.Name).Description("The name of the item.");
            Field<StringGraphType>("language", "The language the item was created in.", resolve: context => context.Source.Language.Name);
            Field<StringGraphType>("database", "The name of the database the item was searched from.", resolve: context => context.Source.Database.Name);
            Field<ItemPathType>("paths", "The Sitecore item path.");
            Field<StringGraphType>("url", "The item's relative URL.", resolve: context => LinkManager.GetItemUrl(context.Source));
            Field<IntGraphType>("version", "The version number of the item", resolve: context => context.Source.Version.Number);
            Field<ListGraphType<KeyValuePairType>>("fields", "The item's custom fields.", resolve: context =>
            {
                return context.Source?.Fields?.Select(f => new KeyValuePair<string, string>(f.Key, f.Value as string)) ?? Enumerable.Empty<KeyValuePair<string, string>>();
            });
            Field<ItemType>("template", "The template of the item.", resolve: context => context.Source.Template.InnerItem);
            Field<ItemType>("parent", "The item's parent.", resolve: context => context.Source.Parent);
            Field<ListGraphType<ItemType>>("children", "The direction children of the item.", resolve: context => context.Source.Children.ToArray());
            Field<ItemStatisticsType>("statistics", "The Sitecore item statistics.");
        }
    }
}