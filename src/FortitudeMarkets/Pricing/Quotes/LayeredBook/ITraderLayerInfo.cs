// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public interface ITraderLayerInfo : IReusableObject<ITraderLayerInfo>, IInterfacesComparable<ITraderLayerInfo>
{
    string? TraderName   { get; }
    decimal TraderVolume { get; }
    bool    IsEmpty      { get; }
}

public interface IMutableTraderLayerInfo : ITraderLayerInfo
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    new string? TraderName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal TraderVolume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new bool IsEmpty { get; set; }

    new IMutableTraderLayerInfo Clone();
}
