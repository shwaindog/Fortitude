using System;

namespace FortitudeMarketsCore.Pricing.PQ.Quotes
{
    [Flags]
    public enum QuoteFieldUpdatedFlags : uint
    {   
        None =                                    0x00_0000,
        SinglePriceUpdatedFlag =                  0x00_0001,
        IsReplayUpdatedFlag =                     0x00_0002,
        SourceSentDateUpdatedFlag =               0x00_0004,
        SourceSentSubSecondUpdatedFlag =          0x00_0008,
        PricePrecisionUpdatedFlag =               0x00_0010,
        PublicationStatusUpdatedFlag =            0x00_0020,
        VolumePrecisionUpdatedFlag =              0x00_0040,
        BidTopPriceUpdatedFlag =                  0x00_0080,
        AskTopPriceUpdatedFlag =                  0x00_0100,
        ExecutableUpdatedFlag =                   0x00_0200,
        AdapterReceivedTimeDateUpdatedFlag =      0x00_0400,
        AdapterReceivedTimeSubSecondUpdatedFlag = 0x00_0800,
        AdapterSentTimeDateUpdatedFlag =          0x00_1000,
        AdapterSentTimeSubSecondUpdatedFlag =     0x00_2000,
        SourceAskTimeDateUpdatedFlag =            0x00_4000,
        SourceAskTimeSubSecondUpdatedFlag =       0x00_8000,
        SourceBidTimeDateUpdatedFlag =            0x01_0000,
        SourceBidTimeSubSecondUpdatedFlag =       0x02_0000,
        BatchIdeUpdatedFlag =                     0x04_0000,
        SourceQuoteReferenceUpdatedFlag =         0x08_0000,
        ValueDateUpdatedFlag =                    0x10_0000
    }
}
