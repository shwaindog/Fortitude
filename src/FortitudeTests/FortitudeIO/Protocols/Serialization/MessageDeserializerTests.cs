#region

using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
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

        var moqConversation = new Mock<IConversation>();
        var moqSocketReadBufferContext = new Mock<ISocketBufferReadContext>();

        var firstCallbackCalled = false;
        var secondCallbackCalled = false;

        moqSocketReadBufferContext.SetupGet(srbc => srbc.Conversation).Returns(moqConversation.Object);
        moqSocketReadBufferContext.SetupGet(srbc => srbc.MessageHeader).Returns(expectedState);

        void FirstCallback(object data, object? state, IConversation? session)
        {
            Assert.AreSame(expectedData, data);
            Assert.AreSame(expectedState, state);
            Assert.AreSame(moqConversation.Object, session);
            firstCallbackCalled = true;
        }

        Assert.IsFalse(binaryDeserializer.IsRegistered(FirstCallback));

        binaryDeserializer.ConversationMessageDeserialized += FirstCallback;
        Assert.IsTrue(binaryDeserializer.IsRegistered(FirstCallback));

        void SecondCallback(object data, object? state, IConversation? session)
        {
            Assert.AreSame(expectedData, data);
            Assert.AreSame(expectedState, state);
            Assert.AreSame(moqConversation.Object, session);
            secondCallbackCalled = true;
        }

        Assert.IsFalse(binaryDeserializer.IsRegistered(SecondCallback));

        binaryDeserializer.ConversationMessageDeserialized += SecondCallback;
        Assert.IsTrue(binaryDeserializer.IsRegistered(SecondCallback));

        binaryDeserializer.CallDispatch(expectedData, moqSocketReadBufferContext.Object);

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

        public void CallDispatch(TM data, IBufferContext bufferContext)
        {
            OnNotify(data, bufferContext);
        }
    }
}
