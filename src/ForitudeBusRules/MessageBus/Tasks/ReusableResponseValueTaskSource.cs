#region

using Fortitude.EventProcessing.BusRules.Messaging;

#endregion

namespace Fortitude.EventProcessing.BusRules.MessageBus.Tasks;

internal class ReusableResponseValueTaskSource<T> : ReusableValueTaskSource<RequestResponse<T>>
{
    public IDispatchResult? DispatchResult { get; set; }

    public override void Reset()
    {
        DispatchResult = null;
        base.Reset();
    }

    public void TrySetResult(T result)
    {
        TrySetResult(new RequestResponse<T>(DispatchResult, result));
    }

    public void SetResult(T result) => SetResult(new RequestResponse<T>(DispatchResult, result));
}
