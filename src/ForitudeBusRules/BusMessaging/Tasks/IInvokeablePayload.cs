#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Tasks;

public interface IInvokeablePayload : IRecyclableObject
{
    bool IsAsyncInvoke { get; }
    void Invoke();
    ValueTask InvokeAsync();
}
