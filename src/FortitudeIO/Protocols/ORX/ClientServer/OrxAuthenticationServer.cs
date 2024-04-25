#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes.Binary;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.Serdes.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public class OrxAuthenticationServer
{
    private static readonly IFLogger SecurityLogger = FLoggerFactory.Instance.GetLogger("Security");

    protected readonly IOrxMessageResponder AcceptorSession;
    private readonly byte currentVersion;

    private readonly IMap<IConversationRequester, DateTime> loggedInSessions =
        new GarbageAndLockFreeMap<IConversationRequester, DateTime>(ReferenceEquals);

    private readonly AutoResetEvent paAre = new(false);

    private readonly IMap<IConversationRequester, DateTime> pendingAuths =
        new GarbageAndLockFreeMap<IConversationRequester, DateTime>(ReferenceEquals);

    protected IRecycler Recycler = new Recycler();
    protected bool Stopping;

    public OrxAuthenticationServer(IOrxMessageResponder acceptorSession, byte currentVersion)
    {
        AcceptorSession = acceptorSession;
        this.currentVersion = currentVersion;
        acceptorSession.SerializationRepository.RegisterSerializer<OrxLogonResponse>();
        acceptorSession.SerializationRepository.RegisterSerializer<OrxLoggedOutMessage>();

        acceptorSession.ClientRemoved += OnClientRemoved;
        acceptorSession.Disconnecting += OnDisconnecting;

        acceptorSession.Disconnecting += ClearPendingAuths;
        acceptorSession.NewClient += OnNewAuthClientConversation;
        acceptorSession.ClientRemoved += RemoveFromPendingAuths;
    }

    protected void ClearPendingAuths()
    {
        lock (pendingAuths)
        {
            pendingAuths.Clear();
        }
    }

    protected void OnNewAuthClientConversation(IConversationRequester cx)
    {
        var clientDecoder = (IOrxStreamDecoder)cx.StreamListener!.Decoder!;
        clientDecoder.MessageDeserializationRepository.RegisterDeserializer<OrxLogonRequest>()
            .AddDeserializedNotifier(
                new PassThroughDeserializedNotifier<OrxLogonRequest>($"{nameof(OrxAuthenticationServer)}.{nameof(OnLogonRequest)}", OnLogonRequest));
        lock (pendingAuths)
        {
            pendingAuths.Add(cx, TimeContext.UtcNow);
            if (pendingAuths.Count == 1)
                ThreadPool.RegisterWaitForSingleObject(paAre, (s, t) => CheckAuthsTimeout(), null, 1000, true);
        }
    }

    protected void RemoveFromPendingAuths(IConversationRequester cx)
    {
        lock (pendingAuths)
        {
            pendingAuths.Remove(cx);
        }
    }

    protected void RemoveFromLoggedIn(IConversationRequester cx)
    {
        lock (loggedInSessions)
        {
            loggedInSessions.Remove(cx);
        }
    }

    private void CheckAuthsTimeout()
    {
        lock (pendingAuths)
        {
            if (pendingAuths.Count > 0)
            {
                var timeouts = pendingAuths.Where(kv => (TimeContext.UtcNow - kv.Value).TotalSeconds > 3)
                    .Select(kv => kv.Key).ToArray();
                foreach (var cx in timeouts)
                {
                    var loggedOutMessage = Recycler.Borrow<OrxLoggedOutMessage>();
                    loggedOutMessage.Configure(currentVersion, "Authentication timeout");
                    cx.Send(loggedOutMessage);
                    AcceptorSession.RemoveClient(cx, CloseReason.RemoteDisconnecting
                        , "Failed to send valid authorization details within the required time limit.");
                }

                ThreadPool.RegisterWaitForSingleObject(paAre, (s, t) => CheckAuthsTimeout(), null, 1000, true);
            }
        }
    }

    protected virtual void OnClientRemoved(IConversationRequester client)
    {
        RemoveFromLoggedIn(client);
    }

    protected virtual void OnDisconnecting()
    {
        Stopping = true;

        foreach (var loggedInSession in loggedInSessions)
        {
            var orxLoggedOutMessage = Recycler.Borrow<OrxLoggedOutMessage>();
            orxLoggedOutMessage.Configure(currentVersion, "Disconnecting");
            loggedInSession.Key.Send(orxLoggedOutMessage);
            orxLoggedOutMessage.DecrementRefCount();
        }

        var timeout = 10;
        while (loggedInSessions.Count > 0 && timeout > 0)
        {
            var count = loggedInSessions.Count;
            if (count == 0) break;
            timeout--;
            Thread.Sleep(1000);
        }

        loggedInSessions.Clear();

        Stopping = false;
    }

    public event OrxAuthenticator? OnAuthenticate;

    protected virtual void OnLogonRequest(OrxLogonRequest request, MessageHeader messageHeader, IConversation repositorySession)
    {
        Validate(request, messageHeader, repositorySession);
    }

    protected virtual bool Validate(OrxLogonRequest request, MessageHeader messageHeader, IConversation conversation)
    {
        var repoSess = (IConversationRequester)conversation!;
        RemoveFromPendingAuths(repoSess);
        if (repoSess.Session is not ISocketSessionContext socketSessionContext) return false;
        if (socketSessionContext.SocketConnection!.IsAuthenticated)
        {
            SecurityLogger.Info("User '{0}' on connectionId '{1}' already authenticated",
                request.Login, repoSess.Id);
            return true;
        }

        MutableString? message = null;
        if (OnAuthenticate != null
            && OnAuthenticate.Invoke(socketSessionContext, request.Login, request.Password, out var authData
                , out message))
        {
            SecurityLogger.Info("User '{0}' was authenticated on connectionId '{1}' via Legacy method",
                request.Login, repoSess.Id);
            socketSessionContext.SocketConnection!.IsAuthenticated = true;

            loggedInSessions.Add(repoSess, TimeContext.UtcNow);
            var orxLogonResponse = Recycler.Borrow<OrxLogonResponse>();
            repoSess.Send(orxLogonResponse);
            orxLogonResponse.DecrementRefCount();

            return true;
        }

        var orxLoggedOutMessage = Recycler.Borrow<OrxLoggedOutMessage>();
        orxLoggedOutMessage.Configure(currentVersion, message);
        repoSess.Send(orxLoggedOutMessage);
        orxLoggedOutMessage.DecrementRefCount();
        return false;
    }
}
