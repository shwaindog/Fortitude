#region

using System.Net;
using FortitudeCommon.Types;
using FortitudeIO.Transports.NewSocketAPI.Config;
using FortitudeIO.Transports.NewSocketAPI.Sockets;
using FortitudeIO.Transports.Sockets;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets;

[TestClass]
public class ConnectionConfigTests
{
    public static SocketConnectionConfig DummyConnectionConfig =>
        new("TestConnectionName", "TestConnectionName", SocketConnectionAttributes.None, 2_000_000, 2_000_000
            , "TestHostname",
            null, false, 9090, 9090);

    public static SocketConnectionConfig ServerConnectionConfigWithValues(string connectionName, string hostname,
        ushort port, ConnectionDirectionType connectionDirectionType, string networkSubAddress, uint reconnectInterval,
        IObservable<IConnectionUpdate>? updateStream = null) =>
        new(connectionName, connectionName, SocketConnectionAttributes.None, 2_000_000, 2_000_000, hostname
            , IPAddress.Parse(networkSubAddress), false, port);

    public static void AssertIsExpected(IConnectionConfig subjectToBeVerified, string name, string hostname,
        int port, ConnectionDirectionType connectionDirectionType, string? networkSubAddress, uint reconnectInterval)
    {
        Assert.AreEqual(name, subjectToBeVerified.ConnectionName);
        Assert.AreEqual(hostname, subjectToBeVerified.Hostname);
        Assert.AreEqual(port, subjectToBeVerified.Port);
        Assert.AreEqual(connectionDirectionType, subjectToBeVerified.ConnectionDirectionType);
        Assert.AreEqual(networkSubAddress, subjectToBeVerified.NetworkSubAddress);
        Assert.AreEqual(reconnectInterval, subjectToBeVerified.ReconnectIntervalMs);
    }

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

    public static void UpdateServerConnectionConfigWithValues(IConnectionConfig updateThis,
        string connectionName, string hostname,
        int port, ConnectionDirectionType connectionDirectionType, string networkSubAddress, uint reconnectInterval,
        IObservable<IConnectionUpdate>? updateStream = null)
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
}
