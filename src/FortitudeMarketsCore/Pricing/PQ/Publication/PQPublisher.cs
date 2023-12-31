﻿#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public class PQPublisher<T> : IPQPublisher where T : IPQLevel0Quote
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPublisher<>));

    private readonly IMap<string, T> pictures = new ConcurrentMap<string, T>();
    private readonly IPQServer<T> pqServer;
    private volatile bool shutdownFlag;

    public PQPublisher(IPQServer<T> pqServer) => this.pqServer = pqServer;

    public void RegisterTickersWithServer(ISourceTickerPublicationConfigRepository tickersIdRef)
    {
        pqServer.StartServices();

        if (!shutdownFlag)
            foreach (var tickerRef in tickersIdRef)
            {
                var picture = pqServer.Register(tickerRef.Ticker);
                if (picture != null) pictures.AddOrUpdate(tickerRef.Ticker, picture);
            }
    }

    public void PublishReset(string ticker, DateTime exchangeTs, DateTime exchangeSentTs, DateTime adapterRecvTs)
    {
        if (pictures.TryGetValue(ticker, out var pqPicture))
        {
            pqPicture!.ResetFields();
            pqPicture.SourceTime = exchangeTs;
            pqPicture.ClientReceivedTime = adapterRecvTs;
            if (pqPicture is IMutableLevel1Quote pq1) pq1.AdapterSentTime = exchangeSentTs;
            pqServer.Publish(pqPicture);
        }
    }

    public void PublishQuoteUpdate(ILevel0Quote quote)
    {
        if (pictures.TryGetValue(quote.SourceTickerQuoteInfo!.Ticker, out var pqPicture))
        {
            // logger.Info("About to publish quote: {0}", quote);
            pqPicture!.CopyFrom(quote);
            pqServer.Publish(pqPicture);
        }
    }

    public void Dispose()
    {
        shutdownFlag = true;

        foreach (var pictureKvp in pictures)
        {
            var now = TimeContext.UtcNow;
            var picture = pictureKvp.Value;
            picture.ResetFields();
            picture.SourceTime = now;
            picture.ClientReceivedTime = now;
            if (picture is IMutableLevel1Quote pq1) pq1.AdapterSentTime = now;
            pqServer.Publish(picture);
        }

        pqServer.Dispose();
    }
}
