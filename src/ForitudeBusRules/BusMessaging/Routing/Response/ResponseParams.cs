// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Messages;

#endregion

namespace FortitudeBusRules.BusMessaging.Routing.Response;

public enum ResponsePublishMethod
{
    ListenerDefaultBroadcastAddress
  , ReceiverChannel
  , AlternativeBroadcastAddress
}

public interface IResponsePublishParams
{
    ResponsePublishMethod   ResponsePublishMethod     { get; }
    IChannelPublishRequest? ChannelRequest            { get; }
    DispatchOptions         PublishDispatchOptions    { get; set; }
    string?                 AlternativePublishAddress { get; set; }
}

public struct ResponsePublishParams : IResponsePublishParams
{
    private readonly int maxInflightEvents = -1;

    public ResponsePublishParams()
    {
        ResponsePublishMethod  = ResponsePublishMethod.ListenerDefaultBroadcastAddress;
        PublishDispatchOptions = new DispatchOptions();
    }

    public ResponsePublishParams(IChannelPublishRequest? channelRequest)
    {
        ResponsePublishMethod  = ResponsePublishMethod.ReceiverChannel;
        ChannelRequest         = channelRequest;
        PublishDispatchOptions = new DispatchOptions();
    }

    public ResponsePublishParams
        (string? alternativePublishAddress, DispatchOptions? dispatchOptions = null)
    {
        ResponsePublishMethod     = ResponsePublishMethod.AlternativeBroadcastAddress;
        AlternativePublishAddress = alternativePublishAddress;
        PublishDispatchOptions    = dispatchOptions ?? new DispatchOptions();
    }

    public string?                 AlternativePublishAddress { get; set; }
    public DispatchOptions         PublishDispatchOptions    { get; set; }
    public ResponsePublishMethod   ResponsePublishMethod     { get; }
    public IChannelPublishRequest? ChannelRequest            { get; }

    public readonly int MaxInflightEvents => ChannelRequest?.Channel.MaxInflight ?? maxInflightEvents;
}
