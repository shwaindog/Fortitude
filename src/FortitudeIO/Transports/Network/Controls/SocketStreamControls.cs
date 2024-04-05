#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Transports.Network.Controls;

public interface IStreamControls : IConversationInitiator
{
    void Connect();
    void Disconnect();
    void StartMessaging();
    void StopMessaging();
}

public abstract class SocketStreamControls : IStreamControls
{
    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(SocketStreamControls));
    protected ISocketSessionContext SocketSessionContext;

    protected SocketStreamControls(ISocketSessionContext socketSessionContext) => SocketSessionContext = socketSessionContext;

    public abstract void Connect();

    public abstract void OnSessionFailure(string reason);

    public void Start()
    {
        Connect();
    }

    public void Stop()
    {
        Disconnect();
    }

    public virtual void StartMessaging()
    {
        if (SocketSessionContext.SocketReceiver != null)
            SocketSessionContext.SocketDispatcher.Listener.RegisterForListen(SocketSessionContext.SocketReceiver);
        SocketSessionContext.SocketDispatcher.Start();
    }

    public virtual void StopMessaging()
    {
        SocketSessionContext.SocketDispatcher.Stop();
    }

    public virtual void Disconnect()
    {
        StopMessaging();
        Logger.Info("Server {0} @{0} stopped", SocketSessionContext.Name,
            SocketSessionContext.SocketConnection!.ConnectedPort);
        SocketSessionContext.OnDisconnected();
    }
}
