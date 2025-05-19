using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

public interface IPQRecentlyTradedHistory : IMutableRecentlyTradedHistory, IPQSupportsNumberPrecisionFieldUpdates<IRecentlyTradedHistory>
  , IPQSupportsStringUpdates<IRecentlyTradedHistory>
{

}


public class PQRecentlyTradedHistory : ReusableObject<IRecentlyTradedHistory>, IPQRecentlyTradedHistory
{
    public override IRecentlyTradedHistory        Clone() => throw new NotImplementedException();

    public override IRecentlyTradedHistory        CopyFrom(IRecentlyTradedHistory source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default) => throw new NotImplementedException();

    public bool                          AreEquivalent(IRecentlyTradedHistory? other, bool exactTypes = false) => throw new NotImplementedException();

    public IMutableRecentlyTradedHistory ResetWithTracking() => throw new NotImplementedException();

    public bool IsEmpty
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public uint UpdateCount => throw new NotImplementedException();

    public void UpdateComplete()
    {
        throw new NotImplementedException();
    }

    public bool HasUpdates
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }
    public int  UpdateField(PQFieldUpdate fieldUpdate) => throw new NotImplementedException();

    public IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
        (DateTime snapShotTime, StorageFlags messageFlags, IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null) =>
        throw new NotImplementedException();

    public bool UpdateFieldString(PQFieldStringUpdate stringUpdate) => throw new NotImplementedException();

    public IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, StorageFlags messageFlags) => throw new NotImplementedException();
}