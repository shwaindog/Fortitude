#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeIO.Protocols;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages;

public class SourceTickerQuoteInfoMessage : ReusableObject<ISourceTickerQuoteInfo>, ISourceTickerQuoteInfo
{
    public SourceTickerQuoteInfoMessage()
    {
        Source = "";
        Ticker = "";
        FormatPrice = "";
    }

    public SourceTickerQuoteInfoMessage(ushort sourceId, string source, ushort tickerId, string ticker, byte maximumPublishedLayers = 20,
        decimal roundingPrecision = 0.0001m, decimal minSubmitSize = 0.01m, decimal maxSubmitSize = 1_000_000m,
        decimal incrementSize = 0.01m, ushort minimumQuoteLife = 100,
        LayerFlags layerFlags = LayerFlags.Price | LayerFlags.Volume,
        LastTradedFlags lastTradedFlags = LastTradedFlags.None) : this()
    {
        SourceId = sourceId;
        TickerId = tickerId;
        Source = source;
        Ticker = ticker;
        MaximumPublishedLayers = maximumPublishedLayers;
        RoundingPrecision = roundingPrecision;
        MinSubmitSize = minSubmitSize;
        MaxSubmitSize = maxSubmitSize;
        IncrementSize = incrementSize;
        MinimumQuoteLife = minimumQuoteLife;
        LayerFlags = layerFlags;
        LastTradedFlags = lastTradedFlags;
    }

    public SourceTickerQuoteInfoMessage(ISourceTickerQuoteInfo toClone) : this()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public uint Id => ((uint)SourceId << 16) | TickerId;
    public ushort SourceId { get; set; }
    public ushort TickerId { get; set; }
    public string Source { get; set; }
    public string Ticker { get; set; }
    public decimal RoundingPrecision { get; set; }
    public decimal MinSubmitSize { get; set; }
    public decimal MaxSubmitSize { get; set; }
    public decimal IncrementSize { get; set; }
    public ushort MinimumQuoteLife { get; set; }
    public LayerFlags LayerFlags { get; set; }
    public byte MaximumPublishedLayers { get; set; }
    public LastTradedFlags LastTradedFlags { get; set; }
    public string FormatPrice { get; set; }


    public bool AreEquivalent(ISourceTickerQuoteInfo? other, bool exactTypes = false)
    {
        var idSame = Id == other?.Id;
        var sourceSame = Source == other?.Source;
        var tickerSame = Ticker == other?.Ticker;
        var roundingSame = RoundingPrecision == other?.RoundingPrecision;
        var minSubmitSizeSame = MinSubmitSize == other?.MinSubmitSize;
        var maxSubmitSizeSame = MaxSubmitSize == other?.MaxSubmitSize;
        var incrementSizeSame = IncrementSize == other?.IncrementSize;
        var minQuoteLifeSame = MinimumQuoteLife == other?.MinimumQuoteLife;
        var layerFlagsSame = LayerFlags == other?.LayerFlags;
        var maxPubLayersSame = MaximumPublishedLayers == other?.MaximumPublishedLayers;
        var lastTradedFlagsSame = LastTradedFlags == other?.LastTradedFlags;
        var formatPriceSame = FormatPrice == other?.FormatPrice;

        return idSame && sourceSame && tickerSame && roundingSame && minSubmitSizeSame && maxSubmitSizeSame && incrementSizeSame &&
               minQuoteLifeSame && lastTradedFlagsSame && layerFlagsSame && maxPubLayersSame && formatPriceSame;
    }

    public override ISourceTickerQuoteInfo Clone() =>
        Recycler?.Borrow<SourceTickerQuoteInfoMessage>().CopyFrom(this) ?? new SourceTickerQuoteInfoMessage(this);

    public override ISourceTickerQuoteInfo CopyFrom(ISourceTickerQuoteInfo source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        SourceId = source.SourceId;
        TickerId = source.TickerId;
        Source = source.Source;
        Ticker = source.Ticker;
        RoundingPrecision = source.RoundingPrecision;
        MinSubmitSize = source.MinSubmitSize;
        MaxSubmitSize = source.MaxSubmitSize;
        IncrementSize = source.IncrementSize;
        MinimumQuoteLife = source.MinimumQuoteLife;
        LayerFlags = source.LayerFlags;
        MaximumPublishedLayers = MaximumPublishedLayers;
        LastTradedFlags = LastTradedFlags;
        FormatPrice = source.FormatPrice;
        return this;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ISourceTickerQuoteInfo, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)SourceId;
            hashCode = (hashCode * 397) ^ TickerId;
            hashCode = (hashCode * 397) ^ Source.GetHashCode();
            hashCode = (hashCode * 397) ^ Ticker.GetHashCode();
            hashCode = (hashCode * 397) ^ MaximumPublishedLayers.GetHashCode();
            hashCode = (hashCode * 397) ^ (FormatPrice != null ? FormatPrice.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ RoundingPrecision.GetHashCode();
            hashCode = (hashCode * 397) ^ MinSubmitSize.GetHashCode();
            hashCode = (hashCode * 397) ^ MaxSubmitSize.GetHashCode();
            hashCode = (hashCode * 397) ^ IncrementSize.GetHashCode();
            hashCode = (hashCode * 397) ^ MinimumQuoteLife.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)LayerFlags;
            hashCode = (hashCode * 397) ^ (int)LastTradedFlags;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"SourceTickerQuoteInfo({nameof(SourceId)}: {SourceId}, {nameof(Source)}: {Source}, " +
        $"{nameof(TickerId)}: {TickerId}, {nameof(Ticker)}: {Ticker},  {nameof(RoundingPrecision)}: {RoundingPrecision}, " +
        $"{nameof(MinSubmitSize)}: {MinSubmitSize}, {nameof(MaxSubmitSize)}: {MaxSubmitSize}, " +
        $"{nameof(IncrementSize)}: {IncrementSize}, {nameof(MinimumQuoteLife)}: {MinimumQuoteLife}, " +
        $"{nameof(LayerFlags)}: {LayerFlags}, {nameof(MaximumPublishedLayers)}: {MaximumPublishedLayers}, " +
        $"{nameof(LastTradedFlags)}: {LastTradedFlags} }}";
}

public class PQSourceTickerInfoResponse : VersionedMessage
{
    public PQSourceTickerInfoResponse() => Version = 1;

    public PQSourceTickerInfoResponse(IEnumerable<ISourceTickerQuoteInfo> requestSourceTickerIds) : this() =>
        SourceTickerQuoteInfos.AddRange(requestSourceTickerIds);

    private PQSourceTickerInfoResponse(PQSourceTickerInfoResponse toClone) : this()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    public override uint MessageId => (uint)PQMessageIds.SourceTickerInfoResponse;

    public List<ISourceTickerQuoteInfo> SourceTickerQuoteInfos { get; } = new();

    public override IVersionedMessage CopyFrom(IVersionedMessage source
        , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is PQSourceTickerInfoResponse pqSourceTickerInfoResponse)
        {
            SourceTickerQuoteInfos.Clear();
            SourceTickerQuoteInfos.AddRange(pqSourceTickerInfoResponse.SourceTickerQuoteInfos);
        }

        return this;
    }

    public override IVersionedMessage Clone() =>
        Recycler?.Borrow<PQSourceTickerInfoResponse>().CopyFrom(this) ?? new PQSourceTickerInfoResponse(this);

    protected bool Equals(PQSourceTickerInfoResponse other)
    {
        var arraysSame = SourceTickerQuoteInfos.SequenceEqual(other.SourceTickerQuoteInfos);
        return arraysSame;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((IPQSnapshotIdsRequest)obj);
    }

    public override int GetHashCode() => SourceTickerQuoteInfos.GetHashCode();
}
