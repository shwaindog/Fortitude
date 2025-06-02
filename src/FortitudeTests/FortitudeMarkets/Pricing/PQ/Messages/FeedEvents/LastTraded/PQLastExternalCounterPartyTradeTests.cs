// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

[TestClass]
public class PQLastExternalCounterPartyTradeTests
{
    private const uint    ExpectedTradeId     = 42;
    private const uint    ExpectedBatchId     = 24_942;
    private const uint    ExpectedOrderId     = 1_772_942;
    private const decimal ExpectedTradePrice  = 2.3456m;
    private const decimal ExpectedTradeVolume = 42_000_111m;

    private const LastTradedTypeFlags      ExpectedTradedTypeFlags     = LastTradedTypeFlags.HasPaidGivenDetails;
    private const LastTradedLifeCycleFlags ExpectedTradeLifeCycleFlags = LastTradedLifeCycleFlags.Confirmed;

    private static readonly DateTime ExpectedTradeTime           = new(2018, 03, 2, 14, 40, 30);
    private static readonly DateTime ExpectedFirstNotifiedTime   = new(2018, 03, 2, 14, 40, 31);
    private static readonly DateTime ExpectedAdapterReceivedTime = new(2018, 03, 2, 14, 40, 41);
    private static readonly DateTime ExpectedUpdateTime          = new(2018, 03, 2, 14, 40, 42);

    private const bool ExpectedWasGiven = true;
    private const bool ExpectedWasPaid  = true;

    private const int ExpectedTraderId       = 34_902;
    private const int ExpectedCounterPartyId = 2_198;

    private const string ExpectedTraderName       = "TraderName-Helen";
    private const string ExpectedCounterPartyName = "CounterPartyName-Valcopp";

    private PQLastExternalCounterPartyTrade emptyLt           = null!;
    private IPQNameIdLookupGenerator        emptyNameIdLookup = null!;
    private IPQNameIdLookupGenerator        nameIdLookup      = null!;
    private PQLastExternalCounterPartyTrade populatedLt       = null!;

    [TestInitialize]
    public void SetUp()
    {
        emptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        nameIdLookup      = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        emptyLt           = new PQLastExternalCounterPartyTrade(emptyNameIdLookup.Clone());
        populatedLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup.Clone(), ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
                {
                    ExternalTraderName = ExpectedTraderName
                };
    }

    [TestMethod]
    public void NewLt_SetsPriceAndVolume_PropertiesInitializedAsExpected()
    {
        var newLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
                {
                    ExternalTraderName = ExpectedTraderName
                };
        Assert.AreEqual(ExpectedTradePrice, newLt.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, newLt.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, newLt.TradeVolume);
        Assert.IsTrue(newLt.WasGiven);
        Assert.IsTrue(newLt.WasPaid);
        Assert.AreEqual(ExpectedTraderName, newLt.ExternalTraderName);
        Assert.IsTrue(newLt.IsTradePriceUpdated);
        Assert.IsTrue(newLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(newLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(newLt.IsTradeVolumeUpdated);
        Assert.IsTrue(newLt.IsWasGivenUpdated);
        Assert.IsTrue(newLt.IsWasPaidUpdated);
        Assert.IsTrue(newLt.IsExternalTraderNameUpdated);

        Assert.AreEqual(0, emptyLt.TradePrice);
        Assert.AreEqual(DateTime.MinValue, emptyLt.TradeTime);
        Assert.AreEqual(0m, emptyLt.TradeVolume);
        Assert.IsFalse(emptyLt.WasGiven);
        Assert.IsFalse(emptyLt.WasPaid);
        Assert.IsNull(emptyLt.ExternalTraderName);
        Assert.IsFalse(emptyLt.IsTradePriceUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(emptyLt.IsTradeVolumeUpdated);
        Assert.IsFalse(emptyLt.IsWasGivenUpdated);
        Assert.IsFalse(emptyLt.IsWasPaidUpdated);
        Assert.IsFalse(emptyLt.IsExternalTraderNameUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_PropertiesInitializedAsExpected()
    {
        var newPopulatedLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup.Clone(), ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
                {
                    ExternalTraderName = ExpectedTraderName
                };
        var fromPQInstance = new PQLastExternalCounterPartyTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromPQInstance.ExternalTraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsExternalTraderNameUpdated);

        var nonPQLt =
            new LastExternalCounterPartyTrade
                (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime);
        var fromNonPqInstance = new PQLastExternalCounterPartyTrade(nonPQLt, emptyNameIdLookup.Clone());
        Assert.AreEqual(ExpectedTradePrice, fromNonPqInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromNonPqInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromNonPqInstance.TradeVolume);
        Assert.IsTrue(fromNonPqInstance.WasGiven);
        Assert.IsTrue(fromNonPqInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromNonPqInstance.ExternalTraderName);
        Assert.IsTrue(fromNonPqInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromNonPqInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromNonPqInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromNonPqInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromNonPqInstance.IsExternalTraderNameUpdated);

        var newEmptyLt = new PQLastExternalCounterPartyTrade(emptyLt, emptyNameIdLookup);
        Assert.AreEqual(0, newEmptyLt.TradePrice);
        Assert.AreEqual(DateTime.MinValue, newEmptyLt.TradeTime);
        Assert.AreEqual(0m, newEmptyLt.TradeVolume);
        Assert.IsFalse(newEmptyLt.WasGiven);
        Assert.IsFalse(newEmptyLt.WasPaid);
        Assert.IsNull(newEmptyLt.ExternalTraderName);
        Assert.IsFalse(newEmptyLt.IsTradePriceUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(newEmptyLt.IsTradeVolumeUpdated);
        Assert.IsFalse(newEmptyLt.IsWasGivenUpdated);
        Assert.IsFalse(newEmptyLt.IsWasPaidUpdated);
        Assert.IsFalse(newEmptyLt.IsExternalTraderNameUpdated);
    }

    [TestMethod]
    public void NewLt_NewFromCloneInstance_WhenOneFieldNonDefaultIsNotUpdatedNewInstanceCopies()
    {
        var newPopulatedLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup.Clone(), ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
                {
                    ExternalTraderName = ExpectedTraderName, IsTradePriceUpdated = false
                };
        var fromPQInstance = new PQLastExternalCounterPartyTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromPQInstance.ExternalTraderName);
        Assert.IsFalse(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsExternalTraderNameUpdated);

        newPopulatedLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup.Clone(), ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
                {
                    ExternalTraderName = ExpectedTraderName, IsTradeTimeDateUpdated = false
                };
        fromPQInstance = new PQLastExternalCounterPartyTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromPQInstance.ExternalTraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsExternalTraderNameUpdated);

        newPopulatedLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
                {
                    ExternalTraderName = ExpectedTraderName, IsTradeTimeSub2MinUpdated = false
                };
        fromPQInstance = new PQLastExternalCounterPartyTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromPQInstance.ExternalTraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsExternalTraderNameUpdated);

        newPopulatedLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
            {
                ExternalTraderName = ExpectedTraderName, IsTradeVolumeUpdated = false
            };
        fromPQInstance = new PQLastExternalCounterPartyTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromPQInstance.ExternalTraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsExternalTraderNameUpdated);

        newPopulatedLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
            {
                ExternalTraderName = ExpectedTraderName, IsWasGivenUpdated = false
            };
        fromPQInstance = new PQLastExternalCounterPartyTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromPQInstance.ExternalTraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsFalse(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsExternalTraderNameUpdated);

        newPopulatedLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
            {
                ExternalTraderName = ExpectedTraderName, IsWasPaidUpdated = false
            };
        fromPQInstance = new PQLastExternalCounterPartyTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromPQInstance.ExternalTraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsFalse(fromPQInstance.IsWasPaidUpdated);
        Assert.IsTrue(fromPQInstance.IsExternalTraderNameUpdated);

        newPopulatedLt =
            new PQLastExternalCounterPartyTrade
                (nameIdLookup, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
            {
                ExternalTraderName = ExpectedTraderName, IsExternalTraderNameUpdated = false
            };
        fromPQInstance = new PQLastExternalCounterPartyTrade(newPopulatedLt, newPopulatedLt.NameIdLookup);
        Assert.AreEqual(ExpectedTradePrice, fromPQInstance.TradePrice);
        Assert.AreEqual(ExpectedTradeTime, fromPQInstance.TradeTime);
        Assert.AreEqual(ExpectedTradeVolume, fromPQInstance.TradeVolume);
        Assert.IsTrue(fromPQInstance.WasGiven);
        Assert.IsTrue(fromPQInstance.WasPaid);
        Assert.AreEqual(ExpectedTraderName, fromPQInstance.ExternalTraderName);
        Assert.IsTrue(fromPQInstance.IsTradePriceUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeDateUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(fromPQInstance.IsTradeVolumeUpdated);
        Assert.IsTrue(fromPQInstance.IsWasGivenUpdated);
        Assert.IsTrue(fromPQInstance.IsWasPaidUpdated);
        Assert.IsFalse(fromPQInstance.IsExternalTraderNameUpdated);
    }

    [TestMethod]
    public void EmptyLt_TraderNameChanged_ExpectedPropertyUpdatedDeltaUpdatesAffected()
    {
        Assert.IsFalse(emptyLt.IsExternalTraderNameUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.AreEqual(null, emptyLt.ExternalTraderName);
        Assert.AreEqual(0, emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).Count());

        emptyLt.ExternalTraderName = ExpectedTraderName;
        Assert.IsTrue(emptyLt.IsExternalTraderNameUpdated);
        Assert.AreEqual(emptyNameIdLookup[ExpectedTraderName], emptyLt.ExternalTraderNameId);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(ExpectedTraderName, emptyLt.ExternalTraderName);
        var sourceUpdates = emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);

        var expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalTraderNameId
                                                  , emptyLt.ExternalTraderNameId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        emptyLt.IsExternalTraderNameUpdated = false;
        Assert.IsFalse(emptyLt.IsExternalTraderNameUpdated);
        Assert.IsFalse(emptyLt.HasUpdates);
        Assert.IsTrue(emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).IsNullOrEmpty());
        var stringUpdates = emptyLt.GetStringUpdates(ExpectedTradeTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, stringUpdates.Count);
        emptyLt.HasUpdates = false;

        var nextExpectedTraderName = "AnotherTraderName";
        emptyLt.ExternalTraderName = nextExpectedTraderName;
        Assert.IsTrue(emptyLt.IsExternalTraderNameUpdated);
        Assert.IsTrue(emptyLt.HasUpdates);
        Assert.AreEqual(nextExpectedTraderName, emptyLt.ExternalTraderName);
        sourceUpdates = emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        stringUpdates = emptyLt.GetStringUpdates(ExpectedTradeTime, PQMessageFlags.Update).ToList();
        Assert.AreEqual(1, stringUpdates.Count);
        expectedFieldUpdate = new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalTraderNameId
                                              , emptyLt.ExternalTraderNameId);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);
        var expectedStringUpdates = new PQFieldStringUpdate
        {
            Field = new PQFieldUpdate(PQFeedFields.LastTradedStringUpdates, CrudCommand.Upsert.ToPQSubFieldId(), 0u)
          , StringUpdate = new PQStringUpdate
            {
                Command = CrudCommand.Upsert, DictionaryId = emptyLt.NameIdLookup[emptyLt.ExternalTraderName]
              , Value   = emptyLt.ExternalTraderName
            }
        };
        Assert.AreEqual(expectedStringUpdates, stringUpdates[0]);

        emptyLt.HasUpdates = false;
        sourceUpdates = (from update in emptyLt.GetDeltaUpdateFields(ExpectedTradeTime, PQMessageFlags.Snapshot)
            where update.TradingSubId == PQTradingSubFieldKeys.LastTradedExternalTraderNameId
            select update).ToList();
        Assert.AreEqual(1, sourceUpdates.Count);
        Assert.AreEqual(expectedFieldUpdate, sourceUpdates[0]);

        var newEmptyNameIdLookup = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        var newEmpty             = new PQLastExternalCounterPartyTrade(newEmptyNameIdLookup);
        newEmpty.UpdateField(sourceUpdates[0]);
        newEmpty.UpdateFieldString(stringUpdates[0]);
        Assert.AreEqual(nextExpectedTraderName, newEmpty.ExternalTraderName);
        Assert.IsTrue(newEmpty.IsExternalTraderNameUpdated);
    }

    [TestMethod]
    public void PopulatedLt_HasUpdates_ClearedAndSetAffectsAllTrackedFields()
    {
        Assert.IsTrue(populatedLt.HasUpdates);
        Assert.IsTrue(populatedLt.IsTradePriceUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(populatedLt.IsTradeVolumeUpdated);
        Assert.IsTrue(populatedLt.IsWasGivenUpdated);
        Assert.IsTrue(populatedLt.IsWasPaidUpdated);
        Assert.IsTrue(populatedLt.IsExternalTraderNameUpdated);
        populatedLt.HasUpdates = false;
        Assert.IsFalse(populatedLt.HasUpdates);
        Assert.IsFalse(populatedLt.IsTradePriceUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(populatedLt.IsTradeVolumeUpdated);
        Assert.IsFalse(populatedLt.IsWasGivenUpdated);
        Assert.IsFalse(populatedLt.IsWasPaidUpdated);
        Assert.IsFalse(populatedLt.IsExternalTraderNameUpdated);
        populatedLt.HasUpdates = true;
        Assert.IsTrue(populatedLt.HasUpdates);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(populatedLt.IsTradeVolumeUpdated);
        Assert.IsTrue(populatedLt.IsWasGivenUpdated);
        Assert.IsTrue(populatedLt.IsWasPaidUpdated);
        Assert.IsTrue(populatedLt.IsExternalTraderNameUpdated);
    }

    [TestMethod]
    public void PopulatedLt_Reset_ReturnsReturnsLayerToEmpty()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.AreNotEqual(0m, populatedLt.TradePrice);
        Assert.AreNotEqual(DateTime.MinValue, populatedLt.TradeTime);
        Assert.AreNotEqual(0m, populatedLt.TradeVolume);
        Assert.IsTrue(populatedLt.WasGiven);
        Assert.IsTrue(populatedLt.WasPaid);
        Assert.IsTrue(populatedLt.IsTradePriceUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsTrue(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsTrue(populatedLt.IsTradeVolumeUpdated);
        Assert.IsTrue(populatedLt.IsWasGivenUpdated);
        Assert.IsTrue(populatedLt.IsWasPaidUpdated);
        Assert.IsTrue(populatedLt.IsExternalTraderNameUpdated);
        populatedLt.StateReset();
        Assert.IsTrue(populatedLt.IsEmpty);
        Assert.AreEqual(0m, populatedLt.TradePrice);
        Assert.AreEqual(DateTime.MinValue, populatedLt.TradeTime);
        Assert.AreEqual(0m, populatedLt.TradeVolume);
        Assert.IsFalse(populatedLt.WasGiven);
        Assert.IsFalse(populatedLt.WasPaid);
        Assert.IsFalse(populatedLt.IsTradePriceUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeDateUpdated);
        Assert.IsFalse(populatedLt.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(populatedLt.IsTradeVolumeUpdated);
        Assert.IsFalse(populatedLt.IsWasGivenUpdated);
        Assert.IsFalse(populatedLt.IsWasPaidUpdated);
        Assert.IsFalse(populatedLt.IsExternalTraderNameUpdated);
    }

    [TestMethod]
    public void EmptyAndPopulatedLt_IsEmpty_ReturnsAsExpected()
    {
        Assert.IsFalse(populatedLt.IsEmpty);
        Assert.IsTrue(emptyLt.IsEmpty);
    }

    [TestMethod]
    public void PopulatedLtWithAllUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsAllPvlFields()
    {
        var pqFieldUpdates =
            populatedLt.GetDeltaUpdateFields(new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Update).ToList();
        AssertContainsAllLtFields(pqFieldUpdates, populatedLt);
    }

    [TestMethod]
    public void PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsSnapshot_ReturnsAllPvlFields()
    {
        populatedLt.HasUpdates = false;
        var pqFieldUpdates =
            populatedLt.GetDeltaUpdateFields(new DateTime(2017, 12, 17, 12, 33, 1), PQMessageFlags.Snapshot).ToList();
        AssertContainsAllLtFields(pqFieldUpdates, populatedLt);
    }

    [TestMethod]
    public void PopulatedLtWithNoUpdates_GetDeltaUpdateFieldsAsUpdate_ReturnsNoUpdates()
    {
        populatedLt.HasUpdates = false;
        var pqFieldUpdates =
            populatedLt.GetDeltaUpdateFields(new DateTime(2017, 11, 04, 16, 33, 59), PQMessageFlags.Update).ToList();
        Assert.AreEqual(0, pqFieldUpdates.Count);
    }

    [TestMethod]
    public void PopulatedLt_GetDeltaUpdatesUpdateReplayThenUpdateFieldNewLt_CopiesAllFieldsToNewLt()
    {
        var pqFieldUpdates =
            populatedLt.GetDeltaUpdateFields(new DateTime(2017, 11, 04, 13, 33, 3)
                                           , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
        var pqStringUpdates =
            populatedLt.GetStringUpdates(new DateTime(2017, 11, 04, 13, 33, 3)
                                       , PQMessageFlags.Update | PQMessageFlags.IncludeReceiverTimes).ToList();
        var newEmpty = new PQLastExternalCounterPartyTrade(nameIdLookup);
        foreach (var pqFieldUpdate in pqFieldUpdates) newEmpty.UpdateField(pqFieldUpdate);
        foreach (var pqStringUpdate in pqStringUpdates) newEmpty.UpdateFieldString(pqStringUpdate);
        Assert.AreEqual(populatedLt, newEmpty);
    }

    [TestMethod]
    public void FullyPopulatedLt_CopyFromToEmptyLt_PvlsEqualEachOther()
    {
        var nonPQLt = new LastExternalCounterPartyTrade(populatedLt);
        emptyLt.CopyFrom(nonPQLt);
        Assert.AreEqual(populatedLt, emptyLt);
    }

    [TestMethod]
    public void FullyPopulatedLt_HasNoUpdatesCopyFrom_OnlyCopiesMinimalData()
    {
        var emptyPriceVolumeLayer = new PQLastExternalCounterPartyTrade(nameIdLookup);
        populatedLt.HasUpdates = false;
        emptyPriceVolumeLayer.CopyFrom(populatedLt);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.TradePrice);
        Assert.AreEqual(DateTime.MinValue, emptyPriceVolumeLayer.TradeTime);
        Assert.AreEqual(0m, emptyPriceVolumeLayer.TradeVolume);
        Assert.IsFalse(emptyPriceVolumeLayer.WasGiven);
        Assert.IsFalse(emptyPriceVolumeLayer.WasPaid);
        Assert.IsNull(emptyPriceVolumeLayer.ExternalTraderName);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradePriceUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeTimeDateUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeTimeSub2MinUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsTradeVolumeUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsWasGivenUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsWasPaidUpdated);
        Assert.IsFalse(emptyPriceVolumeLayer.IsExternalTraderNameUpdated);
    }

    [TestMethod]
    public void FromInterfacePopulatedLastTrade_Cloned_ReturnsNewIdenticalCopy()
    {
        var clone = ((ICloneable)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = populatedLt.Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((ICloneable<ILastTrade>)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((ILastPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IMutableLastPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((ILastExternalCounterPartyTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IMutableLastExternalCounterPartyTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IPQLastTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IPQLastPaidGivenTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
        clone = ((IPQLastExternalCounterPartyTrade)populatedLt).Clone();
        Assert.AreNotSame(clone, populatedLt);
        Assert.AreEqual(populatedLt, clone);
    }

    [TestMethod]
    public void FullyPopulatedLtCloned_OneDifferenceAtATimeAreEquivalentExact_CorrectlyReturnsWhenDifferent()
    {
        var fullyPopulatedClone = (PQLastExternalCounterPartyTrade)((ICloneable)populatedLt).Clone();
        AssertAreEquivalentMeetsExpectedExactComparisonType(true, populatedLt,
                                                            fullyPopulatedClone);
        AssertAreEquivalentMeetsExpectedExactComparisonType(false, populatedLt,
                                                            fullyPopulatedClone);
    }

    public static void AssertContainsAllLtFields
    (IList<PQFieldUpdate> checkFieldUpdates, IPQLastExternalCounterPartyTrade lt)
    {
        PQLastPaidGivenTradeTests.AssertContainsAllLtFields(checkFieldUpdates, lt);

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalCounterPartyId, lt.ExternalCounterPartyId),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalCounterPartyId)
                      , $"For lastTrade {lt.GetType().Name}");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalCounterPartyNameId, lt.ExternalCounterPartyNameId),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalCounterPartyNameId)
                      , $"For lastTrade {lt.GetType().Name}");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalTraderId, lt.ExternalTraderId),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalTraderId)
                      , $"For lastTrade {lt.GetType().Name}");

        Assert.AreEqual(new PQFieldUpdate(PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalTraderNameId, lt.ExternalTraderNameId),
                        PQTickInstantTests.ExtractFieldUpdateWithId
                            (checkFieldUpdates, PQFeedFields.LastTradedTickTrades, PQTradingSubFieldKeys.LastTradedExternalTraderNameId)
                      , $"For lastTrade {lt.GetType().Name}");
    }

    [TestMethod]
    public void FullyPopulatedLtSameObjOrClones_Equals_ReturnsTrue()
    {
        Assert.AreEqual(populatedLt, populatedLt);
        Assert.AreEqual(populatedLt, ((ICloneable)populatedLt).Clone());
        Assert.AreEqual(populatedLt, ((ICloneable<ILastTrade>)populatedLt).Clone());
    }

    [TestMethod]
    public void FullyPopulatedPvl_GetHashCode_ReturnNumberNoException()
    {
        var hashCode = populatedLt.GetHashCode();
        Assert.IsTrue(hashCode != 0);
    }

    [TestMethod]
    public void FullyPopulatedPvl_ToString_ReturnsNameAndValues()
    {
        var toString = populatedLt.ToString();

        Assert.IsTrue(toString.Contains(populatedLt.GetType().Name));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeId)}: {populatedLt.TradeId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.BatchId)}: {populatedLt.BatchId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradePrice)}: {populatedLt.TradePrice:N5}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeTime)}: {populatedLt.TradeTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeTypeFlags)}: {populatedLt.TradeTypeFlags}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeLifeCycleStatus)}: {populatedLt.TradeLifeCycleStatus}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.FirstNotifiedTime)}: {populatedLt.FirstNotifiedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.AdapterReceivedTime)}: {populatedLt.AdapterReceivedTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.UpdateTime)}: {populatedLt.UpdateTime:O}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.OrderId)}: {populatedLt.OrderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.TradeVolume)}: {populatedLt.TradeVolume:N2}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasGiven)}: {populatedLt.WasGiven}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.WasPaid)}: {populatedLt.WasPaid}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.ExternalCounterPartyId)}: {populatedLt.ExternalCounterPartyId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.ExternalCounterPartyName)}: {populatedLt.ExternalCounterPartyName}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.ExternalTraderId)}: {populatedLt.ExternalTraderId}"));
        Assert.IsTrue(toString.Contains($"{nameof(populatedLt.ExternalTraderName)}: {populatedLt.ExternalTraderName}"));
    }

    public static void AssertAreEquivalentMeetsExpectedExactComparisonType
    (bool exactComparison,
        PQLastExternalCounterPartyTrade? original, PQLastExternalCounterPartyTrade? changingLastTraderPaidGivenTrade = null,
        PQLastTradedList? originalRecentlyTraded = null, PQLastTradedList? changingRecentlyTraded = null,
        PQPublishableLevel3Quote? originalQuote = null, PQPublishableLevel3Quote? changingQuote = null)
    {
        if (original == null) return;


        PQLastPaidGivenTradeTests
            .AssertAreEquivalentMeetsExpectedExactComparisonType
                (exactComparison, original, changingLastTraderPaidGivenTrade, originalRecentlyTraded
               , changingRecentlyTraded, originalQuote, changingQuote);

        if (original.GetType() == typeof(PQLastExternalCounterPartyTrade))
            Assert.AreEqual(!exactComparison,
                            changingLastTraderPaidGivenTrade!.AreEquivalent
                                (new LastExternalCounterPartyTrade(original), exactComparison));

        changingLastTraderPaidGivenTrade!.ExternalCounterPartyId = 2_882_660;
        Assert.IsFalse(original.AreEquivalent(changingLastTraderPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                           originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTraderPaidGivenTrade.ExternalCounterPartyId = original.ExternalCounterPartyId;
        Assert.IsTrue(changingLastTraderPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTraderPaidGivenTrade.ExternalCounterPartyName = "Changed CounterParty Name";
        Assert.IsFalse(original.AreEquivalent(changingLastTraderPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                           originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTraderPaidGivenTrade.ExternalCounterPartyName = original.ExternalCounterPartyName;
        Assert.IsTrue(changingLastTraderPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTraderPaidGivenTrade.ExternalTraderId = 882_660;
        Assert.IsFalse(original.AreEquivalent(changingLastTraderPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                           originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTraderPaidGivenTrade.ExternalTraderId = original.ExternalTraderId;
        Assert.IsTrue(changingLastTraderPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTraderPaidGivenTrade.ExternalTraderName = "Changed Trader Name";
        Assert.IsFalse(original.AreEquivalent(changingLastTraderPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsFalse(
                           originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsFalse(originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTraderPaidGivenTrade.ExternalTraderName = original.ExternalTraderName;
        Assert.IsTrue(changingLastTraderPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));

        changingLastTraderPaidGivenTrade.IsExternalTraderNameUpdated = !changingLastTraderPaidGivenTrade.IsExternalTraderNameUpdated;
        Assert.AreEqual(!exactComparison, original.AreEquivalent(changingLastTraderPaidGivenTrade, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.AreEqual(!exactComparison,
                            originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null)
            Assert.AreEqual(!exactComparison,
                            originalQuote.AreEquivalent(changingQuote, exactComparison));
        changingLastTraderPaidGivenTrade.IsExternalTraderNameUpdated = original.IsExternalTraderNameUpdated;
        Assert.IsTrue(changingLastTraderPaidGivenTrade.AreEquivalent(original, exactComparison));
        if (originalRecentlyTraded != null)
            Assert.IsTrue(
                          originalRecentlyTraded.AreEquivalent(changingRecentlyTraded, exactComparison));
        if (originalQuote != null) Assert.IsTrue(originalQuote.AreEquivalent(changingQuote, exactComparison));
    }
}
