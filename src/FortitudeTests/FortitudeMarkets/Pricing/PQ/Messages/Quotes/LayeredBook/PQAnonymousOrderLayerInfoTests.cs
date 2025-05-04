// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeTests.FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

[TestClass]
public class PQAnonymousOrderLayerInfoTests
{
    private const LayerOrderFlags OrderFlags = LayerOrderFlags.CreatedFromSource | LayerOrderFlags.IsInternallyCreatedOrder;

    private const int     OrderId              = 80085;
    private const decimal OrderVolume          = 100_000.50m;
    private const decimal OrderRemainingVolume = 50_000.25m;

    private readonly DateTime CreatedTime = new DateTime(2025, 4, 21, 6, 27, 23).AddMilliseconds(123).AddMicroseconds(456);
    private readonly DateTime UpdatedTime = new DateTime(2025, 4, 21, 12, 8, 59).AddMilliseconds(789).AddMicroseconds(213);

    private IPQAnonymousOrderLayerInfo emptyAoli = null!;

    private IPQNameIdLookupGenerator   emptyNameIdLookup = null!;
    private IPQNameIdLookupGenerator   nameIdLookup      = null!;
    private IPQAnonymousOrderLayerInfo populatedAoli     = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        nameIdLookup      = new PQNameIdLookupGenerator(PQQuoteFields.LayerNameDictionaryUpsertCommand);
        emptyAoli         = new PQAnonymousOrderLayerInfo();
        populatedAoli     = new PQAnonymousOrderLayerInfo(OrderId, OrderFlags, CreatedTime, OrderVolume, UpdatedTime, OrderRemainingVolume);
    }

    [TestMethod]
    public void NewPqAoli_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newAoli = new PQAnonymousOrderLayerInfo(OrderId, OrderFlags, CreatedTime, OrderVolume);
        Assert.AreEqual(OrderId, newAoli.OrderId);
        Assert.AreEqual(OrderFlags, newAoli.OrderFlags);
        Assert.AreEqual(CreatedTime, newAoli.CreatedTime);
        Assert.AreEqual(CreatedTime, newAoli.UpdatedTime);
        Assert.AreEqual(OrderVolume, newAoli.OrderVolume);
        Assert.AreEqual(OrderVolume, newAoli.OrderRemainingVolume);

        Assert.AreEqual(0, emptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, emptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, emptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, emptyAoli.UpdatedTime);
        Assert.AreEqual(0m, emptyAoli.OrderVolume);
        Assert.AreEqual(0m, emptyAoli.OrderRemainingVolume);
    }

    [TestMethod]
    public void NewPqAoli_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var fromInstance = new PQAnonymousOrderLayerInfo(populatedAoli);
        Assert.AreEqual(populatedAoli.OrderId, fromInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderFlags, fromInstance.OrderFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdatedTime, fromInstance.UpdatedTime);
        Assert.AreEqual(populatedAoli.OrderVolume, fromInstance.OrderVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromInstance.OrderRemainingVolume);
        Assert.IsTrue(fromInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromInstance.IsOrderFlagsUpdated);
        Assert.IsTrue(fromInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromInstance.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(fromInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(fromInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromInstance.IsEmpty);
        Assert.IsTrue(fromInstance.HasUpdates);

        var fromEmptyAoli = new PQAnonymousOrderLayerInfo(emptyAoli);
        Assert.AreEqual(0, fromEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromEmptyAoli.OrderRemainingVolume);
        Assert.IsFalse(fromEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromEmptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromEmptyAoli.IsEmpty);
        Assert.IsFalse(fromEmptyAoli.HasUpdates);

        var nonPqInstance     = new AnonymousOrderLayerInfo(populatedAoli);
        var fromNonPQInstance = new PQAnonymousOrderLayerInfo(nonPqInstance);
        Assert.AreEqual(populatedAoli.OrderId, fromNonPQInstance.OrderId);
        Assert.AreEqual(populatedAoli.OrderFlags, fromNonPQInstance.OrderFlags);
        Assert.AreEqual(populatedAoli.CreatedTime, fromNonPQInstance.CreatedTime);
        Assert.AreEqual(populatedAoli.UpdatedTime, fromNonPQInstance.UpdatedTime);
        Assert.AreEqual(populatedAoli.OrderVolume, fromNonPQInstance.OrderVolume);
        Assert.AreEqual(populatedAoli.OrderRemainingVolume, fromNonPQInstance.OrderRemainingVolume);
        Assert.IsTrue(fromNonPQInstance.IsOrderIdUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderFlagsUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(fromNonPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromNonPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromNonPQInstance.IsEmpty);
        Assert.IsTrue(fromNonPQInstance.HasUpdates);

        var newNonPqEmptyAoli  = new AnonymousOrderLayerInfo(emptyAoli);
        var fromNonPqEmptyAoli = new PQAnonymousOrderLayerInfo(newNonPqEmptyAoli);
        fromNonPqEmptyAoli.HasUpdates = false;
        Assert.AreEqual(0, fromNonPqEmptyAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, fromNonPqEmptyAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, fromNonPqEmptyAoli.UpdatedTime);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderVolume);
        Assert.AreEqual(0m, fromNonPqEmptyAoli.OrderRemainingVolume);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(fromNonPqEmptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromNonPqEmptyAoli.IsEmpty);
        Assert.IsFalse(fromNonPqEmptyAoli.HasUpdates);
    }

    [TestMethod]
    public void NewPqAoli_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedAoli = new PQAnonymousOrderLayerInfo(OrderId, OrderFlags, CreatedTime, OrderVolume, UpdatedTime, OrderRemainingVolume)
        {
            IsOrderFlagsUpdated = false, IsCreatedTimeDateUpdated = false, IsCreatedTimeSub2MinUpdated = false, IsUpdatedTimeDateUpdated = false
          , IsUpdatedTimeSub2MinUpdated = false, IsOrderVolumeUpdated = false, IsOrderRemainingVolumeUpdated = false
        };
        var fromPQInstance = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsOrderIdUpdated    = false;
        newPopulatedAoli.IsOrderFlagsUpdated = true;
        fromPQInstance                       = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsOrderFlagsUpdated      = false;
        newPopulatedAoli.IsCreatedTimeDateUpdated = true;
        fromPQInstance                            = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsCreatedTimeDateUpdated    = false;
        newPopulatedAoli.IsCreatedTimeSub2MinUpdated = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsCreatedTimeSub2MinUpdated = false;
        newPopulatedAoli.IsUpdatedTimeDateUpdated    = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsUpdatedTimeDateUpdated    = false;
        newPopulatedAoli.IsUpdatedTimeSub2MinUpdated = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsUpdatedTimeSub2MinUpdated = false;
        newPopulatedAoli.IsOrderVolumeUpdated        = true;
        fromPQInstance                               = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);

        newPopulatedAoli.IsOrderVolumeUpdated          = false;
        newPopulatedAoli.IsOrderRemainingVolumeUpdated = true;
        fromPQInstance                                 = new PQAnonymousOrderLayerInfo(newPopulatedAoli);
        Assert.AreEqual(OrderId, fromPQInstance.OrderId);
        Assert.AreEqual(OrderFlags, fromPQInstance.OrderFlags);
        Assert.AreEqual(CreatedTime, fromPQInstance.CreatedTime);
        Assert.AreEqual(UpdatedTime, fromPQInstance.UpdatedTime);
        Assert.AreEqual(OrderVolume, fromPQInstance.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, fromPQInstance.OrderRemainingVolume);
        Assert.IsFalse(fromPQInstance.IsOrderIdUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderFlagsUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsOrderVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsEmpty);
        Assert.IsTrue(fromPQInstance.HasUpdates);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderIdChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.IsTrue(newEmptyPqAoli.IsEmpty);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedOrderId = 101101;
        emptyAoli.OrderId = changedOrderId;

        Assert.AreEqual(changedOrderId, emptyAoli.OrderId);
        Assert.IsTrue(emptyAoli.IsOrderIdUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderId = changedOrderId;

        Assert.AreEqual(changedOrderId, newEmptyPqAoli.OrderId);
        Assert.IsTrue(newEmptyPqAoli.IsOrderIdUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderFlagsChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.IsTrue(newEmptyPqAoli.IsEmpty);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedOrderFlags = LayerOrderFlags.CreatedFromSimulator | LayerOrderFlags.IsSyntheticTrackingOrder | LayerOrderFlags.NotExternalVolume;
        emptyAoli.OrderFlags = changedOrderFlags;

        Assert.AreEqual(changedOrderFlags, emptyAoli.OrderFlags);
        Assert.IsTrue(emptyAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderFlags = changedOrderFlags;

        Assert.AreEqual(changedOrderFlags, newEmptyPqAoli.OrderFlags);
        Assert.IsTrue(newEmptyPqAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderCreatedTimeChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo
        {
            CreatedTime = DateTime.UnixEpoch
        };
        newEmptyPqAoli.UpdateComplete();
        emptyAoli.CreatedTime = DateTime.UnixEpoch;
        emptyAoli.UpdateComplete();

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
        Assert.IsFalse(emptyAoli.HasUpdates);

        var changedCreatedSubHour = DateTime.UnixEpoch.AddMinutes(1);
        emptyAoli.CreatedTime = changedCreatedSubHour;

        Assert.AreEqual(changedCreatedSubHour, emptyAoli.CreatedTime);
        Assert.IsTrue(emptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(emptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.CreatedTime = changedCreatedSubHour;

        Assert.AreEqual(changedCreatedSubHour, newEmptyPqAoli.CreatedTime);
        Assert.IsTrue(newEmptyPqAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        newEmptyPqAoli.CreatedTime = DateTime.UnixEpoch;
        newEmptyPqAoli.HasUpdates  = false;
        emptyAoli.CreatedTime      = DateTime.UnixEpoch;
        emptyAoli.HasUpdates       = false;
        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedCreatedDate = DateTime.UnixEpoch.AddHours(1);
        emptyAoli.CreatedTime = changedCreatedDate;

        Assert.AreEqual(changedCreatedDate, emptyAoli.CreatedTime);
        Assert.IsFalse(emptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.CreatedTime = changedCreatedDate;

        Assert.AreEqual(changedCreatedDate, newEmptyPqAoli.CreatedTime);
        Assert.IsFalse(newEmptyPqAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        emptyAoli             = new PQAnonymousOrderLayerInfo();
        emptyAoli.CreatedTime = DateTime.UnixEpoch;
        emptyAoli.HasUpdates  = false;

        newEmptyPqAoli = new PQAnonymousOrderLayerInfo
        {
            CreatedTime = DateTime.UnixEpoch, HasUpdates = false
        };
        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedCreatedDateAndHour = DateTime.UnixEpoch.AddMinutes(90);
        emptyAoli.CreatedTime = changedCreatedDateAndHour;

        Assert.AreEqual(changedCreatedDateAndHour, emptyAoli.CreatedTime);
        Assert.IsTrue(emptyAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.CreatedTime = changedCreatedDateAndHour;

        Assert.AreEqual(changedCreatedDateAndHour, newEmptyPqAoli.CreatedTime);
        Assert.IsTrue(newEmptyPqAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderUpdatedTimeChanged_ExpectedPropertiesUpdated()
    {
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo
        {
            UpdatedTime = DateTime.UnixEpoch
        };
        newEmptyPqAoli.UpdateComplete();
        emptyAoli.UpdatedTime = DateTime.UnixEpoch;
        emptyAoli.UpdateComplete();

        Assert.IsFalse(emptyAoli.HasUpdates);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);
        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedUpdatedSubHour = DateTime.UnixEpoch.AddMinutes(1);
        emptyAoli.UpdatedTime = changedUpdatedSubHour;

        Assert.AreEqual(changedUpdatedSubHour, emptyAoli.UpdatedTime);
        Assert.IsTrue(emptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(emptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.UpdatedTime = changedUpdatedSubHour;

        Assert.AreEqual(changedUpdatedSubHour, newEmptyPqAoli.UpdatedTime);
        Assert.IsTrue(newEmptyPqAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
        newEmptyPqAoli.UpdatedTime = DateTime.UnixEpoch;
        newEmptyPqAoli.HasUpdates  = false;
        emptyAoli.UpdatedTime      = DateTime.UnixEpoch;
        emptyAoli.HasUpdates       = false;

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedUpdatedHour = DateTime.UnixEpoch.AddHours(2);
        emptyAoli.UpdatedTime = changedUpdatedHour;

        Assert.AreEqual(changedUpdatedHour, emptyAoli.UpdatedTime);
        Assert.IsFalse(emptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.UpdatedTime = changedUpdatedHour;

        Assert.AreEqual(changedUpdatedHour, newEmptyPqAoli.UpdatedTime);
        Assert.IsFalse(newEmptyPqAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
        newEmptyPqAoli.UpdatedTime = DateTime.UnixEpoch;
        newEmptyPqAoli.HasUpdates  = false;
        emptyAoli.UpdatedTime      = DateTime.UnixEpoch;
        emptyAoli.HasUpdates       = false;

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedUpdatedDateAndHour = DateTime.UnixEpoch.AddMinutes(3);
        emptyAoli.UpdatedTime = changedUpdatedDateAndHour;

        Assert.AreEqual(changedUpdatedDateAndHour, emptyAoli.UpdatedTime);
        Assert.IsTrue(emptyAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(emptyAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.UpdatedTime = changedUpdatedDateAndHour;

        Assert.AreEqual(changedUpdatedDateAndHour, newEmptyPqAoli.UpdatedTime);
        Assert.IsTrue(newEmptyPqAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderVolumeChanged_ExpectedPropertiesUpdated()
    {
        Assert.AreEqual(0m, emptyAoli.OrderVolume);
        Assert.IsFalse(emptyAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(emptyAoli.IsEmpty);
        Assert.IsFalse(emptyAoli.HasUpdates);
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.AreEqual(0m, newEmptyPqAoli.OrderVolume);
        Assert.IsFalse(newEmptyPqAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsEmpty);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedOrderVolume = 4_294_967_280;
        emptyAoli.OrderVolume = changedOrderVolume;

        Assert.AreEqual(changedOrderVolume, emptyAoli.OrderVolume);
        Assert.IsTrue(emptyAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderVolume = changedOrderVolume;

        Assert.AreEqual(changedOrderVolume, newEmptyPqAoli.OrderVolume);
        Assert.IsTrue(newEmptyPqAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyPqAoli_OrderRemainingVolumeChanged_ExpectedPropertiesUpdated()
    {
        Assert.AreEqual(0m, emptyAoli.OrderVolume);
        Assert.IsFalse(emptyAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(emptyAoli.IsEmpty);
        Assert.IsFalse(emptyAoli.HasUpdates);
        var newEmptyPqAoli = new PQAnonymousOrderLayerInfo();
        Assert.AreEqual(0m, newEmptyPqAoli.OrderVolume);
        Assert.IsFalse(newEmptyPqAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(newEmptyPqAoli.IsEmpty);
        Assert.IsFalse(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);

        var changedOrderRemainingVolume = 2_294_967_280;
        emptyAoli.OrderRemainingVolume = changedOrderRemainingVolume;

        Assert.AreEqual(changedOrderRemainingVolume, emptyAoli.OrderRemainingVolume);
        Assert.IsTrue(emptyAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(emptyAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.HasUpdates);

        Assert.AreNotEqual(emptyAoli, newEmptyPqAoli);

        newEmptyPqAoli.OrderRemainingVolume = changedOrderRemainingVolume;

        Assert.AreEqual(changedOrderRemainingVolume, newEmptyPqAoli.OrderRemainingVolume);
        Assert.IsTrue(newEmptyPqAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(newEmptyPqAoli.IsEmpty);
        Assert.IsTrue(newEmptyPqAoli.HasUpdates);

        Assert.AreEqual(emptyAoli, newEmptyPqAoli);
    }

    [TestMethod]
    public void EmptyAndPopulatedPqAoli_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedAoli.IsEmpty);
        Assert.IsTrue(emptyAoli.IsEmpty);
    }

    [TestMethod]
    public void PopulatedPqAoli_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.AreEqual(OrderId, populatedAoli.OrderId);
        Assert.AreEqual(OrderFlags, populatedAoli.OrderFlags);
        Assert.AreEqual(CreatedTime, populatedAoli.CreatedTime);
        Assert.AreEqual(UpdatedTime, populatedAoli.UpdatedTime);
        Assert.AreEqual(OrderVolume, populatedAoli.OrderVolume);
        Assert.AreEqual(OrderRemainingVolume, populatedAoli.OrderRemainingVolume);
        Assert.IsTrue(populatedAoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedAoli.IsOrderFlagsUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsFalse(populatedAoli.IsEmpty);
        Assert.IsTrue(populatedAoli.HasUpdates);
        populatedAoli.IsEmpty = true;
        Assert.AreEqual(0, populatedAoli.OrderId);
        Assert.AreEqual(LayerOrderFlags.None, populatedAoli.OrderFlags);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.CreatedTime);
        Assert.AreEqual(DateTime.MinValue, populatedAoli.UpdatedTime);
        Assert.AreEqual(0m, populatedAoli.OrderVolume);
        Assert.AreEqual(0m, populatedAoli.OrderRemainingVolume);
        Assert.IsTrue(populatedAoli.IsOrderIdUpdated);
        Assert.IsTrue(populatedAoli.IsOrderFlagsUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsUpdatedTimeDateUpdated);
        Assert.IsTrue(populatedAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsTrue(populatedAoli.IsOrderVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsEmpty);
        Assert.IsTrue(populatedAoli.HasUpdates);
        populatedAoli.StateReset();
        Assert.IsFalse(populatedAoli.IsOrderIdUpdated);
        Assert.IsFalse(populatedAoli.IsOrderFlagsUpdated);
        Assert.IsFalse(populatedAoli.IsCreatedTimeDateUpdated);
        Assert.IsFalse(populatedAoli.IsCreatedTimeSub2MinUpdated);
        Assert.IsFalse(populatedAoli.IsUpdatedTimeDateUpdated);
        Assert.IsFalse(populatedAoli.IsUpdatedTimeSub2MinUpdated);
        Assert.IsFalse(populatedAoli.IsOrderVolumeUpdated);
        Assert.IsFalse(populatedAoli.IsOrderRemainingVolumeUpdated);
        Assert.IsTrue(populatedAoli.IsEmpty);
        Assert.IsFalse(populatedAoli.HasUpdates);
    }

    [TestMethod]
    public void FullyPopulatedAoli_CopyFromNonPQToEmptyQuote_PvlsEqualEachOther()
    {
        var nonPQTraderLayerInfo = new AnonymousOrderLayerInfo(populatedAoli);
        emptyAoli.CopyFrom(nonPQTraderLayerInfo);
        Assert.AreEqual(populatedAoli, emptyAoli);
    }

    [TestMethod]
    public void FullyPopulatedPqAoli_Clone_ClonedInstanceEqualsOriginal()
    {
        var clonedQuote = ((IMutableAnonymousOrderLayerInfo)populatedAoli).Clone();
        Assert.AreNotSame(clonedQuote, populatedAoli);
        Assert.AreEqual(populatedAoli, clonedQuote);

        var cloned2 = (PQAnonymousOrderLayerInfo)((ICloneable)populatedAoli).Clone();
        Assert.AreNotSame(cloned2, populatedAoli);
        Assert.AreEqual(populatedAoli, cloned2);

        var cloned3 = (PQAnonymousOrderLayerInfo)populatedAoli.Clone();
        Assert.AreNotSame(cloned3, populatedAoli);
        Assert.AreEqual(populatedAoli, cloned3);

        var cloned4 = ((PQAnonymousOrderLayerInfo)populatedAoli).Clone();
        Assert.AreNotSame(cloned4, populatedAoli);
        Assert.AreEqual(populatedAoli, cloned4);
    }

    [TestMethod]
    public void FullyPopulatedPqAoliCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQAnonymousOrderLayerInfo)((ICloneable)populatedAoli).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (true, populatedAoli, fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType
            (false, populatedAoli, fullyPopulatedClone);
    }

    [TestMethod]
    public void FullyPopulatedPqAoliSameObj_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedAoli, populatedAoli);
        Assert.AreEqual(populatedAoli, ((ICloneable)populatedAoli).Clone());
        Assert.AreEqual(populatedAoli, ((IMutableAnonymousOrderLayerInfo)populatedAoli).Clone());
        Assert.AreEqual(populatedAoli, ((ICloneable<IAnonymousOrderLayerInfo>)populatedAoli).Clone());
        Assert.AreEqual(populatedAoli, ((PQAnonymousOrderLayerInfo)populatedAoli).Clone());
        Assert.AreEqual(populatedAoli, populatedAoli.Clone());
    }

    [TestMethod]
    public void FullyPopulatedTli_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedAoli.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedTli_ToString_ReturnsNameAndValues()
    {
        var toString = populatedAoli.ToString()!;

        Assert.IsTrue(toString.Contains(populatedAoli.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderId)}: {populatedAoli.OrderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderFlags)}: {populatedAoli.OrderFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.CreatedTime)}: {populatedAoli.CreatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.UpdatedTime)}: {populatedAoli.UpdatedTime}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderVolume)}: {populatedAoli.OrderVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedAoli.OrderRemainingVolume)}: {populatedAoli.OrderRemainingVolume:N2}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        IPQAnonymousOrderLayerInfo? original,
        IPQAnonymousOrderLayerInfo? changingTraderLayerInfo,
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

        if (original.GetType() == typeof(PQAnonymousOrderLayerInfo))
            Assert.AreEqual(!exactComparison, original.AreEquivalent(
                                                                     new AnonymousOrderLayerInfo(changingTraderLayerInfo), exactComparison));

        AnonymousOrderLayerInfoTests.AssertAreEquivalentMeetsExpectedExactComparisonType
            (exactComparison, original, changingTraderLayerInfo, originalTraderPriceVolumeLayer
           , changingTraderPriceVolumeLayer, originalOrderBook, changingOrderBook, originalQuote, changingQuote);

        changingTraderLayerInfo.IsOrderIdUpdated = !changingTraderLayerInfo.IsOrderIdUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsOrderIdUpdated = original.IsOrderIdUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsOrderFlagsUpdated = !changingTraderLayerInfo.IsOrderFlagsUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsOrderFlagsUpdated = original.IsOrderFlagsUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsCreatedTimeDateUpdated = !changingTraderLayerInfo.IsCreatedTimeDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsCreatedTimeDateUpdated = original.IsCreatedTimeDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsCreatedTimeSub2MinUpdated = !changingTraderLayerInfo.IsCreatedTimeSub2MinUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsCreatedTimeSub2MinUpdated = original.IsCreatedTimeSub2MinUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsUpdatedTimeDateUpdated = !changingTraderLayerInfo.IsUpdatedTimeDateUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsUpdatedTimeDateUpdated = original.IsUpdatedTimeDateUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsUpdatedTimeSub2MinUpdated = !changingTraderLayerInfo.IsUpdatedTimeSub2MinUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsUpdatedTimeSub2MinUpdated = original.IsUpdatedTimeSub2MinUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsOrderVolumeUpdated = !changingTraderLayerInfo.IsOrderVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsOrderVolumeUpdated = original.IsOrderVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingTraderLayerInfo.IsOrderRemainingVolumeUpdated = !changingTraderLayerInfo.IsOrderRemainingVolumeUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.AreEqual(!exactComparison,
                            originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.AreEqual(!exactComparison,
                            originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingTraderLayerInfo.IsOrderRemainingVolumeUpdated = original.IsOrderRemainingVolumeUpdated;
        Assert.IsTrue(original.AreEquivalent(changingTraderLayerInfo, exactComparison));
        if (originalTraderPriceVolumeLayer != null)
            Assert.IsTrue(
                          originalTraderPriceVolumeLayer.AreEquivalent(changingTraderPriceVolumeLayer, exactComparison));
        if (originalOrderBook != null)
            Assert.IsTrue(
                          originalOrderBook.AreEquivalent(changingOrderBook, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
