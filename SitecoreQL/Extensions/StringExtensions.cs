using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreQL.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string input)
        {
            string newName = input.Replace(" ", string.Empty).Replace("_", string.Empty);

            return $"{newName[0].ToString().ToLowerInvariant()}{newName.Substring(1)}";
        }
    }
}