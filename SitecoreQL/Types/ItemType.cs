using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GraphQL.Types;
using GraphQLParser;
using SitecoreQL.Query;
using SitecoreQL.Repositories;

namespace SitecoreQL.Types
{
    public class ItemType : ObjectGraphType<ItemQuery.GraphQLSearchResultItem>
    {
        public ItemType(IReadOnlyRepository<ItemQuery.GraphQLSearchResultItem> repo)
        {
            Name = "SitecoreItem";
            Description = "Sitecore item.";

            Field<GraphQL.Types.IdGraphType>("id", "The item's unique ID.", resolve: context => context.Source.ItemId.Guid.ToString());
            Field(x => x.Name, true).Description("The name of the item.");
            Field(x => x.Language, true).Description("The language the item was created in.");
            Field(x => x.DatabaseName, true).Description("The name of the database the item was searched from.");
            Field(x => x.Path, true).Description("The Sitecore item path.");
            Field(x => x.Url, true).Description("The item's relative URL.");
            Field(x => x.CreatedDate, true).Description("The date the item was created.");
            Field(x => x.CreatedBy, true).Description("The username of the person who created the item.");
            Field(x => x.Updated, true).Description("The date the item was last updated.");
            Field(x => x.UpdatedBy, true).Description("The username of the person who last updated the item.");
            Field(x => x.Version, true).Description("The version number of the item");
            Field<ListGraphType<KeyValuePairType>>("fields", "The item's custom fields.", resolve: context =>
            {
                return context.Source?.GetItem().Fields?.Select(f => new KeyValuePair<string, string>(f.Key, f.Value as string)) ?? Enumerable.Empty<KeyValuePair<string,string>>();
            });
            Field<ItemType>("template", "The template of the item.", resolve: context => repo.GetById(context.Source.TemplateId.Guid));
            Field<ItemType>("parent", "The item's parent.", resolve: context => repo.GetById(context.Source.Parent.Guid));
            Field<ListGraphType<ItemType>>("children", "The direction children of the item.", resolve: context => repo.GetMany(x => x.Parent == context.Source.ItemId).Select(h => h.Document));
        }
    }

    public class KeyValuePairType : ObjectGraphType<KeyValuePair<string, string>>
    {
        public KeyValuePairType()
        {
            Field(x => x.Key);
            Field(x => x.Value);
        }
    }
}