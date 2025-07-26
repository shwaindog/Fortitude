using System.Collections.Concurrent;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.DataStructures.Memory.Buffers;

public static class Base2SizingCharArrayPool
{
    private static readonly ConcurrentDictionary<int, Recycler> SizedRecyclerDict = new();

    public static RecyclingCharArray SourceRecyclingCharArray(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        var recycler = SizedRecyclerDict
            .GetOrAdd
                (power2Size, size =>
                    new Recycler().RegisterFactory(() => new RecyclingCharArray(size)));

        return recycler.Borrow<RecyclingCharArray>().EnsureIsAtSize(power2Size);
    }

    public static CharArrayStringBuilder SourceCharArrayStringBuilder(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        var recycler = SizedRecyclerDict
            .GetOrAdd
                (power2Size, size =>
                    new Recycler().RegisterFactory(() => new CharArrayStringBuilder(size)));

        return recycler.Borrow<CharArrayStringBuilder>().EnsureIsAtSize(power2Size);
    }

    public static AsyncReadWriterRecyclingCharArray SourceAsyncReadWriterRecyclingCharArray(this int atLeastOfSize)
    {
        var power2Size = atLeastOfSize.NextPowerOfTwo();

        var recycler = SizedRecyclerDict
            .GetOrAdd
                (power2Size, size =>
                    new Recycler().RegisterFactory(() => new AsyncReadWriterRecyclingCharArray(size)));

        return recycler.Borrow<AsyncReadWriterRecyclingCharArray>().EnsureIsAtSize(power2Size);
    }
}
