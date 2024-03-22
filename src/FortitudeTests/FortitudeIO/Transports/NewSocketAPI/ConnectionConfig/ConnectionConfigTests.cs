#region

using System.Net;
using FortitudeIO.Transports.NewSocketAPI;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.NewSocketAPI.ConnectionConfig;

[TestClass]
public class ConnectionConfigTests
{
    public static SocketConnectionConfig DummyConnectionConfig =>
        new("TestConnectionName", "TestConnectionName", SocketConnectionAttributes.None, 2_000_000, 2_000_000
            , "TestHostname",
            null, false, 9090, 9090);

    public static SocketConnectionConfig ServerConnectionConfigWithValues(string connectionName, string hostname,
        ushort port, string networkSubAddress, uint reconnectInterval,
        IObservable<IConnectionUpdate>? updateStream = null) =>
        new(connectionName, connectionName, SocketConnectionAttributes.None, 2_000_000, 2_000_000, hostname
            , IPAddress.Parse(networkSubAddress), false, port);

    public static void AssertIsExpected(ISocketConnectionConfig subjectToBeVerified, string name, string description
        , string hostname,
        int port, string? networkSubAddress)
    {
        Assert.AreEqual(name, subjectToBeVerified.InstanceName);
        Assert.AreEqual(description, subjectToBeVerified.SocketDescription);
        Assert.AreEqual(hostname, subjectToBeVerified.Hostname!.ToString());
        Assert.AreEqual(port, subjectToBeVerified.PortStartRange);
        Assert.AreEqual(networkSubAddress, subjectToBeVerified.SubnetMask?.ToString());
    }
}
