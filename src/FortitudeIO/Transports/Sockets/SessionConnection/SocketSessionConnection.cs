using System;
using FortitudeCommon.DataStructures.LinkedLists;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Authentication;
using FortitudeIO.Sockets;

namespace FortitudeIO.Transports.Sockets.SessionConnection
{
    public class SocketSessionConnection : ISocketSessionConnection
    {
        public SocketSessionConnection(ISocketSessionReceiver socketSessionReceiver, ISocketSessionSender socketSessionSender,
            Action<ISocketSessionConnection, string, int> errorHandler)
        {
            SessionReceiver = socketSessionReceiver;
            if (socketSessionReceiver != null)
            {
                socketSessionReceiver.Parent = this;
            }
            SessionSender = socketSessionSender;
            if (socketSessionSender != null)
            {
                socketSessionSender.Parent = this;
            }
            Error = errorHandler;
        }
        
        public IUserData AuthData { get; set; }

        public ISocketSessionConnection Previous { get; set; }

        public ISocketSessionConnection Next { get; set; }
        
        public event Action<ISocketSessionConnection, string, int> Error;
        public void OnError(ISocketSessionConnection sessionConnection, string message, int id)
        {
            Error?.Invoke(sessionConnection, message, id);
        }
        
        public ISocketSessionReceiver SessionReceiver { get; }
        ISessionReceiver ISessionConnection.SessionReceiver => SessionReceiver;
        public ISocketSessionSender SessionSender { get; }
        ISessionSender ISessionConnection.SessionSender => SessionSender;

        public long Id => SessionReceiver?.Id ?? (SessionSender?.Id ?? -1);

        public IOSSocket Socket => SessionReceiver?.Socket ?? (SessionSender?.Socket);
        public IntPtr Handle => SessionReceiver?.Handle ?? (SessionSender?.Handle ?? IntPtr.Zero);

        ISessionConnection IDoublyLinkedListNode<ISessionConnection>.Previous
        {
            get => Previous;
            set => Previous = (ISocketSessionConnection)value;
        }

        ISessionConnection IDoublyLinkedListNode<ISessionConnection>.Next
        {
            get => Next;
            set => Next = (ISocketSessionConnection)value;
        }

        public bool Active { get; set; }
        public string SessionDescription => SessionReceiver?.SessionDescription ?? (SessionSender?.SessionDescription);
    }
}