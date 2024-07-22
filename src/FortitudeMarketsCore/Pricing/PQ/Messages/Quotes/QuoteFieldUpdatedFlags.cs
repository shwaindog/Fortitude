// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

[Flags]
public enum QuoteFieldUpdatedFlags : ulong
{
    None                                  = 0x0000_0000
  , SingleValueUpdatedFlag                = 0x0000_0001
  , IsReplayUpdatedFlag                   = 0x0000_0002
  , SourceSentDateUpdatedFlag             = 0x0000_0004
  , SourceSentSubHourUpdatedFlag          = 0x0000_0008
  , FeedSyncStatusFlag                    = 0x0000_0010
  , SocketReceivedDateUpdatedFlag         = 0x0000_0020
  , SocketReceivedSubHourUpdatedFlag      = 0x0000_0040
  , ProcessedDateUpdatedFlag              = 0x0000_0080
  , ProcessedSubHourUpdatedFlag           = 0x0000_0100
  , DispatchedDateUpdatedFlag             = 0x0000_0200
  , DispatchedSubHourUpdatedFlag          = 0x0000_0400
  , ClientReceivedDateUpdatedFlag         = 0x0000_0800
  , ClientReceivedSubHourUpdatedFlag      = 0x0000_1000
  , AdapterReceivedTimeDateUpdatedFlag    = 0x0000_2000
  , AdapterReceivedTimeSubHourUpdatedFlag = 0x0000_4000
  , AdapterSentTimeDateUpdatedFlag        = 0x0000_8000
  , AdapterSentTimeSubHourUpdatedFlag     = 0x0001_0000
  , SourceAskTimeDateUpdatedFlag          = 0x0002_0000
  , SourceAskTimeSubHourUpdatedFlag       = 0x0004_0000
  , SourceBidTimeDateUpdatedFlag          = 0x0008_0000
  , SourceBidTimeSubHourUpdatedFlag       = 0x0010_0000
  , ValidFromDateUpdatedFlag              = 0x0020_0000
  , ValidFromTimeSubHourUpdatedFlag       = 0x0040_0000
  , ValidToDateUpdatedFlag                = 0x0080_0000
  , ValidToSubHourUpdatedFlag             = 0x0100_0000
  , PreviousQuoteBidTopUpdatedFlag        = 0x0200_0000
  , BidTopPriceUpdatedFlag                = 0x0400_0000
  , PreviousQuoteAskTopUpdatedFlag        = 0x0800_0000
  , AskTopPriceUpdatedFlag                = 0x1000_0000
  , ExecutableUpdatedFlag                 = 0x2000_0000
  , BatchIdeUpdatedFlag                   = 0x4000_0000
  , SourceQuoteReferenceUpdatedFlag       = 0x8000_0000
  , ValueDateUpdatedFlag                  = 0x01_0000_0000
}

public static class QuoteFieldUpdatedFlagsExtensions { }
