using System;
using GraphQL.Types;
using SitecoreQL.Query;

namespace SitecoreQL.Schema
{
    public class SitecoreSchema : GraphQL.Types.Schema
    {
        public SitecoreSchema(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (ItemQuery)resolveType(typeof(ItemQuery));
        }
    }
}