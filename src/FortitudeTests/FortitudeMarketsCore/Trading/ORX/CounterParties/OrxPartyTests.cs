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
    public class OrxPartyTests
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
        public void NewParty_Serialize_DeserializesProperly()
        {
            var orxOrxClientOrderIdSerializer = new OrxByteSerializer<Parties>();
            var originalClientOrderId = new Parties()
            {
                FirstParty = BuildSpotOrder(),
                SecondParty = BuildSpotOrder(),
                ThirdParty = BuildSpotOrder(),
                FourthParty = BuildSpotOrder(),
            };

            dispatchContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
                byteBuffer, 0, 0);

            var orderSubmitRequestsDeserializer = new OrxByteDeserializer<Parties>(new OrxDeserializerLookup(
                new OrxRecyclingFactory()));

            var deserializedOrxClientOrderId = (Parties)orderSubmitRequestsDeserializer
                .Deserialize(dispatchContext);

            Assert.AreEqual(originalClientOrderId.FirstParty, deserializedOrxClientOrderId.FirstParty);
            Assert.AreEqual(originalClientOrderId.SecondParty, deserializedOrxClientOrderId.SecondParty);
            Assert.AreEqual(originalClientOrderId.ThirdParty, deserializedOrxClientOrderId.ThirdParty);
            Assert.AreEqual(originalClientOrderId.FourthParty, deserializedOrxClientOrderId.FourthParty);
        }

        private OrxParty BuildSpotOrder()
        {
            return new OrxParty("BuySidePartyId", "BuySideName",
                new OrxParty("BuySideParentPartyId", "BuySideParentPartyName", null,
                    "BuySideParentClientPartyId", null),
                "BuySideClientParty", new OrxBookingInfo("BuySidePortfolio", "BuySideSubPortfolio"));
        }

        public class Parties
        {
            [OrxMandatoryField(0)]
            public OrxParty FirstParty { get; set; }
            [OrxMandatoryField(1)]
            public OrxParty SecondParty { get; set; }
            [OrxOptionalField(1)]
            public OrxParty ThirdParty { get; set; }
            [OrxOptionalField(2)]
            public OrxParty FourthParty { get; set; }
        }
    }
}