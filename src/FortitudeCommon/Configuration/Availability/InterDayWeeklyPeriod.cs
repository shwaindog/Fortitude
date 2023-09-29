using System;

namespace FortitudeCommon.Configuration.Availability
{
    public class InterDayWeeklyPeriod : IInterDayWeeklyPeriod
    {
        public InterDayWeeklyPeriod(TimeZone startTimeZone, DayOfWeek startDayOfWeek, int startHour, TimeZone stopTimeZone, DayOfWeek stopDayOfWeek, int stopHour)
        {
            StartTimeZone = startTimeZone;
            StartDayOfWeek = startDayOfWeek;
            StartHour = startHour;
            StopTimeZone = stopTimeZone;
            StopDayOfWeek = stopDayOfWeek;
            StopHour = stopHour;
        }

        public TimeZone StartTimeZone { get; private set; }
        public DayOfWeek StartDayOfWeek { get; private set; }
        public int StartHour { get; private set; }
        public TimeZone StopTimeZone { get; private set; }
        public DayOfWeek StopDayOfWeek { get; private set; }
        public int StopHour { get; private set; }

        public bool ShouldBeUp(DateTimeOffset atThisDateTime)
        {
            DateTime toStartTimeZoneDateTime = StartTimeZone.ToLocalTime(atThisDateTime.UtcDateTime);
            DateTime toEndTimeZoneDateTime = StopTimeZone.ToLocalTime(atThisDateTime.UtcDateTime);
            DayOfWeek startTimeZoneCurrentDayOfWeek = toStartTimeZoneDateTime.DayOfWeek;
            DayOfWeek stopTimeZoneCurrentDayOfWeek = toEndTimeZoneDateTime.DayOfWeek;
            if (startTimeZoneCurrentDayOfWeek == StartDayOfWeek)
            {
                if (stopTimeZoneCurrentDayOfWeek == StopDayOfWeek)
                {
                    return toStartTimeZoneDateTime.Hour >= StartHour && toEndTimeZoneDateTime.Hour < StopHour;
                }
                return toStartTimeZoneDateTime.Hour >= StartHour;
            }
            if (stopTimeZoneCurrentDayOfWeek == StopDayOfWeek)
            {
                return toEndTimeZoneDateTime.Hour < StopHour;
            }
            return toStartTimeZoneDateTime.DayOfWeek > StartDayOfWeek &&
                    toEndTimeZoneDateTime.DayOfWeek < StopDayOfWeek;
        }

        public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
        {
            if (!ShouldBeUp(fromNow))
            {
                return TimeSpan.Zero;
            }
            DateTime toEndTimeZoneDateTime = StopTimeZone.ToLocalTime(fromNow.UtcDateTime);
            TimeSpan sumTimeSpan = TimeSpan.Zero;
            for (DayOfWeek currentDayOfWeek = toEndTimeZoneDateTime.DayOfWeek; currentDayOfWeek < StopDayOfWeek; currentDayOfWeek++)
            {
                sumTimeSpan += TimeSpan.FromDays(1);
            }
            return sumTimeSpan + TimeSpan.FromHours(StopHour) - toEndTimeZoneDateTime.TimeOfDay;
        }

        public DateTimeOffset NextScheduledRestartTime(DateTimeOffset fromNow)
        {

            DateTime toStartTimeZoneDateTime = StartTimeZone.ToLocalTime(fromNow.UtcDateTime);
            TimeSpan sumTimeSpan = TimeSpan.Zero;
            if (ShouldBeUp(fromNow))
            {
                sumTimeSpan = ExpectedRemainingUpTime(fromNow);
                toStartTimeZoneDateTime += sumTimeSpan;
            }
            
            for (DayOfWeek currentDayOfWeek = toStartTimeZoneDateTime.DayOfWeek; currentDayOfWeek > StartDayOfWeek; currentDayOfWeek = (DayOfWeek)((int)(currentDayOfWeek+1) % (int)(DayOfWeek.Saturday + 1)))
            {
                sumTimeSpan += TimeSpan.FromDays(1);
            }
            sumTimeSpan += TimeSpan.FromHours(StartHour) - toStartTimeZoneDateTime.TimeOfDay;
            return fromNow + sumTimeSpan;
        }
    }
}