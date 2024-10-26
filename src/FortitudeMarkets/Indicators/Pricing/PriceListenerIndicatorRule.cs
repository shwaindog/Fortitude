// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeMarkets.Pricing;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.PQ.Subscription.BusRules;

#endregion

namespace FortitudeMarkets.Indicators.Pricing;

public class PriceListenerIndicatorRule<TQuoteType> : Rule
    where TQuoteType : class, ITickInstant
{
    private readonly string feedTickerListenAddress;

    protected SourceTickerIdentifier SourceTickerIdentifier;

    private ISubscription? tickerListenSubscription;

    public PriceListenerIndicatorRule(SourceTickerIdentifier sourceTickerIdentifier, string ruleName) : base(ruleName)
    {
        SourceTickerIdentifier = sourceTickerIdentifier;

        feedTickerListenAddress = sourceTickerIdentifier.Source.SubscribeToTickerQuotes(sourceTickerIdentifier.Ticker);
    }

    public override async ValueTask StartAsync()
    {
        tickerListenSubscription = await Context.MessageBus.RegisterListenerAsync<TQuoteType>
            (this, feedTickerListenAddress, ReceiveQuoteMessageHandler);
    }

    public override async ValueTask StopAsync()
    {
        await tickerListenSubscription.NullSafeUnsubscribe();
    }

    protected virtual ValueTask ReceiveQuoteMessageHandler
        (IBusMessage<TQuoteType> priceQuoteMessage) =>
        ReceiveQuoteHandler(priceQuoteMessage.Payload.Body());

    protected virtual ValueTask ReceiveQuoteHandler(TQuoteType priceQuote) => ValueTask.CompletedTask;
}
