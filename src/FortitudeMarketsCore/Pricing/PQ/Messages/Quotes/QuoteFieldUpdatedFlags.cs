// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

[Flags]
public enum QuoteFieldUpdatedFlags : uint
{
    None                                    = 0x0000_0000
  , SinglePriceUpdatedFlag                  = 0x0000_0001
  , IsReplayUpdatedFlag                     = 0x0000_0002
  , SourceSentDateUpdatedFlag               = 0x0000_0004
  , SourceSentSubHourUpdatedFlag            = 0x0000_0008
  , SocketReceivedDateUpdatedFlag           = 0x0000_0010
  , SocketReceivedSubHourUpdatedFlag        = 0x0000_0020
  , ProcessedDateUpdatedFlag                = 0x0000_0040
  , ProcessedSubHourUpdatedFlag             = 0x0000_0080
  , DispatchedDateUpdatedFlag               = 0x0000_0100
  , DispatchedSubHourUpdatedFlag            = 0x0000_0200
  , ClientReceivedDateUpdatedFlag           = 0x0000_0400
  , ClientReceivedSubHourUpdatedFlag        = 0x0000_0800
  , AdapterReceivedTimeDateUpdatedFlag      = 0x0000_1000
  , AdapterReceivedTimeSubSecondUpdatedFlag = 0x0000_2000
  , AdapterSentTimeDateUpdatedFlag          = 0x0000_4000
  , AdapterSentTimeSubSecondUpdatedFlag     = 0x0000_8000
  , SourceAskTimeDateUpdatedFlag            = 0x0001_0000
  , SourceAskTimeSubSecondUpdatedFlag       = 0x0002_0000
  , SourceBidTimeDateUpdatedFlag            = 0x0004_0000
  , SourceBidTimeSubSecondUpdatedFlag       = 0x0008_0000
  , PublicationStatusUpdatedFlag            = 0x0010_0000
  , PreviousQuoteBidTopUpdatedFlag          = 0x0020_0000
  , BidTopPriceUpdatedFlag                  = 0x0040_0000
  , PreviousQuoteAskTopUpdatedFlag          = 0x0080_0000
  , AskTopPriceUpdatedFlag                  = 0x0100_0000
  , ExecutableUpdatedFlag                   = 0x0200_0000
  , BatchIdeUpdatedFlag                     = 0x0400_0000
  , SourceQuoteReferenceUpdatedFlag         = 0x0800_0000
  , ValueDateUpdatedFlag                    = 0x1000_0000
}

public static class QuoteFieldUpdatedFlagsExtensions { }
