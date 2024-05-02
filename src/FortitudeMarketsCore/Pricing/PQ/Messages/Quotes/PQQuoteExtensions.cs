#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

public static class PQQuoteExtensions
{
    public static PQLevel0Quote MostCompactPQInstance(this ISourceTickerQuoteInfo srcTickerQuoteInfo)
    {
        var quoteLevel = srcTickerQuoteInfo.MostCompactQuoteLevel();
        switch (quoteLevel)
        {
            case QuoteLevel.Level0: return new PQLevel0Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level1: return new PQLevel1Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level2: return new PQLevel2Quote(srcTickerQuoteInfo);
            default: return new PQLevel3Quote(srcTickerQuoteInfo);
        }
    }

    public static PQLevel0Quote PublishedTypePQInstance(this ISourceTickerQuoteInfo srcTickerQuoteInfo)
    {
        var quoteLevel = srcTickerQuoteInfo.PublishedQuoteLevel;
        switch (quoteLevel)
        {
            case QuoteLevel.Level0: return new PQLevel0Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level1: return new PQLevel1Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level2: return new PQLevel2Quote(srcTickerQuoteInfo);
            default: return new PQLevel3Quote(srcTickerQuoteInfo);
        }
    }

    public static Func<PQLevel0Quote> MostRestrictivePQInstanceFactory(this ISourceTickerQuoteInfo srcTickerQuoteInfo, QuoteLevel upperBound)
    {
        var srcTickerQuoteLevel = srcTickerQuoteInfo.PublishedQuoteLevel;
        var quoteLevel = (QuoteLevel)Math.Min((byte)srcTickerQuoteLevel, (byte)upperBound);
        switch (quoteLevel)
        {
            case QuoteLevel.Level0: return () => new PQLevel0Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level1: return () => new PQLevel1Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level2: return () => new PQLevel2Quote(srcTickerQuoteInfo);
            default: return () => new PQLevel3Quote(srcTickerQuoteInfo);
        }
    }

    public static Func<PQLevel0Quote> LeastRestrictivePQInstanceFactory(this ISourceTickerQuoteInfo srcTickerQuoteInfo, QuoteLevel lowerBound)
    {
        var srcTickerQuoteLevel = srcTickerQuoteInfo.PublishedQuoteLevel;
        var quoteLevel = (QuoteLevel)Math.Max((byte)srcTickerQuoteLevel, (byte)lowerBound);
        switch (quoteLevel)
        {
            case QuoteLevel.Level0: return () => new PQLevel0Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level1: return () => new PQLevel1Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level2: return () => new PQLevel2Quote(srcTickerQuoteInfo);
            default: return () => new PQLevel3Quote(srcTickerQuoteInfo);
        }
    }

    public static Func<PQLevel0Quote> PublishedTypePQInstanceFactory(this ISourceTickerQuoteInfo srcTickerQuoteInfo)
    {
        var quoteLevel = srcTickerQuoteInfo.PublishedQuoteLevel;
        switch (quoteLevel)
        {
            case QuoteLevel.Level0: return () => new PQLevel0Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level1: return () => new PQLevel1Quote(srcTickerQuoteInfo);
            case QuoteLevel.Level2: return () => new PQLevel2Quote(srcTickerQuoteInfo);
            default: return () => new PQLevel3Quote(srcTickerQuoteInfo);
        }
    }
}
