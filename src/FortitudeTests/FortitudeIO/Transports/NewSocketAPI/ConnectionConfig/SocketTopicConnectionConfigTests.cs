#region

using FortitudeIO.Transports.NewSocketAPI.Config;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.NewSocketAPI.ConnectionConfig;

[TestClass]
public class NetworkTopicConnectionConfigTests
{
    public static NetworkTopicConnectionConfig DummyTopicConnectionConfig =>
        new("TestConnectionName", SocketConversationProtocol.TcpClient
            , new List<IEndpointConfig> { new EndpointConfig("TestHostname", 9090) },
            "TestDescription");

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
