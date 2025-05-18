// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.ComponentModel;
using FortitudeMarkets.Pricing.PQ.Messages;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

public interface IDeserializeStateTransitionFactory<T> where T : IPQMessage
{
    SyncStateBase<T> TransitionToState(QuoteSyncState desiredState, SyncStateBase<T> currentState);
}

internal class DeserializeStateTransitionFactory<T> : IDeserializeStateTransitionFactory<T>
    where T : IPQMessage
{
    private SyncStateBase<T>? inSyncState;
    private SyncStateBase<T>? replay;

    private SynchronisingState<T>? synchronisingState;

    private SyncStateBase<T>? timedOut;

    public virtual SyncStateBase<T> TransitionToState(QuoteSyncState desiredState, SyncStateBase<T> currentState)
    {
        switch (desiredState)
        {
            case QuoteSyncState.InSync:        return GetInSyncState(currentState);
            case QuoteSyncState.Synchronising: return GetSynchronisingState(currentState);
            case QuoteSyncState.Replay:        return GetReplayState(currentState);
            case QuoteSyncState.Stale:         return GetStaleState(currentState);

            case QuoteSyncState.InitializationState: throw new InvalidEnumArgumentException("Should never request to change to intialization state");

            default: return GetSynchronisingState(currentState);
        }
    }

    public virtual SyncStateBase<T> GetReplayState(SyncStateBase<T> currentState)
    {
        if (replay != null) return replay;
        replay = new ReplayState<T>(currentState.LinkedDeserializer);
        return replay;
    }

    public virtual SyncStateBase<T> GetStaleState(SyncStateBase<T> currentState)
    {
        if (timedOut != null) return timedOut;
        timedOut = new StaleState<T>(currentState.LinkedDeserializer);
        return timedOut;
    }

    public virtual SyncStateBase<T> GetSynchronisingState(SyncStateBase<T> currentState)
    {
        if (synchronisingState != null) return synchronisingState;
        synchronisingState = new SynchronisingState<T>(currentState.LinkedDeserializer);
        return synchronisingState;
    }

    public virtual SyncStateBase<T> GetInSyncState(SyncStateBase<T> currentState)
    {
        if (inSyncState != null) return inSyncState;
        inSyncState = new InSyncState<T>(currentState.LinkedDeserializer);
        return inSyncState;
    }
}
