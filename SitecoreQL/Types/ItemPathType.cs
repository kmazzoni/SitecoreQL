using GraphQL.Types;
using Sitecore.Data;

namespace SitecoreQL.Types
{
    public class ItemPathType : ObjectGraphType<ItemPath>
    {
        public ItemPathType()
        {
            Field(x => x.ContentPath);
            Field(x => x.FullPath);
            Field(x => x.IsContentItem);
            Field(x => x.IsFullyQualified);
            Field(x => x.IsMasterPart);
            Field(x => x.IsMediaItem);
            Field(x => x.MediaPath);
        }
    }
}