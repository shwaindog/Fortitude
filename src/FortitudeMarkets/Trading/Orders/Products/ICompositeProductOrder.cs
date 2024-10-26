#region

using FortitudeMarkets.Trading.Orders.Venues;

#endregion

namespace FortitudeMarkets.Trading.Orders.Products;

public interface ICompositeProductOrder : IProductOrder
{
    int NumberOfLegs { get; }
    List<string> AllTickers { get; }
    List<IVenue> Markets { get; }
}
