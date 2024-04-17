#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Logging;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public class PQQuoteDeserializer<T> : PQDeserializerBase<T>, IPQQuoteDeserializer<T>
    where T : PQLevel0Quote, new()
{
    public const int MaxBufferedUpdates = 128;
    private readonly DeserializeStateTransitionFactory<T> stateTransitionFactory;
    private readonly StaticRing<T> syncRing;
    private SyncStateBase<T> currentSyncState;

    public PQQuoteDeserializer(ITickerPricingSubscriptionConfig tickerPricingSubscriptionConfig) : base(tickerPricingSubscriptionConfig
        .SourceTickerQuoteInfo)
    {
        SyncRetryMs = tickerPricingSubscriptionConfig.PricingServerConfig.SyncRetryIntervalMs;
        currentSyncState = new InitializationState<T>(this);
        stateTransitionFactory = new DeserializeStateTransitionFactory<T>();
        AllowUpdatesCatchup = tickerPricingSubscriptionConfig.PricingServerConfig.AllowUpdatesCatchup;
        syncRing = new StaticRing<T>(MaxBufferedUpdates, () =>
        {
            var newQuote = QuoteFactory(tickerPricingSubscriptionConfig.SourceTickerQuoteInfo);
            newQuote.EnsureRelatedItemsAreConfigured(PublishedQuote);
            return newQuote;
        }, true);
    }


    public PQQuoteDeserializer(PQQuoteDeserializer<T> toClone) : base(toClone)
    {
        SyncRetryMs = toClone.SyncRetryMs;
        currentSyncState = new InitializationState<T>(this);
        stateTransitionFactory = new DeserializeStateTransitionFactory<T>();
        AllowUpdatesCatchup = toClone.AllowUpdatesCatchup;
        syncRing = new StaticRing<T>(MaxBufferedUpdates, () =>
        {
            var newQuote = QuoteFactory(toClone.Identifier);
            newQuote.EnsureRelatedItemsAreConfigured(PublishedQuote);
            return newQuote;
        }, true);
    }

    public bool AllowUpdatesCatchup { get; }

    public override PQLevel0Quote Deserialize(ISerdeContext readContext)
    {
        if (readContext is IMessageBufferContext bufferContext)
        {
            if (bufferContext is SocketBufferReadContext dispachContext)
            {
                dispachContext.DispatchLatencyLogger?.Add(SocketDataLatencyLogger.EnterDeserializer);
                dispachContext.DeserializerTime = TimeContext.UtcNow;
            }

            currentSyncState.ProcessInState(bufferContext);

            return PublishedQuote;
        }

        throw new ArgumentException("Expected readContext to be of type IBufferContext");
    }

    public void ClearSyncRing()
    {
        syncRing.Clear(syncRing.Capacity);
    }

    public T ClaimSyncSlotEntry() => syncRing.Claim();

    public uint SyncRetryMs { get; }

    public void SwitchSyncState(QuoteSyncState newState)
    {
        currentSyncState = stateTransitionFactory.TransitionToState(newState, currentSyncState);
    }

    public bool Synchronize(out uint misMatchedSeqId)
    {
        foreach (var ent in syncRing)
        {
            if (ent != null && ent.PQSequenceId > PublishedQuote.PQSequenceId)
            {
                if (ent.PQSequenceId != PublishedQuote.PQSequenceId + 1)
                {
                    misMatchedSeqId = ent.PQSequenceId;
                    if (ent.PQSequenceId - PublishedQuote.PQSequenceId <= syncRing.Capacity) return false;
                    syncRing.Clear(1);
                    continue;
                }

                PublishedQuote.CopyFrom((ILevel0Quote)ent);
                PublishedQuote.ClientReceivedTime = ent.ClientReceivedTime;
                PublishedQuote.ProcessedTime = ent.ProcessedTime;
            }

            syncRing.Clear(1);
        }

        SwitchSyncState(QuoteSyncState.InSync);
        misMatchedSeqId = 0;
        return true;
    }

    public override bool HasTimedOutAndNeedsSnapshot(DateTime utcNow) => currentSyncState.HasJustGoneStale(utcNow);

    public override bool CheckResync(DateTime utcNow) => currentSyncState.EligibleForResync(utcNow);

    public override IMessageDeserializer Clone() => new PQQuoteDeserializer<T>(this);
}
