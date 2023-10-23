#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Sockets;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public abstract class OrxAuthenticatedClient
{
    protected readonly MutableString DefaultAccount;
    protected readonly ILoginCredentials LoginCredentials;
    protected readonly string ServerName;
    protected readonly IOrxSubscriber Subscriber;
    protected bool IsLoggedIn;
    protected bool IsServiceAvailable;
    protected IFLogger Logger;

    protected OrxAuthenticatedClient(IOrxSubscriber subscriber, string serverName,
        ILoginCredentials loginCredentials, string defaultAccount)
    {
        LoginCredentials = loginCredentials;
        DefaultAccount = defaultAccount;

        ServerName = serverName;
        Subscriber = subscriber;
        Logger = FLoggerFactory.Instance.GetLogger(Subscriber.Logger.Name + ".Executions");


        Subscriber.StreamToPublisher.RegisterSerializer<OrxLogonRequest>();

        Subscriber.RegisterDeserializer<OrxLogonResponse>(HandleLoggedIn);
        Subscriber.RegisterDeserializer<OrxLoggedOutMessage>(HandleLoggedOut);
    }

    public virtual bool IsAvailable => Subscriber.IsConnected && IsServiceAvailable && IsLoggedIn;

    private event Action<string?, bool>? StatusUpdateHandlers;

    protected void NotifyStatusUpdateHandlers(string? key, bool status)
    {
        StatusUpdateHandlers?.Invoke(key, status);
    }

    protected void AppendStatusHandler(Action<string?, bool>? callback)
    {
        StatusUpdateHandlers += callback;
    }

    protected void RemoveStatusHandler(Action<string?, bool>? callback)
    {
        StatusUpdateHandlers -= callback;
    }

    protected void OnConnected()
    {
        Subscriber.Send(new OrxLogonRequest(LoginCredentials, DefaultAccount.ToString(), "0.3", 0));
    }

    protected void HandleLoggedIn(OrxLogonResponse message, object? context, ISession? cx)
    {
        Subscriber.Logger.Info("Logon to " + ServerName + " Adapter accepted");
        IsLoggedIn = true;
        StatusUpdateHandlers?.Invoke(string.Empty, IsAvailable);
    }

    protected void HandleLoggedOut(OrxLoggedOutMessage message, object? context, ISession? cx)
    {
        Subscriber.Logger.Info("Logon to " + ServerName + " Adapter refused: " + message.Reason);
        IsLoggedIn = false;
        StatusUpdateHandlers?.Invoke(string.Empty, IsAvailable);
    }
}
