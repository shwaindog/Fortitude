namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;

[Flags]
public enum PQMessageFlags : byte
{
    None = 0x00
    , IsQuote = 0x01
    , PublishAll = 0x04
}
