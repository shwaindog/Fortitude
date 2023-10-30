#region

using System.Net.Sockets;
using FortitudeCommon.OSWrapper.NetworkingWrappers;

#endregion

namespace FortitudeTests.FortitudeCommon.OSWrapper.NetworkingWrappers;

public class DirectOSNetworkingStub : IDirectOSNetworkingApi
{
    private readonly Func<int>? getLastErrorCallback;
    private readonly Queue<SocketDataRead> itemsToReadFromSocket = new();
    private readonly Func<IntPtr, byte[], int, SocketFlags, int>? sendFunc;

    public DirectOSNetworkingStub(Func<int>? getLastErrorCallback,
        Func<IntPtr, byte[], int, SocketFlags, int>? sendFunc)
    {
        this.getLastErrorCallback = getLastErrorCallback;
        this.sendFunc = sendFunc;
    }

    public int NumberQueuedMessages => itemsToReadFromSocket.Count;

    public int Select(int ignoredParameter, IntPtr[] readfds, IntPtr[]? writefds, IntPtr[]? exceptfds,
        ref TimeValue timeout) =>
        throw new NotImplementedException();

    public int GetLastCallError() => getLastErrorCallback!.Invoke();

    public unsafe int Recv(IntPtr socketHandle, byte* pinnedBuffer, int len, ref bool partialMsg)
    {
        var pinnedByte = *pinnedBuffer;
        var bytesToWrite = itemsToReadFromSocket.Dequeue();
        if (bytesToWrite?.BytesToReadFromSocket == null || !bytesToWrite.BytesToReadFromSocket.Any())
            return bytesToWrite?.BytesToReadFromSocket == null ? -1 : 0;
        var bytesRead = 0;
        for (var i = bytesToWrite.NumStartBytesToWrite
             ; i < bytesToWrite.EndBytesToWrite
               && i - bytesToWrite.NumStartBytesToWrite < len
             ; i++)
        {
            *pinnedBuffer = bytesToWrite.BytesToReadFromSocket[i];
            pinnedBuffer++;
            bytesRead++;
        }

        partialMsg = bytesToWrite.BytesToReadFromSocket.Length - 1 ==
                     bytesToWrite.EndBytesToWrite - bytesToWrite.NumStartBytesToWrite;
        return bytesRead;
    }

    public int IoCtlSocket(IntPtr socketHandle, ref int arg)
    {
        try
        {
            var bytesToWrite = itemsToReadFromSocket.Peek();
            arg = 0;
            if (bytesToWrite.BytesToReadFromSocket == null) return 0;

            arg = itemsToReadFromSocket.Where(msg => msg?.BytesToReadFromSocket != null)
                .Sum(msg => msg.BytesToReadFromSocket?.Length ?? 0);
            return 0;
        }
        catch
        {
            // ignored
        }

        return -1;
    }


    public unsafe int Send(IntPtr socketHandle, byte* pinnedBuffer, int len, SocketFlags socketFlags)
    {
        var copiedArray = new byte[len];
        for (var i = 0; i < len; i++) copiedArray[i] = *(pinnedBuffer + i);
        return sendFunc!(socketHandle, copiedArray, len, socketFlags);
    }

    public void QueueResponseBytes(byte[]? responseBytes, bool fullWrite)
    {
        if (responseBytes?.Length == 0)
        {
            itemsToReadFromSocket.Enqueue(new SocketDataRead(responseBytes, 0, 0));
            return;
        }

        if (fullWrite)
        {
            itemsToReadFromSocket.Enqueue(new SocketDataRead(responseBytes, 0, responseBytes?.Length ?? 0));
        }
        else
        {
            var halfMarker = responseBytes?.Length / 2 ?? 0;
            if (halfMarker <= 0)
            {
                itemsToReadFromSocket.Enqueue(new SocketDataRead(responseBytes, 0, responseBytes?.Length ?? 0));
                return;
            }

            itemsToReadFromSocket.Enqueue(new SocketDataRead(responseBytes, 0, halfMarker));
            itemsToReadFromSocket.Enqueue(new SocketDataRead(responseBytes, halfMarker, responseBytes?.Length ?? 0));
        }
    }

    public void ClearQueue()
    {
        itemsToReadFromSocket.Clear();
    }

    private class SocketDataRead
    {
        public SocketDataRead(byte[]? bytesToReadFromSocket, int numStartBytesToWrite, int endBytesToWrite)
        {
            BytesToReadFromSocket = bytesToReadFromSocket;
            NumStartBytesToWrite = numStartBytesToWrite;
            EndBytesToWrite = endBytesToWrite;
        }

        public byte[]? BytesToReadFromSocket { get; }
        public int NumStartBytesToWrite { get; }
        public int EndBytesToWrite { get; }
    }
}
