namespace FortitudeCommon.DataStructures.Lists;

public static class ListExtensions
{
    private static Random random = new();

    public static void Shuffle<T>(this IList<T> toShuffle)
    {
        var n = toShuffle.Count;
        while (n > 1)
        {
            n--;
            var k = random.Next(n + 1);
            (toShuffle[k], toShuffle[n]) = (toShuffle[n], toShuffle[k]);
        }
    }

    public static T AddReturn<T>(this IList<T> list, T value)
    {
        list.Add(value);
        return value;
    }
}
