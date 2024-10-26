// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Publication;
using FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using Moq;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Publication;

[TestClass]
public class PQPublisherTests
{
    private Mock<IMarketConnectionConfig>   moqMarketConnectionConfig = null!;
    private Mock<IPQLevel1Quote>            moqPQLevel1Quote          = null!;
    private Mock<IPQServer<IPQLevel1Quote>> moqPqServer               = null!;
    private Mock<ISourceTickerInfo>         moqSourceTickerInfo       = null!;
    private PQPublisher<IPQLevel1Quote>     pqPublisher               = null!;

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

        moqMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqMarketConnectionConfig
            .Setup
                (stpcr => stpcr.AllSourceTickerInfos)
            .Returns(new List<ISourceTickerInfo>
            {
                new SourceTickerInfo(1, "First", 1, "First", Level3Quote, Unknown)
              , new SourceTickerInfo(1, "First", 2, "Second", Level3Quote, Unknown)
              , new SourceTickerInfo(2, "Second", 1, "First", Level3Quote, Unknown)
            });

        pqPublisher.RegisterTickersWithServer(moqMarketConnectionConfig.Object);
        moqPqServer.Verify(pqs => pqs.Register(It.IsAny<string>()), Times.Exactly(3));
    }

    [TestMethod]
    public void ConfiguredTicker_PublishReset_PushesEmptyPictureWithUpdatedTimestampsToServer()
    {
        SetupTickerWithPublisher();

        moqPQLevel1Quote.Setup(pql1q => pql1q.ResetFields()).Verifiable();
        moqPQLevel1Quote.SetupSet(pql1q => pql1q.SourceTime         = It.IsAny<DateTime>()).Verifiable();
        moqPQLevel1Quote.SetupSet(pql1q => pql1q.ClientReceivedTime = It.IsAny<DateTime>()).Verifiable();
        moqPQLevel1Quote.SetupSet(pql1q => pql1q.AdapterSentTime    = It.IsAny<DateTime>()).Verifiable();

        moqPqServer.Setup(pqs => pqs.Publish(moqPQLevel1Quote.Object)).Verifiable();

        pqPublisher.PublishReset("MoqTicker", new DateTime(2017, 03, 05, 17, 09, 03)
                               , new DateTime(2017, 03, 05, 17, 09, 03)
                               , new DateTime(2017, 03, 05, 17, 09, 03));

        moqPQLevel1Quote.Verify();
        moqPqServer.Verify();
    }

    [TestMethod]
    public void TickerWithValues_PublishQuoteUpdate_CopysQuoteDetailsTo()
    {
        SetupTickerWithPublisher();

        moqPQLevel1Quote.Setup(pql1q => pql1q.CopyFrom(It.IsAny<ITickInstant>(), CopyMergeFlags.Default))
                        .Verifiable();
        moqPqServer.Setup(pqs => pqs.Publish(moqPQLevel1Quote.Object)).Verifiable();

        var dummyTickInstant    = new PQTickInstantTests.DummyPQTickInstant();
        var moqSourceTickerInfo = new Mock<ISourceTickerInfo>();
        moqSourceTickerInfo.As<ISourceTickerInfo>().SetupGet(stqi => stqi.Ticker).Returns("MoqTicker");
        dummyTickInstant.SourceTickerInfo = moqSourceTickerInfo.Object;

        pqPublisher.PublishQuoteUpdate(dummyTickInstant);

        moqPqServer.Verify();
        moqPQLevel1Quote.Verify();
    }


    [TestMethod]
    public void TickerWithValues_Dispose_PublishesResetToAllTickers()
    {
        SetupTickerWithPublisher();

        moqPQLevel1Quote.Setup(pql1q => pql1q.ResetFields()).Verifiable();
        moqPQLevel1Quote.SetupSet(pql1q => pql1q.SourceTime         = It.IsAny<DateTime>()).Verifiable();
        moqPQLevel1Quote.SetupSet(pql1q => pql1q.ClientReceivedTime = It.IsAny<DateTime>()).Verifiable();
        moqPQLevel1Quote.SetupSet(pql1q => pql1q.AdapterSentTime    = It.IsAny<DateTime>()).Verifiable();

        moqPqServer.Setup(pqs => pqs.Publish(moqPQLevel1Quote.Object)).Verifiable();

        pqPublisher.Dispose();

        moqPQLevel1Quote.Verify();
        moqPqServer.Verify();
    }

    private void SetupTickerWithPublisher()
    {
        pqPublisher = new PQPublisher<IPQLevel1Quote>(moqPqServer.Object);

        moqSourceTickerInfo = new Mock<ISourceTickerInfo>();
        moqSourceTickerInfo.SetupGet(stpc => stpc.Ticker).Returns("MoqTicker");
        moqSourceTickerInfo.SetupGet(stpc => stpc.RoundingPrecision).Returns(0.00001m);
        moqSourceTickerInfo.SetupGet(stpc => stpc.IncrementSize).Returns(1m);

        moqMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqMarketConnectionConfig.Setup(stpcr => stpcr.AllSourceTickerInfos)
                                 .Returns(new List<ISourceTickerInfo> { moqSourceTickerInfo.Object });

        moqPQLevel1Quote = new Mock<IPQLevel1Quote>();
        moqPqServer.Setup(pqs => pqs.Register(It.IsAny<string>()))
                   .Returns(moqPQLevel1Quote.Object);
        pqPublisher.RegisterTickersWithServer(moqMarketConnectionConfig.Object);
    }
}
