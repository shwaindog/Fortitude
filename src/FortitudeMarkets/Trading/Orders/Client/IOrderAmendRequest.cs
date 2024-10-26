namespace FortitudeMarkets.Trading.Orders.Client;

public interface IOrderAmendRequest : IOrderSubmitRequest
{
    IOrderAmend? Amendment { get; set; }
}
