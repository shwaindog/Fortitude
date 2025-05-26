using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.LastTraded;

public interface IOnTickLastTraded : ILastTradedList, IInterfacesComparable<IOnTickLastTraded>, ICloneable<IOnTickLastTraded>
  , IShowsEmpty
{  
    new IOnTickLastTraded Clone();
}

public interface IMutableOnTickLastTraded : IOnTickLastTraded, IMutableLastTradedList, ICloneable<IMutableOnTickLastTraded>
  , ITrackableReset<IMutableOnTickLastTraded>, IEmptyable
{
    new IMutableOnTickLastTraded Clone();

    new IMutableOnTickLastTraded ResetWithTracking();
}