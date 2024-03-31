namespace FortitudeIO.Transports.Network.Dispatcher;

public interface ISocketDispatcher : ISocketDispatcherCommon
{
    ISocketDispatcherListener Listener { get; set; }
    ISocketDispatcherSender Sender { get; set; }
}

public class SocketDispatcher : ISocketDispatcher
{
    public SocketDispatcher(ISocketDispatcherListener listener, ISocketDispatcherSender sender)
    {
        Listener = listener;
        Sender = sender;
    }

    public void Start(Action? threadInitialization)
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

    public string Name
    {
        get => Listener.Name;
        set
        {
            Listener.Name = value;
            Sender.Name = value;
        }
    }

    public ISocketDispatcherListener Listener { get; set; }

    public ISocketDispatcherSender Sender { get; set; }

    public override string ToString() => $"SocketDispatcher({nameof(Name)}: {Name})";
}
