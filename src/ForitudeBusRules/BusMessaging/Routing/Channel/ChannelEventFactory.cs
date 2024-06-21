// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.Channel;

public interface IChannelEventFactory<TEvent> : IChannel<TEvent>, IRecycler
    where TEvent : class, new()
{
    Func<TEvent> SourceEvent { get; }
}

public class ChannelWrappingEventFactory<TEvent> : IChannelEventFactory<TEvent> where TEvent : class, new()
{
    protected readonly IChannel<TEvent> BackingChannel;
    protected readonly IRecycler        BackingRecycler;

    public ChannelWrappingEventFactory(IChannel<TEvent> backingChannel, IRecycler? backingRecycler = null)
    {
        BackingChannel  = backingChannel;
        BackingRecycler = backingRecycler ?? new Recycler();
    }

    public bool IsOpen        => BackingChannel.IsOpen;
    public bool ReceiverAlive => BackingChannel.ReceiverAlive;

    public Type EventType => BackingChannel.EventType;

    public ValueTask<bool> Publish(IRule sender, TEvent toPublish) => BackingChannel.Publish(sender, toPublish);

    public ValueTask<bool> Publish(IRule sender, IEnumerable<TEvent> batchToPublish) => BackingChannel.Publish(sender, batchToPublish);

    public ValueTask<bool> PublishComplete(IRule sender) => BackingChannel.PublishComplete(sender);

    public ValueTask<bool> Close(IRule caller) => BackingChannel.Close(caller);

    public T Borrow<T>() where T : class, new() => BackingRecycler.Borrow<T>();

    public object Borrow(Type type) => BackingRecycler.Borrow(type);

    public void Recycle(object trash) => BackingRecycler.Recycle(trash);

    public Func<TEvent> SourceEvent => Borrow<TEvent>;
}

public interface IChannelLimitedEventFactory<TEvent> : IChannelEventFactory<TEvent>, ILimitedRecycler
    where TEvent : class, new()
{
    Func<TEvent?> TrySourceEvent { get; }
}

public class ChannelWrappingLimitedEventFactory<TEvent> : ChannelWrappingEventFactory<TEvent>, IChannelLimitedEventFactory<TEvent>
    where TEvent : class, new()
{
    public ChannelWrappingLimitedEventFactory
        (IChannel<TEvent> backingChannel, ILimitedRecycler limitedRecycler) : base(backingChannel, limitedRecycler) { }

    private ILimitedRecycler LimitedRecycler => (ILimitedRecycler)BackingRecycler;

    public int MaxTypeBorrowLimit => LimitedRecycler.MaxTypeBorrowLimit;

    public T? TryBorrow<T>() where T : class, new() => LimitedRecycler.TryBorrow<T>();

    public object? TryBorrow(Type type) => LimitedRecycler.TryBorrow(type);

    public Func<TEvent?> TrySourceEvent => TryBorrow<TEvent>;
}
