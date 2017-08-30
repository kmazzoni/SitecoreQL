using System.Linq;
using GraphQL.Types;
using SitecoreQL.Query;
using SitecoreQL.Repositories;

namespace SitecoreQL.Types
{
    public class ItemType : ObjectGraphType<ItemQuery.GraphQLSearchResultItem>
    {
        public ItemType(IReadOnlyRepository<ItemQuery.GraphQLSearchResultItem> repo) 
        {
            Field<GraphQL.Types.IdGraphType>("id", resolve: context => context.Source.ItemId.Guid.ToString());
            Field(x => x.Name, true);
            Field(x => x.Language, true);
            Field(x => x.DatabaseName, true);
            Field(x => x.Path, true);
            Field(x => x.Url, true);
            Field(x => x.CreatedDate, true);
            Field(x => x.CreatedBy, true);
            Field(x => x.Updated, true);
            Field(x => x.UpdatedBy, true);
            Field(x => x.Version, true);
            Field<ItemType>("template", resolve: context => repo.GetById(context.Source.TemplateId.Guid));
            Field<ItemType>("parent", resolve: context => repo.GetById(context.Source.Parent.Guid));
            Field<ListGraphType<ItemType>>("children", resolve: context => repo.GetMany(x => x.Parent == context.Source.ItemId).Select(h => h.Document));
        }
    }
}