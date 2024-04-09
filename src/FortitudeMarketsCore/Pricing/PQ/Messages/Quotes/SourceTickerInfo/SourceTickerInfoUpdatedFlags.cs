namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.SourceTickerInfo;

[Flags]
public enum SourceTickerInfoUpdatedFlags : uint
{
    None = 0x0000
    , SourceTickerId = 0x0001
    , SourceName = 0x0002
    , TickerName = 0x0004
    , RoundingPrecision = 0x0008
    , MinSubmitSize = 0x0010
    , MaxSubmitSize = 0x0020
    , IncrementSize = 0x0040
    , MinimumQuoteLife = 0x0080
    , LayerFlags = 0x0100
    , MaximumPublishedLayers = 0x0200
    , LastTradedFlags = 0x0400
}
