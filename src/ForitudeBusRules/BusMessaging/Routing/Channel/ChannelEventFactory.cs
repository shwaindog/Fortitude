// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.Rules;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.Channel;

public interface IChannelEventFactory<TEvent> : IChannel<TEvent>, IRecycler
    where TEvent : class
{
    Func<TEvent> SourceEvent { get; }
}

public class ChannelWrappingEventFactory<TEvent> : ReusableObject<IChannel>, IChannelEventFactory<TEvent> where TEvent : class, new()
{
    protected IChannel<TEvent> BackingChannel  = null!;
    protected IRecycler        BackingRecycler = null!;

    public ChannelWrappingEventFactory() { }

    public ChannelWrappingEventFactory(IChannel<TEvent> backingChannel, IRecycler? backingRecycler = null)
    {
        BackingChannel  = backingChannel;
        BackingRecycler = backingRecycler ?? new Recycler();
    }

    public ChannelWrappingEventFactory(ChannelWrappingEventFactory<TEvent> toClone)
    {
        BackingChannel  = toClone.BackingChannel;
        BackingRecycler = toClone.BackingRecycler;
    }

    public int Id => BackingChannel.Id;

    public bool IsOpen        => BackingChannel.IsOpen;
    public bool ReceiverAlive => BackingChannel.ReceiverAlive;

    public virtual int MaxInflight { get; set; }

    public Type EventType => BackingChannel.EventType;

    public ValueTask<bool> Publish(IRule sender, TEvent toPublish) => BackingChannel.Publish(sender, toPublish);

    public ValueTask<bool> Publish(IRule sender, IEnumerable<TEvent> batchToPublish) => BackingChannel.Publish(sender, batchToPublish);

    public ValueTask<bool> PublishComplete(IRule sender) => BackingChannel.PublishComplete(sender);

    public ValueTask<bool> Close(IRule caller) => BackingChannel.Close(caller);

    public T Borrow<T>() where T : class, new() => BackingRecycler.Borrow<T>();

    public object Borrow(Type type) => BackingRecycler.Borrow(type);

    public void Recycle(object trash) => BackingRecycler.Recycle(trash);

    public Func<TEvent> SourceEvent => Borrow<TEvent>;

    public override IChannel CopyFrom(IChannel source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ChannelWrappingEventFactory<TEvent> channelFactory)
        {
            BackingChannel  = channelFactory.BackingChannel;
            BackingRecycler = channelFactory.BackingRecycler;
        }
        return this;
    }

    public override IChannel Clone() =>
        Recycler?.Borrow<ChannelWrappingEventFactory<TEvent>>().CopyFrom(this) ?? new ChannelWrappingEventFactory<TEvent>(this);

    public ChannelWrappingEventFactory<TEvent> Configure(IChannel<TEvent> backingChannel, IRecycler? backingRecycler = null)
    {
        BackingChannel  = backingChannel;
        BackingRecycler = backingRecycler ?? new Recycler();
        return this;
    }
}

public class ChannelWrappingEventFactory<TEvent, TConcrete> : ReusableObject<IChannel>, IChannelEventFactory<TEvent>
    where TEvent : class where TConcrete : class, TEvent, new()
{
    protected IChannel<TEvent> BackingChannel  = null!;
    protected IRecycler        BackingRecycler = null!;

    public ChannelWrappingEventFactory() { }

    public ChannelWrappingEventFactory(IChannel<TEvent> backingChannel, IRecycler? backingRecycler = null)
    {
        BackingChannel  = backingChannel;
        BackingRecycler = backingRecycler ?? new Recycler();
    }

    public ChannelWrappingEventFactory(ChannelWrappingEventFactory<TEvent, TConcrete> toClone)
    {
        BackingChannel  = toClone.BackingChannel;
        BackingRecycler = toClone.BackingRecycler;
    }

    public int  Id            => BackingChannel.Id;
    public bool IsOpen        => BackingChannel.IsOpen;
    public bool ReceiverAlive => BackingChannel.ReceiverAlive;

    public virtual int MaxInflight { get; set; }

    public Type EventType => BackingChannel.EventType;

    public ValueTask<bool> Publish(IRule sender, TEvent toPublish) => BackingChannel.Publish(sender, toPublish);

    public ValueTask<bool> Publish(IRule sender, IEnumerable<TEvent> batchToPublish) => BackingChannel.Publish(sender, batchToPublish);

    public ValueTask<bool> PublishComplete(IRule sender) => BackingChannel.PublishComplete(sender);

    public ValueTask<bool> Close(IRule caller) => BackingChannel.Close(caller);

    public T Borrow<T>() where T : class, new() => BackingRecycler.Borrow<T>();

    public object Borrow(Type type) => BackingRecycler.Borrow(type);

    public void Recycle(object trash) => BackingRecycler.Recycle(trash);

    public Func<TEvent> SourceEvent => Borrow<TConcrete>;

    public override IChannel CopyFrom(IChannel source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is ChannelWrappingEventFactory<TEvent, TConcrete> channelFactory)
        {
            BackingChannel  = channelFactory.BackingChannel;
            BackingRecycler = channelFactory.BackingRecycler;
        }
        return this;
    }

    public override IChannel Clone() =>
        Recycler?.Borrow<ChannelWrappingEventFactory<TEvent, TConcrete>>().CopyFrom(this) ?? new ChannelWrappingEventFactory<TEvent, TConcrete>(this);

    public ChannelWrappingEventFactory<TEvent, TConcrete> Configure(IChannel<TEvent> backingChannel, IRecycler? backingRecycler = null)
    {
        BackingChannel  = backingChannel;
        BackingRecycler = backingRecycler ?? new Recycler();
        return this;
    }
}

public interface IChannelLimitedEventFactory<TEvent> : IChannelEventFactory<TEvent>, ILimitedRecycler
    where TEvent : class
{
    Func<TEvent?> TrySourceEvent { get; }
}

public class ChannelWrappingLimitedEventFactory<TEvent> : ChannelWrappingEventFactory<TEvent>, IChannelLimitedEventFactory<TEvent>
    where TEvent : class, new()
{
    public ChannelWrappingLimitedEventFactory() { }

    public ChannelWrappingLimitedEventFactory
        (IChannel<TEvent> backingChannel, ILimitedRecycler limitedRecycler) : base(backingChannel, limitedRecycler) { }


    public ChannelWrappingLimitedEventFactory(ChannelWrappingEventFactory<TEvent> toClone) : base(toClone) { }

    private ILimitedRecycler LimitedRecycler => (ILimitedRecycler)BackingRecycler;

    public int MaxTypeBorrowLimit
    {
        get => LimitedRecycler.MaxTypeBorrowLimit;
        set => LimitedRecycler.MaxTypeBorrowLimit = value;
    }

    public override int MaxInflight
    {
        get => LimitedRecycler.MaxTypeBorrowLimit;
        set => LimitedRecycler.MaxTypeBorrowLimit = value;
    }

    public T? TryBorrow<T>() where T : class, new() => LimitedRecycler.TryBorrow<T>();

    public object? TryBorrow(Type type) => LimitedRecycler.TryBorrow(type);

    public Func<TEvent?> TrySourceEvent => TryBorrow<TEvent>;

    public override IChannel Clone() =>
        Recycler?.Borrow<ChannelWrappingLimitedEventFactory<TEvent>>().CopyFrom(this) ?? new ChannelWrappingLimitedEventFactory<TEvent>(this);

    public new ChannelWrappingLimitedEventFactory<TEvent> Configure(IChannel<TEvent> backingChannel, IRecycler? backingRecycler = null)
    {
        BackingChannel  = backingChannel;
        BackingRecycler = backingRecycler ?? new Recycler();
        return this;
    }
}

public class ChannelWrappingLimitedEventFactory<TEvent, TConcrete> : ChannelWrappingEventFactory<TEvent, TConcrete>
  , IChannelLimitedEventFactory<TEvent>
    where TEvent : class where TConcrete : class, TEvent, new()
{
    public ChannelWrappingLimitedEventFactory() { }

    public ChannelWrappingLimitedEventFactory
        (IChannel<TEvent> backingChannel, ILimitedRecycler limitedRecycler) : base(backingChannel, limitedRecycler) { }

    public ChannelWrappingLimitedEventFactory(ChannelWrappingEventFactory<TEvent, TConcrete> toClone) : base(toClone) { }

    private ILimitedRecycler LimitedRecycler => (ILimitedRecycler)BackingRecycler;

    public int MaxTypeBorrowLimit
    {
        get => LimitedRecycler.MaxTypeBorrowLimit;
        set => LimitedRecycler.MaxTypeBorrowLimit = value;
    }
    public override int MaxInflight
    {
        get => LimitedRecycler.MaxTypeBorrowLimit;
        set => LimitedRecycler.MaxTypeBorrowLimit = value;
    }

    public T? TryBorrow<T>() where T : class, new() => LimitedRecycler.TryBorrow<T>();

    public object? TryBorrow(Type type) => LimitedRecycler.TryBorrow(type);

    public Func<TEvent?> TrySourceEvent => TryBorrow<TConcrete>;

    public override IChannel Clone() =>
        Recycler?.Borrow<ChannelWrappingLimitedEventFactory<TEvent, TConcrete>>().CopyFrom(this) ??
        new ChannelWrappingLimitedEventFactory<TEvent, TConcrete>(this);

    public new ChannelWrappingLimitedEventFactory<TEvent, TConcrete> Configure(IChannel<TEvent> backingChannel, IRecycler? backingRecycler = null)
    {
        BackingChannel  = backingChannel;
        BackingRecycler = backingRecycler ?? new Recycler();
        return this;
    }
}
