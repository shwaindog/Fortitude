// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarketsApi.Pricing.Quotes.LayeredBook;

// The reason for so many sub-types of IPriceVolume Layer is to reduce Quote memory waste when
// duplicated 1000s of times in a ring.
public interface IPriceVolumeLayer : IReusableObject<IPriceVolumeLayer>, IInterfacesComparable<IPriceVolumeLayer>
{
    LayerType  LayerType          { get; }
    LayerFlags SupportsLayerFlags { get; }

    decimal Price   { get; }
    decimal Volume  { get; }
    bool    IsEmpty { get; }
}

public interface IMutablePriceVolumeLayer : IPriceVolumeLayer
{
    new decimal Price   { get; set; }
    new decimal Volume  { get; set; }
    new bool    IsEmpty { get; set; }
}
