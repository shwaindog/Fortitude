// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Reactive.Disposables;
using FortitudeCommon.AsyncProcessing;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Subscription.Standalone;

public interface IPQTickerFeedSubscriptionQuoteStream<out T> : IPQTickerFeedSubscription, IObservable<T>
    where T : class, ILevel0Quote { }

public class PQTickerFeedSubscriptionQuoteStream<T> : PQTickerFeedSubscription, IObserver<T>,
    IPQTickerFeedSubscriptionQuoteStream<T> where T : class, IPQLevel0Quote
{
    private readonly ISyncLock quoteLockLight;

    private IDisposable? addActionOnUnsubscribe;
    private Exception?   lastException;

    private IList<IObserver<T>>? observers = new List<IObserver<T>>();

    public PQTickerFeedSubscriptionQuoteStream
    (IPricingServerConfig feedServerConfig,
        ISourceTickerQuoteInfo sourceTickerQuoteInfo, T publishedQuote)
        : base(feedServerConfig, sourceTickerQuoteInfo) =>
        quoteLockLight = publishedQuote.Lock;

    public void OnNext(T value)
    {
        for (var i = 0; i < (observers?.Count ?? 0); i++) observers![i].OnNext(value);
    }

    public void OnError(Exception error)
    {
        lastException = error;
        for (var i = 0; i < (observers?.Count ?? 0); i++) observers![i].OnError(error);
        observers?.Clear();
    }

    public void OnCompleted()
    {
        for (var i = 0; i < (observers?.Count ?? 0); i++) observers![i].OnCompleted();
        observers = null;
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        if (lastException != null) observer.OnError(lastException);
        if (observers == null)
        {
            observer.OnCompleted();
            return Disposable.Empty;
        }

        quoteLockLight.Acquire();
        try
        {
            observers.Add(observer);
        }
        finally
        {
            quoteLockLight.Release();
        }

        return Disposable.Create(() =>
        {
            quoteLockLight.Acquire();
            try
            {
                observers.Remove(observer);
                CheckAnyObserversLeft();
            }
            finally
            {
                quoteLockLight.Release();
            }
        });
    }

    public override void Unsubscribe()
    {
        addActionOnUnsubscribe?.Dispose();
        addActionOnUnsubscribe = null;
    }

    public void AddCleanupAction(IDisposable underlyingSubscription)
    {
        addActionOnUnsubscribe = new CompositeDisposable
            (underlyingSubscription, addActionOnUnsubscribe ?? Disposable.Empty);
    }

    private void CheckAnyObserversLeft()
    {
        if ((observers?.Count ?? 0) == 0)
        {
            addActionOnUnsubscribe?.Dispose();
            addActionOnUnsubscribe = null;
        }
    }
}
