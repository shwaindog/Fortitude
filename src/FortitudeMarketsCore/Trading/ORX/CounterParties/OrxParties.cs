#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeMarketsApi.Trading.Counterparties;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.CounterParties;

public class OrxParties : IParties
{
    private int refCount = 0;

    public OrxParties() { }

    public OrxParties(IParties toClone)
    {
        if (toClone is OrxParties orxParties)
        {
            BuySide = orxParties.BuySide?.Clone() as OrxParty;
            SellSide = orxParties.SellSide?.Clone() as OrxParty;
        }
        else
        {
            BuySide = toClone.BuySide != null ? new OrxParty(toClone.BuySide) : null;
            SellSide = toClone.SellSide != null ? new OrxParty(toClone.SellSide) : null;
        }
    }

    public OrxParties(OrxParty buySide, OrxParty sellSide)
    {
        BuySide = buySide;
        SellSide = sellSide;
    }

    [OrxOptionalField(0)] public OrxParty? BuySide { get; set; }

    [OrxOptionalField(1)] public OrxParty? SellSide { get; set; }

    IParty? IParties.BuySide
    {
        get => BuySide;
        set => BuySide = value as OrxParty;
    }

    IParty? IParties.SellSide
    {
        get => SellSide;
        set => SellSide = value as OrxParty;
    }

    public IParties Clone() => new OrxParties(this);

    public void CopyFrom(IParties parties, CopyMergeFlags copyMergeFlags)
    {
        if (parties.BuySide != null)
        {
            var orxBuySideParty = Recycler!.Borrow<OrxParty>();
            orxBuySideParty.CopyFrom(parties.BuySide, copyMergeFlags);
            BuySide = orxBuySideParty;
        }

        if (parties.SellSide != null)
        {
            var orxSellSideParty = Recycler!.Borrow<OrxParty>();
            orxSellSideParty.CopyFrom(parties.SellSide, copyMergeFlags);
            SellSide = orxSellSideParty;
        }
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IParties)source, copyMergeFlags);
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

    protected bool Equals(OrxParties other)
    {
        var buySideSame = Equals(BuySide, other.BuySide);
        var sellSideSame = Equals(SellSide, other.SellSide);
        return buySideSame && sellSideSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((OrxParties)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((BuySide != null ? BuySide.GetHashCode() : 0) * 397) ^
                   (SellSide != null ? SellSide.GetHashCode() : 0);
        }
    }
}
