// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

public class StaleState<T> : InSyncState<T> where T : IPQMessage
{
    public StaleState(IPQMessagePublishingDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.Stale) { }

    protected override void ProcessNextExpectedUpdate(IMessageBufferContext bufferContext, uint sequenceId)
    {
        LinkedDeserializer.UpdateEntity(bufferContext, LinkedDeserializer.PublishedQuote, sequenceId);
        Logger.Info("Stream {0} recovered after timeout, RecvSeqID={1}", LinkedDeserializer.Identifier,
                    sequenceId);
        SwitchState(QuoteSyncState.InSync);
        var sockBuffContext = bufferContext as SocketBufferReadContext;
        PublishQuoteRunAction(FeedSyncStatus.Good, sockBuffContext?.DispatchLatencyLogger,
                              LinkedDeserializer.OnSyncOk);
    }

    public override bool HasJustGoneStale(DateTime utcNow) => false;
}
