namespace FortitudeCommon.DataStructures.Collections;

public static class CollectionExtensions
{
    public static bool IsNullOrNone<T>(this IEnumerable<T> collection) => !(collection?.Any() ?? false);
    public static bool IsNotNullOrNone<T>(this IEnumerable<T> collection) => !collection.IsNullOrNone();
    public static string JoinToString<T>(this IEnumerable<T> collection, string separator = ", ") => string.Join(separator, collection);
}
