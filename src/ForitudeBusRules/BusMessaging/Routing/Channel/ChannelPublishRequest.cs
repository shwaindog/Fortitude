// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeBusRules.BusMessaging.Routing.Channel;

public interface IChannelPublishRequest
{
    public int      BatchSize   { get; }
    public int      ResultLimit { get; }
    public IChannel Channel     { get; }
}

public readonly struct ChannelPublishRequest<TEvent> : IChannelPublishRequest
{
    public ChannelPublishRequest(IChannel<TEvent> publishChannel, int resultLimit = -1, int batchSize = 1)
    {
        BatchSize      = batchSize;
        ResultLimit    = resultLimit;
        PublishChannel = publishChannel;
    }

    public int              BatchSize      { get; }
    public int              ResultLimit    { get; }
    public IChannel<TEvent> PublishChannel { get; }
    public IChannel         Channel        => PublishChannel;
}

public class ChannelRequest<TEvent> : IChannelPublishRequest
{
    public ChannelRequest(IChannel<TEvent> publishChannel, int resultLimit = -1, int batchSize = 1)
    {
        BatchSize      = batchSize;
        ResultLimit    = resultLimit;
        PublishChannel = publishChannel;
    }

    public IChannel<TEvent> PublishChannel { get; }

    public int      BatchSize   { get; }
    public int      ResultLimit { get; }
    public IChannel Channel     => PublishChannel;
}
