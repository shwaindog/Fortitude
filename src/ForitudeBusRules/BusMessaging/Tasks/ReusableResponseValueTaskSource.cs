#region

using FortitudeBusRules.Messages;
using FortitudeCommon.AsyncProcessing.Tasks;

#endregion

namespace FortitudeBusRules.BusMessaging.Tasks;

public interface IResponseValueTaskSource<in T> : IReusableAsyncResponse<T>
{
    public IDispatchResult? DispatchResult { get; set; }
}

public class ReusableResponseValueTaskSource<T> : ReusableValueTaskSource<T>
    , IResponseValueTaskSource<T>
{
    public IDispatchResult? DispatchResult { get; set; }

    public override void StateReset()
    {
        DispatchResult = null;
        base.StateReset();
    }
}
