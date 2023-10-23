namespace FortitudeMarketsApi.Trading.Orders.Client;

public interface IOrderAmendRequest : IOrderSubmitRequest
{
    IOrderAmend? Amendment { get; set; }
}
