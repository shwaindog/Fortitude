// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeBusRules.BusMessaging.Routing.Channel;

public struct ChannelEvent<TEvent>
{
    private TEvent? eventItem;

    public ChannelEvent(int channelId, int sequenceNumber)
    {
        ChannelId      = channelId;
        SequenceNumber = sequenceNumber;
    }

    public ChannelEvent(int channelId, TEvent eventItem, int sequenceNumber)
    {
        ChannelId      = channelId;
        SequenceNumber = sequenceNumber;
        Event          = eventItem;
    }

    public int  ChannelId      { get; }
    public int  SequenceNumber { get; set; }
    public bool IsLastEvent    => eventItem == null;
    public TEvent Event
    {
        readonly get => eventItem!;
        set => eventItem = value;
    }
}

public struct ChannelBatchedEvents<TEvent>
{
    private IEnumerable<TEvent>? events;

    public ChannelBatchedEvents(int channelId) => ChannelId = channelId;

    public ChannelBatchedEvents(int channelId, IEnumerable<TEvent> events)
    {
        ChannelId = channelId;
        Events    = events;
    }

    public int ChannelId { get; }

    public bool IsLastBatchEvent => events == null;

    public IEnumerable<TEvent> Events
    {
        readonly get => events!;
        set => events = value;
    }
}
