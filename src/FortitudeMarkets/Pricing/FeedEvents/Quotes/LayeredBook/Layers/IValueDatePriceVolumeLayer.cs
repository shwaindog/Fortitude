// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public interface IValueDatePriceVolumeLayer : IPriceVolumeLayer, ICloneable<IValueDatePriceVolumeLayer>
{
    DateTime                       ValueDate { get; }
    new IValueDatePriceVolumeLayer Clone();
}

public interface IMutableValueDatePriceVolumeLayer : IMutablePriceVolumeLayer, IValueDatePriceVolumeLayer,
    ICloneable<IMutableValueDatePriceVolumeLayer>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new DateTime ValueDate { get; set; }

    new IMutableValueDatePriceVolumeLayer Clone();
}
