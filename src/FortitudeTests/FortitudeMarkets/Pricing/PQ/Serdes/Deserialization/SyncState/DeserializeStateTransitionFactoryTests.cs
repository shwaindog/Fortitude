// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.ComponentModel;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Serdes.Deserialization.SyncState;

[TestClass]
public class DeserializeStateTransitionFactoryTests
{
    private DeserializeStateTransitionFactory<PQPublishableTickInstant> deserializeStateTransitionFactory = null!;

    private Mock<IPQQuotePublishingDeserializer<PQPublishableTickInstant>> deserialzer = null!;

    private InitializationState<PQPublishableTickInstant> initializationState = null!;

    [TestInitialize]
    public void SetUp()
    {
        deserialzer         = new Mock<IPQQuotePublishingDeserializer<PQPublishableTickInstant>>();
        initializationState = new InitializationState<PQPublishableTickInstant>(deserialzer.Object);

        deserializeStateTransitionFactory = new DeserializeStateTransitionFactory<PQPublishableTickInstant>();
    }

    [TestMethod]
    public void NewTransitionFactory_TransitionToState_MovesThroughAllPossibleStates()
    {
        var inSyncState = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.InSync, initializationState);
        Assert.AreEqual(QuoteSyncState.InSync, inSyncState.State);
        var synchronising = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.Synchronising, initializationState);
        Assert.AreEqual(QuoteSyncState.Synchronising, synchronising.State);
        var replay = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.Replay, initializationState);
        Assert.AreEqual(QuoteSyncState.Replay, replay.State);
        var timeout = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.Stale, initializationState);
        Assert.AreEqual(QuoteSyncState.Stale, timeout.State);
    }

    [TestMethod]
    public void ExistingRetrievedState_TransitionToState_ReturnsSameInstance()
    {
        var originalInSyncState = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.InSync, initializationState);
        var originalSynchronising = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.Synchronising, initializationState);
        var originalReplay = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.Replay, initializationState);
        var originalTimeout = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.Stale, initializationState);


        var inSyncState = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.InSync, initializationState);
        Assert.AreSame(originalInSyncState, inSyncState);
        var synchronising = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.Synchronising, initializationState);
        Assert.AreSame(originalSynchronising, synchronising);
        var replay = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.Replay, initializationState);
        Assert.AreSame(originalReplay, replay);
        var timeout = deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.Stale, initializationState);
        Assert.AreSame(originalTimeout, timeout);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidEnumArgumentException))]
    public void NewTransitionFactory_TransitionToStateInitialization_ThrowsException()
    {
        deserializeStateTransitionFactory
            .TransitionToState(QuoteSyncState.InitializationState, initializationState);
    }
}
