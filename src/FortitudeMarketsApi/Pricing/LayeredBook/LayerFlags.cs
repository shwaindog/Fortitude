namespace FortitudeMarketsApi.Pricing.LayeredBook;

[Flags]
public enum LayerFlags : uint
{
    None = 0x00
    , Price = 0x01
    , Volume = 0x02
    , SourceQuoteReference = 0x04
    , SourceName = 0x08
    , TraderCount = 0x10
    , TraderName = 0x30
    , TraderSize = 0x50
    , ValueDate = 0x80
    , Executable = 0x1_00
    , Reserved = 0x0E_00
    , UserExtensibile = 0xF0_00
}

public static class LayerFlagsExtensions
{
    public static LayerFlags SupportedLayerFlags(this LayerType layerType)
    {
        switch (layerType)
        {
            case LayerType.PriceVolume: return LayerFlags.Price | LayerFlags.Volume;
            case LayerType.ValueDatePriceVolume: return LayerFlags.ValueDate | LayerFlags.Price | LayerFlags.Volume;
            case LayerType.SourcePriceVolume: return LayerFlags.SourceName | LayerFlags.Price | LayerFlags.Volume;
            case LayerType.SourceQuoteRefPriceVolume:
                return LayerFlags.SourceQuoteReference | LayerFlags.SourceName | LayerFlags.Price | LayerFlags.Volume;
            case LayerType.TraderPriceVolume:
                return LayerFlags.TraderCount | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.Price | LayerFlags.Volume;
            case LayerType.SourceQuoteRefTraderValueDatePriceVolume:
                return LayerFlags.ValueDate | LayerFlags.SourceQuoteReference | LayerFlags.SourceName | LayerFlags.TraderCount
                       | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.Price | LayerFlags.Volume;
            default: return LayerFlags.None;
        }
    }

    public static bool HasPrice(this LayerFlags flags) => (flags & LayerFlags.Price) > 0;
    public static bool HasVolume(this LayerFlags flags) => (flags & LayerFlags.Volume) > 0;
    public static bool HasSourceName(this LayerFlags flags) => (flags & LayerFlags.SourceName) > 0;
    public static bool HasExecutable(this LayerFlags flags) => (flags & LayerFlags.Executable) > 0;
    public static bool HasSourceQuoteReference(this LayerFlags flags) => (flags & LayerFlags.SourceQuoteReference) > 0;
    public static bool HasTraderCount(this LayerFlags flags) => (flags & LayerFlags.TraderCount) > 0;
    public static bool HasTraderName(this LayerFlags flags) => (flags & LayerFlags.TraderName) > 0;
    public static bool HasTraderSize(this LayerFlags flags) => (flags & LayerFlags.TraderSize) > 0;
    public static bool HasValueDate(this LayerFlags flags) => (flags & LayerFlags.ValueDate) > 0;
    public static bool HasAllOf(this LayerFlags flags, LayerFlags checkAllFound) => (flags & checkAllFound) == checkAllFound;
    public static bool HasNoneOf(this LayerFlags flags, LayerFlags checkNonAreSet) => (flags & checkNonAreSet) == 0;
    public static bool HasAnyOf(this LayerFlags flags, LayerFlags checkAnyAreFound) => (flags & checkAnyAreFound) > 0;
    public static bool IsExactly(this LayerFlags flags, LayerFlags checkAllFound) => flags == checkAllFound;
}
