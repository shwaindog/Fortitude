namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

[Flags]
public enum LayerFieldUpdatedFlags : ushort
{
    None = 0x00
    , PriceUpdatedFlag = 0x01
    , VolumeUpdatedFlag = 0x02
    , SourceNameUpdatedFlag = 0x04
    , ExecutableUpdatedFlag = 0x08
    , SourceQuoteRefUpdatedFlag = 0x10
    , ValueDateUpdatedFlag = 0x20
}
