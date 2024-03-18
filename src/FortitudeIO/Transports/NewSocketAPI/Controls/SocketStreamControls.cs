#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Transports.NewSocketAPI.Sockets;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Controls;

public interface IStreamControls
{
    void Connect();
    void Disconnect();
    void StartMessaging();
    void StopMessaging();
}

public abstract class SocketStreamControls : IStreamControls
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SocketStreamControls));
    protected ISocketSessionContext SocketSocketSessionContext;

    protected SocketStreamControls(ISocketSessionContext socketSocketSessionContext) =>
        SocketSocketSessionContext = socketSocketSessionContext;

    public abstract void Connect();


    public virtual void StartMessaging()
    {
        SocketSocketSessionContext.SocketDispatcher.Start();
    }

    public virtual void StopMessaging()
    {
        SocketSocketSessionContext.SocketDispatcher.Stop();
    }

    public virtual void Disconnect()
    {
        StopMessaging();
        Logger.Info("Server {0} @{0} stopped", SocketSocketSessionContext.Name,
            SocketSocketSessionContext.SocketConnection!.ConnectedPort);
        SocketSocketSessionContext.OnDisconnected();
    }
}
