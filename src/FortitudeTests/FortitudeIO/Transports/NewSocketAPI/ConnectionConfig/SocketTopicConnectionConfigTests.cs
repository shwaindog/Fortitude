#region

using FortitudeIO.Transports.NewSocketAPI;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.NewSocketAPI.ConnectionConfig;

[TestClass]
public class SocketTopicConnectionConfigTests
{
    public static SocketTopicConnectionConfig DummyTopicConnectionConfig =>
        new("TestConnectionName", SocketConversationProtocol.TcpClient
            , new List<ISocketConnectionConfig> { new SocketConnectionConfig("TestHostname", 9090) },
            "TestDescription");

    public static SocketTopicConnectionConfig ServerConnectionConfigWithValues(string connectionName, string hostname,
        ushort port, string networkSubAddress, uint reconnectInterval,
        IObservable<IConnectionUpdate>? updateStream = null) =>
        new(connectionName, SocketConversationProtocol.TcpClient
            , new List<ISocketConnectionConfig>
                { new SocketConnectionConfig(hostname, port, subnetMask: networkSubAddress) },
            connectionName, reconnectConfig: new SocketReconnectConfig(reconnectInterval));

    public static void AssertIsExpected(ISocketTopicConnectionConfig subjectToBeVerified, string name
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
