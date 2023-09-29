using System;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Trading.Orders;
using FortitudeMarketsApi.Trading.Orders.Products;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.Orders.Products;
using FortitudeMarketsCore.Trading.ORX.Orders.Products.General;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Orders.Products.General
{
    [TestClass]
    public class OrxSpotOrderTests
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
        public void NewSpotOrders_Serialize_DeserializesProperly()
        {
            var orxOrxClientOrderIdSerializer = new OrxByteSerializer<SpotOrders>();
            var originalClientOrderId = new SpotOrders()
            {
                FirstSpotOrder = BuildSpotOrder(),
                SecondSpotOrder = BuildSpotOrder(),
                ThirdSpotOrder = BuildSpotOrder(),
                FourthSpotOrder = BuildSpotOrder(),
            };

            dispatchContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
                byteBuffer, 0, 0);

            var orderSubmitRequestsDeserializer = new OrxByteDeserializer<SpotOrders>(new OrxDeserializerLookup(
                new OrxRecyclingFactory()));

            var deserializedOrxClientOrderId = (SpotOrders)orderSubmitRequestsDeserializer
                .Deserialize(dispatchContext);

            Assert.AreEqual(originalClientOrderId.FirstSpotOrder, deserializedOrxClientOrderId.FirstSpotOrder);
            Assert.AreEqual(originalClientOrderId.SecondSpotOrder, deserializedOrxClientOrderId.SecondSpotOrder);
            Assert.AreEqual(originalClientOrderId.ThirdSpotOrder, deserializedOrxClientOrderId.ThirdSpotOrder);
            Assert.AreEqual(originalClientOrderId.FourthSpotOrder, deserializedOrxClientOrderId.FourthSpotOrder);
        }

        private OrxSpotOrder BuildSpotOrder()
        {
            return new OrxSpotOrder("TestTicker", OrderSide.Bid, 1.234567m, 988_765_123, 
                OrderType.Limit, 100_000m, 0.0001m, 1000m, 
                FillExpectation.Complete, new OrxVenuePriceQuoteId(123, 234, 1234567u, 123456u, 
                    new DateTime(2018, 4, 1, 19, 11, 21)), 1.23456m, 100_000m);
        }

        public class SpotOrders
        {
            [OrxMandatoryField(0)]
            public OrxSpotOrder FirstSpotOrder { get; set; }
            [OrxMandatoryField(1)]
            public OrxSpotOrder SecondSpotOrder { get; set; }
            [OrxOptionalField(1)]
            public OrxSpotOrder ThirdSpotOrder { get; set; }
            [OrxOptionalField(2)]
            public OrxSpotOrder FourthSpotOrder { get; set; }
        }
    }
}