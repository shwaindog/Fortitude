#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Trading.Orders.Server;

public interface IOrderUpdate : ITradingMessage, IStoreState<IOrderUpdate>
{
    IOrder? Order { get; set; }
    OrderUpdateEventType OrderUpdateType { get; set; }
    DateTime AdapterUpdateTime { get; set; }
    DateTime ClientReceivedTime { get; set; }

    new IOrderUpdate Clone();
}
