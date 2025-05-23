namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

[Flags]
public enum LastTradeUpdated : uint
{
    None                                   = 0x00_00_00
  , TradeIdUpdated                         = 0x00_00_01
  , TradeBatchIdUpdated                    = 0x00_00_02
  , TradeFirstNotifiedDateUpdated          = 0x00_00_04
  , TradeFirstNotifiedSub2MinTimeUpdated   = 0x00_00_08
  , TradeAdapterReceivedDateUpdated        = 0x00_00_10
  , TradeAdapterReceivedSub2MinTimeUpdated = 0x00_00_20
  , TradeUpdateDateUpdated                 = 0x00_00_40
  , TradeUpdateSub2MinTimeUpdated          = 0x00_00_80
  , TradeTypeFlagsUpdated                  = 0x00_01_00
  , TradeLifeCycleStatusUpdated            = 0x00_02_00
  , TradePriceUpdated                      = 0x00_04_00
  , TradeTimeDateUpdated                   = 0x00_08_00
  , TradeTimeSub2MinUpdated                = 0x00_10_00
  , TradeOrderIdUpdated                    = 0x00_20_00
  , TradeWasPaidUpdated                    = 0x00_40_00
  , TradeWasGivenUpdated                   = 0x00_80_00
  , TradeVolumeUpdated                     = 0x01_00_00
  , ExternalCounterPartyIdUpdated          = 0x02_00_00
  , ExternalCounterPartyNameUpdated        = 0x04_00_00
  , ExternalTraderIdUpdated                = 0x08_00_00
  , ExternalTraderNameUpdated              = 0x10_00_00
  , AllFlagsMask                           = 0x1F_FF_FF
}
