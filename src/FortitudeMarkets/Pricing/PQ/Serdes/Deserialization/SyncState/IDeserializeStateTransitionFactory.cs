// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

public interface IDeserializeStateTransitionFactory<T> where T : PQTickInstant, new()
{
    SyncStateBase<T> TransitionToState(QuoteSyncState desiredState, SyncStateBase<T> currentState);
}