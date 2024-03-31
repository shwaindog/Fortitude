namespace FortitudeIO.Transports.Network.State;

public enum SocketSessionState
{
    New
    , Connecting
    , Connected
    , Reconnecting
    , Disconnecting
    , Disconnected
}
