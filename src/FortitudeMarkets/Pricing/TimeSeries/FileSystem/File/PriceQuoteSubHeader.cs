// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.Storage.TimeSeries.FileSystem.File.Header;

#endregion

namespace FortitudeMarkets.Pricing.TimeSeries.FileSystem.File;

public interface IPriceFileHeader : IFileSubHeader
{
    IPricingInstrumentId PricingInstrumentId { get; }
}
