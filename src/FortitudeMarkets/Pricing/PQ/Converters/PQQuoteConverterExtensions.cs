// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Routing.Channel;
using FortitudeBusRules.Rules;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.TimeSeries.BusRules;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Converters;

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
            case nameof(IPublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPublishableTickInstant, PublishableTickInstant>
                    (new InterQueueChannel<IPublishableTickInstant>(rule, (Func<ChannelEvent<IPublishableTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutablePublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new
                    ChannelWrappingLimitedEventFactory<IMutablePublishableTickInstant, PublishableTickInstant>
                    (new InterQueueChannel<IMutablePublishableTickInstant>(rule, (Func<ChannelEvent<IMutablePublishableTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(TickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<TickInstant>
                    (new InterQueueChannel<TickInstant>(rule, (Func<ChannelEvent<TickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PublishableTickInstant>
                    (new InterQueueChannel<PublishableTickInstant>(rule, (Func<ChannelEvent<PublishableTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQTickInstant, PQTickInstant>
                    (new InterQueueChannel<IPQTickInstant>(rule, (Func<ChannelEvent<IPQTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQPublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQPublishableTickInstant, PQPublishableTickInstant>
                    (new InterQueueChannel<IPQPublishableTickInstant>(rule, (Func<ChannelEvent<IPQPublishableTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQTickInstant>
                    (new InterQueueChannel<PQTickInstant>(rule, (Func<ChannelEvent<PQTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQPublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQPublishableTickInstant>
                    (new InterQueueChannel<PQPublishableTickInstant>(rule, (Func<ChannelEvent<PQPublishableTickInstant>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel1Quote, Level1PriceQuote>
                    (new InterQueueChannel<ILevel1Quote>(rule, (Func<ChannelEvent<ILevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel1Quote, Level1PriceQuote>
                    (new InterQueueChannel<IMutableLevel1Quote>(rule, (Func<ChannelEvent<IMutableLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPublishableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPublishableLevel1Quote, PublishableLevel1PriceQuote>
                    (new InterQueueChannel<IPublishableLevel1Quote>(rule, (Func<ChannelEvent<IPublishableLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutablePublishableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutablePublishableLevel1Quote, PublishableLevel1PriceQuote>
                    (new InterQueueChannel<IMutablePublishableLevel1Quote>(rule, (Func<ChannelEvent<IMutablePublishableLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level1PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level1PriceQuote>
                    (new InterQueueChannel<Level1PriceQuote>(rule, (Func<ChannelEvent<Level1PriceQuote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PublishableLevel1PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PublishableLevel1PriceQuote>
                    (new InterQueueChannel<PublishableLevel1PriceQuote>(rule, (Func<ChannelEvent<PublishableLevel1PriceQuote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel1Quote, PQLevel1Quote>
                    (new InterQueueChannel<IPQLevel1Quote>(rule, (Func<ChannelEvent<IPQLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQPublishableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQPublishableLevel1Quote, PQPublishableLevel1Quote>
                    (new InterQueueChannel<IPQPublishableLevel1Quote>(rule, (Func<ChannelEvent<IPQPublishableLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel1Quote>
                    (new InterQueueChannel<PQLevel1Quote>(rule, (Func<ChannelEvent<PQLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQPublishableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQPublishableLevel1Quote>
                    (new InterQueueChannel<PQPublishableLevel1Quote>(rule, (Func<ChannelEvent<PQPublishableLevel1Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel2Quote, Level2PriceQuote>
                    (new InterQueueChannel<ILevel2Quote>(rule, (Func<ChannelEvent<ILevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel2Quote, Level2PriceQuote>
                    (new InterQueueChannel<IMutableLevel2Quote>(rule, (Func<ChannelEvent<IMutableLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPublishableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPublishableLevel2Quote, PublishableLevel2PriceQuote>
                    (new InterQueueChannel<IPublishableLevel2Quote>(rule, (Func<ChannelEvent<IPublishableLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutablePublishableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutablePublishableLevel2Quote, PublishableLevel2PriceQuote>
                    (new InterQueueChannel<IMutablePublishableLevel2Quote>(rule, (Func<ChannelEvent<IMutablePublishableLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level2PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level2PriceQuote>
                    (new InterQueueChannel<Level2PriceQuote>(rule, (Func<ChannelEvent<Level2PriceQuote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PublishableLevel2PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PublishableLevel2PriceQuote>
                    (new InterQueueChannel<PublishableLevel2PriceQuote>(rule, (Func<ChannelEvent<PublishableLevel2PriceQuote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel2Quote, PQLevel2Quote>
                    (new InterQueueChannel<IPQLevel2Quote>(rule, (Func<ChannelEvent<IPQLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQPublishableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQPublishableLevel2Quote, PQPublishableLevel2Quote>
                    (new InterQueueChannel<IPQPublishableLevel2Quote>(rule, (Func<ChannelEvent<IPQPublishableLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel2Quote>
                    (new InterQueueChannel<PQLevel2Quote>(rule, (Func<ChannelEvent<PQLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQPublishableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQPublishableLevel2Quote>
                    (new InterQueueChannel<PQPublishableLevel2Quote>(rule, (Func<ChannelEvent<PQPublishableLevel2Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel3Quote, Level3PriceQuote>
                    (new InterQueueChannel<ILevel3Quote>(rule, (Func<ChannelEvent<ILevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel3Quote, Level3PriceQuote>
                    (new InterQueueChannel<IMutableLevel3Quote>(rule, (Func<ChannelEvent<IMutableLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPublishableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPublishableLevel3Quote, PublishableLevel3PriceQuote>
                    (new InterQueueChannel<IPublishableLevel3Quote>(rule, (Func<ChannelEvent<IPublishableLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutablePublishableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutablePublishableLevel3Quote, PublishableLevel3PriceQuote>
                    (new InterQueueChannel<IMutablePublishableLevel3Quote>(rule, (Func<ChannelEvent<IMutablePublishableLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level3PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level3PriceQuote>
                    (new InterQueueChannel<Level3PriceQuote>(rule, (Func<ChannelEvent<Level3PriceQuote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PublishableLevel3PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PublishableLevel3PriceQuote>
                    (new InterQueueChannel<PublishableLevel3PriceQuote>(rule, (Func<ChannelEvent<PublishableLevel3PriceQuote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel3Quote, PQLevel3Quote>
                    (new InterQueueChannel<IPQLevel3Quote>(rule, (Func<ChannelEvent<IPQLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQPublishableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQPublishableLevel3Quote, PQPublishableLevel3Quote>
                    (new InterQueueChannel<IPQPublishableLevel3Quote>(rule, (Func<ChannelEvent<IPQPublishableLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel3Quote>
                    (new InterQueueChannel<PQLevel3Quote>(rule, (Func<ChannelEvent<PQLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQPublishableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQPublishableLevel3Quote>
                    (new InterQueueChannel<PQPublishableLevel3Quote>(rule, (Func<ChannelEvent<PQPublishableLevel3Quote>, ValueTask<bool>>)(object)receiveQuoteHandler)
                   , limitedRecycler);
        }
        throw new Exception("Expected TQuoteLevel to be a known quoteType");
    }

    public static IChannelLimitedEventFactory<TQuoteLevel> CreateChannelFactory<TQuoteLevel>
        (this IListeningRule rule, Func<ChannelEvent<TQuoteLevel>, bool> receiveQuoteHandler, ILimitedRecycler limitedRecycler)
        where TQuoteLevel : class, IPublishableTickInstant
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
            case nameof(IPublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPublishableTickInstant, PublishableTickInstant>
                    (new InterQueueChannel<IPublishableTickInstant>(rule, (Func<ChannelEvent<IPublishableTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutablePublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new
                    ChannelWrappingLimitedEventFactory<IMutablePublishableTickInstant, PublishableTickInstant>
                    (new InterQueueChannel<IMutablePublishableTickInstant>(rule, (Func<ChannelEvent<IMutablePublishableTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(TickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<TickInstant>
                    (new InterQueueChannel<TickInstant>(rule, (Func<ChannelEvent<TickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PublishableTickInstant>
                    (new InterQueueChannel<PublishableTickInstant>(rule, (Func<ChannelEvent<PublishableTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQTickInstant, PQTickInstant>
                    (new InterQueueChannel<IPQTickInstant>(rule, (Func<ChannelEvent<IPQTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQPublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQPublishableTickInstant, PQPublishableTickInstant>
                    (new InterQueueChannel<IPQPublishableTickInstant>(rule, (Func<ChannelEvent<IPQPublishableTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQTickInstant>
                    (new InterQueueChannel<PQTickInstant>(rule, (Func<ChannelEvent<PQTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQPublishableTickInstant):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQPublishableTickInstant>
                    (new InterQueueChannel<PQPublishableTickInstant>(rule, (Func<ChannelEvent<PQPublishableTickInstant>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel1Quote, Level1PriceQuote>
                    (new InterQueueChannel<ILevel1Quote>(rule, (Func<ChannelEvent<ILevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel1Quote, Level1PriceQuote>
                    (new InterQueueChannel<IMutableLevel1Quote>(rule, (Func<ChannelEvent<IMutableLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPublishableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPublishableLevel1Quote, PublishableLevel1PriceQuote>
                    (new InterQueueChannel<IPublishableLevel1Quote>(rule, (Func<ChannelEvent<IPublishableLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutablePublishableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutablePublishableLevel1Quote, PublishableLevel1PriceQuote>
                    (new InterQueueChannel<IMutablePublishableLevel1Quote>(rule, (Func<ChannelEvent<IMutablePublishableLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level1PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level1PriceQuote>
                    (new InterQueueChannel<Level1PriceQuote>(rule, (Func<ChannelEvent<Level1PriceQuote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PublishableLevel1PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PublishableLevel1PriceQuote>
                    (new InterQueueChannel<PublishableLevel1PriceQuote>(rule, (Func<ChannelEvent<PublishableLevel1PriceQuote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel1Quote, PQLevel1Quote>
                    (new InterQueueChannel<IPQLevel1Quote>(rule, (Func<ChannelEvent<IPQLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQPublishableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQPublishableLevel1Quote, PQPublishableLevel1Quote>
                    (new InterQueueChannel<IPQPublishableLevel1Quote>(rule, (Func<ChannelEvent<IPQPublishableLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel1Quote>
                    (new InterQueueChannel<PQLevel1Quote>(rule, (Func<ChannelEvent<PQLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQPublishableLevel1Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQPublishableLevel1Quote>
                    (new InterQueueChannel<PQPublishableLevel1Quote>(rule, (Func<ChannelEvent<PQPublishableLevel1Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel2Quote, Level2PriceQuote>
                    (new InterQueueChannel<ILevel2Quote>(rule, (Func<ChannelEvent<ILevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel2Quote, Level2PriceQuote>
                    (new InterQueueChannel<IMutableLevel2Quote>(rule, (Func<ChannelEvent<IMutableLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPublishableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPublishableLevel2Quote, PublishableLevel2PriceQuote>
                    (new InterQueueChannel<IPublishableLevel2Quote>(rule, (Func<ChannelEvent<IPublishableLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutablePublishableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutablePublishableLevel2Quote, PublishableLevel2PriceQuote>
                    (new InterQueueChannel<IMutablePublishableLevel2Quote>(rule, (Func<ChannelEvent<IMutablePublishableLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level2PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level2PriceQuote>
                    (new InterQueueChannel<Level2PriceQuote>(rule, (Func<ChannelEvent<Level2PriceQuote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PublishableLevel2PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PublishableLevel2PriceQuote>
                    (new InterQueueChannel<PublishableLevel2PriceQuote>(rule, (Func<ChannelEvent<PublishableLevel2PriceQuote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel2Quote, PQLevel2Quote>
                    (new InterQueueChannel<IPQLevel2Quote>(rule, (Func<ChannelEvent<IPQLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQPublishableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQPublishableLevel2Quote, PQPublishableLevel2Quote>
                    (new InterQueueChannel<IPQPublishableLevel2Quote>(rule, (Func<ChannelEvent<IPQPublishableLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel2Quote>
                    (new InterQueueChannel<PQLevel2Quote>(rule, (Func<ChannelEvent<PQLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQPublishableLevel2Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQPublishableLevel2Quote>
                    (new InterQueueChannel<PQPublishableLevel2Quote>(rule, (Func<ChannelEvent<PQPublishableLevel2Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(ILevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<ILevel3Quote, Level3PriceQuote>
                    (new InterQueueChannel<ILevel3Quote>(rule, (Func<ChannelEvent<ILevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutableLevel3Quote, Level3PriceQuote>
                    (new InterQueueChannel<IMutableLevel3Quote>(rule, (Func<ChannelEvent<IMutableLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPublishableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPublishableLevel3Quote, PublishableLevel3PriceQuote>
                    (new InterQueueChannel<IPublishableLevel3Quote>(rule, (Func<ChannelEvent<IPublishableLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IMutablePublishableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IMutablePublishableLevel3Quote, PublishableLevel3PriceQuote>
                    (new InterQueueChannel<IMutablePublishableLevel3Quote>(rule, (Func<ChannelEvent<IMutablePublishableLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(Level3PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<Level3PriceQuote>
                    (new InterQueueChannel<Level3PriceQuote>(rule, (Func<ChannelEvent<Level3PriceQuote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PublishableLevel3PriceQuote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PublishableLevel3PriceQuote>
                    (new InterQueueChannel<PublishableLevel3PriceQuote>(rule, (Func<ChannelEvent<PublishableLevel3PriceQuote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQLevel3Quote, PQLevel3Quote>
                    (new InterQueueChannel<IPQLevel3Quote>(rule, (Func<ChannelEvent<IPQLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(IPQPublishableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<IPQPublishableLevel3Quote, PQPublishableLevel3Quote>
                    (new InterQueueChannel<IPQPublishableLevel3Quote>(rule, (Func<ChannelEvent<IPQPublishableLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQLevel3Quote>
                    (new InterQueueChannel<PQLevel3Quote>(rule, (Func<ChannelEvent<PQLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
            case nameof(PQPublishableLevel3Quote):
                return (IChannelLimitedEventFactory<TQuoteLevel>)new ChannelWrappingLimitedEventFactory<PQPublishableLevel3Quote>
                    (new InterQueueChannel<PQPublishableLevel3Quote>(rule, (Func<ChannelEvent<PQPublishableLevel3Quote>, bool>)(object)receiveQuoteHandler)
                   , limitedRecycler);
        }
        throw new Exception("Expected TQuoteLevel to be a known quoteType");
    }

    public static ChannelPublishRequest<TQuoteLevel> ToChannelPublishRequest<TQuoteLevel>
        (this IChannel<TQuoteLevel> channelFactory, int resultLimit = -1, int batchSize = 1) where TQuoteLevel : class, IPublishableTickInstant =>
        new(channelFactory, resultLimit, batchSize);

    public static ChannelPublishRequest<TQuoteLevel> CreateChannelFactoryPublishRequest<TQuoteLevel>
    (this IListeningRule rule, Func<ChannelEvent<TQuoteLevel>, bool> receiveQuoteHandler, ILimitedRecycler limitedRecycler
      , int resultLimit = -1, int batchSize = 1) where TQuoteLevel : class, IPublishableTickInstant =>
        new(rule.CreateChannelFactory(receiveQuoteHandler, limitedRecycler), resultLimit, batchSize);

    public static HistoricalQuotesRequest<TQuoteLevel> ToHistoricalQuotesRequest<TQuoteLevel>
    (this ChannelPublishRequest<TQuoteLevel> channelRequest, SourceTickerIdentifier sourceTickerIdentifier
      , UnboundedTimeRange? timeRange = null, bool inReverseChronologicalOrder = false)
        where TQuoteLevel : class, ITimeSeriesEntry, IPublishableTickInstant =>
        new(sourceTickerIdentifier, channelRequest, timeRange, inReverseChronologicalOrder);


    public static TickerQuoteDetailLevel GetQuoteLevel<TQuoteLevel>() where TQuoteLevel : IPublishableTickInstant
    {
        var quoteLevelType = typeof(TQuoteLevel);

        switch (quoteLevelType.Name)
        {
            case nameof(IPublishableTickInstant) or nameof(IMutablePublishableTickInstant) or nameof(TickInstant) or nameof(IPQPublishableTickInstant)
              or nameof(PQPublishableTickInstant):
                return TickerQuoteDetailLevel.SingleValue;
            case nameof(IPublishableLevel1Quote) or nameof(IMutablePublishableLevel1Quote) or nameof(PublishableLevel1PriceQuote) or nameof(IPQPublishableLevel1Quote) or nameof(PQPublishableLevel1Quote):
                return TickerQuoteDetailLevel.Level1Quote;
            case nameof(IPublishableLevel2Quote) or nameof(IMutablePublishableLevel2Quote) or nameof(PublishableLevel2PriceQuote) or nameof(IPQPublishableLevel2Quote) or nameof(PQPublishableLevel2Quote):
                return TickerQuoteDetailLevel.Level2Quote;
            case nameof(IPublishableLevel3Quote) or nameof(IMutablePublishableLevel3Quote) or nameof(PublishableLevel3PriceQuote) or nameof(IPQPublishableLevel3Quote) or nameof(PQPublishableLevel3Quote):
                return TickerQuoteDetailLevel.Level3Quote;
        }
        throw new Exception("Expected TQuoteLevel to be a known quoteType");
    }

    public static bool IsPQQuoteType<TQuoteLevel>() where TQuoteLevel : class, IPublishableTickInstant
    {
        var quoteLevelType = typeof(TQuoteLevel);

        switch (quoteLevelType.Name)
        {
            case nameof(IPQPublishableTickInstant) or nameof(PQPublishableTickInstant): return true;
            case nameof(IPQPublishableLevel1Quote) or nameof(PQPublishableLevel1Quote): return true;
            case nameof(IPQPublishableLevel2Quote) or nameof(PQPublishableLevel2Quote): return true;
            case nameof(IPQPublishableLevel3Quote) or nameof(PQPublishableLevel3Quote): return true;
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

    public static PQPublishableTickInstant ToPublishableL0PQQuote(this PublishableTickInstant vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQPublishableTickInstant>();
            borrowed.CopyFrom(vanillaQuote, CopyMergeFlags.Default);
            return borrowed;
        }

        return new PQPublishableTickInstant(vanillaQuote);
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

    public static PQPublishableLevel1Quote ToPublishableL1PQQuote(this PublishableLevel1PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQPublishableLevel1Quote>();
            borrowed.CopyFrom(vanillaQuote, CopyMergeFlags.Default);
            return borrowed;
        }

        return new PQPublishableLevel1Quote(vanillaQuote);
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

    public static PQPublishableLevel2Quote ToPublishableL2PQQuote(this PublishableLevel2PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQPublishableLevel2Quote>();
            borrowed.CopyFrom(vanillaQuote, CopyMergeFlags.Default);
            return borrowed;
        }

        return new PQPublishableLevel2Quote(vanillaQuote);
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

    public static PQPublishableLevel3Quote ToPublishableL3PQQuote(this PublishableLevel3PriceQuote vanillaQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PQPublishableLevel3Quote>();
            borrowed.CopyFrom(vanillaQuote);
            return borrowed;
        }

        return new PQPublishableLevel3Quote(vanillaQuote);
    }

    public static TickInstant ToL0PriceQuote(this PQTickInstant pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<TickInstant>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new TickInstant(pqQuote);
    }

    public static PublishableTickInstant ToPublishableL0PriceQuote(this PQPublishableTickInstant pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PublishableTickInstant>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new PublishableTickInstant(pqQuote);
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

    public static PublishableLevel1PriceQuote ToPublishableL1PriceQuote(this PQPublishableLevel1Quote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PublishableLevel1PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new PublishableLevel1PriceQuote(pqQuote);
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

    public static PublishableLevel2PriceQuote ToPublishableL2PriceQuote(this PQPublishableLevel2Quote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PublishableLevel2PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new PublishableLevel2PriceQuote(pqQuote);
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

    public static PublishableLevel3PriceQuote ToPublishableL3PriceQuote(this PQPublishableLevel3Quote pqQuote, IRecycler? recycler = null)
    {
        if (recycler != null)
        {
            var borrowed = recycler.Borrow<PublishableLevel3PriceQuote>();
            borrowed.CopyFrom(pqQuote);
            return borrowed;
        }

        return new PublishableLevel3PriceQuote(pqQuote);
    }
}
