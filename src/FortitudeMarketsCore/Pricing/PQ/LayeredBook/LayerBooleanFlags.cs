namespace FortitudeMarketsCore.Pricing.PQ.LayeredBook;

[Flags]
public enum LayerBooleanFlags : ushort
{
    None = 0x0000
    , IsExecutableFlag = 0x0001
}
