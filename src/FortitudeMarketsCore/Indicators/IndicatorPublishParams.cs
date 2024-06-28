// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Messages;

#endregion

namespace FortitudeMarketsCore.Indicators;

public enum PublishSelection
{
    DefaultIndicatorBroadcastAddress
  , ReceiverChannel
  , AlternativeAddress
}

public enum PersistOptions
{
    IndicatorDefault
  , PersistToStorage
  , NoPersist
}

public interface IIndicatorPublishParams
{
    PublishSelection        PublishSelection          { get; }
    PersistOptions          Persist                   { get; }
    IChannelPublishRequest? ChannelRequest            { get; }
    DispatchOptions         PublishDispatchOptions    { get; set; }
    string?                 AlternativePublishAddress { get; set; }
}

public struct IndicatorPublishParams : IIndicatorPublishParams
{
    public IndicatorPublishParams()
    {
        PublishSelection       = PublishSelection.DefaultIndicatorBroadcastAddress;
        Persist                = PersistOptions.IndicatorDefault;
        PublishDispatchOptions = new DispatchOptions();
    }

    public IndicatorPublishParams(IChannelPublishRequest? channelRequest, PersistOptions persistOptions = PersistOptions.IndicatorDefault)
    {
        PublishSelection       = PublishSelection.ReceiverChannel;
        ChannelRequest         = channelRequest;
        Persist                = persistOptions;
        PublishDispatchOptions = new DispatchOptions();
    }

    public IndicatorPublishParams
        (string? alternativePublishAddress, DispatchOptions? dispatchOptions = null, PersistOptions persistOptions = PersistOptions.IndicatorDefault)
    {
        PublishSelection          = PublishSelection.AlternativeAddress;
        AlternativePublishAddress = alternativePublishAddress;
        PublishDispatchOptions    = dispatchOptions ?? new DispatchOptions();
        Persist                   = persistOptions;
    }

    public string?          AlternativePublishAddress { get; set; }
    public DispatchOptions  PublishDispatchOptions    { get; set; }
    public PublishSelection PublishSelection          { get; }
    public PersistOptions   Persist                   { get; }

    public IChannelPublishRequest? ChannelRequest { get; }
}
