#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols.ORX.Serdes;
using FortitudeMarketsApi.Trading.Counterparties;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.CounterParties;

public class OrxParties : ReusableObject<IParties>, IParties
{
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
    public bool AutoRecycledByProducer { get; set; }

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

    public override IParties Clone() => Recycler?.Borrow<OrxParties>().CopyFrom(this) ?? new OrxParties(this);

    public override IParties CopyFrom(IParties parties, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BuySide = parties.BuySide.SyncOrRecycle(BuySide);
        SellSide = parties.SellSide.SyncOrRecycle(SellSide);
        return this;
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
