using System;
using GraphQL.Language.AST;
using GraphQL.Types;
using Sitecore.Data;

namespace SitecoreQL.Types
{
    public class IdGraphType : ScalarGraphType
    {
        public IdGraphType()
        {
            Name = "ID";
        }

        public override object Serialize(object value)
        {
            return new ID(value.ToString()).Guid.ToString();
        }

        public override object ParseValue(object value)
        {
            if (value == null) return null;
            Guid result1;
            if (Guid.TryParse(value.ToString(), out result1))
            {
                return new ID(result1);
            }
                
            return null;
        }

        public override object ParseLiteral(IValue value)
        {
            StringValue intValue = value as StringValue;
            if (intValue != null)
                return new ID(intValue.Value);

            return null;
        }
    }
}