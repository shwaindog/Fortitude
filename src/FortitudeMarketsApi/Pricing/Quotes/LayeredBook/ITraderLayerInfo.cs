// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

public interface ITraderLayerInfo : IReusableObject<ITraderLayerInfo>, IInterfacesComparable<ITraderLayerInfo>
{
    string? TraderName   { get; }
    decimal TraderVolume { get; }
    bool    IsEmpty      { get; }
}

public interface IMutableTraderLayerInfo : ITraderLayerInfo
{
    new string? TraderName   { get; set; }
    new decimal TraderVolume { get; set; }
    new bool    IsEmpty      { get; set; }

    new IMutableTraderLayerInfo Clone();
}
