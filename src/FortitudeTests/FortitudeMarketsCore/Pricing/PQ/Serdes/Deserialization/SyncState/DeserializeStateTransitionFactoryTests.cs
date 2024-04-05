#region

using System.ComponentModel;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

[TestClass]
public class DeserializeStateTransitionFactoryTests
{
    private DeserializeStateTransitionFactory<PQLevel0Quote> deserializeStateTransitionFactory = null!;

    private Mock<IPQQuoteDeserializer<PQLevel0Quote>> deserialzer = null!;

    private InitializationState<PQLevel0Quote> initializationState = null!;

    [TestInitialize]
    public void SetUp()
    {
        deserialzer = new Mock<IPQQuoteDeserializer<PQLevel0Quote>>();
        initializationState = new InitializationState<PQLevel0Quote>(deserialzer.Object);
        deserializeStateTransitionFactory = new DeserializeStateTransitionFactory<PQLevel0Quote>();
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
