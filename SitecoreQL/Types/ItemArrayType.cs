using GraphQL.Types;
using Sitecore.Data.Items;

namespace SitecoreQL.Types
{
    public class ItemArrayType : ObjectGraphType<Item[]>
    {
        public ItemArrayType()
        {
            Name = "ItemArray";

            Field<IntGraphType>("count", "The number of items returned from the xpath query.", resolve: context => context.Source.Length);
            Field<ListGraphType<ItemType>>("items", "The items matching the xpath query.", resolve: context => context.Source);
        }
    }
}