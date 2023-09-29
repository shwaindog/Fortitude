using System;
using System.Collections.Generic;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization;
using FortitudeMarketsCore.Pricing.PQ.Subscription;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Serialization
{
    [TestClass]
    public class PQServerSerializationFactoryTests
    {
        private PQServerSerializationFactory snapshotServerSerializationFactory;

        [TestInitialize]
        public void SetUp()
        {
            snapshotServerSerializationFactory = new PQServerSerializationFactory(PQFeedType.Snapshot);
        }

        [TestMethod]
        public void NewSerializationFactory_GetSerializer_ReturnsAppropriateSerializerForMessageType()
        {
            var quoteSerializer = snapshotServerSerializationFactory.GetSerializer<IPQLevel0Quote>(0);
            Assert.IsInstanceOfType(quoteSerializer, typeof(PQQuoteSerializer));
            
            var heartBeatSerializer = snapshotServerSerializationFactory.GetSerializer<IEnumerable<IPQLevel0Quote>>(1);
            Assert.IsInstanceOfType(heartBeatSerializer, typeof(PQHeartbeatSerializer));
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void QuoteSerializerWrongId_GetSerializerWrongIdsForType_ThrowsNotSupportedException()
        {
            snapshotServerSerializationFactory.GetSerializer<IPQLevel0Quote>(1);
        }

        [TestMethod, ExpectedException(typeof(NotSupportedException))]
        public void HeartBeatSerializerWrongId_GetSerializerWrongIdsForType_ThrowsNotSupportedException()
        {
            snapshotServerSerializationFactory.GetSerializer<IEnumerable<IPQLevel0Quote>>(0);
        }
    }
}