﻿#region

using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeMarkets.Trading.ORX.Session;

#endregion

namespace FortitudeMarkets.Trading.ORX.Publication;

public sealed class OrxTickerServer
{
    private readonly Dictionary<string, OrxTickerMessage> cache = new();
    private readonly OrxServerMessaging orxServerMessaging;

    public OrxTickerServer(OrxServerMessaging orxServerMessaging)
    {
        this.orxServerMessaging = orxServerMessaging;
        orxServerMessaging.SerializationRepository.RegisterSerializer<OrxTickerMessage>();
        orxServerMessaging.NewClient += cx =>
        {
            lock (cache)
            {
                foreach (var m in cache.Values) cx.Send(m);
            }
        };
    }

    public void OnTickerUpdate(string exchange, string ticker,
        long contractSize, long minSize, long sizeInc, long maxSize,
        decimal priceInc,
        uint mql)
    {
        OrxTickerMessage? m;
        lock (cache)
        {
            if (!cache.TryGetValue(exchange + ":" + ticker, out m))
                cache[exchange + ":" + ticker] = m = new OrxTickerMessage(exchange, ticker);
        }

        m.UpdateData(contractSize, minSize, sizeInc, maxSize, priceInc, mql);
        orxServerMessaging.Broadcast(m);
    }

    public void OnTickerUpdate(string exchange, string ticker, bool tradable)
    {
        OrxTickerMessage? m;
        lock (cache)
        {
            cache.TryGetValue(exchange + ":" + ticker, out m);
        }

        if (m != null)
        {
            m.UpdateStatus(tradable);
            orxServerMessaging.Broadcast(m);
        }
    }
}
