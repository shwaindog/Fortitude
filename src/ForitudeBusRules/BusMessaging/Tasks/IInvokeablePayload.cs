#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Tasks;

public interface IInvokeablePayload : IRecyclableObject
{
    void Invoke();
}
