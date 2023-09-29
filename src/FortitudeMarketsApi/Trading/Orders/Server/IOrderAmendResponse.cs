namespace FortitudeMarketsApi.Trading.Orders.Server
{
    public interface IOrderAmendResponse : IOrderUpdate
    {
        AmendType AmendType { get; set; }
        IOrderId OldOrderId { get; set; }
        new IOrderAmendResponse Clone();
    }
}
