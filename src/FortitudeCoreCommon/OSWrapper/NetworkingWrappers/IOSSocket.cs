using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace FortitudeCommon.OSWrapper.NetworkingWrappers
{
    public interface IOSSocket : IDisposable
    {
        Socket UnderlyingSocket { get; }
        int Available { get; }
        EndPoint LocalEndPoint { get; }
        EndPoint RemoteEndPoint { get; }
        IntPtr Handle { get; }
        bool Blocking { get; set; }
        bool UseOnlyOverlappedIO { get; set; }
        bool Connected { get; }
        AddressFamily AddressFamily { get; }
        SocketType SocketType { get; }
        ProtocolType ProtocolType { get; }
        bool IsBound { get; }
        bool ExclusiveAddressUse { get; set; }
        int ReceiveBufferSize { get; set; }
        int SendBufferSize { get; set; }
        int ReceiveTimeout { get; set; }
        int SendTimeout { get; set; }
        LingerOption LingerState { get; set; }
        bool NoDelay { get; set; }
        short Ttl { get; set; }
        bool DontFragment { get; set; }
        bool MulticastLoopback { get; set; }
        bool EnableBroadcast { get; set; }
        bool DualMode { get; set; }
        void Bind(EndPoint localEP);
        void Connect(EndPoint remoteEP);
        void Connect(IPAddress address, int port);
        void Connect(string host, int port);
        void Connect(IPAddress[] addresses, int port);
        void Close();
        void Close(int timeout);
        void Listen(int backlog);
        IOSSocket Accept();
        int Send(byte[] buffer, int size, SocketFlags socketFlags);
        int Send(byte[] buffer, SocketFlags socketFlags);
        int Send(byte[] buffer);
        int Send(IList<ArraySegment<byte>> buffers);
        int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags);
        int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode);
        void SendFile(string fileName);
        void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags);
        int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags);
        int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode);
        int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP);
        int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP);
        int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP);
        int SendTo(byte[] buffer, EndPoint remoteEP);
        int Receive(byte[] buffer, int size, SocketFlags socketFlags);
        int Receive(byte[] buffer, SocketFlags socketFlags);
        int Receive(byte[] buffer);
        int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags);
        int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode);
        int Receive(IList<ArraySegment<byte>> buffers);
        int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags);
        int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode);

        int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP,
            out IPPacketInformation ipPacketInformation);

        int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP);
        int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP);
        int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP);
        int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP);
        int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue);
        int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue);
        void SetIPProtectionLevel(IPProtectionLevel level);
        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue);
        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue);
        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue);
        void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue);
        object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName);
        void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue);
        byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength);
        bool Poll(int microSeconds, SelectMode mode);
        IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state);
        IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state);
        SocketInformation DuplicateAndClose(int targetProcessId);
        IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state);
        IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state);
        IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state);
        IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state);
        void Disconnect(bool reuseSocket);
        void EndConnect(IAsyncResult asyncResult);
        void EndDisconnect(IAsyncResult asyncResult);

        IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback,
            object state);

        IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode,
            AsyncCallback callback, object state);

        IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags,
            AsyncCallback callback, object state);

        IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback,
            object state);

        IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode,
            AsyncCallback callback, object state);

        int EndSend(IAsyncResult asyncResult);
        int EndSend(IAsyncResult asyncResult, out SocketError errorCode);
        void EndSendFile(IAsyncResult asyncResult);

        IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP,
            AsyncCallback callback, object state);

        int EndSendTo(IAsyncResult asyncResult);

        IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback,
            object state);

        IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags,
            out SocketError errorCode, AsyncCallback callback, object state);

        IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback,
            object state);

        IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode,
            AsyncCallback callback, object state);

        int EndReceive(IAsyncResult asyncResult);
        int EndReceive(IAsyncResult asyncResult, out SocketError errorCode);

        IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags,
            ref EndPoint remoteEP, AsyncCallback callback, object state);

        int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint,
            out IPPacketInformation ipPacketInformation);

        IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags,
            ref EndPoint remoteEP, AsyncCallback callback, object state);

        int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint);
        IAsyncResult BeginAccept(AsyncCallback callback, object state);
        IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state);
        IAsyncResult BeginAccept(IOSSocket acceptSocket, int receiveSize, AsyncCallback callback, object state);
        IOSSocket EndAccept(IAsyncResult asyncResult);
        IOSSocket EndAccept(out byte[] buffer, IAsyncResult asyncResult);
        IOSSocket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult);
        void Shutdown(SocketShutdown how);
        bool AcceptAsync(SocketAsyncEventArgs e);
        bool ConnectAsync(SocketAsyncEventArgs e);
        bool DisconnectAsync(SocketAsyncEventArgs e);
        bool ReceiveAsync(SocketAsyncEventArgs e);
        bool ReceiveFromAsync(SocketAsyncEventArgs e);
        bool ReceiveMessageFromAsync(SocketAsyncEventArgs e);
        bool SendAsync(SocketAsyncEventArgs e);
        bool SendPacketsAsync(SocketAsyncEventArgs e);
        bool SendToAsync(SocketAsyncEventArgs e);
    }
}
