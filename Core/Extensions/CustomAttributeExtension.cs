using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AppCore.Extensions
{
    public static class CustomAttributeExtension
    {
        public static T GetCustomAttribute<T>(this ICustomAttributeProvider provider, bool inherit = false) where T : Attribute
        {
            var attribute = provider
                .GetCustomAttributes(inherit)
                .OfType<T>()
                .FirstOrDefault();

            return attribute;
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this ICustomAttributeProvider provider, bool inherit = false) where T : Attribute
        {
            var attributes = provider
                .GetCustomAttributes(inherit)
                .OfType<T>();

            return attributes;
        }
    }
}
