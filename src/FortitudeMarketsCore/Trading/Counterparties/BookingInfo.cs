#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Counterparties;

#endregion

namespace FortitudeMarketsCore.Trading.Counterparties;

public class BookingInfo : IBookingInfo
{
    private int refCount = 0;

    public BookingInfo(IBookingInfo toClone)
    {
        Portfolio = toClone.Portfolio;
        SubPortfolio = toClone.SubPortfolio;
    }

    public BookingInfo(string portfolio, string subPortfolio)
        : this((MutableString)portfolio, (MutableString)subPortfolio) { }

    public BookingInfo(IMutableString portfolio, IMutableString subPortfolio)
    {
        Portfolio = portfolio;
        SubPortfolio = subPortfolio;
    }

    public IMutableString? Portfolio { get; set; }
    public IMutableString? SubPortfolio { get; set; }

    public void CopyFrom(IBookingInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        throw new NotImplementedException();
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        throw new NotImplementedException();
    }

    public int RefCount => refCount;
    public bool RecycleOnRefCountZero { get; set; } = true;
    public bool AutoRecycledByProducer { get; set; }
    public bool IsInRecycler { get; set; }
    public IRecycler? Recycler { get; set; }
    public int DecrementRefCount() => Interlocked.Decrement(ref refCount);

    public int IncrementRefCount() => Interlocked.Increment(ref refCount);

    public bool Recycle()
    {
        if (refCount == 0 || !RecycleOnRefCountZero) Recycler!.Recycle(this);

        return IsInRecycler;
    }


    public IBookingInfo Clone() => new BookingInfo(this);
}
