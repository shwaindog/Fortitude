using System;
using System.Collections.Generic;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.ORX.Authentication;
using FortitudeIO.Protocols.ORX.Serialization;
using FortitudeIO.Protocols.ORX.Serialization.Deserialization;
using FortitudeIO.Protocols.ORX.Serialization.ObjectRecycling;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Trading.Executions;
using FortitudeMarketsCore.Trading;
using FortitudeMarketsCore.Trading.ORX.CounterParties;
using FortitudeMarketsCore.Trading.ORX.Executions;
using FortitudeMarketsCore.Trading.ORX.Orders;
using FortitudeMarketsCore.Trading.ORX.Orders.Venues;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Trading.ORX.Executions
{
    [TestClass]
    public class OrxExecutionsTests
    {
        private DispatchContext dispatchContext;
        private byte[] byteBuffer;
        private const int BufferSize = 4096;

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
        public void NewVenueOrders_Serialize_DeserializesProperly()
        {
            var orxOrxClientOrderIdSerializer = new OrxByteSerializer<Executions>();
            var originalClientOrderId = new Executions()
            {
                FirstExecutions = BuildVenueOrders(),
                SecondExecutions = BuildVenueOrders(),
                ThirdExecutions = BuildVenueOrders(),
                FourthExecutions = BuildVenueOrders(),
            };

            dispatchContext.MessageSize = orxOrxClientOrderIdSerializer.Serialize(originalClientOrderId,
                byteBuffer, 0, 0);

            var venueOrdersDeserializer = new OrxByteDeserializer<Executions>(new OrxDeserializerLookup(
                new OrxRecyclingFactory()));

            var deserializedVenueOrders = (Executions)venueOrdersDeserializer
                .Deserialize(dispatchContext);

            Assert.AreEqual(originalClientOrderId.FirstExecutions, deserializedVenueOrders.FirstExecutions);
            Assert.AreEqual(originalClientOrderId.SecondExecutions, deserializedVenueOrders.SecondExecutions);
            Assert.AreEqual(originalClientOrderId.ThirdExecutions, deserializedVenueOrders.ThirdExecutions);
            Assert.AreEqual(originalClientOrderId.FourthExecutions, deserializedVenueOrders.FourthExecutions);
        }

        private OrxExecutions BuildVenueOrders()
        {
            return new OrxExecutions(new List<OrxExecution>
            {
                new OrxExecution(
                    new OrxExecutionId("FirstVenueExecutionId", 1234, "FirstVenuesBookingSystemId"),
                    new OrxVenue(1234, "FirstVenue"), new OrxVenueOrderId("FirstVenueClientOrderId", "FirstVenuesOrderId"),
                    new OrxOrderId(123, "Testing 123", 234, "Testing 234", new OrxOrderId(345, "Testing 345"), "TrackingId1234"), 
                    new DateTime(2018, 4, 1, 23, 44, 54), 1.23456m, 1_234_345m,
                    new OrxParty("PartyId", "PartyName", null,
                        "PartyId", null), DateTimeConstants.UnixEpoch, ExecutionType.CounterPartyGave, ExecutionStageType.Trade),
                new OrxExecution(
                    new OrxExecutionId("SecondVenueExecutionId", 1234, "SecondVenuesBookingSystemId"),
                    new OrxVenue(2345, "SecondVenue"), new OrxVenueOrderId("SecondVenueClientOrderId", "SecondVenuesOrderId"),
                    new OrxOrderId(123, "Testing 123", 234, "Testing 234", new OrxOrderId(345, "Testing 345"), "TrackingId1234"),
                    new DateTime(2018, 4, 1, 23, 44, 54), 1.23456m, 1_234_345m,
                    new OrxParty("PartyId", "PartyName", null,
                        "PartyId", null), DateTimeConstants.UnixEpoch, ExecutionType.CounterPartyGave, ExecutionStageType.Trade),
            });
        }

        public class Executions
        {
            [OrxMandatoryField(0)]
            public OrxExecutions FirstExecutions { get; set; }
            [OrxMandatoryField(1)]
            public OrxExecutions SecondExecutions { get; set; }
            [OrxOptionalField(2)]
            public OrxExecutions ThirdExecutions { get; set; }
            [OrxOptionalField(3)]
            public OrxExecutions FourthExecutions { get; set; }
        }
    }
}