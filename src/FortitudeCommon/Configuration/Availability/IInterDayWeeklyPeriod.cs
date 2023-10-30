using System;

namespace FortitudeCommon.Configuration.Availability
{
    public interface IInterDayWeeklyPeriod : IAvailability
    {
        TimeZoneInfo StartTimeZone { get; }
        DayOfWeek StartDayOfWeek { get;  }
        int StartHour { get; }
        TimeZoneInfo StopTimeZone { get;  }
        DayOfWeek StopDayOfWeek { get;  }
        int StopHour { get; }
    }
}