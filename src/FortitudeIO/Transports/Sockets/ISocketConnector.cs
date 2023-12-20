namespace FortitudeIO.Transports.Sockets;

public interface ISocketConnector
{
    bool IsConnected { get; }
    void Connect();
    void Disconnect();
}
