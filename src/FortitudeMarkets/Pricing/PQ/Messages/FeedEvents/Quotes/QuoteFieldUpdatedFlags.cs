// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

[Flags]
public enum QuoteFieldUpdatedFlags : uint
{
    None                             = 0x00_00_00
  , SingleValueUpdatedFlag           = 0x00_00_01
  , SourceSentDateUpdatedFlag        = 0x00_00_02
  , SourceSentSub2MinUpdatedFlag     = 0x00_00_04
  , SourceAskTimeDateUpdatedFlag     = 0x00_00_08
  , SourceAskTimeSub2MinUpdatedFlag  = 0x00_00_10
  , SourceBidTimeDateUpdatedFlag     = 0x00_00_20
  , SourceBidTimeSub2MinUpdatedFlag  = 0x00_00_40
  , ValidFromDateUpdatedFlag         = 0x00_00_80
  , ValidFromTimeSub2MinUpdatedFlag  = 0x00_01_00
  , ValidToDateUpdatedFlag           = 0x00_02_00
  , ValidToSub2MinUpdatedFlag        = 0x00_04_00
  , BidTopPriceUpdatedFlag           = 0x00_08_00
  , AskTopPriceUpdatedFlag           = 0x00_10_00
  , ExecutableUpdatedFlag            = 0x00_20_00
  , BatchIdeUpdatedFlag              = 0x00_40_00
  , SourceQuoteReferenceUpdatedFlag  = 0x00_80_00
  , ValueDateUpdatedFlag             = 0x01_00_00
}

[Flags]
public enum PublishableQuoteFieldUpdatedFlags : uint
{
    None                                  = 0x00_00_00
  , FeedSyncStatusFlag                    = 0x00_00_01
  , IsSourceReplayUpdatedFlag             = 0x00_00_02
  , IsAdapterReplayUpdatedFlag            = 0x00_00_04
  , IsFromSourceSnapshotUpdatedFlag       = 0x00_00_08
  , IsFromAdapterSnapshotUpdatedFlag      = 0x00_00_10
  , IsFromStorageUpdatedFlag              = 0x00_00_20
  , SocketReceivedDateUpdatedFlag         = 0x00_00_40
  , SocketReceivedSub2MinUpdatedFlag      = 0x00_00_80
  , ProcessedDateUpdatedFlag              = 0x00_01_00
  , ProcessedSub2MinUpdatedFlag           = 0x00_02_00
  , DispatchedDateUpdatedFlag             = 0x00_04_00
  , DispatchedSub2MinUpdatedFlag          = 0x00_08_00
  , ClientReceivedDateUpdatedFlag         = 0x00_10_00
  , ClientReceivedSub2MinUpdatedFlag      = 0x00_20_00
}

public static class QuoteFieldUpdatedFlagsExtensions { }
