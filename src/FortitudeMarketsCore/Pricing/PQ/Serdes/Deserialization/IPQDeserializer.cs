#region

using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion


namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public interface IPQDeserializer : INotifyingMessageDeserializer,
    IDoublyLinkedListNode<IPQDeserializer>
{
    ISourceTickerQuoteInfo Identifier { get; }
    event Action<IPQDeserializer> ReceivedUpdate;
    event Action<IPQDeserializer> SyncOk;
    event Action<IPQDeserializer> OutOfSync;
    void OnReceivedUpdate(IPQDeserializer quoteDeserializer);
    void OnSyncOk(IPQDeserializer quoteDeserializer);
    void OnOutOfSync(IPQDeserializer quoteDeserializer);
    bool HasTimedOutAndNeedsSnapshot(DateTime utcNow);
    bool CheckResync(DateTime utcNow);
}

public interface IPQDeserializer<T> : IPQDeserializer, INotifyingMessageDeserializer<T>, IObservable<T> where T : class, IPQLevel0Quote
{
    T PublishedQuote { get; }
}
