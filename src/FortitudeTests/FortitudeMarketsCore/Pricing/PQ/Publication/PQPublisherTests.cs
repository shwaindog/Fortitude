using System;
using System.Collections.Generic;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeTests.FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication
{
    [TestClass]
    public class PQPublisherTests
    {
        private Mock<IPQServer<IPQLevel1Quote>> moqPqServer;
        private PQPublisher<IPQLevel1Quote> pqPublisher;
        private Mock<ISourceTickerPublicationConfig> moqSourceTickerPublicationConfig;
        private Mock<ISourceTickerPublicationConfigRepository> moqSourceTickerPubConfigRepo;
        private Mock<IPQLevel1Quote> moqPQLevel1Quote;

        [TestInitialize]
        public void SetUp()
        {
            moqPqServer = new Mock<IPQServer<IPQLevel1Quote>>();
        }

        [TestMethod]
        public void NewPQPublisher_RegisterTickersWithServer_WaitsToServerStartToRegister()
        {
            pqPublisher = new PQPublisher<IPQLevel1Quote>(moqPqServer.Object);

            moqPqServer.Verify(pqs => pqs.Register(It.IsAny<string>()), Times.Never);

            moqSourceTickerPubConfigRepo = new Mock<ISourceTickerPublicationConfigRepository>();
            moqSourceTickerPubConfigRepo.Setup(stpcr => stpcr.GetEnumerator())
                .Returns(() =>
                    SourceTickerPublicationConfigRepositoryTests.SampleSourceTickerPublicationConfigRepo()
                        .GetEnumerator());

            pqPublisher.RegisterTickersWithServer(moqSourceTickerPubConfigRepo.Object);
            moqPqServer.Verify(pqs => pqs.Register(It.IsAny<string>()), Times.Exactly(3));
        }

        [TestMethod]
        public void ConfiguredTicker_PublishReset_PushesEmptyPictureWithUpdatedTimestampsToServer()
        {
            SetupTickerWithPublisher();

            moqPQLevel1Quote.Setup(pql0qp => pql0qp.ResetFields()).Verifiable();
            moqPQLevel1Quote.SetupSet(pql0qp => pql0qp.SourceTime = It.IsAny<DateTime>()).Verifiable();
            moqPQLevel1Quote.SetupSet(pql0qp => pql0qp.ClientReceivedTime = It.IsAny<DateTime>()).Verifiable();
            moqPQLevel1Quote.SetupSet(pql0qp => pql0qp.AdapterSentTime = It.IsAny<DateTime>()).Verifiable();

            moqPqServer.Setup(pqs => pqs.Publish(moqPQLevel1Quote.Object)).Verifiable();

            pqPublisher.PublishReset("MoqTicker", new DateTime(2017, 03, 05, 17, 09, 03),
                new DateTime(2017, 03, 05, 17, 09, 03), new DateTime(2017, 03, 05, 17, 09, 03));

            moqPQLevel1Quote.Verify();
            moqPqServer.Verify();
        }

        [TestMethod]
        public void TickerWithValues_PublishQuoteUpdate_CopysQuoteDetailsTo()
        {
            SetupTickerWithPublisher();

            moqPQLevel1Quote.Setup(pql0qp => pql0qp.CopyFrom(It.IsAny<IPQLevel0Quote>(), CopyMergeFlags.Default)).Verifiable();
            moqPqServer.Setup(pqs => pqs.Publish(moqPQLevel1Quote.Object)).Verifiable();

            var dummyLevel0Quote = new PQLevel0QuoteTests.DummyPQLevel0Quote();
            var moqSourceTickerQuoteInfo = new Mock<IMutableSourceTickerQuoteInfo>();
            moqSourceTickerQuoteInfo.As<ISourceTickerQuoteInfo>().SetupGet(stqi => stqi.Ticker).Returns("MoqTicker");
            dummyLevel0Quote.SourceTickerQuoteInfo = moqSourceTickerQuoteInfo.Object;

            pqPublisher.PublishQuoteUpdate(dummyLevel0Quote);

            moqPqServer.Verify();
            moqPQLevel1Quote.Verify(); 
        }


        [TestMethod]
        public void TickerWithValues_Dispose_PublishesResetToAllTickers ()
        {
            SetupTickerWithPublisher();

            moqPQLevel1Quote.Setup(pql0qp => pql0qp.ResetFields()).Verifiable();
            moqPQLevel1Quote.SetupSet(pql0qp => pql0qp.SourceTime = It.IsAny<DateTime>()).Verifiable();
            moqPQLevel1Quote.SetupSet(pql0qp => pql0qp.ClientReceivedTime = It.IsAny<DateTime>()).Verifiable();
            moqPQLevel1Quote.SetupSet(pql0qp => pql0qp.AdapterSentTime = It.IsAny<DateTime>()).Verifiable();

            moqPqServer.Setup(pqs => pqs.Publish(moqPQLevel1Quote.Object)).Verifiable();

            pqPublisher.Dispose();

            moqPQLevel1Quote.Verify();
            moqPqServer.Verify();

        }

        private void SetupTickerWithPublisher()
        {
            pqPublisher = new PQPublisher<IPQLevel1Quote>(moqPqServer.Object);

            moqSourceTickerPublicationConfig = new Mock<ISourceTickerPublicationConfig>();
            moqSourceTickerPublicationConfig.SetupGet(stpc => stpc.Ticker).Returns("MoqTicker");
            moqSourceTickerPublicationConfig.SetupGet(stpc => stpc.RoundingPrecision).Returns(0.00001m);
            moqSourceTickerPublicationConfig.SetupGet(stpc => stpc.IncrementSize).Returns(1m);

            moqSourceTickerPubConfigRepo = new Mock<ISourceTickerPublicationConfigRepository>();
            moqSourceTickerPubConfigRepo.Setup(stpcr => stpcr.GetEnumerator())
                .Returns( () => new List<ISourceTickerPublicationConfig>
                    { moqSourceTickerPublicationConfig.Object}.GetEnumerator());

            moqPQLevel1Quote = new Mock<IPQLevel1Quote>();
            moqPqServer.Setup(pqs => pqs.Register(It.IsAny<string>()))
                .Returns(moqPQLevel1Quote.Object);
            pqPublisher.RegisterTickersWithServer(moqSourceTickerPubConfigRepo.Object);
        }
    }
}