using System;
using System.Collections.Generic;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Trading.Orders.Venues;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Venues
{
    [TestClass]
    public class OrxVenueCriteriaTests
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
        public void NewVenueCriterias_Serialize_DeserializesProperly()
        {
            var orxOrxClientOrderIdSerializer = new OrxByteSerializer<VenueCriterias>();
            var originalClientOrderId = new VenueCriterias()
            {
                FirstVenueCriteria = BuildSpotOrder(),
                SecondVenueCriteria = BuildSpotOrder(),
                ThirdVenueCriteria = BuildSpotOrder(),
                FourthVenueCriteria = BuildSpotOrder(),
            };

            dispatchContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
                byteBuffer, 0, 0);

            var orderSubmitRequestsDeserializer = new OrxByteDeserializer<VenueCriterias>(new OrxDeserializerLookup(
                new OrxRecyclingFactory()));

            var deserializedOrxClientOrderId = (VenueCriterias)orderSubmitRequestsDeserializer
                .Deserialize(dispatchContext);

            Assert.AreEqual(originalClientOrderId.FirstVenueCriteria, deserializedOrxClientOrderId.FirstVenueCriteria);
            Assert.AreEqual(originalClientOrderId.SecondVenueCriteria, deserializedOrxClientOrderId.SecondVenueCriteria);
            Assert.AreEqual(originalClientOrderId.ThirdVenueCriteria, deserializedOrxClientOrderId.ThirdVenueCriteria);
            Assert.AreEqual(originalClientOrderId.FourthVenueCriteria, deserializedOrxClientOrderId.FourthVenueCriteria);
        }

        private OrxVenueCriteria BuildSpotOrder()
        {
            return new OrxVenueCriteria(new List<OrxVenue>
            {
                new OrxVenue(1234, "FirstOrxVenue"),
                new OrxVenue(45678, "SecondOrxVenue")
            }, VenueSelectionMethod.Default);
        }

        public class VenueCriterias
        {
            [OrxMandatoryField(0)]
            public OrxVenueCriteria FirstVenueCriteria { get; set; }
            [OrxMandatoryField(1)]
            public OrxVenueCriteria SecondVenueCriteria { get; set; }
            [OrxOptionalField(1)]
            public OrxVenueCriteria ThirdVenueCriteria { get; set; }
            [OrxOptionalField(2)]
            public OrxVenueCriteria FourthVenueCriteria { get; set; }
        }
    }
}