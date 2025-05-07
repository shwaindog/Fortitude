// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Linq.Expressions;
using System.Text;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Summaries;

#endregion

// ReSharper disable UnusedMethodReturnValue.Local
// ReSharper disable InconsistentNaming

namespace FortitudeMarkets.Pricing.Quotes;

public static class QuoteExtensionMethods
{
    private const int PropertyNamePadding              = -21;
    private const int secondLineIndexAdditionalPadding = 4;

    private static readonly int secondLinePadding = Math.Abs(PropertyNamePadding) + 1;

    public static string DiffQuotes(this ITickInstant? ti1, ITickInstant? ti2, bool exactValues = false)
    {
        if (ti1 == null && ti2 == null) return "";
        if (ti1 == null) return "q1 is null";
        if (ti2 == null) return "q2 is null";
        var sb = new StringBuilder(50);

        sb.AddIfDifferent(ti1, ti2, q => q.SourceTickerInfo!);
        sb.AddIfDifferent(ti1, ti2, q => q.IsReplay);
        sb.AddIfDifferent(ti1, ti2, q => q.SingleTickValue);
        sb.AddIfDifferent(ti1, ti2, q => q.SourceTime);
        sb.AddIfDifferent(ti1, ti2, q => q.ClientReceivedTime, exactValues);

        var l1q1 = ti1 as ILevel1Quote;
        var l1q2 = ti2 as ILevel1Quote;
        if (l1q1 == null && l1q2 != null)
        {
            sb.Append("q1 is only a tick instant, but q2 is a level1 quote");
        }
        else if (l1q1 != null && l1q2 == null)
        {
            sb.Append("q2 is only a tick instant, but q1 is a level1 quote");
        }
        else if (l1q1 != null) //no need for && l2q2 != null
        {
            sb.AddIfDifferent(l1q1, l1q2, q => q.SourceBidTime);
            sb.AddIfDifferent(l1q1, l1q2, q => q.SourceAskTime);
            sb.AddIfDifferent(l1q1, l1q2, q => q.AdapterReceivedTime);
            sb.AddIfDifferent(l1q1, l1q2, q => q.AdapterSentTime);
            sb.AddIfDifferent(l1q1, l1q2, q => q.BidPriceTop);
            sb.AddIfDifferent(l1q1, l1q2, q => q.AskPriceTop);
            sb.AddIfDifferent(l1q1, l1q2, q => q.Executable);
            sb.AddIfDifferent(l1q1, l1q2, q => q.SummaryPeriod!);
            sb.AddIfDifferent(l1q1, l1q2, q => q.IsAskPriceTopChanged!);
            sb.AddIfDifferent(l1q1, l1q2, q => q.IsBidPriceTopChanged!);
        }

        var l2q1 = ti1 as ILevel2Quote;
        var l2q2 = ti2 as ILevel2Quote;
        if (l2q1 == null && l2q2 != null)
        {
            sb.Append("q1 is only a level1 quote, but q2 is a level2 quote");
        }
        else if (l2q1 != null && l2q2 == null)
        {
            sb.Append("q2 is only a level1 quote, but q1 is a level2 quote");
        }
        else if (l2q1 != null) //no need for && l2q2 != null
        {
            sb.AddIfDifferent(l2q1, l2q2, q => q.OrderBook, exactValues);
        }

        var l3q1 = ti1 as ILevel3Quote;
        var l3q2 = ti2 as ILevel3Quote;
        if (l3q1 == null && l3q2 != null)
        {
            sb.Append("q1 is only a level1 quote, but q2 is a level2 quote");
        }
        else if (l3q1 != null && l3q2 == null)
        {
            sb.Append("q2 is only a level1 quote, but q1 is a level2 quote");
        }
        else if (l3q1 != null) //no need for && l3q2 != null
        {
            sb.AddIfDifferent(l3q1, l3q2, q => q.RecentlyTraded!);
            sb.AddIfDifferent(l3q1, l3q2, q => q.BatchId);
            sb.AddIfDifferent(l3q1, l3q2, q => q.SourceQuoteReference);
        }

        return sb.Length > 0 ? sb.ToString() : "";
    }

    private static StringBuilder AddIfDifferent<T>
        (this StringBuilder sb, T q1, T q2, Expression<Func<T, string>> property, bool makeThisCheck = true) where T : ITickInstant
    {
        var evaluator = property.Compile();
        var q1Value   = evaluator(q1);
        var q2Value   = evaluator(q2);
        if (makeThisCheck && q1Value != q2Value)
        {
            var propertyName = property.GetPropertyName();
            sb.Append($"{propertyName,PropertyNamePadding}:q1={(q1Value != null ? "\"" : "")}" +
                      $"{q1Value ?? "null"}{(q1Value != null ? "\"" : "")}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"q2={(q2Value != null ? "\"" : "")}{q2Value ?? "null"}{(q2Value != null ? "\"" : "")}\n");
        }

        return sb;
    }

    private static StringBuilder AddIfDifferent<T>
        (this StringBuilder sb, T? q1, T? q2, Expression<Func<T, DateTime>> property, bool makeThisCheck = true) where T : ITickInstant
    {
        var       evaluator = property.Compile();
        DateTime? q1Value   = q1 != null ? evaluator(q1) : null;
        DateTime? q2Value   = q2 != null ? evaluator(q2) : null;
        if (makeThisCheck && q1Value != q2Value)
        {
            var propertyName = property.GetPropertyName();
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1Value:O}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"q2={q2Value:O}\n");
        }

        return sb;
    }

    private static StringBuilder AddIfDifferent<T>
        (this StringBuilder sb, T? q1, T? q2, Expression<Func<T, decimal>> property, bool makeThisCheck = true) where T : ITickInstant
    {
        var      evaluator = property.Compile();
        decimal? q1Value   = q1 != null ? evaluator(q1) : null;
        decimal? q2Value   = q2 != null ? evaluator(q2) : null;
        if (makeThisCheck && q1Value != q2Value)
        {
            var propertyName = property.GetPropertyName();
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1Value:N6}\n")
              .Insert(sb.Length, " ", secondLinePadding).Append($"q2={q2Value:N6}\n");
        }

        return sb;
    }

    private static StringBuilder AddIfDifferent<T>
        (this StringBuilder sb, T? q1, T? q2, Expression<Func<T, bool>> property, bool makeThisCheck = true) where T : ITickInstant
    {
        var   evaluator = property.Compile();
        bool? q1Value   = q1 != null ? evaluator(q1) : null;
        bool? q2Value   = q2 != null ? evaluator(q2) : null;
        if (makeThisCheck && q1Value != q2Value)
        {
            var propertyName = property.GetPropertyName();
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1Value}\n")
              .Insert(sb.Length, " ", secondLinePadding).Append($" q2={q2Value}\n");
        }

        return sb;
    }

    private static StringBuilder AddIfDifferent<T>
        (this StringBuilder sb, T? q1, T? q2, Expression<Func<T, ISourceTickerInfo>> property, bool exactValue = false)
        where T : ITickInstant
    {
        var evaluator    = property.Compile();
        var q1Value      = q1 != null ? evaluator(q1) : null;
        var q2Value      = q2 != null ? evaluator(q2) : null;
        var propertyName = property.GetPropertyName();
        if (q1Value == null && q2Value == null) return sb;
        if ((q1Value != null && q2Value == null) || q1Value == null) //not requiring && q2Value != null
            sb.Append($"{propertyName,PropertyNamePadding}:q1={(q1Value != null ? "not null" : "null")}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"q2={(q2Value != null ? "not null" : "null")}\n");
        var areSame = false;
        if (q1Value is IInterfacesComparable<ISourceTickerInfo> comparableQ1)
            areSame                                                                        = comparableQ1.AreEquivalent(q2Value, exactValue);
        else if (q2Value is IInterfacesComparable<ISourceTickerInfo> comparableQ2) areSame = comparableQ2.AreEquivalent(q1Value, exactValue);
        if (!areSame)
            sb.Append($"{propertyName,PropertyNamePadding}:q1=").Append(q1Value).Append("\n")
              .Insert(sb.Length, " ", secondLinePadding).Append("q2=").Append(q2Value).Append("\n");
        return sb;
    }

    private static StringBuilder AddIfDifferent<T>
        (this StringBuilder sb, T? q1, T? q2, Expression<Func<T, IPricePeriodSummary>> property, bool exactValue = false)
        where T : ITickInstant
    {
        var evaluator    = property.Compile();
        var q1Value      = q1 != null ? evaluator(q1) : null;
        var q2Value      = q2 != null ? evaluator(q2) : null;
        var propertyName = property.GetPropertyName();
        if ((q1Value == null || q1Value.IsEmpty) && (q2Value == null || q2Value.IsEmpty)) return sb;
        if ((q1Value is { IsEmpty: false } && q2Value == null)
         || (q1Value == null && q2Value is { IsEmpty: false })) //not requiring && q2Value != null
            sb.Append($"{propertyName,PropertyNamePadding}:q1={(q1Value != null ? "not null" : "null")}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"q2={(q2Value != null ? "not null" : "null")}\n");
        var areSame = false;
        if (q1Value is IInterfacesComparable<IPricePeriodSummary> comparableQ1)
            areSame                                                                          = comparableQ1.AreEquivalent(q2Value, exactValue);
        else if (q2Value is IInterfacesComparable<IPricePeriodSummary> comparableQ2) areSame = comparableQ2.AreEquivalent(q1Value, exactValue);
        if (!areSame)
            sb.Append($"{propertyName,PropertyNamePadding}:q1=").Append(q1Value).Append("\n")
              .Insert(sb.Length, " ", secondLinePadding).Append("q2=").Append(q2Value).Append("\n");
        return sb;
    }

    private static StringBuilder AddIfDifferent
        (this StringBuilder sb, ILevel2Quote? q1, ILevel2Quote? q2, Expression<Func<ILevel2Quote, IOrderBook>> property, bool exactTypes = true)
    {
        var evaluator    = property.Compile();
        var q1OrderBook      = q1 != null ? evaluator(q1) : null;
        var q2OrderBook      = q2 != null ? evaluator(q2) : null;
        var propertyName = property.GetPropertyName();
        if (q1OrderBook == null && q2OrderBook == null) return sb;
        if ((q1OrderBook != null && q2OrderBook == null) || q1OrderBook == null) //not requiring && q2Value != null
            sb.Append($"{propertyName,PropertyNamePadding}:q1.OrderBook={(q1OrderBook != null ? "not null" : "null")}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"q2.OrderBook={(q2OrderBook != null ? "not null" : "null")}\n");
        
        sb.AddIfDifferent(q1OrderBook, q2OrderBook, q => q.DailyTickUpdateCount);
        sb.AddIfDifferent("OrderBook", q1OrderBook?.OpenInterest, q2OrderBook?.OpenInterest, !exactTypes);

        sb.AddIfDifferent("BidSide", q1OrderBook?.BidSide, q2OrderBook?.BidSide, exactTypes);
        sb.AddIfDifferent("AskSide", q1OrderBook?.AskSide, q2OrderBook?.AskSide, exactTypes);

        return sb;
    }

    private static StringBuilder AddIfDifferent
        (this StringBuilder sb, string side, IOrderBookSide? q1obs, IOrderBookSide? q2obs, bool exactTypes = true)
    {
        if (q1obs == null && q2obs == null) return sb;
        if ((q1obs != null && q2obs == null) || q1obs == null) //not requiring && q2Value != null
            sb.Append($"{side,PropertyNamePadding}:q1={(q1obs != null ? "not null" : "null")}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"q2={(q2obs != null ? "not null" : "null")}\n");
        
        sb.AddIfDifferent(q1obs, q2obs, q => q.DailyTickUpdateCount);
        sb.AddIfDifferent($"OrderBook.{side}", q1obs?.OpenInterestSide, q2obs?.OpenInterestSide, !exactTypes);
        var maxLayers = Math.Max(q1obs?.Capacity ?? int.MinValue,
                                 q2obs?.Capacity ?? int.MinValue);
        for (var i = 0; i < maxLayers; i++)
        {
            var l1 = i < (q1obs?.Capacity ?? int.MinValue) ? q1obs?[i] : null;
            var l2 = i < (q2obs?.Capacity ?? int.MinValue) ? q2obs?[i] : null;
            sb.AddIfDifferent(side, i, l1, l2, exactTypes);
        }

        return sb;
    }


    private static StringBuilder AddIfDifferent
        (this StringBuilder sb, string propertyName, int level, IPriceVolumeLayer? l1, IPriceVolumeLayer? l2, bool exactTypes = true)
    {
        if ((l1 == null && l2 != null) || (l1 != null && l2 == null)
                                       || (l1 != null && !l1.AreEquivalent(l2, exactTypes)) || (l2 != null && !l2.AreEquivalent(l1, exactTypes)))
            sb.Append($"{propertyName,PropertyNamePadding + secondLineIndexAdditionalPadding}[{level,2}]:l1=")
              .Append(l1?.ToString() ?? "null").Append("\n")
              .Insert(sb.Length, " ", secondLinePadding).Append("l2=")
              .Append(l2?.ToString() ?? "null").Append("\n");
        return sb;
    }

    public static string DiffPriceVolumeLayer
        (string propertyName, IPriceVolumeLayer? l1, IPriceVolumeLayer? l2, int level)
    {
        var sb = new StringBuilder();
        sb.AddIfDifferent(propertyName, level, l1, l2);
        return sb.ToString();
    }

    private static StringBuilder AddIfDifferent
        (this StringBuilder sb, ILevel3Quote? q1, ILevel3Quote? q2, Expression<Func<ILevel3Quote, IRecentlyTraded>> property)
    {
        var evaluator    = property.Compile();
        var q1Value      = q1 != null ? evaluator(q1) : null;
        var q2Value      = q2 != null ? evaluator(q2) : null;
        var propertyName = property.GetPropertyName();
        if (q1Value == null && q2Value == null) return sb;
        if ((q1Value != null && q2Value == null) || q1Value == null) //not requiring && q2Value != null
            sb.Append($"{propertyName,PropertyNamePadding}:q1={(q1Value != null ? "not null" : "null")}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"q2={(q2Value != null ? "not null" : "null")}\n");
        var maxLayers = Math.Max(q1Value?.Count ?? int.MinValue, q2Value?.Count ?? int.MinValue);
        for (var i = 0; i < maxLayers; i++)
        {
            var lt1 = i < (q1Value?.Count ?? int.MinValue) ? q1Value?[i] : null;
            var lt2 = i < (q2Value?.Count ?? int.MinValue) ? q2Value?[i] : null;
            sb.AddIfDifferent(propertyName, i, lt1, lt2);
        }

        return sb;
    }

    private static StringBuilder AddIfDifferent
        (this StringBuilder sb, string propertyName, int level, ILastTrade? lt1, ILastTrade? lt2)
    {
        if ((lt1 == null && lt2 != null) || (lt1 != null && lt2 == null) || (lt1 != null && (!lt1.Equals(lt2)
                                                                                          || !lt2.Equals(lt1))))
            sb.Append($"{propertyName,PropertyNamePadding + secondLineIndexAdditionalPadding}[{level,2}]:" +
                      $"lt1={lt1?.ToString() ?? "null"}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"lt2={lt2?.ToString() ?? "null"}\n");

        return sb;
    }

    private static StringBuilder AddIfDifferent
        (this StringBuilder sb, ILevel3Quote? q1, ILevel3Quote? q2, Expression<Func<ILevel3Quote, uint>> property)
    {
        var   evaluator = property.Compile();
        uint? q1Value   = q1 != null ? evaluator(q1) : null;
        uint? q2Value   = q2 != null ? evaluator(q2) : null;
        if (q1Value != q2Value)
        {
            var propertyName = property.GetPropertyName();
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1Value}\n")
              .Insert(sb.Length, " ", secondLinePadding).Append($" q2={q2Value}\n");
        }

        return sb;
    }

    private static StringBuilder AddIfDifferent
        (this StringBuilder sb, IOrderBook? q1ob, IOrderBook? q2ob, Expression<Func<IOrderBook, uint>> property)
    {
        var   evaluator = property.Compile();
        uint? q1Value   = q1ob != null ? evaluator(q1ob) : null;
        uint? q2Value   = q2ob != null ? evaluator(q2ob) : null;
        if (q1Value != q2Value)
        {
            var propertyName = $"OrderBook.{property.GetPropertyName()}";
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1Value}\n")
              .Insert(sb.Length, " ", secondLinePadding).Append($" q2={q2Value}\n");
        }

        return sb;
    }

    private static StringBuilder AddIfDifferent
        (this StringBuilder sb, IOrderBookSide? q1obs, IOrderBookSide? q2obs, Expression<Func<IOrderBookSide, uint>> property)
    {
        var   evaluator = property.Compile();
        uint? q1Value   = q1obs != null ? evaluator(q1obs) : null;
        uint? q2Value   = q2obs != null ? evaluator(q2obs) : null;
        if (q1obs == null && q2obs == null) return sb;
        if ((q1obs != null && q2obs == null) || q1obs == null) //not requiring && q2Value != null
            sb.Append($"{ "OrderBookSide",PropertyNamePadding}:q1={(q1obs != null ? "not null" : "null")}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"q2={(q2obs != null ? "not null" : "null")}\n");
        if (q1Value != q2Value)
        {
            var propertyName = $"{q1obs.BookSide}.OrderBookSide.{property.GetPropertyName()}";
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1Value}\n")
              .Insert(sb.Length, " ", secondLinePadding).Append($" q2={q2Value}\n");
        }

        return sb;
    }

    private static StringBuilder AddIfDifferent
        (this StringBuilder sb, string pathPrefix, IOpenInterest? q1oi, IOpenInterest? q2oi, bool skipOnBothPublishDataSource)
    {
        if (q1oi == null && q2oi == null) return sb;
        if ((q1oi != null && q2oi == null) || q1oi == null) //not requiring && q2Value != null
            sb.Append($"{ $"{pathPrefix}.OpenInterest",PropertyNamePadding}:q1={(q1oi != null ? "not null" : "null")}\n")
              .Insert(sb.Length, " ", secondLinePadding)
              .Append($"q2={(q2oi != null ? "not null" : "null")}\n");
        if (skipOnBothPublishDataSource && q1oi!.DataSource == MarketDataSource.Published && q2oi!.DataSource == MarketDataSource.Published) return sb;
        if (q1oi!.DataSource != q2oi!.DataSource)
        {
            var propertyName = $"{pathPrefix}.OpenInterest.DataSource";
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1oi.DataSource}\n")
              .Insert(sb.Length, " ", secondLinePadding).Append($" q2={q2oi.DataSource}\n");
        }
        if (q1oi.UpdateTime != q2oi.UpdateTime)
        {
            var propertyName = $"{pathPrefix}.OpenInterest.UpdateTime";
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1oi.UpdateTime}\n")
              .Insert(sb.Length, " ", secondLinePadding).Append($" q2={q2oi.UpdateTime}\n");
        }
        if (q1oi.Volume != q2oi.Volume)
        {
            var propertyName = $"{pathPrefix}.OpenInterest.Volume";
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1oi.Volume:N2}\n")
              .Insert(sb.Length, " ", secondLinePadding).Append($" q2={q2oi.Volume:N2}\n");
        }
        if (q1oi.Vwap != q2oi.Vwap)
        {
            var propertyName = $"{pathPrefix}.OpenInterest.Vwap";
            sb.Append($"{propertyName,PropertyNamePadding}:q1={q1oi.Vwap:N6}\n")
              .Insert(sb.Length, " ", secondLinePadding).Append($" q2={q2oi.Vwap:N6}\n");
        }

        return sb;
    }
}
