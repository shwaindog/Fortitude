using System.Collections.Generic;
using System.Text;

namespace FortitudeCommon.DataStructures.Maps.IdMap
{
    public static class NameIdLookupHelpers
    {
        public static string ListDictionaryContents<K, V>(this IDictionary<K, V> printContents)
        {
            var sb = new StringBuilder(printContents.Count * 20);
            var firstLine = true;
            foreach (var kvp in printContents)
            {
                if (!firstLine) sb.Append(", ");
                sb.Append(kvp.Key).Append(":").Append(kvp.Value);
                firstLine = false;
            }
            return sb.ToString();
        }
    }
}