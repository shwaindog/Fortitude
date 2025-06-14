// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarkets.Config.Availability;

public static class TimeZoneInfoExtensions
{
    public static readonly TimeZoneInfo NewYork    = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
    public static readonly TimeZoneInfo NewZealand = TimeZoneInfo.FindSystemTimeZoneById("New Zealand Standard Time");
}
