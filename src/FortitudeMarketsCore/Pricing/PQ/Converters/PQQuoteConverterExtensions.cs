// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.TimeSeries.BusRules;
using FortitudeMarketsCore.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Converters;

public static class PQQuoteConverterExtensions
{
    public static IChannelLimitedEventFactory<TQuoteLevel> CreateChannelFactory<TQuoteLevel>
        (this IListeningRule rule, Func<ChannelEvent<TQuoteLevel>, ValueTask<bool>> receiveQuoteHandler, ILimitedRecycler limitedRecycler)
        where TQuoteLevel : class, ITickInstant
    {
        var quoteLevelType = typeof(TQuoteLevel);

        switch (quoteLevelType.Name)
        {
            case nameof(ITickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ITickInstant, TickInstant>
                    (new InterQueueChannel<ITickInstant>(rule, (Func<ChannelEvent<ITickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new
                    ChannelWrappingLimitedEventFactory<IMutableTickInstant, TickInstant>
                    (new InterQueueChannel<IMutableTickInstant>(rule, (Func<ChannelEvent<IMutableTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(TickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<TickInstant>
                    (new InterQueueChannel<TickInstant>(rule, (Func<ChannelEvent<TickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQTickInstant, PQTickInstant>
                    (new InterQueueChannel<IPQTickInstant>(rule, (Func<ChannelEvent<IPQTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQTickInstant>
                    (new InterQueueChannel<PQTickInstant>(rule, (Func<ChannelEvent<PQTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel1Quote, Level1PriceQuote>
                    (new InterQueueChannel<ILevel1Quote>(rule, (Func<ChannelEvent<ILevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel1Quote, Level1PriceQuote>
                    (new InterQueueChannel<IMutableLevel1Quote>(rule, (Func<ChannelEvent<IMutableLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level1PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level1PriceQuote>
                    (new InterQueueChannel<Level1PriceQuote>(rule, (Func<ChannelEvent<Level1PriceQuote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel1Quote, PQLevel1Quote>
                    (new InterQueueChannel<IPQLevel1Quote>(rule, (Func<ChannelEvent<IPQLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel1Quote>
                    (new InterQueueChannel<PQLevel1Quote>(rule, (Func<ChannelEvent<PQLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel2Quote, Level2PriceQuote>
                    (new InterQueueChannel<ILevel2Quote>(rule, (Func<ChannelEvent<ILevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel2Quote, Level2PriceQuote>
                    (new InterQueueChannel<IMutableLevel2Quote>(rule, (Func<ChannelEvent<IMutableLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level2PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level2PriceQuote>
                    (new InterQueueChannel<Level2PriceQuote>(rule, (Func<ChannelEvent<Level2PriceQuote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel2Quote, PQLevel2Quote>
                    (new InterQueueChannel<IPQLevel2Quote>(rule, (Func<ChannelEvent<IPQLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel2Quote>
                    (new InterQueueChannel<PQLevel2Quote>(rule, (Func<ChannelEvent<PQLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel3Quote, Level3PriceQuote>
                    (new InterQueueChannel<ILevel3Quote>(rule, (Func<ChannelEvent<ILevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel3Quote, Level3PriceQuote>
                    (new InterQueueChannel<IMutableLevel3Quote>(rule, (Func<ChannelEvent<IMutableLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level3PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level3PriceQuote>
                    (new InterQueueChannel<Level3PriceQuote>(rule, (Func<ChannelEvent<Level3PriceQuote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel3Quote, PQLevel3Quote>
                    (new InterQueueChannel<IPQLevel3Quote>(rule, (Func<ChannelEvent<IPQLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel3Quote>
                    (new InterQueueChannel<PQLevel3Quote>(rule, (Func<ChannelEvent<PQLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
        }
        throw new Exception("Expected TQuoteLevel to be a known quoteType");
    }

    public static IChannelLimitedEventFactory<TQuoteLevel> CreateChannelFactory<TQuoteLevel>
        (this IListeningRule rule, Func<ChannelEvent<TQuoteLevel>, bool> receiveQuoteHandler, ILimitedRecycler limitedRecycler)
        where TQuoteLevel : class, ITickInstant
    {
        var quoteLevelType = typeof(TQuoteLevel);

        switch (quoteLevelType.Name)
        {
            case nameof(ITickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ITickInstant, TickInstant>
                    (new InterQueueChannel<ITickInstant>(rule, (Func<ChannelEvent<ITickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new
                    ChannelWrappingLimitedEventFactory<IMutableTickInstant, TickInstant>
                    (new InterQueueChannel<IMutableTickInstant>(rule, (Func<ChannelEvent<IMutableTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(TickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<TickInstant>
                    (new InterQueueChannel<TickInstant>(rule, (Func<ChannelEvent<TickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQTickInstant, PQTickInstant>
                    (new InterQueueChannel<IPQTickInstant>(rule, (Func<ChannelEvent<IPQTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQTickInstant>
                    (new InterQueueChannel<PQTickInstant>(rule, (Func<ChannelEvent<PQTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel1Quote, Level1PriceQuote>
                    (new InterQueueChannel<ILevel1Quote>(rule, (Func<ChannelEvent<ILevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel1Quote, Level1PriceQuote>
                    (new InterQueueChannel<IMutableLevel1Quote>(rule, (Func<ChannelEvent<IMutableLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level1PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level1PriceQuote>
                    (new InterQueueChannel<Level1PriceQuote>(rule, (Func<ChannelEvent<Level1PriceQuote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel1Quote, PQLevel1Quote>
                    (new InterQueueChannel<IPQLevel1Quote>(rule, (Func<ChannelEvent<IPQLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel1Quote>
                    (new InterQueueChannel<PQLevel1Quote>(rule, (Func<ChannelEvent<PQLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel2Quote, Level2PriceQuote>
                    (new InterQueueChannel<ILevel2Quote>(rule, (Func<ChannelEvent<ILevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel2Quote, Level2PriceQuote>
                    (new InterQueueChannel<IMutableLevel2Quote>(rule, (Func<ChannelEvent<IMutableLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level2PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level2PriceQuote>
                    (new InterQueueChannel<Level2PriceQuote>(rule, (Func<ChannelEvent<Level2PriceQuote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel2Quote, PQLevel2Quote>
                    (new InterQueueChannel<IPQLevel2Quote>(rule, (Func<ChannelEvent<IPQLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel2Quote>
                    (new InterQueueChannel<PQLevel2Quote>(rule, (Func<ChannelEvent<PQLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel3Quote, Level3PriceQuote>
                    (new InterQueueChannel<ILevel3Quote>(rule, (Func<ChannelEvent<ILevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel3Quote, Level3PriceQuote>
                    (new InterQueueChannel<IMutableLevel3Quote>(rule, (Func<ChannelEvent<IMutableLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level3PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level3PriceQuote>
                    (new InterQueueChannel<Level3PriceQuote>(rule, (Func<ChannelEvent<Level3PriceQuote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel3Quote, PQLevel3Quote>
                    (new InterQueueChannel<IPQLevel3Quote>(rule, (Func<ChannelEvent<IPQLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel3Quote>
                    (new InterQueueChannel<PQLevel3Quote>(rule, (Func<ChannelEvent<PQLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
        }
        throw new Exception("Expected TQuoteLevel to be a known quoteType");
    }

    public static ChannelPublishRequest<TQuoteLevel> ToChannelPublishRequest<TQuoteLevel>
        (this IChannel<TQuoteLevel> channelFactory, int resultLimit = -1, int batchSize = 1) where TQuoteLevel : class, ITickInstant =>
        new(channelFactory, resultLimit, batchSize);

    public static ChannelPublishRequest<TQuoteLevel> CreateChannelFactoryPublishRequest<TQuoteLevel>
    (this IListeningRule rule, Func<ChannelEvent<TQuoteLevel>, bool> receiveQuoteHandler, ILimitedRecycler limitedRecycler
      , int resultLimit = -1, int batchSize = 1) where TQuoteLevel : class, ITickInstant =>
        new(rule.CreateChannelFactory(receiveQuoteHandler, limitedRecycler), resultLimit, batchSize);

    public static HistoricalQuotesRequest<TQuoteLevel> ToHistoricalQuotesRequest<TQuoteLevel>
        (this ChannelPublishRequest<TQuoteLevel> channelRequest, SourceTickerIdentifier sourceTickerIdentifier, UnboundedTimeRange timeRange)
        where TQuoteLevel : class, ITimeSeriesEntry<TQuoteLevel>, ITickInstant =>
        new(sourceTickerIdentifier, channelRequest, timeRange);


    public static TickerDetailLevel GetQuoteLevel<TQuoteLevel>() where TQuoteLevel : ITickInstant
    {
        var quoteLevelType = typeof(TQuoteLevel);

        switch (quoteLevelType.Name)
        {
            case nameof(ITickInstant) or nameof(IMutableTickInstant) or nameof(TickInstant) or nameof(IPQTickInstant)
              or nameof(PQTickInstant):
                return TickerDetailLevel.SingleValue;
            case nameof(ILevel1Quote) or nameof(IMutableLevel1Quote) or nameof(Level1PriceQuote) or nameof(IPQLevel1Quote) or nameof(PQLevel1Quote):
                return TickerDetailLevel.Level1Quote;
            case nameof(ILevel2Quote) or nameof(IMutableLevel2Quote) or nameof(Level2PriceQuote) or nameof(IPQLevel2Quote) or nameof(PQLevel2Quote):
                return TickerDetailLevel.Level2Quote;
            case nameof(ILevel3Quote) or nameof(IMutableLevel3Quote) or nameof(Level3PriceQuote) or nameof(IPQLevel3Quote) or nameof(PQLevel3Quote):
                return TickerDetailLevel.Level3Quote;
        }
        throw new Exception("Expected TQuoteLevel to be a known quoteType");
    }

    public static bool IsPQQuoteType<TQuoteLevel>() where TQuoteLevel : class, ITickInstant
    {
        var quoteLevelType = typeof(TQuoteLevel);

        switch (quoteLevelType.Name)
        {
            case nameof(IPQTickInstant) or nameof(PQTickInstant): return true;
            case nameof(IPQLevel1Quote) or nameof(PQLevel1Quote): return true;
            case nameof(IPQLevel2Quote) or nameof(PQLevel2Quote): return true;
            case nameof(IPQLevel3Quote) or nameof(PQLevel3Quote): return true;
        }
        return false;
    }

    public static PQTickInstant ToL0PQQuote(this TickInstant vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQTickInstant>();
            borrowed.CopyFrom(vanillaQuote);
            return borrowed;
        }

        return new PQTickInstant(vanillaQuote);
    }

    public static PQLevel1Quote ToL1PQQuote(this Level1PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQLevel1Quote>();
            borrowed.CopyFrom(vanillaQuote);
            return borrowed;
        }

        return new PQLevel1Quote(vanillaQuote);
    }

    public static PQLevel2Quote ToL2PQQuote(this Level2PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQLevel2Quote>();
            borrowed.CopyFrom(vanillaQuote);
            return borrowed;
        }

        return new PQLevel2Quote(vanillaQuote);
    }

    public static PQLevel3Quote ToL3PQQuote(this Level3PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQLevel3Quote>();
            borrowed.CopyFrom(vanillaQuote);
            return borrowed;
        }

        return new PQLevel3Quote(vanillaQuote);
    }

    public static TickInstant ToL0PriceQuote(this TickInstant pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<TickInstant>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new TickInstant(pqQuote);
    }

    public static Level1PriceQuote ToL1PriceQuote(this PQLevel1Quote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<Level1PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new Level1PriceQuote(pqQuote);
    }

    public static Level2PriceQuote ToL2PriceQuote(this PQLevel2Quote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<Level2PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new Level2PriceQuote(pqQuote);
    }

    public static Level3PriceQuote ToL3PriceQuote(this PQLevel3Quote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<Level3PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new Level3PriceQuote(pqQuote);
    }
}
