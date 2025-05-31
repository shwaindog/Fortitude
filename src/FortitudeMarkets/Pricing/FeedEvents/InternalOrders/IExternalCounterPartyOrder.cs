// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents.InternalOrders;

public interface IAdditionalExternalCounterPartyOrderInfo : IReusableObject<IAdditionalExternalCounterPartyOrderInfo>, IShowsEmpty
  , IInterfacesComparable<IAdditionalExternalCounterPartyOrderInfo>
{
    int ExternalCounterPartyId { get; }

    string? ExternalCounterPartyName { get; }

    int ExternalTraderId { get; }

    string? ExternalTraderName { get; }
}

public interface IMutableAdditionalExternalCounterPartyOrderInfo : IAdditionalExternalCounterPartyOrderInfo
  , ICloneable<IMutableAdditionalExternalCounterPartyOrderInfo>, IEmptyable, ITrackableReset<IMutableAdditionalExternalCounterPartyOrderInfo>
  , ITransferState<IMutableAdditionalExternalCounterPartyOrderInfo>
{
    new int ExternalCounterPartyId { get; set; }

    new string? ExternalCounterPartyName { get; set; }

    new int ExternalTraderId { get; set; }

    new string? ExternalTraderName { get; set; }

    new IMutableAdditionalExternalCounterPartyOrderInfo Clone();
}

public interface IExternalCounterPartyOrder : IAnonymousOrder, IAdditionalExternalCounterPartyOrderInfo, ICloneable<IExternalCounterPartyOrder>
  , IInterfacesComparable<IExternalCounterPartyOrder>
{
    const OrderGenesisFlags HasExternalCounterPartyOrderInfoFlags
        = OrderGenesisFlags.IsExternalOrder | OrderGenesisFlags.HasExternalCounterPartyInfo;

    const OrderGenesisFlags AllExceptHasExternalCounterPartyInfoFlags = ~(HasExternalCounterPartyOrderInfoFlags);

    new IExternalCounterPartyOrder Clone();
}

public interface IMutableExternalCounterPartyOrder : IExternalCounterPartyOrder, IMutableAdditionalExternalCounterPartyOrderInfo,
    IMutableAnonymousOrder, ICloneable<IMutableExternalCounterPartyOrder>, ITransferState<IMutableExternalCounterPartyOrder>
  , IInterfacesComparable<IMutableExternalCounterPartyOrder>, ITrackableReset<IMutableExternalCounterPartyOrder>
{
    new IMutableExternalCounterPartyOrder ResetWithTracking();
    new IMutableExternalCounterPartyOrder Clone();
}
