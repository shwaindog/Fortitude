using System;
using System.Linq;

namespace FortitudeCommon.Types
{
    public static class EnumHelper
    {
        public static TEnum AllFlags<TEnum>(this TEnum myEnum)
            where TEnum : struct
        {
            var enumType = typeof(TEnum);
            var enumValues = Enum.GetValues(enumType);
            var newValue = (from object value in enumValues
                             select (long) Convert.ChangeType(value, TypeCode.Int64))
                             .Aggregate<long, long>(0, (current, v) => current | v);
            return (TEnum)Enum.ToObject(enumType, newValue);
        }
    }
}
