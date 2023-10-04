namespace FortitudeCommon.DataStructures.Collections;

public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => !(collection?.Any() ?? false);
    public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> collection) => !collection.IsNullOrEmpty();
}
