﻿#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.CounterParties;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.CounterParties;

[TestClass]
public class OrxPartiesTests
{
    private const int BufferSize = 2048;
    private byte[] byteBuffer = null!;
    private ReadSocketBufferContext readSocketBufferContext = null!;

    [TestInitialize]
    public void SetUp()
    {
        byteBuffer = new byte[BufferSize];

        readSocketBufferContext = new ReadSocketBufferContext
        {
            EncodedBuffer = new ReadWriteBuffer(byteBuffer)
            , DispatchLatencyLogger = new PerfLogger("", TimeSpan.MaxValue, "")
            , MessageVersion = TradingVersionInfo.CurrentVersion
        };
    }

    [TestMethod]
    public void NewParties_Serialize_DeserializesProperly()
    {
        var orxOrxClientOrderIdSerializer = new OrxByteSerializer<TestParties>();
        var originalClientOrderId = new TestParties
        {
            FirstParties = BuildSpotOrder(), SecondParties = BuildSpotOrder(), ThirdParties = BuildSpotOrder()
            , FourthParties = BuildSpotOrder()
        };

        readSocketBufferContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
            byteBuffer, 0, OrxMessageHeader.HeaderSize);

        var orderSubmitRequestsDeserializer = new OrxByteDeserializer<TestParties>(new OrxDeserializerLookup(
            new Recycler()));

        var deserializedOrxClientOrderId = orderSubmitRequestsDeserializer.Deserialize(readSocketBufferContext);

        Assert.AreEqual(originalClientOrderId.FirstParties, deserializedOrxClientOrderId.FirstParties);
        Assert.AreEqual(originalClientOrderId.SecondParties, deserializedOrxClientOrderId.SecondParties);
        Assert.AreEqual(originalClientOrderId.ThirdParties, deserializedOrxClientOrderId.ThirdParties);
        Assert.AreEqual(originalClientOrderId.FourthParties, deserializedOrxClientOrderId.FourthParties);
    }

    private OrxParties BuildSpotOrder() =>
        new(new OrxParty("BuySidePartyId", "BuySideName",
                new OrxParty("BuySideParentPartyId", "BuySideParentPartyName", null,
                    "BuySideParentClientPartyId", new OrxBookingInfo("", "")),
                "BuySideClientParty", new OrxBookingInfo("BuySidePortfolio", "BuySideSubPortfolio")),
            new OrxParty("SellSidePartyId", "SellSideName",
                new OrxParty("SellSideParentPartyId", "SellSideParentPartyName", null,
                    "SellSideParentClientPartyId", new OrxBookingInfo("", "")),
                "SellSideClientParty", new OrxBookingInfo("SellSidePortfolio", "SellSideSubPortfolio")));

    public class TestParties
    {
        [OrxMandatoryField(0)] public OrxParties? FirstParties { get; set; }

        [OrxMandatoryField(1)] public OrxParties? SecondParties { get; set; }

        [OrxOptionalField(1)] public OrxParties? ThirdParties { get; set; }

        [OrxOptionalField(2)] public OrxParties? FourthParties { get; set; }
    }
}
