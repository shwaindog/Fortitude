// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

public static class PQFieldConverters
{
    private const long DateTimeTicksToNanoSeconds = 100;
    private const long TicksIn2Mins = TimeSpan.TicksPerMinute * 2;

    // reason it is broken into hours and sub hours is that the hour component updates very infrequently
    // could have used minutes but then extra data would be sent every minute instead of hour.
    // Would rather have 10 nano precision and updates to the date component every hour than nano precision
    // and updates to the date component every minute.
    public static void Update2MinuteIntervalsFromUnixEpoch(ref DateTime updateDatePart, uint twoMinIntervalsFromUnixEpoch)
    {
        updateDatePart =
            new DateTime(
                         DateTime.UnixEpoch.Ticks + twoMinIntervalsFromUnixEpoch * TicksIn2Mins +
                         updateDatePart.Ticks % TicksIn2Mins, DateTimeKind.Utc);
    }

    public static void UpdateSub2MinComponent(ref DateTime updateSubHour, long nanoSeconds)
    {
        updateSubHour = updateSubHour.AddTicks(nanoSeconds / DateTimeTicksToNanoSeconds - updateSubHour.Ticks % TicksIn2Mins);
    }

    public static DateTime DateTimeFromParts(uint hoursFromUnixEpoch, long tensOfNanoSeconds) =>
        new(DateTimeConstants.UnixEpochTicks + hoursFromUnixEpoch * TicksIn2Mins +
            tensOfNanoSeconds / 10, DateTimeKind.Utc);

    public static uint Get2MinIntervalsFromUnixEpoch
        (this DateTime ts) =>
        (uint)((Math.Max(ts.Ticks, DateTime.UnixEpoch.Ticks) - DateTime.UnixEpoch.Ticks) / TicksIn2Mins);

    public static long GetSub2MinComponent(this DateTime ts) => (ts.Ticks % TicksIn2Mins) * DateTimeTicksToNanoSeconds;

    public static long AppendScaleFlagsToUintToMakeLong(this PQFieldFlags highestNumberBytes, uint lowestBytes)
    {
        var result = ((long)highestNumberBytes & (long)PQFieldFlags.NegativeAndScaleMask) << 32;
        result |= lowestBytes;
        return result;
    }

    public static PQFieldFlags BreakLongToUShortAndScaleFlags(this long breakThis, out uint lowestBytes)
    {
        lowestBytes = (uint)breakThis;
        breakThis   = breakThis >> 32;
        return (PQFieldFlags)(breakThis & (long)PQFieldFlags.NegativeAndScaleMask);
    }
}
