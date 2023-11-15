#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Trading.Counterparties;

#endregion

namespace FortitudeMarketsCore.Trading.Counterparties;

public class Parties : IParties
{
    private int refCount = 0;

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

    public void CopyFrom(IParties source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        BuySide = source.BuySide;
        SellSide = source.SellSide;
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


    public IParties Clone() => new Parties(this);
}
