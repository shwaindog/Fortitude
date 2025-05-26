namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

[Flags]
public enum LastTradedTypeFlags : uint
{
    None                                   = 0
  , SourceSentPaidGivenDetails             = 0x00_00_00_01
  , PendingTradeVolumeTradeUpdate          = 0x00_00_00_02
  , NoTradeVolumeTradeProvided             = 0x00_00_00_04
  , SourceSentCounterPartyDetails          = 0x00_00_00_08
  , PublishedWithInternalOrderTradeDetails = 0x00_00_00_10
  , IsInternalOrderTradeUpdate             = 0x00_00_00_20
  , HasInternalOrderDetails                = 0x00_00_00_40
  , HasPaidGivenDetails                    = 0x00_00_00_80
  , HasCounterPartyDetails                 = 0x00_00_01_00
  , HasInternalOrderTradeDetails           = 0x00_00_02_00
  , IsTradeUnknownPassiveOrder             = 0x00_00_04_00
  , IsTradeUnknownAggressiveOrder          = 0x00_00_08_00
  , IsTradeExternalPassiveOrder            = 0x00_00_10_00
  , IsTradeExternalAggressiveOrder         = 0x00_00_20_00
  , IsTradeInternalPassiveOrder            = 0x00_00_40_00
  , IsTradeInternalAggressiveOrder         = 0x00_00_80_00
  , CompletesInternalOrderVolumeTrade      = 0x00_01_00_00
  , PartialInternalOrderVolumeTrade        = 0x00_02_00_00

  , TriggeredFromSource  = 0x01_00_00_00
  , TriggeredFromAdapter = 0x02_00_00_00
}