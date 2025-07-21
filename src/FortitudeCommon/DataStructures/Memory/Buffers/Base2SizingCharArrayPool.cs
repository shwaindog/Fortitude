using System.Collections.Concurrent;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.DataStructures.Memory;

public class Base2SizingCharArrayPool
{
    private readonly ConcurrentDictionary<int, Recycler> sizedRecycler = new ();

    public RecyclingCharArray Borrow(int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        var recycler = sizedRecycler.GetOrAdd(power2Size, _ => new Recycler());

        return recycler.Borrow<RecyclingCharArray>().EnsureIsAtSize(power2Size);
    }
}