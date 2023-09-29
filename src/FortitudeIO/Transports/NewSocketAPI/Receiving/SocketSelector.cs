using System;
using System.Collections.Generic;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.Sockets.Logging;

namespace FortitudeIO.Transports.NewSocketAPI.Receiving
{
    public interface ISocketSelector
    {
        void Register(ISocketReceiver receiver);
        void Unregister(ISocketReceiver receiver);
        int CountRegisteredReceivers { get; }
        bool HasRegisteredReceiver(ISocketReceiver receiver);
        DateTime WakeTs { get; }
        IEnumerable<ISocketReceiver> WatchSocketsForRecv(IPerfLogger socketReadTraceLogger = null);
    }

    public sealed class SocketSelector : ISocketSelector
    {
        private readonly object sync = new object();
        private IDictionary<IntPtr, ISocketReceiver> allRegisteredSocketsDict = 
            new Dictionary<IntPtr, ISocketReceiver>();
        private TimeValue timeout;
        private IntPtr[] allRegisteredSocketHandles = { (IntPtr)0 };
        private readonly IDirectOSNetworkingApi directOSNetworkingApi;

        private uint roll;
        private IntPtr[] rollingFds = { (IntPtr)0 };

        public SocketSelector(int timeoutMs, IOSNetworkingController networkingController)
        {
            timeout = new TimeValue(timeoutMs);
            directOSNetworkingApi = networkingController.DirectOSNetworkingApi;
        }

        public DateTime WakeTs { get; private set; }

        public int CountRegisteredReceivers => allRegisteredSocketsDict.Count;
        public bool HasRegisteredReceiver(ISocketReceiver receiver)
        {
            return allRegisteredSocketsDict.ContainsKey(receiver.SocketHandle);
        }

        public void Register(ISocketReceiver receiver)
        {
            lock (sync)
            {
                var tmp = new Dictionary<IntPtr, ISocketReceiver>(allRegisteredSocketsDict)
                    {
                        {receiver.SocketHandle, receiver}
                    };
                allRegisteredSocketHandles = ToIntPtrArray(new List<ISocketReceiver>(
                    (allRegisteredSocketsDict = tmp).Values));
            }
        }

        public void Unregister(ISocketReceiver receiver)
        {
            lock (sync)
            {
                var tmp = new Dictionary<IntPtr, ISocketReceiver>(allRegisteredSocketsDict);
                tmp.Remove(receiver.SocketHandle);
                allRegisteredSocketHandles = ToIntPtrArray(new List<ISocketReceiver>(
                    (allRegisteredSocketsDict = tmp).Values));
            }
        }

        public IEnumerable<ISocketReceiver> WatchSocketsForRecv(
            IPerfLogger socketReadTraceLogger = null)
        {
            var fds = ShiftOrderOfHandles(allRegisteredSocketHandles);
            if (fds.Length <= 1)
            {
                throw new Exception("Select called on empty socket list");
            }
            if (socketReadTraceLogger != null)
            {
                socketReadTraceLogger.Start();
                socketReadTraceLogger.Add(SocketDataLatencyLogger.StartDataDetection);
            }
            if (directOSNetworkingApi.Select(0, fds, null, null, ref timeout) == -1)
            {
                throw new Exception("Win32 error " + directOSNetworkingApi.GetLastWin32Error() + " on select call");
            }
            socketReadTraceLogger?.Add(SocketDataLatencyLogger.SocketDataDetected);
            WakeTs = TimeContext.UtcNow;
            int length = (int)fds[0];
            for (int i = 0; i < length; i++)
            {
                ISocketReceiver sessionConnection;
                if (allRegisteredSocketsDict.TryGetValue(fds[i + 1], out sessionConnection))
                {
                    yield return sessionConnection;
                }
            }
        }

        private unsafe IntPtr[] ShiftOrderOfHandles(IntPtr[] fds)
        {
            if (fds.Length != rollingFds.Length)
            {
                rollingFds = new IntPtr[fds.Length];
            }
            uint count = (uint)(rollingFds.Length - 1);
            fixed (IntPtr* fsrc = fds)
            {
                fixed (IntPtr* fdst = rollingFds)
                {
                    IntPtr* src = fsrc;
                    IntPtr* dst = fdst;
                    *dst++ = *src++;
                    for (uint i = 0; i < count; i++)
                    {
                        *dst++ = *(src + ((roll + i) % count));
                    }
                }
            }
            roll++;
            return rollingFds;
        }

        private IntPtr[] ToIntPtrArray(List<ISocketReceiver> cxs)
        {
            IntPtr[] set = new IntPtr[cxs.Count + 1];
            set[0] = (IntPtr)cxs.Count;
            for (int i = 1; i < set.Length; i++)
            {
                set[i] = cxs[i - 1].SocketHandle;
            }
            return set;
        }
    }
}
