// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerOrders;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.LayeredBook.LayerOrders;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

[TestClass]
public class CounterPartyOrderLayerInfoTests
{
    private const LayerOrderFlags OrderFlags = LayerOrderFlags.ExplicitlyDefinedFromSource | LayerOrderFlags.IsInternallyCreatedOrder;

    private const int     OrderNumber          = 80085;
    private const decimal OrderVolume          = 100_000.50m;
    private const decimal OrderRemainingVolume = 50_000.25m;
    private const string  CounterPartyName     = "TestCounterPartyName";
    private const string  TraderName           = "TestTraderName";

    private readonly DateTime CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private readonly DateTime UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private CounterPartyOrderLayerInfo emptyCpoli = null!;

    private CounterPartyOrderLayerInfo populatedCpoli = null!;

    private PQNameIdLookupGenerator pqNameIdLookupGenerator = null!;

    [TestInitialize]
    public void SetUp()
    {
        pqNameIdLookupGenerator = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);

        emptyCpoli = new CounterPartyOrderLayerInfo();

        populatedCpoli =
            new CounterPartyOrderLayerInfo(OrderNumber, OrderFlags, CreatedTime, OrderVolume, UpdatedTime,
                                           OrderRemainingVolume, CounterPartyName, TraderName);
    }

    [TestMethod]
    public void NewCpoli_SetsValues_PropertiesInitializedAsExpected()
    {
        var newCpoli = new CounterPartyOrderLayerInfo(OrderNumber, OrderFlags, CreatedTime, OrderVolume, UpdatedTime,
                                                      OrderRemainingVolume, CounterPartyName, TraderName);

        Assert.AreEqual(OrderNumber, newCpoli.OrderId);
        Assert.AreEqual(OrderFlags, newCpoli.OrderFlags);
        Assert.AreEqual(CreatedTime, newCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, newCpoli.UpdatedTime);
        Assert.AreEqual(OrderVolume, newCpoli.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, newCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, newCpoli.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, newCpoli.ExternalTraderName);

        Assert.AreEqual(0, emptyCpoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, emptyCpoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, emptyCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, emptyCpoli.UpdatedTime);
        Assert.AreEqual(0m, emptyCpoli.OrderVolume);
        Assert.AreEqual(0m, emptyCpoli.OrderRemainingVolume);
        Assert.IsNull(emptyCpoli.ExternalCounterPartyName);
        Assert.IsNull(emptyCpoli.ExternalTraderName);
    }

    [TestMethod]
    public void NewCpoli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var fromPopulatedInstance = new CounterPartyOrderLayerInfo(populatedCpoli);

        Assert.AreEqual(OrderNumber, fromPopulatedInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPopulatedInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPopulatedInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPopulatedInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPopulatedInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPopulatedInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPopulatedInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromPopulatedInstance.ExternalTraderName);


        var newEmptyCpoli = new CounterPartyOrderLayerInfo(emptyCpoli);
        Assert.AreEqual(0, newEmptyCpoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, newEmptyCpoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, newEmptyCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, newEmptyCpoli.UpdatedTime);
        Assert.AreEqual(0m, newEmptyCpoli.OrderVolume);
        Assert.AreEqual(0m, newEmptyCpoli.OrderRemainingVolume);
        Assert.IsNull(newEmptyCpoli.ExternalCounterPartyName);
        Assert.IsNull(newEmptyCpoli.ExternalTraderName);

        var pqCpoli        = new PQCounterPartyOrderLayerInfo(populatedCpoli, pqNameIdLookupGenerator);
        var fromPqInstance = new CounterPartyOrderLayerInfo(pqCpoli);

        Assert.AreEqual(OrderNumber, fromPqInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPqInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPqInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPqInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPqInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPqInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPqInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromPqInstance.ExternalTraderName);

        var newFromPqEmptyCpoli = new CounterPartyOrderLayerInfo(new PQCounterPartyOrderLayerInfo(pqNameIdLookupGenerator));
        Assert.AreEqual(0, newFromPqEmptyCpoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, newFromPqEmptyCpoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, newFromPqEmptyCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, newFromPqEmptyCpoli.UpdatedTime);
        Assert.AreEqual(0m, newFromPqEmptyCpoli.OrderVolume);
        Assert.AreEqual(0m, newFromPqEmptyCpoli.OrderRemainingVolume);
        Assert.IsNull(newFromPqEmptyCpoli.ExternalCounterPartyName);
        Assert.IsNull(newFromPqEmptyCpoli.ExternalTraderName);
    }

    [TestMethod]
    public void EmptyAndPopulatedCpoli_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedCpoli.IsEmpty);
        Assert.IsTrue(emptyCpoli.IsEmpty);
    }

    [TestMethod]
    public void PopulatedCpoli_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedCpoli.IsEmpty);
        Assert.AreNotEqual(0, populatedCpoli.OrderId);
        Assert.AreNotEqual(LayerOrderFlags.None, populatedCpoli.OrderFlags);
        Assert.AreNotEqual(DateTime.MinValue, populatedCpoli.CreatedTime);
        Assert.AreNotEqual(DateTime.MinValue, populatedCpoli.UpdatedTime);
        Assert.AreNotEqual(0m, populatedCpoli.OrderVolume);
        Assert.AreNotEqual(0m, populatedCpoli.OrderRemainingVolume);
        Assert.IsNotNull(populatedCpoli.ExternalCounterPartyName);
        Assert.IsNotNull(populatedCpoli.ExternalTraderName);
        Assert.IsFalse(populatedCpoli.IsEmpty);
        populatedCpoli.StateReset();
        Assert.AreEqual(0, populatedCpoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, populatedCpoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.UpdatedTime);
        Assert.AreEqual(0m, populatedCpoli.OrderVolume);
        Assert.AreEqual(0m, populatedCpoli.OrderRemainingVolume);
        Assert.IsNull(populatedCpoli.ExternalCounterPartyName);
        Assert.IsNull(populatedCpoli.ExternalTraderName);
        Assert.IsTrue(populatedCpoli.IsEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPqCpoli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQTraderLayerInfo = new PQCounterPartyOrderLayerInfo(populatedCpoli, pqNameIdLookupGenerator);
        emptyCpoli.CopyFrom(nonPQTraderLayerInfo);
        Assert.AreEqual(populatedCpoli, emptyCpoli);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = ((IMutableAnonymousOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = ((IAnonymousOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = ((IMutableCounterPartyOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = ((ICounterPartyOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = populatedCpoli.Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
    }

    [TestMethod]
    public void FullyPopulatedTliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableCounterPartyOrderLayerInfo)populatedCpoli).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedCpoli, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedTli_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedCpoli.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedCpoli.ToString();

        Assert.IsTrue(toString.Contains(populatedCpoli.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderId)}: {populatedCpoli.OrderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderFlags)}: {populatedCpoli.OrderFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.CreatedTime)}: {populatedCpoli.CreatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.UpdatedTime)}: {populatedCpoli.UpdatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderVolume)}: {populatedCpoli.OrderVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderRemainingVolume)}: {populatedCpoli.OrderRemainingVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.ExternalCounterPartyName)}: {populatedCpoli.ExternalCounterPartyName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.ExternalTraderName)}: {populatedCpoli.ExternalTraderName}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableCounterPartyOrderLayerInfo? original,
        IMutableCounterPartyOrderLayerInfo? changingTraderLayerInfo,
        IOrdersPriceVolumeLayer? originalTraderPriceVolumeLayer = null,
        IOrdersPriceVolumeLayer? changingTraderPriceVolumeLayer = null,
        IOrderBookSide? originalOrderBook = null,
        IOrderBookSide? changingOrderBook = null,
        IPublishableLevel2Quote? originalQuote = null,
        IPublishableLevel2Quote? changingQuote = null)
    {
        if (original == null && changingTraderLayerInfo == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingTraderLayerInfo);

        AnonymousOrderLayerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingTraderLayerInfo, originalTraderPriceVolumeLayer, changingTraderPriceVolumeLayer
           , originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingTraderLayerInfo.ExternalCounterPartyName = "TestChangedCounterPartyName";
        Assert.IsFalse(original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.ExternalCounterPartyName = original.ExternalCounterPartyName;
        Assert.IsTrue(original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.ExternalTraderName = "TestChangedTraderName";
        Assert.IsFalse(original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.ExternalTraderName = original.ExternalTraderName;
        Assert.IsTrue(original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
