using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface IInterDayWeeklyPeriod : IAvailability
    {
        TimeZone StartTimeZone { get; }
        DayOfWeek StartDayOfWeek { get;  }
        int StartHour { get; }
        TimeZone StopTimeZone { get;  }
        DayOfWeek StopDayOfWeek { get;  }
        int StopHour { get; }
    }
}