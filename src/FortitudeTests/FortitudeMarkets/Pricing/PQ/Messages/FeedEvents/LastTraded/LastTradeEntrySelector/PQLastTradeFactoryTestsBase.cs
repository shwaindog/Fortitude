// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded.LastTradeEntrySelector;

public class PQLastTradeFactoryTestsBase
{
    protected const uint    ExpectedTradeId     = 42;
    protected const uint    ExpectedBatchId     = 24_942;
    protected const uint    ExpectedOrderId     = 1_772_942;
    protected const decimal ExpectedTradePrice  = 2.3456m;
    protected const decimal ExpectedTradeVolume = 42_000_111m;

    protected const LastTradedTypeFlags      ExpectedTradedTypeFlags     = LastTradedTypeFlags.HasPaidGivenDetails;
    protected const LastTradedLifeCycleFlags ExpectedTradeLifeCycleFlags = LastTradedLifeCycleFlags.Confirmed;

    protected static readonly DateTime ExpectedTradeTime           = new(2018, 03, 2, 14, 40, 30);
    protected static readonly DateTime ExpectedFirstNotifiedTime   = new(2018, 03, 2, 14, 40, 31);
    protected static readonly DateTime ExpectedAdapterReceivedTime = new(2018, 03, 2, 14, 40, 41);
    protected static readonly DateTime ExpectedUpdateTime          = new(2018, 03, 2, 14, 40, 42);

    protected const bool ExpectedWasGiven = true;
    protected const bool ExpectedWasPaid  = true;

    protected const int ExpectedTraderId       = 34_902;
    protected const int ExpectedCounterPartyId = 2_198;

    protected const string ExpectedTraderName       = "TraderName-Helen";
    protected const string ExpectedCounterPartyName = "CounterPartyName-Valcopp";


    protected IPQNameIdLookupGenerator   NameIdLookupGenerator                   = null!;
    protected PQLastPaidGivenTrade       PopulatedLastPaidGivenTrade             = null!;
    protected PQLastTrade                PopulatedLastTrade                      = null!;
    protected PQLastExternalCounterPartyTrade PopulatedLastExternalCounterPartyTrade = null!;

    [TestInitialize]
    public void Setup()
    {
        NameIdLookupGenerator = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);
        PopulatedLastTrade    = new PQLastTrade
            (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradedTypeFlags
           , ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime, ExpectedUpdateTime);
        PopulatedLastPaidGivenTrade = 
            new PQLastPaidGivenTrade
                (ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime);
        PopulatedLastExternalCounterPartyTrade = 
            new PQLastExternalCounterPartyTrade
                (NameIdLookupGenerator, ExpectedTradeId, ExpectedBatchId, ExpectedTradePrice, ExpectedTradeTime, ExpectedTradeVolume
               , ExpectedCounterPartyId, ExpectedCounterPartyName, ExpectedTraderId, ExpectedTraderName, ExpectedOrderId, ExpectedWasPaid
               , ExpectedWasGiven, ExpectedTradedTypeFlags, ExpectedTradeLifeCycleFlags, ExpectedFirstNotifiedTime, ExpectedAdapterReceivedTime
               , ExpectedUpdateTime)
        {
            ExternalTraderName = "PopulatedTraderName"
        };
    }
}
