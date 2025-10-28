// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

public static class PQQuoteExtensions
{
    public static PQPublishableTickInstant MostCompactPQInstance(this ISourceTickerInfo srcTickerInfo, IRecycler? recycler = null)
    {
        var quoteLevel = srcTickerInfo.MostCompactQuoteLevel();
        switch (quoteLevel)
        {
            case TickerQuoteDetailLevel.SingleValue:
                return recycler?.Borrow<PQPublishableTickInstant>().SetSourceTickerInfo(srcTickerInfo) ??
                       new PQPublishableTickInstant(srcTickerInfo);
            case TickerQuoteDetailLevel.Level1Quote:
                return recycler?.Borrow<PQPublishableLevel1Quote>().SetSourceTickerInfo(srcTickerInfo) ?? new PQPublishableLevel1Quote(srcTickerInfo);
            case TickerQuoteDetailLevel.Level2Quote: return new PQPublishableLevel2Quote(srcTickerInfo);

            default: return new PQPublishableLevel3Quote(srcTickerInfo);
        }
    }

    public static PQPublishableTickInstant PublishedTypePQInstance(this ISourceTickerInfo srcTickerInfo, IRecycler? recycler = null)
    {
        var quoteLevel = srcTickerInfo.PublishedTickerQuoteDetailLevel;
        switch (quoteLevel)
        {
            case TickerQuoteDetailLevel.SingleValue: return new PQPublishableTickInstant(srcTickerInfo);
            case TickerQuoteDetailLevel.Level1Quote: return new PQPublishableLevel1Quote(srcTickerInfo);
            case TickerQuoteDetailLevel.Level2Quote: return new PQPublishableLevel2Quote(srcTickerInfo);

            default: return new PQPublishableLevel3Quote(srcTickerInfo);
        }
    }

    public static Func<PQPublishableTickInstant> MostRestrictivePQInstanceFactory
    (this ISourceTickerInfo srcTickerInfo, TickerQuoteDetailLevel upperBound
      , IRecycler? recycler = null)
    {
        var srcTickerQuoteLevel = srcTickerInfo.PublishedTickerQuoteDetailLevel;
        var quoteLevel          = (TickerQuoteDetailLevel)Math.Min((byte)srcTickerQuoteLevel, (byte)upperBound);
        switch (quoteLevel)
        {
            case TickerQuoteDetailLevel.SingleValue: return () => new PQPublishableTickInstant(srcTickerInfo);
            case TickerQuoteDetailLevel.Level1Quote: return () => new PQPublishableLevel1Quote(srcTickerInfo);
            case TickerQuoteDetailLevel.Level2Quote: return () => new PQPublishableLevel2Quote(srcTickerInfo);

            default: return () => new PQPublishableLevel3Quote(srcTickerInfo);
        }
    }

    public static Func<PQPublishableTickInstant> LeastRestrictivePQInstanceFactory
    (this ISourceTickerInfo srcTickerInfo, TickerQuoteDetailLevel lowerBound
      , IRecycler? recycler = null)
    {
        var srcTickerQuoteLevel = srcTickerInfo.PublishedTickerQuoteDetailLevel;
        var quoteLevel          = (TickerQuoteDetailLevel)Math.Max((byte)srcTickerQuoteLevel, (byte)lowerBound);
        switch (quoteLevel)
        {
            case TickerQuoteDetailLevel.SingleValue: return () => new PQPublishableTickInstant(srcTickerInfo);
            case TickerQuoteDetailLevel.Level1Quote: return () => new PQPublishableLevel1Quote(srcTickerInfo);
            case TickerQuoteDetailLevel.Level2Quote: return () => new PQPublishableLevel2Quote(srcTickerInfo);

            default: return () => new PQPublishableLevel3Quote(srcTickerInfo);
        }
    }

    public static Func<PQPublishableTickInstant> PublishedTypePQInstanceFactory
        (this ISourceTickerInfo srcTickerInfo, IRecycler? recycler = null)
    {
        var quoteLevel = srcTickerInfo.PublishedTickerQuoteDetailLevel;
        switch (quoteLevel)
        {
            case TickerQuoteDetailLevel.SingleValue: return () => new PQPublishableTickInstant(srcTickerInfo);
            case TickerQuoteDetailLevel.Level1Quote: return () => new PQPublishableLevel1Quote(srcTickerInfo);
            case TickerQuoteDetailLevel.Level2Quote: return () => new PQPublishableLevel2Quote(srcTickerInfo);

            default: return () => new PQPublishableLevel3Quote(srcTickerInfo);
        }
    }
}
