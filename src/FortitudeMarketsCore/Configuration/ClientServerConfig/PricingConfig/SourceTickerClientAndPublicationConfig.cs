#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;

#endregion

namespace FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;

public class SourceTickerClientAndPublicationConfig : SourceTickerPublicationConfig,
    ISourceTickerClientAndPublicationConfig
{
    public SourceTickerClientAndPublicationConfig(ISourceTickerPublicationConfig serverConfig,
        uint resyncIntervalMs = 4000, bool allowUpdateCatchups = true)
        : base(serverConfig.Id, serverConfig.Source, serverConfig.Ticker, serverConfig.MaximumPublishedLayers,
            serverConfig.RoundingPrecision, serverConfig.MinSubmitSize, serverConfig.MaxSubmitSize,
            serverConfig.IncrementSize, serverConfig.MinimumQuoteLife, serverConfig.LayerFlags,
            serverConfig.LastTradedFlags, serverConfig.MarketPriceQuoteServer)
    {
        SyncRetryIntervalMs = resyncIntervalMs;
        AllowUpdatesCatchup = allowUpdateCatchups;
    }

    public SourceTickerClientAndPublicationConfig(uint uniqueId, string source, string ticker,
        byte maximumPublishedLayers = 20, decimal roundingPrecision = 0.0001m, decimal minSubmitSize = 0.01m,
        decimal maxSubmitSize = 1_000_000, decimal incrementSize = 0.01m, ushort minimumQuoteLife = 100,
        LayerFlags layerFlags = LayerFlags.Price | LayerFlags.Volume,
        LastTradedFlags lastTradedFlags = LastTradedFlags.None,
        ISnapshotUpdatePricingServerConfig? marketPriceQuoteServer = null,
        uint resyncIntervalMs = 4000, bool allowUpdateCatchups = true)
        : base(uniqueId, source, ticker, maximumPublishedLayers, roundingPrecision, minSubmitSize, maxSubmitSize,
            incrementSize, minimumQuoteLife, layerFlags, lastTradedFlags, marketPriceQuoteServer)
    {
        SyncRetryIntervalMs = resyncIntervalMs;
        AllowUpdatesCatchup = allowUpdateCatchups;
    }

    public SourceTickerClientAndPublicationConfig(ISourceTickerClientAndPublicationConfig toClone)
        : base(toClone.Id, toClone.Source, toClone.Ticker, toClone.MaximumPublishedLayers, toClone.RoundingPrecision,
            toClone.MinSubmitSize, toClone.MaxSubmitSize, toClone.IncrementSize, toClone.MinimumQuoteLife,
            toClone.LayerFlags, toClone.LastTradedFlags, toClone.MarketPriceQuoteServer)
    {
        SyncRetryIntervalMs = toClone.SyncRetryIntervalMs;
        AllowUpdatesCatchup = toClone.AllowUpdatesCatchup;
    }

    public uint SyncRetryIntervalMs { get; }

    public bool AllowUpdatesCatchup { get; }

    public override object Clone() => new SourceTickerClientAndPublicationConfig(this);

    ISourceTickerClientAndPublicationConfig ISourceTickerClientAndPublicationConfig.Clone() =>
        (ISourceTickerClientAndPublicationConfig)Clone();

    public override bool AreEquivalent(IUniqueSourceTickerIdentifier? other, bool exactTypes = false)
    {
        if (!(other is ISourceTickerClientAndPublicationConfig srcTkrClientAndPubConfig)) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);
        var syncRetrySame = SyncRetryIntervalMs == srcTkrClientAndPubConfig.SyncRetryIntervalMs;
        var allowUpdatesCatchupSame = AllowUpdatesCatchup == srcTkrClientAndPubConfig.AllowUpdatesCatchup;

        return baseSame && syncRetrySame && allowUpdatesCatchupSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return AreEquivalent((ISourceTickerClientAndPublicationConfig)obj, true);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)SyncRetryIntervalMs;
            hashCode = (hashCode * 397) ^ AllowUpdatesCatchup.GetHashCode();
            return hashCode;
        }
    }

    public override string ToString() =>
        $"SourceTickerClientAndPublicationConfig {{{nameof(Id)}: {Id}, {nameof(Source)}: {Source}, " +
        $"{nameof(Ticker)}: {Ticker},  {nameof(RoundingPrecision)}: {RoundingPrecision}, " +
        $"{nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, " +
        $"{nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(LayerFlags)}: {LayerFlags}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, " +
        $"{nameof(LastTradedFlags)}: {LastTradedFlags}, {nameof(MarketPriceQuoteServer)}: " +
        $"{MarketPriceQuoteServer}, {nameof(SyncRetryIntervalMs)}: {SyncRetryIntervalMs}, " +
        $"{nameof(AllowUpdatesCatchup)}: {AllowUpdatesCatchup}   }}";
}
