// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeMarketsApi.Configuration.ClientServerConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IQuotePublisher<in T> : IDisposable where T : ITickInstant
{
    void PublishReset(string ticker, DateTime exchangeTs, DateTime exchangeSentTs, DateTime adapterRecvTs);
    void PublishQuoteUpdate(T quote);
}

public interface IPQPublisher : IQuotePublisher<ITickInstant>
{
    void RegisterTickersWithServer(IMarketConnectionConfig marketConnectionConfig);

    void SetNextSequenceNumberToZero(string ticker);

    void SetNextSequenceNumberToFullUpdate(string ticker);

    void PublishQuoteUpdateAs(ITickInstant quote, PQMessageFlags? withMessageFlags = null);
}

public class PQPublisher<T> : IPQPublisher where T : IPQTickInstant
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPublisher<>));

    private readonly IMap<string, T> pictures = new ConcurrentMap<string, T>();
    private readonly IPQServer<T>    pqServer;
    private volatile bool            shutdownFlag;

    public PQPublisher(IPQServer<T> pqServer) => this.pqServer = pqServer;

    public void RegisterTickersWithServer(IMarketConnectionConfig marketConnectionConfig)
    {
        pqServer.StartServices();

        if (!shutdownFlag)
            foreach (var tickerRef in marketConnectionConfig.AllSourceTickerInfos)
            {
                var picture = pqServer.Register(tickerRef.Ticker);
                if (picture != null) pictures.AddOrUpdate(tickerRef.Ticker, picture);
            }
    }

    public void SetNextSequenceNumberToZero(string ticker)
    {
        pqServer.SetNextSequenceNumberToZero(ticker);
    }

    public void SetNextSequenceNumberToFullUpdate(string ticker)
    {
        pqServer.SetNextSequenceNumberToFullUpdate(ticker);
    }

    public void Dispose()
    {
        shutdownFlag = true;

        foreach (var pictureKvp in pictures)
        {
            var now     = TimeContext.UtcNow;
            var picture = pictureKvp.Value;
            picture.ResetFields();
            picture.SourceTime         = now;
            picture.ClientReceivedTime = now;
            if (picture is IMutableLevel1Quote pq1) pq1.AdapterSentTime = now;
            pqServer.Publish(picture);
        }

        pqServer.Dispose();
    }

    public void PublishReset(string ticker, DateTime exchangeTs, DateTime exchangeSentTs, DateTime adapterRecvTs)
    {
        if (pictures.TryGetValue(ticker, out var pqPicture))
        {
            pqPicture!.ResetFields();
            pqPicture.SourceTime         = exchangeTs;
            pqPicture.ClientReceivedTime = adapterRecvTs;
            if (pqPicture is IMutableLevel1Quote pq1) pq1.AdapterSentTime = exchangeSentTs;
            pqServer.Publish(pqPicture);
        }
    }

    public void PublishQuoteUpdateAs(ITickInstant quote, PQMessageFlags? withMessageFlags = null)
    {
        if (pictures.TryGetValue(quote.SourceTickerInfo!.Ticker, out var pqPicture))
        {
            // logger.Info("About to publish quote: {0}", quote);
            pqPicture!.CopyFrom(quote);
            pqPicture.OverrideSerializationFlags = withMessageFlags;
            pqServer.Publish(pqPicture);
            pqPicture.OverrideSerializationFlags = null;
        }
    }

    public void PublishQuoteUpdate(ITickInstant quote)
    {
        PublishQuoteUpdateAs(quote);
    }

    public void RegisterTickersWithServer(ISourceTickerInfo sourceTickerInfo)
    {
        pqServer.StartServices();

        if (!shutdownFlag)
        {
            var picture = pqServer.Register(sourceTickerInfo.Ticker);
            if (picture != null) pictures.AddOrUpdate(sourceTickerInfo.Ticker, picture);
        }
    }
}
