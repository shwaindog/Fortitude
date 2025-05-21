#region

using FortitudeIO.Transports.Network.Construction;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Network.Construction;

[TestClass]
public class SocketDataLatencyLoggerFactoryTests
{
    [TestMethod]
    public void NewSocketDataLatencyLoggerFactory_Instance_CreatesNewInstance()
    {
        var instance = SocketDataLatencyLoggerFactory.Instance;
        Assert.IsNotNull(instance);
        var instance2 = SocketDataLatencyLoggerFactory.Instance;
        Assert.AreSame(instance, instance2);

        SocketDataLatencyLoggerFactory.ClearSocketDataLatencyLoggerFactory();
        var instance3 = SocketDataLatencyLoggerFactory.Instance;
        Assert.IsNotNull(instance);
        Assert.AreNotSame(instance, instance3);
    }

    [TestMethod]
    public void NewSocketDataLatencyLoggerFactory_GetSocketLatencyLogger_ReturnsSameInstanceForSameKey()
    {
        var expectedKey = "SameKey";

        var socketDataLatencyLogger = SocketDataLatencyLoggerFactory.Instance
            .GetSocketDataLatencyLogger(expectedKey);

        var socketDataLatencyLogger2 = SocketDataLatencyLoggerFactory.Instance
            .GetSocketDataLatencyLogger(expectedKey);
        Assert.AreSame(socketDataLatencyLogger, socketDataLatencyLogger2);

        var diffKey = "DiffKey";
        var diffSocketDataLatencyLogger = SocketDataLatencyLoggerFactory.Instance
            .GetSocketDataLatencyLogger(diffKey);
        Assert.AreNotSame(socketDataLatencyLogger, diffSocketDataLatencyLogger);
    }
}
