namespace FortitudeIO.Transports.Sockets.Dispatcher;

public interface ISocketDispatcher : ISocketDispatcherCommon
{
    ISocketDispatcherListener Listener { get; set; }
    ISocketDispatcherSender Sender { get; set; }
}

public class SocketDispatcher(ISocketDispatcherListener listener, ISocketDispatcherSender sender)
    : ISocketDispatcher
{
    public void Start()
    {
        Listener.Start();
        Sender.Start();
    }

    public void Stop()
    {
        Listener.Stop();
        Sender.Stop();
    }

    public int UsageCount => Math.Max(Listener.UsageCount, Sender.UsageCount);

    public string DispatcherDescription
    {
        get => Listener.DispatcherDescription;
        set
        {
            Listener.DispatcherDescription = value;
            Sender.DispatcherDescription = value;
        }
    }

    public ISocketDispatcherListener Listener { get; set; } = listener;

    public ISocketDispatcherSender Sender { get; set; } = sender;
}
