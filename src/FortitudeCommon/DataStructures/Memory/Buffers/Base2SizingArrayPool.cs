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
            .RegisterFactory(() => new RecyclingByteArray(bufferSize));

    public static RecyclingCharArray SourceRecyclingCharArray(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        var recycler = SizedRecyclerDict.GetOrAdd (power2Size, CreateSizedRecyclerPool);

        return recycler.Borrow<RecyclingCharArray>().EnsureIsAtSize(power2Size);
    }

    public static RecyclingByteArray SourceRecyclingByteArray(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        var recycler = SizedRecyclerDict .GetOrAdd (power2Size, CreateSizedRecyclerPool);

        return recycler.Borrow<RecyclingByteArray>().EnsureIsAtSize(power2Size);
    }
}
