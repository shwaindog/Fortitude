using System;

namespace FortitudeCommon.Configuration.KeyValueProperties
{
    public static class ConfigItemExtensionMethods
    {
        public static decimal? AsDecimal(this IConfigItem item)
        {
            if (item == null) return null;
            decimal result;
            return decimal.TryParse(item.Value, out result) ? result : (decimal?) null;
        }

        public static double? AsDouble(this IConfigItem item)
        {
            if (item == null) return null;
            double result;
            return double.TryParse(item.Value, out result) ? result : (double?) null;
        }

        public static int? AsInt(this IConfigItem item)
        {
            if (item == null) return null;
            int result;
            return int.TryParse(item.Value, out result) ? result : (int?) null;
        }

        public static uint? AsUInt(this IConfigItem item)
        {
            if (item == null) return null;
            uint result;
            return uint.TryParse(item.Value, out result) ? result : (uint?) null;
        }

        public static short? AsShort(this IConfigItem item)
        {
            if (item == null) return null;
            short result;
            return short.TryParse(item.Value, out result) ? result : (short?) null;
        }

        public static ushort? AsUShort(this IConfigItem item)
        {
            if (item == null) return null;
            ushort result;
            return ushort.TryParse(item.Value, out result) ? result : (ushort?) null;
        }

        public static byte? AsByte(this IConfigItem item)
        {
            if (item == null) return null;
            byte result;
            return byte.TryParse(item.Value, out result) ? result : (byte?) null;
        }

        public static sbyte? AsSByte(this IConfigItem item)
        {
            if (item == null) return null;
            sbyte result;
            return sbyte.TryParse(item.Value, out result) ? result : (sbyte?) null;
        }

        public static DateTime? AsDateTime(this IConfigItem item)
        {
            if (item == null) return null;
            DateTime result;
            return DateTime.TryParse(item.Value, out result) ? result : (DateTime?) null;
        }
    }
}