#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;

public interface ISubscription : IRecyclableObject, IAsyncValueTaskDisposable
{
    void Unsubscribe();

    ValueTask UnsubscribeAsync();
}
