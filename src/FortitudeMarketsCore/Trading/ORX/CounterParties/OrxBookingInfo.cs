#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Counterparties;
using FortitudeMarketsCore.Trading.Counterparties;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.CounterParties;

public class OrxBookingInfo : IBookingInfo
{
    private int refCount = 0;

    public OrxBookingInfo() { }

    public OrxBookingInfo(IBookingInfo toClone)
    {
        Portfolio = toClone.Portfolio != null ? new MutableString(toClone.Portfolio) : null;
        SubPortfolio = toClone.SubPortfolio != null ? new MutableString(toClone.SubPortfolio) : null;
    }

    public OrxBookingInfo(string portfolio, string subPortfolio)
        : this((MutableString)portfolio, (MutableString)subPortfolio) { }

    public OrxBookingInfo(IMutableString portfolio, IMutableString subPortfolio)
    {
        Portfolio = (MutableString)portfolio;
        SubPortfolio = (MutableString)subPortfolio;
    }

    [OrxMandatoryField(0)] public MutableString? Portfolio { get; set; }

    [OrxOptionalField(1)] public MutableString? SubPortfolio { get; set; }

    IMutableString? IBookingInfo.Portfolio
    {
        get => Portfolio;
        set => Portfolio = value as MutableString;
    }

    IMutableString? IBookingInfo.SubPortfolio
    {
        get => SubPortfolio;
        set => SubPortfolio = value as MutableString;
    }

    public IBookingInfo Clone() => new BookingInfo(this);

    public void CopyFrom(IBookingInfo bookingInfo, CopyMergeFlags copyMergeFlags)
    {
        Portfolio = bookingInfo.Portfolio != null ?
            Recycler!.Borrow<MutableString>().Clear().Append(bookingInfo.Portfolio) :
            null;
        SubPortfolio = bookingInfo.SubPortfolio != null ?
            Recycler!.Borrow<MutableString>().Clear().Append(bookingInfo.SubPortfolio) :
            null;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IBookingInfo)source, copyMergeFlags);
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
        return true;
    }

    protected bool Equals(OrxBookingInfo other)
    {
        var portfolioSame = Equals(Portfolio, other.Portfolio);
        var subPortfolioSame = Equals(SubPortfolio, other.SubPortfolio);

        return portfolioSame && subPortfolioSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxBookingInfo)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Portfolio != null ? Portfolio.GetHashCode() : 0) * 397) ^
                   (SubPortfolio != null ? SubPortfolio.GetHashCode() : 0);
        }
    }
}
