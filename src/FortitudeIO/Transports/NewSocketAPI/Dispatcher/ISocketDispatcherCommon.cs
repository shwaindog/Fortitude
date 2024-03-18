namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher;

public interface ISocketDispatcherCommon
{
    int UsageCount { get; }
    string Name { get; set; }
    void Start(Action? threadInitializer = null);
    void Stop();
}
