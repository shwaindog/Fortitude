// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

[Flags]
public enum QuoteFieldUpdatedFlags : uint
{
    None                            = 0x00_00_00
  , SingleValueUpdatedFlag          = 0x00_00_01
  , IsReplayUpdatedFlag             = 0x00_00_02
  , SourceSentDateUpdatedFlag       = 0x00_00_04
  , SourceSentSubHourUpdatedFlag    = 0x00_00_08
  , SourceAskTimeDateUpdatedFlag    = 0x00_00_10
  , SourceAskTimeSubHourUpdatedFlag = 0x00_00_20
  , SourceBidTimeDateUpdatedFlag    = 0x00_00_40
  , SourceBidTimeSubHourUpdatedFlag = 0x00_00_80
  , ValidFromDateUpdatedFlag        = 0x00_01_00
  , ValidFromTimeSubHourUpdatedFlag = 0x00_02_00
  , ValidToDateUpdatedFlag          = 0x00_04_00
  , ValidToSubHourUpdatedFlag       = 0x00_08_00
  , BidTopPriceUpdatedFlag          = 0x00_10_00
  , AskTopPriceUpdatedFlag          = 0x00_20_00
  , ExecutableUpdatedFlag           = 0x00_40_00
  , BatchIdeUpdatedFlag             = 0x00_80_00
  , SourceQuoteReferenceUpdatedFlag = 0x01_00_00
  , ValueDateUpdatedFlag            = 0x02_00_00
}

[Flags]
public enum PublishableQuoteFieldUpdatedFlags : uint
{
    None                                  = 0x00_00
  , FeedSyncStatusFlag                    = 0x00_01
  , SocketReceivedDateUpdatedFlag         = 0x00_02
  , SocketReceivedSubHourUpdatedFlag      = 0x00_04
  , ProcessedDateUpdatedFlag              = 0x00_08
  , ProcessedSubHourUpdatedFlag           = 0x00_10
  , DispatchedDateUpdatedFlag             = 0x00_20
  , DispatchedSubHourUpdatedFlag          = 0x00_40
  , ClientReceivedDateUpdatedFlag         = 0x00_80
  , ClientReceivedSubHourUpdatedFlag      = 0x01_00
  , AdapterReceivedTimeDateUpdatedFlag    = 0x02_00
  , AdapterReceivedTimeSubHourUpdatedFlag = 0x04_00
  , AdapterSentTimeDateUpdatedFlag        = 0x08_00
  , AdapterSentTimeSubHourUpdatedFlag     = 0x10_00
}

public static class QuoteFieldUpdatedFlagsExtensions { }
