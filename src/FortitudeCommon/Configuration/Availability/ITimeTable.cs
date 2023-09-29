using System;
using System.Collections.Generic;

namespace FortitudeCommon.Configuration.Availability
{
    public interface ITimeTable : IAvailability
    {
        TimeZone HostTimeZone { get; }
        IWeeklyTimeTable RegularTimeTable { get; }
        IList<IFixedCalendarDate> AnnualHolidays { get; }
        IList<IDateTimePeriod> ShiftingHolidays { get; }
        IList<IDateTimePeriod> ExtraAvailability { get; }
        ITimeTable Clone();
    } 
}