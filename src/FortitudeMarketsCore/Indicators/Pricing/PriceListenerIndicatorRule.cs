// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeMarketsApi.Pricing;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing;

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
            (this, feedTickerListenAddress, ReceiveQuoteHandler);
    }

    public override async ValueTask StopAsync()
    {
        await tickerListenSubscription.NullSafeUnsubscribe();
    }

    protected virtual ValueTask ReceiveQuoteHandler(IBusMessage<TQuoteType> priceQuoteMessage) => ValueTask.CompletedTask;
}
