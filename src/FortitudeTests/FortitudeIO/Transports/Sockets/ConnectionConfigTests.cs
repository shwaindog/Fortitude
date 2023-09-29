using System;
using System.Reactive.Subjects;
using FortitudeCommon.Configuration;
using FortitudeCommon.EventProcessing;
using FortitudeCommon.Types;
using FortitudeIO.Transports.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeIO.Transports.Sockets
{
    [TestClass]
    public class ConnectionConfigTests
    {
        public static ConnectionConfig DummyConnectionConfig =>
            new ConnectionConfig("TestConnectionName", "TestHostname",
                9090, ConnectionDirectionType.Both, "none", 250,
                null);

        public static ConnectionConfig ServerConnectionConfigWithValues(string connectionName, string hostname,
            int port, ConnectionDirectionType connectionDirectionType, string networkSubAddress, uint reconnectInterval,
            IObservable<IConnectionUpdate> updateStream = null)
        {
            return new ConnectionConfig(connectionName, hostname,
                port, connectionDirectionType, networkSubAddress, reconnectInterval, updateStream);
        }

        public static void AssertIsExpected(IConnectionConfig subjectToBeVerified, string name, string hostname,
            int port, ConnectionDirectionType connectionDirectionType, string networkSubAddress, uint reconnectInterval)
        {
            Assert.AreEqual(name, subjectToBeVerified.ConnectionName);
            Assert.AreEqual(hostname, subjectToBeVerified.Hostname);
            Assert.AreEqual(port, subjectToBeVerified.Port);
            Assert.AreEqual(connectionDirectionType, subjectToBeVerified.ConnectionDirectionType);
            Assert.AreEqual(networkSubAddress, subjectToBeVerified.NetworkSubAddress);
            Assert.AreEqual(reconnectInterval, subjectToBeVerified.ReconnectIntervalMs);
        }

        public static void UpdateServerConnectionConfigWithValues(IConnectionConfig updateThis,
            string connectionName, string hostname,
            int port, ConnectionDirectionType connectionDirectionType, string networkSubAddress, uint reconnectInterval,
            IObservable<IConnectionUpdate> updateStream = null)
        {
            NonPublicInvocator.SetInstanceProperty(updateThis,
                ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.ConnectionName),
                connectionName, true);
            NonPublicInvocator.SetInstanceProperty(updateThis,
                ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.Hostname),
                hostname, true);
            NonPublicInvocator.SetInstanceProperty(updateThis,
                ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.NetworkSubAddress),
                networkSubAddress, true);
            NonPublicInvocator.SetInstanceProperty(updateThis,
                ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.ConnectionDirectionType),
                connectionDirectionType, true);
            NonPublicInvocator.SetInstanceProperty(updateThis,
                ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.ReconnectIntervalMs),
                reconnectInterval, true);
            NonPublicInvocator.SetInstanceProperty(updateThis,
                ReflectionHelper.GetPropertyName((ConnectionConfig x) => x.Port),
                port, true);
        }

        [TestMethod]
        public void ServerConnectionConfig_PublishUpdateStream_UpdatesExistingItem()
        {
            var updateStream = new Subject<IConnectionUpdate>();
            var checkUpdates = ServerConnectionConfigWithValues("OriginalConnectionName", "OriginalHostName", 5678,
                ConnectionDirectionType.Both, "OriginalSubAddress", 678, updateStream);
            AssertIsExpected(checkUpdates, "OriginalConnectionName", "OriginalHostName", 5678,
                ConnectionDirectionType.Both, "OriginalSubAddress", 678);

            var updatedConnectionConfig = checkUpdates.Clone();
            UpdateServerConnectionConfigWithValues(updatedConnectionConfig, "NewConnectionName", "NewStringHostName",
                3456, ConnectionDirectionType.Both, "NewSubAddress", 123);

            updateStream.OnNext(new ConnectionUpdate(updatedConnectionConfig, EventType.Updated));

            AssertIsExpected(checkUpdates, "NewConnectionName", "NewStringHostName", 3456,
                ConnectionDirectionType.Both, "NewSubAddress", 123);
        }
    }
}