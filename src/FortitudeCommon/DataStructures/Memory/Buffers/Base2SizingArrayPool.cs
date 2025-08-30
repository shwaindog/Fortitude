using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.DataStructures.Memory.Buffers;

public static class Base2SizingArrayPool
{
    private static readonly ConcurrentDictionary<int, Recycler> SizedRecyclerDict = new();

    public static Recycler CreateSizedRecyclerPool(int bufferSize) =>
        new Recycler($"Base2SizingArrayPool({bufferSize})")
            .RegisterFactory(() => new RecyclingCharArray(bufferSize))
            .RegisterFactory(() => new CharArrayStringBuilder(bufferSize))
            .RegisterFactory(() => new RecyclingByteArray(bufferSize));

    public static RecyclingCharArray SourceRecyclingCharArray(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        var recycler = SizedRecyclerDict.GetOrAdd (power2Size, CreateSizedRecyclerPool);

        return recycler.Borrow<RecyclingCharArray>().EnsureIsAtSize(power2Size);
    }

    public static CharArrayStringBuilder SourceCharArrayStringBuilder(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

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

    public static RecyclingByteArray SourceRecyclingByteArray(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        var recycler = SizedRecyclerDict .GetOrAdd (power2Size, CreateSizedRecyclerPool);

        return recycler.Borrow<RecyclingByteArray>().EnsureIsAtSize(power2Size);
    }
}
