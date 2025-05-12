using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

public class SourceTickerInfoTests
{
    public static readonly SourceTickerInfo BaseL2PriceVolumeSti = 
        new(ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level2Quote, Unknown
       , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
       , layerFlags: LayerFlagsExtensions.PriceVolumeLayerFlags
          , lastTradedFlags: LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo BaseL3PriceVolumeSti = 
        new(ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
       , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
       , layerFlags: LayerFlagsExtensions.PriceVolumeLayerFlags
          , lastTradedFlags: LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo SimpleL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags);

    public static readonly SourceTickerInfo SourceNameL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags);

    public static readonly SourceTickerInfo OrdersCountL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags);

    public static readonly SourceTickerInfo OrdersAnonL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags);

    public static readonly SourceTickerInfo OrdersCounterPartyL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags);

    public static readonly SourceTickerInfo ValueDateL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags);

    public static readonly SourceTickerInfo FullSupportL2PriceVolumeSti = 
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags);

    public static readonly SourceTickerInfo SimpleL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags);

    public static readonly SourceTickerInfo SimpleL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo SimpleL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo SimpleL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo SimpleL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo SourceNameL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags);

    public static readonly SourceTickerInfo SourceNameL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo SourceNameL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo SourceNameL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo SourceNameL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo SourceQuoteRefL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo SourceQuoteRefL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo OrdersCountL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags);

    public static readonly SourceTickerInfo  OrdersCountL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo  OrdersCountL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  OrdersCountL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  OrdersCountL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo OrdersAnonL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags);

    public static readonly SourceTickerInfo  OrdersAnonL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo OrdersAnonL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  OrdersAnonL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  OrdersAnonL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo OrdersCounterPartyL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags);

    public static readonly SourceTickerInfo OrdersCounterPartyL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo OrdersCounterPartyL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  OrdersCounterPartyL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  OrdersCounterPartyL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo ValueDateL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags);

    public static readonly SourceTickerInfo ValueDateL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo ValueDateL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  ValueDateL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  ValueDateL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly SourceTickerInfo FullSupportL3PriceVolumeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags);

    public static readonly SourceTickerInfo FullSupportL3NoRecentlyTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly SourceTickerInfo FullSupportL3JustTradeTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly SourceTickerInfo  FullSupportL3PaidOrGivenTradeSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly SourceTickerInfo  FullSupportL3TraderNamePaidOrGivenSti = 
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

}