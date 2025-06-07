using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.MarketEvents;

public interface IMarketTradingStateEvent : IReusableObject<IMarketTradingStateEvent>, IInterfacesComparable<IMarketTradingStateEvent>, IShowsEmpty
{
    uint EventSequenceId { get; }

    MarketTradingStatusType MarketTradingStatus { get; }

    DateTime StartTime { get; }

    int EstimatedLengthMs { get; }

    DateTime? EndedAt { get; }
}

public interface IMutableMarketTradingStateEvent : IMarketTradingStateEvent, ICloneable<IMutableMarketTradingStateEvent>, IEmptyable, ITrackableReset<IMutableMarketTradingStateEvent>
{
    new uint EventSequenceId { get; set; }

    new MarketTradingStatusType MarketTradingStatus { get; set; }

    new DateTime StartTime { get; set; }

    new int EstimatedLengthMs { get; set; }
    
    new DateTime? EndedAt { get; set; }

    
    new IMutableMarketTradingStateEvent Clone();
}