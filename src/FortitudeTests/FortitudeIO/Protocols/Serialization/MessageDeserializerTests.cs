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
        var expectedMessageHeader = new MessageHeader(1, 0, 0, 1);

        var moqConversation = new Mock<IConversation>();
        var moqSocketReadBufferContext = new Mock<ISocketBufferReadContext>();

        var firstCallbackCalled = false;
        var secondCallbackCalled = false;

        moqSocketReadBufferContext.SetupGet(srbc => srbc.Conversation).Returns(moqConversation.Object);
        moqSocketReadBufferContext.SetupGet(srbc => srbc.MessageHeader).Returns(expectedMessageHeader);

        void FirstCallback(object data, MessageHeader messageHeader, IConversation session)
        {
            Assert.AreSame(expectedData, data);
            Assert.IsTrue(expectedMessageHeader.Equals(messageHeader));
            Assert.AreSame(moqConversation.Object, session);
            firstCallbackCalled = true;
        }

        Assert.IsFalse(binaryDeserializer.IsRegistered(FirstCallback));

        binaryDeserializer.ConversationMessageDeserialized += FirstCallback;
        Assert.IsTrue(binaryDeserializer.IsRegistered(FirstCallback));

        void SecondCallback(object data, MessageHeader messageHeader, IConversation session)
        {
            Assert.AreSame(expectedData, data);
            Assert.IsTrue(expectedMessageHeader.Equals(messageHeader));
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

        public override IMessageDeserializer Clone() => this;
    }
}
