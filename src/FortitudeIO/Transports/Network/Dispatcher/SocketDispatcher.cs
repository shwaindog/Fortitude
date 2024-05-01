namespace FortitudeIO.Transports.Network.Dispatcher;

public interface ISocketDispatcher : ISocketDispatcherCommon
{
    ISocketDispatcherListener? Listener { get; set; }
    ISocketDispatcherSender? Sender { get; set; }
}

public class SocketDispatcher : ISocketDispatcher
{
    public SocketDispatcher(ISocketDispatcherListener? listener, ISocketDispatcherSender? sender)
    {
        Listener = listener;
        Sender = sender;
    }

    public void Start(Action? threadInitialization)
    {
        Listener?.Start();
        Sender?.Start();
    }

    public void Stop()
    {
        Listener?.Stop();
        Sender?.Stop();
    }

    public void StopImmediate()
    {
        Listener?.StopImmediate();
        Sender?.StopImmediate();
    }

    public int UsageCount => Math.Max(Listener?.UsageCount ?? 0, Sender?.UsageCount ?? 0);

    public string Name
    {
        get => Listener?.Name ?? Sender?.Name ?? "No Listener or Sender";
        set
        {
            if (Listener != null) Listener.Name = value;
            if (Sender != null) Sender.Name = value;
        }
    }

    public ISocketDispatcherListener? Listener { get; set; }

    public ISocketDispatcherSender? Sender { get; set; }

    public override string ToString() => $"SocketDispatcher({nameof(Name)}: {Name})";
}
