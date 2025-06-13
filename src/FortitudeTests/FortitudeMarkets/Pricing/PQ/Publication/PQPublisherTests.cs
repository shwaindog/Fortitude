// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Config;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Publication;
using Moq;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[TestClass]
public class PQPublisherTests
{
    private Mock<IMarketConnectionConfig>              moqMarketConnectionConfig = null!;
    private Mock<IPQPublishableLevel1Quote>            moqPQLevel1Quote          = null!;
    private Mock<IPQServer<IPQPublishableLevel1Quote>> moqPqServer               = null!;
    private Mock<ISourceTickerInfo>                    moqSourceTickerInfo       = null!;
    private PQPublisher<IPQPublishableLevel1Quote>     pqPublisher               = null!;
    private PQSourceTickerInfo                         firstSourceTickerInfo = null!;
    private PQSourceTickerInfo                         secondSourceTickerInfo = null!;
    private PQSourceTickerInfo                         thirdSourceTickerInfo = null!;

    [TestInitialize]
    public void SetUp()
    {
        moqPqServer               = new Mock<IPQServer<IPQPublishableLevel1Quote>>();

        moqMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        firstSourceTickerInfo     = new PQSourceTickerInfo(1, "First", 1, "First", Level3Quote, MarketClassification.Unknown);
        secondSourceTickerInfo    = new PQSourceTickerInfo(1, "First", 2, "Second", Level3Quote, MarketClassification.Unknown);
        thirdSourceTickerInfo     = new PQSourceTickerInfo(2, "Second", 1, "First", Level3Quote, MarketClassification.Unknown);
        moqMarketConnectionConfig
            .Setup
                (stpcr => stpcr.AllSourceTickerInfos)
            .Returns(new List<ISourceTickerInfo>
            {
                firstSourceTickerInfo
              , secondSourceTickerInfo
              , thirdSourceTickerInfo
            });
    }

    [TestMethod]
    public void NewPQPublisher_RegisterTickersWithServer_WaitsToServerStartToRegister()
    {
        pqPublisher = new PQPublisher<IPQPublishableLevel1Quote>(moqPqServer.Object);

        moqPqServer.Verify(pqs => pqs.Register(It.IsAny<string>()), Times.Never);


        pqPublisher.RegisterStreamWithServer(moqMarketConnectionConfig.Object);
        moqPqServer.Verify(pqs => pqs.Register(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void ConfiguredTicker_PublishReset_PushesEmptyPictureWithUpdatedTimestampsToServer()
    {
        SetupTickerWithPublisher();

        var l1QuoteAsPubTickInstant = moqPQLevel1Quote.As<IPQPublishableTickInstant>();
        var lQ1AsPqMesg             = moqPQLevel1Quote.As<IPQMessage>();
        lQ1AsPqMesg.Setup(pqm => pqm.ResetWithTracking()).Verifiable();
        lQ1AsPqMesg.Setup(pqm => pqm.SetPublisherStateToConnectivityStatus(It.IsAny<PublisherStates>(), It.IsAny<DateTime>())).Verifiable();
        l1QuoteAsPubTickInstant.SetupSet(pqPl1Q => pqPl1Q.ClientReceivedTime = It.IsAny<DateTime>()).Verifiable();
        moqPQLevel1Quote.SetupSet(pqPl1Q => pqPl1Q.AdapterSentTime           = It.IsAny<DateTime>()).Verifiable();

        moqPqServer.Setup(pqs => pqs.Publish(moqPQLevel1Quote.Object)).Verifiable();

        pqPublisher.PublishReset("MoqTicker", new DateTime(2017, 03, 05, 17, 09, 03)
                               , new DateTime(2017, 03, 05, 17, 09, 03)
                               , new DateTime(2017, 03, 05, 17, 09, 03));

        moqPQLevel1Quote.Verify();
        moqPqServer.Verify();
    }

    [TestMethod]
    public void TickerWithValues_PublishQuoteUpdate_CopiesQuoteDetailsTo()
    {
        pqPublisher = new PQPublisher<IPQPublishableLevel1Quote>(moqPqServer.Object);
        moqPqServer.Setup(pqs => pqs.Register(It.IsAny<string>())).Returns(new PQPublishableLevel1Quote(firstSourceTickerInfo)).Verifiable();
        pqPublisher.RegisterStreamWithServer(moqMarketConnectionConfig.Object);

        var pubTickInstant = new PQPublishableLevel1Quote(firstSourceTickerInfo);
        moqPqServer.Setup(pqs => pqs.Publish(It.IsAny<IPQPublishableLevel1Quote>())).Verifiable();


        pqPublisher.PublishQuoteUpdate(pubTickInstant);

        moqPqServer.Verify();
    }


    [TestMethod]
    public void TickerWithValues_Dispose_PublishesResetToAllTickers()
    {
        SetupTickerWithPublisher();

        var lQ1AsPqMesg = moqPQLevel1Quote.As<IPQMessage>();
        lQ1AsPqMesg.Setup(pqm => pqm.ResetWithTracking()).Verifiable();
        lQ1AsPqMesg.Setup(pqm => pqm.SetPublisherStateToConnectivityStatus(It.IsAny<PublisherStates>(), It.IsAny<DateTime>())).Verifiable();
        moqPQLevel1Quote.SetupSet(pqPl1Q => pqPl1Q.ClientReceivedTime = It.IsAny<DateTime>()).Verifiable();
        moqPQLevel1Quote.SetupSet(pqPl1Q => pqPl1Q.AdapterSentTime    = It.IsAny<DateTime>()).Verifiable();

        moqPqServer.Setup(pqs => pqs.Publish(moqPQLevel1Quote.Object)).Verifiable();

        pqPublisher.Dispose();

        moqPQLevel1Quote.Verify();
        moqPqServer.Verify();
    }

    private void SetupTickerWithPublisher()
    {
        pqPublisher = new PQPublisher<IPQPublishableLevel1Quote>(moqPqServer.Object);

        moqSourceTickerInfo = new Mock<ISourceTickerInfo>();
        moqSourceTickerInfo.SetupGet(sti => sti.InstrumentName).Returns("MoqTicker");
        moqSourceTickerInfo.SetupGet(sti => sti.RoundingPrecision).Returns(0.00001m);
        moqSourceTickerInfo.SetupGet(sti => sti.IncrementSize).Returns(1m);

        moqMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqMarketConnectionConfig.Setup(mcc => mcc.AllSourceTickerInfos)
                                 .Returns(new List<ISourceTickerInfo> { moqSourceTickerInfo.Object });

        moqPQLevel1Quote = new Mock<IPQPublishableLevel1Quote>();
        moqPqServer.Setup(pqs => pqs.Register(It.IsAny<string>()))
                   .Returns(moqPQLevel1Quote.Object);
        pqPublisher.RegisterStreamWithServer(moqMarketConnectionConfig.Object);
    }
}
