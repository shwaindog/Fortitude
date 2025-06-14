namespace FortitudeMarkets.Config;

[Flags]
public enum MarketConnectionType
{
    Unknown = 0x0000
    , Pricing = 0x0001
    , Trading = 0x0002
}
