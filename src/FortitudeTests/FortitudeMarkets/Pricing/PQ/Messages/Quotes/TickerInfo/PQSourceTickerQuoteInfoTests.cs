﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.TickerInfo;

[TestClass]
public class PQSourceTickerInfoTests
{
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
                   , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
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
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedId = ushort.MaxValue;
        emptySrcTkrInfo.SourceId = expectedId;
        Assert.IsTrue(emptySrcTkrInfo.IsIdUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        var expectedStreamId = (uint)expectedId << 16;
        Assert.AreEqual(expectedStreamId, emptySrcTkrInfo.SourceTickerId);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.SourceTickerId, expectedStreamId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsIdUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsIdUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.SourceTickerId
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
        Assert.IsFalse(emptySrcTkrInfo.IsSourceUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual("", emptySrcTkrInfo.Source);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedSource = "NewSourceName";
        emptySrcTkrInfo.Source = expectedSource;
        Assert.IsTrue(emptySrcTkrInfo.IsSourceUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedSource, emptySrcTkrInfo.Source);
        var sourceUpdates = emptySrcTkrInfo.GetStringUpdates(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = ExpectedSourceStringUpdate(expectedSource);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsSourceUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsSourceUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetStringUpdates(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetStringUpdates(testDateTime, StorageFlags.Snapshot)
            where update.Field.Id == PQFieldKeys.SourceTickerNames && update.StringUpdate.DictionaryId == 0
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateFieldString(sourceUpdates[0]);
        Assert.AreEqual(expectedSource, newEmpty.Source);
        Assert.IsFalse(newEmpty.IsSourceUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_TickerChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsTickerUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual("", emptySrcTkrInfo.Ticker);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedTicker = "NewTickerName";
        emptySrcTkrInfo.Ticker = expectedTicker;
        Assert.IsTrue(emptySrcTkrInfo.IsTickerUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedTicker, emptySrcTkrInfo.Ticker);
        var tickerUpdates = emptySrcTkrInfo.GetStringUpdates(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, tickerUpdates.Count);
        var expectedFieldUpdate = ExpectedTickerStringUpdate(expectedTicker);
        Assert.AreEqual(expectedFieldUpdate, tickerUpdates[0]);

        emptySrcTkrInfo.IsTickerUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsTickerUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetStringUpdates(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        tickerUpdates = (from update in emptySrcTkrInfo.GetStringUpdates(testDateTime, StorageFlags.Snapshot)
            where update.Field.Id == PQFieldKeys.SourceTickerNames && update.StringUpdate.DictionaryId == 1
            select update).ToList();
        Assert.AreEqual(1, tickerUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, tickerUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateFieldString(tickerUpdates[0]);
        Assert.AreEqual(expectedTicker, newEmpty.Ticker);
        Assert.IsFalse(newEmpty.IsTickerUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_RoundingPrecisionChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsRoundingPrecisionUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrInfo.RoundingPrecision);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedRoundPrecision = 0.001m;
        emptySrcTkrInfo.RoundingPrecision = expectedRoundPrecision;
        Assert.IsTrue(emptySrcTkrInfo.IsRoundingPrecisionUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedRoundPrecision, emptySrcTkrInfo.RoundingPrecision);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(expectedRoundPrecision)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedRoundPrecision);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.PriceRoundingPrecision, roundingNoDecimal,
                                                    decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsRoundingPrecisionUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsRoundingPrecisionUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.PriceRoundingPrecision
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedRoundPrecision, newEmpty.RoundingPrecision);
        Assert.IsFalse(newEmpty.IsRoundingPrecisionUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MinSubmitSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsMinSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrInfo.MinSubmitSize);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedMinSubmitSize = 0.01m;
        emptySrcTkrInfo.MinSubmitSize = expectedMinSubmitSize;
        Assert.IsTrue(emptySrcTkrInfo.IsMinSubmitSizeUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedMinSubmitSize, emptySrcTkrInfo.MinSubmitSize);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(expectedMinSubmitSize)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedMinSubmitSize);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.MinSubmitSize, roundingNoDecimal,
                                                    decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsMinSubmitSizeUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsMinSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.MinSubmitSize
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMinSubmitSize, newEmpty.MinSubmitSize);
        Assert.IsFalse(newEmpty.IsMinSubmitSizeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MaxSubmitSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsMaxSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrInfo.MaxSubmitSize);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedMaxSubmitSize = 100m;
        emptySrcTkrInfo.MaxSubmitSize = expectedMaxSubmitSize;
        Assert.IsTrue(emptySrcTkrInfo.IsMaxSubmitSizeUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedMaxSubmitSize, emptySrcTkrInfo.MaxSubmitSize);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(expectedMaxSubmitSize)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedMaxSubmitSize);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.MaxSubmitSize, roundingNoDecimal,
                                                    decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsMaxSubmitSizeUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsMaxSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.MaxSubmitSize
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMaxSubmitSize, newEmpty.MaxSubmitSize);
        Assert.IsFalse(newEmpty.IsMaxSubmitSizeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_IncrementSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsIncrementSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrInfo.IncrementSize);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedIncrementSize = 100m;
        emptySrcTkrInfo.IncrementSize = expectedIncrementSize;
        Assert.IsTrue(emptySrcTkrInfo.IsIncrementSizeUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedIncrementSize, emptySrcTkrInfo.IncrementSize);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces     = BitConverter.GetBytes(decimal.GetBits(expectedIncrementSize)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedIncrementSize);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.IncrementSize, roundingNoDecimal,
                                                    decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsIncrementSizeUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsIncrementSizeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.IncrementSize
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedIncrementSize, newEmpty.IncrementSize);
        Assert.IsFalse(newEmpty.IsIncrementSizeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MinimumQuoteLifeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsMinimumQuoteLifeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0, emptySrcTkrInfo.MinimumQuoteLife);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedMinimumQuoteLife = (ushort)1000;
        emptySrcTkrInfo.MinimumQuoteLife = expectedMinimumQuoteLife;
        Assert.IsTrue(emptySrcTkrInfo.IsMinimumQuoteLifeUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedMinimumQuoteLife, emptySrcTkrInfo.MinimumQuoteLife);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.MinimumQuoteLifeMs, expectedMinimumQuoteLife);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsMinimumQuoteLifeUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsMinimumQuoteLifeUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.MinimumQuoteLifeMs
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMinimumQuoteLife, newEmpty.MinimumQuoteLife);
        Assert.IsFalse(newEmpty.IsMinimumQuoteLifeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_LayerFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsLayerFlagsUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(LayerFlags.None, emptySrcTkrInfo.LayerFlags);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedLayerFlags = LayerFlags.TraderName | LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderSize;
        emptySrcTkrInfo.LayerFlags = expectedLayerFlags;
        Assert.IsTrue(emptySrcTkrInfo.IsLayerFlagsUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedLayerFlags, emptySrcTkrInfo.LayerFlags);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerFlags, (uint)expectedLayerFlags);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsLayerFlagsUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsLayerFlagsUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.LayerFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLayerFlags, newEmpty.LayerFlags);
        Assert.IsFalse(newEmpty.IsLayerFlagsUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MaximumPublishedLayersChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsMaximumPublishedLayersUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(0, emptySrcTkrInfo.MaximumPublishedLayers);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedMaximumPublishedLayers = (byte)100;
        emptySrcTkrInfo.MaximumPublishedLayers = expectedMaximumPublishedLayers;
        Assert.IsTrue(emptySrcTkrInfo.IsMaximumPublishedLayersUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedMaximumPublishedLayers, emptySrcTkrInfo.MaximumPublishedLayers);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.MaximumPublishedLayers,
                                                    expectedMaximumPublishedLayers);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsMaximumPublishedLayersUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsMaximumPublishedLayersUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.MaximumPublishedLayers
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMaximumPublishedLayers, newEmpty.MaximumPublishedLayers);
        Assert.IsFalse(newEmpty.IsMaximumPublishedLayersUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_LastTradedFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrInfo.IsLastTradedFlagsUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(LastTradedFlags.None, emptySrcTkrInfo.LastTradedFlags);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        var expectedLastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice
                                                                 | LastTradedFlags.LastTradedVolume;
        emptySrcTkrInfo.LastTradedFlags = expectedLastTradedFlags;
        Assert.IsTrue(emptySrcTkrInfo.IsLastTradedFlagsUpdated);
        Assert.IsTrue(emptySrcTkrInfo.HasUpdates);
        Assert.AreEqual(expectedLastTradedFlags, emptySrcTkrInfo.LastTradedFlags);
        var sourceUpdates = emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LastTradedFlags, (uint)expectedLastTradedFlags);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrInfo.IsLastTradedFlagsUpdated = false;
        Assert.IsFalse(emptySrcTkrInfo.IsLastTradedFlagsUpdated);
        Assert.IsFalse(emptySrcTkrInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrInfo.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.Id == PQFieldKeys.LastTradedFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerInfo(emptySrcTkrInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLastTradedFlags, newEmpty.LastTradedFlags);
        Assert.IsFalse(newEmpty.IsLastTradedFlagsUpdated);
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
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        AssertSourceTickerInfoContainsAllFields(pqFieldUpdates, fullyPopulatedSrcTkrInfo);
        var pqStringFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(2, pqStringFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllQuoteInfoFields()
    {
        fullyPopulatedSrcTkrInfo.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Snapshot).ToList();
        AssertSourceTickerInfoContainsAllFields(pqFieldUpdates, fullyPopulatedSrcTkrInfo);
        var pqStringFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Snapshot).ToList();
        Assert.AreEqual(2, pqStringFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
    {
        fullyPopulatedSrcTkrInfo.HasUpdates = false;
        var pqFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        var pqStringFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqStringFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        fullyPopulatedSrcTkrInfo.HasUpdates = true;
        var pqFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        var newEmpty = new PQSourceTickerInfo(new SourceTickerInfo(0, "", 0, "", Level3Quote, Unknown));
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        var stringFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update);
        foreach (var stringUpdate in stringFieldUpdates) newEmpty.UpdateFieldString(stringUpdate);
        Assert.AreEqual(fullyPopulatedSrcTkrInfo, newEmpty);
    }

    [TestMethod]
    public void PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerInfo()
    {
        var pqFieldUpdates =
            fullyPopulatedSrcTkrInfo.GetStringUpdates
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(ExpectedSourceStringUpdate(fullyPopulatedSrcTkrInfo.Source),
                        PQTickInstantTests.ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFieldKeys.SourceTickerNames, 0));
        Assert.AreEqual(ExpectedTickerStringUpdate(fullyPopulatedSrcTkrInfo.Ticker),
                        PQTickInstantTests.ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFieldKeys.SourceTickerNames, 1));
    }

    [TestMethod]
    public void EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateField_ReturnsSizeFoundInField()
    {
        var expectedSize         = 37;
        var pqStringFieldSize    = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, expectedSize);
        var sizeToReadFromBuffer = emptySrcTkrInfo.UpdateField(pqStringFieldSize);
        Assert.AreEqual(expectedSize, sizeToReadFromBuffer);
    }

    [TestMethod]
    public void EmptyQuoteInfo_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues()
    {
        var expectedNewTicker = "NewTestTickerName";
        var expectedNewSource = "NewTestSourceName";

        var tickerStringUpdate = ExpectedTickerStringUpdate(expectedNewTicker);
        var sourceStringUpdate = ExpectedSourceStringUpdate(expectedNewSource);

        emptySrcTkrInfo.UpdateFieldString(tickerStringUpdate);
        Assert.AreEqual(expectedNewTicker, emptySrcTkrInfo.Ticker);
        emptySrcTkrInfo.UpdateFieldString(sourceStringUpdate);
        Assert.AreEqual(expectedNewSource, emptySrcTkrInfo.Source);
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
        PQSourceTickerInfo original, PQSourceTickerInfo changingSrcTkrInfo)
    {
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo));
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original));

        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(null, exactComparison));
        Assert.AreEqual(!exactComparison,
                        changingSrcTkrInfo.AreEquivalent(new SourceTickerInfo(original)
                                                       , exactComparison));

        changingSrcTkrInfo.RoundingPrecision = 1.2345678m;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
        changingSrcTkrInfo.RoundingPrecision = original.RoundingPrecision;
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrInfo.MinSubmitSize = 9.8765432m;
        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrInfo.MinSubmitSize = original.MinSubmitSize;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo, exactComparison));

        changingSrcTkrInfo.MaxSubmitSize = 1.2345678m;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
        changingSrcTkrInfo.MaxSubmitSize = original.MaxSubmitSize;
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrInfo.IncrementSize = 9.8765432m;
        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrInfo.IncrementSize = original.IncrementSize;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo, exactComparison));

        changingSrcTkrInfo.MinimumQuoteLife = 1000;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
        changingSrcTkrInfo.MinimumQuoteLife = original.MinimumQuoteLife;
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrInfo.LayerFlags ^= LayerFlags.Volume.AllFlags();
        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrInfo.LayerFlags = original.LayerFlags;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo, exactComparison));

        changingSrcTkrInfo.MaximumPublishedLayers = 100;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
        changingSrcTkrInfo.MaximumPublishedLayers = original.MaximumPublishedLayers;
        Assert.IsTrue(changingSrcTkrInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrInfo.LastTradedFlags ^= changingSrcTkrInfo.LastTradedFlags.AllFlags();
        Assert.IsFalse(changingSrcTkrInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrInfo.LastTradedFlags          = original.LastTradedFlags;
        changingSrcTkrInfo.IsLastTradedFlagsUpdated = original.IsLastTradedFlagsUpdated;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrInfo, exactComparison));
    }

    public static void AssertSourceTickerInfoContainsAllFields
    (IList<PQFieldUpdate> checkFieldUpdates,
        ISourceTickerInfo srcTkrInfo)
    {
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceTickerId, srcTkrInfo.SourceTickerId),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceTickerId));
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(
                                                                  srcTkrInfo.RoundingPrecision)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                       srcTkrInfo.RoundingPrecision);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.PriceRoundingPrecision, roundingNoDecimal, decimalPlaces),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.PriceRoundingPrecision));
        decimalPlaces = BitConverter.GetBytes(decimal.GetBits(srcTkrInfo.MinSubmitSize)[3])[2];
        var minSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                        srcTkrInfo.MinSubmitSize);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.MinSubmitSize, minSubmitNoDecimal, decimalPlaces),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.MinSubmitSize));
        decimalPlaces = BitConverter.GetBytes(decimal.GetBits(srcTkrInfo.MaxSubmitSize)[3])[2];
        var maxSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                        srcTkrInfo.MaxSubmitSize);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.MaxSubmitSize, maxSubmitNoDecimal, decimalPlaces),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.MaxSubmitSize));
        decimalPlaces = BitConverter.GetBytes(decimal.GetBits(srcTkrInfo.IncrementSize)[3])[2];
        var incrementSizeNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                            srcTkrInfo.IncrementSize);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.IncrementSize, incrementSizeNoDecimal, decimalPlaces),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.IncrementSize));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerFlags, (uint)srcTkrInfo.LayerFlags),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.LayerFlags));
        if (srcTkrInfo.LastTradedFlags != LastTradedFlags.None)
            Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LastTradedFlags, (uint)srcTkrInfo.LastTradedFlags),
                            PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.LastTradedFlags));
    }


    public static PQFieldStringUpdate ExpectedSourceStringUpdate(string sourceValue) =>
        new()
        {
            Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpsert), StringUpdate
                = new PQStringUpdate
                {
                    DictionaryId = 0, Value = sourceValue, Command = CrudCommand.Upsert
                }
        };


    public static PQFieldStringUpdate ExpectedTickerStringUpdate(string tickerValue) =>
        new()
        {
            Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpsert), StringUpdate
                = new PQStringUpdate
                {
                    DictionaryId = 1, Value = tickerValue, Command = CrudCommand.Upsert
                }
        };
}
