// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.Options;

public enum TimeStyleFormat
{
    Default
  , StringYyyyMMddToss
  , StringYyyyMMddToms
  , StringYyyyMMddTous
  , NanosFromUnixEpoch
  , MicrosFromUnixEpoch
  , MillsFromUnixEpoch
  , SecondsFromUnixEpoch
  , TicksFrom001Ad
}

public static class TimeStyleFormatExtensions
{
    public static bool TimeFormatIsNumber(this TimeStyleFormat timeStyle) =>
        timeStyle is TimeStyleFormat.SecondsFromUnixEpoch
                  or TimeStyleFormat.MillsFromUnixEpoch
                  or TimeStyleFormat.MicrosFromUnixEpoch
                  or TimeStyleFormat.NanosFromUnixEpoch
                  or TimeStyleFormat.TicksFrom001Ad;
}
