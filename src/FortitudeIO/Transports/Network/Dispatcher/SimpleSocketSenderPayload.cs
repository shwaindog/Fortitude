#region

using FortitudeIO.Transports.Network.Publishing;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public class SimpleSocketSenderPayload : ICanCarrySocketSenderPayload
{
    private SendOrPostCallback? sendOrPostCallback;
    private object? state;
    public ISocketSender? SocketSender { get; set; }

    public bool IsSocketSenderItem => SocketSender != null;

    public bool IsTaskCallbackItem => sendOrPostCallback != null;

    public void SetAsTaskCallbackItem(SendOrPostCallback callback, object? state)
    {
        sendOrPostCallback = callback;
        this.state = state;
        SocketSender = null;
    }

    public void InvokeTaskCallback()
    {
        sendOrPostCallback?.Invoke(state);
    }

    public void SetAsSocketSenderItem(ISocketSender socketSender)
    {
        sendOrPostCallback = null;
        state = null;
        SocketSender = socketSender;
    }
}
