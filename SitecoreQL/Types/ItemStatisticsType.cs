using GraphQL.Types;
using Sitecore.Data.Items;

namespace SitecoreQL.Types
{
    public class ItemStatisticsType : ObjectGraphType<ItemStatistics>
    {
        public ItemStatisticsType()
        {
            Field(x => x.Created, true).Description("The date the item was created.");
            Field(x => x.CreatedBy, true).Description("The username of the person who created the item.");
            Field(x => x.Updated, true).Description("The date the item was last updated.");
            Field(x => x.UpdatedBy, true).Description("The username of the person who last updated the item.");
        }
    }
}