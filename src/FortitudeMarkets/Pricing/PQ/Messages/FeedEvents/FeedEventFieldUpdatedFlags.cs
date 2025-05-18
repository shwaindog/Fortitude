// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents;

[Flags]
public enum FeedEventFieldUpdatedFlags : uint
{
    None                                      = 0x00_00_00_00
  , FeedSyncStatusFlag                        = 0x00_00_00_01
  , IsSourceReplayUpdatedFlag                 = 0x00_00_00_02
  , IsAdapterReplayUpdatedFlag                = 0x00_00_00_04
  , IsFromSourceSnapshotUpdatedFlag           = 0x00_00_00_08
  , IsFromAdapterSnapshotUpdatedFlag          = 0x00_00_00_10
  , IsFromStorageUpdatedFlag                  = 0x00_00_00_20
  , LastSourceFeedSentDateUpdatedFlag         = 0x00_00_00_40
  , LastSourceFeedSentSub2MinUpdatedFlag      = 0x00_00_00_80
  , SocketReceivedDateUpdatedFlag             = 0x00_00_01_00
  , SocketReceivedSub2MinUpdatedFlag          = 0x00_00_02_00
  , ProcessedDateUpdatedFlag                  = 0x00_00_04_00
  , ProcessedSub2MinUpdatedFlag               = 0x00_00_08_00
  , DispatchedDateUpdatedFlag                 = 0x00_00_10_00
  , DispatchedSub2MinUpdatedFlag              = 0x00_00_20_00
  , ClientReceivedDateUpdatedFlag             = 0x00_00_40_00
  , ClientReceivedSub2MinUpdatedFlag          = 0x00_00_80_00
  , LastAdapterReceivedTimeDateUpdatedFlag    = 0x00_01_00_00
  , LastAdapterReceivedTimeSub2MinUpdatedFlag = 0x00_02_00_00
  , AdapterSentTimeDateUpdatedFlag            = 0x00_04_00_00
  , AdapterSentTimeSub2MinUpdatedFlag         = 0x00_08_00_00
  , DownstreamDateUpdatedFlag                 = 0x00_10_00_00
  , DownstreamSub2MinTimeUpdatedFlag          = 0x00_20_00_00
  , SourceSequenceNumberUpdatedFlag           = 0x00_40_00_00
  , AdapterSequenceNumberUpdatedFlag          = 0x00_80_00_00
  , ClientSequenceNumberUpdatedFlag           = 0x01_00_00_00
  , FeedSequenceNumberUpdatedFlag             = 0x02_00_00_00
  , FeedMarketConnectivityStatusUpdatedFlag   = 0x04_00_00_00
  , FeedEventUpdateTypeUpdatedFlag            = 0x08_00_00_00
}
