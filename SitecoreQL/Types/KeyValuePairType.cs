using System.Collections.Generic;
using GraphQL.Types;

namespace SitecoreQL.Types
{
    public class KeyValuePairType : ObjectGraphType<KeyValuePair<string, string>>
    {
        public KeyValuePairType()
        {
            Field(x => x.Key);
            Field(x => x.Value);
        }
    }
}