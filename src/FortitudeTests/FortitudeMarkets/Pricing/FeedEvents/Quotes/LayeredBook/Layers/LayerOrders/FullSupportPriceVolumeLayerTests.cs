﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.InternalOrders;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

[TestClass]
public class FullSupportPriceVolumeLayerTests
{
    private const int     NumOfOrders    = 2;
    private const uint    OrderCount     = 2;
    private const decimal InternalVolume = 1_949_672m;

    private IPQNameIdLookupGenerator    emptyNameIdLookup = null!;
    private FullSupportPriceVolumeLayer emptyPvl          = null!;

    private bool[] expectNotPopulated = null!;
    private bool[] expectPopulated    = null!;

    private const QuoteInstantBehaviorFlags QuoteBehavior = QuoteInstantBehaviorFlags.None;

    private FullSupportPriceVolumeLayer populatedPvl = null!;

    private decimal  populatedQuotePrice;
    private uint     populatedQuoteRef;
    private decimal  populatedQuoteVolume;
    private string   populatedSourceName = null!;
    private DateTime populatedValueDate;

    [TestInitialize]
    public void SetUp()
    {
        expectPopulated    = [true, true];
        expectNotPopulated = [false, false];

        populatedSourceName  = "TestSourceName";
        emptyNameIdLookup    = new PQNameIdLookupGenerator(PQFeedFields.QuoteLayerStringUpdates);
        emptyPvl             = new FullSupportPriceVolumeLayer();
        populatedValueDate   = new DateTime(2017, 12, 26, 21, 00, 00); // only to the nearest hour.
        populatedQuoteRef    = 4_2949_672u;
        populatedQuotePrice  = 4.2949_672m;
        populatedQuoteVolume = 42_949_672m;

        populatedPvl = new FullSupportPriceVolumeLayer
            (populatedQuotePrice, populatedQuoteVolume, populatedValueDate, populatedSourceName, true, populatedQuoteRef);
        OrdersPriceVolumeLayerTests.AddCounterPartyOrders(populatedPvl, NumOfOrders);
    }

    [TestMethod]
    public void NewPvl_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        Assert.AreEqual(populatedQuotePrice, populatedPvl.Price);
        Assert.AreEqual(populatedQuoteVolume, populatedPvl.Volume);
        Assert.AreEqual(populatedValueDate, populatedPvl.ValueDate);
        Assert.AreEqual(populatedSourceName, populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.AreEqual(populatedQuoteRef, populatedPvl.SourceQuoteReference);
        OrdersPriceVolumeLayerTests.AssertOrderLayersAreExpected(populatedPvl, expectPopulated);

        Assert.AreEqual(0, emptyPvl.Price);
        Assert.AreEqual(0, emptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, emptyPvl.ValueDate);
        Assert.AreEqual(null, emptyPvl.SourceName);
        Assert.IsFalse(emptyPvl.Executable);
        Assert.AreEqual(0u, emptyPvl.SourceQuoteReference);
        OrdersPriceVolumeLayerTests.AssertOrderLayersAreExpected(emptyPvl, expectNotPopulated);

        var newEmptyPvl = new FullSupportPriceVolumeLayer();
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, newEmptyPvl.ValueDate);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
        OrdersPriceVolumeLayerTests.AssertOrderLayersAreExpected(newEmptyPvl, expectNotPopulated);
    }

    [TestMethod]
    public void NewPvl_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedPvl = new FullSupportPriceVolumeLayer
            (populatedQuotePrice, populatedQuoteVolume, populatedValueDate, populatedSourceName, true, populatedQuoteRef, OrderCount, InternalVolume);
        OrdersPriceVolumeLayerTests.AddCounterPartyOrders(newPopulatedPvl, NumOfOrders);
        var fromPQInstance = new FullSupportPriceVolumeLayer(newPopulatedPvl);
        Assert.AreEqual(populatedQuotePrice, fromPQInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromPQInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromPQInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromPQInstance.SourceName);
        Assert.IsTrue(fromPQInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromPQInstance.SourceQuoteReference);
        OrdersPriceVolumeLayerTests.AssertOrderLayersAreExpected(fromPQInstance, expectPopulated);

        var pqTraderPvl = new PQFullSupportPriceVolumeLayer
            (emptyNameIdLookup.Clone(), populatedQuotePrice, populatedQuoteVolume, populatedValueDate, populatedSourceName, true,
             populatedQuoteRef, OrderCount, InternalVolume);
        OrdersPriceVolumeLayerTests.AddCounterPartyOrders(pqTraderPvl, NumOfOrders);
        var fromNonPqInstance = new FullSupportPriceVolumeLayer(pqTraderPvl);
        Assert.AreEqual(populatedQuotePrice, fromNonPqInstance.Price);
        Assert.AreEqual(populatedQuoteVolume, fromNonPqInstance.Volume);
        Assert.AreEqual(populatedValueDate, fromNonPqInstance.ValueDate);
        Assert.AreEqual(populatedSourceName, fromNonPqInstance.SourceName);
        Assert.IsTrue(fromNonPqInstance.Executable);
        Assert.AreEqual(populatedQuoteRef, fromNonPqInstance.SourceQuoteReference);
        OrdersPriceVolumeLayerTests.AssertOrderLayersAreExpected(fromNonPqInstance, expectPopulated);

        var newEmptyPvl = new FullSupportPriceVolumeLayer(emptyPvl);
        Assert.AreEqual(0, newEmptyPvl.Price);
        Assert.AreEqual(0, newEmptyPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, newEmptyPvl.ValueDate);
        Assert.AreEqual(null, newEmptyPvl.SourceName);
        Assert.IsFalse(newEmptyPvl.Executable);
        Assert.AreEqual(0u, newEmptyPvl.SourceQuoteReference);
        OrdersPriceVolumeLayerTests.AssertOrderLayersAreExpected(newEmptyPvl, expectNotPopulated);
    }

    [TestMethod]
    public void PopulatedSrcQuoteRefPvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new SourceQuoteRefPriceVolumeLayer(populatedPvl);
        var emptyClone          = new FullSupportPriceVolumeLayer(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcQtRefTrdVlDtPvl = new PQSourceQuoteRefPriceVolumeLayer(populatedPvl, emptyNameIdLookup);
        emptyClone = new FullSupportPriceVolumeLayer(pqSrcQtRefTrdVlDtPvl);
        pqSrcQtRefTrdVlDtPvl.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedSrcNamePvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new SourcePriceVolumeLayer(populatedPvl);
        var emptyClone          = new FullSupportPriceVolumeLayer(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqSrcPvl = new PQSourcePriceVolumeLayer(populatedPvl, emptyNameIdLookup);
        emptyClone = new FullSupportPriceVolumeLayer(pqSrcPvl);
        pqSrcPvl.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedValueDatePvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new ValueDatePriceVolumeLayer(populatedPvl);
        var emptyClone          = new FullSupportPriceVolumeLayer(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqValueDatePvl = new ValueDatePriceVolumeLayer(populatedPvl);
        emptyClone = new FullSupportPriceVolumeLayer(pqValueDatePvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void PopulatedTraderPvl_NewFromCloneInstance_PvlsEquivalentEachOther()
    {
        var nonExactPriceVolume = new OrdersPriceVolumeLayer(populatedPvl, populatedPvl.LayerType);
        var emptyClone          = new FullSupportPriceVolumeLayer(nonExactPriceVolume);
        nonExactPriceVolume.AreEquivalent(emptyClone);

        var pqTrdPvl = new OrdersPriceVolumeLayer(populatedPvl, populatedPvl.LayerType);
        emptyClone = new FullSupportPriceVolumeLayer(pqTrdPvl);
        nonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void EmptyLayer_Mutate_UpdatesFields()
    {
        const decimal expectedPrice       = 3.45678m;
        const decimal expectedVolume      = 5.67890m;
        const string  expectedSourceName  = "NewSourceName";
        const uint    expectedSrcQuoteRef = 612656;
        var           expectedValueDate   = new DateTime(2018, 2, 10, 18, 0, 0);

        emptyPvl.Price      = expectedPrice;
        emptyPvl.Volume     = expectedVolume;
        emptyPvl.SourceName = expectedSourceName;
        emptyPvl.Executable = true;

        emptyPvl.SourceQuoteReference = expectedSrcQuoteRef;

        emptyPvl.ValueDate = expectedValueDate;

        Assert.AreEqual(expectedPrice, emptyPvl.Price);
        Assert.AreEqual(expectedVolume, emptyPvl.Volume);
        Assert.AreEqual(expectedSourceName, emptyPvl.SourceName);
        Assert.IsTrue(emptyPvl.Executable);
        Assert.AreEqual(expectedSrcQuoteRef, emptyPvl.SourceQuoteReference);
        Assert.AreEqual(expectedValueDate, emptyPvl.ValueDate);
    }

    [TestMethod]
    public void EmptyAndPopulatedPvl_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.IsTrue(emptyPvl.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPvl_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedPvl.IsEmpty);
        Assert.AreNotEqual(0m, populatedPvl.Price);
        Assert.AreNotEqual(0m, populatedPvl.Volume);
        Assert.AreNotEqual(DateTime.MinValue, populatedPvl.ValueDate);
        Assert.AreNotEqual(null, populatedPvl.SourceName);
        Assert.IsTrue(populatedPvl.Executable);
        Assert.AreEqual(populatedQuoteRef, populatedPvl.SourceQuoteReference);
        for (var i = 0; i < NumOfOrders; i++)
        {
            var anonOrderLayerInfo = populatedPvl[i];
            Assert.AreNotEqual(0m, anonOrderLayerInfo.OrderDisplayVolume);
            Assert.IsFalse(anonOrderLayerInfo.IsEmpty);
            if (anonOrderLayerInfo is IExternalCounterPartyOrder counterPartyOrderLayerInfo)
            {
                Assert.AreNotEqual(null, counterPartyOrderLayerInfo.ExternalCounterPartyName);
                Assert.AreNotEqual(null, counterPartyOrderLayerInfo.ExternalTraderName);
            }
        }

        populatedPvl.StateReset();
        Assert.IsTrue(populatedPvl.IsEmpty);
        Assert.AreEqual(0m, populatedPvl.Price);
        Assert.AreEqual(0m, populatedPvl.Volume);
        Assert.AreEqual(DateTime.MinValue, populatedPvl.ValueDate);
        Assert.AreEqual(null, populatedPvl.SourceName);
        Assert.IsFalse(populatedPvl.Executable);
        Assert.AreEqual(0u, populatedPvl.SourceQuoteReference);
        for (var i = 0; i < NumOfOrders; i++)
        {
            var anonOrderLayerInfo = populatedPvl[i];
            Assert.AreEqual(0m, anonOrderLayerInfo.OrderDisplayVolume);
            Assert.IsTrue(anonOrderLayerInfo.IsEmpty);
            if (anonOrderLayerInfo is IExternalCounterPartyOrder counterPartyOrderLayerInfo)
                Assert.AreEqual(null, counterPartyOrderLayerInfo.ExternalTraderName);
        }
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromNonPQSrcQtRefOrderVlDtToEmptyQuote_PvlsEqualEachOther()
    {
        var pqPriceVolume = new PQFullSupportPriceVolumeLayer(populatedPvl, emptyNameIdLookup);
        emptyPvl.CopyFrom(pqPriceVolume, QuoteBehavior);
        Assert.AreEqual(populatedPvl, emptyPvl);

        var fromConstructor = new FullSupportPriceVolumeLayer(pqPriceVolume);
        Assert.AreEqual(fromConstructor, populatedPvl);
    }

    [TestMethod]
    public void FullyPopulatedPvl_NewAndCopyFromSrcQtRefToEmptyQuote_PvlsEqualEachOther()
    {
        var pqNonExactQuote = new PQSourceQuoteRefPriceVolumeLayer(populatedPvl, emptyNameIdLookup);
        var emptyClone      = (IMutableFullSupportPriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(pqNonExactQuote, QuoteBehavior);
        pqNonExactQuote.AreEquivalent(emptyClone);

        var pqSrcQtRefTrdVlDtPvl = new SourceQuoteRefPriceVolumeLayer(populatedPvl);
        emptyClone = emptyPvl.Clone();
        emptyClone.CopyFrom(pqSrcQtRefTrdVlDtPvl, QuoteBehavior);
        pqNonExactQuote.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromSrcPvlToEmptyQuote_PvlsEqualEachOther()
    {
        var pqNonExactPriceVolume = new PQSourcePriceVolumeLayer(populatedPvl, emptyNameIdLookup);
        var emptyClone            = (IMutableFullSupportPriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(pqNonExactPriceVolume, QuoteBehavior);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);

        var srcPvl = new SourcePriceVolumeLayer(populatedPvl);
        emptyClone = emptyPvl.Clone();
        emptyClone.CopyFrom(srcPvl, QuoteBehavior);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromValueDatePvlToEmptyQuote_PvlsEqualEachOther()
    {
        var pqNonExactPriceVolume = new PQValueDatePriceVolumeLayer(populatedPvl);
        var emptyClone            = (IMutableFullSupportPriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(pqNonExactPriceVolume, QuoteBehavior);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);

        var valueDatePvl = new ValueDatePriceVolumeLayer(populatedPvl);
        emptyClone = emptyPvl.Clone();
        emptyClone.CopyFrom(valueDatePvl, QuoteBehavior);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_CopyFromTraderPvlToEmptyQuote_PvlsEqualEachOther()
    {
        var pqNonExactPriceVolume = new PQOrdersPriceVolumeLayer(populatedPvl, LayerType.OrdersFullPriceVolume, emptyNameIdLookup);
        var emptyClone            = (IMutableFullSupportPriceVolumeLayer)emptyPvl.Clone();
        emptyClone.CopyFrom(pqNonExactPriceVolume, QuoteBehavior);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);

        var trdPvl = new OrdersPriceVolumeLayer(populatedPvl, populatedPvl.LayerType);
        emptyClone = emptyPvl.Clone();
        emptyClone.CopyFrom(trdPvl, QuoteBehavior);
        pqNonExactPriceVolume.AreEquivalent(emptyClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IFullSupportPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote);
        var clonedQuote2 = ((IValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote2, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote2);
        var clonedQuote3 = ((ISourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote3, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote3);
        var clonedQuote4 = ((ICloneable<ISourcePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote4, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote4);
        var clonedQuote5 = ((IMutableSourcePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote5, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote5);
        var clonedQuote6 = ((ISourcePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote6, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote6);
        var clonedQuote8 = ((ICloneable<ISourceQuoteRefPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote8, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote8);
        var clonedQuote10 = ((ICloneable<IFullSupportPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote10, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote10);
        var clonedQuote12 = ((ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote12, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote12);
        var clonedQuote13 = ((IMutableSourceQuoteRefPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote13, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote13);
        var clonedQuote14 = ((ICloneable<IMutableFullSupportPriceVolumeLayer>)
            populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote14, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote14);
        var clonedQuote15 = ((IMutableFullSupportPriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote15, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote15);
        var clonedQuote16 = ((ICloneable<IMutableValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote16, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote16);
        var clonedQuote17 = ((IMutableValueDatePriceVolumeLayer)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote17, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote17);
        var clonedQuote18 = ((ICloneable<IValueDatePriceVolumeLayer>)populatedPvl).Clone();
        Assert.AreNotSame(clonedQuote18, populatedPvl);
        Assert.AreEqual(populatedPvl, clonedQuote18);
        var cloned20 = (FullSupportPriceVolumeLayer)((ICloneable)populatedPvl).Clone();
        Assert.AreNotSame(cloned20, populatedPvl);
        Assert.AreEqual(populatedPvl, cloned20);
    }

    [TestMethod]
    public void FullyPopulatedPvlCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (IMutableFullSupportPriceVolumeLayer)
            ((ICloneable)populatedPvl).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedPvl, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedPvl_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedPvl.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedPvl.ToString();

        Assert.IsTrue(toString.Contains(populatedPvl.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Price)}: {populatedPvl.Price:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Volume)}: {populatedPvl.Volume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.OrdersCount)}: {populatedPvl.OrdersCount}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.InternalVolume)}: {populatedPvl.InternalVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.SourceName)}: {populatedPvl.SourceName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.Executable)}: {populatedPvl.Executable}"));
        Assert.IsTrue
            (toString.Contains($"{nameof(populatedPvl.SourceQuoteReference)}: {populatedPvl.SourceQuoteReference:N0}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedPvl.ValueDate)}: {populatedPvl.ValueDate}"));
        Assert.IsTrue(toString.Contains("Orders: ["));
        for (var i = 0; i < populatedPvl.OrdersCount; i++) Assert.IsTrue(toString.Contains(populatedPvl[i].ToString()!));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IMutableFullSupportPriceVolumeLayer? original,
        IMutableFullSupportPriceVolumeLayer? changingPriceVolumeLayer,
        IOrderBookSide? originalOrderBookSide = null,
        IOrderBookSide? changingOrderBookSide = null,
        IOrderBook? originalOrderBook = null,
        IOrderBook? changingOrderBook = null,
        IPublishableLevel2Quote? originalQuote = null,
        IPublishableLevel2Quote? changingQuote = null)
    {
        if (original == null && changingPriceVolumeLayer == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingPriceVolumeLayer);

        OrdersPriceVolumeLayerTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingPriceVolumeLayer, originalOrderBookSide,
             changingOrderBookSide, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingPriceVolumeLayer.SourceName = "TestChangedSourceName";
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsFalse
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.SourceName = original.SourceName;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.Executable = !changingPriceVolumeLayer.Executable;
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsFalse
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.Executable = original.Executable;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.SourceQuoteReference = 98765432;
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsFalse
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.SourceQuoteReference = original.SourceQuoteReference;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingPriceVolumeLayer.ValueDate = new DateTime(2017, 11, 30, 21, 52, 02);
        Assert.IsFalse(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsFalse
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsFalse(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.IsFalse
                (originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingPriceVolumeLayer.ValueDate = original.ValueDate;
        Assert.IsTrue(original.AreEquivalent(changingPriceVolumeLayer, exactComparison));
        if (originalOrderBookSide != null)
            Assert.IsTrue
                (originalOrderBookSide.AreEquivalent(changingOrderBookSide, exactComparison));
        if (originalOrderBook != null) Assert.IsTrue(originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
