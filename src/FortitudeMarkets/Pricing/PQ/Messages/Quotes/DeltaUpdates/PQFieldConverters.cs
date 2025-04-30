// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

public static class PQFieldConverters
{
    // reason it is broken into hours and sub hours is that the hour component updates very infrequently
    // could have used minutes but then extra data would be sent every minute instead of hour.
    // Would rather have 10 nano precision and updates to the date component every hour than nano precision
    // and updates to the date component every minute.
    public static void UpdateHoursFromUnixEpoch(ref DateTime updateDatePart, uint hoursFromUnixEpoch)
    {
        updateDatePart =
            new DateTime(
                         DateTime.UnixEpoch.Ticks + hoursFromUnixEpoch * TimeSpan.TicksPerHour +
                         updateDatePart.Ticks % TimeSpan.TicksPerHour, DateTimeKind.Utc);
    }

    public static void UpdateSubHourComponent(ref DateTime updateSubHour, long tensOfNanos)
    {
        updateSubHour = updateSubHour.AddTicks(tensOfNanos / 10 - updateSubHour.Ticks % TimeSpan.TicksPerHour);
    }

    public static DateTime DateTimeFromParts(uint hoursFromUnixEpoch, long tensOfNanoSeconds) =>
        new(DateTimeConstants.UnixEpochTicks + hoursFromUnixEpoch * TimeSpan.TicksPerHour +
            tensOfNanoSeconds / 10, DateTimeKind.Utc);

    public static uint GetHoursFromUnixEpoch
        (this DateTime ts) =>
        (uint)((Math.Max(ts.Ticks, DateTime.UnixEpoch.Ticks) - DateTime.UnixEpoch.Ticks) / TimeSpan.TicksPerHour);

    public static long GetSubHourComponent(this DateTime ts) => ts.Ticks % TimeSpan.TicksPerHour * 10;

    public static long AppendUintToMakeLong(this ushort highestNumberByte, uint lowestBytes)
    {
        var result = (long)highestNumberByte << 32;
        result |= lowestBytes;
        return result;
    }

    public static ushort BreakLongToUShortAndUint(this long breakThis, out uint lowestBytes)
    {
        lowestBytes = (uint)breakThis;
        breakThis   = breakThis >> 32;
        return (ushort)breakThis;
    }
}
