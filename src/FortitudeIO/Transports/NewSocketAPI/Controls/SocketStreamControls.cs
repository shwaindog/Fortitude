#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Conversations;
using FortitudeIO.Transports.NewSocketAPI.State;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Controls;

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
    protected ISocketSessionContext SocketSocketSessionContext;

    protected SocketStreamControls(ISocketSessionContext socketSocketSessionContext) =>
        SocketSocketSessionContext = socketSocketSessionContext;

    public abstract void Connect();


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
