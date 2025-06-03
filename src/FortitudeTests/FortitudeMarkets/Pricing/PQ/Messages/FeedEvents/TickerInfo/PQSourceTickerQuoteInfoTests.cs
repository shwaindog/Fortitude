// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;

[TestClass]
public class PQSourceTickerInfoTests
{
    public static readonly PQSourceTickerInfo BaseL2PriceVolumeSti =
        new(ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level2Quote, Unknown
          , 20, 0.000001m, 0.0001m, 1m, 10_000_000m, 1000m
          , layerFlags: LayerFlagsExtensions.PriceVolumeLayerFlags
          , lastTradedFlags: LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo BaseL3PriceVolumeSti =
        new(ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
          , 20, 0.000001m, 0.0001m, 1m, 10_000_000m, 1000m
          , layerFlags: LayerFlagsExtensions.PriceVolumeLayerFlags
          , lastTradedFlags: LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo SimpleL2PriceVolumeSti =
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags);

    public static readonly PQSourceTickerInfo SourceNameL2PriceVolumeSti =
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags);

    public static readonly PQSourceTickerInfo SourceQuoteRefL2PriceVolumeSti =
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags);

    public static readonly PQSourceTickerInfo OrdersCountL2PriceVolumeSti =
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags);

    public static readonly PQSourceTickerInfo OrdersAnonL2PriceVolumeSti =
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags);

    public static readonly PQSourceTickerInfo OrdersCounterPartyL2PriceVolumeSti =
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags);

    public static readonly PQSourceTickerInfo ValueDateL2PriceVolumeSti =
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags);

    public static readonly PQSourceTickerInfo FullSupportL2PriceVolumeSti =
        BaseL2PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags);

    public static readonly PQSourceTickerInfo SimpleL3PriceVolumeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags);

    public static readonly PQSourceTickerInfo SimpleL3NoRecentlyTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly PQSourceTickerInfo SimpleL3JustTradeTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo SimpleL3PaidOrGivenTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly PQSourceTickerInfo SimpleL3TraderNamePaidOrGivenSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.PriceVolumeLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly PQSourceTickerInfo SourceNameL3PriceVolumeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags);

    public static readonly PQSourceTickerInfo SourceNameL3NoRecentlyTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly PQSourceTickerInfo SourceNameL3JustTradeTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo SourceNameL3PaidOrGivenTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly PQSourceTickerInfo SourceNameL3TraderNamePaidOrGivenSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly PQSourceTickerInfo SourceQuoteRefL3PriceVolumeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags);

    public static readonly PQSourceTickerInfo SourceQuoteRefL3NoRecentlyTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly PQSourceTickerInfo SourceQuoteRefL3JustTradeTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo SourceQuoteRefL3PaidOrGivenTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly PQSourceTickerInfo SourceQuoteRefL3TraderNamePaidOrGivenSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSourceQuoteRefFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly PQSourceTickerInfo OrdersCountL3PriceVolumeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags);

    public static readonly PQSourceTickerInfo OrdersCountL3NoRecentlyTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly PQSourceTickerInfo OrdersCountL3JustTradeTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo OrdersCountL3PaidOrGivenTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly PQSourceTickerInfo OrdersCountL3TraderNamePaidOrGivenSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullOrdersCountFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly PQSourceTickerInfo OrdersAnonL3PriceVolumeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags);

    public static readonly PQSourceTickerInfo OrdersAnonL3NoRecentlyTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly PQSourceTickerInfo OrdersAnonL3JustTradeTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo OrdersAnonL3PaidOrGivenTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly PQSourceTickerInfo OrdersAnonL3TraderNamePaidOrGivenSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullAnonymousOrderFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly PQSourceTickerInfo OrdersCounterPartyL3PriceVolumeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags);

    public static readonly PQSourceTickerInfo OrdersCounterPartyL3NoRecentlyTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly PQSourceTickerInfo OrdersCounterPartyL3JustTradeTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo OrdersCounterPartyL3PaidOrGivenTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly PQSourceTickerInfo OrdersCounterPartyL3TraderNamePaidOrGivenSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullCounterPartyOrdersFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly PQSourceTickerInfo ValueDateL3PriceVolumeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags);

    public static readonly PQSourceTickerInfo ValueDateL3NoRecentlyTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly PQSourceTickerInfo ValueDateL3JustTradeTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo ValueDateL3PaidOrGivenTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly PQSourceTickerInfo ValueDateL3TraderNamePaidOrGivenSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullValueDateFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    public static readonly PQSourceTickerInfo FullSupportL3PriceVolumeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags);

    public static readonly PQSourceTickerInfo FullSupportL3NoOnTickLastTradedSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlags.None);

    public static readonly PQSourceTickerInfo FullSupportL3JustTradeTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.LastTradedPriceAndTimeFlags);

    public static readonly PQSourceTickerInfo FullSupportL3PaidOrGivenTradeSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullPaidOrGivenFlags);

    public static readonly PQSourceTickerInfo FullSupportL3TraderNamePaidOrGivenSti =
        BaseL3PriceVolumeSti.WithLayerFlags(LayerFlagsExtensions.FullSupportLayerFlags)
                            .WithLastTradedFlags(LastTradedFlagsExtensions.FullTraderNamePaidOrGivenFlags);

    private PQSourceTickerInfo emptySrcTkrInfo = null!;

    private PQSourceTickerInfo fullyPopulatedSrcTkrInfo = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        fullyPopulatedSrcTkrInfo =
            new PQSourceTickerInfo
                (new SourceTickerInfo
                    (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, FxMajor
                   , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
                   , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
                   , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                      LastTradedFlags.LastTradedTime));
        emptySrcTkrInfo =
            new PQSourceTickerInfo
                (new SourceTickerInfo
                    (0, "", 0, "", Level2Quote, Unknown, 0,
                     0m, 0m, 0m, 0m, 0m, 0, 0, true, false, LayerFlags.None));

        testDateTime = new DateTime(2017, 11, 07, 18, 33, 24);
    }

    [TestMethod]
    public void EmptyQuoteInfo_IdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsIdUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0u, emptySrcTkrInfo.SourceTickerId);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedId = ushort.MaxValue;
        emptySrcTkrInfo.SourceId = expectedId;
        Assert.IsTrue(emptySrcTkrInfo.IsIdUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        var expectedStreamId = (uint)expectedId << 16;
        Assert.AreEqual(expectedStreamId, emptySrcTkrInfo.SourceTickerId);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.SourceTickerId, expectedStreamId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsIdUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsIdUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.SourceTickerId
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedStreamId, newEmpty.SourceTickerId);
        Assert.IsFalse(newEmpty.IsIdUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_SourceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsSourceNameUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsNull(emptySrcTkrInfo.SourceName);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedSource = "NewSourceName";
        emptySrcTkrInfo.SourceName = expectedSource;
        Assert.IsTrue(emptySrcTkrInfo.IsSourceNameUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedSource, emptySrcTkrInfo.SourceName);
        var sourceUpdates = emptySrcTkrInfo.GetStringUpdates(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = ExpectedSourceStringUpdate(expectedSource);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsSourceNameUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsSourceNameUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        sourceUpdates = emptySrcTkrInfo.GetStringUpdates(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        emptySrcTkrInfo.HasUpdates = false;

        sourceUpdates = emptySrcTkrInfo.GetStringUpdates(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, sourceUpdates.Count);

        sourceUpdates = (from update in emptySrcTkrInfo.GetStringUpdates(testDateTime, PQMessageFlags.Snapshot)
            where update.Field.Id == PQFeedFields.SourceTickerNames && update.StringUpdate.DictionaryId == emptySrcTkrInfo.SourceNameId
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateFieldString(sourceUpdates[0]);
        Assert.AreEqual(expectedSource, newEmpty.SourceName);
        Assert.IsFalse(newEmpty.IsSourceNameUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_TickerChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsInstrumentNameUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsNull(emptySrcTkrInfo.InstrumentName);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedTicker = "NewTickerName";
        emptySrcTkrInfo.InstrumentName = expectedTicker;
        Assert.IsTrue(emptySrcTkrInfo.IsInstrumentNameUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedTicker, emptySrcTkrInfo.InstrumentName);
        var tickerUpdates = emptySrcTkrInfo.GetStringUpdates(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, tickerUpdates.Count);
        var expectedFieldUpdate = ExpectedTickerStringUpdate(expectedTicker);
        expectedFieldUpdate = expectedFieldUpdate.WithDictionaryId(emptySrcTkrInfo.InstrumentNameId);
        Assert.AreEqual(expectedFieldUpdate, tickerUpdates[0]);

        emptySrcTkrInfo.IsInstrumentNameUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsInstrumentNameUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        tickerUpdates = emptySrcTkrInfo.GetStringUpdates(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, tickerUpdates.Count);
        emptySrcTkrInfo.HasUpdates = false;

        tickerUpdates = emptySrcTkrInfo.GetStringUpdates(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, tickerUpdates.Count);

        tickerUpdates = (from update in emptySrcTkrInfo.GetStringUpdates(testDateTime, PQMessageFlags.Snapshot)
            where update.Field.Id == PQFeedFields.SourceTickerNames && update.StringUpdate.DictionaryId == emptySrcTkrInfo.InstrumentNameId
            select update).ToList();
        Assert.AreEqual(1, tickerUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, tickerUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateFieldString(tickerUpdates[0]);
        Assert.AreEqual(expectedTicker, newEmpty.InstrumentName);
        Assert.IsFalse(newEmpty.IsInstrumentNameUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_RoundingPrecisionChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsRoundingPrecisionUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrInfo.RoundingPrecision);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedRoundPrecision = 0.001m;
        emptySrcTkrInfo.RoundingPrecision = expectedRoundPrecision;
        Assert.IsTrue(emptySrcTkrInfo.IsRoundingPrecisionUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedRoundPrecision, emptySrcTkrInfo.RoundingPrecision);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces       = BitConverter.GetBytes(decimal.GetBits(expectedRoundPrecision)[3])[2];
        var roundingNoDecimal   = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedRoundPrecision);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.PriceRoundingPrecision, roundingNoDecimal, (PQFieldFlags)decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsRoundingPrecisionUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsRoundingPrecisionUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.PriceRoundingPrecision
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedRoundPrecision, newEmpty.RoundingPrecision);
        Assert.IsTrue(newEmpty.IsRoundingPrecisionUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MinSubmitSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsMinSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrInfo.MinSubmitSize);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedMinSubmitSize = 0.01m;
        emptySrcTkrInfo.MinSubmitSize = expectedMinSubmitSize;
        Assert.IsTrue(emptySrcTkrInfo.IsMinSubmitSizeUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedMinSubmitSize, emptySrcTkrInfo.MinSubmitSize);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(expectedMinSubmitSize)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedMinSubmitSize);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.MinSubmitSize, roundingNoDecimal,
                                                    (PQFieldFlags)decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsMinSubmitSizeUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsMinSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.MinSubmitSize
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMinSubmitSize, newEmpty.MinSubmitSize);
        Assert.IsTrue(newEmpty.IsMinSubmitSizeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MaxSubmitSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsMaxSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrInfo.MaxSubmitSize);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedMaxSubmitSize = 100m;
        emptySrcTkrInfo.MaxSubmitSize = expectedMaxSubmitSize;
        Assert.IsTrue(emptySrcTkrInfo.IsMaxSubmitSizeUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedMaxSubmitSize, emptySrcTkrInfo.MaxSubmitSize);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(expectedMaxSubmitSize)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedMaxSubmitSize);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.MaxSubmitSize, roundingNoDecimal,
                                                    (PQFieldFlags)decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsMaxSubmitSizeUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsMaxSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.MaxSubmitSize
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMaxSubmitSize, newEmpty.MaxSubmitSize);
        Assert.IsTrue(newEmpty.IsMaxSubmitSizeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_IncrementSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsIncrementSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrInfo.IncrementSize);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedIncrementSize = 100m;
        emptySrcTkrInfo.IncrementSize = expectedIncrementSize;
        Assert.IsTrue(emptySrcTkrInfo.IsIncrementSizeUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedIncrementSize, emptySrcTkrInfo.IncrementSize);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(expectedIncrementSize)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedIncrementSize);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.IncrementSize, roundingNoDecimal,
                                                    (PQFieldFlags)decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsIncrementSizeUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsIncrementSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.IncrementSize
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedIncrementSize, newEmpty.IncrementSize);
        Assert.IsTrue(newEmpty.IsIncrementSizeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MinimumQuoteLifeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsMinimumQuoteLifeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0, emptySrcTkrInfo.MinimumQuoteLife);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedMinimumQuoteLife = (ushort)1000;
        emptySrcTkrInfo.MinimumQuoteLife = expectedMinimumQuoteLife;
        Assert.IsTrue(emptySrcTkrInfo.IsMinimumQuoteLifeUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedMinimumQuoteLife, emptySrcTkrInfo.MinimumQuoteLife);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.MinimumQuoteLifeMs, expectedMinimumQuoteLife);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsMinimumQuoteLifeUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsMinimumQuoteLifeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.MinimumQuoteLifeMs
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMinimumQuoteLife, newEmpty.MinimumQuoteLife);
        Assert.IsTrue(newEmpty.IsMinimumQuoteLifeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_LayerFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsLayerFlagsUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(LayerFlags.None, emptySrcTkrInfo.LayerFlags);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedLayerFlags = LayerFlags.OrderTraderName | LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderSize;
        emptySrcTkrInfo.LayerFlags = expectedLayerFlags;
        Assert.IsTrue(emptySrcTkrInfo.IsLayerFlagsUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedLayerFlags, emptySrcTkrInfo.LayerFlags);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.QuoteLayerFlags, (uint)expectedLayerFlags);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsLayerFlagsUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsLayerFlagsUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.QuoteLayerFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLayerFlags, newEmpty.LayerFlags);
        Assert.IsTrue(newEmpty.IsLayerFlagsUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MaximumPublishedLayersChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsMaximumPublishedLayersUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0, emptySrcTkrInfo.MaximumPublishedLayers);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedMaximumPublishedLayers = (byte)100;
        emptySrcTkrInfo.MaximumPublishedLayers = expectedMaximumPublishedLayers;
        Assert.IsTrue(emptySrcTkrInfo.IsMaximumPublishedLayersUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedMaximumPublishedLayers, emptySrcTkrInfo.MaximumPublishedLayers);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.MaximumPublishedLayers,
                                                    expectedMaximumPublishedLayers);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsMaximumPublishedLayersUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsMaximumPublishedLayersUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.MaximumPublishedLayers
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMaximumPublishedLayers, newEmpty.MaximumPublishedLayers);
        Assert.IsTrue(newEmpty.IsMaximumPublishedLayersUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_LastTradedFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsLastTradedFlagsUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(LastTradedFlags.None, emptySrcTkrInfo.LastTradedFlags);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        var expectedLastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice
                                                                 | LastTradedFlags.LastTradedVolume;
        emptySrcTkrInfo.LastTradedFlags = expectedLastTradedFlags;
        Assert.IsTrue(emptySrcTkrInfo.IsLastTradedFlagsUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedLastTradedFlags, emptySrcTkrInfo.LastTradedFlags);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.LastTradedFlags, (uint)expectedLastTradedFlags);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsLastTradedFlagsUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsLastTradedFlagsUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.Id == PQFeedFields.LastTradedFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLastTradedFlags, newEmpty.LastTradedFlags);
        Assert.IsTrue(newEmpty.IsLastTradedFlagsUpdated);
    }

    [TestMethod]
    public void PopulatedQuoteInfo_FormatPrice_Returns0MatchingNumberOfDecimalPlaces()
    {
        fullyPopulatedSrcTkrInfo.RoundingPrecision = 1.2345m;
        Assert.AreEqual("0.0000", fullyPopulatedSrcTkrInfo.FormatPrice);
        fullyPopulatedSrcTkrInfo.RoundingPrecision = 6m;
        Assert.AreEqual("0", fullyPopulatedSrcTkrInfo.FormatPrice);
        fullyPopulatedSrcTkrInfo.RoundingPrecision = 7890m;
        Assert.AreEqual("0000", fullyPopulatedSrcTkrInfo.FormatPrice);
    }

    [TestMethod]
    public void PopulatedQuoteInfo_GetDeltaUpdateFieldsAsUpdate_ReturnsAllQuoteInfoFields()
    {
        var pqFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        AssertSourceTickerInfoContainsAllFields(pqFieldUpdates, fullyPopulatedSrcTkrInfo);
        var pqStringFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(2, pqStringFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllQuoteInfoFields()
    {
        fullyPopulatedSrcTkrInfo.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Snapshot).ToList();
        AssertSourceTickerInfoContainsAllFields(pqFieldUpdates, fullyPopulatedSrcTkrInfo);
        var pqStringFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Snapshot).ToList();
        Assert.AreEqual(2, pqStringFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
    {
        fullyPopulatedSrcTkrInfo.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        var pqStringFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqStringFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        fullyPopulatedSrcTkrInfo.HasUpdates = true;
        var pqFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        var newEmpty = new PQSourceTickerInfo(new SourceTickerInfo(0, "", 0, "", Level3Quote, Unknown));
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        var stringFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update);
        foreach (var stringUpdate in stringFieldUpdates) newEmpty.UpdateFieldString(stringUpdate);
        Assert.AreEqual(fullyPopulatedSrcTkrInfo, newEmpty);
    }

    [TestMethod]
    public void PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerInfo()
    {
        var pqFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(ExpectedSourceStringUpdate(fullyPopulatedSrcTkrInfo.SourceName),
                        PQTickInstantTests.ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFeedFields.SourceTickerNames, 1));
        Assert.AreEqual(ExpectedTickerStringUpdate(fullyPopulatedSrcTkrInfo.InstrumentName),
                        PQTickInstantTests.ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFeedFields.SourceTickerNames, 2));
    }


    [TestMethod]
    public void EmptyQuoteInfo_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues()
    {
        var expectedNewTicker = "NewTestTickerName";
        var expectedNewSource = "NewTestSourceName";

        var tickerStringUpdate = ExpectedTickerStringUpdate(expectedNewTicker);
        var sourceStringUpdate = ExpectedSourceStringUpdate(expectedNewSource);

        emptySrcTkrInfo.UpdateField(new PQFieldUpdate(PQFeedFields.InstrumentNameId, tickerStringUpdate.StringUpdate.DictionaryId));
        emptySrcTkrInfo.UpdateFieldString(tickerStringUpdate);
        Assert.AreEqual(expectedNewTicker, emptySrcTkrInfo.InstrumentName);
        emptySrcTkrInfo.UpdateField(new PQFieldUpdate(PQFeedFields.SourceNameId, sourceStringUpdate.StringUpdate.DictionaryId));
        emptySrcTkrInfo.UpdateFieldString(sourceStringUpdate);
        Assert.AreEqual(expectedNewSource, emptySrcTkrInfo.SourceName);
    }

    [TestMethod]
    public void FullyPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptySrcTkrInfo.CopyFrom(fullyPopulatedSrcTkrInfo);

        Assert.AreEqual(fullyPopulatedSrcTkrInfo, emptySrcTkrInfo);
    }

    [TestMethod]
    public void NonPQPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualToEachOther()
    {
        var nonPQQuoteInfo = new SourceTickerInfo(fullyPopulatedSrcTkrInfo);
        emptySrcTkrInfo.CopyFrom(nonPQQuoteInfo);
        Assert.IsTrue(fullyPopulatedSrcTkrInfo.AreEquivalent(emptySrcTkrInfo));
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        var cloned = ((ISourceTickerInfo)fullyPopulatedSrcTkrInfo).Clone();
        Assert.AreEqual(fullyPopulatedSrcTkrInfo, cloned);

        var cloned2 = (ISourceTickerInfo)((ICloneable)fullyPopulatedSrcTkrInfo).Clone();
        Assert.AreEqual(fullyPopulatedSrcTkrInfo, cloned2);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQSourceTickerInfo)fullyPopulatedSrcTkrInfo.Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedSrcTkrInfo, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedSrcTkrInfo, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(fullyPopulatedSrcTkrInfo, fullyPopulatedSrcTkrInfo);
        Assert.AreEqual(fullyPopulatedSrcTkrInfo, ((ISourceTickerInfo)fullyPopulatedSrcTkrInfo).Clone());
    }

    [TestMethod]
    public void EmptyQuote_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = emptySrcTkrInfo.GetHashCode();
        hashCode = fullyPopulatedSrcTkrInfo.GetHashCode();
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQSourceTickerInfo original, IPQSourceTickerInfo changingSrcTkrInfo)
    {
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo));
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original));

        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(null, exactComparison));
        Assert.AreEqual(!exactComparison,
                        changingSrcTkrInfo.AreEquivalent(new SourceTickerInfo(original)
                                                       , exactComparison));

        changingSrcTkrInfo.RoundingPrecision = 1.2345678m;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
        changingSrcTkrInfo.RoundingPrecision          = original.RoundingPrecision;
        changingSrcTkrInfo.IsRoundingPrecisionUpdated = original.IsRoundingPrecisionUpdated;
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrInfo.MinSubmitSize = 9.8765432m;
        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrInfo.MinSubmitSize          = original.MinSubmitSize;
        changingSrcTkrInfo.IsMinSubmitSizeUpdated = original.IsMinSubmitSizeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo, exactComparison));

        changingSrcTkrInfo.MaxSubmitSize = 1.2345678m;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
        changingSrcTkrInfo.MaxSubmitSize          = original.MaxSubmitSize;
        changingSrcTkrInfo.IsMaxSubmitSizeUpdated = original.IsMaxSubmitSizeUpdated;
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrInfo.IncrementSize = 9.8765432m;
        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrInfo.IncrementSize          = original.IncrementSize;
        changingSrcTkrInfo.IsIncrementSizeUpdated = original.IsIncrementSizeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo, exactComparison));

        changingSrcTkrInfo.MinimumQuoteLife = 1000;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
        changingSrcTkrInfo.MinimumQuoteLife          = original.MinimumQuoteLife;
        changingSrcTkrInfo.IsMinimumQuoteLifeUpdated = original.IsMinimumQuoteLifeUpdated;
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrInfo.LayerFlags ^= LayerFlags.Volume.AllFlags();
        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrInfo.LayerFlags          = original.LayerFlags;
        changingSrcTkrInfo.IsLayerFlagsUpdated = original.IsLayerFlagsUpdated;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo, exactComparison));

        changingSrcTkrInfo.MaximumPublishedLayers = 100;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
        changingSrcTkrInfo.MaximumPublishedLayers          = original.MaximumPublishedLayers;
        changingSrcTkrInfo.IsMaximumPublishedLayersUpdated = original.IsMaximumPublishedLayersUpdated;
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrInfo.LastTradedFlags ^= changingSrcTkrInfo.LastTradedFlags.AllFlags();
        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrInfo.LastTradedFlags          = original.LastTradedFlags;
        changingSrcTkrInfo.IsLastTradedFlagsUpdated = original.IsLastTradedFlagsUpdated;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
    }

    public static void AssertSourceTickerInfoContainsAllFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        IPQSourceTickerInfo srcTkrInfo)
    {
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.SourceTickerId, srcTkrInfo.SourceTickerId),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.SourceTickerId),
                        $"For {srcTkrInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(
                                                                  srcTkrInfo.RoundingPrecision)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                       srcTkrInfo.RoundingPrecision);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.PriceRoundingPrecision, roundingNoDecimal, (PQFieldFlags)decimalPlaces),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.PriceRoundingPrecision),
                        $"For {srcTkrInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        decimalPlaces = BitConverter.GetBytes(decimal.GetBits(srcTkrInfo.MinSubmitSize)[3])[2];
        var minSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                        srcTkrInfo.MinSubmitSize);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.MinSubmitSize, minSubmitNoDecimal, (PQFieldFlags)decimalPlaces),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.MinSubmitSize),
                        $"For {srcTkrInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        decimalPlaces = BitConverter.GetBytes(decimal.GetBits(srcTkrInfo.MaxSubmitSize)[3])[2];
        var maxSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                        srcTkrInfo.MaxSubmitSize);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.MaxSubmitSize, maxSubmitNoDecimal, (PQFieldFlags)decimalPlaces),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.MaxSubmitSize),
                        $"For {srcTkrInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        decimalPlaces = BitConverter.GetBytes(decimal.GetBits(srcTkrInfo.IncrementSize)[3])[2];
        var incrementSizeNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                            srcTkrInfo.IncrementSize);
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.IncrementSize, incrementSizeNoDecimal, (PQFieldFlags)decimalPlaces),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.IncrementSize),
                        $"For {srcTkrInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.QuoteLayerFlags, (uint)srcTkrInfo.LayerFlags),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.QuoteLayerFlags),
                        $"For {srcTkrInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
        if (srcTkrInfo.IsLastTradedFlagsUpdated)
            Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedFlags, (uint)srcTkrInfo.LastTradedFlags),
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFeedFields.LastTradedFlags),
                            $"For {srcTkrInfo} with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }


    public static PQFieldStringUpdate ExpectedSourceStringUpdate(string sourceValue) =>
        new()
        {
            Field = new PQFieldUpdate(PQFeedFields.SourceTickerNames, CrudCommand.Upsert.ToPQSubFieldId(), 0)
          , StringUpdate = new PQStringUpdate
            {
                DictionaryId = 1, Value = sourceValue, Command = CrudCommand.Upsert
            }
        };


    public static PQFieldStringUpdate ExpectedTickerStringUpdate(string tickerValue) =>
        new()
        {
            Field = new PQFieldUpdate(PQFeedFields.SourceTickerNames, CrudCommand.Upsert.ToPQSubFieldId(), 0)
          , StringUpdate = new PQStringUpdate
            {
                DictionaryId = 2, Value = tickerValue, Command = CrudCommand.Upsert
            }
        };
}
