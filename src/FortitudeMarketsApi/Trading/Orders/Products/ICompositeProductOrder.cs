#region

using FortitudeMarketsApi.Trading.Orders.Venues;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Products;

public interface ICompositeProductOrder : IProductOrder
{
    int NumberOfLegs { get; }
    List<string> AllTickers { get; }
    List<IVenue> Markets { get; }
}
