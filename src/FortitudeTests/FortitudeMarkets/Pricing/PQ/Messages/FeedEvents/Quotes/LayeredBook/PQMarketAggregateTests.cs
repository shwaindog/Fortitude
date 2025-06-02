using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook;

[TestClass]
public class PQMarketAggregateTests
{
    private PQMarketAggregate emptyMarketAggregate     = null!;
    private PQMarketAggregate populatedMarketAggregate = null!;

    private const decimal ExpectedVolume = 234_981m;
    private const decimal ExpectedVwap   = 23.1092m;

    private static readonly DateTime ExpectedUpdateTime = new(2025, 5, 6, 16, 11, 52);


    private static DateTime testDateTime = new(2025, 5, 26, 18, 33, 24);

    [TestInitialize]
    public void SetUp()
    {
        emptyMarketAggregate     = new PQMarketAggregate();
        testDateTime             = new DateTime(2025, 5, 29, 14, 11, 52);
        populatedMarketAggregate = new PQMarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
    }

    [TestMethod]
    public void NewMa_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newOi = new PQMarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        Assert.AreEqual(MarketDataSource.Adapter, newOi.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, newOi.UpdateTime);
        Assert.AreEqual(ExpectedVolume, newOi.Volume);
        Assert.AreEqual(ExpectedVwap, newOi.Vwap);
        Assert.IsTrue(newOi.IsDataSourceUpdated);
        Assert.IsTrue(newOi.IsUpdatedDateUpdated);
        Assert.IsTrue(newOi.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(newOi.IsVolumeUpdated);
        Assert.IsTrue(newOi.IsVwapUpdated);

        Assert.AreEqual(MarketDataSource.None, emptyMarketAggregate.DataSource);
        Assert.AreEqual(DateTime.MinValue, emptyMarketAggregate.UpdateTime);
        Assert.AreEqual(0m, emptyMarketAggregate.Volume);
        Assert.AreEqual(0m, emptyMarketAggregate.Vwap);
        Assert.IsFalse(emptyMarketAggregate.IsDataSourceUpdated);
        Assert.IsFalse(emptyMarketAggregate.IsUpdatedDateUpdated);
        Assert.IsFalse(emptyMarketAggregate.IsUpdatedSub2MinTimeUpdated);
        Assert.IsFalse(emptyMarketAggregate.IsVolumeUpdated);
        Assert.IsFalse(emptyMarketAggregate.IsVwapUpdated);
    }

    [TestMethod]
    public void NewMa_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedOi = new PQMarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        var fromPQInstance = new PQMarketAggregate(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsTrue(fromPQInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        var nonPQOi           = new MarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        var fromNonPqInstance = new PQMarketAggregate(nonPQOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromNonPqInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromNonPqInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromNonPqInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromNonPqInstance.Vwap);
        Assert.IsTrue(fromNonPqInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromNonPqInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVwapUpdated);

        var newEmptyOi = new PQMarketAggregate(emptyMarketAggregate);
        Assert.AreEqual(MarketDataSource.None, newEmptyOi.DataSource);
        Assert.AreEqual(DateTime.MinValue, newEmptyOi.UpdateTime);
        Assert.AreEqual(0m, newEmptyOi.Volume);
        Assert.AreEqual(0m, newEmptyOi.Vwap);
        Assert.IsFalse(newEmptyOi.IsDataSourceUpdated);
        Assert.IsFalse(newEmptyOi.IsUpdatedDateUpdated);
        Assert.IsFalse(newEmptyOi.IsUpdatedSub2MinTimeUpdated);
        Assert.IsFalse(newEmptyOi.IsVolumeUpdated);
        Assert.IsFalse(newEmptyOi.IsVwapUpdated);
    }

    [TestMethod]
    public void NewMa_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedOi = new PQMarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsDataSourceUpdated = false };
        var fromPQInstance = new PQMarketAggregate(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsFalse(fromPQInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        newPopulatedOi = new PQMarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsUpdatedDateUpdated = false };
        fromPQInstance = new PQMarketAggregate(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsTrue(fromPQInstance.IsDataSourceUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        newPopulatedOi = new PQMarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsUpdatedSub2MinTimeUpdated = false };
        fromPQInstance = new PQMarketAggregate(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsTrue(fromPQInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        newPopulatedOi = new PQMarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsVolumeUpdated = false };
        fromPQInstance = new PQMarketAggregate(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsTrue(fromPQInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        newPopulatedOi = new PQMarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsVwapUpdated = false };
        fromPQInstance = new PQMarketAggregate(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsTrue(fromPQInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsVwapUpdated);
    }

    [TestMethod]
    public void EmptyMa_DataSourceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyMarketAggregate.IsDataSourceUpdated);
        Assert.IsFalse(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(MarketDataSource.None, emptyMarketAggregate.DataSource);
        Assert.AreEqual(0, emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        const MarketDataSource expectedDataSource = MarketDataSource.Venue;
        emptyMarketAggregate.DataSource = expectedDataSource;
        Assert.IsTrue(emptyMarketAggregate.IsDataSourceUpdated);
        Assert.IsTrue(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(expectedDataSource, emptyMarketAggregate.DataSource);
        var sourceUpdates = emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate
            = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateSource, (uint)expectedDataSource);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyMarketAggregate.IsDataSourceUpdated = false;
        Assert.IsFalse(emptyMarketAggregate.IsDataSourceUpdated);
        Assert.IsFalse(emptyMarketAggregate.HasUpdates);
        Assert.IsTrue(emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        const MarketDataSource nextExpectedDataSource = MarketDataSource.Adapter;
        emptyMarketAggregate.DataSource = nextExpectedDataSource;
        Assert.IsTrue(emptyMarketAggregate.IsDataSourceUpdated);
        Assert.IsTrue(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(nextExpectedDataSource, emptyMarketAggregate.DataSource);
        sourceUpdates = emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate
            = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateSource, (uint)nextExpectedDataSource);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateSource
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQMarketAggregate();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedDataSource, newEmpty.DataSource);
        Assert.IsTrue(newEmpty.IsDataSourceUpdated);
    }

    [TestMethod]
    public void EmptyMa_UpdateTimeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyMarketAggregate.IsDataSourceUpdated);
        Assert.IsFalse(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(MarketDataSource.None, emptyMarketAggregate.DataSource);
        Assert.AreEqual(0, emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());


        var expectedUpdateTime = new DateTime(2025, 5, 19, 9, 30, 26);
        emptyMarketAggregate.UpdateTime = expectedUpdateTime;
        Assert.IsTrue(emptyMarketAggregate.IsUpdatedDateUpdated);
        Assert.IsTrue(emptyMarketAggregate.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(expectedUpdateTime, emptyMarketAggregate.UpdateTime);
        var layerUpdates = emptyMarketAggregate
                           .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(2, layerUpdates.Count);
        var hoursSinceUnixEpoch = expectedUpdateTime.Get2MinIntervalsFromUnixEpoch();
        var expectedDateLayerField
            = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateUpdateDate, hoursSinceUnixEpoch);
        var extended = expectedUpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBottom);
        var expectedSub2Min = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime
                                                        , subHourBottom, extended);
        Assert.AreEqual(expectedDateLayerField, layerUpdates[0]);
        Assert.AreEqual(expectedSub2Min, layerUpdates[1]);

        emptyMarketAggregate.IsUpdatedDateUpdated        = false;
        emptyMarketAggregate.IsUpdatedSub2MinTimeUpdated = false;
        Assert.IsFalse(emptyMarketAggregate.HasUpdates);
        Assert.IsTrue(emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        emptyMarketAggregate.IsUpdatedDateUpdated        = true;
        emptyMarketAggregate.IsUpdatedSub2MinTimeUpdated = true;
        var quoteUpdates =
            (from update in emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update)
                where update.PricingSubId is PQPricingSubFieldKeys.MarketAggregateUpdateDate or PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime
                select update).ToList();
        Assert.AreEqual(2, quoteUpdates.Count);
        Assert.AreEqual(expectedDateLayerField, quoteUpdates[0]);
        Assert.AreEqual(expectedSub2Min, quoteUpdates[1]);

        emptyMarketAggregate.UpdateTime = DateTime.UnixEpoch;
        emptyMarketAggregate.HasUpdates = false;

        var newEmpty = new PQMarketAggregate();
        newEmpty.UpdateField(quoteUpdates[0]);
        newEmpty.UpdateField(quoteUpdates[1]);
        Assert.AreEqual(expectedUpdateTime, newEmpty.UpdateTime);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsUpdatedDateUpdated);
        Assert.IsTrue(newEmpty.IsUpdatedSub2MinTimeUpdated);
    }

    [TestMethod]
    public void EmptyMa_VolumeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyMarketAggregate.IsVolumeUpdated);
        Assert.IsFalse(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(0m, emptyMarketAggregate.Volume);
        Assert.AreEqual(0, emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        const decimal expectedVolume = 3_123_721m;
        emptyMarketAggregate.Volume = expectedVolume;
        Assert.IsTrue(emptyMarketAggregate.IsVolumeUpdated);
        Assert.IsTrue(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(expectedVolume, emptyMarketAggregate.Volume);
        var sourceUpdates = emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateVolume, expectedVolume
                                                  , (PQFieldFlags)6);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyMarketAggregate.IsVolumeUpdated = false;
        Assert.IsFalse(emptyMarketAggregate.IsVolumeUpdated);
        Assert.IsFalse(emptyMarketAggregate.HasUpdates);
        Assert.IsTrue(emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        const decimal nextExpectedVolume = 221_124m;
        emptyMarketAggregate.Volume = nextExpectedVolume;
        Assert.IsTrue(emptyMarketAggregate.IsVolumeUpdated);
        Assert.IsTrue(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(nextExpectedVolume, emptyMarketAggregate.Volume);
        sourceUpdates = emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateVolume, nextExpectedVolume
                                              , (PQFieldFlags)6);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.PricingSubId is PQPricingSubFieldKeys.MarketAggregateVolume
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQMarketAggregate();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedVolume, newEmpty.Volume);
        Assert.IsTrue(newEmpty.IsVolumeUpdated);
    }

    [TestMethod]
    public void EmptyMa_VwapChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyMarketAggregate.IsVwapUpdated);
        Assert.IsFalse(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(0m, emptyMarketAggregate.Vwap);
        Assert.AreEqual(0, emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        const decimal expectedVwap = 3_721m;
        emptyMarketAggregate.Vwap = expectedVwap;
        Assert.IsTrue(emptyMarketAggregate.IsVwapUpdated);
        Assert.IsTrue(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(expectedVwap, emptyMarketAggregate.Vwap);
        var sourceUpdates = emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate
            = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateVwap, expectedVwap, (PQFieldFlags)2);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyMarketAggregate.IsVwapUpdated = false;
        Assert.IsFalse(emptyMarketAggregate.IsVwapUpdated);
        Assert.IsFalse(emptyMarketAggregate.HasUpdates);
        Assert.IsTrue(emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).IsNullOrEmpty());

        const decimal nextExpectedVwap = 124.0238m;
        emptyMarketAggregate.Vwap = nextExpectedVwap;
        Assert.IsTrue(emptyMarketAggregate.IsVwapUpdated);
        Assert.IsTrue(emptyMarketAggregate.HasUpdates);
        Assert.AreEqual(nextExpectedVwap, emptyMarketAggregate.Vwap);
        sourceUpdates = emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateVwap, nextExpectedVwap
                                              , (PQFieldFlags)2);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyMarketAggregate.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot)
            where update.PricingSubId is PQPricingSubFieldKeys.MarketAggregateVwap
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQMarketAggregate();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedVwap, newEmpty.Vwap);
        Assert.IsTrue(newEmpty.IsVwapUpdated);
    }

    [TestMethod]
    public void PopulatedMa_HasUpdates_ClearedAndSetAffectsAllTrackedFields()
    {
        Assert.IsTrue(populatedMarketAggregate.HasUpdates);
        Assert.IsTrue(populatedMarketAggregate.IsDataSourceUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsUpdatedDateUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsVolumeUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsVwapUpdated);
        populatedMarketAggregate.HasUpdates = false;
        Assert.IsFalse(populatedMarketAggregate.HasUpdates);
        Assert.IsFalse(populatedMarketAggregate.IsDataSourceUpdated);
        Assert.IsFalse(populatedMarketAggregate.IsUpdatedDateUpdated);
        Assert.IsFalse(populatedMarketAggregate.IsUpdatedSub2MinTimeUpdated);
        Assert.IsFalse(populatedMarketAggregate.IsVolumeUpdated);
        Assert.IsFalse(populatedMarketAggregate.IsVwapUpdated);
    }

    [TestMethod]
    public void EmptyAndPopulatedMa_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedMarketAggregate.IsEmpty);
        Assert.IsTrue(emptyMarketAggregate.IsEmpty);
    }

    [TestMethod]
    public void PopulatedMa_SetIsEmpty_ReturnsReturnsLayerToEmptyExceptUpdatedFlags()
    {
        Assert.IsTrue(populatedMarketAggregate.HasUpdates);
        Assert.IsTrue(populatedMarketAggregate.IsDataSourceUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsUpdatedDateUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsVolumeUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsVwapUpdated);
        Assert.AreEqual(MarketDataSource.Adapter, populatedMarketAggregate.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, populatedMarketAggregate.UpdateTime);
        Assert.AreEqual(ExpectedVolume, populatedMarketAggregate.Volume);
        Assert.AreEqual(ExpectedVwap, populatedMarketAggregate.Vwap);
        populatedMarketAggregate.IsEmpty = true;
        Assert.IsTrue(populatedMarketAggregate.IsEmpty);
        Assert.AreEqual(MarketDataSource.None, populatedMarketAggregate.DataSource);
        Assert.AreEqual(DateTime.MinValue, populatedMarketAggregate.UpdateTime);
        Assert.AreEqual(0m, populatedMarketAggregate.Volume);
        Assert.AreEqual(0m, populatedMarketAggregate.Vwap);
        Assert.IsTrue(populatedMarketAggregate.HasUpdates);
        Assert.IsTrue(populatedMarketAggregate.IsDataSourceUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsUpdatedDateUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsVolumeUpdated);
        Assert.IsTrue(populatedMarketAggregate.IsVwapUpdated);
    }

    [TestMethod]
    public void PopulatedMaWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllOiFields()
    {
        var pqFieldUpdates =
            populatedMarketAggregate.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllOpenInterestFields(pqFieldUpdates, PQFeedFields.ParentContextRemapped, populatedMarketAggregate);
    }

    [TestMethod]
    public void PopulatedMaWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllOiFields()
    {
        populatedMarketAggregate.HasUpdates = false;
        var pqFieldUpdates =
            populatedMarketAggregate.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllOpenInterestFields(pqFieldUpdates, PQFeedFields.ParentContextRemapped, populatedMarketAggregate);
    }

    [TestMethod]
    public void PopulatedMaWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedMarketAggregate.HasUpdates = false;
        var pqFieldUpdates =
            populatedMarketAggregate.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedMa_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewOi_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates =
            populatedMarketAggregate.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQMarketAggregate();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(populatedMarketAggregate, newEmpty);
    }

    [TestMethod]
    public void emptyMa_LayerInternalVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        emptyMarketAggregate.HasUpdates = false;

        AssertDataSourceFieldUpdatesReturnAsExpected(emptyMarketAggregate);
    }

    public static void AssertAllMarketAggregateFieldsUpdatesReturnAsExpected
    (
        IPQMarketAggregate? marketAgg,
        PQFeedFields expectedFinalField = PQFeedFields.QuoteOpenInterestTotal,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        AssertDataSourceFieldUpdatesReturnAsExpected(marketAgg, expectedFinalField, orderBookSide, orderBook, l2Quote);
        AssertUpdatedTimeFieldUpdatesReturnAsExpected(marketAgg, expectedFinalField, orderBookSide, orderBook, l2Quote);
        AssertVolumeFieldUpdatesReturnAsExpected(marketAgg, expectedFinalField, orderBookSide, orderBook, l2Quote);
        AssertVwapFieldUpdatesReturnAsExpected(marketAgg, expectedFinalField, orderBookSide, orderBook, l2Quote);
    }

    public static void AssertDataSourceFieldUpdatesReturnAsExpected
    (
        IPQMarketAggregate? marketAgg,
        PQFeedFields expectedFinalField = PQFeedFields.QuoteOpenInterestTotal,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (marketAgg == null) return;
        var bsNotNull  = orderBookSide != null;
        var bkNotNull  = orderBook != null;
        var l2QNotNull = l2Quote != null;
        var sidedDepth = orderBookSide?.BookSide != BookSide.AskBook ? PQDepthKey.None : PQDepthKey.AskSide;

        var mktAggQtField = PQFeedFields.ParentContextRemapped;
        var bkSideQtField = PQFeedFields.QuoteOpenInterestSided;
        var bkQtField     = expectedFinalField != PQFeedFields.QuoteDailyTradedAggregate ? expectedFinalField : PQFeedFields.QuoteDailyTradedAggregate;
        var qtQtField     = bkQtField;

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(marketAgg.IsDataSourceUpdated);
        Assert.IsFalse(marketAgg.HasUpdates);
        marketAgg.DataSource = MarketDataSource.Adapter;
        Assert.IsTrue(marketAgg.HasUpdates);
        marketAgg.UpdateComplete();
        marketAgg.DataSource          = MarketDataSource.None;
        marketAgg.IsDataSourceUpdated = false;
        marketAgg.HasUpdates          = false;

        Assert.AreEqual(0, marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedDataSource = MarketDataSource.Venue;
        marketAgg.DataSource = expectedDataSource;
        Assert.IsTrue(marketAgg.HasUpdates);
        Assert.AreEqual(expectedDataSource, marketAgg.DataSource);
        Assert.IsTrue(marketAgg.IsDataSourceUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        var mktAggUpdates = marketAgg
                            .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, mktAggUpdates.Count);
        var expectedMktAgg
            = new PQFieldUpdate(mktAggQtField, PQPricingSubFieldKeys.MarketAggregateSource, (uint)marketAgg.DataSource);
        var expectedBookSide  = expectedMktAgg.WithFieldId(bkSideQtField);
        var expectedOrderBook = expectedBookSide.WithFieldId(bkQtField).WithDepth(sidedDepth);
        var expectedQuote     = expectedOrderBook.WithFieldId(qtQtField);
        Assert.AreEqual(expectedMktAgg, mktAggUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedQuote, l2QUpdates[2]);


        marketAgg.IsDataSourceUpdated = false;
        Assert.IsFalse(marketAgg.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == qtQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateSource
                                                 && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedQuote, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(l2QUpdates[0]);

            var foundAgg =
                (expectedFinalField switch
                 {
                     PQFeedFields.QuoteOpenInterestTotal => newEmpty.OrderBook.OpenInterest
                   , PQFeedFields.QuoteOpenInterestSided => orderBookSide?.BookSide == BookSide.AskBook
                         ? newEmpty.OrderBook.AskSide.OpenInterestSide
                         : newEmpty.OrderBook.BidSide.OpenInterestSide
                   , PQFeedFields.QuoteDailyTradedAggregate => newEmpty.OrderBook.OpenInterest
                   , // Todo replace total daily traded when added
                     _ => throw new ArgumentException("Currently only supports OpenInterestTotal, OpenInterestSided and DailyTradedTotal")
                 })!;
            Assert.AreEqual(expectedDataSource, foundAgg.DataSource);
            Assert.IsTrue(foundAgg.IsDataSourceUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == bkQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateSource
                                                 && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bkUpdates[0]);
            var foundAgg =
                (expectedFinalField switch
                 {
                     PQFeedFields.QuoteOpenInterestTotal => newEmpty.OpenInterest
                   , PQFeedFields.QuoteOpenInterestSided => orderBookSide?.BookSide == BookSide.AskBook
                         ? newEmpty.AskSide.OpenInterestSide
                         : newEmpty.BidSide.OpenInterestSide
                   , PQFeedFields.QuoteDailyTradedAggregate => newEmpty.OpenInterest
                   , // Todo replace total daily traded when added
                     _ => throw new ArgumentException("Currently only supports OpenInterestTotal, OpenInterestSided and DailyTradedTotal")
                 })!;
            Assert.AreEqual(expectedDataSource, foundAgg.DataSource);
            Assert.IsTrue(foundAgg.IsDataSourceUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == bkSideQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateSource
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            newEmpty.UpdateField(bsUpdates[0]);
            var foundAgg = newEmpty.OpenInterestSide;
            Assert.AreEqual(expectedDataSource, foundAgg!.DataSource);
            Assert.IsTrue(foundAgg.IsDataSourceUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        mktAggUpdates =
            (from update in marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == mktAggQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateSource
                select update).ToList();
        Assert.AreEqual(1, mktAggUpdates.Count);
        Assert.AreEqual(expectedMktAgg, mktAggUpdates[0]);

        var newLayer = new PQMarketAggregate();
        newLayer.UpdateField(mktAggUpdates[0]);
        Assert.AreEqual(expectedDataSource, newLayer.DataSource);
        Assert.IsTrue(newLayer.IsDataSourceUpdated);
        Assert.IsTrue(newLayer.HasUpdates);

        marketAgg.DataSource = MarketDataSource.None;
        marketAgg.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void EmptyMa_UpdateTimeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        emptyMarketAggregate.HasUpdates = false;

        AssertUpdatedTimeFieldUpdatesReturnAsExpected(emptyMarketAggregate);
    }

    public static void AssertUpdatedTimeFieldUpdatesReturnAsExpected
    (
        IPQMarketAggregate? marketAgg,
        PQFeedFields expectedFinalField = PQFeedFields.QuoteOpenInterestTotal,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (marketAgg == null) return;
        var bsNotNull  = orderBookSide != null;
        var bkNotNull  = orderBook != null;
        var l2QNotNull = l2Quote != null;
        var sidedDepth = orderBookSide?.BookSide != BookSide.AskBook ? PQDepthKey.None : PQDepthKey.AskSide;

        var bkSideQtField = PQFeedFields.QuoteOpenInterestSided;
        var bkQtField     = expectedFinalField != PQFeedFields.QuoteDailyTradedAggregate ? expectedFinalField : PQFeedFields.QuoteDailyTradedAggregate;
        
        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(marketAgg.IsUpdatedDateUpdated);
        Assert.IsFalse(marketAgg.IsUpdatedSub2MinTimeUpdated);
        Assert.IsFalse(marketAgg.HasUpdates);
        marketAgg.UpdateTime = DateTime.Now;
        Assert.IsTrue(marketAgg.HasUpdates);
        marketAgg.UpdateComplete();
        marketAgg.UpdateTime                  = DateTime.MinValue;
        marketAgg.IsUpdatedDateUpdated        = false;
        marketAgg.IsUpdatedSub2MinTimeUpdated = false;
        marketAgg.HasUpdates                  = false;

        Assert.AreEqual(0, marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedUpdateTime = new DateTime(2025, 7, 8, 19, 27, 53);
        marketAgg.UpdateTime = expectedUpdateTime;
        Assert.IsTrue(marketAgg.HasUpdates);
        Assert.AreEqual(expectedUpdateTime, marketAgg.UpdateTime);
        Assert.IsTrue(marketAgg.IsUpdatedDateUpdated);
        Assert.IsTrue(marketAgg.IsUpdatedSub2MinTimeUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(4, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(2, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(2, bsUpdates.Count);
        var orderInfoUpdates = marketAgg
                               .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(2, orderInfoUpdates.Count);
        var hoursSinceUnixEpoch = expectedUpdateTime.Get2MinIntervalsFromUnixEpoch();
        var expectedMktAggDate
            = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateUpdateDate, hoursSinceUnixEpoch);
        var extended = expectedUpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBottom);
        var expectedMktAggTime = new PQFieldUpdate(PQFeedFields.ParentContextRemapped, PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime, subHourBottom
                                                 , extended);
        var expectedBookSideDate  = expectedMktAggDate.WithFieldId(bkSideQtField);
        var expectedBookSideTime  = expectedMktAggTime.WithFieldId(bkSideQtField);
        var expectedOrderBookDate = expectedBookSideDate.WithFieldId(bkQtField).WithDepth(sidedDepth);
        var expectedOrderBookTime = expectedBookSideTime.WithFieldId(bkQtField).WithDepth(sidedDepth);
        Assert.AreEqual(expectedMktAggDate, orderInfoUpdates[0]);
        Assert.AreEqual(expectedMktAggTime, orderInfoUpdates[1]);
        if (bsNotNull)
        {
            Assert.AreEqual(expectedBookSideDate, bsUpdates[0]);
            Assert.AreEqual(expectedBookSideTime, bsUpdates[1]);
        }
        if (bkNotNull)
        {
            Assert.AreEqual(expectedOrderBookDate, bkUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, bkUpdates[1]);
        }
        if (l2QNotNull)
        {
            Assert.AreEqual(expectedOrderBookDate, l2QUpdates[2]);
            Assert.AreEqual(expectedOrderBookTime, l2QUpdates[3]);
        }

        marketAgg.IsUpdatedDateUpdated        = false;
        marketAgg.IsUpdatedSub2MinTimeUpdated = false;
        Assert.IsFalse(marketAgg.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == bkQtField
                       && update is { PricingSubId: PQPricingSubFieldKeys.MarketAggregateUpdateDate or PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime }
                       && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(2, l2QUpdates.Count);
            Assert.AreEqual(expectedOrderBookDate, l2QUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, l2QUpdates[1]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);

            var foundAgg =
                (expectedFinalField switch
                 {
                     PQFeedFields.QuoteOpenInterestTotal => newEmpty.OrderBook.OpenInterest
                   , PQFeedFields.QuoteOpenInterestSided => orderBookSide?.BookSide == BookSide.AskBook
                         ? newEmpty.OrderBook.AskSide.OpenInterestSide
                         : newEmpty.OrderBook.BidSide.OpenInterestSide
                   , PQFeedFields.QuoteDailyTradedAggregate => newEmpty.OrderBook.OpenInterest
                   , // Todo replace total daily traded when added
                     _ => throw new ArgumentException("Currently only supports OpenInterestTotal, OpenInterestSided and DailyTradedTotal")
                 })!;
            foundAgg.DataSource = MarketDataSource.Venue; // required for book to stop generating from published layers
            foundAgg.HasUpdates = false;
            newEmpty.UpdateField(l2QUpdates[0]);
            newEmpty.UpdateField(l2QUpdates[1]);
            Assert.AreEqual(expectedUpdateTime, foundAgg.UpdateTime);
            Assert.IsTrue(foundAgg.IsUpdatedDateUpdated);
            Assert.IsTrue(foundAgg.IsUpdatedSub2MinTimeUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == bkQtField
                       && update is { PricingSubId: PQPricingSubFieldKeys.MarketAggregateUpdateDate or PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime }
                       && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(2, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBookDate, bkUpdates[0]);
            Assert.AreEqual(expectedOrderBookTime, bkUpdates[1]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            var foundAgg =
                (expectedFinalField switch
                 {
                     PQFeedFields.QuoteOpenInterestTotal => newEmpty.OpenInterest
                   , PQFeedFields.QuoteOpenInterestSided => orderBookSide?.BookSide == BookSide.AskBook
                         ? newEmpty.AskSide.OpenInterestSide
                         : newEmpty.BidSide.OpenInterestSide
                   , PQFeedFields.QuoteDailyTradedAggregate => newEmpty.OpenInterest
                   , // Todo replace total daily traded when added
                     _ => throw new ArgumentException("Currently only supports OpenInterestTotal, OpenInterestSided and DailyTradedTotal")
                 })!;
            foundAgg.DataSource = MarketDataSource.Venue; // required for book to stop generating from published layers
            foundAgg.HasUpdates = false;
            newEmpty.UpdateField(bkUpdates[0]);
            newEmpty.UpdateField(bkUpdates[1]);
            Assert.AreEqual(expectedUpdateTime, foundAgg.UpdateTime);
            Assert.IsTrue(foundAgg.IsUpdatedDateUpdated);
            Assert.IsTrue(foundAgg.IsUpdatedSub2MinTimeUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == bkSideQtField
                       && update is { PricingSubId: PQPricingSubFieldKeys.MarketAggregateUpdateDate or PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime }
                    select update).ToList();
            Assert.AreEqual(2, bsUpdates.Count);
            Assert.AreEqual(expectedBookSideDate, bsUpdates[0]);
            Assert.AreEqual(expectedBookSideTime, bsUpdates[1]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            var foundAgg = newEmpty.OpenInterestSide!;
            foundAgg.DataSource = MarketDataSource.Venue; // required for book to stop generating from published layers
            foundAgg.HasUpdates = false;
            newEmpty.UpdateField(bsUpdates[0]);
            newEmpty.UpdateField(bsUpdates[1]);
            Assert.AreEqual(expectedUpdateTime, foundAgg.UpdateTime);
            Assert.IsTrue(foundAgg.IsUpdatedDateUpdated);
            Assert.IsTrue(foundAgg.IsUpdatedSub2MinTimeUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        orderInfoUpdates =
            (from update in marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update is { PricingSubId: PQPricingSubFieldKeys.MarketAggregateUpdateDate or PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime }
                select update).ToList();
        Assert.AreEqual(2, orderInfoUpdates.Count);
        Assert.AreEqual(expectedMktAggDate, orderInfoUpdates[0]);
        Assert.AreEqual(expectedMktAggTime, orderInfoUpdates[1]);

        var newMarketAgg = new PQMarketAggregate();
        newMarketAgg.UpdateField(orderInfoUpdates[0]);
        newMarketAgg.UpdateField(orderInfoUpdates[1]);
        Assert.AreEqual(expectedUpdateTime, newMarketAgg.UpdateTime);
        Assert.IsTrue(newMarketAgg.IsUpdatedDateUpdated);
        Assert.IsTrue(newMarketAgg.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(newMarketAgg.HasUpdates);

        marketAgg.UpdateTime = DateTime.MinValue;
        marketAgg.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }


    [TestMethod]
    public void emptyMa_MarketAggregateVolumeChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        emptyMarketAggregate.HasUpdates = false;

        AssertVolumeFieldUpdatesReturnAsExpected(emptyMarketAggregate);
    }

    public static void AssertVolumeFieldUpdatesReturnAsExpected
    (
        IPQMarketAggregate? marketAgg,
        PQFeedFields expectedFinalField = PQFeedFields.QuoteOpenInterestTotal,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (marketAgg == null) return;
        var bsNotNull  = orderBookSide != null;
        var bkNotNull  = orderBook != null;
        var l2QNotNull = l2Quote != null;
        var sidedDepth = orderBookSide?.BookSide != BookSide.AskBook ? PQDepthKey.None : PQDepthKey.AskSide;

        var mktAggQtField = PQFeedFields.ParentContextRemapped;
        var bkSideQtField = PQFeedFields.QuoteOpenInterestSided;
        var bkQtField     = expectedFinalField != PQFeedFields.QuoteDailyTradedAggregate ? expectedFinalField : PQFeedFields.QuoteDailyTradedAggregate;
        var qtQtField     = bkQtField;

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(marketAgg.IsVolumeUpdated);
        Assert.IsFalse(marketAgg.HasUpdates);
        marketAgg.Volume = 987_123;
        Assert.IsTrue(marketAgg.HasUpdates);
        marketAgg.UpdateComplete();
        marketAgg.Volume          = 0m;
        marketAgg.IsVolumeUpdated = false;
        marketAgg.HasUpdates      = false;

        Assert.AreEqual(0, marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedVolume = 987_567m;
        marketAgg.Volume = expectedVolume;
        Assert.IsTrue(marketAgg.HasUpdates);
        Assert.AreEqual(expectedVolume, marketAgg.Volume);
        Assert.IsTrue(marketAgg.IsVolumeUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        var mktAggUpdates = marketAgg
                            .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, mktAggUpdates.Count);
        var expectedMktAgg
            = new PQFieldUpdate(mktAggQtField, PQPricingSubFieldKeys.MarketAggregateVolume, expectedVolume, precisionSettings.VolumeScalingPrecision);
        var expectedBookSide  = expectedMktAgg.WithFieldId(bkSideQtField);
        var expectedOrderBook = expectedBookSide.WithFieldId(bkQtField).WithDepth(sidedDepth);
        var expectedQuote     = expectedOrderBook.WithFieldId(qtQtField);
        Assert.AreEqual(expectedMktAgg, mktAggUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedQuote, l2QUpdates[2]);


        marketAgg.IsVolumeUpdated = false;
        Assert.IsFalse(marketAgg.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == qtQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateVolume
                                                 && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedQuote, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);

            var foundAgg =
                (expectedFinalField switch
                 {
                     PQFeedFields.QuoteOpenInterestTotal => newEmpty.OrderBook.OpenInterest
                   , PQFeedFields.QuoteOpenInterestSided => orderBookSide?.BookSide == BookSide.AskBook
                         ? newEmpty.OrderBook.AskSide.OpenInterestSide
                         : newEmpty.OrderBook.BidSide.OpenInterestSide
                   , PQFeedFields.QuoteDailyTradedAggregate => newEmpty.OrderBook.OpenInterest
                   , // Todo replace total daily traded when added
                     _ => throw new ArgumentException("Currently only supports OpenInterestTotal, OpenInterestSided and DailyTradedTotal")
                 })!;
            foundAgg.DataSource = MarketDataSource.Venue; // required for book to stop generating from published layers
            foundAgg.HasUpdates = false;
            newEmpty.UpdateField(l2QUpdates[0]);
            Assert.AreEqual(expectedVolume, foundAgg.Volume);
            Assert.IsTrue(foundAgg.IsVolumeUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == bkQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateVolume
                                                 && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            var foundAgg =
                (expectedFinalField switch
                 {
                     PQFeedFields.QuoteOpenInterestTotal => newEmpty.OpenInterest
                   , PQFeedFields.QuoteOpenInterestSided => orderBookSide?.BookSide == BookSide.AskBook
                         ? newEmpty.AskSide.OpenInterestSide
                         : newEmpty.BidSide.OpenInterestSide
                   , PQFeedFields.QuoteDailyTradedAggregate => newEmpty.OpenInterest
                   , // Todo replace total daily traded when added
                     _ => throw new ArgumentException("Currently only supports OpenInterestTotal, OpenInterestSided and DailyTradedTotal")
                 })!;
            foundAgg.DataSource = MarketDataSource.Venue; // required for book to stop generating from published layers
            foundAgg.HasUpdates = false;
            newEmpty.UpdateField(bkUpdates[0]);
            Assert.AreEqual(expectedVolume, foundAgg.Volume);
            Assert.IsTrue(foundAgg.IsVolumeUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == bkSideQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateVolume
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            var foundAgg = newEmpty.OpenInterestSide!;
            foundAgg.DataSource = MarketDataSource.Venue; // required for book to stop generating from published layers
            foundAgg.HasUpdates = false;
            newEmpty.UpdateField(bsUpdates[0]);
            Assert.AreEqual(expectedVolume, foundAgg.Volume);
            Assert.IsTrue(foundAgg.IsVolumeUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        mktAggUpdates =
            (from update in marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == mktAggQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateVolume
                select update).ToList();
        Assert.AreEqual(1, mktAggUpdates.Count);
        Assert.AreEqual(expectedMktAgg, mktAggUpdates[0]);

        var newLayer = new PQMarketAggregate();
        newLayer.UpdateField(mktAggUpdates[0]);
        Assert.AreEqual(expectedVolume, newLayer.Volume);
        Assert.IsTrue(newLayer.IsVolumeUpdated);
        Assert.IsTrue(newLayer.HasUpdates);

        marketAgg.Volume     = 0;
        marketAgg.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }


    [TestMethod]
    public void emptyMa_MarketAggregateVwapChanged_ExpectedPropertiesUpdatedDeltaUpdatesAffected()
    {
        emptyMarketAggregate.HasUpdates = false;

        AssertVwapFieldUpdatesReturnAsExpected(emptyMarketAggregate);
    }

    public static void AssertVwapFieldUpdatesReturnAsExpected
    (
        IPQMarketAggregate? marketAgg,
        PQFeedFields expectedFinalField = PQFeedFields.QuoteOpenInterestTotal,
        IPQOrderBookSide? orderBookSide = null,
        IPQOrderBook? orderBook = null,
        IPQPublishableLevel2Quote? l2Quote = null
    )
    {
        if (marketAgg == null) return;
        var bsNotNull  = orderBookSide != null;
        var bkNotNull  = orderBook != null;
        var l2QNotNull = l2Quote != null;
        var sidedDepth = orderBookSide?.BookSide != BookSide.AskBook ? PQDepthKey.None : PQDepthKey.AskSide;

        var mktAggQtField = PQFeedFields.ParentContextRemapped;
        var bkSideQtField = PQFeedFields.QuoteOpenInterestSided;
        var bkQtField     = expectedFinalField != PQFeedFields.QuoteDailyTradedAggregate ? expectedFinalField : PQFeedFields.QuoteDailyTradedAggregate;
        var qtQtField     = bkQtField;

        testDateTime = testDateTime.AddHours(1).AddMinutes(1);

        Assert.IsFalse(marketAgg.IsVwapUpdated);
        Assert.IsFalse(marketAgg.HasUpdates);
        marketAgg.Vwap = 987_123;
        Assert.IsTrue(marketAgg.HasUpdates);
        marketAgg.UpdateComplete();
        marketAgg.Vwap          = 0m;
        marketAgg.IsVwapUpdated = false;
        marketAgg.HasUpdates    = false;

        Assert.AreEqual(0, marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bsNotNull) Assert.AreEqual(0, orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (bkNotNull) Assert.AreEqual(0, orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());
        if (l2QNotNull) Assert.AreEqual(2, l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update).Count());

        var expectedVwap = 567m;
        marketAgg.Vwap = expectedVwap;
        Assert.IsTrue(marketAgg.HasUpdates);
        Assert.AreEqual(expectedVwap, marketAgg.Vwap);
        Assert.IsTrue(marketAgg.IsVwapUpdated);
        var precisionSettings = l2Quote?.SourceTickerInfo ?? PQSourceTickerInfoTests.OrdersCountL3TraderNamePaidOrGivenSti;
        var l2QUpdates = l2QNotNull
            ? l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bkUpdates = bkNotNull
            ? orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        var bsUpdates = bsNotNull
            ? orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList()
            : [];
        if (l2QNotNull) Assert.AreEqual(3, l2QUpdates.Count);
        if (bkNotNull) Assert.AreEqual(1, bkUpdates.Count);
        if (bsNotNull) Assert.AreEqual(1, bsUpdates.Count);
        var mktAggUpdates = marketAgg
                            .GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).ToList();
        Assert.AreEqual(1, mktAggUpdates.Count);
        var expectedMktAgg
            = new PQFieldUpdate(mktAggQtField, PQPricingSubFieldKeys.MarketAggregateVwap, expectedVwap, precisionSettings.PriceScalingPrecision);
        var expectedBookSide  = expectedMktAgg.WithFieldId(bkSideQtField);
        var expectedOrderBook = expectedBookSide.WithFieldId(bkQtField).WithDepth(sidedDepth);
        var expectedQuote     = expectedOrderBook.WithFieldId(qtQtField);
        Assert.AreEqual(expectedMktAgg, mktAggUpdates[0]);
        if (bsNotNull) Assert.AreEqual(expectedBookSide, bsUpdates[0]);
        if (bkNotNull) Assert.AreEqual(expectedOrderBook, bkUpdates[0]);
        if (l2QNotNull) Assert.AreEqual(expectedQuote, l2QUpdates[2]);


        marketAgg.IsVwapUpdated = false;
        Assert.IsFalse(marketAgg.HasUpdates);
        if (bsNotNull) Assert.IsFalse(orderBookSide!.HasUpdates);
        if (bkNotNull) Assert.IsFalse(orderBook!.HasUpdates);
        if (l2QNotNull)
        {
            Assert.IsTrue(l2Quote!.HasUpdates);
            l2Quote.IsAdapterSentTimeDateUpdated    = false;
            l2Quote.IsAdapterSentTimeSub2MinUpdated = false;
            Assert.IsFalse(l2Quote.HasUpdates);
            Assert.AreEqual(2, l2Quote.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).Count());
        }
        Assert.IsTrue(marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Update, precisionSettings).IsNullOrEmpty());

        if (l2QNotNull)
        {
            l2QUpdates =
                (from update in l2Quote!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == qtQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateVwap
                                                 && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(1, l2QUpdates.Count);
            Assert.AreEqual(expectedQuote, l2QUpdates[0]);

            var newEmpty = new PQPublishableLevel2Quote(l2Quote.SourceTickerInfo ?? precisionSettings);
            var foundAgg =
                (expectedFinalField switch
                 {
                     PQFeedFields.QuoteOpenInterestTotal => newEmpty.OrderBook.OpenInterest
                   , PQFeedFields.QuoteOpenInterestSided => orderBookSide?.BookSide == BookSide.AskBook
                         ? newEmpty.OrderBook.AskSide.OpenInterestSide
                         : newEmpty.OrderBook.BidSide.OpenInterestSide
                   , PQFeedFields.QuoteDailyTradedAggregate => newEmpty.OrderBook.OpenInterest
                   , // Todo replace total daily traded when added
                     _ => throw new ArgumentException("Currently only supports OpenInterestTotal, OpenInterestSided and DailyTradedTotal")
                 })!;
            foundAgg.DataSource = MarketDataSource.Venue; // required for book to stop generating from published layers
            foundAgg.HasUpdates = false;
            newEmpty.UpdateField(l2QUpdates[0]);

            Assert.AreEqual(expectedVwap, foundAgg.Vwap);
            Assert.IsTrue(foundAgg.IsVwapUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bkNotNull)
        {
            bkUpdates =
                (from update in orderBook!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == bkQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateVwap
                                                 && update.DepthId == sidedDepth
                    select update).ToList();
            Assert.AreEqual(1, bkUpdates.Count);
            Assert.AreEqual(expectedOrderBook, bkUpdates[0]);

            var newEmpty = new PQOrderBook(l2Quote?.SourceTickerInfo ?? precisionSettings);
            var foundAgg =
                (expectedFinalField switch
                 {
                     PQFeedFields.QuoteOpenInterestTotal => newEmpty.OpenInterest
                   , PQFeedFields.QuoteOpenInterestSided => orderBookSide?.BookSide == BookSide.AskBook
                         ? newEmpty.AskSide.OpenInterestSide
                         : newEmpty.BidSide.OpenInterestSide
                   , PQFeedFields.QuoteDailyTradedAggregate => newEmpty.OpenInterest
                   , // Todo replace total daily traded when added
                     _ => throw new ArgumentException("Currently only supports OpenInterestTotal, OpenInterestSided and DailyTradedTotal")
                 })!;
            foundAgg.DataSource = MarketDataSource.Venue; // required for book to stop generating from published layers
            foundAgg.HasUpdates = false;
            newEmpty.UpdateField(bkUpdates[0]);
            Assert.AreEqual(expectedVwap, foundAgg.Vwap);
            Assert.IsTrue(foundAgg.IsVwapUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        if (bsNotNull)
        {
            bsUpdates =
                (from update in orderBookSide!.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                    where update.Id == bkSideQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateVwap
                    select update).ToList();
            Assert.AreEqual(1, bsUpdates.Count);
            Assert.AreEqual(expectedBookSide, bsUpdates[0]);

            var newEmpty = new PQOrderBookSide(orderBookSide.BookSide, l2Quote?.SourceTickerInfo ?? precisionSettings);
            var foundAgg = newEmpty.OpenInterestSide!;
            foundAgg.DataSource = MarketDataSource.Venue;  // required for book to stop generating from published layers
            foundAgg.HasUpdates = false;
            newEmpty.UpdateField(bsUpdates[0]);
            Assert.AreEqual(expectedVwap, foundAgg.Vwap);
            Assert.IsTrue(foundAgg.IsVwapUpdated);
            Assert.IsTrue(foundAgg.HasUpdates);
            Assert.IsTrue(newEmpty.HasUpdates);
        }
        mktAggUpdates =
            (from update in marketAgg.GetDeltaUpdateFields(testDateTime, PQMessageFlags.Snapshot, precisionSettings)
                where update.Id == mktAggQtField && update.PricingSubId == PQPricingSubFieldKeys.MarketAggregateVwap
                select update).ToList();
        Assert.AreEqual(1, mktAggUpdates.Count);
        Assert.AreEqual(expectedMktAgg, mktAggUpdates[0]);

        var newLayer = new PQMarketAggregate();
        newLayer.UpdateField(mktAggUpdates[0]);
        Assert.AreEqual(expectedVwap, newLayer.Vwap);
        Assert.IsTrue(newLayer.IsVwapUpdated);
        Assert.IsTrue(newLayer.HasUpdates);

        marketAgg.Vwap       = 0;
        marketAgg.HasUpdates = false;
        if (l2QNotNull) l2Quote!.HasUpdates = false;
    }

    [TestMethod]
    public void FullyPopulatedMa_CopyFromToEmptyPvl_PvlsEqualEachOther()
    {
        var nonPQOpenInterest = new MarketAggregate(populatedMarketAggregate);
        emptyMarketAggregate.CopyFrom(nonPQOpenInterest);
        Assert.AreEqual(populatedMarketAggregate, emptyMarketAggregate);
    }

    [TestMethod]
    public void FullyPopulatedMa_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQMarketAggregate();
        populatedMarketAggregate.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedMarketAggregate);
        Assert.IsTrue(emptyPriceVolumeLayer.IsEmpty);
        Assert.AreEqual(MarketDataSource.None, emptyPriceVolumeLayer.DataSource);
        Assert.AreEqual(DateTime.MinValue, emptyPriceVolumeLayer.UpdateTime);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Volume);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.Vwap);
        Assert.IsFalse(emptyPriceVolumeLayer.HasUpdates);
        Assert.IsFalse(emptyPriceVolumeLayer.IsDataSourceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsUpdatedDateUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsUpdatedSub2MinTimeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsVwapUpdated);
    }

    [TestMethod]
    public void NonPQPopulatedMa_CopyFromToEmptyPvl_LayersEquivalentToEachOther()
    {
        var nonPQOi  = new MarketAggregate(populatedMarketAggregate);
        var newEmpty = new PQMarketAggregate();
        newEmpty.CopyFrom(nonPQOi);
        Assert.IsTrue(populatedMarketAggregate.AreEquivalent(newEmpty));
    }

    [TestMethod]
    public void FullyPopulatedMa_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedPvl = ((ICloneable<IPQMarketAggregate>)populatedMarketAggregate).Clone();

        Assert.AreNotSame(clonedPvl, populatedMarketAggregate);
        Assert.AreEqual(populatedMarketAggregate, clonedPvl);

        var cloned2 = (PQMarketAggregate)((ICloneable)populatedMarketAggregate).Clone();
        Assert.AreNotSame(cloned2, populatedMarketAggregate);
        Assert.AreEqual(populatedMarketAggregate, cloned2);
    }

    [TestMethod]
    public void FullyPopulatedMaCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQMarketAggregate)((ICloneable)populatedMarketAggregate).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedMarketAggregate, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedMarketAggregate, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedMaSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedMarketAggregate, populatedMarketAggregate);
        Assert.AreEqual(populatedMarketAggregate, ((ICloneable)populatedMarketAggregate).Clone());
        Assert.AreEqual(populatedMarketAggregate, ((ICloneable<IMarketAggregate>)populatedMarketAggregate).Clone());
    }

    [TestMethod]
    public void FullyPopulatedMa_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedMarketAggregate.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedMa_ToString_ReturnsNameAndValues()
    {
        var toString = populatedMarketAggregate.ToString();

        Assert.IsTrue(toString.Contains(populatedMarketAggregate.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedMarketAggregate.DataSource)}: {populatedMarketAggregate.DataSource}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedMarketAggregate.UpdateTime)}: {populatedMarketAggregate.UpdateTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedMarketAggregate.Volume)}: {populatedMarketAggregate.Volume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedMarketAggregate.Vwap)}: {populatedMarketAggregate.Vwap:N5}"));
    }

    public static void AssertContainsAllOpenInterestFields
    (IList<PQFieldUpdate> checkFieldUpdates, PQFeedFields targetField, IPQMarketAggregate oi,
        PQFieldFlags priceScale = (PQFieldFlags)2, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        Assert.AreEqual(new PQFieldUpdate(targetField, PQPricingSubFieldKeys.MarketAggregateSource, (uint)oi.DataSource),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, targetField, PQPricingSubFieldKeys.MarketAggregateSource),
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");

        var orderUpdatedDate = oi.UpdateTime.Get2MinIntervalsFromUnixEpoch();
        var orderUpdatedDateFu
            = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, targetField, PQPricingSubFieldKeys.MarketAggregateUpdateDate);
        Assert.AreEqual(new PQFieldUpdate(targetField, PQPricingSubFieldKeys.MarketAggregateUpdateDate, orderUpdatedDate)
                      , orderUpdatedDateFu,
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");

        var orderUpdatedSub2MinExtended
            = oi.UpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var orderUpdatedSub2Min);
        var orderUpdatedSub2MinFu
            = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, targetField, PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime);
        Assert.AreEqual(new PQFieldUpdate(targetField, PQPricingSubFieldKeys.MarketAggregateUpdateSub2MinTime, orderUpdatedSub2Min, orderUpdatedSub2MinExtended)
                      , orderUpdatedSub2MinFu,
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");


        Assert.AreEqual(new PQFieldUpdate(targetField, PQPricingSubFieldKeys.MarketAggregateVolume, oi.Volume, volumeScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, targetField, PQPricingSubFieldKeys.MarketAggregateVolume, volumeScale)
                       ,
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");

        Assert.AreEqual(new PQFieldUpdate(targetField, PQPricingSubFieldKeys.MarketAggregateVwap, oi.Vwap, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, targetField, PQPricingSubFieldKeys.MarketAggregateVwap, priceScale),
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQMarketAggregate? original, IPQMarketAggregate? changingOpenInterest,
        IOrderBookSide? originalOrderBookSide = null,
        IOrderBookSide? changingOrderBookSide = null,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        IPublishableLevel2Quote? originalQuote = null,
        IPublishableLevel2Quote? changingQuote = null)
    {
        if (original == null && changingOpenInterest == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingOpenInterest);

        if (original.GetType() == changingOpenInterest.GetType())
            Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        else
            Assert.AreEqual(!exactComparison, original.AreEquivalent(
                                                                     changingOpenInterest, exactComparison));

        if (original.GetType() == typeof(PQMarketAggregate))
            Assert.AreEqual(!exactComparison,
                            changingOpenInterest.AreEquivalent(new MarketAggregate(original), exactComparison));

        // PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
        //                                                                           original, changingOpenInterest, originalOrderBook
        //                                                                         , changingOrderBook, originalQuote, changingQuote);

        changingOpenInterest.IsDataSourceUpdated = !changingOpenInterest.IsDataSourceUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsDataSourceUpdated = original.IsDataSourceUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingOpenInterest.IsUpdatedDateUpdated = !changingOpenInterest.IsUpdatedDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsUpdatedDateUpdated = original.IsUpdatedDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingOpenInterest.IsUpdatedSub2MinTimeUpdated = !changingOpenInterest.IsUpdatedSub2MinTimeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsUpdatedSub2MinTimeUpdated = original.IsUpdatedSub2MinTimeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingOpenInterest.IsVolumeUpdated = !changingOpenInterest.IsVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsVolumeUpdated = original.IsVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingOpenInterest.IsVwapUpdated = !changingOpenInterest.IsVwapUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsVwapUpdated = original.IsVwapUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue(
                          originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
