#region

using FortitudeIO.Transports.Sockets;

#endregion

namespace FortitudeTests.FortitudeIO.Transports.Sockets;

[TestClass]
public class SocketsConfigurationContextTests
{
    [TestMethod]
    public void NewSocketsConfigurationContext_Instance_CreatesNewInstance()
    {
        var instance = SocketsConfigurationContext.Instance;
        Assert.IsNotNull(instance);
        var instance2 = SocketsConfigurationContext.Instance;
        Assert.AreSame(instance, instance2);

        SocketsConfigurationContext.ClearPreviousConfig();
        var instance3 = SocketsConfigurationContext.Instance;
        Assert.IsNotNull(instance);
        Assert.AreNotSame(instance, instance3);

        Assert.AreEqual("FortitudeMarketsCore.Comms.Sockets", instance["LoggerName"]);
    }
}
