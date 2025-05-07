using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQOpenInterestTests
{
    private PQOpenInterest emptyOpenInterest     = null!;
    private PQOpenInterest populatedOpenInterest = null!;

    private const decimal ExpectedVolume = 234_981m;
    private const decimal ExpectedVwap   = 23.1092m;

    private static readonly DateTime ExpectedUpdateTime = new(2025, 5, 6, 16, 11, 52);

    private DateTime testDateTime;

    [TestInitialize]
    public void SetUp()
    {
        emptyOpenInterest     = new PQOpenInterest();
        testDateTime          = new DateTime(2017, 12, 17, 16, 11, 52);
        populatedOpenInterest = new PQOpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
    }

    [TestMethod]
    public void NewOi_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newOi = new PQOpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        Assert.AreEqual(MarketDataSource.Adapter, newOi.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, newOi.UpdateTime);
        Assert.AreEqual(ExpectedVolume, newOi.Volume);
        Assert.AreEqual(ExpectedVwap, newOi.Vwap);
        Assert.IsTrue(newOi.IsDataSourceUpdated);
        Assert.IsTrue(newOi.IsUpdatedDateUpdated);
        Assert.IsTrue(newOi.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(newOi.IsVolumeUpdated);
        Assert.IsTrue(newOi.IsVwapUpdated);

        Assert.AreEqual(MarketDataSource.None, emptyOpenInterest.DataSource);
        Assert.AreEqual(DateTime.MinValue, emptyOpenInterest.UpdateTime);
        Assert.AreEqual(0m, emptyOpenInterest.Volume);
        Assert.AreEqual(0m, emptyOpenInterest.Vwap);
        Assert.IsFalse(emptyOpenInterest.IsDataSourceUpdated);
        Assert.IsFalse(emptyOpenInterest.IsUpdatedDateUpdated);
        Assert.IsFalse(emptyOpenInterest.IsUpdatedSub2MinTimeUpdated);
        Assert.IsFalse(emptyOpenInterest.IsVolumeUpdated);
        Assert.IsFalse(emptyOpenInterest.IsVwapUpdated);
    }

    [TestMethod]
    public void NewOi_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedOi = new PQOpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        var fromPQInstance = new PQOpenInterest(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsTrue(fromPQInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        var nonPQOi           = new OpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        var fromNonPqInstance = new PQOpenInterest(nonPQOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromNonPqInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromNonPqInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromNonPqInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromNonPqInstance.Vwap);
        Assert.IsTrue(fromNonPqInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromNonPqInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsVwapUpdated);

        var newEmptyOi = new PQOpenInterest(emptyOpenInterest);
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
    public void NewOi_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedOi = new PQOpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsDataSourceUpdated = false };
        var fromPQInstance = new PQOpenInterest(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsFalse(fromPQInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        newPopulatedOi = new PQOpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsUpdatedDateUpdated = false };
        fromPQInstance = new PQOpenInterest(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsTrue(fromPQInstance.IsDataSourceUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        newPopulatedOi = new PQOpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsUpdatedSub2MinTimeUpdated = false };
        fromPQInstance = new PQOpenInterest(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsTrue(fromPQInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        newPopulatedOi = new PQOpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsVolumeUpdated = false };
        fromPQInstance = new PQOpenInterest(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);
        Assert.IsTrue(fromPQInstance.IsDataSourceUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedDateUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedSub2MinTimeUpdated);
        Assert.IsFalse(fromPQInstance.IsVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsVwapUpdated);

        newPopulatedOi = new PQOpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime)
            { IsVwapUpdated = false };
        fromPQInstance = new PQOpenInterest(newPopulatedOi);
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
    public void EmptyOi_DataSourceChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyOpenInterest.IsDataSourceUpdated);
        Assert.IsFalse(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(MarketDataSource.None, emptyOpenInterest.DataSource);
        Assert.AreEqual(0, emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        const MarketDataSource expectedDataSource = MarketDataSource.Venue;
        emptyOpenInterest.DataSource = expectedDataSource;
        Assert.IsTrue(emptyOpenInterest.IsDataSourceUpdated);
        Assert.IsTrue(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(expectedDataSource, emptyOpenInterest.DataSource);
        var sourceUpdates = emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestSource, (uint)expectedDataSource);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyOpenInterest.IsDataSourceUpdated = false;
        Assert.IsFalse(emptyOpenInterest.IsDataSourceUpdated);
        Assert.IsFalse(emptyOpenInterest.HasUpdates);
        Assert.IsTrue(emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        const MarketDataSource nextExpectedDataSource = MarketDataSource.Adapter;
        emptyOpenInterest.DataSource = nextExpectedDataSource;
        Assert.IsTrue(emptyOpenInterest.IsDataSourceUpdated);
        Assert.IsTrue(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(nextExpectedDataSource, emptyOpenInterest.DataSource);
        sourceUpdates = emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestSource, (uint)nextExpectedDataSource);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.SubId == PQSubFieldKeys.OpenInterestSource
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQOpenInterest();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedDataSource, newEmpty.DataSource);
        Assert.IsTrue(newEmpty.IsDataSourceUpdated);
    }

    [TestMethod]
    public void EmptyOi_UpdateTimeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyOpenInterest.IsDataSourceUpdated);
        Assert.IsFalse(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(MarketDataSource.None, emptyOpenInterest.DataSource);
        Assert.AreEqual(0, emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());


        var expectedUpdateTime = new DateTime(2025, 5, 19, 9, 30, 26);
        emptyOpenInterest.UpdateTime = expectedUpdateTime;
        Assert.IsTrue(emptyOpenInterest.IsUpdatedDateUpdated);
        Assert.IsTrue(emptyOpenInterest.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(expectedUpdateTime, emptyOpenInterest.UpdateTime);
        var layerUpdates = emptyOpenInterest
                           .GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(2, layerUpdates.Count);
        var hoursSinceUnixEpoch = expectedUpdateTime.Get2MinIntervalsFromUnixEpoch();
        var expectedDateLayerField
            = new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestUpdateDate, hoursSinceUnixEpoch);
        var extended = expectedUpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var subHourBottom);
        var expectedSubHourLayerField = new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestUpdateSub2MinTime
                                                        , subHourBottom, extended);
        Assert.AreEqual(expectedDateLayerField, layerUpdates[0]);
        Assert.AreEqual(expectedSubHourLayerField, layerUpdates[1]);

        emptyOpenInterest.IsUpdatedDateUpdated        = false;
        emptyOpenInterest.IsUpdatedSub2MinTimeUpdated = false;
        Assert.IsFalse(emptyOpenInterest.HasUpdates);
        Assert.IsTrue(emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        emptyOpenInterest.IsUpdatedDateUpdated        = true;
        emptyOpenInterest.IsUpdatedSub2MinTimeUpdated = true;
        var quoteUpdates =
            (from update in emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update)
                where update.SubId is PQSubFieldKeys.OpenInterestUpdateDate or PQSubFieldKeys.OpenInterestUpdateSub2MinTime
                select update).ToList();
        Assert.AreEqual(2, quoteUpdates.Count);
        Assert.AreEqual(expectedDateLayerField, quoteUpdates[0]);
        Assert.AreEqual(expectedSubHourLayerField, quoteUpdates[1]);

        emptyOpenInterest.UpdateTime = DateTime.UnixEpoch;
        emptyOpenInterest.HasUpdates = false;

        var newEmpty = new PQOpenInterest();
        newEmpty.UpdateField(quoteUpdates[0]);
        newEmpty.UpdateField(quoteUpdates[1]);
        Assert.AreEqual(expectedUpdateTime, newEmpty.UpdateTime);
        Assert.IsTrue(newEmpty.HasUpdates);
        Assert.IsTrue(newEmpty.IsUpdatedDateUpdated);
        Assert.IsTrue(newEmpty.IsUpdatedSub2MinTimeUpdated);
    }

    [TestMethod]
    public void EmptyOi_VolumeChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyOpenInterest.IsVolumeUpdated);
        Assert.IsFalse(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(0m, emptyOpenInterest.Volume);
        Assert.AreEqual(0, emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        const decimal expectedVolume = 3_123_721m;
        emptyOpenInterest.Volume = expectedVolume;
        Assert.IsTrue(emptyOpenInterest.IsVolumeUpdated);
        Assert.IsTrue(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(expectedVolume, emptyOpenInterest.Volume);
        var sourceUpdates = emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestVolume, expectedVolume
                                                  , (PQFieldFlags)6);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyOpenInterest.IsVolumeUpdated = false;
        Assert.IsFalse(emptyOpenInterest.IsVolumeUpdated);
        Assert.IsFalse(emptyOpenInterest.HasUpdates);
        Assert.IsTrue(emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        const decimal nextExpectedVolume = 221_124m;
        emptyOpenInterest.Volume = nextExpectedVolume;
        Assert.IsTrue(emptyOpenInterest.IsVolumeUpdated);
        Assert.IsTrue(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(nextExpectedVolume, emptyOpenInterest.Volume);
        sourceUpdates = emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestVolume, nextExpectedVolume
                                              , (PQFieldFlags)6);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.SubId is PQSubFieldKeys.OpenInterestVolume
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQOpenInterest();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedVolume, newEmpty.Volume);
        Assert.IsTrue(newEmpty.IsVolumeUpdated);
    }

    [TestMethod]
    public void EmptyOi_VwapChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyOpenInterest.IsVwapUpdated);
        Assert.IsFalse(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(0m, emptyOpenInterest.Vwap);
        Assert.AreEqual(0, emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).Count());

        const decimal expectedVwap = 3_721m;
        emptyOpenInterest.Vwap = expectedVwap;
        Assert.IsTrue(emptyOpenInterest.IsVwapUpdated);
        Assert.IsTrue(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(expectedVwap, emptyOpenInterest.Vwap);
        var sourceUpdates = emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate
            = new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestVwap, expectedVwap, (PQFieldFlags)2);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyOpenInterest.IsVwapUpdated = false;
        Assert.IsFalse(emptyOpenInterest.IsVwapUpdated);
        Assert.IsFalse(emptyOpenInterest.HasUpdates);
        Assert.IsTrue(emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).IsNullOrEmpty());

        const decimal nextExpectedVwap = 124.0238m;
        emptyOpenInterest.Vwap = nextExpectedVwap;
        Assert.IsTrue(emptyOpenInterest.IsVwapUpdated);
        Assert.IsTrue(emptyOpenInterest.HasUpdates);
        Assert.AreEqual(nextExpectedVwap, emptyOpenInterest.Vwap);
        sourceUpdates = emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestVwap, nextExpectedVwap
                                              , (PQFieldFlags)2);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        sourceUpdates = (from update in emptyOpenInterest.GetDeltaUpdateFields(testDateTime, StorageFlags.Snapshot)
            where update.SubId is PQSubFieldKeys.OpenInterestVwap
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmpty = new PQOpenInterest();
        newEmpty.UpdateField(sourceUpdates[0]);
        Assert.AreEqual(nextExpectedVwap, newEmpty.Vwap);
        Assert.IsTrue(newEmpty.IsVwapUpdated);
    }

    [TestMethod]
    public void PopulatedOi_HasUpdates_ClearedAndSetAffectsAllTrackedFields()
    {
        Assert.IsTrue(populatedOpenInterest.HasUpdates);
        Assert.IsTrue(populatedOpenInterest.IsDataSourceUpdated);
        Assert.IsTrue(populatedOpenInterest.IsUpdatedDateUpdated);
        Assert.IsTrue(populatedOpenInterest.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(populatedOpenInterest.IsVolumeUpdated);
        Assert.IsTrue(populatedOpenInterest.IsVwapUpdated);
        populatedOpenInterest.HasUpdates = false;
        Assert.IsFalse(populatedOpenInterest.HasUpdates);
        Assert.IsFalse(populatedOpenInterest.IsDataSourceUpdated);
        Assert.IsFalse(populatedOpenInterest.IsUpdatedDateUpdated);
        Assert.IsFalse(populatedOpenInterest.IsUpdatedSub2MinTimeUpdated);
        Assert.IsFalse(populatedOpenInterest.IsVolumeUpdated);
        Assert.IsFalse(populatedOpenInterest.IsVwapUpdated);
    }

    [TestMethod]
    public void EmptyAndPopulatedOi_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedOpenInterest.IsEmpty);
        Assert.IsTrue(emptyOpenInterest.IsEmpty);
    }

    [TestMethod]
    public void PopulatedOi_SetIsEmpty_ReturnsReturnsLayerToEmptyExceptUpdatedFlags()
    {
        Assert.IsTrue(populatedOpenInterest.HasUpdates);
        Assert.IsTrue(populatedOpenInterest.IsDataSourceUpdated);
        Assert.IsTrue(populatedOpenInterest.IsUpdatedDateUpdated);
        Assert.IsTrue(populatedOpenInterest.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(populatedOpenInterest.IsVolumeUpdated);
        Assert.IsTrue(populatedOpenInterest.IsVwapUpdated);
        Assert.AreEqual(MarketDataSource.Adapter, populatedOpenInterest.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, populatedOpenInterest.UpdateTime);
        Assert.AreEqual(ExpectedVolume, populatedOpenInterest.Volume);
        Assert.AreEqual(ExpectedVwap, populatedOpenInterest.Vwap);
        populatedOpenInterest.IsEmpty = true;
        Assert.IsTrue(populatedOpenInterest.IsEmpty);
        Assert.AreEqual(MarketDataSource.None, populatedOpenInterest.DataSource);
        Assert.AreEqual(DateTime.MinValue, populatedOpenInterest.UpdateTime);
        Assert.AreEqual(0m, populatedOpenInterest.Volume);
        Assert.AreEqual(0m, populatedOpenInterest.Vwap);
        Assert.IsTrue(populatedOpenInterest.HasUpdates);
        Assert.IsTrue(populatedOpenInterest.IsDataSourceUpdated);
        Assert.IsTrue(populatedOpenInterest.IsUpdatedDateUpdated);
        Assert.IsTrue(populatedOpenInterest.IsUpdatedSub2MinTimeUpdated);
        Assert.IsTrue(populatedOpenInterest.IsVolumeUpdated);
        Assert.IsTrue(populatedOpenInterest.IsVwapUpdated);
    }

    [TestMethod]
    public void PopulatedOiWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllOiFields()
    {
        var pqFieldUpdates =
            populatedOpenInterest.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Update).ToList();
        AssertContainsAllOpenInterestFields(pqFieldUpdates, populatedOpenInterest);
    }

    [TestMethod]
    public void PopulatedOiWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllOiFields()
    {
        populatedOpenInterest.HasUpdates = false;
        var pqFieldUpdates =
            populatedOpenInterest.GetDeltaUpdateFields
                (new DateTime(2017, 12, 17, 12, 33, 1), StorageFlags.Snapshot).ToList();
        AssertContainsAllOpenInterestFields(pqFieldUpdates, populatedOpenInterest);
    }

    [TestMethod]
    public void PopulatedOiWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedOpenInterest.HasUpdates = false;
        var pqFieldUpdates =
            populatedOpenInterest.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 16, 33, 59), StorageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedOi_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewOi_CopiesAllFieldsToNewPvl()
    {
        var pqFieldUpdates =
            populatedOpenInterest.GetDeltaUpdateFields
                (new DateTime(2017, 11, 04, 13, 33, 3)
               , StorageFlags.Update | StorageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQOpenInterest();
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        Assert.AreEqual(populatedOpenInterest, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther()
    {
        var nonPQOpenInterest = new OpenInterest(populatedOpenInterest);
        emptyOpenInterest.CopyFrom(nonPQOpenInterest);
        Assert.AreEqual(populatedOpenInterest, emptyOpenInterest);
    }

    [TestMethod]
    public void FullyPopulatedPvl_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQOpenInterest();
        populatedOpenInterest.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedOpenInterest);
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
    public void NonPQPopulatedPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther()
    {
        var nonPQOi  = new OpenInterest(populatedOpenInterest);
        var newEmpty = new PQOpenInterest();
        newEmpty.CopyFrom(nonPQOi);
        Assert.IsTrue(populatedOpenInterest.AreEquivalent(newEmpty));
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedPvl = ((ICloneable<IPQOpenInterest>)populatedOpenInterest).Clone();

        Assert.AreNotSame(clonedPvl, populatedOpenInterest);
        Assert.AreEqual(populatedOpenInterest, clonedPvl);

        var cloned2 = (PQOpenInterest)((ICloneable)populatedOpenInterest).Clone();
        Assert.AreNotSame(cloned2, populatedOpenInterest);
        Assert.AreEqual(populatedOpenInterest, cloned2);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQOpenInterest)((ICloneable)populatedOpenInterest).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedOpenInterest, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedOpenInterest, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedPvlSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedOpenInterest, populatedOpenInterest);
        Assert.AreEqual(populatedOpenInterest, ((ICloneable)populatedOpenInterest).Clone());
        Assert.AreEqual(populatedOpenInterest, ((ICloneable<IOpenInterest>)populatedOpenInterest).Clone());
    }

    [TestMethod]
    public void FullyPopulatedPvl_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedOpenInterest.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedOpenInterest.ToString();

        Assert.IsTrue(toString.Contains(populatedOpenInterest.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedOpenInterest.DataSource)}: {populatedOpenInterest.DataSource}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedOpenInterest.UpdateTime)}: {populatedOpenInterest.UpdateTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedOpenInterest.Volume)}: {populatedOpenInterest.Volume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedOpenInterest.Vwap)}: {populatedOpenInterest.Vwap:N5}"));
    }

    public static void AssertContainsAllOpenInterestFields
    (IList<PQFieldUpdate> checkFieldUpdates, IPQOpenInterest oi,
        PQFieldFlags priceScale = (PQFieldFlags)2, PQFieldFlags volumeScale = (PQFieldFlags)6)
    {
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestSource, (uint)oi.DataSource),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OpenInterestTotal
                                                                  , PQSubFieldKeys.OpenInterestSource),
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");

        var orderUpdatedDate = oi.UpdateTime.Get2MinIntervalsFromUnixEpoch();
        var orderUpdatedDateFu
            = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OpenInterestTotal
                                                        , PQSubFieldKeys.OpenInterestUpdateDate);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestUpdateDate, orderUpdatedDate)
                      , orderUpdatedDateFu,
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");

        var orderUpdatedSub2MinExtended
            = oi.UpdateTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var orderUpdatedSubHour);
        var orderUpdatedSubHourFu
            = PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OpenInterestTotal
                                                        , PQSubFieldKeys.OpenInterestUpdateSub2MinTime);
        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestUpdateSub2MinTime, orderUpdatedSubHour, orderUpdatedSub2MinExtended)
                      , orderUpdatedSubHourFu,
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");


        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestVolume, oi.Volume, volumeScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OpenInterestTotal
                                                                  , PQSubFieldKeys.OpenInterestVolume, volumeScale),
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");

        Assert.AreEqual(new PQFieldUpdate(PQQuoteFields.OpenInterestTotal, PQSubFieldKeys.OpenInterestVwap, oi.Vwap, priceScale),
                        PQTickInstantTests.ExtractFieldUpdateWithId(checkFieldUpdates, PQQuoteFields.OpenInterestTotal
                                                                  , PQSubFieldKeys.OpenInterestVwap, priceScale),
                        $"For {oi}  with these fields\n{string.Join(",\n", checkFieldUpdates)}");
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQOpenInterest? original, IPQOpenInterest? changingOpenInterest,
        IOrderBookSide? originalOrderBook = null,
        IOrderBookSide? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingOpenInterest == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingOpenInterest);

        if (original.GetType() == changingOpenInterest.GetType())
            Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        else
            Assert.AreEqual(!exactComparison, original.AreEquivalent(
                                                                     changingOpenInterest, exactComparison));

        if (original.GetType() == typeof(PQOpenInterest))
            Assert.AreEqual(!exactComparison,
                            changingOpenInterest.AreEquivalent(new OpenInterest(original), exactComparison));

        // PriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType(exactComparison,
        //                                                                           original, changingOpenInterest, originalOrderBook
        //                                                                         , changingOrderBook, originalQuote, changingQuote);

        changingOpenInterest.IsDataSourceUpdated = !changingOpenInterest.IsDataSourceUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsDataSourceUpdated = original.IsDataSourceUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingOpenInterest.IsUpdatedDateUpdated = !changingOpenInterest.IsUpdatedDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsUpdatedDateUpdated = original.IsUpdatedDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingOpenInterest.IsUpdatedSub2MinTimeUpdated = !changingOpenInterest.IsUpdatedSub2MinTimeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsUpdatedSub2MinTimeUpdated = original.IsUpdatedSub2MinTimeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingOpenInterest.IsVolumeUpdated = !changingOpenInterest.IsVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsVolumeUpdated = original.IsVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingOpenInterest.IsVwapUpdated = !changingOpenInterest.IsVwapUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingOpenInterest.IsVwapUpdated = original.IsVwapUpdated;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}