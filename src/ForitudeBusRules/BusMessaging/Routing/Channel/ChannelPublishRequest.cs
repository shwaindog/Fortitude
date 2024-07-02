// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.Channel;

public interface IChannelPublishRequest
{
    public int       BatchSize   { get; }
    public int       ResultLimit { get; }
    public IChannel  Channel     { get; }
    public FlowRate? FlowRate    { get; }
}

public readonly struct ChannelPublishRequest<TEvent> : IChannelPublishRequest
{
    public ChannelPublishRequest(IChannel<TEvent> publishChannel, int resultLimit = -1, int batchSize = 1, FlowRate? flowRate = null)
    {
        BatchSize      = batchSize;
        ResultLimit    = resultLimit;
        PublishChannel = publishChannel;
        FlowRate       = flowRate;
    }

    public int              BatchSize      { get; }
    public int              ResultLimit    { get; }
    public IChannel<TEvent> PublishChannel { get; }
    public FlowRate?        FlowRate       { get; }
    public IChannel         Channel        => PublishChannel;
}

public class ChannelRequest<TEvent> : IChannelPublishRequest
{
    public ChannelRequest(IChannel<TEvent> publishChannel, int resultLimit = -1, int batchSize = 1, FlowRate? flowRate = null)
    {
        BatchSize      = batchSize;
        ResultLimit    = resultLimit;
        PublishChannel = publishChannel;
        FlowRate       = flowRate;
    }

    public IChannel<TEvent> PublishChannel { get; }

    public int       BatchSize   { get; }
    public int       ResultLimit { get; }
    public FlowRate? FlowRate    { get; }
    public IChannel  Channel     => PublishChannel;
}
