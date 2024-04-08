namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

[Flags]
public enum PQMessageFlags : byte
{
    None = 0x00
    , IsQuote = 0x01
    , PublishAll = 0x04
}
