// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.Options;

public enum DateTimeStyleFormat
{
    Default
  , StringYyyyMMddOnly
  , StringYyyyMMddToss
  , StringYyyyMMddToms
  , StringYyyyMMddTous
  , NanosFromUnixEpoch
  , MicrosFromUnixEpoch
  , MillsFromUnixEpoch
  , SecondsFromUnixEpoch
  , TicksFrom001Ad
}
public enum TimeStyleFormat
{
    Default
  , StringHHmmss
  , StringHHmmssToMs
  , StringHHmmssToUs
  , StringHHmmssToTicks
}

public static class TimeStyleFormatExtensions
{
    public static bool TimeFormatIsNumber(this DateTimeStyleFormat dateTimeStyle) =>
        dateTimeStyle is DateTimeStyleFormat.SecondsFromUnixEpoch
                  or DateTimeStyleFormat.MillsFromUnixEpoch
                  or DateTimeStyleFormat.MicrosFromUnixEpoch
                  or DateTimeStyleFormat.NanosFromUnixEpoch
                  or DateTimeStyleFormat.TicksFrom001Ad;
}
