// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeBusRules.BusMessaging.Messages.ListeningSubscriptions;
using FortitudeBusRules.Messages;
using FortitudeBusRules.Rules;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Subscription.BusRules;

#endregion

namespace FortitudeMarketsCore.Indicators.Pricing;

public class PriceListenerIndicatorRule<TQuoteType> : Rule
    where TQuoteType : class, ILevel0Quote
{
    private readonly string feedTickerListenAddress;

    protected ISourceTickerIdentifier TickerId;

    private ISubscription tickerListenSubscription = null!;

    public PriceListenerIndicatorRule(ISourceTickerIdentifier tickerId, string ruleName) : base(ruleName)
    {
        TickerId = tickerId;

        feedTickerListenAddress = tickerId.Source.SubscribeToTickerQuotes(tickerId.Ticker);
    }

    public override async ValueTask StartAsync()
    {
        tickerListenSubscription = await Context.MessageBus.RegisterListenerAsync<TQuoteType>
            (this, feedTickerListenAddress, ReceiveQuoteHandler);
    }

    public override async ValueTask StopAsync()
    {
        await tickerListenSubscription.UnsubscribeAsync();
    }

    protected virtual ValueTask ReceiveQuoteHandler(IBusMessage<TQuoteType> priceQuoteMessage) => ValueTask.CompletedTask;
}
