// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.InternalOrders;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

[TestClass]
public class ExternalCounterPartyOrderTests
{
    private const int       OrderNumber          = 80085;
    private const decimal   OrderVolume          = 100_000.50m;
    private const decimal   OrderRemainingVolume = 50_000.25m;
    private const string    CounterPartyName     = "TestCounterPartyName";
    private const string    TraderName           = "TestTraderName";
    private const OrderType ExpectedOrderType    = OrderType.Limit;
    private const uint ExpectedTrackingId    = 57;

    private const OrderGenesisFlags   ExpectedGenesisFlags   = OrderGenesisFlags.IsExternalOrder | OrderGenesisFlags.HasExternalCounterPartyInfo;
    private const OrderLifeCycleState ExpectedOrderLifeCycle = OrderLifeCycleState.ConfirmedActiveOnMarket;

    private static readonly DateTime CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private static readonly DateTime UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private ExternalCounterPartyOrder emptyCpoli = null!;

    private ExternalCounterPartyOrder populatedCpoli = null!;

    private PQNameIdLookupGenerator pqNameIdLookupGenerator = null!;

    [TestInitialize]
    public void SetUp()
    {
        pqNameIdLookupGenerator = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);

        emptyCpoli = new ExternalCounterPartyOrder();

        populatedCpoli =
            new ExternalCounterPartyOrder
                (new AnonymousOrder(OrderNumber, CreatedTime, OrderVolume,  ExpectedOrderType, ExpectedGenesisFlags, ExpectedOrderLifeCycle, UpdatedTime, OrderRemainingVolume, ExpectedTrackingId)
                 {
                     ExternalCounterPartyOrderInfo = new AdditionalExternalCounterPartyInfo(1, CounterPartyName, 2, TraderName)
                 });
    }

    [TestMethod]
    public void NewCpoli_SetsValues_PropertiesInitializedAsExpected()
    {
        var newCpoli = new ExternalCounterPartyOrder
            (new AnonymousOrder(OrderNumber, CreatedTime, OrderVolume,  ExpectedOrderType, ExpectedGenesisFlags, 
                                ExpectedOrderLifeCycle, UpdatedTime, OrderRemainingVolume, ExpectedTrackingId)
            {
                ExternalCounterPartyOrderInfo = new AdditionalExternalCounterPartyInfo(1, CounterPartyName, 2, TraderName)
            });

        Assert.AreEqual(OrderNumber, newCpoli.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, newCpoli.GenesisFlags);
        Assert.AreEqual(CreatedTime, newCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, newCpoli.UpdateTime);
        Assert.AreEqual(OrderVolume, newCpoli.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, newCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, newCpoli.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, newCpoli.ExternalTraderName);

        Assert.AreEqual(0, emptyCpoli.OrderId);
        Assert.AreEqual(OrderGenesisFlags.None | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, emptyCpoli.GenesisFlags);
        Assert.AreEqual(DateTime.MinValue, emptyCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, emptyCpoli.UpdateTime);
        Assert.AreEqual(0m, emptyCpoli.OrderDisplayVolume);
        Assert.AreEqual(0m, emptyCpoli.OrderRemainingVolume);
        Assert.IsNull(emptyCpoli.ExternalCounterPartyName);
        Assert.IsNull(emptyCpoli.ExternalTraderName);
    }

    [TestMethod]
    public void NewCpoli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var fromPopulatedInstance = new ExternalCounterPartyOrder(populatedCpoli);

        Assert.AreEqual(OrderNumber, fromPopulatedInstance.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, fromPopulatedInstance.GenesisFlags);
        Assert.AreEqual(CreatedTime, fromPopulatedInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPopulatedInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPopulatedInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPopulatedInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPopulatedInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromPopulatedInstance.ExternalTraderName);


        var newEmptyCpoli = new ExternalCounterPartyOrder(emptyCpoli);
        Assert.AreEqual(0, newEmptyCpoli.OrderId);
        Assert.AreEqual(OrderGenesisFlags.None | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, newEmptyCpoli.GenesisFlags);
        Assert.AreEqual(DateTime.MinValue, newEmptyCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, newEmptyCpoli.UpdateTime);
        Assert.AreEqual(0m, newEmptyCpoli.OrderDisplayVolume);
        Assert.AreEqual(0m, newEmptyCpoli.OrderRemainingVolume);
        Assert.IsNull(newEmptyCpoli.ExternalCounterPartyName);
        Assert.IsNull(newEmptyCpoli.ExternalTraderName);

        var pqCpoli        = new PQExternalCounterPartyOrder(populatedCpoli);
        var fromPqInstance = new ExternalCounterPartyOrder(pqCpoli);

        Assert.AreEqual(OrderNumber, fromPqInstance.OrderId);
        Assert.AreEqual(ExpectedGenesisFlags, fromPqInstance.GenesisFlags);
        Assert.AreEqual(CreatedTime, fromPqInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPqInstance.UpdateTime);
        Assert.AreEqual(OrderVolume, fromPqInstance.OrderDisplayVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPqInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPqInstance.ExternalCounterPartyName);
        Assert.AreEqual(TraderName, fromPqInstance.ExternalTraderName);

        var newFromPqEmptyCpoli = new ExternalCounterPartyOrder(new PQExternalCounterPartyOrder(pqNameIdLookupGenerator));
        Assert.AreEqual(0, newFromPqEmptyCpoli.OrderId);
        Assert.AreEqual(OrderGenesisFlags.None | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, newFromPqEmptyCpoli.GenesisFlags);
        Assert.AreEqual(DateTime.MinValue, newFromPqEmptyCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, newFromPqEmptyCpoli.UpdateTime);
        Assert.AreEqual(0m, newFromPqEmptyCpoli.OrderDisplayVolume);
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
        Assert.AreNotEqual(OrderGenesisFlags.None, populatedCpoli.GenesisFlags);
        Assert.AreNotEqual(DateTime.MinValue, populatedCpoli.CreatedTime);
        Assert.AreNotEqual(DateTime.MinValue, populatedCpoli.UpdateTime);
        Assert.AreNotEqual(0m, populatedCpoli.OrderDisplayVolume);
        Assert.AreNotEqual(0m, populatedCpoli.OrderRemainingVolume);
        Assert.IsNotNull(populatedCpoli.ExternalCounterPartyName);
        Assert.IsNotNull(populatedCpoli.ExternalTraderName);
        Assert.IsFalse(populatedCpoli.IsEmpty);
        populatedCpoli.StateReset();
        Assert.AreEqual(0, populatedCpoli.OrderId);
        Assert.AreEqual(OrderGenesisFlags.None | IExternalCounterPartyOrder.HasExternalCounterPartyOrderInfoFlags, populatedCpoli.GenesisFlags);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.UpdateTime);
        Assert.AreEqual(0m, populatedCpoli.OrderDisplayVolume);
        Assert.AreEqual(0m, populatedCpoli.OrderRemainingVolume);
        Assert.IsNull(populatedCpoli.ExternalCounterPartyName);
        Assert.IsNull(populatedCpoli.ExternalTraderName);
        Assert.IsTrue(populatedCpoli.IsEmpty);
    }

    [TestMethod]
    public void FullyPopulatedPqCpoli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQTraderLayerInfo = new PQExternalCounterPartyOrder(populatedCpoli);
        emptyCpoli.CopyFrom(nonPQTraderLayerInfo);
        Assert.AreEqual(populatedCpoli, emptyCpoli);
    }

    [TestMethod]
    public void FromInterfacePopulatedNameLookupId_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = ((IMutableAnonymousOrder)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = ((IAnonymousOrder)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = ((IMutableExternalCounterPartyOrder)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = ((IExternalCounterPartyOrder)populatedCpoli).Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
        clone = populatedCpoli.Clone();
        Assert.AreNotSame(clone, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clone);
    }

    [TestMethod]
    public void FullyPopulatedTliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = ((IMutableExternalCounterPartyOrder)populatedCpoli).Clone();
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

        Assert.IsTrue(toString!.Contains(populatedCpoli.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderId)}: {populatedCpoli.OrderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.GenesisFlags)}: {populatedCpoli.GenesisFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.CreatedTime)}: {populatedCpoli.CreatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.UpdateTime)}: {populatedCpoli.UpdateTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderDisplayVolume)}: {populatedCpoli.OrderDisplayVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderRemainingVolume)}: {populatedCpoli.OrderRemainingVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.ExternalCounterPartyName)}: {populatedCpoli.ExternalCounterPartyName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.ExternalTraderName)}: {populatedCpoli.ExternalTraderName}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableExternalCounterPartyOrder? original,
        IMutableExternalCounterPartyOrder? changingTraderLayerInfo,
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

        AnonymousOrderTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingTraderLayerInfo, originalTraderPriceVolumeLayer, changingTraderPriceVolumeLayer
           , originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingTraderLayerInfo.ExternalCounterPartyName = "TestChangedCounterPartyName";
        Assert.IsFalse(original.AreEquivalent((IAnonymousOrder)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.ExternalCounterPartyName = original.ExternalCounterPartyName;
        Assert.IsTrue(original.AreEquivalent((IAnonymousOrder)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.ExternalTraderName = "TestChangedTraderName";
        Assert.IsFalse(original.AreEquivalent((IAnonymousOrder)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsFalse
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsFalse
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.ExternalTraderName = ((IExternalCounterPartyOrder)original).ExternalTraderName;
        Assert.IsTrue(original.AreEquivalent((IAnonymousOrder)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue
                (originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue
                (originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
