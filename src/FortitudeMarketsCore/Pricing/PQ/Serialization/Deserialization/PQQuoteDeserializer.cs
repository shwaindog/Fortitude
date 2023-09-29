using System;
using FortitudeCommon.Chronometry;
using FortitudeCommon.EventProcessing.Disruption.Rings;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.Logging;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization
{
    public class PQQuoteDeserializer<T> : PQDeserializerBase<T>, IPQQuoteDeserializer<T> 
        where T : class, IPQLevel0Quote
    {
        public const int MaxBufferedUpdates = 128;
        private readonly StaticRing<T> syncRing;
        private SyncStateBase<T> currentSyncState;
        private readonly IDeserializeStateTransitionFactory<T> stateTransitionFactory;


        public PQQuoteDeserializer(ISourceTickerClientAndPublicationConfig identifier) : base(identifier)
        {
            SyncRetryMs = identifier.SyncRetryIntervalMs;
            currentSyncState = new InitializationState<T>(this);
            stateTransitionFactory = new DeserializeStateTransitionFactory<T>();
            AllowUpdatesCatchup = identifier.AllowUpdatesCatchup;
            syncRing = new StaticRing<T>(MaxBufferedUpdates, () =>
            {
                var newQuote = QuoteFactory(identifier);
                newQuote.EnsureRelatedItemsAreConfigured(PublishedQuote);
                return newQuote;
            }, true);
        }

        public bool AllowUpdatesCatchup { get; }

        public override object Deserialize(DispatchContext dispatchContext)
        {
            dispatchContext.DispatchLatencyLogger.Add(SocketDataLatencyLogger.EnterDeserializer);
            dispatchContext.DeserializerTimestamp = TimeContext.UtcNow;

            currentSyncState.ProcessInState(dispatchContext);

            return PublishedQuote;
        }
        
        public void ClearSyncRing()
        {
            syncRing.Clear(syncRing.Capacity);
        }
        public T ClaimSyncSlotEntry()
        {
            return syncRing.Claim();
        }

        public uint SyncRetryMs { get; }

        public void SwitchSyncState(QuoteSyncState newState)
        {
            currentSyncState = stateTransitionFactory.TransitionToState(newState, currentSyncState);
        }

        public bool Synchronize(out uint misMatchedSeqId)
        {
            foreach (var ent in syncRing)
            {
                if (ent.PQSequenceId > PublishedQuote.PQSequenceId)
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
                    PublishedQuote.ProcessedTime = ent.ProcessedTime;
                }
                syncRing.Clear(1);
            }
            SwitchSyncState(QuoteSyncState.InSync);
            misMatchedSeqId = 0;
            return true;
        }

        public override bool HasTimedOutAndNeedsSnapshot(DateTime utcNow)
        {
            return currentSyncState.HasJustGoneStale(utcNow);
        }

        public override bool CheckResync(DateTime utcNow)
        {
            return currentSyncState.EligibleForResync(utcNow);
        }
    }
}