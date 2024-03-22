#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Authentication;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public abstract class OrxAuthenticatedClient
{
    protected readonly MutableString DefaultAccount;
    protected readonly ILoginCredentials LoginCredentials;
    protected readonly IOrxMessageRequester MessageRequester;
    protected readonly string ServerName;
    protected bool IsLoggedIn;
    protected bool IsServiceAvailable;
    protected IFLogger Logger;

    protected OrxAuthenticatedClient(IOrxMessageRequester messageRequester, string serverName,
        ILoginCredentials loginCredentials, string defaultAccount)
    {
        LoginCredentials = loginCredentials;
        DefaultAccount = defaultAccount;

        ServerName = serverName;
        MessageRequester = messageRequester;
        Logger = FLoggerFactory.Instance.GetLogger(messageRequester.Name + ".Executions");


        MessageRequester.SerializationRepository.RegisterSerializer<OrxLogonRequest>();

        MessageRequester.DeserializationRepository.RegisterDeserializer<OrxLogonResponse>(HandleLoggedIn);
        MessageRequester.DeserializationRepository.RegisterDeserializer<OrxLoggedOutMessage>(HandleLoggedOut);
    }

    public virtual bool IsAvailable => MessageRequester.IsStarted && IsServiceAvailable && IsLoggedIn;

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
        MessageRequester.Send(new OrxLogonRequest(LoginCredentials, DefaultAccount.ToString(), "0.3", 0));
    }

    protected void HandleLoggedIn(OrxLogonResponse message, object? context, IConversation? cx)
    {
        Logger.Info("Logon to " + ServerName + " Adapter accepted");
        IsLoggedIn = true;
        StatusUpdateHandlers?.Invoke(string.Empty, IsAvailable);
    }

    protected void HandleLoggedOut(OrxLoggedOutMessage message, object? context, IConversation? cx)
    {
        Logger.Info("Logon to " + ServerName + " Adapter refused: " + message.Reason);
        IsLoggedIn = false;
        StatusUpdateHandlers?.Invoke(string.Empty, IsAvailable);
    }
}
