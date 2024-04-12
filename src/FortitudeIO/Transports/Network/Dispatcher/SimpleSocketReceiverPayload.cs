#region

using FortitudeIO.Transports.Network.Receiving;

#endregion

namespace FortitudeIO.Transports.Network.Dispatcher;

public class SimpleSocketReceiverPayload : ICanCarrySocketReceiverPayload
{
    private SendOrPostCallback? sendOrPostCallback;
    private object? state;
    public AsyncTaskSocketReceiverCommand SocketReceiverCommand { get; set; } = AsyncTaskSocketReceiverCommand.Add;
    public ISocketReceiver? SocketReceiver { get; set; }

    public bool IsTaskCallbackItem => SocketReceiverCommand == AsyncTaskSocketReceiverCommand.TaskCallback;

    public void SetAsTaskCallbackItem(SendOrPostCallback callback, object? state)
    {
        SocketReceiverCommand = AsyncTaskSocketReceiverCommand.TaskCallback;
        sendOrPostCallback = callback;
        this.state = state;
        SocketReceiver = null;
    }

    public void InvokeTaskCallback()
    {
        sendOrPostCallback?.Invoke(state);
        SocketReceiverCommand = AsyncTaskSocketReceiverCommand.NotSet;
    }

    public bool IsSocketReceiverItem => SocketReceiverCommand is AsyncTaskSocketReceiverCommand.Add or AsyncTaskSocketReceiverCommand.Remove;

    public bool IsSocketAdd => SocketReceiverCommand == AsyncTaskSocketReceiverCommand.Add;

    public void SetAsSocketReceiverItem(ISocketReceiver socketReceiver, bool isAdd)
    {
        SocketReceiver = socketReceiver;
        SocketReceiverCommand = isAdd ? AsyncTaskSocketReceiverCommand.Add : AsyncTaskSocketReceiverCommand.Remove;
        sendOrPostCallback = null;
        state = null;
    }
}
