#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

[TestClass]
public class PQSourceTickerQuoteInfoTests
{
    private PQSourceTickerQuoteInfo emptySrcTkrQtInfo = null!;
    private PQSourceTickerQuoteInfo fullyPopulatedSrcTkrQtInfo = null!;

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        fullyPopulatedSrcTkrQtInfo = new PQSourceTickerQuoteInfo(new SourceTickerQuoteInfo(
            ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
            LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
            | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                                                  | LastTradedFlags.LastTradedVolume |
                                                                  LastTradedFlags.LastTradedTime));
        emptySrcTkrQtInfo = new PQSourceTickerQuoteInfo(new SourceTickerQuoteInfo(0, "", 0, "", 0, 0m, 0m,
            0m, 0m, 0, LayerFlags.None));

        testDateTime = new DateTime(2017, 11, 07, 18, 33, 24);
    }

    [TestMethod]
    public void EmptyQuoteInfo_IdChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsIdUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(0u, emptySrcTkrQtInfo.Id);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedId = ushort.MaxValue;
        emptySrcTkrQtInfo.SourceId = expectedId;
        Assert.IsTrue(emptySrcTkrQtInfo.IsIdUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        var expectedStreamId = (uint)expectedId << 16;
        Assert.AreEqual(expectedStreamId, emptySrcTkrQtInfo.Id);
        var sourceUpdates = emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.SourceTickerId, expectedStreamId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsIdUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsIdUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
            where update.Id == PQFieldKeys.SourceTickerId
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedStreamId, newEmpty.Id);
        Assert.IsFalse(newEmpty.IsIdUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_SourceChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsSourceUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual("", emptySrcTkrQtInfo.Source);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedSource = "NewSourceName";
        emptySrcTkrQtInfo.Source = expectedSource;
        Assert.IsTrue(emptySrcTkrQtInfo.IsSourceUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedSource, emptySrcTkrQtInfo.Source);
        var sourceUpdates = emptySrcTkrQtInfo.GetStringUpdates(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = ExpectedSourceStringUpdate(expectedSource);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsSourceUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsSourceUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetStringUpdates(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetStringUpdates(testDateTime, UpdateStyle.FullSnapshot)
            where update.Field.Id == PQFieldKeys.SourceTickerNames && update.StringUpdate.DictionaryId == 0
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateFieldString(sourceUpdates[0]);
        Assert.AreEqual(expectedSource, newEmpty.Source);
        Assert.IsFalse(newEmpty.IsSourceUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_TickerChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsTickerUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual("", emptySrcTkrQtInfo.Ticker);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedTicker = "NewTickerName";
        emptySrcTkrQtInfo.Ticker = expectedTicker;
        Assert.IsTrue(emptySrcTkrQtInfo.IsTickerUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedTicker, emptySrcTkrQtInfo.Ticker);
        var tickerUpdates = emptySrcTkrQtInfo.GetStringUpdates(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, tickerUpdates.Count);
        var expectedFieldUpdate = ExpectedTickerStringUpdate(expectedTicker);
        Assert.AreEqual(expectedFieldUpdate, tickerUpdates[0]);

        emptySrcTkrQtInfo.IsTickerUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsTickerUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetStringUpdates(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        tickerUpdates = (from update in emptySrcTkrQtInfo.GetStringUpdates(testDateTime, UpdateStyle.FullSnapshot)
            where update.Field.Id == PQFieldKeys.SourceTickerNames && update.StringUpdate.DictionaryId == 1
            select update).ToList();
        Assert.AreEqual(1, tickerUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, tickerUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateFieldString(tickerUpdates[0]);
        Assert.AreEqual(expectedTicker, newEmpty.Ticker);
        Assert.IsFalse(newEmpty.IsTickerUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_RoundingPrecisionChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsRoundingPrecisionUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrQtInfo.RoundingPrecision);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedRoundPrecision = 0.001m;
        emptySrcTkrQtInfo.RoundingPrecision = expectedRoundPrecision;
        Assert.IsTrue(emptySrcTkrQtInfo.IsRoundingPrecisionUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedRoundPrecision, emptySrcTkrQtInfo.RoundingPrecision);
        var sourceUpdates = emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(expectedRoundPrecision)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedRoundPrecision);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.RoundingPrecision, roundingNoDecimal,
            decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsRoundingPrecisionUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsRoundingPrecisionUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
            where update.Id == PQFieldKeys.RoundingPrecision
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedRoundPrecision, newEmpty.RoundingPrecision);
        Assert.IsFalse(newEmpty.IsRoundingPrecisionUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MinSubmitSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsMinSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrQtInfo.MinSubmitSize);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedMinSubmitSize = 0.01m;
        emptySrcTkrQtInfo.MinSubmitSize = expectedMinSubmitSize;
        Assert.IsTrue(emptySrcTkrQtInfo.IsMinSubmitSizeUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedMinSubmitSize, emptySrcTkrQtInfo.MinSubmitSize);
        var sourceUpdates = emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(expectedMinSubmitSize)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedMinSubmitSize);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.MinSubmitSize, roundingNoDecimal,
            decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsMinSubmitSizeUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsMinSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
            where update.Id == PQFieldKeys.MinSubmitSize
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMinSubmitSize, newEmpty.MinSubmitSize);
        Assert.IsFalse(newEmpty.IsMinSubmitSizeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MaxSubmitSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsMaxSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrQtInfo.MaxSubmitSize);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedMaxSubmitSize = 100m;
        emptySrcTkrQtInfo.MaxSubmitSize = expectedMaxSubmitSize;
        Assert.IsTrue(emptySrcTkrQtInfo.IsMaxSubmitSizeUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedMaxSubmitSize, emptySrcTkrQtInfo.MaxSubmitSize);
        var sourceUpdates = emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(expectedMaxSubmitSize)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedMaxSubmitSize);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.MaxSubmitSize, roundingNoDecimal,
            decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsMaxSubmitSizeUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsMaxSubmitSizeUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
            where update.Id == PQFieldKeys.MaxSubmitSize
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMaxSubmitSize, newEmpty.MaxSubmitSize);
        Assert.IsFalse(newEmpty.IsMaxSubmitSizeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_IncrementSizeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsIncrementSizeUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(0m, emptySrcTkrQtInfo.IncrementSize);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedIncrementSize = 100m;
        emptySrcTkrQtInfo.IncrementSize = expectedIncrementSize;
        Assert.IsTrue(emptySrcTkrQtInfo.IsIncrementSizeUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedIncrementSize, emptySrcTkrQtInfo.IncrementSize);
        var sourceUpdates = emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(expectedIncrementSize)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) * expectedIncrementSize);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.IncrementSize, roundingNoDecimal,
            decimalPlaces);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsIncrementSizeUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsIncrementSizeUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
            where update.Id == PQFieldKeys.IncrementSize
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedIncrementSize, newEmpty.IncrementSize);
        Assert.IsFalse(newEmpty.IsIncrementSizeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MinimumQuoteLifeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsMinimumQuoteLifeUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(0, emptySrcTkrQtInfo.MinimumQuoteLife);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedMinimumQuoteLife = (ushort)1000;
        emptySrcTkrQtInfo.MinimumQuoteLife = expectedMinimumQuoteLife;
        Assert.IsTrue(emptySrcTkrQtInfo.IsMinimumQuoteLifeUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedMinimumQuoteLife, emptySrcTkrQtInfo.MinimumQuoteLife);
        var sourceUpdates = emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.MinimumQuoteLife, expectedMinimumQuoteLife);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsMinimumQuoteLifeUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsMinimumQuoteLifeUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
            where update.Id == PQFieldKeys.MinimumQuoteLife
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMinimumQuoteLife, newEmpty.MinimumQuoteLife);
        Assert.IsFalse(newEmpty.IsMinimumQuoteLifeUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_LayerFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsLayerFlagsUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(LayerFlags.None, emptySrcTkrQtInfo.LayerFlags);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedLayerFlags = LayerFlags.TraderName | LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderSize;
        emptySrcTkrQtInfo.LayerFlags = expectedLayerFlags;
        Assert.IsTrue(emptySrcTkrQtInfo.IsLayerFlagsUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedLayerFlags, emptySrcTkrQtInfo.LayerFlags);
        var sourceUpdates = emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LayerFlags, (uint)expectedLayerFlags);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsLayerFlagsUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsLayerFlagsUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
            where update.Id == PQFieldKeys.LayerFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLayerFlags, newEmpty.LayerFlags);
        Assert.IsFalse(newEmpty.IsLayerFlagsUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_MaximumPublishedLayersChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsMaximumPublishedLayersUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(0, emptySrcTkrQtInfo.MaximumPublishedLayers);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedMaximumPublishedLayers = (byte)100;
        emptySrcTkrQtInfo.MaximumPublishedLayers = expectedMaximumPublishedLayers;
        Assert.IsTrue(emptySrcTkrQtInfo.IsMaximumPublishedLayersUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedMaximumPublishedLayers, emptySrcTkrQtInfo.MaximumPublishedLayers);
        var sourceUpdates = emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.MaximumPublishedLayers,
            expectedMaximumPublishedLayers);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsMaximumPublishedLayersUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsMaximumPublishedLayersUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
            where update.Id == PQFieldKeys.MaximumPublishedLayers
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedMaximumPublishedLayers, newEmpty.MaximumPublishedLayers);
        Assert.IsFalse(newEmpty.IsMaximumPublishedLayersUpdated);
    }

    [TestMethod]
    public void EmptyQuoteInfo_LastTradedFlagsChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptySrcTkrQtInfo.IsLastTradedFlagsUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(LastTradedFlags.None, emptySrcTkrQtInfo.LastTradedFlags);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        var expectedLastTradedFlags = LastTradedFlags.TraderName | LastTradedFlags.LastTradedPrice
                                                                 | LastTradedFlags.LastTradedVolume;
        emptySrcTkrQtInfo.LastTradedFlags = expectedLastTradedFlags;
        Assert.IsTrue(emptySrcTkrQtInfo.IsLastTradedFlagsUpdated);
        Assert.IsTrue(emptySrcTkrQtInfo.HasUpdates);
        Assert.AreEqual(expectedLastTradedFlags, emptySrcTkrQtInfo.LastTradedFlags);
        var sourceUpdates = emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        var expectedFieldUpdate = new PQFieldUpdate(PQFieldKeys.LastTradedFlags, (uint)expectedLastTradedFlags);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptySrcTkrQtInfo.IsLastTradedFlagsUpdated = false;
        Assert.IsFalse(emptySrcTkrQtInfo.IsLastTradedFlagsUpdated);
        Assert.IsFalse(emptySrcTkrQtInfo.HasUpdates);
        Assert.IsTrue(emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.Updates).IsNullOrEmpty());

        sourceUpdates = (from update in emptySrcTkrQtInfo.GetDeltaUpdateFields(testDateTime, UpdateStyle.FullSnapshot)
            where update.Id == PQFieldKeys.LastTradedFlags
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQSourceTickerQuoteInfo(emptySrcTkrQtInfo);
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(expectedLastTradedFlags, newEmpty.LastTradedFlags);
        Assert.IsFalse(newEmpty.IsLastTradedFlagsUpdated);
    }

    [TestMethod]
    public void PopulatedQuoteInfo_FormatPrice_Returns0MatchingNumberOfDecimalPlaces()
    {
        fullyPopulatedSrcTkrQtInfo.RoundingPrecision = 1.2345m;
        Assert.AreEqual("0.0000", fullyPopulatedSrcTkrQtInfo.FormatPrice);
        fullyPopulatedSrcTkrQtInfo.RoundingPrecision = 6m;
        Assert.AreEqual("0", fullyPopulatedSrcTkrQtInfo.FormatPrice);
        fullyPopulatedSrcTkrQtInfo.RoundingPrecision = 7890m;
        Assert.AreEqual("0000", fullyPopulatedSrcTkrQtInfo.FormatPrice);
    }

    [TestMethod]
    public void PopulatedQuoteInfo_GetDeltaUpdateFieldsAsUpdate_ReturnsAllQuoteInfoFields()
    {
        var pqFieldUpdates = fullyPopulatedSrcTkrQtInfo.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
        AssertSourceTickerInfoContainsAllFields(pqFieldUpdates, fullyPopulatedSrcTkrQtInfo);
        var pqStringFieldUpdates = fullyPopulatedSrcTkrQtInfo.GetStringUpdates(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
        Assert.AreEqual(2, pqStringFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllQuoteInfoFields()
    {
        fullyPopulatedSrcTkrQtInfo.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedSrcTkrQtInfo.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.FullSnapshot).ToList();
        AssertSourceTickerInfoContainsAllFields(pqFieldUpdates, fullyPopulatedSrcTkrQtInfo);
        var pqStringFieldUpdates = fullyPopulatedSrcTkrQtInfo.GetStringUpdates(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.FullSnapshot).ToList();
        Assert.AreEqual(2, pqStringFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuoteInfoWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoFields()
    {
        fullyPopulatedSrcTkrQtInfo.HasUpdates = false;
        var pqFieldUpdates = fullyPopulatedSrcTkrQtInfo.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
        var pqStringFieldUpdates = fullyPopulatedSrcTkrQtInfo.GetStringUpdates(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
        Assert.AreEqual(0, pqStringFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedQuote_GetDeltaUpdatesUpdateThenUpdateFieldNewQuote_CopiesAllFieldsToNewQuote()
    {
        fullyPopulatedSrcTkrQtInfo.HasUpdates = true;
        var pqFieldUpdates = fullyPopulatedSrcTkrQtInfo.GetDeltaUpdateFields(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
        var newEmpty = new PQSourceTickerQuoteInfo(new SourceTickerQuoteInfo(0, "", 0, ""));
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        var stringFieldUpdates = fullyPopulatedSrcTkrQtInfo.GetStringUpdates(new DateTime(2017, 11, 04, 16, 33, 59),
            UpdateStyle.Updates);
        foreach (var stringUpdate in stringFieldUpdates) newEmpty.UpdateFieldString(stringUpdate);
        Assert.AreEqual(fullyPopulatedSrcTkrQtInfo, newEmpty);
    }

    [TestMethod]
    public void PopulatedQuote_GetStringUpdates_GetsSourceAndTickerFromSourceTickerQuoteInfo()
    {
        var pqFieldUpdates = fullyPopulatedSrcTkrQtInfo.GetStringUpdates(
            new DateTime(2017, 11, 04, 16, 33, 59), UpdateStyle.Updates).ToList();
        Assert.AreEqual(ExpectedSourceStringUpdate(
                fullyPopulatedSrcTkrQtInfo.Source),
            PQLevel0QuoteTests.ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFieldKeys.SourceTickerNames, 0));
        Assert.AreEqual(ExpectedTickerStringUpdate(
                fullyPopulatedSrcTkrQtInfo.Ticker),
            PQLevel0QuoteTests.ExtractFieldStringUpdateWithId(pqFieldUpdates, PQFieldKeys.SourceTickerNames, 1));
    }

    [TestMethod]
    public void EmptyQuote_ReceiveSourceTickerStringFieldUpdateInUpdateField_ReturnsSizeFoundInField()
    {
        var expectedSize = 37;
        var pqStringFieldSize = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, expectedSize);
        var sizeToReadFromBuffer = emptySrcTkrQtInfo.UpdateField(pqStringFieldSize);
        Assert.AreEqual(expectedSize, sizeToReadFromBuffer);
    }

    [TestMethod]
    public void EmptyQuoteInfo_ReceiveSourceTickerStringFieldUpdateInUpdateFieldString_UpdatesStringValues()
    {
        var expectedNewTicker = "NewTestTickerName";
        var expectedNewSource = "NewTestSourceName";

        var tickerStringUpdate = ExpectedTickerStringUpdate(expectedNewTicker);
        var sourceStringUpdate = ExpectedSourceStringUpdate(expectedNewSource);

        emptySrcTkrQtInfo.UpdateFieldString(tickerStringUpdate);
        Assert.AreEqual(expectedNewTicker, emptySrcTkrQtInfo.Ticker);
        emptySrcTkrQtInfo.UpdateFieldString(sourceStringUpdate);
        Assert.AreEqual(expectedNewSource, emptySrcTkrQtInfo.Source);
    }

    [TestMethod]
    public void FullyPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualEachOther()
    {
        emptySrcTkrQtInfo.CopyFrom(fullyPopulatedSrcTkrQtInfo);

        Assert.AreEqual(fullyPopulatedSrcTkrQtInfo, emptySrcTkrQtInfo);
    }

    [TestMethod]
    public void NonPQPopulatedQuoteInfo_CopyFromToEmptyQuote_QuotesEqualToEachOther()
    {
        var nonPQQuoteInfo = new SourceTickerQuoteInfo(fullyPopulatedSrcTkrQtInfo);
        emptySrcTkrQtInfo.CopyFrom(nonPQQuoteInfo);
        Assert.IsTrue(fullyPopulatedSrcTkrQtInfo.AreEquivalent(emptySrcTkrQtInfo));
    }

    [TestMethod]
    public void FullyPopulatedQuote_Clone_ClonedInstanceEqualsOriginal()
    {
        var cloned = ((ISourceTickerQuoteInfo)fullyPopulatedSrcTkrQtInfo).Clone();
        Assert.AreEqual(fullyPopulatedSrcTkrQtInfo, cloned);

        var cloned2 = (ISourceTickerQuoteInfo)((ICloneable)fullyPopulatedSrcTkrQtInfo).Clone();
        Assert.AreEqual(fullyPopulatedSrcTkrQtInfo, cloned2);
    }

    [TestMethod]
    public void TwoFullyPopulatedQuotes_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQSourceTickerQuoteInfo)fullyPopulatedSrcTkrQtInfo.Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, fullyPopulatedSrcTkrQtInfo, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, fullyPopulatedSrcTkrQtInfo, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedQuoteSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(fullyPopulatedSrcTkrQtInfo, fullyPopulatedSrcTkrQtInfo);
        Assert.AreEqual(fullyPopulatedSrcTkrQtInfo, ((ISourceTickerQuoteInfo)fullyPopulatedSrcTkrQtInfo).Clone());
    }

    [TestMethod]
    public void EmptyQuote_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = emptySrcTkrQtInfo.GetHashCode();
        hashCode = fullyPopulatedSrcTkrQtInfo.GetHashCode();
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType(bool exactComparison,
        PQSourceTickerQuoteInfo original, PQSourceTickerQuoteInfo changingSrcTkrQtInfo)
    {
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrQtInfo));
        Assert.IsTrue(changingSrcTkrQtInfo.AreEquivalent(original));

        Assert.IsFalse(changingSrcTkrQtInfo.AreEquivalent(null, exactComparison));
        Assert.AreEqual(!exactComparison,
            changingSrcTkrQtInfo.AreEquivalent(new SourceTickerQuoteInfo(original), exactComparison));

        changingSrcTkrQtInfo.RoundingPrecision = 1.2345678m;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrQtInfo, exactComparison));
        changingSrcTkrQtInfo.RoundingPrecision = original.RoundingPrecision;
        Assert.IsTrue(changingSrcTkrQtInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrQtInfo.MinSubmitSize = 9.8765432m;
        Assert.IsFalse(changingSrcTkrQtInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrQtInfo.MinSubmitSize = original.MinSubmitSize;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrQtInfo, exactComparison));

        changingSrcTkrQtInfo.MaxSubmitSize = 1.2345678m;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrQtInfo, exactComparison));
        changingSrcTkrQtInfo.MaxSubmitSize = original.MaxSubmitSize;
        Assert.IsTrue(changingSrcTkrQtInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrQtInfo.IncrementSize = 9.8765432m;
        Assert.IsFalse(changingSrcTkrQtInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrQtInfo.IncrementSize = original.IncrementSize;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrQtInfo, exactComparison));

        changingSrcTkrQtInfo.MinimumQuoteLife = 1000;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrQtInfo, exactComparison));
        changingSrcTkrQtInfo.MinimumQuoteLife = original.MinimumQuoteLife;
        Assert.IsTrue(changingSrcTkrQtInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrQtInfo.LayerFlags ^= LayerFlags.Volume.AllFlags();
        Assert.IsFalse(changingSrcTkrQtInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrQtInfo.LayerFlags = original.LayerFlags;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrQtInfo, exactComparison));

        changingSrcTkrQtInfo.MaximumPublishedLayers = 100;
        Assert.IsFalse(original.AreEquivalent(changingSrcTkrQtInfo, exactComparison));
        changingSrcTkrQtInfo.MaximumPublishedLayers = original.MaximumPublishedLayers;
        Assert.IsTrue(changingSrcTkrQtInfo.AreEquivalent(original, exactComparison));

        changingSrcTkrQtInfo.LastTradedFlags ^= changingSrcTkrQtInfo.LastTradedFlags.AllFlags();
        Assert.IsFalse(changingSrcTkrQtInfo.AreEquivalent(original, exactComparison));
        changingSrcTkrQtInfo.LastTradedFlags = original.LastTradedFlags;
        changingSrcTkrQtInfo.IsLastTradedFlagsUpdated = original.IsLastTradedFlagsUpdated;
        Assert.IsTrue(original.AreEquivalent(changingSrcTkrQtInfo, exactComparison));
    }

    public static void AssertSourceTickerInfoContainsAllFields(IList<PQFieldUpdate> checkFieldUpdates,
        ISourceTickerQuoteInfo srcTkrInfo)
    {
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.SourceTickerId, srcTkrInfo.Id),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.SourceTickerId));
        var decimalPlaces = BitConverter.GetBytes(decimal.GetBits(
            srcTkrInfo.RoundingPrecision)[3])[2];
        var roundingNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                       srcTkrInfo.RoundingPrecision);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.RoundingPrecision, roundingNoDecimal, decimalPlaces),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.RoundingPrecision));
        decimalPlaces = BitConverter.GetBytes(decimal.GetBits(srcTkrInfo.MinSubmitSize)[3])[2];
        var minSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                        srcTkrInfo.MinSubmitSize);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.MinSubmitSize, minSubmitNoDecimal, decimalPlaces),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.MinSubmitSize));
        decimalPlaces = BitConverter.GetBytes(decimal.GetBits(srcTkrInfo.MaxSubmitSize)[3])[2];
        var maxSubmitNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                        srcTkrInfo.MaxSubmitSize);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.MaxSubmitSize, maxSubmitNoDecimal, decimalPlaces),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.MaxSubmitSize));
        decimalPlaces = BitConverter.GetBytes(decimal.GetBits(srcTkrInfo.IncrementSize)[3])[2];
        var incrementSizeNoDecimal = (uint)((decimal)Math.Pow(10, decimalPlaces) *
                                            srcTkrInfo.IncrementSize);
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.IncrementSize, incrementSizeNoDecimal, decimalPlaces),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.IncrementSize));
        Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LayerFlags, (uint)srcTkrInfo.LayerFlags),
            PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.LayerFlags));
        if (srcTkrInfo.LastTradedFlags != LastTradedFlags.None)
            Assert.AreEqual(new PQFieldUpdate(PQFieldKeys.LastTradedFlags, (uint)srcTkrInfo.LastTradedFlags),
                PQLevel0QuoteTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQFieldKeys.LastTradedFlags));
    }


    public static PQFieldStringUpdate ExpectedSourceStringUpdate(string sourceValue) =>
        new()
        {
            Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpdate), StringUpdate
                = new PQStringUpdate
                {
                    DictionaryId = 0, Value = sourceValue, Command = CrudCommand.Update
                }
        };


    public static PQFieldStringUpdate ExpectedTickerStringUpdate(string tickerValue) =>
        new()
        {
            Field = new PQFieldUpdate(PQFieldKeys.SourceTickerNames, 0, PQFieldFlags.IsUpdate), StringUpdate
                = new PQStringUpdate
                {
                    DictionaryId = 1, Value = tickerValue, Command = CrudCommand.Update
                }
        };
}
