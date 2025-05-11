
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

[TestClass]
public class MarketAggregateTests
{
    private MarketAggregate emptyMarketAggregate     = null!;
    private MarketAggregate populatedMarketAggregate = null!;

    private const decimal ExpectedVolume = 234_981m;
    private const decimal ExpectedVwap   = 23.1092m;

    private static readonly DateTime ExpectedUpdateTime = new(2025, 5, 6, 16, 11, 52);

    [TestInitialize]
    public void SetUp()
    {
        emptyMarketAggregate     = new MarketAggregate();
        populatedMarketAggregate = new MarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newOi = new MarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        Assert.AreEqual(MarketDataSource.Adapter, newOi.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, newOi.UpdateTime);
        Assert.AreEqual(ExpectedVolume, newOi.Volume);
        Assert.AreEqual(ExpectedVwap, newOi.Vwap);

        Assert.AreEqual(MarketDataSource.None, emptyMarketAggregate.DataSource);
        Assert.AreEqual(DateTime.MinValue, emptyMarketAggregate.UpdateTime);
        Assert.AreEqual(0m, emptyMarketAggregate.Volume);
        Assert.AreEqual(0m, emptyMarketAggregate.Vwap);
    }

    [TestMethod]
    public void NewOi_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedOi = new PQMarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        var fromPQInstance  = new MarketAggregate(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);

        var nonPQOi           = new MarketAggregate(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        var fromNonPqInstance = new MarketAggregate(nonPQOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromNonPqInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromNonPqInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromNonPqInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromNonPqInstance.Vwap);

        var newEmptyOi = new MarketAggregate(emptyMarketAggregate);
        Assert.AreEqual(MarketDataSource.None, newEmptyOi.DataSource);
        Assert.AreEqual(DateTime.MinValue, newEmptyOi.UpdateTime);
        Assert.AreEqual(0m, newEmptyOi.Volume);
        Assert.AreEqual(0m, newEmptyOi.Vwap);
    }

    [TestMethod]
    public void EmptyLayer_Mutate_UpdatesFields()
    {
        emptyMarketAggregate.DataSource = MarketDataSource.Adapter;
        emptyMarketAggregate.UpdateTime = ExpectedUpdateTime;
        emptyMarketAggregate.Volume     = ExpectedVolume;
        emptyMarketAggregate.Vwap     = ExpectedVwap;
        
        Assert.AreEqual(MarketDataSource.Adapter, emptyMarketAggregate.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, emptyMarketAggregate.UpdateTime);
        Assert.AreEqual(ExpectedVolume, emptyMarketAggregate.Volume);
        Assert.AreEqual(ExpectedVwap, emptyMarketAggregate.Vwap);
    }

    [TestMethod]
    public void PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedMarketAggregate.IsEmpty);
        Assert.AreEqual(MarketDataSource.Adapter, populatedMarketAggregate.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, populatedMarketAggregate.UpdateTime);
        Assert.AreEqual(ExpectedVolume, populatedMarketAggregate.Volume);
        Assert.AreEqual(ExpectedVwap, populatedMarketAggregate.Vwap);
        populatedMarketAggregate.StateReset();
        Assert.IsTrue(populatedMarketAggregate.IsEmpty);
        Assert.AreEqual(MarketDataSource.None, populatedMarketAggregate.DataSource);
        Assert.AreEqual(DateTime.MinValue, populatedMarketAggregate.UpdateTime);
        Assert.AreEqual(0m, populatedMarketAggregate.Volume);
        Assert.AreEqual(0m, populatedMarketAggregate.Vwap);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther()
    {
        emptyMarketAggregate.CopyFrom(populatedMarketAggregate);
        Assert.AreEqual(populatedMarketAggregate, emptyMarketAggregate);
    }

    [TestMethod]
    public void PQPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther()
    {
        var pqPvl    = new PQMarketAggregate(populatedMarketAggregate);
        var newEmpty = new MarketAggregate();
        newEmpty.CopyFrom(pqPvl);
        Assert.AreEqual(populatedMarketAggregate, newEmpty);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        IMarketAggregate clone = populatedMarketAggregate.Clone();
        Assert.AreNotSame(clone, populatedMarketAggregate);
        Assert.AreEqual(populatedMarketAggregate, clone);
        clone = ((IMarketAggregate)populatedMarketAggregate).Clone();
        Assert.AreNotSame(clone, populatedMarketAggregate);
        Assert.AreEqual(populatedMarketAggregate, clone);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (MarketAggregate)((ICloneable)populatedMarketAggregate).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedMarketAggregate, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedMarketAggregate.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedMarketAggregate.ToString();

        Assert.IsTrue(toString.Contains(populatedMarketAggregate.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedMarketAggregate.DataSource)}: {populatedMarketAggregate.DataSource}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedMarketAggregate.UpdateTime)}: {populatedMarketAggregate.UpdateTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedMarketAggregate.Volume)}: {populatedMarketAggregate.Volume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedMarketAggregate.Vwap)}: {populatedMarketAggregate.Vwap:N5}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableMarketAggregate? original, IMutableMarketAggregate? changingOpenInterest,
        IOrderBookSide? originalOrderBookSide = null,
        IOrderBookSide? changingOrderBookSide = null,
        IMutableOrderBook? originalOrderBook = null,
        IMutableOrderBook? changingOrderBook = null,
        IPublishableLevel2Quote? originalLevel2Quote = null,
        IPublishableLevel2Quote? changingLevel2Quote = null)
    {
        if (original == null && changingOpenInterest == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingOpenInterest);

        if (original.GetType() == changingOpenInterest.GetType())
            Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        else
            Assert.AreEqual
                (!exactComparison, original.AreEquivalent(changingOpenInterest, exactComparison));

        changingOpenInterest.DataSource = changingOpenInterest.DataSource == MarketDataSource.Venue ? MarketDataSource.Adapter : MarketDataSource.Venue;
        Assert.IsFalse(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsFalse
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalLevel2Quote != null) Assert.IsFalse(originalLevel2Quote.AreEquivalent(changingLevel2Quote, exactComparison));
        changingOpenInterest.DataSource = original.DataSource;
        Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalLevel2Quote != null) Assert.IsTrue(originalLevel2Quote.AreEquivalent(changingLevel2Quote, exactComparison));

        if (original.DataSource is not (MarketDataSource.None or MarketDataSource.Published))
        {
            changingOpenInterest.UpdateTime = new DateTime(2025, 5, 7, 19, 53, 11);
            Assert.IsFalse(original.AreEquivalent(changingOpenInterest, exactComparison));
            if (originalOrderBookSide != null)
                Assert.IsFalse
                    (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
            if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalLevel2Quote != null) Assert.IsFalse(originalLevel2Quote.AreEquivalent(changingLevel2Quote, exactComparison));
            changingOpenInterest.UpdateTime = original.UpdateTime;
            Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
            if (originalOrderBookSide != null)
                Assert.IsTrue
                    (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
            if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalLevel2Quote != null) Assert.IsTrue(originalLevel2Quote.AreEquivalent(changingLevel2Quote, exactComparison));

            changingOpenInterest.Volume = 98765432m;
            Assert.IsFalse(original.AreEquivalent(changingOpenInterest, exactComparison));
            if (originalOrderBookSide != null)
                Assert.IsFalse
                    (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
            if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalLevel2Quote != null) Assert.IsFalse(originalLevel2Quote.AreEquivalent(changingLevel2Quote, exactComparison));
            changingOpenInterest.Volume = original.Volume;
            Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
            if (originalOrderBookSide != null)
                Assert.IsTrue
                    (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
            if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalLevel2Quote != null) Assert.IsTrue(originalLevel2Quote.AreEquivalent(changingLevel2Quote, exactComparison));

            changingOpenInterest.Vwap = 98765432m;
            Assert.IsFalse(original.AreEquivalent(changingOpenInterest, exactComparison));
            if (originalOrderBookSide != null)
                Assert.IsFalse
                    (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
            if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalLevel2Quote != null) Assert.IsFalse(originalLevel2Quote.AreEquivalent(changingLevel2Quote, exactComparison));
            changingOpenInterest.Vwap = original.Vwap;
            Assert.IsTrue(original.AreEquivalent(changingOpenInterest, exactComparison));
            if (originalOrderBookSide != null)
                Assert.IsTrue
                    (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
            if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
            if (originalLevel2Quote != null) Assert.IsTrue(originalLevel2Quote.AreEquivalent(changingLevel2Quote, exactComparison));
        }
    }
}