using System;

namespace FortitudeCommon.Chronometry
{
    public static class DateTimeConstants
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly long UnixEpochTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
        public const string FullDateTimeFormatToMicro = "O";
    }
}