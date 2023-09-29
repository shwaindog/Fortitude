using System;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.CounterParties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.CounterParties
{
    [TestClass]
    public class OrxPartiesTests
    {
        private DispatchContext dispatchContext;
        private byte[] byteBuffer;
        private const int BufferSize = 2048;

        [TestInitialize]
        public void SetUp()
        {
            byteBuffer = new byte[BufferSize];

            dispatchContext = new DispatchContext
            {
                EncodedBuffer = new ReadWriteBuffer(byteBuffer),
                DispatchLatencyLogger = new PerfLogger("", TimeSpan.MaxValue, ""),
                MessageVersion = TradingVersionInfo.CurrentVersion
            };
        }

        [TestMethod]
        public void NewParties_Serialize_DeserializesProperly()
        {
            var orxOrxClientOrderIdSerializer = new OrxByteSerializer<TestParties>();
            var originalClientOrderId = new TestParties()
            {
                FirstParties = BuildSpotOrder(),
                SecondParties = BuildSpotOrder(),
                ThirdParties = BuildSpotOrder(),
                FourthParties = BuildSpotOrder(),
            };

            dispatchContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
                byteBuffer, 0, 0);

            var orderSubmitRequestsDeserializer = new OrxByteDeserializer<TestParties>(new OrxDeserializerLookup(
                new OrxRecyclingFactory()));

            var deserializedOrxClientOrderId = (TestParties)orderSubmitRequestsDeserializer
                .Deserialize(dispatchContext);

            Assert.AreEqual(originalClientOrderId.FirstParties, deserializedOrxClientOrderId.FirstParties);
            Assert.AreEqual(originalClientOrderId.SecondParties, deserializedOrxClientOrderId.SecondParties);
            Assert.AreEqual(originalClientOrderId.ThirdParties, deserializedOrxClientOrderId.ThirdParties);
            Assert.AreEqual(originalClientOrderId.FourthParties, deserializedOrxClientOrderId.FourthParties);
        }

        private OrxParties BuildSpotOrder()
        {
            return new OrxParties(new OrxParty("BuySidePartyId", "BuySideName", 
                new OrxParty("BuySideParentPartyId", "BuySideParentPartyName", null,
                    "BuySideParentClientPartyId", null), 
                "BuySideClientParty", new OrxBookingInfo("BuySidePortfolio", "BuySideSubPortfolio")),
                new OrxParty("SellSidePartyId", "SellSideName",
                new OrxParty("SellSideParentPartyId", "SellSideParentPartyName", null, 
                    "SellSideParentClientPartyId", null),
                "SellSideClientParty", new OrxBookingInfo("SellSidePortfolio", "SellSideSubPortfolio")));
        }

        public class TestParties
        {
            [OrxMandatoryField(0)]
            public OrxParties FirstParties { get; set; }
            [OrxMandatoryField(1)]
            public OrxParties SecondParties { get; set; }
            [OrxOptionalField(1)]
            public OrxParties ThirdParties { get; set; }
            [OrxOptionalField(2)]
            public OrxParties FourthParties { get; set; }
        }
    }
}