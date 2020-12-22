using System;

namespace MyBeers.Common.Extensions
{
    public static class Converters
    {
        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
