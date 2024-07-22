// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

internal class ReplayState<T> : SyncStateBase<T> where T : PQTickInstant, new()
{
    public ReplayState(IPQQuotePublishingDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.Replay) { }
}
