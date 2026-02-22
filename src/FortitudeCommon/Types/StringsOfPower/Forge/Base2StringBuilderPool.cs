using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StringsOfPower.Forge;

public static class Base2StringBuilderPool
{
    private static readonly ConcurrentDictionary<int, Recycler> SizedRecyclerDict = new();

    public static Recycler CreateSizedRecyclerPool(int bufferSize) =>
        new Recycler($"Base2StringBuilderPool({bufferSize})")
            .RegisterFactory(() => new MutableString(bufferSize))
            .RegisterFactory(() => new CharArrayStringBuilder(bufferSize));


    public static CharArrayStringBuilder SourceCharArrayStringBuilder(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        power2Size = Math.Clamp(power2Size, 128, 2 * 1024 * 1024);
        var recycler = SizedRecyclerDict.GetOrAdd(power2Size, CreateSizedRecyclerPool);

        return recycler.Borrow<CharArrayStringBuilder>().EnsureIsAtSize(power2Size);
    }

    public static bool EnsureRecyclerMatchesCurrentCapacity(this CharArrayStringBuilder checkRecyclerMatches)
    {
        if (checkRecyclerMatches.Recycler == null) return false;
        var recycler = SizedRecyclerDict.GetOrAdd(checkRecyclerMatches.Capacity, CreateSizedRecyclerPool);
        if(ReferenceEquals(recycler, checkRecyclerMatches.Recycler)) return false;
        checkRecyclerMatches.Recycler =  recycler;
        return true;
    }

    public static MutableString SourceMutableString(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        var recycler = SizedRecyclerDict .GetOrAdd (power2Size, CreateSizedRecyclerPool);

        return recycler.Borrow<MutableString>();
    }
}
