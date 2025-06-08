// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.Monitoring.Logging;
using FortitudeMarkets.Configuration.ClientServerConfig;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents;
#endregion

namespace FortitudeMarkets.Pricing.PQ.Publication;

public interface IQuotePublisher<in T> : IDisposable where T : IMutablePublishableTickInstant
{
    void PublishReset(string ticker, DateTime exchangeTs, DateTime exchangeSentTs, DateTime adapterRecvTs);
    void PublishQuoteUpdate(T quote);
}

public interface IPQPublisher : IQuotePublisher<IPQPublishableTickInstant>
{
    void RegisterStreamWithServer(IMarketConnectionConfig marketConnectionConfig);

    void SetNextSequenceNumberToZero(string ticker);

    void SetNextSequenceNumberToFullUpdate(string ticker);

    void PublishQuoteUpdateAs(IPQPublishableTickInstant quote, PQMessageFlags? withMessageFlags = null);
}

public interface IPQMessagePublisher
{
    void PublishMessageUpdateAs(IPQMessage quote, PQMessageFlags? withMessageFlags = null);
    void PublishMessageUpdate(IPQMessage quote);
}

public class PQPublisher<T> : IPQPublisher where T : IPQMessage
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQPublisher<>));

    private readonly IMap<string, T> pictures = new ConcurrentMap<string, T>();
    private readonly IPQServer<T>    pqServer;
    private volatile bool            shutdownFlag;

    public PQPublisher(IPQServer<T> pqServer) => this.pqServer = pqServer;

    public void RegisterStreamWithServer(IMarketConnectionConfig marketConnectionConfig)
    {
        pqServer.StartServices();

        if (!shutdownFlag)
            foreach (var tickerRef in marketConnectionConfig.AllSourceTickerInfos)
            {
                var picture = pqServer.Register(tickerRef.InstrumentName);
                if (picture != null) pictures.AddOrUpdate(tickerRef.InstrumentName, picture);
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
            picture.ResetWithTracking();
            picture.SetPublisherStateToConnectivityStatus(PublisherStates.DisconnectionImmanent, now);
            picture.ClientReceivedTime = now;
            picture.HasUpdates         = true;
            if (picture is IMutablePublishableLevel1Quote pq1) pq1.AdapterSentTime = now;
            pqServer.Publish(picture);
        }

        pqServer.Dispose();
    }

    public void PublishReset(string ticker, DateTime exchangeTs, DateTime exchangeSentTs, DateTime adapterRecvTs)
    {
        if (pictures.TryGetValue(ticker, out var pqPicture))
        {
            pqPicture!.ResetWithTracking();
            pqPicture.SetPublisherStateToConnectivityStatus(PublisherStates.DisconnectionImmanent, exchangeTs);
            pqPicture.ClientReceivedTime = adapterRecvTs;
            pqPicture.HasUpdates         = true;
            if (pqPicture is IMutablePublishableLevel1Quote pq1) pq1.AdapterSentTime = exchangeSentTs;
            pqServer.Publish(pqPicture);
        }
    }

    public void PublishQuoteUpdate(IMutablePublishableTickInstant quote)
    {
        PublishQuoteUpdateAs(quote);
    }

    public void PublishMessageUpdateAs(IPQMessage quote, PQMessageFlags? withMessageFlags = null)
    {
        if (pictures.TryGetValue(quote.StreamName, out var pqPicture))
        {
            if (!quote.FeedMarketConnectivityStatus.HasIsAdapterReplay())
            {
                quote.TriggerTimeUpdates(TimeContext.UtcNow);
            }
            // logger.Info("About to publish quote: {0}", quote);
            pqPicture!.CopyFrom(quote);
            if (withMessageFlags.HasCompleteFlag() || withMessageFlags.HasSnapshotFlag())
            {
                pqPicture.HasUpdates = true;
            }
            pqPicture.OverrideSerializationFlags = withMessageFlags;
            pqServer.Publish(pqPicture);
            pqPicture.OverrideSerializationFlags = null;
        }
    }

    public void PublishQuoteUpdateAs(IMutablePublishableTickInstant quote, PQMessageFlags? withMessageFlags = null)
    {
        if (pictures.TryGetValue(quote.SourceTickerInfo!.InstrumentName, out var pqPicture))
        {
            if (!quote.FeedMarketConnectivityStatus.HasIsAdapterReplay())
            {
                quote.TriggerTimeUpdates(TimeContext.UtcNow);
            }
            // logger.Info("About to publish quote: {0}", quote);
            pqPicture!.CopyFrom(quote);
            if (withMessageFlags.HasCompleteFlag() || withMessageFlags.HasSnapshotFlag())
            {
                pqPicture.HasUpdates = true;
            }
            pqPicture.OverrideSerializationFlags = withMessageFlags;
            pqServer.Publish(pqPicture);
            pqPicture.OverrideSerializationFlags = null;
        }
    }

    public void PublishQuoteUpdate(IPQPublishableTickInstant quote)
    {
        PublishQuoteUpdateAs(quote);
    }

    public void PublishQuoteUpdateAs(IPQPublishableTickInstant quote, PQMessageFlags? withMessageFlags = null)
    {
        if (pictures.TryGetValue(quote.SourceTickerInfo!.InstrumentName, out var pqPicture))
        {
            if (!quote.FeedMarketConnectivityStatus.HasIsAdapterReplay())
            {
                quote.TriggerTimeUpdates(TimeContext.UtcNow);
            }
            // logger.Info("About to publish quote: {0}", quote);
            pqPicture!.CopyFrom(quote);
            if (withMessageFlags.HasCompleteFlag() || withMessageFlags.HasSnapshotFlag())
            {
                pqPicture.HasUpdates = true;
            }
            pqPicture.OverrideSerializationFlags = withMessageFlags;
            pqServer.Publish(pqPicture);
            pqPicture.OverrideSerializationFlags = null;
        }
    }

    public void PublishMessageUpdate(IPQMessage quote)
    {
        PublishMessageUpdateAs(quote);
    }

    public void RegisterTickersWithServer(ISourceTickerInfo sourceTickerInfo)
    {
        pqServer.StartServices();

        if (!shutdownFlag)
        {
            var picture = pqServer.Register(sourceTickerInfo.InstrumentName);
            if (picture != null) pictures.AddOrUpdate(sourceTickerInfo.InstrumentName, picture);
        }
    }
}
