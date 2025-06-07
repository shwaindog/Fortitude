// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents;

[Flags]
public enum FeedEventFieldUpdatedFlags : uint
{
    None                                      = 0x00_00_00_00
  , SourceTimeDateUpdatedFlag                 = 0x00_00_00_01
  , SourceSub2MinTimeUpdatedFlag              = 0x00_00_00_02
  , LastSourceFeedSentTimeDateUpdatedFlag     = 0x00_00_00_01
  , LastSourceFeedSentSub2MinTimeUpdatedFlag  = 0x00_00_00_02
  , LastAdapterReceivedTimeDateUpdatedFlag    = 0x00_00_00_04
  , LastAdapterReceivedSub2TimeMinUpdatedFlag = 0x00_00_00_08
  , AdapterSentTimeDateUpdatedFlag            = 0x00_00_00_10
  , AdapterSentSub2MinTimeUpdatedFlag         = 0x00_00_00_20
  , DownstreamTimeDateUpdatedFlag             = 0x00_00_00_40
  , DownstreamSub2MinTimeUpdatedFlag          = 0x00_00_00_80
  , SourceSequenceNumberUpdatedFlag           = 0x00_00_01_00
  , AdapterSequenceNumberUpdatedFlag          = 0x00_00_02_00
  , ClientSequenceNumberUpdatedFlag           = 0x00_00_04_00
  , FeedSequenceNumberUpdatedFlag             = 0x00_00_08_00
  , FeedMarketConnectivityStatusUpdatedFlag   = 0x00_00_10_00
  , FeedEventUpdateTypeUpdatedFlag            = 0x00_00_20_00
}
