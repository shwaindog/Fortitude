#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;

public class SourceTickerPublicationConfig : SourceTickerQuoteInfo, IMutableSourceTickerPublicationConfig
{
    public SourceTickerPublicationConfig(uint uniqueId, string source, string ticker,
        byte maximumPublishedLayers = 20, decimal roundingPrecision = 0.0001m, decimal minSubmitSize = 0.01m,
        decimal maxSubmitSize = 1_000_000m, decimal incrementSize = 0.01m, ushort minimumQuoteLife = 100,
        LayerFlags layerFlags = LayerFlags.Price | LayerFlags.Volume,
        LastTradedFlags lastTradedFlags = LastTradedFlags.None,
        ISnapshotUpdatePricingServerConfig? marketPriceQuoteServer = null)
        : base(uniqueId, source, ticker, maximumPublishedLayers, roundingPrecision, minSubmitSize, maxSubmitSize,
            incrementSize, minimumQuoteLife, layerFlags, lastTradedFlags) =>
        MarketPriceQuoteServer = marketPriceQuoteServer;

    private SourceTickerPublicationConfig(ISourceTickerPublicationConfig toClone) : base(toClone) =>
        MarketPriceQuoteServer = toClone?.MarketPriceQuoteServer?.Clone();

    public ISnapshotUpdatePricingServerConfig? MarketPriceQuoteServer { get; set; }

    public override object Clone() => new SourceTickerPublicationConfig(this);

    IMutableSourceTickerPublicationConfig IMutableSourceTickerPublicationConfig.Clone() => (IMutableSourceTickerPublicationConfig)Clone();

    ISourceTickerPublicationConfig ISourceTickerPublicationConfig.Clone() => (ISourceTickerPublicationConfig)Clone();

    public override bool AreEquivalent(ISourceTickerQuoteInfo? other, bool exactTypes = false)
    {
        if (!(other is ISourceTickerPublicationConfig otherSrcTkrId)) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);
        var mrktPxQtServerSame = MarketPriceQuoteServer?.AreEquivalent(otherSrcTkrId.MarketPriceQuoteServer, exactTypes)
                                 ?? otherSrcTkrId.MarketPriceQuoteServer == null;
        return baseSame && mrktPxQtServerSame;
    }

    public override bool AreEquivalent(IUniqueSourceTickerIdentifier? other, bool exactTypes = false)
    {
        if (!(other is ISourceTickerPublicationConfig otherSrcTkrId)) return false;
        return AreEquivalent(otherSrcTkrId, exactTypes);
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IUniqueSourceTickerIdentifier, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (MarketPriceQuoteServer?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public override string ToString() =>
        $"SourceTickerPublicationConfig {{{nameof(Id)}: {Id}, {nameof(Source)}: {Source}, " +
        $"{nameof(Ticker)}: {Ticker},  {nameof(RoundingPrecision)}: {RoundingPrecision}, " +
        $"{nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, " +
        $"{nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(LayerFlags)}: {LayerFlags}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, " +
        $"{nameof(LastTradedFlags)}: {LastTradedFlags}, {nameof(MarketPriceQuoteServer)}: " +
        $"{MarketPriceQuoteServer} }}";
}
