#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Conversations;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Transports.Network.State;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public class OrxAuthenticationServer
{
    private static readonly IFLogger SecurityLogger = FLoggerFactory.Instance.GetLogger("Security");
    private readonly byte currentVersion;

    private readonly IMap<IConversationRequester, DateTime> loggedInSessions =
        new GarbageAndLockFreeMap<IConversationRequester, DateTime>(ReferenceEquals);

    protected readonly IOrxMessageResponder MessageMessageResponder;
    private readonly AutoResetEvent paAre = new(false);

    private readonly IMap<IConversationRequester, DateTime> pendingAuths =
        new GarbageAndLockFreeMap<IConversationRequester, DateTime>(ReferenceEquals);

    protected IRecycler OrxRecyclingFactory;
    protected bool Stopping;

    public OrxAuthenticationServer(IOrxMessageResponder messageMessageResponder, byte currentVersion)
    {
        OrxRecyclingFactory = messageMessageResponder.RecyclingFactory;
        MessageMessageResponder = messageMessageResponder;
        this.currentVersion = currentVersion;
        messageMessageResponder.DeserializationRepository.RegisterDeserializer<OrxLogonRequest>(OnLogonRequest);
        messageMessageResponder.SerializationRepository.RegisterSerializer<OrxLogonResponse>();
        messageMessageResponder.SerializationRepository.RegisterSerializer<OrxLoggedOutMessage>();

        messageMessageResponder.ClientRemoved += OnClientRemoved;
        messageMessageResponder.Disconnecting += OnDisconnecting;

        messageMessageResponder.Disconnecting += ClearPendingAuths;
        messageMessageResponder.NewClient += AddToPendingAuths;
        messageMessageResponder.ClientRemoved += RemoveFromPendingAuths;
    }

    protected void ClearPendingAuths()
    {
        lock (pendingAuths)
        {
            pendingAuths.Clear();
        }
    }

    protected void AddToPendingAuths(IConversationRequester cx)
    {
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
                    var loggedOutMessage = OrxRecyclingFactory.Borrow<OrxLoggedOutMessage>();
                    loggedOutMessage.Configure(currentVersion, "Authentication timeout");
                    MessageMessageResponder.Send(cx, loggedOutMessage);
                    MessageMessageResponder.RemoveClient(cx);
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
            var orxLoggedOutMessage = OrxRecyclingFactory.Borrow<OrxLoggedOutMessage>();
            orxLoggedOutMessage.Configure(currentVersion, "Disconnecting");
            MessageMessageResponder.Send(loggedInSession.Key, orxLoggedOutMessage);
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

    protected virtual void OnLogonRequest(OrxLogonRequest request, object? context, IConversation? repositorySession)
    {
        Validate(request, context, repositorySession);
    }

    protected virtual bool Validate(OrxLogonRequest request, object? context, IConversation? repositorySession)
    {
        var repoSess = (IConversationRequester)repositorySession!;
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
            MessageMessageResponder.Send(repoSess, OrxRecyclingFactory.Borrow<OrxLogonResponse>());

            return true;
        }

        var orxLoggedOutMessage = OrxRecyclingFactory.Borrow<OrxLoggedOutMessage>();
        orxLoggedOutMessage.Configure(currentVersion, message);
        MessageMessageResponder.Send(repoSess, orxLoggedOutMessage);
        return false;
    }
}
