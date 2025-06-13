// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeIO.Transports.Network.Config;
using FortitudeMarkets.Config;
using FortitudeMarkets.Config.Availability;
using FortitudeMarkets.Config.PricingConfig;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using Microsoft.Extensions.Configuration;

namespace FortitudeTests.FortitudeMarkets.Config;

[TestClass]
public class MarketsConfigTests
{
    private IList<IMarketConnectionConfig> dummyServerConfigs = null!;

    private IMarketConnectionConfig marketConnectionConfig1 = null!;
    private IMarketConnectionConfig marketConnectionConfig2 = null!;

    private MarketsConfig marketsConfig = null!;

    [TestInitialize]
    public void ManualSetup()
    {
        marketsConfig = new MarketsConfig("MarketsConfigTests");

        dummyServerConfigs = MarketConnectionConfigTests.ListOfSampleServerConfigs;

        marketConnectionConfig1 = dummyServerConfigs[0];
        marketConnectionConfig2 = dummyServerConfigs[1];
    }


    [TestMethod]
    public void NonEmptyRepo_FindConfig_RetirevesConfig()
    {
        marketsConfig.AddOrUpdate(marketConnectionConfig1);
        marketsConfig.AddOrUpdate(marketConnectionConfig2);

        var foundItem = marketsConfig.Find(marketConnectionConfig2.SourceName!);

        marketConnectionConfig2.ConnectionName = marketsConfig.ConnectionName;

        Assert.AreEqual(foundItem, marketConnectionConfig2);
    }


    [TestMethod]
    public void NonEmptyRepo_SubscribeToUpdateStream_RetirevesExistingConfig()
    {
        marketsConfig.AddOrUpdate(marketConnectionConfig1);
        marketsConfig.AddOrUpdate(marketConnectionConfig2);
    }


    [TestMethod]
    public void EmptyRepo_AddOrUpdate_UpdateEventsAreReceived()
    {
        marketsConfig.AddOrUpdate(marketConnectionConfig1);
        marketsConfig.AddOrUpdate(marketConnectionConfig2);

        // attempt to addorupdate a second time does nothing
        marketsConfig.AddOrUpdate(marketConnectionConfig2);

        var serverConfig21 = marketConnectionConfig2.Clone();
        serverConfig21.ConnectionName = marketsConfig.ConnectionName;
        MarketConnectionConfigTests.UpdateServerConfigWithValues(serverConfig21, "UpdatedTestConfigName2", MarketConnectionType.Pricing);
        marketsConfig.AddOrUpdate(serverConfig21);

        var foundConfig = marketsConfig.Find("UpdatedTestConfigName2");
        // replaced config is now equal to original as it was updated
        Assert.IsTrue(serverConfig21.Equals(foundConfig));
    }

    [TestMethod]
    public void OpenSampleFile_SimplePricingOnlyConfig_HasFileConfigValues()
    {
        var config = new ConfigurationBuilder()
                     .AddJsonFile("FortitudeMarkets/Config/SampleMarketConfig.json")
                     .Build();
        marketsConfig = new MarketsConfig(config, "MarketsConfig");

        Assert.IsNotNull(marketsConfig);
        var marketConnConfig = marketsConfig.Markets["SimplePricingOnlyConfig"];
        Assert.IsNotNull(marketConnConfig);
        Assert.AreEqual(1, marketConnConfig.SourceId);
        Assert.AreEqual("SimplePricingOnlyConfig", marketConnConfig.SourceName);
        Assert.AreEqual(MarketConnectionType.Pricing, marketConnConfig.MarketConnectionType);
        var sourceTickerConfig = marketConnConfig.SourceTickerConfig;
        Assert.IsNotNull(sourceTickerConfig);
        Assert.AreEqual(CountryCityCodes.USinNY, sourceTickerConfig.SourcePublishLocation);
        Assert.AreEqual(TickerAvailability.PricingEnabled, sourceTickerConfig.DefaultTickerAvailability);
        Assert.AreEqual(TickerQuoteDetailLevel.Level2Quote, sourceTickerConfig.DefaultPublishTickerQuoteDetailLevel);
        Assert.AreEqual
            (new MarketClassification
                 (AssetType.Equity, AssetCategory.Emerging, ProductType.Spot, MarketRegion.UnitedStates)
           , sourceTickerConfig.DefaultMarketClassificationConfig.MarketClassification);
        Assert.AreEqual(0.01m, sourceTickerConfig.DefaultPip);
        Assert.AreEqual(100m, sourceTickerConfig.DefaultMaxSubmitSize);
        Assert.AreEqual((ushort)4, sourceTickerConfig.DefaultMaximumPublishedLayers);
        Assert.AreEqual(2, sourceTickerConfig.Tickers.Count);
        var firstTicker = sourceTickerConfig.Tickers["AAPL"];
        Assert.IsNotNull(firstTicker);
        Assert.AreEqual(1, firstTicker.InstrumentId);
        var secondTicker = sourceTickerConfig.Tickers["ADBE"];
        Assert.IsNotNull(secondTicker);
        Assert.AreEqual(2, secondTicker.InstrumentId);
        var pricingServerConfig = marketConnConfig.PricingServerConfig;
        Assert.IsNotNull(pricingServerConfig);
        Assert.AreEqual("SimplePricingOnlyConfigPricing", pricingServerConfig.ConnectionName);
        var snapshotConfig = pricingServerConfig.SnapshotConnectionConfig;
        Assert.IsNotNull(snapshotConfig);
        Assert.AreEqual("SimplePricingOnlyConfigPricingSnapshot", snapshotConfig.ConnectionName);
        Assert.AreEqual(SocketConversationProtocol.TcpAcceptor, snapshotConfig.ConversationProtocol);
        Assert.AreEqual(1, snapshotConfig.AvailableConnections.Count());
        var firstEndpoint = snapshotConfig.AvailableConnections.First();
        Assert.IsNotNull(firstEndpoint);
        Assert.AreEqual("SimplePricingOnlyConfigPricingSnapshot_PrimarySnapshot", firstEndpoint.InstanceName);
        Assert.AreEqual("169.254.224.238", firstEndpoint.Hostname);
        Assert.AreEqual((ushort)4100, firstEndpoint.Port);
        var updatesConfig = pricingServerConfig.UpdateConnectionConfig;
        Assert.IsNotNull(updatesConfig);
        Assert.AreEqual("SimplePricingOnlyConfigPricingUpdates", updatesConfig.ConnectionName);
        Assert.AreEqual(SocketConversationProtocol.UdpPublisher, updatesConfig.ConversationProtocol);
        Assert.AreEqual(1, updatesConfig.AvailableConnections.Count());
        firstEndpoint = updatesConfig.AvailableConnections.First();
        Assert.IsNotNull(firstEndpoint);
        Assert.AreEqual("SimplePricingOnlyConfigPricingUpdates_PrimaryUpdate", firstEndpoint.InstanceName);
        Assert.AreEqual("169.254.224.238", firstEndpoint.Hostname);
        Assert.AreEqual((ushort)4100, firstEndpoint.Port);

        Assert.IsNull(marketConnConfig.TradingServerConfig);

        foreach (var srcTkrInfo in marketConnConfig.AllSourceTickerInfos)
        {
            switch (srcTkrInfo.InstrumentId)
            {
                case 1 :
                    Assert.AreEqual("AAPL", srcTkrInfo.InstrumentName);
                    break;
                case 2 :
                    Assert.AreEqual("ADBE", srcTkrInfo.InstrumentName);
                    break;
            }
            Assert.AreEqual(TickerQuoteDetailLevel.Level2Quote,  srcTkrInfo.PublishedTickerQuoteDetailLevel);
            Assert.AreEqual
                (new MarketClassification(AssetType.Equity, AssetCategory.Emerging, ProductType.Spot, MarketRegion.UnitedStates)
               , srcTkrInfo.MarketClassification);
            Assert.AreEqual(SourceTickerInfo.DefaultRoundingPrecision, srcTkrInfo.RoundingPrecision);
            Assert.AreEqual(0.01m, srcTkrInfo.Pip);
            Assert.AreEqual(SourceTickerInfo.DefaultMinSubmitSize, srcTkrInfo.MinSubmitSize);
            Assert.AreEqual(100m, srcTkrInfo.MaxSubmitSize);
            Assert.AreEqual(SourceTickerInfo.DefaultIncrementSize, srcTkrInfo.IncrementSize);
            Assert.AreEqual(SourceTickerInfo.DefaultMinimumQuoteLife, srcTkrInfo.MinimumQuoteLife);
            Assert.AreEqual(SourceTickerInfo.DefaultDefaultMaxValidMs, srcTkrInfo.DefaultMaxValidMs);
            Assert.AreEqual((ushort)4, srcTkrInfo.MaximumPublishedLayers);
            Assert.AreEqual(SourceTickerInfo.PriceVolumeFlags, srcTkrInfo.LayerFlags);
            Assert.AreEqual(SourceTickerInfo.DefaultLastTradedFlags, srcTkrInfo.LastTradedFlags);
            Assert.AreEqual(CountryCityCodes.USinNY, srcTkrInfo.SourcePublishLocation);
            Assert.AreEqual(CountryCityCodes.AUinMEL, srcTkrInfo.AdapterReceiveLocation);
            Assert.AreEqual(CountryCityCodes.Unknown, srcTkrInfo.ClientReceiveLocation);
        }
    }

    [TestMethod]
    public void OpenSampleFile_SimpleTradingOnlyConfig_HasFileConfigValues()
    {
        var config = new ConfigurationBuilder()
                     .AddJsonFile("FortitudeMarkets/Config/SampleMarketConfig.json")
                     .Build();
        marketsConfig = new MarketsConfig(config, "MarketsConfig");

        Assert.IsNotNull(marketsConfig);
        var marketConnConfig = marketsConfig.Markets["SimpleTradingOnlyConfig"];
        Assert.IsNotNull(marketConnConfig);
        Assert.AreEqual(2, marketConnConfig.SourceId);
        Assert.AreEqual("SimpleTradingOnlyConfig", marketConnConfig.SourceName);
        Assert.AreEqual(MarketConnectionType.Trading, marketConnConfig.MarketConnectionType);
        var sourceTickerConfig = marketConnConfig.SourceTickerConfig;
        Assert.IsNotNull(sourceTickerConfig);
        Assert.AreEqual(CountryCityCodes.GBinLON, sourceTickerConfig.SourcePublishLocation);
        Assert.AreEqual(TickerAvailability.TradingEnabled, sourceTickerConfig.DefaultTickerAvailability);
        Assert.AreEqual(TickerQuoteDetailLevel.None, sourceTickerConfig.DefaultPublishTickerQuoteDetailLevel);
        Assert.AreEqual
            (new MarketClassification
                 (AssetType.FX, AssetCategory.Major, ProductType.Spot, MarketRegion.UnitedKingdom)
           , sourceTickerConfig.DefaultMarketClassificationConfig.MarketClassification);
        Assert.AreEqual( 0.0001m, sourceTickerConfig.DefaultPip);
        Assert.AreEqual(1_000_000m, sourceTickerConfig.DefaultMaxSubmitSize);
        Assert.AreEqual((ushort)0, sourceTickerConfig.DefaultMaximumPublishedLayers);
        Assert.AreEqual(2, sourceTickerConfig.Tickers.Count);
        var firstTicker = sourceTickerConfig.Tickers["EURUSD"];
        Assert.IsNotNull(firstTicker);
        Assert.AreEqual(1, firstTicker.InstrumentId);
        var secondTicker = sourceTickerConfig.Tickers["USDJPY"];
        Assert.IsNotNull(secondTicker);
        Assert.AreEqual(2, secondTicker.InstrumentId);
        var tradingServerConfig = marketConnConfig.TradingServerConfig;
        Assert.IsNotNull(tradingServerConfig);
        Assert.AreEqual("SimpleTradingOnlyConfigTrading", tradingServerConfig.ConnectionName);
        var tradingConfig = tradingServerConfig.TradingServerConnectionConfig;
        Assert.IsNotNull(tradingConfig);
        Assert.AreEqual("SimpleTradingOnlyConfigTrading", tradingConfig.ConnectionName);
        Assert.AreEqual(SocketConversationProtocol.TcpAcceptor, tradingConfig.ConversationProtocol);
        Assert.AreEqual(1, tradingConfig.AvailableConnections.Count());
        var firstEndpoint = tradingConfig.AvailableConnections.First();
        Assert.IsNotNull(firstEndpoint);
        Assert.AreEqual("SimpleTradingOnlyConfigTrading_PrimaryTradingServer", firstEndpoint.InstanceName);
        Assert.AreEqual("169.254.224.238", firstEndpoint.Hostname);
        Assert.AreEqual((ushort)4111, firstEndpoint.Port);

        Assert.IsNull(marketConnConfig.PricingServerConfig);

        foreach (var srcTkrInfo in marketConnConfig.AllSourceTickerInfos)
        {
            switch (srcTkrInfo.InstrumentId)
            {
                case 1 :
                    Assert.AreEqual("EURUSD", srcTkrInfo.InstrumentName);
                    break;
                case 2 :
                    Assert.AreEqual("USDJPY", srcTkrInfo.InstrumentName);
                    break;
            }
            Assert.AreEqual(TickerQuoteDetailLevel.None,  srcTkrInfo.PublishedTickerQuoteDetailLevel);
            Assert.AreEqual
                (new MarketClassification(AssetType.FX, AssetCategory.Major, ProductType.Spot, MarketRegion.UnitedKingdom)
               , srcTkrInfo.MarketClassification);
            Assert.AreEqual(SourceTickerInfo.DefaultRoundingPrecision, srcTkrInfo.RoundingPrecision);
            Assert.AreEqual(0.0001m, srcTkrInfo.Pip);
            Assert.AreEqual(SourceTickerInfo.DefaultMinSubmitSize, srcTkrInfo.MinSubmitSize);
            Assert.AreEqual(1_000_000m, srcTkrInfo.MaxSubmitSize);
            Assert.AreEqual(SourceTickerInfo.DefaultIncrementSize, srcTkrInfo.IncrementSize);
            Assert.AreEqual(SourceTickerInfo.DefaultMinimumQuoteLife, srcTkrInfo.MinimumQuoteLife);
            Assert.AreEqual(SourceTickerInfo.DefaultDefaultMaxValidMs, srcTkrInfo.DefaultMaxValidMs);
            Assert.AreEqual((ushort)0, srcTkrInfo.MaximumPublishedLayers);
            Assert.AreEqual(SourceTickerInfo.DefaultLayerFlags, srcTkrInfo.LayerFlags);
            Assert.AreEqual(SourceTickerInfo.DefaultLastTradedFlags, srcTkrInfo.LastTradedFlags);
            Assert.AreEqual(CountryCityCodes.GBinLON, srcTkrInfo.SourcePublishLocation);
            Assert.AreEqual(CountryCityCodes.AUinMEL, srcTkrInfo.AdapterReceiveLocation);
            Assert.AreEqual(CountryCityCodes.Unknown, srcTkrInfo.ClientReceiveLocation);
        }
    }

    [TestMethod]
    public void OpenSampleFile_SimpleConfigWithAvailabilityTimeTable_HasFileConfigValues()
    {
        var config = new ConfigurationBuilder()
                     .AddJsonFile("FortitudeMarkets/Config/SampleMarketConfig.json")
                     .Build();
        marketsConfig = new MarketsConfig(config, "MarketsConfig");

        Assert.IsNotNull(marketsConfig);
        var marketConnConfig = marketsConfig.Markets["SimpleConfigWithAvailability"];
        Assert.IsNotNull(marketConnConfig);
        Assert.AreEqual(3, marketConnConfig.SourceId);
        Assert.AreEqual("SimpleConfigWithAvailability", marketConnConfig.SourceName);
        Assert.AreEqual(MarketConnectionType.Pricing | MarketConnectionType.Trading, marketConnConfig.MarketConnectionType);
        var sourceTickerConfig = marketConnConfig.SourceTickerConfig;
        Assert.IsNotNull(sourceTickerConfig);
        Assert.AreEqual(CountryCityCodes.USinCH, sourceTickerConfig.SourcePublishLocation);
        Assert.AreEqual(TickerAvailability.PricingAndTradingEnabled, sourceTickerConfig.DefaultTickerAvailability);
        Assert.AreEqual(TickerQuoteDetailLevel.Level3Quote, sourceTickerConfig.DefaultPublishTickerQuoteDetailLevel);
        var tradingTimeTable = sourceTickerConfig.DefaultTickerTradingTimeTableConfig;
        Assert.IsNotNull(tradingTimeTable);
        var tradingSchedule = tradingTimeTable.TradingScheduleConfig;
        Assert.IsNotNull(tradingSchedule);
        Assert.AreEqual(TimeSpan.FromMinutes(2), tradingSchedule.TradingStartsFromOpenTimeSpan);
        var prefTradingTimes = tradingSchedule.OverridePreferredTradingTimes;
        Assert.IsNotNull(prefTradingTimes);
        Assert.AreEqual(TimeZoneInfo.FindSystemTimeZoneById("Australia/Melbourne"), prefTradingTimes.OverrideTimeZone);
        Assert.AreEqual(TimeSpan.FromHours(14), prefTradingTimes.StartTime);
        Assert.AreEqual(TimeSpan.FromHours(20), prefTradingTimes.StopTime);
        var highActivityTimes = tradingSchedule.HighActivityTimes;
        Assert.IsNotNull(highActivityTimes);
        Assert.AreEqual(TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"), highActivityTimes.OverrideTimeZone);
        Assert.AreEqual(TimeSpan.FromHours(9), highActivityTimes.StartTime);
        Assert.AreEqual(TimeSpan.FromHours(17), highActivityTimes.StopTime);
        Assert.AreEqual(TimeSpan.FromMinutes(2), tradingSchedule.AnnounceClosingSoonFromCloseTimeSpan);
        var venueOperatingTimes = tradingTimeTable.HighLiquidityTimeTable;
        Assert.IsNotNull(venueOperatingTimes);
        Assert.AreEqual(TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"), venueOperatingTimes.OperatingTimeZone);
        var weeklyTimeTable = venueOperatingTimes.WeeklyTimeTableConfig;
        Assert.IsNotNull(weeklyTimeTable);
        Assert.AreEqual(DayOfWeekFlags.Tuesday | DayOfWeekFlags.Wednesday | DayOfWeekFlags.Thursday
                      , weeklyTimeTable.DaysOfWeek);
        var startTimes = weeklyTimeTable.StartTimes;
        Assert.IsNotNull(startTimes);
        Assert.AreEqual(TimeSpan.FromHours(10), startTimes.TimeOfDay);
        Assert.AreEqual(TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"), startTimes.OverrideTimeZone);
        var stopTimes = weeklyTimeTable.StopTimes;
        Assert.IsNotNull(stopTimes);
        Assert.AreEqual(TimeSpan.FromHours(16), stopTimes.TimeOfDay);
        Assert.AreEqual(TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"), stopTimes.OverrideTimeZone);
        var calendarHolidays = venueOperatingTimes.CalendarHolidays;
        Assert.IsNotNull(calendarHolidays);


        Assert.AreEqual
            (new MarketClassification
                 (AssetType.Commodity, AssetCategory.Soft, ProductType.Spot, MarketRegion.UnitedStates)
           , sourceTickerConfig.DefaultMarketClassificationConfig.MarketClassification);
        Assert.AreEqual( 0.01m, sourceTickerConfig.DefaultPip);
        Assert.AreEqual(1_000m, sourceTickerConfig.DefaultMaxSubmitSize);
        Assert.AreEqual((ushort)20, sourceTickerConfig.DefaultMaximumPublishedLayers);
        Assert.AreEqual(LayerFlagsExtensions.FullAnonymousOrderFlags, sourceTickerConfig.DefaultLayerFlags);
        Assert.AreEqual(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags, sourceTickerConfig.DefaultLastTradedFlags);
        Assert.AreEqual(2, sourceTickerConfig.Tickers.Count);
        var firstTicker = sourceTickerConfig.Tickers["CORN"];
        Assert.IsNotNull(firstTicker);
        Assert.AreEqual(1, firstTicker.InstrumentId);
        var secondTicker = sourceTickerConfig.Tickers["SOYB"];
        Assert.IsNotNull(secondTicker);
        Assert.AreEqual(2, secondTicker.InstrumentId);

        var tradingServerConfig = marketConnConfig.TradingServerConfig;
        Assert.IsNotNull(tradingServerConfig);
        Assert.AreEqual("SimpleConfigWithAvailabilityTrading", tradingServerConfig.ConnectionName);
        var tradingConfig = tradingServerConfig.TradingServerConnectionConfig;
        Assert.IsNotNull(tradingConfig);
        Assert.AreEqual("SimpleConfigWithAvailabilityTrading", tradingConfig.ConnectionName);
        Assert.AreEqual(SocketConversationProtocol.TcpAcceptor, tradingConfig.ConversationProtocol);
        Assert.AreEqual(1, tradingConfig.AvailableConnections.Count());
        var firstEndpoint = tradingConfig.AvailableConnections.First();
        Assert.IsNotNull(firstEndpoint);
        Assert.AreEqual("SimpleConfigWithAvailabilityTrading_PrimaryTradingServer", firstEndpoint.InstanceName);
        Assert.AreEqual("169.254.224.238", firstEndpoint.Hostname);
        Assert.AreEqual((ushort)4121, firstEndpoint.Port);
        
        var pricingServerConfig = marketConnConfig.PricingServerConfig;
        Assert.IsNotNull(pricingServerConfig);
        Assert.AreEqual("SimpleConfigWithAvailabilityPricing", pricingServerConfig.ConnectionName);
        var snapshotConfig = pricingServerConfig.SnapshotConnectionConfig;
        Assert.IsNotNull(snapshotConfig);
        Assert.AreEqual("SimpleConfigWithAvailabilityPricingSnapshot", snapshotConfig.ConnectionName);
        Assert.AreEqual(SocketConversationProtocol.TcpAcceptor, snapshotConfig.ConversationProtocol);
        Assert.AreEqual(1, snapshotConfig.AvailableConnections.Count());
        firstEndpoint = snapshotConfig.AvailableConnections.First();
        Assert.IsNotNull(firstEndpoint);
        Assert.AreEqual("SimpleConfigWithAvailabilityPricingSnapshot_PrimarySnapshot", firstEndpoint.InstanceName);
        Assert.AreEqual("169.254.224.238", firstEndpoint.Hostname);
        Assert.AreEqual((ushort)4120, firstEndpoint.Port);
        var updatesConfig = pricingServerConfig.UpdateConnectionConfig;
        Assert.IsNotNull(updatesConfig);
        Assert.AreEqual("SimpleConfigWithAvailabilityPricingUpdates", updatesConfig.ConnectionName);
        Assert.AreEqual(SocketConversationProtocol.UdpPublisher, updatesConfig.ConversationProtocol);
        Assert.AreEqual(1, updatesConfig.AvailableConnections.Count());
        firstEndpoint = updatesConfig.AvailableConnections.First();
        Assert.IsNotNull(firstEndpoint);
        Assert.AreEqual("SimpleConfigWithAvailabilityPricingUpdates_PrimaryUpdate", firstEndpoint.InstanceName);
        Assert.AreEqual("169.254.224.238", firstEndpoint.Hostname);
        Assert.AreEqual((ushort)4120, firstEndpoint.Port);

        venueOperatingTimes = marketConnConfig.VenueOperatingTimeTableConfig;
        Assert.IsNotNull(venueOperatingTimes);
        Assert.AreEqual(TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"), venueOperatingTimes.OperatingTimeZone);
        weeklyTimeTable = venueOperatingTimes.WeeklyTimeTableConfig;
        Assert.IsNotNull(weeklyTimeTable);
        Assert.AreEqual(DayOfWeekFlags.Monday | DayOfWeekFlags.Tuesday | DayOfWeekFlags.Wednesday | DayOfWeekFlags.Thursday | DayOfWeekFlags.Friday
                      , weeklyTimeTable.DaysOfWeek);
        startTimes = weeklyTimeTable.StartTimes;
        Assert.IsNotNull(startTimes);
        Assert.AreEqual(TimeSpan.FromHours(9), startTimes.TimeOfDay);
        Assert.AreEqual(TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"), startTimes.OverrideTimeZone);
        stopTimes = weeklyTimeTable.StopTimes;
        Assert.IsNotNull(stopTimes);
        Assert.AreEqual(TimeSpan.FromHours(17), stopTimes.TimeOfDay);
        Assert.AreEqual(TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"), stopTimes.OverrideTimeZone);
        calendarHolidays = venueOperatingTimes.CalendarHolidays;
        Assert.IsNotNull(calendarHolidays);
        Assert.AreEqual(NamedHoliday.NewYearsDay, calendarHolidays[0].HolidayName);
        Assert.AreEqual(MonthFlags.January, calendarHolidays[0].Month);
        Assert.AreEqual((byte)1, calendarHolidays[0].Day);
        Assert.AreEqual(NamedHoliday.MemorialDay, calendarHolidays[1].HolidayName);
        Assert.AreEqual(MonthFlags.May, calendarHolidays[1].Month);
        var floatingWeek = calendarHolidays[1].FloatingWeekday!;
        Assert.IsNotNull(floatingWeek);
        Assert.AreEqual(DayOfWeek.Monday, floatingWeek.Value.Weekday);
        Assert.AreEqual(MonthlyFloatingWeekDayOccurence.LastInMonth, floatingWeek.Value.OccurenceInMonth);
        Assert.AreEqual(NamedHoliday.IndependenceDay, calendarHolidays[2].HolidayName);
        Assert.AreEqual(MonthFlags.July, calendarHolidays[2].Month);
        Assert.AreEqual((byte)4, calendarHolidays[2].Day);
        Assert.AreEqual(NamedHoliday.LaborDay, calendarHolidays[3].HolidayName);
        Assert.AreEqual(MonthFlags.September, calendarHolidays[3].Month);
        floatingWeek = calendarHolidays[3].FloatingWeekday!;
        Assert.IsNotNull(floatingWeek);
        Assert.AreEqual(DayOfWeek.Monday, floatingWeek.Value.Weekday);
        Assert.AreEqual(MonthlyFloatingWeekDayOccurence.FirstInMonth, floatingWeek.Value.OccurenceInMonth);
        Assert.AreEqual(NamedHoliday.Thanksgiving, calendarHolidays[4].HolidayName);
        Assert.AreEqual(MonthFlags.November, calendarHolidays[4].Month);
        floatingWeek = calendarHolidays[4].FloatingWeekday!;
        Assert.IsNotNull(floatingWeek);
        Assert.AreEqual(DayOfWeek.Thursday, floatingWeek.Value.Weekday);
        Assert.AreEqual(MonthlyFloatingWeekDayOccurence.FourthInMonth, floatingWeek.Value.OccurenceInMonth);
        Assert.AreEqual(NamedHoliday.ChristmasDay, calendarHolidays[5].HolidayName);
        Assert.AreEqual(MonthFlags.December, calendarHolidays[5].Month);
        Assert.AreEqual((byte)25, calendarHolidays[5].Day);
        var followsIrregular = venueOperatingTimes.FollowsIrregularHolidays;
        Assert.IsNotNull(followsIrregular);
        Assert.AreEqual(NamedHoliday.EasterGoodFriday, followsIrregular[0]);
        var upcomingIrregular = venueOperatingTimes.UpcomingIrregularHolidays;
        Assert.IsNotNull(upcomingIrregular);
        Assert.AreEqual(NamedHoliday.EasterGoodFriday, upcomingIrregular[0].HolidayName);
        Assert.AreEqual((ushort?)2026, upcomingIrregular[0].Year);
        Assert.AreEqual(MonthFlags.April, upcomingIrregular[0].Month);
        Assert.AreEqual((byte)3, upcomingIrregular[0].Day);

        foreach (var srcTkrInfo in marketConnConfig.AllSourceTickerInfos)
        {
            switch (srcTkrInfo.InstrumentId)
            {
                case 1 :
                    Assert.AreEqual("CORN", srcTkrInfo.InstrumentName);
                    break;
                case 2 :
                    Assert.AreEqual("SOYB", srcTkrInfo.InstrumentName);
                    break;
            }
            Assert.AreEqual(TickerQuoteDetailLevel.Level3Quote,  srcTkrInfo.PublishedTickerQuoteDetailLevel);
            Assert.AreEqual
                (new MarketClassification(AssetType.Commodity, AssetCategory.Soft, ProductType.Spot, MarketRegion.UnitedStates)
               , srcTkrInfo.MarketClassification);
            Assert.AreEqual(SourceTickerInfo.DefaultRoundingPrecision, srcTkrInfo.RoundingPrecision);
            Assert.AreEqual(0.01m, srcTkrInfo.Pip);
            Assert.AreEqual(SourceTickerInfo.DefaultMinSubmitSize, srcTkrInfo.MinSubmitSize);
            Assert.AreEqual(1_000m, srcTkrInfo.MaxSubmitSize);
            Assert.AreEqual(SourceTickerInfo.DefaultIncrementSize, srcTkrInfo.IncrementSize);
            Assert.AreEqual(SourceTickerInfo.DefaultMinimumQuoteLife, srcTkrInfo.MinimumQuoteLife);
            Assert.AreEqual(SourceTickerInfo.DefaultDefaultMaxValidMs, srcTkrInfo.DefaultMaxValidMs);
            Assert.AreEqual((ushort)20, srcTkrInfo.MaximumPublishedLayers);
            Assert.AreEqual(LayerFlagsExtensions.FullAnonymousOrderFlags, srcTkrInfo.LayerFlags);
            Assert.AreEqual(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags, srcTkrInfo.LastTradedFlags);
            Assert.AreEqual(CountryCityCodes.USinCH, srcTkrInfo.SourcePublishLocation);
            Assert.AreEqual(CountryCityCodes.AUinMEL, srcTkrInfo.AdapterReceiveLocation);
            Assert.AreEqual(CountryCityCodes.Unknown, srcTkrInfo.ClientReceiveLocation);
        }
    }
}
