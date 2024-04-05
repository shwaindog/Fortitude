#region

using FortitudeIO.Transports.Network.Config;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Config;

[TestClass]
public class NetworkTopicConnectionConfigTests
{
    public static NetworkTopicConnectionConfig DummyTopicConnectionConfig =>
        new("TestConnectionName", SocketConversationProtocol.TcpClient
            , new List<IEndpointConfig> { new EndpointConfig("TestHostname", 9090) },
            "TestDescription");

    [TestMethod]
    public void LoadJsonFile_NetworkTopicConnectionConfig_ContainsValuesInFile()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("FortitudeIO/Transports/Network/Config/TestNetworkTopicConnectConfigLoads.json")
            .Build();
        var networkTopicConnectionConfig
            = new NetworkTopicConnectionConfig(config, "NetworkTopicConnectionConfig");

        Assert.AreEqual("TestCanLoadFromJson", networkTopicConnectionConfig.TopicName);
        Assert.AreEqual(SocketConversationProtocol.TcpClient, networkTopicConnectionConfig.ConversationProtocol);
        Assert.AreEqual(2, networkTopicConnectionConfig.AvailableConnections.Count);
        Assert.AreEqual("FirstInstanceName", networkTopicConnectionConfig.AvailableConnections[0].InstanceName);
        Assert.AreEqual("FirstHostname", networkTopicConnectionConfig.AvailableConnections[0].Hostname);
        Assert.AreEqual(1U, networkTopicConnectionConfig.AvailableConnections[0].Port);
        Assert.IsNull(networkTopicConnectionConfig.AvailableConnections[0].SubnetMask);
        Assert.AreEqual("SecondInstanceName", networkTopicConnectionConfig.AvailableConnections[1].InstanceName);
        Assert.AreEqual("SecondHostname", networkTopicConnectionConfig.AvailableConnections[1].Hostname);
        Assert.AreEqual(2U, networkTopicConnectionConfig.AvailableConnections[1].Port);
        Assert.AreEqual("127.0.0.1", networkTopicConnectionConfig.AvailableConnections[1].SubnetMask);
        Assert.AreEqual("This topic tests that the application can load this json config", networkTopicConnectionConfig.TopicDescription);
        Assert.AreEqual(9_000, networkTopicConnectionConfig.ReceiveBufferSize);
        Assert.AreEqual(7_000, networkTopicConnectionConfig.SendBufferSize);
        Assert.AreEqual(SocketConnectionAttributes.Fast | SocketConnectionAttributes.Multicast, networkTopicConnectionConfig.ConnectionAttributes);
        Assert.AreEqual(ConnectionSelectionOrder.ListedOrder, networkTopicConnectionConfig.ConnectionSelectionOrder);
        Assert.AreEqual(1_000U, networkTopicConnectionConfig.ConnectionTimeoutMs);
        Assert.AreEqual(9_000U, networkTopicConnectionConfig.ResponseTimeoutMs);
        Assert.IsNotNull(networkTopicConnectionConfig.ReconnectConfig);
        Assert.AreEqual(5_000U, networkTopicConnectionConfig.ReconnectConfig.StartReconnectIntervalMs);
        Assert.AreEqual(20_000U, networkTopicConnectionConfig.ReconnectConfig.MaxReconnectIntervalMs);
        Assert.AreEqual(1_000U, networkTopicConnectionConfig.ReconnectConfig.IncrementReconnectIntervalMs);

        var newEndPoints = new List<IEndpointConfig>
        {
            new EndpointConfig("MyFirstChangedHostname", 10, "MyFirstChangedInstance", "192.168.1.1")
        };
        networkTopicConnectionConfig.AvailableConnections = newEndPoints;
        Assert.AreEqual(1, networkTopicConnectionConfig.AvailableConnections.Count);
        Assert.AreEqual("MyFirstChangedInstance", networkTopicConnectionConfig.AvailableConnections[0].InstanceName);
        Assert.AreEqual("MyFirstChangedHostname", networkTopicConnectionConfig.AvailableConnections[0].Hostname);
        Assert.AreEqual("192.168.1.1", networkTopicConnectionConfig.AvailableConnections[0].SubnetMask);
    }

    public static NetworkTopicConnectionConfig ServerConnectionConfigWithValues(string connectionName, string hostname,
        ushort port, string networkSubAddress, uint reconnectInterval,
        IObservable<IConnectionUpdate>? updateStream = null) =>
        new(connectionName, SocketConversationProtocol.TcpClient
            , new List<IEndpointConfig>
                { new EndpointConfig(hostname, port, subnetMask: networkSubAddress) },
            connectionName, reconnectConfig: new SocketReconnectConfig(reconnectInterval));

    public static void AssertIsExpected(INetworkTopicConnectionConfig subjectToBeVerified, string name
        , string description
        , string hostname,
        int port, string? networkSubAddress)
    {
        Assert.AreEqual(name, subjectToBeVerified.TopicName);
        Assert.AreEqual(description, subjectToBeVerified.TopicDescription);
        Assert.AreEqual(hostname, subjectToBeVerified.Current.Hostname);
        Assert.AreEqual(port, subjectToBeVerified.Current.Port);
        Assert.AreEqual(networkSubAddress, subjectToBeVerified.Current.SubnetMask);
    }
}
