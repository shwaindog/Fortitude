// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing;

#endregion

namespace FortitudeMarketsApi.Indicators.Pricing;

public readonly struct IndicatorSourceTickerIdentifier
{
    public readonly long IndicatorSourceTickerId;

    public IndicatorSourceTickerIdentifier(long indicatorSourceTickerId) => IndicatorSourceTickerId = indicatorSourceTickerId;

    public IndicatorSourceTickerIdentifier(ushort indicatorId, PricingInstrumentId pricingInstrumentId) =>
        IndicatorSourceTickerId = ((long)pricingInstrumentId.SourceTickerId << 32) | ((long)indicatorId << 16);

    public uint   SourceTickerId => (uint)((SourceId << 16) | TickerId);
    public ushort IndicatorId    => (ushort)((IndicatorSourceTickerId >> 16) & 0xFFFF);
    public ushort SourceId       => (ushort)((IndicatorSourceTickerId >> 32) & 0xFFFF);
    public ushort TickerId       => (ushort)((IndicatorSourceTickerId >> 48) & 0xFFFF);
    public string IndicatorName  => IndicatorExtensions.GetRegisteredIndicatorName(IndicatorId);
    public string Ticker         => SourceTickerIdentifierExtensions.GetRegisteredTickerName(SourceTickerId);
    public string Source         => SourceTickerIdentifierExtensions.GetRegisteredSourceName(SourceId);

    public static implicit operator SourceTickerIdentifier(IndicatorSourceTickerIdentifier identifier) =>
        new(identifier.SourceId, identifier.TickerId);
}

public static class IndicatorSourceTickerIdentifierExtensions
{
    // public static bool Register
}
