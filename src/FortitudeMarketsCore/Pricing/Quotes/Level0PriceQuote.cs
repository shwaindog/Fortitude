#region

using System.Diagnostics.CodeAnalysis;
using FortitudeCommon.Chronometry;
using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Pricing.Quotes;

public class Level0PriceQuote : IMutableLevel0Quote
{
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public Level0PriceQuote(ISourceTickerQuoteInfo sourceTickerQuoteInfo, DateTime? sourceTime = null,
        bool isReplay = false, decimal singlePrice = 0m, DateTime? clientReceivedTime = null)
    {
        SourceTickerQuoteInfo = sourceTickerQuoteInfo is SourceTickerQuoteInfo ?
            (IMutableSourceTickerQuoteInfo)sourceTickerQuoteInfo :
            new SourceTickerQuoteInfo(sourceTickerQuoteInfo);
        SourceTime = sourceTime ?? DateTimeConstants.UnixEpoch;
        IsReplay = isReplay;
        SinglePrice = singlePrice;
        ClientReceivedTime = clientReceivedTime ?? DateTimeConstants.UnixEpoch;
    }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    public Level0PriceQuote(ILevel0Quote toClone)
    {
        SourceTickerQuoteInfo = toClone.SourceTickerQuoteInfo is SourceTickerQuoteInfo ?
            (IMutableSourceTickerQuoteInfo)toClone.SourceTickerQuoteInfo :
            new SourceTickerQuoteInfo(toClone.SourceTickerQuoteInfo!);
        SourceTime = toClone.SourceTime;
        IsReplay = toClone.IsReplay;
        SinglePrice = toClone.SinglePrice;
        ClientReceivedTime = toClone.ClientReceivedTime;
    }

    public IMutableSourceTickerQuoteInfo? SourceTickerQuoteInfo { get; set; }
    ISourceTickerQuoteInfo? ILevel0Quote.SourceTickerQuoteInfo => SourceTickerQuoteInfo;
    public virtual DateTime SourceTime { get; set; }
    public bool IsReplay { get; set; }
    public virtual decimal SinglePrice { get; set; }
    public DateTime ClientReceivedTime { get; set; }

    public virtual void CopyFrom(ILevel0Quote source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        ClientReceivedTime = source.ClientReceivedTime;
        SourceTickerQuoteInfo = source.SourceTickerQuoteInfo as IMutableSourceTickerQuoteInfo;
        SourceTime = source.SourceTime;
        IsReplay = source.IsReplay;
        SinglePrice = source.SinglePrice;
    }

    public void CopyFrom(IStoreState source, CopyMergeFlags copyMergeFlags)
    {
        CopyFrom((ILevel0Quote)source, copyMergeFlags);
    }

    public virtual object Clone() => new Level0PriceQuote(this);

    ILevel0Quote ICloneable<ILevel0Quote>.Clone() => (ILevel0Quote)Clone();

    ILevel0Quote ILevel0Quote.Clone() => (ILevel0Quote)Clone();

    IMutableLevel0Quote IMutableLevel0Quote.Clone() => (IMutableLevel0Quote)Clone();

    public virtual bool AreEquivalent(ILevel0Quote? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var srcTickersAreEquivalent = SourceTickerQuoteInfo?.AreEquivalent(other.SourceTickerQuoteInfo, exactTypes)
                                      ?? other.SourceTickerQuoteInfo == null;
        var sourceTimesSame = SourceTime.Equals(other.SourceTime);
        var replayIsSame = IsReplay == other.IsReplay;
        var singlePriceSame = SinglePrice == other.SinglePrice;
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
