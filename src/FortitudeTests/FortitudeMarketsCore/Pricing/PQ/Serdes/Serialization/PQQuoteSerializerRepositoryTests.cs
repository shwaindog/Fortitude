// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

[TestClass]
public class PQClientQuoteSerializerRepositoryTests
{
    private const ushort ExpectedSourceId = ushort.MaxValue;
    private const ushort ExpectedTickerd  = ushort.MaxValue;

    private PQClientQuoteSerializerRepository pqClientQuoteSerializerRepository = null!;

    private ISourceTickerInfo sourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        sourceTickerInfo = new SourceTickerInfo
            (ExpectedSourceId, "TestSource", ExpectedTickerd, "TestTicker", Level3Quote, Unknown
           , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
           , layerFlags: LayerFlags.Volume | LayerFlags.Price
           , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                              LastTradedFlags.LastTradedTime);

        pqClientQuoteSerializerRepository = new PQClientQuoteSerializerRepository(new Recycler(), null);
    }

    [TestMethod]
    public void EmptyQuoteSerializationFactory_GetSerializerUintArray_ReturnsPQRequestSerializer()
    {
        pqClientQuoteSerializerRepository.RegisterSerializer<PQSnapshotIdsRequest>();
        var uintArraySerializer
            = pqClientQuoteSerializerRepository.GetSerializer<PQSnapshotIdsRequest>((uint)PQMessageIds.SnapshotIdsRequest);
        Assert.IsNotNull(uintArraySerializer);
        Assert.IsInstanceOfType(uintArraySerializer, typeof(PQSnapshotIdsRequestSerializer));
    }

    [TestMethod]
    public void NoEnteredDeserializer_GetSerializerNonSupportedType_ReturnsNull()
    {
        var result = pqClientQuoteSerializerRepository.GetSerializer<PQTickInstant>(sourceTickerInfo.SourceTickerId);
        Assert.IsNull(result);
    }
}
