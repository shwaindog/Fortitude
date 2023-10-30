#region

using System.Globalization;
using FortitudeCommon.OSWrapper.NetworkingWrappers;
using FortitudeIO.Protocols.Authentication;

#endregion

namespace FortitudeIO.Transports.Sockets.SessionConnection;

public class SocketSessionConnectionBase
{
    private static long idGen;
    protected readonly IDirectOSNetworkingApi DirectOSNetworkingApi;

    public SocketSessionConnectionBase(IOSSocket socket, IDirectOSNetworkingApi directOSNetworkingApi,
        string sessionDescription)
    {
        DirectOSNetworkingApi = directOSNetworkingApi;
        Socket = socket;
        Id = Interlocked.Increment(ref idGen);
        SessionDescription = sessionDescription;
        Socket = socket;
        Handle = socket.Handle;
    }

    public ISocketSessionConnection? Parent { get; set; }
    public bool Active { get; set; }
    public long Id { get; }
    public IOSSocket Socket { get; }
    public IntPtr Handle { get; }
    public string SessionDescription { get; }
    public IUserData? AuthData { get; set; }

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => Id.ToString(CultureInfo.InvariantCulture) + " " + SessionDescription;

    public override bool Equals(object? obj)
    {
        var sid = obj as SocketSessionConnectionBase;
        return sid != null && sid.Id == Id;
    }
}
