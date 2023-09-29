namespace FortitudeIO.Transports.NewSocketAPI.Dispatcher
{
    public interface ISocketDispatcherCommon
    {
        void Start();
        void Stop();
        int UsageCount { get; }
        string DispatcherDescription { get; set; }
    }
}