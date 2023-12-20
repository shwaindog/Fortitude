#region

using FortitudeCommon.Types;
using FortitudeMarketsApi.Pricing.LayeredBook;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes;

public interface ILevel2Quote : ILevel1Quote, ICloneable<ILevel2Quote>
{
    IOrderBook BidBook { get; }
    bool IsBidBookChanged { get; }
    IOrderBook AskBook { get; }
    bool IsAskBookChanged { get; }
    new ILevel2Quote Clone();
}
