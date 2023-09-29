using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Sockets;
using FortitudeIO.Transports.Sockets.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeIO.Protocols.Serialization
{
    [TestClass]
    public class BinaryDeserializerTests
    {
        [TestMethod]
        public void RegisteredDeserializerWithCallbacks_Dispatch_LogsBeforePublishThenCallsRegisteredCallbacks()
        {
            var binaryDeserializer = new DummyBinaryDeserializer<object>();
            
            object expectedData = new object();
            object expectedState = new object();

            var moqSession = new Mock<ISession>();
            var moqPerfLogger = new Mock<IPerfLogger>();

            bool firstCallbackCalled = false;
            bool secondCallbackCalled = false;

            void FirstCallback(object data, object state, ISession session)
            {
                Assert.AreSame(expectedData, data);
                Assert.AreSame(expectedState, state);
                Assert.AreSame(moqSession.Object, session);
                firstCallbackCalled = true;
            }
            Assert.IsFalse(binaryDeserializer.IsRegistered(FirstCallback));

            binaryDeserializer.Deserialized += FirstCallback;
            Assert.IsTrue(binaryDeserializer.IsRegistered(FirstCallback));

            void SecondCallback(object data, object state, ISession session)
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

        internal class DummyBinaryDeserializer<Tm> : BinaryDeserializer<Tm> where Tm : class
        {
            public override object Deserialize(DispatchContext dispatchContext)
            {
                return null;
            }

            public void CallDispatch(Tm data, object state, ISession repositorySession,
                IPerfLogger detectionToPublishLatencyLogger)
            {
                Dispatch(data, state, repositorySession, detectionToPublishLatencyLogger);
            }
        }
    }
}