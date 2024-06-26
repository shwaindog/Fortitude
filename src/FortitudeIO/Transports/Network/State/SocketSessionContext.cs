﻿#region

using FortitudeIO.Conversations;
using FortitudeIO.Protocols;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Transports.Network.Config;
using FortitudeIO.Transports.Network.Construction;
using FortitudeIO.Transports.Network.Controls;
using FortitudeIO.Transports.Network.Conversations;
using FortitudeIO.Transports.Network.Dispatcher;
using FortitudeIO.Transports.Network.Publishing;
using FortitudeIO.Transports.Network.Receiving;
using FortitudeIO.Transports.Network.Sockets;

#endregion

namespace FortitudeIO.Transports.Network.State;

public interface ISocketSessionContext : ISocketConversation, IConversationSession
{
    new int Id { get; }
    new string Name { get; set; }
    IStreamControls? StreamControls { get; set; }
    ISocketConversation? OwningConversation { get; set; }
    SocketSessionState SocketSessionState { get; }
    SocketConversationProtocol SocketConversationProtocol { get; }
    INetworkTopicConnectionConfig NetworkTopicConnectionConfig { get; }
    ISocketConnection? SocketConnection { get; }
    ISocketReceiver? SocketReceiver { get; set; }
    ISocketSender? SocketSender { get; set; }
    ISocketDispatcher SocketDispatcher { get; }
    ISocketFactoryResolver SocketFactoryResolver { get; }
    ISocketDispatcherResolver SocketDispatcherResolver { get; }
    IMessageSerdesRepositoryFactory SerdesFactory { get; set; }
    void OnSocketStateChanged(SocketSessionState newSessionState);
    void OnDisconnected(CloseReason closeReason, string? reason = null);
    void OnReconnecting();
    void OnDisconnecting();
    void OnConnected(ISocketConnection socketConnection);
    void OnStarted();
    void OnStopped();

    void SetDisconnected();

    event Action? SocketReceiverUpdated;
    event Action? SocketSenderUpdated;
}

public class SocketSessionContext : ISocketSessionContext
{
    private static int idGen;
    private readonly ISocketConnectivityChanged socketConnectivityChanged;
    private ISocketReceiver? socketReceiver;
    private ISocketSender? socketSender;


    public SocketSessionContext(string preIdName, ConversationType conversationType,
        SocketConversationProtocol socketConversationProtocol, INetworkTopicConnectionConfig networkConnectionConfig,
        ISocketFactoryResolver socketFactoryResolver, IMessageSerdesRepositoryFactory serdesFactory,
        ISocketDispatcherResolver? socketDispatcherResolver = null)
    {
        Id = Interlocked.Increment(ref idGen);
        Name = IdInjectedName(preIdName, Id);
        SocketFactoryResolver = socketFactoryResolver;
        SocketDispatcherResolver = socketDispatcherResolver ?? socketFactoryResolver.SocketDispatcherResolver;
        SocketDispatcher = SocketDispatcherResolver.Resolve(networkConnectionConfig);
        SerdesFactory = serdesFactory;
        ConversationType = conversationType;
        SocketConversationProtocol = socketConversationProtocol;
        NetworkTopicConnectionConfig = networkConnectionConfig;
        socketConnectivityChanged = socketFactoryResolver.ConnectionChangedHandlerResolver!(this);
        StateChanged = socketConnectivityChanged.GetOnConnectionChangedHandler();
    }

    public int Id { get; }

    public IStreamControls? StreamControls { get; set; }
    public ISocketConversation? OwningConversation { get; set; }
    public ConversationType ConversationType { get; }
    public SocketConversationProtocol SocketConversationProtocol { get; }
    public ISocketFactoryResolver SocketFactoryResolver { get; }
    public ISocketDispatcherResolver SocketDispatcherResolver { get; }
    public IMessageSerdesRepositoryFactory SerdesFactory { get; set; }
    public ISocketDispatcher SocketDispatcher { get; set; }
    public ISocketConnection? SocketConnection { get; private set; }
    public INetworkTopicConnectionConfig NetworkTopicConnectionConfig { get; }
    public SocketSessionState SocketSessionState { get; set; }

    public string Name { get; set; }

    public ISocketReceiver? SocketReceiver
    {
        get => socketReceiver;
        set
        {
            if (socketReceiver == value) return;
            socketReceiver = value;
            SocketReceiverUpdated?.Invoke();
        }
    }

    public ISocketSender? SocketSender
    {
        get => socketSender;
        set
        {
            if (socketSender == value) return;
            socketSender = value;
            SocketSenderUpdated?.Invoke();
        }
    }

    public event Action<SocketSessionState>? StateChanged;

#pragma warning disable 67
    public event Action<string, int>? Error;
#pragma warning restore 67
    public event Action<ISocketConnection>? SocketConnected;
    public event Action? Connected;
    public event Action? Disconnected;
    public event Action? Disconnecting;
    public event Action? Started;
    public event Action? Stopped;
    public event Action? SocketReceiverUpdated;
    public event Action? SocketSenderUpdated;

    public ConversationState ConversationState
    {
        get
        {
            return SocketSessionState switch
            {
                SocketSessionState.New => ConversationState.New
                , SocketSessionState.Connected => ConversationState.Started
                , SocketSessionState.Connecting => ConversationState.Starting
                , SocketSessionState.Disconnecting => ConversationState.Stopping
                , SocketSessionState.Reconnecting => ConversationState.Starting
                , SocketSessionState.Disconnected => ConversationState.Stopped, _ => ConversationState.Stopped
            };
        }
    }

    public void Start()
    {
        StreamControls?.Start();
    }

    public void Stop(CloseReason closeReason = CloseReason.Completed, string? reason = null)
    {
        StreamControls?.Stop(closeReason, reason);
        OnStopped();
    }

    public bool IsStarted => SocketConnection?.IsConnected ?? false;
    public IConversationSession Session => this;

    public void OnSocketStateChanged(SocketSessionState newSessionState)
    {
        var oldState = SocketSessionState;
        SocketSessionState = newSessionState;
        StateChanged?.Invoke(oldState);
    }

    public void OnConnected(ISocketConnection socketConnection)
    {
        SocketConnection = socketConnection;
        OnSocketStateChanged(SocketSessionState.Connected);
        SocketConnected?.Invoke(socketConnection);
        Connected?.Invoke();
    }

    public void SetDisconnected()
    {
        SocketConnection?.OSSocket?.Close(1_000);
        SocketConnection = null;
        StreamControls?.StopMessaging();
    }

    public void OnDisconnected(CloseReason closeReason, string? reason = null)
    {
        socketConnectivityChanged.CloseReason = closeReason;
        socketConnectivityChanged.ReasonText = reason;
        OnSocketStateChanged(SocketSessionState.Disconnected);
        Disconnected?.Invoke();
    }

    public void OnReconnecting()
    {
        OnSocketStateChanged(SocketSessionState.Reconnecting);
    }

    public void OnDisconnecting()
    {
        if (SocketSessionState != SocketSessionState.Disconnecting)
        {
            if (SocketSessionState != SocketSessionState.Disconnected) OnSocketStateChanged(SocketSessionState.Disconnecting);

            Disconnecting?.Invoke();
        }
    }

    public void OnSessionFailure(string reason) => StreamControls?.OnSessionFailure(reason);

    public void OnStarted()
    {
        Started?.Invoke();
    }

    public void OnStopped()
    {
        Stopped?.Invoke();
    }

    private string IdInjectedName(string preIdName, int id)
    {
        var splitOnUnderscores = preIdName.Split('_').ToList();
        var idAndValue = $"ID-{id}";
        if (splitOnUnderscores.Count > 1)
            splitOnUnderscores.Insert(1, idAndValue);
        else
            splitOnUnderscores.Add(idAndValue);
        return string.Join("_", splitOnUnderscores);
    }

    public override string ToString() =>
        $"SocketSessionContext({nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(ConversationType)}: {ConversationType}, " +
        $"{nameof(SocketDispatcher)}: {SocketDispatcher}, {nameof(SocketConnection)}: {SocketConnection}, " +
        $"{nameof(SocketSessionState)}: {SocketSessionState}, " +
        $"{nameof(IsStarted)}: {IsStarted})";
}
