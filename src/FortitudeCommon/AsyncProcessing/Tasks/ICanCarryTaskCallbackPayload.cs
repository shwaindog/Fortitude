namespace FortitudeCommon.AsyncProcessing.Tasks;

public interface ICanCarryTaskCallbackPayload
{
    bool IsTaskCallbackItem { get; }
    void SetAsTaskCallbackItem(SendOrPostCallback callback, object? state);
    void InvokeTaskCallback();
}
