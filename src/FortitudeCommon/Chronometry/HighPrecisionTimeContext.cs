using System;

namespace FortitudeCommon.Chronometry
{
    public sealed class HighPrecisionTimeContext : ITimeContext
    {
        private readonly DateTimeEx dtx = new DateTimeEx(10);

        public DateTime UtcNow => dtx.UtcNow;

        public DateTime LocalTimeNow => dtx.UtcNow.ToLocalTime();
    }
}