using System;
using System.Collections.Generic;
using System.Linq;

namespace FortitudeCommon.Configuration.Availability
{
    public class DayOfWeekAvailability : IDayOfWeekAvailability
    {
        private readonly TimeSpan oneYearTimespan = new TimeSpan(365,0,0,0);
        public DayOfWeekAvailability(DayOfWeek dayOfWeek, IList<ITimeZoneTimePeriod> expectedRestartPeriod)
        {
            DayOfWeek = dayOfWeek;
            ExpectedRestartPeriod = expectedRestartPeriod;
        }

        public DayOfWeek DayOfWeek { get; private set; }
        public IList<ITimeZoneTimePeriod> ExpectedRestartPeriod { get; private set; }
        public bool ShouldBeUp(DateTimeOffset atThisDateTime)
        {
            return !(  from restartTimePeriod in ExpectedRestartPeriod 
                let timeZoneDateTime = restartTimePeriod.TimeZone.ToLocalTime(atThisDateTime.UtcDateTime) 
                where timeZoneDateTime.DayOfWeek == DayOfWeek 
                where restartTimePeriod.IntraDayAvailability.StartTime > timeZoneDateTime.TimeOfDay
                      && ((restartTimePeriod.IntraDayAvailability.EndTime) 
                          < timeZoneDateTime.TimeOfDay) 
                select restartTimePeriod).Any();
        }

        public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
        {
            var restartsForDay = (from restartTimePeriod in ExpectedRestartPeriod
                let timeZoneDateTime = restartTimePeriod.TimeZone.ToLocalTime(fromNow.UtcDateTime)
                where timeZoneDateTime.DayOfWeek == DayOfWeek
                where restartTimePeriod.IntraDayAvailability.StartTime > timeZoneDateTime.TimeOfDay
                select restartTimePeriod.IntraDayAvailability.StartTime - timeZoneDateTime.TimeOfDay).ToArray();
            return restartsForDay.Any() ? restartsForDay.Min().Value : oneYearTimespan;
        }

        public DateTimeOffset NextScheduledRestartTime(DateTimeOffset fromNow)
        {
            return fromNow + ExpectedRemainingUpTime(fromNow);
        }
    }
}