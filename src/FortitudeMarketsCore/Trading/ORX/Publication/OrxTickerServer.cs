#region

using FortitudeIO.Protocols.ORX.ClientServer;
using FortitudeMarketsCore.Trading.ORX.Session;

#endregion

namespace FortitudeMarketsCore.Trading.ORX.Publication;

public sealed class OrxTickerServer
{
    private readonly Dictionary<string, OrxTickerMessage> cache = new();
    private readonly OrxServerMessaging orxServerMessaging;

    public OrxTickerServer(OrxServerMessaging orxServerMessaging)
    {
        this.orxServerMessaging = orxServerMessaging;
        orxServerMessaging.RegisterSerializer<OrxTickerMessage>();
        orxServerMessaging.NewClient += cx =>
        {
            lock (cache)
            {
                foreach (var m in cache.Values) orxServerMessaging.Send(cx, m);
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
