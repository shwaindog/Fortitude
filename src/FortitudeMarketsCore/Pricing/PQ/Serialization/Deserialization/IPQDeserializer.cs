#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;

public interface IPQDeserializer : ICallbackBinaryDeserializer<PQLevel0Quote>,
    IDoublyLinkedListNode<IPQDeserializer>
{
    IUniqueSourceTickerIdentifier Identifier { get; }
    event Action<IPQDeserializer> ReceivedUpdate;
    event Action<IPQDeserializer> SyncOk;
    event Action<IPQDeserializer> OutOfSync;
    void OnReceivedUpdate(IPQDeserializer quoteDeserializer);
    void OnSyncOk(IPQDeserializer quoteDeserializer);
    void OnOutOfSync(IPQDeserializer quoteDeserializer);
    bool HasTimedOutAndNeedsSnapshot(DateTime utcNow);
    bool CheckResync(DateTime utcNow);
}

public interface IPQDeserializer<out T> : IPQDeserializer, IObservable<T> where T : IPQLevel0Quote
{
    T PublishedQuote { get; }
}
