// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Types;
using FortitudeIO.TimeSeries;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.TimeSeries;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes;

public class Level0PriceQuote : ReusableObject<ILevel0Quote>, IMutableLevel0Quote
{
    protected static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(Level0PriceQuote));
    public Level0PriceQuote() { }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public Level0PriceQuote(ISourceTickerQuoteInfo sourceTickerQuoteInfo, DateTime? sourceTime = null,
        bool isReplay = false, decimal singlePrice = 0m, DateTime? clientReceivedTime = null)
    {
        SourceTickerQuoteInfo = sourceTickerQuoteInfo is SourceTickerQuoteInfo
            ? sourceTickerQuoteInfo
            : new SourceTickerQuoteInfo(sourceTickerQuoteInfo);
        SourceTime         = sourceTime ?? DateTimeConstants.UnixEpoch;
        IsReplay           = isReplay;
        SinglePrice        = singlePrice;
        ClientReceivedTime = clientReceivedTime ?? DateTimeConstants.UnixEpoch;
    }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public Level0PriceQuote(ILevel0Quote toClone)
    {
        SourceTickerQuoteInfo = toClone.SourceTickerQuoteInfo is SourceTickerQuoteInfo
            ? toClone.SourceTickerQuoteInfo
            : new SourceTickerQuoteInfo(toClone.SourceTickerQuoteInfo!);
        SourceTime         = toClone.SourceTime;
        IsReplay           = toClone.IsReplay;
        SinglePrice        = toClone.SinglePrice;
        ClientReceivedTime = toClone.ClientReceivedTime;
    }

    public virtual QuoteLevel QuoteLevel => QuoteLevel.Level0;

    public ISourceTickerQuoteInfo? SourceTickerQuoteInfo { get; set; }

    ISourceTickerQuoteInfo? ILevel0Quote.SourceTickerQuoteInfo => SourceTickerQuoteInfo;

    public virtual DateTime SourceTime         { get; set; }
    public         bool     IsReplay           { get; set; }
    public virtual decimal  SinglePrice        { get; set; }
    public         DateTime ClientReceivedTime { get; set; }

    public DateTime StorageTime(IStorageTimeResolver<ILevel0Quote>? resolver = null)
    {
        resolver ??= QuoteStorageTimeResolver.Instance;
        return resolver.ResolveStorageTime(this);
    }

    public override ILevel0Quote CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ClientReceivedTime = source.ClientReceivedTime;

        if (SourceTickerQuoteInfo == null)
            SourceTickerQuoteInfo = source.SourceTickerQuoteInfo is SourceTickerQuoteInfo
                ? source.SourceTickerQuoteInfo
                : new SourceTickerQuoteInfo(source.SourceTickerQuoteInfo!);
        else
            SourceTickerQuoteInfo.CopyFrom(source.SourceTickerQuoteInfo!, copyMergeFlags);

        SourceTime  = source.SourceTime;
        IsReplay    = source.IsReplay;
        SinglePrice = source.SinglePrice;
        return this;
    }

    ILevel0Quote ICloneable<ILevel0Quote>.Clone() => Clone();

    public override IMutableLevel0Quote Clone() =>
        (IMutableLevel0Quote?)Recycler?.Borrow<Level0PriceQuote>().CopyFrom(this) ?? new Level0PriceQuote(this);

    public virtual bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var srcTickersAreEquivalent =
            SourceTickerQuoteInfo?.AreEquivalent(other.SourceTickerQuoteInfo, exactTypes)
         ?? other.SourceTickerQuoteInfo == null;
        var sourceTimesSame        = SourceTime.Equals(other.SourceTime);
        var replayIsSame           = IsReplay == other.IsReplay;
        var singlePriceSame        = SinglePrice == other.SinglePrice;
        var clientReceivedTimeSame = ClientReceivedTime.Equals(other.ClientReceivedTime);
        return srcTickersAreEquivalent && sourceTimesSame && replayIsSame && singlePriceSame
            && clientReceivedTimeSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILevel0Quote, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = SourceTickerQuoteInfo != null ? SourceTickerQuoteInfo.GetHashCode() : 0;
            hashCode = (hashCode * 397) ^ SourceTime.GetHashCode();
            hashCode = (hashCode * 397) ^ IsReplay.GetHashCode();
            hashCode = (hashCode * 397) ^ SinglePrice.GetHashCode();
            hashCode = (hashCode * 397) ^ ClientReceivedTime.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"Level0PriceQuote {{{nameof(SourceTickerQuoteInfo)}: {SourceTickerQuoteInfo}, " +
        $"{nameof(SourceTime)}: {SourceTime:O}, {nameof(IsReplay)}: {IsReplay}, {nameof(SinglePrice)}: " +
        $"{SinglePrice:N5}, {nameof(ClientReceivedTime)}: {ClientReceivedTime:O} }}";
}
