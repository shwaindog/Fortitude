#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Trading.Counterparties;

#endregion

namespace FortitudeMarkets.Trading.Counterparties;

public class Parties : ReusableObject<IParties>, IParties
{
    public Parties() { }

    public Parties(IParties toClone)
    {
        BuySide = toClone.BuySide?.Clone();
        SellSide = toClone.SellSide?.Clone();
    }

    public Parties(IParty? buySide, IParty? sellSide)
    {
        BuySide = buySide;
        SellSide = sellSide;
    }

    public IParty? BuySide { get; set; }
    public IParty? SellSide { get; set; }


    public override IParties Clone() => Recycler?.Borrow<Parties>().CopyFrom(this) ?? new Parties(this);

    public override IParties CopyFrom(IParties source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BuySide = source.BuySide?.CopyOrClone(BuySide as Party);
        SellSide = source.SellSide?.CopyOrClone(SellSide as Party);
        return this;
    }
}
