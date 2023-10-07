#region

using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

#endregion

namespace FortitudeCommon.OSWrapper.NetworkingWrappers;

public class OSSocket : IOSSocket
{
    public OSSocket(Socket socket) => UnderlyingSocket = socket;

    public void Dispose()
    {
        UnderlyingSocket.Dispose();
    }

    public Socket UnderlyingSocket { get; }

    public int Available => UnderlyingSocket.Available;
    public EndPoint LocalEndPoint => UnderlyingSocket.LocalEndPoint!;
    public EndPoint RemoteEndPoint => UnderlyingSocket.RemoteEndPoint!;
    public IntPtr Handle => UnderlyingSocket.Handle;

    public bool Blocking
    {
        get => UnderlyingSocket.Blocking;
        set => UnderlyingSocket.Blocking = value;
    }

    [Obsolete("UseOnlyOverlappedIO has been deprecated and is not supported.")]
    public bool UseOnlyOverlappedIO
    {
        get => UnderlyingSocket.UseOnlyOverlappedIO;
        set => UnderlyingSocket.UseOnlyOverlappedIO = value;
    }

    public bool Connected => UnderlyingSocket.Connected;
    public AddressFamily AddressFamily => UnderlyingSocket.AddressFamily;
    public SocketType SocketType => UnderlyingSocket.SocketType;
    public ProtocolType ProtocolType => UnderlyingSocket.ProtocolType;
    public bool IsBound => UnderlyingSocket.IsBound;

    public bool ExclusiveAddressUse
    {
        get => UnderlyingSocket.ExclusiveAddressUse;
        set => UnderlyingSocket.ExclusiveAddressUse = value;
    }

    public int ReceiveBufferSize
    {
        get => UnderlyingSocket.ReceiveBufferSize;
        set => UnderlyingSocket.ReceiveBufferSize = value;
    }

    public int SendBufferSize
    {
        get => UnderlyingSocket.SendBufferSize;
        set => UnderlyingSocket.SendBufferSize = value;
    }

    public int ReceiveTimeout
    {
        get => UnderlyingSocket.ReceiveTimeout;
        set => UnderlyingSocket.ReceiveTimeout = value;
    }

    public int SendTimeout
    {
        get => UnderlyingSocket.SendTimeout;
        set => UnderlyingSocket.SendTimeout = value;
    }

    public LingerOption LingerState
    {
        get => UnderlyingSocket.LingerState!;
        set => UnderlyingSocket.LingerState = value;
    }

    public bool NoDelay
    {
        get => UnderlyingSocket.NoDelay;
        set => UnderlyingSocket.NoDelay = value;
    }

    public short Ttl
    {
        get => UnderlyingSocket.Ttl;
        set => UnderlyingSocket.Ttl = value;
    }

    public bool DontFragment
    {
        get => UnderlyingSocket.DontFragment;
        set => UnderlyingSocket.DontFragment = value;
    }

    public bool MulticastLoopback
    {
        get => UnderlyingSocket.MulticastLoopback;
        set => UnderlyingSocket.MulticastLoopback = value;
    }

    public bool EnableBroadcast
    {
        get => UnderlyingSocket.EnableBroadcast;
        set => UnderlyingSocket.EnableBroadcast = value;
    }

    public bool DualMode
    {
        get => UnderlyingSocket.DualMode;
        set => UnderlyingSocket.DualMode = value;
    }

    public void Bind(EndPoint localEP)
    {
        UnderlyingSocket.Bind(localEP);
    }

    public void Connect(EndPoint remoteEP)
    {
        UnderlyingSocket.Connect(remoteEP);
    }

    public void Connect(IPAddress address, int port)
    {
        UnderlyingSocket.Connect(address, port);
    }

    public void Connect(string host, int port)
    {
        UnderlyingSocket.Connect(host, port);
    }

    public void Connect(IPAddress[] addresses, int port)
    {
        UnderlyingSocket.Connect(addresses, port);
    }

    public void Close()
    {
        UnderlyingSocket.Close();
    }

    public void Close(int timeout)
    {
        UnderlyingSocket.Close(timeout);
    }

    public void Listen(int backlog)
    {
        UnderlyingSocket.Listen(backlog);
    }

    public IOSSocket Accept() => new OSSocket(UnderlyingSocket.Accept());

    public int Send(byte[] buffer, int size, SocketFlags socketFlags) =>
        UnderlyingSocket.Send(buffer, size, socketFlags);

    public int Send(byte[] buffer, SocketFlags socketFlags) => UnderlyingSocket.Send(buffer, socketFlags);

    public int Send(byte[] buffer) => UnderlyingSocket.Send(buffer);

    public int Send(IList<ArraySegment<byte>> buffers) => UnderlyingSocket.Send(buffers);

    public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags) =>
        UnderlyingSocket.Send(buffers, socketFlags);

    public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode) =>
        UnderlyingSocket.Send(buffers, socketFlags, out errorCode);

    public void SendFile(string fileName)
    {
        UnderlyingSocket.SendFile(fileName);
    }

    public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
    {
        UnderlyingSocket.SendFile(fileName, preBuffer, postBuffer, flags);
    }

    public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags) =>
        UnderlyingSocket.Send(buffer, offset, size, socketFlags);

    public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode) =>
        UnderlyingSocket.Send(buffer, offset, size, socketFlags, out errorCode);

    public int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP) =>
        UnderlyingSocket.SendTo(buffer, offset, size, socketFlags, remoteEP);

    public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP) =>
        UnderlyingSocket.SendTo(buffer, size, socketFlags, remoteEP);

    public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP) =>
        UnderlyingSocket.SendTo(buffer, socketFlags, remoteEP);

    public int SendTo(byte[] buffer, EndPoint remoteEP) => UnderlyingSocket.SendTo(buffer, remoteEP);

    public int Receive(byte[] buffer, int size, SocketFlags socketFlags) =>
        UnderlyingSocket.Receive(buffer, size, socketFlags);

    public int Receive(byte[] buffer, SocketFlags socketFlags) => UnderlyingSocket.Receive(buffer, socketFlags);

    public int Receive(byte[] buffer) => UnderlyingSocket.Receive(buffer);

    public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags) =>
        UnderlyingSocket.Receive(buffer, offset, size, socketFlags);

    public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode) =>
        UnderlyingSocket.Receive(buffer, offset, size, socketFlags, out errorCode);

    public int Receive(IList<ArraySegment<byte>> buffers) => UnderlyingSocket.Receive(buffers);

    public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags) =>
        UnderlyingSocket.Receive(buffers, socketFlags);

    public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode) =>
        UnderlyingSocket.Receive(buffers, socketFlags, out errorCode);

    public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags,
        ref EndPoint remoteEP,
        out IPPacketInformation ipPacketInformation) =>
        UnderlyingSocket.ReceiveMessageFrom(buffer, offset, size, ref socketFlags, ref remoteEP,
            out ipPacketInformation);

    public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP) =>
        UnderlyingSocket.ReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP);

    public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP) =>
        UnderlyingSocket.ReceiveFrom(buffer, size, socketFlags, ref remoteEP);

    public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP) =>
        UnderlyingSocket.ReceiveFrom(buffer, socketFlags, ref remoteEP);

    public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP) => UnderlyingSocket.ReceiveFrom(buffer, ref remoteEP);

    public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue) =>
        UnderlyingSocket.IOControl(ioControlCode, optionInValue, optionOutValue);

    public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[]? optionOutValue) =>
        UnderlyingSocket.IOControl(ioControlCode, optionInValue, optionOutValue);


    public void SetIPProtectionLevel(IPProtectionLevel level)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) UnderlyingSocket.SetIPProtectionLevel(level);
    }

    public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
    {
        UnderlyingSocket.SetSocketOption(optionLevel, optionName, optionValue);
    }

    public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
    {
        UnderlyingSocket.SetSocketOption(optionLevel, optionName, optionValue);
    }

    public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
    {
        UnderlyingSocket.SetSocketOption(optionLevel, optionName, optionValue);
    }

    public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
    {
        UnderlyingSocket.SetSocketOption(optionLevel, optionName, optionValue);
    }

    public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName) =>
        UnderlyingSocket.GetSocketOption(optionLevel, optionName)!;

    public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
    {
        UnderlyingSocket.GetSocketOption(optionLevel, optionName, optionValue);
    }

    public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength) =>
        UnderlyingSocket.GetSocketOption(optionLevel, optionName, optionLength);

    public bool Poll(int microSeconds, SelectMode mode) => UnderlyingSocket.Poll(microSeconds, mode);

    public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginSendFile(fileName, callback, state);

    public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginConnect(remoteEP, callback, state);

    public SocketInformation DuplicateAndClose(int targetProcessId) =>
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            UnderlyingSocket.DuplicateAndClose(targetProcessId) :
            new SocketInformation();

    public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state) =>
        UnderlyingSocket.BeginConnect(host, port, requestCallback, state);

    public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state) =>
        UnderlyingSocket.BeginConnect(address, port, requestCallback, state);

    public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state) =>
        UnderlyingSocket.BeginConnect(addresses, port, requestCallback, state);

    public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginDisconnect(reuseSocket, callback, state);

    public void Disconnect(bool reuseSocket)
    {
        UnderlyingSocket.Disconnect(reuseSocket);
    }

    public void EndConnect(IAsyncResult asyncResult)
    {
        UnderlyingSocket.EndConnect(asyncResult);
    }

    public void EndDisconnect(IAsyncResult asyncResult)
    {
        UnderlyingSocket.EndDisconnect(asyncResult);
    }

    public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags,
        AsyncCallback callback,
        object state) =>
        UnderlyingSocket.BeginSend(buffer, offset, size, socketFlags, callback, state);

    public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags,
        out SocketError errorCode,
        AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginSend(buffer, offset, size, socketFlags, out errorCode, callback, state)!;

    public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer,
        TransmitFileOptions flags,
        AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginSendFile(fileName, preBuffer, postBuffer, flags, callback, state);

    public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags,
        AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginSend(buffers, socketFlags, callback, state);

    public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags,
        out SocketError errorCode, AsyncCallback callback,
        object state) =>
        UnderlyingSocket.BeginSend(buffers, socketFlags, out errorCode, callback, state)!;

    public int EndSend(IAsyncResult asyncResult) => UnderlyingSocket.EndSend(asyncResult);

    public int EndSend(IAsyncResult asyncResult, out SocketError errorCode) =>
        UnderlyingSocket.EndSend(asyncResult, out errorCode);

    public void EndSendFile(IAsyncResult asyncResult)
    {
        UnderlyingSocket.EndSendFile(asyncResult);
    }

    public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP,
        AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginSendTo(buffer, offset, size, socketFlags, remoteEP, callback, state);

    public int EndSendTo(IAsyncResult asyncResult) => UnderlyingSocket.EndSendTo(asyncResult);

    public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags,
        AsyncCallback callback,
        object state) =>
        UnderlyingSocket.BeginReceive(buffer, offset, size, socketFlags, callback, state);

    public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags,
        out SocketError errorCode,
        AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginReceive(buffer, offset, size, socketFlags, out errorCode, callback, state)!;

    public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags,
        AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginReceive(buffers, socketFlags, callback, state);

    public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags,
        out SocketError errorCode, AsyncCallback callback,
        object state) =>
        UnderlyingSocket.BeginReceive(buffers, socketFlags, out errorCode, callback, state)!;

    public int EndReceive(IAsyncResult asyncResult) => UnderlyingSocket.EndReceive(asyncResult);

    public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode) =>
        UnderlyingSocket.EndReceive(asyncResult, out errorCode);

    public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags,
        ref EndPoint remoteEP,
        AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginReceiveMessageFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);

    public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint,
        out IPPacketInformation ipPacketInformation) =>
        UnderlyingSocket.EndReceiveMessageFrom(asyncResult, ref socketFlags, ref endPoint, out ipPacketInformation);

    public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags,
        ref EndPoint remoteEP,
        AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);

    public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint) =>
        UnderlyingSocket.EndReceiveFrom(asyncResult, ref endPoint);

    public IAsyncResult BeginAccept(AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginAccept(callback, state);

    public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginAccept(receiveSize, callback, state);

    public IAsyncResult BeginAccept(IOSSocket acceptSocket, int receiveSize, AsyncCallback callback, object state) =>
        UnderlyingSocket.BeginAccept(acceptSocket.UnderlyingSocket, receiveSize, callback, state);

    public IOSSocket EndAccept(IAsyncResult asyncResult) => new OSSocket(UnderlyingSocket.EndAccept(asyncResult));

    public IOSSocket EndAccept(out byte[] buffer, IAsyncResult asyncResult) =>
        new OSSocket(UnderlyingSocket.EndAccept(out buffer, asyncResult));

    public IOSSocket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult) =>
        new OSSocket(UnderlyingSocket.EndAccept(out buffer, out bytesTransferred, asyncResult));

    public void Shutdown(SocketShutdown how)
    {
        UnderlyingSocket.Shutdown(how);
    }

    public bool AcceptAsync(SocketAsyncEventArgs e) => UnderlyingSocket.AcceptAsync(e);

    public bool ConnectAsync(SocketAsyncEventArgs e) => UnderlyingSocket.ConnectAsync(e);

    public bool DisconnectAsync(SocketAsyncEventArgs e) => UnderlyingSocket.DisconnectAsync(e);

    public bool ReceiveAsync(SocketAsyncEventArgs e) => UnderlyingSocket.ReceiveAsync(e);

    public bool ReceiveFromAsync(SocketAsyncEventArgs e) => UnderlyingSocket.ReceiveFromAsync(e);

    public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e) => UnderlyingSocket.ReceiveMessageFromAsync(e);

    public bool SendAsync(SocketAsyncEventArgs e) => UnderlyingSocket.SendAsync(e);

    public bool SendPacketsAsync(SocketAsyncEventArgs e) => UnderlyingSocket.SendPacketsAsync(e);

    public bool SendToAsync(SocketAsyncEventArgs e) => UnderlyingSocket.SendToAsync(e);
}
