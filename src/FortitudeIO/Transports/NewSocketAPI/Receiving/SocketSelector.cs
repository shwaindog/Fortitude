#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Transports.NewSocketAPI.Logging;

#endregion

namespace FortitudeIO.Transports.NewSocketAPI.Receiving;

public interface ISocketSelector
{
    int CountRegisteredReceivers { get; }
    DateTime WakeTs { get; }
    void Register(ISocketReceiver receiver);
    void Unregister(ISocketReceiver receiver);
    bool HasRegisteredReceiver(ISocketReceiver receiver);
    IEnumerable<ISocketReceiver> WatchSocketsForRecv(IPerfLogger? socketReadTraceLogger = null);
}

public sealed class SocketSelector : ISocketSelector
{
    private readonly IDirectOSNetworkingApi directOSNetworkingApi;
    private readonly object sync = new();
    private IntPtr[] allRegisteredSocketHandles = { 0 };

    private IDictionary<IntPtr, ISocketReceiver> allRegisteredSocketsDict =
        new Dictionary<IntPtr, ISocketReceiver>();

    private uint roll;
    private IntPtr[] rollingFds = { 0 };
    private TimeValue timeout;

    public SocketSelector(int timeoutMs, IOSNetworkingController networkingController)
    {
        timeout = new TimeValue(timeoutMs);
        directOSNetworkingApi = networkingController.DirectOSNetworkingApi;
    }

    public DateTime WakeTs { get; private set; }

    public int CountRegisteredReceivers => allRegisteredSocketsDict.Count;

    public bool HasRegisteredReceiver(ISocketReceiver receiver) =>
        allRegisteredSocketsDict.ContainsKey(receiver.SocketHandle);

    public void Register(ISocketReceiver receiver)
    {
        lock (sync)
        {
            var tmp = new Dictionary<IntPtr, ISocketReceiver>(allRegisteredSocketsDict)
            {
                { receiver.SocketHandle, receiver }
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
        IPerfLogger? socketReadTraceLogger = null)
    {
        var fds = ShiftOrderOfHandles(allRegisteredSocketHandles);
        if (fds.Length <= 1) throw new Exception("Select called on empty socket list");
        if (socketReadTraceLogger != null)
        {
            socketReadTraceLogger.Start();
            socketReadTraceLogger.Add(SocketDataLatencyLogger.StartDataDetection);
        }

        if (directOSNetworkingApi.Select(0, fds, null, null, ref timeout) == -1)
            throw new Exception("Win32 error " + directOSNetworkingApi.GetLastCallError() + " on select call");
        socketReadTraceLogger?.Add(SocketDataLatencyLogger.SocketDataDetected);
        WakeTs = TimeContext.UtcNow;
        var length = (int)fds[0];
        for (var i = 0; i < length; i++)
            if (allRegisteredSocketsDict.TryGetValue(fds[i + 1], out var sessionConnection))
                yield return sessionConnection;
    }

    private unsafe IntPtr[] ShiftOrderOfHandles(IntPtr[] fds)
    {
        if (fds.Length != rollingFds.Length) rollingFds = new IntPtr[fds.Length];
        var count = (uint)(rollingFds.Length - 1);
        fixed (IntPtr* fsrc = fds)
        {
            fixed (IntPtr* fdst = rollingFds)
            {
                var src = fsrc;
                var dst = fdst;
                *dst++ = *src++;
                for (uint i = 0; i < count; i++) *dst++ = *(src + (roll + i) % count);
            }
        }

        roll++;
        return rollingFds;
    }

    private static IntPtr[] ToIntPtrArray(List<ISocketReceiver> cxs)
    {
        var set = new IntPtr[cxs.Count + 1];
        set[0] = cxs.Count;
        for (var i = 1; i < set.Length; i++) set[i] = cxs[i - 1].SocketHandle;
        return set;
    }
}
