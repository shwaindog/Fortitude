// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.Channel;

public interface IChannel
{
    bool IsOpen        { get; }
    bool ReceiverAlive { get; }

    Type EventType { get; }

    ValueTask<bool> PublishComplete(IRule sender);
    ValueTask<bool> Close(IRule caller);
}

public interface IChannel<in TEvent> : IChannel
{
    ValueTask<bool> Publish(IRule sender, TEvent toPublish);
    ValueTask<bool> Publish(IRule sender, IEnumerable<TEvent> batchToPublish);
}

public static class ChannelExtensions
{
    public static bool IsFactoryChannel(this IChannel checkChannel) => checkChannel is IRecycler;

    public static IChannelEventFactory<TEvent>? AsFactoryChannel<TEvent>(this IChannel checkChannel) where TEvent : class, new() =>
        checkChannel as IChannelEventFactory<TEvent>;

    public static Func<TEvent>? GetChannelEventOrNew<TEvent>(this IChannel checkChannel) where TEvent : class, new() =>
        checkChannel.AsFactoryChannel<TEvent>()?.SourceEvent;
}
