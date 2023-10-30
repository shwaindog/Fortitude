namespace FortitudeIO.Transports.Sockets.Dispatcher;

public interface ISocketDispatcherCommon
{
    int UsageCount { get; }
    string DispatcherDescription { get; set; }
    void Start();
    void Stop();
}
