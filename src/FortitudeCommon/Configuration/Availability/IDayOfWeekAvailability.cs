using System;
using System.Collections.Generic;

namespace FortitudeCommon.Configuration.Availability
{
    public interface IDayOfWeekAvailability : IAvailability
    {
        DayOfWeek DayOfWeek { get; }
        IList<ITimeZoneTimePeriod> ExpectedRestartPeriod { get; }
    }
}