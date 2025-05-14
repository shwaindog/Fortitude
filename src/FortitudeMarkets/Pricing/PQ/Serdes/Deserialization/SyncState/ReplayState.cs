// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

internal class ReplayState<T> : SyncStateBase<T> where T : IPQMutableMessage
{
    public ReplayState(IPQMessagePublishingDeserializer<T> linkedDeserializer)
        : base(linkedDeserializer, QuoteSyncState.Replay) { }
}
