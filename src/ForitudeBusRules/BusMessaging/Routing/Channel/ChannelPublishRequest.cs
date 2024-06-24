// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeBusRules.BusMessaging.Routing.Channel;

public struct ChannelPublishRequest<TEvent>
{
    public ChannelPublishRequest(IChannel<TEvent> publishChannel, int resultLimit = -1, int batchSize = 1)
    {
        BatchSize      = batchSize;
        ResultLimit    = resultLimit;
        PublishChannel = publishChannel;
    }

    public int BatchSize   { get; }
    public int ResultLimit { get; }

    public IChannel<TEvent> PublishChannel { get; }
}
