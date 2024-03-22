#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports;
using FortitudeIO.Transports.NewSocketAPI.Logging;
using Moq;

#endregion

namespace FortitudeTests.FortitudeIO.Protocols.Serialization;

[TestClass]
public class MessageDeserializerTests
{
    [TestMethod]
    public void RegisteredDeserializerWithCallbacks_Dispatch_LogsBeforePublishThenCallsRegisteredCallbacks()
    {
        var binaryDeserializer = new DummyMessageDeserializer<DummyMessage>();

        var expectedData = new DummyMessage();
        var expectedState = new DummyMessage();

        var moqSession = new Mock<ISession>();
        var moqPerfLogger = new Mock<IPerfLogger>();

        var firstCallbackCalled = false;
        var secondCallbackCalled = false;

        void FirstCallback(object data, object? state, ISession? session)
        {
            Assert.AreSame(expectedData, data);
            Assert.AreSame(expectedState, state);
            Assert.AreSame(moqSession.Object, session);
            firstCallbackCalled = true;
        }

        Assert.IsFalse(binaryDeserializer.IsRegistered(FirstCallback));

        binaryDeserializer.Deserialized += FirstCallback;
        Assert.IsTrue(binaryDeserializer.IsRegistered(FirstCallback));

        void SecondCallback(object data, object? state, ISession? session)
        {
            Assert.AreSame(expectedData, data);
            Assert.AreSame(expectedState, state);
            Assert.AreSame(moqSession.Object, session);
            secondCallbackCalled = true;
        }

        Assert.IsFalse(binaryDeserializer.IsRegistered(SecondCallback));

        binaryDeserializer.Deserialized += SecondCallback;
        Assert.IsTrue(binaryDeserializer.IsRegistered(SecondCallback));

        moqPerfLogger.Setup(pl => pl.Add(SocketDataLatencyLogger.BeforePublish)).Verifiable();

        binaryDeserializer.CallDispatch(expectedData, expectedState, moqSession.Object, moqPerfLogger.Object);

        moqPerfLogger.Verify();
        Assert.IsTrue(firstCallbackCalled);
        Assert.IsTrue(secondCallbackCalled);
    }

    private class DummyMessage : VersionedMessage
    {
        public override uint MessageId => 88888;
        public override IVersionedMessage Clone() => this;
    }

    internal class DummyMessageDeserializer<TM> : MessageDeserializer<TM> where TM : class, IVersionedMessage, new()
    {
        public override TM? Deserialize(ISerdeContext serdeContext) => null;

        public void CallDispatch(TM data, object state, ISession repositorySession,
            IPerfLogger detectionToPublishLatencyLogger)
        {
            Dispatch(data, state, repositorySession, detectionToPublishLatencyLogger);
        }
    }
}
