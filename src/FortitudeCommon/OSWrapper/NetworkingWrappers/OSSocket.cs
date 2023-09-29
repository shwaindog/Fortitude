using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace FortitudeCommon.OSWrapper.NetworkingWrappers
{
    public class OSSocket : IOSSocket
    {
        private readonly Socket socket;

        public OSSocket(Socket socket)
        {
            this.socket = socket;
        }

        public void Dispose()
        {
            socket.Dispose();
        }

        public Socket UnderlyingSocket => socket;
        public int Available => socket.Available;
        public EndPoint LocalEndPoint => socket.LocalEndPoint;
        public EndPoint RemoteEndPoint => socket.RemoteEndPoint;
        public IntPtr Handle => socket.Handle;
        public bool Blocking
        {
            get { return socket.Blocking; }
            set { socket.Blocking = value; }
        }
        public bool UseOnlyOverlappedIO
        {
            get { return socket.UseOnlyOverlappedIO; }
            set { socket.UseOnlyOverlappedIO = value; }
        }
        public bool Connected => socket.Connected;
        public AddressFamily AddressFamily => socket.AddressFamily;
        public SocketType SocketType => socket.SocketType;
        public ProtocolType ProtocolType => socket.ProtocolType;
        public bool IsBound => socket.IsBound;
        public bool ExclusiveAddressUse
        {
            get { return socket.ExclusiveAddressUse; }
            set { socket.ExclusiveAddressUse = value; }
        }

        public int ReceiveBufferSize
        {
            get { return socket.ReceiveBufferSize; }
            set { socket.ReceiveBufferSize = value; }
        }

        public int SendBufferSize
        {
            get { return socket.SendBufferSize; }
            set { socket.SendBufferSize = value; }
        }

        public int ReceiveTimeout
        {
            get { return socket.ReceiveTimeout; }
            set { socket.ReceiveTimeout = value; }
        }

        public int SendTimeout
        {
            get { return socket.SendTimeout; }
            set { socket.SendTimeout = value; }
        }

        public LingerOption LingerState
        {
            get { return socket.LingerState; }
            set { socket.LingerState = value; }
        }

        public bool NoDelay
        {
            get { return socket.NoDelay; }
            set { socket.NoDelay = value; }
        }

        public short Ttl
        {
            get { return socket.Ttl; }
            set { socket.Ttl = value; }
        }

        public bool DontFragment
        {
            get { return socket.DontFragment; }
            set { socket.DontFragment = value; }
        }

        public bool MulticastLoopback
        {
            get { return socket.MulticastLoopback; }
            set { socket.MulticastLoopback = value; }
        }

        public bool EnableBroadcast
        {
            get { return socket.EnableBroadcast; }
            set { socket.EnableBroadcast = value; }
        }

        public bool DualMode
        {
            get { return socket.DualMode; }
            set { socket.DualMode = value; }
        }

        public void Bind(EndPoint localEP)
        {
            socket.Bind(localEP);
        }

        public void Connect(EndPoint remoteEP)
        {
            socket.Connect(remoteEP);
        }

        public void Connect(IPAddress address, int port)
        {
            socket.Connect(address, port);
        }

        public void Connect(string host, int port)
        {
            socket.Connect(host, port);
        }

        public void Connect(IPAddress[] addresses, int port)
        {
            socket.Connect(addresses, port);
        }

        public void Close()
        {
            socket.Close();
        }

        public void Close(int timeout)
        {
            socket.Close(timeout);
        }

        public void Listen(int backlog)
        {
            socket.Listen(backlog);
        }

        public IOSSocket Accept()
        {
            return new OSSocket(socket.Accept());
        }

        public int Send(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return socket.Send(buffer, size, socketFlags);
        }

        public int Send(byte[] buffer, SocketFlags socketFlags)
        {
            return socket.Send(buffer, socketFlags);
        }

        public int Send(byte[] buffer)
        {
            return socket.Send(buffer);
        }

        public int Send(IList<ArraySegment<byte>> buffers)
        {
            return socket.Send(buffers);
        }

        public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return socket.Send(buffers, socketFlags);
        }

        public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return socket.Send(buffers, socketFlags, out errorCode);
        }

        public void SendFile(string fileName)
        {
            socket.SendFile(fileName);
        }

        public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
        {
            socket.SendFile(fileName, preBuffer, postBuffer, flags);
        }

        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return socket.Send(buffer, offset, size, socketFlags);
        }

        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        {
            return socket.Send(buffer, offset, size, socketFlags, out errorCode);
        }

        public int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP)
        {
            return socket.SendTo(buffer, offset, size, socketFlags, remoteEP);
        }

        public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP)
        {
            return socket.SendTo(buffer, size, socketFlags, remoteEP);
        }

        public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP)
        {
            return socket.SendTo(buffer, socketFlags, remoteEP);
        }

        public int SendTo(byte[] buffer, EndPoint remoteEP)
        {
            return socket.SendTo(buffer, remoteEP);
        }

        public int Receive(byte[] buffer, int size, SocketFlags socketFlags)
        {
            return socket.Receive(buffer, size, socketFlags);
        }

        public int Receive(byte[] buffer, SocketFlags socketFlags)
        {
            return socket.Receive(buffer, socketFlags);
        }

        public int Receive(byte[] buffer)
        {
            return socket.Receive(buffer);
        }

        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags)
        {
            return socket.Receive(buffer, offset, size, socketFlags);
        }

        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode)
        {
            return socket.Receive(buffer, offset, size, socketFlags, out errorCode);
        }

        public int Receive(IList<ArraySegment<byte>> buffers)
        {
            return socket.Receive(buffers);
        }

        public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags)
        {
            return socket.Receive(buffers, socketFlags);
        }

        public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode)
        {
            return socket.Receive(buffers, socketFlags, out errorCode);
        }

        public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP,
            out IPPacketInformation ipPacketInformation)
        {
            return socket.ReceiveMessageFrom(buffer, offset, size, ref socketFlags, ref remoteEP, out ipPacketInformation);
        }

        public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
        {
            return socket.ReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP);
        }

        public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP)
        {
            return socket.ReceiveFrom(buffer, size, socketFlags, ref remoteEP);
        }

        public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP)
        {
            return socket.ReceiveFrom(buffer, socketFlags, ref remoteEP);
        }

        public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP)
        {
            return socket.ReceiveFrom(buffer, ref remoteEP);
        }

        public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return socket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue)
        {
            return socket.IOControl(ioControlCode, optionInValue, optionOutValue);
        }

        public void SetIPProtectionLevel(IPProtectionLevel level)
        {
            socket.SetIPProtectionLevel(level);
        }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue)
        {
            socket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            socket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue)
        {
            socket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue)
        {
            socket.SetSocketOption(optionLevel, optionName, optionValue);
        }

        public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName)
        {
            return socket.GetSocketOption(optionLevel, optionName);
        }

        public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue)
        {
            socket.GetSocketOption(optionLevel, optionName, optionValue);
        }

        public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength)
        {
            return socket.GetSocketOption(optionLevel, optionName, optionLength);
        }

        public bool Poll(int microSeconds, SelectMode mode)
        {
            return socket.Poll(microSeconds, mode);
        }

        public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state)
        {
            return socket.BeginSendFile(fileName, callback, state);
        }

        public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state)
        {
            return socket.BeginConnect(remoteEP, callback, state);
        }

        public SocketInformation DuplicateAndClose(int targetProcessId)
        {
            return socket.DuplicateAndClose(targetProcessId);
        }

        public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state)
        {
            return socket.BeginConnect(host, port, requestCallback, state);
        }

        public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state)
        {
            return socket.BeginConnect(address, port, requestCallback, state);
        }

        public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state)
        {
            return socket.BeginConnect(addresses, port, requestCallback, state);
        }

        public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state)
        {
            return socket.BeginDisconnect(reuseSocket, callback, state);
        }

        public void Disconnect(bool reuseSocket)
        {
            socket.Disconnect(reuseSocket);
        }

        public void EndConnect(IAsyncResult asyncResult)
        {
            socket.EndConnect(asyncResult);
        }

        public void EndDisconnect(IAsyncResult asyncResult)
        {
            socket.EndDisconnect(asyncResult);
        }

        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback,
            object state)
        {
            return socket.BeginSend(buffer, offset, size, socketFlags, callback, state);
        }

        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode,
            AsyncCallback callback, object state)
        {
            return socket.BeginSend(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags,
            AsyncCallback callback, object state)
        {
            return socket.BeginSendFile(fileName, preBuffer, postBuffer, flags, callback, state);
        }

        public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            return socket.BeginSend(buffers, socketFlags, callback, state);
        }

        public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback,
            object state)
        {
            return socket.BeginSend(buffers, socketFlags, out errorCode, callback, state);
        }

        public int EndSend(IAsyncResult asyncResult)
        {
            return socket.EndSend(asyncResult);
        }

        public int EndSend(IAsyncResult asyncResult, out SocketError errorCode)
        {
            return socket.EndSend(asyncResult, out errorCode);
        }

        public void EndSendFile(IAsyncResult asyncResult)
        {
            socket.EndSendFile(asyncResult);
        }

        public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP,
            AsyncCallback callback, object state)
        {
            return socket.BeginSendTo(buffer, offset, size, socketFlags, remoteEP, callback, state);
        }

        public int EndSendTo(IAsyncResult asyncResult)
        {
            return socket.EndSendTo(asyncResult);
        }

        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback,
            object state)
        {
            return socket.BeginReceive(buffer, offset, size, socketFlags, callback, state);
        }

        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode,
            AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffer, offset, size, socketFlags, out errorCode, callback, state);
        }

        public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffers, socketFlags, callback, state);
        }

        public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback,
            object state)
        {
            return socket.BeginReceive(buffers, socketFlags, out errorCode, callback, state);
        }

        public int EndReceive(IAsyncResult asyncResult)
        {
            return socket.EndReceive(asyncResult);
        }

        public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode)
        {
            return socket.EndReceive(asyncResult, out errorCode);
        }

        public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP,
            AsyncCallback callback, object state)
        {
            return socket.BeginReceiveMessageFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
        }

        public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint,
            out IPPacketInformation ipPacketInformation)
        {
            return socket.EndReceiveMessageFrom(asyncResult, ref socketFlags, ref endPoint, out ipPacketInformation);
        }

        public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP,
            AsyncCallback callback, object state)
        {
            return socket.BeginReceiveFrom(buffer, offset, size, socketFlags, ref remoteEP, callback, state);
        }

        public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint)
        {
            return socket.EndReceiveFrom(asyncResult, ref endPoint);
        }

        public IAsyncResult BeginAccept(AsyncCallback callback, object state)
        {
            return socket.BeginAccept(callback, state);
        }

        public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state)
        {
            return socket.BeginAccept(receiveSize, callback, state);
        }

        public IAsyncResult BeginAccept(IOSSocket acceptSocket, int receiveSize, AsyncCallback callback, object state)
        {
            return socket.BeginAccept(acceptSocket.UnderlyingSocket, receiveSize, callback, state);
        }

        public IOSSocket EndAccept(IAsyncResult asyncResult)
        {
            return new OSSocket(socket.EndAccept(asyncResult));
        }

        public IOSSocket EndAccept(out byte[] buffer, IAsyncResult asyncResult)
        {
            return new OSSocket(socket.EndAccept(out buffer, asyncResult));
        }

        public IOSSocket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult)
        {
            return new OSSocket(socket.EndAccept(out buffer, out bytesTransferred, asyncResult));
        }

        public void Shutdown(SocketShutdown how)
        {
            socket.Shutdown(how);
        }

        public bool AcceptAsync(SocketAsyncEventArgs e)
        {
            return socket.AcceptAsync(e);
        }

        public bool ConnectAsync(SocketAsyncEventArgs e)
        {
            return socket.ConnectAsync(e);
        }

        public bool DisconnectAsync(SocketAsyncEventArgs e)
        {
            return socket.DisconnectAsync(e);
        }

        public bool ReceiveAsync(SocketAsyncEventArgs e)
        {
            return socket.ReceiveAsync(e);
        }

        public bool ReceiveFromAsync(SocketAsyncEventArgs e)
        {
            return socket.ReceiveFromAsync(e);
        }

        public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e)
        {
            return socket.ReceiveMessageFromAsync(e);
        }

        public bool SendAsync(SocketAsyncEventArgs e)
        {
            return socket.SendAsync(e);
        }

        public bool SendPacketsAsync(SocketAsyncEventArgs e)
        {
            return socket.SendPacketsAsync(e);
        }

        public bool SendToAsync(SocketAsyncEventArgs e)
        {
            return socket.SendToAsync(e);
        }
    }
}
