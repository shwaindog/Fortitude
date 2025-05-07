
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

[TestClass]
public class OpenInterestTests
{
    private OpenInterest emptyOpenInterest     = null!;
    private OpenInterest populatedOpenInterest = null!;

    private const decimal ExpectedVolume = 234_981m;
    private const decimal ExpectedVwap   = 23.1092m;

    private static readonly DateTime ExpectedUpdateTime = new(2025, 5, 6, 16, 11, 52);

    [TestInitialize]
    public void SetUp()
    {
        emptyOpenInterest     = new OpenInterest();
        populatedOpenInterest = new OpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newOi = new OpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        Assert.AreEqual(MarketDataSource.Adapter, newOi.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, newOi.UpdateTime);
        Assert.AreEqual(ExpectedVolume, newOi.Volume);
        Assert.AreEqual(ExpectedVwap, newOi.Vwap);

        Assert.AreEqual(MarketDataSource.None, emptyOpenInterest.DataSource);
        Assert.AreEqual(DateTime.MinValue, emptyOpenInterest.UpdateTime);
        Assert.AreEqual(0m, emptyOpenInterest.Volume);
        Assert.AreEqual(0m, emptyOpenInterest.Vwap);
    }

    [TestMethod]
    public void NewOi_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedOi = new PQOpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        var fromPQInstance  = new OpenInterest(newPopulatedOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromPQInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromPQInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromPQInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromPQInstance.Vwap);

        var nonPQOi           = new OpenInterest(MarketDataSource.Adapter, ExpectedVolume, ExpectedVwap, ExpectedUpdateTime);
        var fromNonPqInstance = new OpenInterest(nonPQOi);
        Assert.AreEqual(MarketDataSource.Adapter, fromNonPqInstance.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, fromNonPqInstance.UpdateTime);
        Assert.AreEqual(ExpectedVolume, fromNonPqInstance.Volume);
        Assert.AreEqual(ExpectedVwap, fromNonPqInstance.Vwap);

        var newEmptyOi = new OpenInterest(emptyOpenInterest);
        Assert.AreEqual(MarketDataSource.None, newEmptyOi.DataSource);
        Assert.AreEqual(DateTime.MinValue, newEmptyOi.UpdateTime);
        Assert.AreEqual(0m, newEmptyOi.Volume);
        Assert.AreEqual(0m, newEmptyOi.Vwap);
    }

    [TestMethod]
    public void EmptyLayer_Mutate_UpdatesFields()
    {
        emptyOpenInterest.DataSource = MarketDataSource.Adapter;
        emptyOpenInterest.UpdateTime = ExpectedUpdateTime;
        emptyOpenInterest.Volume     = ExpectedVolume;
        emptyOpenInterest.Vwap     = ExpectedVwap;
        
        Assert.AreEqual(MarketDataSource.Adapter, emptyOpenInterest.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, emptyOpenInterest.UpdateTime);
        Assert.AreEqual(ExpectedVolume, emptyOpenInterest.Volume);
        Assert.AreEqual(ExpectedVwap, emptyOpenInterest.Vwap);
    }

    [TestMethod]
    public void PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedOpenInterest.IsEmpty);
        Assert.AreEqual(MarketDataSource.Adapter, populatedOpenInterest.DataSource);
        Assert.AreEqual(ExpectedUpdateTime, populatedOpenInterest.UpdateTime);
        Assert.AreEqual(ExpectedVolume, populatedOpenInterest.Volume);
        Assert.AreEqual(ExpectedVwap, populatedOpenInterest.Vwap);
        populatedOpenInterest.StateReset();
        Assert.IsTrue(populatedOpenInterest.IsEmpty);
        Assert.AreEqual(MarketDataSource.None, populatedOpenInterest.DataSource);
        Assert.AreEqual(DateTime.MinValue, populatedOpenInterest.UpdateTime);
        Assert.AreEqual(0m, populatedOpenInterest.Volume);
        Assert.AreEqual(0m, populatedOpenInterest.Vwap);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromToEmptyPvl_PvlsEqualEachOther()
    {
        emptyOpenInterest.CopyFrom(populatedOpenInterest);
        Assert.AreEqual(populatedOpenInterest, emptyOpenInterest);
    }

    [TestMethod]
    public void PQPvl_CopyFromToEmptyPvl_LayersEquivalentToEachOther()
    {
        var pqPvl    = new PQOpenInterest(populatedOpenInterest);
        var newEmpty = new OpenInterest();
        newEmpty.CopyFrom(pqPvl);
        Assert.AreEqual(populatedOpenInterest, newEmpty);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        IOpenInterest clone = populatedOpenInterest.Clone();
        Assert.AreNotSame(clone, populatedOpenInterest);
        Assert.AreEqual(populatedOpenInterest, clone);
        clone = ((IOpenInterest)populatedOpenInterest).Clone();
        Assert.AreNotSame(clone, populatedOpenInterest);
        Assert.AreEqual(populatedOpenInterest, clone);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (OpenInterest)((ICloneable)populatedOpenInterest).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedOpenInterest, fullyPopulatedClone);
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

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableOpenInterest? original, IMutableOpenInterest? changingOpenInterest,
        IOrderBookSide? originalOrderBookSide = null,
        IOrderBookSide? changingOrderBookSide = null,
        IMutableOrderBook? originalOrderBook = null,
        IMutableOrderBook? changingOrderBook = null,
        ILevel2Quote? originalLevel2Quote = null,
        ILevel2Quote? changingLevel2Quote = null)
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