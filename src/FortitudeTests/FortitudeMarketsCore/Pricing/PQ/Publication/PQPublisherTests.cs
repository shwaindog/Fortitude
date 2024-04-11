#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Publication;
using FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using Moq;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Publication;

[TestClass]
public class PQPublisherTests
{
    private Mock<IMarketConnectionConfig> moqMarketConnectionConfig = null!;
    private Mock<IPQLevel1Quote> moqPQLevel1Quote = null!;
    private Mock<IPQServer<IPQLevel1Quote>> moqPqServer = null!;
    private Mock<ISourceTickerQuoteInfo> moqSourceTickerQuoteInfo = null!;
    private PQPublisher<IPQLevel1Quote> pqPublisher = null!;

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
        moqMarketConnectionConfig.Setup(stpcr => stpcr.AllSourceTickerInfos)
            .Returns(new List<ISourceTickerQuoteInfo>()
            {
                new SourceTickerQuoteInfo(1, "First", 1, "First")
                , new SourceTickerQuoteInfo(1, "First", 2, "Second")
                , new SourceTickerQuoteInfo(2, "Second", 1, "First")
            });

        pqPublisher.RegisterTickersWithServer(moqMarketConnectionConfig.Object);
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

        moqPQLevel1Quote.Setup(pql0qp => pql0qp.CopyFrom(It.IsAny<ILevel0Quote>(), CopyMergeFlags.Default))
            .Verifiable();
        moqPqServer.Setup(pqs => pqs.Publish(moqPQLevel1Quote.Object)).Verifiable();

        var dummyLevel0Quote = new PQLevel0QuoteTests.DummyPQLevel0Quote();
        var moqSourceTickerQuoteInfo = new Mock<ISourceTickerQuoteInfo>();
        moqSourceTickerQuoteInfo.As<ISourceTickerQuoteInfo>().SetupGet(stqi => stqi.Ticker).Returns("MoqTicker");
        dummyLevel0Quote.SourceTickerQuoteInfo = moqSourceTickerQuoteInfo.Object;

        pqPublisher.PublishQuoteUpdate(dummyLevel0Quote);

        moqPqServer.Verify();
        moqPQLevel1Quote.Verify();
    }


    [TestMethod]
    public void TickerWithValues_Dispose_PublishesResetToAllTickers()
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

        moqSourceTickerQuoteInfo = new Mock<ISourceTickerQuoteInfo>();
        moqSourceTickerQuoteInfo.SetupGet(stpc => stpc.Ticker).Returns("MoqTicker");
        moqSourceTickerQuoteInfo.SetupGet(stpc => stpc.RoundingPrecision).Returns(0.00001m);
        moqSourceTickerQuoteInfo.SetupGet(stpc => stpc.IncrementSize).Returns(1m);

        moqMarketConnectionConfig = new Mock<IMarketConnectionConfig>();
        moqMarketConnectionConfig.Setup(stpcr => stpcr.AllSourceTickerInfos)
            .Returns(new List<ISourceTickerQuoteInfo> { moqSourceTickerQuoteInfo.Object });

        moqPQLevel1Quote = new Mock<IPQLevel1Quote>();
        moqPqServer.Setup(pqs => pqs.Register(It.IsAny<string>()))
            .Returns(moqPQLevel1Quote.Object);
        pqPublisher.RegisterTickersWithServer(moqMarketConnectionConfig.Object);
    }
}
