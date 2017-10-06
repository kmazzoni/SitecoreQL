using GraphQL.Types;
using Sitecore.Data.Items;

namespace SitecoreQL.Types
{
    public class ItemArrayType : ObjectGraphType<Item[]>
    {
        public ItemArrayType()
        {
            Field<IntGraphType>("count", "", resolve: context => context.Source.Length);
            Field<ListGraphType<ItemType>>("items", "", resolve: context => context.Source);
        }
    }
}