// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQCounterPartyOrderLayerInfoTests
{
    private const    int                           OrderId = 80085;
    private const    LayerOrderFlags               OrderFlags = LayerOrderFlags.CreatedFromSource | LayerOrderFlags.IsInternallyCreatedOrder;
    private const    decimal                       OrderVolume = 100_000.50m;
    private const    decimal                       OrderRemainingVolume = 50_000.25m;
    private const    string                        CounterPartyName = "TestCounterPartyName";
    private const    string                        TraderName = "TestTraderName";
    private readonly DateTime                      CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private readonly DateTime                      UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);
    private          IPQCounterPartyOrderLayerInfo emptyCpoli = null!;

    private IPQNameIdLookupGenerator      emptyNameIdLookup = null!;
    private IPQNameIdLookupGenerator      nameIdLookup      = null!;
    private IPQCounterPartyOrderLayerInfo populatedCpoli    = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        nameIdLookup      = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        emptyCpoli        = new PQCounterPartyOrderLayerInfo(emptyNameIdLookup.Clone());
        populatedCpoli = new PQCounterPartyOrderLayerInfo(nameIdLookup, OrderId, OrderFlags, CreatedTime, OrderVolume, UpdatedTime,
                                                          OrderRemainingVolume, CounterPartyName, TraderName);
    }

    [TestMethod]
    public void NewTli_SetsValues_PropertiesInitializedAsExpected()
    {
        var newCpoli = new PQCounterPartyOrderLayerInfo(nameIdLookup, OrderId, OrderFlags, CreatedTime, OrderVolume, UpdatedTime,
                                                        OrderRemainingVolume, CounterPartyName, TraderName);

        Assert.AreEqual(OrderId, newCpoli.OrderId);
        Assert.AreEqual(OrderFlags, newCpoli.OrderFlags);
        Assert.AreEqual(CreatedTime, newCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, newCpoli.UpdatedTime);
        Assert.AreEqual(OrderVolume, newCpoli.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, newCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, newCpoli.CounterPartyName);
        Assert.AreEqual(TraderName, newCpoli.TraderName);
        Assert.IsNotNull(newCpoli.NameIdLookup);
        Assert.IsTrue(newCpoli.IsTraderNameUpdated);
        Assert.IsTrue(newCpoli.IsOrderIdUpdated);
        Assert.IsTrue(newCpoli.IsOrderFlagsUpdated);
        Assert.IsTrue(newCpoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(newCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(newCpoli.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(newCpoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(newCpoli.IsOrderVolumeUpdated);
        Assert.IsTrue(newCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(newCpoli.IsCounterPartyNameUpdated);
        Assert.IsTrue(newCpoli.IsTraderNameUpdated);
        Assert.IsFalse(newCpoli.IsEmpty);
        Assert.IsTrue(newCpoli.HasUpdates);

        var fromEmptyAoli = new PQCounterPartyOrderLayerInfo(emptyNameIdLookup);
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);
        Assert.IsNotNull(fromEmptyAoli.NameIdLookup);
        Assert.IsNull(fromEmptyAoli.CounterPartyName);
        Assert.IsNull(fromEmptyAoli.TraderName);
        Assert.IsFalse(fromEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCounterPartyNameUpdated);
        Assert.IsFalse(fromEmptyAoli.IsTraderNameUpdated);
        Assert.IsTrue(fromEmptyAoli.IsEmpty);
        Assert.IsFalse(fromEmptyAoli.HasUpdates);

        var nonPqInstance = new CounterPartyOrderLayerInfo(populatedCpoli);
        var fromNonPQInstance
            = new PQCounterPartyOrderLayerInfo(nonPqInstance, new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand));
        Assert.AreEqual(populatedCpoli.OrderId, fromNonPQInstance.OrderId);
        Assert.AreEqual(populatedCpoli.OrderFlags, fromNonPQInstance.OrderFlags);
        Assert.AreEqual(populatedCpoli.CreatedTime, fromNonPQInstance.CreatedTime);
        Assert.AreEqual(populatedCpoli.UpdatedTime, fromNonPQInstance.UpdatedTime);
        Assert.AreEqual(populatedCpoli.OrderVolume, fromNonPQInstance.OrderVolume);
        Assert.AreEqual(populatedCpoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.AreEqual(populatedCpoli.CounterPartyName, fromNonPQInstance.CounterPartyName);
        Assert.AreEqual(TraderName, fromNonPQInstance.TraderName);
        Assert.IsNotNull(fromNonPQInstance.NameIdLookup);
        Assert.IsTrue(fromNonPQInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderFlagsUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCounterPartyNameUpdated);
        Assert.IsTrue(fromNonPQInstance.IsTraderNameUpdated);
        Assert.IsFalse(fromNonPQInstance.IsEmpty);
        Assert.IsTrue(fromNonPQInstance.HasUpdates);

        var newNonPqEmptyAoli = new CounterPartyOrderLayerInfo(emptyCpoli);
        var fromNonPqEmptyAoli
            = new PQCounterPartyOrderLayerInfo(newNonPqEmptyAoli, new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand));
        Assert.AreEqual(0, fromNonPqEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromNonPqEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderRemainingVolume);
        Assert.IsNull(fromNonPqEmptyAoli.CounterPartyName);
        Assert.IsNull(fromNonPqEmptyAoli.TraderName);
        Assert.IsNotNull(fromNonPqEmptyAoli.NameIdLookup);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCounterPartyNameUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsTraderNameUpdated);
        Assert.IsTrue(fromNonPqEmptyAoli.IsEmpty);
        Assert.IsFalse(fromNonPqEmptyAoli.HasUpdates);
    }

    [TestMethod]
    public void NewTli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newCpoli = new PQCounterPartyOrderLayerInfo(populatedCpoli, new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand));

        Assert.AreEqual(OrderId, newCpoli.OrderId);
        Assert.AreEqual(OrderFlags, newCpoli.OrderFlags);
        Assert.AreEqual(CreatedTime, newCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, newCpoli.UpdatedTime);
        Assert.AreEqual(OrderVolume, newCpoli.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, newCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, newCpoli.CounterPartyName);
        Assert.AreEqual(TraderName, newCpoli.TraderName);
        Assert.IsNotNull(newCpoli.NameIdLookup);
        Assert.IsTrue(newCpoli.IsTraderNameUpdated);
        Assert.IsTrue(newCpoli.IsOrderIdUpdated);
        Assert.IsTrue(newCpoli.IsOrderFlagsUpdated);
        Assert.IsTrue(newCpoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(newCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(newCpoli.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(newCpoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(newCpoli.IsOrderVolumeUpdated);
        Assert.IsTrue(newCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(newCpoli.IsCounterPartyNameUpdated);
        Assert.IsTrue(newCpoli.IsTraderNameUpdated);
        Assert.IsFalse(newCpoli.IsEmpty);
        Assert.IsTrue(newCpoli.HasUpdates);

        var fromEmptyAoli = new PQCounterPartyOrderLayerInfo(emptyCpoli, new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand));
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);
        Assert.IsNotNull(fromEmptyAoli.NameIdLookup);
        Assert.IsNull(fromEmptyAoli.CounterPartyName);
        Assert.IsNull(fromEmptyAoli.TraderName);
        Assert.IsFalse(fromEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCounterPartyNameUpdated);
        Assert.IsFalse(fromEmptyAoli.IsTraderNameUpdated);
        Assert.IsTrue(fromEmptyAoli.IsEmpty);
        Assert.IsFalse(fromEmptyAoli.HasUpdates);

        var nonPqInstance = new CounterPartyOrderLayerInfo(populatedCpoli);
        var fromNonPQInstance
            = new PQCounterPartyOrderLayerInfo(nonPqInstance, new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand));
        Assert.AreEqual(populatedCpoli.OrderId, fromNonPQInstance.OrderId);
        Assert.AreEqual(populatedCpoli.OrderFlags, fromNonPQInstance.OrderFlags);
        Assert.AreEqual(populatedCpoli.CreatedTime, fromNonPQInstance.CreatedTime);
        Assert.AreEqual(populatedCpoli.UpdatedTime, fromNonPQInstance.UpdatedTime);
        Assert.AreEqual(populatedCpoli.OrderVolume, fromNonPQInstance.OrderVolume);
        Assert.AreEqual(populatedCpoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.AreEqual(populatedCpoli.CounterPartyName, fromNonPQInstance.CounterPartyName);
        Assert.AreEqual(populatedCpoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.IsNotNull(fromNonPQInstance.NameIdLookup);
        Assert.IsTrue(fromNonPQInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderFlagsUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCounterPartyNameUpdated);
        Assert.IsTrue(fromNonPQInstance.IsTraderNameUpdated);
        Assert.IsFalse(fromNonPQInstance.IsEmpty);
        Assert.IsTrue(fromNonPQInstance.HasUpdates);

        var newNonPqEmptyAoli = new CounterPartyOrderLayerInfo(emptyCpoli);
        var fromNonPqEmptyAoli
            = new PQCounterPartyOrderLayerInfo(newNonPqEmptyAoli, new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand));
        Assert.AreEqual(0, fromNonPqEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromNonPqEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderRemainingVolume);
        Assert.IsNull(fromNonPqEmptyAoli.CounterPartyName);
        Assert.IsNull(fromNonPqEmptyAoli.TraderName);
        Assert.IsNotNull(fromNonPqEmptyAoli.NameIdLookup);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCounterPartyNameUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsTraderNameUpdated);
        Assert.IsTrue(fromNonPqEmptyAoli.IsEmpty);
        Assert.IsFalse(fromNonPqEmptyAoli.HasUpdates);
    }

    [TestMethod]
    public void NewTli_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedAoli = new PQCounterPartyOrderLayerInfo(emptyNameIdLookup, OrderId, OrderFlags, CreatedTime, OrderVolume,
                                                                UpdatedTime, OrderRemainingVolume, CounterPartyName, TraderName)
        {
            IsOrderIdUpdated         = false, IsOrderFlagsUpdated = false, IsCreatedTimeDateUpdated = false, IsCreatedTimeSub2MinUpdated = false
          , IsUpdatedTimeDateUpdated = false, IsUpdatedTimeSub2MinUpdated = false, IsOrderVolumeUpdated = false, IsOrderRemainingVolumeUpdated = false
          , IsTraderNameUpdated      = false
        };
        var fromPQInstance
            = new PQCounterPartyOrderLayerInfo(newPopulatedAoli, new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand));
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPQInstance.CounterPartyName);
        Assert.AreEqual(TraderName, fromPQInstance.TraderName);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsTraderNameUpdated);
        Assert.IsTrue(fromPQInstance.IsCounterPartyNameUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsCounterPartyNameUpdated = false;
        newPopulatedAoli.IsTraderNameUpdated       = true;
        fromPQInstance = new PQCounterPartyOrderLayerInfo(newPopulatedAoli
                                                        , new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand));
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, fromPQInstance.CounterPartyName);
        Assert.AreEqual(TraderName, fromPQInstance.TraderName);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsCounterPartyNameUpdated);
        Assert.IsTrue(fromPQInstance.IsTraderNameUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);
    }

    [TestMethod]
    public void EmptyCpoli_CounterPartyNameChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqCpoli = new PQCounterPartyOrderLayerInfo();
        Assert.IsTrue(newEmptyPqCpoli.IsEmpty);
        Assert.IsFalse(newEmptyPqCpoli.HasUpdates);

        Assert.AreEqual(emptyCpoli, newEmptyPqCpoli);

        var expectedCounterPartyName = "SetCounterPartyNameToThis";
        emptyCpoli.CounterPartyName = expectedCounterPartyName;

        Assert.AreEqual(expectedCounterPartyName, emptyCpoli.CounterPartyName);
        Assert.AreEqual(1, emptyCpoli.OrderCounterPartyNameId);
        Assert.IsTrue(emptyCpoli.IsCounterPartyNameUpdated);
        Assert.AreEqual(1, emptyCpoli.NameIdLookup.Count);
        Assert.IsFalse(emptyCpoli.IsEmpty);
        Assert.IsTrue(emptyCpoli.HasUpdates);

        Assert.AreNotEqual(emptyCpoli, newEmptyPqCpoli);

        newEmptyPqCpoli.CounterPartyName = expectedCounterPartyName;

        Assert.AreEqual(expectedCounterPartyName, newEmptyPqCpoli.CounterPartyName);
        Assert.AreEqual(1, newEmptyPqCpoli.OrderCounterPartyNameId);
        Assert.IsTrue(newEmptyPqCpoli.IsCounterPartyNameUpdated);
        Assert.AreEqual(1, newEmptyPqCpoli.NameIdLookup.Count);
        Assert.IsFalse(newEmptyPqCpoli.IsEmpty);
        Assert.IsTrue(newEmptyPqCpoli.HasUpdates);

        Assert.AreEqual(emptyCpoli, newEmptyPqCpoli);
    }

    [TestMethod]
    public void EmptyPqCpoli_TradeNameChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqCpoli = new PQCounterPartyOrderLayerInfo();
        Assert.IsTrue(newEmptyPqCpoli.IsEmpty);
        Assert.IsFalse(newEmptyPqCpoli.HasUpdates);

        Assert.AreEqual(emptyCpoli, newEmptyPqCpoli);

        var expectedTraderName = "SetTraderNameToThis";
        emptyCpoli.TraderName = expectedTraderName;

        Assert.AreEqual(expectedTraderName, emptyCpoli.TraderName);
        Assert.AreEqual(1, emptyCpoli.OrderTraderNameId);
        Assert.IsTrue(emptyCpoli.IsTraderNameUpdated);
        Assert.AreEqual(1, emptyCpoli.NameIdLookup.Count);
        Assert.IsFalse(emptyCpoli.IsEmpty);
        Assert.IsTrue(emptyCpoli.HasUpdates);

        Assert.AreNotEqual(emptyCpoli, newEmptyPqCpoli);

        newEmptyPqCpoli.TraderName = expectedTraderName;

        Assert.AreEqual(expectedTraderName, newEmptyPqCpoli.TraderName);
        Assert.AreEqual(1, newEmptyPqCpoli.OrderTraderNameId);
        Assert.IsTrue(newEmptyPqCpoli.IsTraderNameUpdated);
        Assert.AreEqual(1, newEmptyPqCpoli.NameIdLookup.Count);
        Assert.IsFalse(newEmptyPqCpoli.IsEmpty);
        Assert.IsTrue(newEmptyPqCpoli.HasUpdates);

        Assert.AreEqual(emptyCpoli, newEmptyPqCpoli);
    }


    [TestMethod]
    public void EmptyAndPopulatedTli_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedCpoli.IsEmpty);
        Assert.IsTrue(emptyCpoli.IsEmpty);
    }

    [TestMethod]
    public void PopulatedTli_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.AreEqual(OrderId, populatedCpoli.OrderId);
        Assert.AreEqual(OrderFlags, populatedCpoli.OrderFlags);
        Assert.AreEqual(CreatedTime, populatedCpoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, populatedCpoli.UpdatedTime);
        Assert.AreEqual(OrderVolume, populatedCpoli.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, populatedCpoli.OrderRemainingVolume);
        Assert.AreEqual(CounterPartyName, populatedCpoli.CounterPartyName);
        Assert.AreEqual(TraderName, populatedCpoli.TraderName);
        Assert.IsTrue(populatedCpoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderFlagsUpdated);
        Assert.IsTrue(populatedCpoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedCpoli.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(populatedCpoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(populatedCpoli.IsTraderNameUpdated);
        Assert.IsTrue(populatedCpoli.IsCounterPartyNameUpdated);
        Assert.IsFalse(populatedCpoli.IsEmpty);
        Assert.IsTrue(populatedCpoli.HasUpdates);
        populatedCpoli.IsEmpty = true;
        Assert.AreEqual(0, populatedCpoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, populatedCpoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedCpoli.UpdatedTime);
        Assert.AreEqual(0m, populatedCpoli.OrderVolume);
        Assert.AreEqual(0m, populatedCpoli.OrderRemainingVolume);
        Assert.IsNull(populatedCpoli.CounterPartyName);
        Assert.IsNull(populatedCpoli.TraderName);
        Assert.IsTrue(populatedCpoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderFlagsUpdated);
        Assert.IsTrue(populatedCpoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedCpoli.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(populatedCpoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(populatedCpoli.IsTraderNameUpdated);
        Assert.IsTrue(populatedCpoli.IsCounterPartyNameUpdated);
        Assert.IsTrue(populatedCpoli.IsEmpty);
        Assert.IsTrue(populatedCpoli.HasUpdates);
        populatedCpoli.StateReset();
        Assert.IsFalse(populatedCpoli.IsOrderIdUpdated);
        Assert.IsFalse(populatedCpoli.IsOrderFlagsUpdated);
        Assert.IsFalse(populatedCpoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(populatedCpoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(populatedCpoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(populatedCpoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(populatedCpoli.IsOrderVolumeUpdated);
        Assert.IsFalse(populatedCpoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(populatedCpoli.IsTraderNameUpdated);
        Assert.IsFalse(populatedCpoli.IsCounterPartyNameUpdated);
        Assert.IsTrue(populatedCpoli.IsEmpty);
        Assert.IsFalse(populatedCpoli.HasUpdates);
    }

    [TestMethod]
    public void FullyPopulatedTli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQTraderLayerInfo = new CounterPartyOrderLayerInfo(populatedCpoli);
        emptyCpoli.CopyFrom(nonPQTraderLayerInfo);
        Assert.AreEqual(populatedCpoli, emptyCpoli);
    }

    [TestMethod]
    public void FullyPopulatedTli_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IAnonymousOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(clonedQuote, populatedCpoli);
        Assert.AreEqual(populatedCpoli, clonedQuote);

        var cloned2 = (PQAnonymousOrderLayerInfo)((ICloneable)populatedCpoli).Clone();
        Assert.AreNotSame(cloned2, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned2);

        var cloned3 = (PQAnonymousOrderLayerInfo)((IMutableAnonymousOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(cloned3, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned3);

        var cloned4 = (PQAnonymousOrderLayerInfo)((ICounterPartyOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(cloned4, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned4);

        var cloned5 = (PQAnonymousOrderLayerInfo)((IMutableCounterPartyOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(cloned5, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned5);

        var cloned6 = (PQAnonymousOrderLayerInfo)((PQCounterPartyOrderLayerInfo)populatedCpoli).Clone();
        Assert.AreNotSame(cloned6, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned6);

        var cloned7 = populatedCpoli.Clone();
        Assert.AreNotSame(cloned7, populatedCpoli);
        Assert.AreEqual(populatedCpoli, cloned7);
    }

    [TestMethod]
    public void FullyPopulatedPqCpoliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQCounterPartyOrderLayerInfo)((ICloneable)populatedCpoli).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedCpoli, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedCpoli, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedTliSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedCpoli, populatedCpoli);
        Assert.AreEqual(populatedCpoli, ((ICloneable)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IAnonymousOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((ICloneable<IAnonymousOrderLayerInfo>)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IMutableAnonymousOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((ICounterPartyOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((IMutableCounterPartyOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, ((PQCounterPartyOrderLayerInfo)populatedCpoli).Clone());
        Assert.AreEqual(populatedCpoli, populatedCpoli.Clone());
    }

    [TestMethod]
    public void FullyPopulatedTli_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedCpoli.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedTli_ToString_ReturnsNameAndValues()
    {
        var toString = populatedCpoli.ToString()!;

        Assert.IsTrue(toString.Contains(populatedCpoli.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderId)}: {populatedCpoli.OrderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderFlags)}: {populatedCpoli.OrderFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.CreatedTime)}: {populatedCpoli.CreatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.UpdatedTime)}: {populatedCpoli.UpdatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderVolume)}: {populatedCpoli.OrderVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.OrderRemainingVolume)}: {populatedCpoli.OrderRemainingVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.TraderName)}: {populatedCpoli.TraderName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedCpoli.CounterPartyName)}: {populatedCpoli.CounterPartyName}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQCounterPartyOrderLayerInfo? original,
        IPQCounterPartyOrderLayerInfo? changingTraderLayerInfo,
        IOrdersPriceVolumeLayer? originalTraderPriceVolumeLayer = null,
        IOrdersPriceVolumeLayer? changingTraderPriceVolumeLayer = null,
        IOrderBookSide? originalOrderBook = null,
        IOrderBookSide? changingOrderBook = null,
        ILevel2Quote? originalQuote = null,
        ILevel2Quote? changingQuote = null)
    {
        if (original == null && changingTraderLayerInfo == null) return;
        Assert.IsNotNull(original);
        Assert.IsNotNull(changingTraderLayerInfo);

        if (original.GetType() == typeof(PQCounterPartyOrderLayerInfo))
            Assert.AreEqual
                (!exactComparison,
                 original.AreEquivalent((IAnonymousOrderLayerInfo)new CounterPartyOrderLayerInfo(changingTraderLayerInfo), exactComparison));

        PQAnonymousOrderLayerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingTraderLayerInfo, originalTraderPriceVolumeLayer
           , changingTraderPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingTraderLayerInfo.IsCounterPartyNameUpdated = !changingTraderLayerInfo.IsCounterPartyNameUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsCounterPartyNameUpdated = original.IsCounterPartyNameUpdated;
        Assert.IsTrue(original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsTraderNameUpdated = !changingTraderLayerInfo.IsTraderNameUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsTraderNameUpdated = original.IsTraderNameUpdated;
        Assert.IsTrue(original.AreEquivalent((IAnonymousOrderLayerInfo)changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
