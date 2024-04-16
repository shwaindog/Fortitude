#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.MessageBus.Tasks;

public interface IInvokeablePayload : IRecyclableObject
{
    void Invoke();
}
