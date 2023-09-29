using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using FortitudeCommon.AsyncProcessing;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Subscription
{
    public class PQTickerFeedSubscriptionQuoteStream<T> : PQTickerFeedSubscription, IObserver<T>,
        IPQTickerFeedSubscriptionQuoteStream<T> where T : class, IPQLevel0Quote
    {
        IList<IObserver<T>> observers = new List<IObserver<T>>();
        private readonly ISyncLock quoteLockLight;
        private Exception lastException;
        private IDisposable addActionOnUnsubscribe;

        public PQTickerFeedSubscriptionQuoteStream(ISnapshotUpdatePricingServerConfig feedServerConfig, 
            ISourceTickerQuoteInfo sourceTickerQuoteInfo, T publishedQuote)
            : base(feedServerConfig, sourceTickerQuoteInfo)
        {
            quoteLockLight = publishedQuote.Lock;
        }
        
        public void OnNext(T value)
        {
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].OnNext(value);
            }
        }

        public void OnError(Exception error)
        {
            lastException = error;
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].OnError(error);
            }
            observers.Clear();
        }

        public void OnCompleted()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                observers[i].OnCompleted();
            }
            observers = null;
        }

        public void AddCleanupAction(IDisposable underlyingSubscription)
        {
            addActionOnUnsubscribe = new CompositeDisposable(underlyingSubscription ?? Disposable.Empty, 
                addActionOnUnsubscribe ?? Disposable.Empty);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (lastException != null)
            {
                observer.OnError(lastException);
            }
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

        private void CheckAnyObserversLeft()
        {
            if (observers.Count == 0)
            {
                addActionOnUnsubscribe?.Dispose();
                addActionOnUnsubscribe = null;
            }
        }
    }
}