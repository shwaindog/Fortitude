// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Logging;
using FortitudeMarkets.Configuration.PricingConfig;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public class PQMessageDeserializer<T> : PQMessageDeserializerBase<T>, IPQMessagePublishingDeserializer<T> where T : class, IPQMessage
{
    public const int MaxBufferedUpdates = 128;

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQMessageDeserializer<T>));

    private readonly DeserializeStateTransitionFactory<T> stateTransitionFactory;

    private readonly StaticRing<T> syncRing;

    private SyncStateBase<T> currentSyncState;

    public PQMessageDeserializer(ITickerPricingSubscriptionConfig tickerPricingSubscriptionConfig)
        : base(tickerPricingSubscriptionConfig.SourceTickerInfo)
    {
        SyncRetryMs            = tickerPricingSubscriptionConfig.PricingServerConfig.SyncRetryIntervalMs;
        currentSyncState       = new InitializationState<T>(this);
        stateTransitionFactory = new DeserializeStateTransitionFactory<T>();
        AllowUpdatesCatchup    = tickerPricingSubscriptionConfig.PricingServerConfig.AllowUpdatesCatchup;
        syncRing = new StaticRing<T>(MaxBufferedUpdates, () =>
        {
            var newQuote = QuoteFactory(tickerPricingSubscriptionConfig.SourceTickerInfo);
            newQuote.EnsureRelatedItemsAreConfigured(PublishedQuote);
            return newQuote;
        }, true);
    }

    public PQMessageDeserializer(PQMessageDeserializer<T> toClone) : base(toClone)
    {
        SyncRetryMs            = toClone.SyncRetryMs;
        currentSyncState       = new InitializationState<T>(this);
        stateTransitionFactory = new DeserializeStateTransitionFactory<T>();
        AllowUpdatesCatchup    = toClone.AllowUpdatesCatchup;
        syncRing = new StaticRing<T>(MaxBufferedUpdates, () =>
        {
            var newQuote = QuoteFactory(toClone.Identifier);
            newQuote.EnsureRelatedItemsAreConfigured(PublishedQuote);
            return newQuote;
        }, true);
    }

    public bool AllowUpdatesCatchup { get; }

    public override T Deserialize(ISerdeContext readContext)
    {
        if (readContext is IMessageBufferContext bufferContext)
        {
            if (bufferContext is SocketBufferReadContext dispatchContext)
            {
                dispatchContext.DispatchLatencyLogger?.Add(SocketDataLatencyLogger.EnterDeserializer);
                dispatchContext.DeserializerTime = TimeContext.UtcNow;
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

                PublishedQuote.CopyFrom(ent);
                PublishedQuote.ClientReceivedTime = ent.ClientReceivedTime;
                PublishedQuote.InboundProcessedTime      = ent.InboundProcessedTime;
            }

            syncRing.Clear(1);
        }

        SwitchSyncState(QuoteSyncState.InSync);
        misMatchedSeqId = 0;
        return true;
    }

    public override bool HasTimedOutAndNeedsSnapshot(DateTime utcNow) => currentSyncState.HasJustGoneStale(utcNow);

    public override bool CheckResync(DateTime utcNow) => currentSyncState.EligibleForResync(utcNow);

    public override IMessageDeserializer Clone() => new PQMessageDeserializer<T>(this);
}
