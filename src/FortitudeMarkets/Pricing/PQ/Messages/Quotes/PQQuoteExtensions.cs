// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes;

public static class PQQuoteExtensions
{
    public static PQTickInstant MostCompactPQInstance(this ISourceTickerInfo srcTickerInfo, IRecycler? recycler = null)
    {
        var quoteLevel = srcTickerInfo.MostCompactQuoteLevel();
        switch (quoteLevel)
        {
            case TickerDetailLevel.SingleValue:
                return recycler?.Borrow<PQTickInstant>()?.SetSourceTickerInfo(srcTickerInfo) ??
                       new PQTickInstant(srcTickerInfo);
            case TickerDetailLevel.Level1Quote:
                return recycler?.Borrow<PQLevel1Quote>()?.SetSourceTickerInfo(srcTickerInfo) ?? new PQLevel1Quote(srcTickerInfo);
            case TickerDetailLevel.Level2Quote: return new PQLevel2Quote(srcTickerInfo);

            default: return new PQLevel3Quote(srcTickerInfo);
        }
    }

    public static PQTickInstant PublishedTypePQInstance(this ISourceTickerInfo srcTickerInfo, IRecycler? recycler = null)
    {
        var quoteLevel = srcTickerInfo.PublishedTickerDetailLevel;
        switch (quoteLevel)
        {
            case TickerDetailLevel.SingleValue: return new PQTickInstant(srcTickerInfo);
            case TickerDetailLevel.Level1Quote: return new PQLevel1Quote(srcTickerInfo);
            case TickerDetailLevel.Level2Quote: return new PQLevel2Quote(srcTickerInfo);

            default: return new PQLevel3Quote(srcTickerInfo);
        }
    }

    public static Func<PQTickInstant> MostRestrictivePQInstanceFactory
    (this ISourceTickerInfo srcTickerInfo, TickerDetailLevel upperBound
      , IRecycler? recycler = null)
    {
        var srcTickerQuoteLevel = srcTickerInfo.PublishedTickerDetailLevel;
        var quoteLevel          = (TickerDetailLevel)Math.Min((byte)srcTickerQuoteLevel, (byte)upperBound);
        switch (quoteLevel)
        {
            case TickerDetailLevel.SingleValue: return () => new PQTickInstant(srcTickerInfo);
            case TickerDetailLevel.Level1Quote: return () => new PQLevel1Quote(srcTickerInfo);
            case TickerDetailLevel.Level2Quote: return () => new PQLevel2Quote(srcTickerInfo);

            default: return () => new PQLevel3Quote(srcTickerInfo);
        }
    }

    public static Func<PQTickInstant> LeastRestrictivePQInstanceFactory
    (this ISourceTickerInfo srcTickerInfo, TickerDetailLevel lowerBound
      , IRecycler? recycler = null)
    {
        var srcTickerQuoteLevel = srcTickerInfo.PublishedTickerDetailLevel;
        var quoteLevel          = (TickerDetailLevel)Math.Max((byte)srcTickerQuoteLevel, (byte)lowerBound);
        switch (quoteLevel)
        {
            case TickerDetailLevel.SingleValue: return () => new PQTickInstant(srcTickerInfo);
            case TickerDetailLevel.Level1Quote: return () => new PQLevel1Quote(srcTickerInfo);
            case TickerDetailLevel.Level2Quote: return () => new PQLevel2Quote(srcTickerInfo);

            default: return () => new PQLevel3Quote(srcTickerInfo);
        }
    }

    public static Func<PQTickInstant> PublishedTypePQInstanceFactory
        (this ISourceTickerInfo srcTickerInfo, IRecycler? recycler = null)
    {
        var quoteLevel = srcTickerInfo.PublishedTickerDetailLevel;
        switch (quoteLevel)
        {
            case TickerDetailLevel.SingleValue: return () => new PQTickInstant(srcTickerInfo);
            case TickerDetailLevel.Level1Quote: return () => new PQLevel1Quote(srcTickerInfo);
            case TickerDetailLevel.Level2Quote: return () => new PQLevel2Quote(srcTickerInfo);

            default: return () => new PQLevel3Quote(srcTickerInfo);
        }
    }
}
