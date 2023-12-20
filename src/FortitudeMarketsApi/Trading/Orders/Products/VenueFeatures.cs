namespace FortitudeMarketsApi.Trading.Orders.Products;

[Flags]
public enum VenueFeatures
{
    None = 0x00
    , Amends = 0x01
    , IceBerg = 0x02
    , Pegged = 0x04
    , WaterMark = 0x08
}
