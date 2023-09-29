using FortitudeIO.Transports.Sockets.SessionConnection;

namespace FortitudeIO.Transports.Sockets
{
    public interface ISocketLink
    {
        string SessionDescription { get; }
        void StartMessaging();
        void StopMessaging();
        void Unregister(ISocketSessionConnection sessionConnection);
    }
}