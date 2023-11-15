#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarketsApi.Trading.Counterparties;

#endregion

namespace FortitudeMarketsCore.Trading.Counterparties;

public class Party : IParty
{
    private int refCount = 0;

    public Party(IParty toClone)
    {
        PartyId = toClone.PartyId;
        Name = toClone.Name;
        ParentParty = toClone.ParentParty;
        ClientPartyId = toClone.ClientPartyId;
        Portfolio = toClone.Portfolio;
    }

    public Party(string partyId, string name, IParty? parentParty, string clientPartyId, IBookingInfo portfolio)
        : this((MutableString)partyId, (MutableString)name, parentParty, (MutableString)clientPartyId, portfolio) { }

    public Party(IMutableString partyId, IMutableString name, IParty? parentParty, IMutableString clientPartyId
        , IBookingInfo portfolio)
    {
        PartyId = partyId;
        Name = name;
        ParentParty = parentParty;
        ClientPartyId = clientPartyId;
        Portfolio = portfolio;
    }

    public IMutableString PartyId { get; set; }
    public IMutableString Name { get; set; }
    public IParty? ParentParty { get; set; }
    public IMutableString ClientPartyId { get; set; }
    public IBookingInfo Portfolio { get; set; }

    public void CopyFrom(IParty source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        PartyId = source.PartyId;
        Name = source.Name;
        ParentParty = source.ParentParty;
        ClientPartyId = source.ClientPartyId;
        Portfolio = source.Portfolio;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        CopyFrom((IParty)source, copyMergeFlags);
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


    public IParty Clone() => new Party(this);
}
