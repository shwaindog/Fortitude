// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

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

public static class SubscriptionExtensions
{
    public static async ValueTask NullSafeUnsubscribe(this ISubscription? subscription) =>
        await (subscription?.UnsubscribeAsync() ?? ValueTask.CompletedTask);
}
