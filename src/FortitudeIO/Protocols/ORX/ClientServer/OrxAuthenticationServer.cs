#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Transports;
using FortitudeIO.Transports.Sockets.SessionConnection;

#endregion

namespace FortitudeIO.Protocols.ORX.ClientServer;

public class OrxAuthenticationServer
{
    private static readonly IFLogger SecurityLogger = FLoggerFactory.Instance.GetLogger("Security");
    private readonly byte currentVersion;

    private readonly IMap<ISocketSessionConnection, DateTime> loggedInSessions =
        new GarbageAndLockFreeMap<ISocketSessionConnection, DateTime>(ReferenceEquals);

    protected readonly IOrxPublisher MessagePublisher;
    private readonly AutoResetEvent paAre = new(false);

    private readonly IMap<ISocketSessionConnection, DateTime> pendingAuths =
        new GarbageAndLockFreeMap<ISocketSessionConnection, DateTime>(ReferenceEquals);

    protected IRecycler OrxRecyclingFactory;
    protected bool Stopping;


    public OrxAuthenticationServer(OrxServerMessaging messagePublisher, byte currentVersion)
    {
        OrxRecyclingFactory = messagePublisher.RecyclingFactory;
        MessagePublisher = messagePublisher;
        this.currentVersion = currentVersion;
        messagePublisher.StreamFromSubscriber.RegisterDeserializer<OrxLogonRequest>(
            (ushort)AuthenticationMessages.LogonRequest, OnLogonRequest);
        messagePublisher.RegisterSerializer<OrxLogonResponse>((ushort)AuthenticationMessages.LoggedInResponse);
        messagePublisher.RegisterSerializer<OrxLoggedOutMessage>((ushort)AuthenticationMessages.LoggedOutResponse);

        messagePublisher.OnClientRemoved += OnClientRemoved;
        messagePublisher.OnDisconnecting += OnDisconnecting;

        messagePublisher.OnDisconnecting += ClearPendingAuths;
        messagePublisher.OnNewClient += AddToPendingAuths;
        messagePublisher.OnClientRemoved += RemoveFromPendingAuths;
    }


    protected void ClearPendingAuths()
    {
        lock (pendingAuths)
        {
            pendingAuths.Clear();
        }
    }

    protected void AddToPendingAuths(ISocketSessionConnection cx)
    {
        lock (pendingAuths)
        {
            pendingAuths.Add(cx, TimeContext.UtcNow);
            if (pendingAuths.Count == 1)
                ThreadPool.RegisterWaitForSingleObject(paAre, (s, t) => CheckAuthsTimeout(), null, 1000, true);
        }
    }

    protected void RemoveFromPendingAuths(ISocketSessionConnection cx)
    {
        lock (pendingAuths)
        {
            pendingAuths.Remove(cx);
        }
    }

    protected void RemoveFromLoggedIn(ISocketSessionConnection cx)
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
                    MessagePublisher.Send(cx, loggedOutMessage);
                    MessagePublisher.RemoveClient(cx);
                }

                ThreadPool.RegisterWaitForSingleObject(paAre, (s, t) => CheckAuthsTimeout(), null, 1000, true);
            }
        }
    }

    protected virtual void OnClientRemoved(ISocketSessionConnection client)
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
            MessagePublisher.Send(loggedInSession.Key, orxLoggedOutMessage);
        }

        var timeout = 10;
        while (loggedInSessions.Any() && timeout > 0)
        {
            var count = loggedInSessions.Count();
            if (count == 0) break;
            timeout--;
            Thread.Sleep(1000);
        }

        loggedInSessions.Clear();

        Stopping = false;
    }

    public virtual event OrxAuthenticator? OnAuthenticate;

    protected virtual void OnLogonRequest(OrxLogonRequest request, object? context, ISession? repositorySession)
    {
        Validate(request, context, repositorySession);
    }

    protected virtual bool Validate(OrxLogonRequest request, object? context, ISession? repositorySession)
    {
        ISocketSessionConnection repoSess = (SocketSessionConnection)repositorySession!;
        RemoveFromPendingAuths(repoSess);
        if (repoSess.AuthData != null)
        {
            SecurityLogger.Info("User '{0}' on connectionId '{1}' already authenticated",
                request.Login, repoSess.Id);
            return true;
        }

        MutableString? message = null;
        if (OnAuthenticate != null
            && OnAuthenticate.Invoke(repoSess, request.Login, request.Password, out var authData, out message))
        {
            SecurityLogger.Info("User '{0}' was authenticated on connectionId '{1}' via Legacy method",
                request.Login, repoSess.Id);
            repoSess.AuthData = authData;

            loggedInSessions.Add(repoSess, TimeContext.UtcNow);
            MessagePublisher.Send(repoSess, OrxRecyclingFactory.Borrow<OrxLogonResponse>());

            return true;
        }

        var orxLoggedOutMessage = OrxRecyclingFactory.Borrow<OrxLoggedOutMessage>();
        orxLoggedOutMessage.Configure(currentVersion, message);
        MessagePublisher.Send(repoSess, orxLoggedOutMessage);
        return false;
    }
}
