#region

using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.Serdes.Binary;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public abstract class OrxAuthenticatedClient
{
    protected readonly IOrxClientRequester ClientRequester;
    protected readonly uint DefaultAccount;
    protected readonly ILoginCredentials LoginCredentials;
    protected readonly string ServerName;
    protected bool IsLoggedIn;
    protected bool IsServiceAvailable;
    protected IFLogger Logger;

    protected OrxAuthenticatedClient(IOrxClientRequester clientRequester, string serverName,
        ILoginCredentials loginCredentials, uint defaultAccount)
    {
        LoginCredentials = loginCredentials;
        DefaultAccount = defaultAccount;

        ServerName = serverName;
        ClientRequester = clientRequester;
        Logger = FLoggerFactory.Instance.GetLogger(clientRequester.Name + ".Executions");


        ClientRequester.SerializationRepository.RegisterSerializer<OrxLogonRequest>();

        ClientRequester.Connected += () =>
        {
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxLogonResponse>()
                .AddDeserializedNotifier(
                    new PassThroughDeserializedNotifier<OrxLogonResponse>($"{nameof(OrxAuthenticatedClient)}.{nameof(HandleLoggedIn)}"
                        , HandleLoggedIn));
            ClientRequester.DeserializationRepository.RegisterDeserializer<OrxLoggedOutMessage>()
                .AddDeserializedNotifier(
                    new PassThroughDeserializedNotifier<OrxLoggedOutMessage>($"{nameof(OrxAuthenticatedClient)}.{nameof(HandleLoggedOut)}"
                        , HandleLoggedOut));
        };
    }

    public virtual bool IsAvailable => ClientRequester.IsStarted && IsServiceAvailable && IsLoggedIn;

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

    protected void OnStarted()
    {
        ClientRequester.Send(new OrxLogonRequest(LoginCredentials, DefaultAccount.ToString(), "0.3", 0));
    }

    protected void HandleLoggedIn(OrxLogonResponse message, MessageHeader messageHeader, IConversation cx)
    {
        Logger.Info("Logon to " + ServerName + " Adapter accepted");
        IsLoggedIn = true;
        StatusUpdateHandlers?.Invoke(string.Empty, IsAvailable);
    }

    protected void HandleLoggedOut(OrxLoggedOutMessage message, MessageHeader messageHeader, IConversation cx)
    {
        Logger.Info("Logon to " + ServerName + " closed " + message.Reason);
        IsLoggedIn = false;
        StatusUpdateHandlers?.Invoke(string.Empty, IsAvailable);
    }
}
