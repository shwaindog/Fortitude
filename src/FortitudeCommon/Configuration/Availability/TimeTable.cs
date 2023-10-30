namespace FortitudeCommon.Configuration.Availability
{
    public class TimeTable : ITimeTable
    {
        public TimeTable(TimeZoneInfo? hostTimeZone = null, IWeeklyTimeTable? regularTimeTable = null, 
            IList<IFixedCalendarDate>? annualHolidays = null, 
            IList<IDateTimePeriod>? shiftingHolidays = null, 
            IList<IDateTimePeriod>? extraAvailability = null)
        {
            HostTimeZone = hostTimeZone ?? TimeZoneInfo.Local;
            RegularTimeTable = regularTimeTable ??  new WeeklyTimeTable(
                new InterDayWeeklyPeriod(
                    TimeZoneInfoExtensions.NewZealand,
                    DayOfWeek.Monday,
                    9,
                    TimeZoneInfoExtensions.NewYork,
                    DayOfWeek.Friday,
                    5
                ), new List<IDayOfWeekAvailability>()
                {
                    new DayOfWeekAvailability(DayOfWeek.Monday, new List<ITimeZoneTimePeriod>{ TimeZoneTimePeriod.NewYorkCloseRestart} ),
                    new DayOfWeekAvailability(DayOfWeek.Tuesday, new List<ITimeZoneTimePeriod>{ TimeZoneTimePeriod.NewYorkCloseRestart} ),
                    new DayOfWeekAvailability(DayOfWeek.Wednesday, new List<ITimeZoneTimePeriod>{ TimeZoneTimePeriod.NewYorkCloseRestart} ),
                    new DayOfWeekAvailability(DayOfWeek.Thursday, new List<ITimeZoneTimePeriod>{ TimeZoneTimePeriod.NewYorkCloseRestart} ),
                }
            );
            AnnualHolidays = annualHolidays ?? new List<IFixedCalendarDate>();
            ShiftingHolidays = shiftingHolidays ?? new List<IDateTimePeriod>();
            ExtraAvailability = extraAvailability ?? new List<IDateTimePeriod>();
        }

        public TimeTable(ITimeTable toClone)
        {
            HostTimeZone = toClone.HostTimeZone;
            RegularTimeTable = toClone.RegularTimeTable;
            AnnualHolidays = toClone.AnnualHolidays;
            ShiftingHolidays = toClone.ShiftingHolidays;
            ExtraAvailability = toClone.ExtraAvailability;
        }

        public bool ShouldBeUp(DateTimeOffset atThisDateTime)
        {
            if (!RegularTimeTable.ShouldBeUp(atThisDateTime))
            {
                return ExtraAvailability.Any(tp => tp.DateTimeContained(atThisDateTime));
            }
            return !(AnnualHolidays.Any(fcd => fcd.DateTimeContained(atThisDateTime))
                     || ShiftingHolidays.Any(dtp => dtp.DateTimeContained(atThisDateTime)))
                   || ExtraAvailability.Any(tp => tp.DateTimeContained(atThisDateTime));
        }

        public TimeSpan ExpectedRemainingUpTime(DateTimeOffset fromNow)
        {
            if (!ShouldBeUp(fromNow)) return TimeSpan.Zero;
            var allRemainingUpTimes =
                (from remainingUpTimes in new[] { RegularTimeTable.ExpectedRemainingUpTime(fromNow) }
                     .Concat(from fcd in AnnualHolidays
                             select fcd.TimeToNextPeriod(fromNow)
                    ).Concat(from dtp in ShiftingHolidays
                             where dtp.TimeToPeriod(fromNow) > TimeSpan.Zero
                             select dtp.TimeToPeriod(fromNow))
                 select remainingUpTimes
                ).ToArray();
            if (allRemainingUpTimes.Any())
            {
                return allRemainingUpTimes.Min();
            }
            return TimeSpan.Zero;
        }

        public DateTimeOffset NextScheduledRestartTime(DateTimeOffset fromNow)
        {
            DateTimeOffset weeklyTimeTableRestart = RegularTimeTable.NextScheduledRestartTime(fromNow);
            var allPossibleRestartTime = new[] { weeklyTimeTableRestart }.Concat(
                from dtp in ExtraAvailability
                where dtp.TimeToPeriod(fromNow) > TimeSpan.Zero
                select fromNow + dtp.TimeToPeriod(fromNow)
                ).ToArray();

            if (allPossibleRestartTime.Any())
            {
                return allPossibleRestartTime.Min();
            }
            return DateTimeOffset.MaxValue;
        }

        public TimeZoneInfo HostTimeZone { get;}

        public IWeeklyTimeTable RegularTimeTable { get; }
        public IList<IFixedCalendarDate> AnnualHolidays { get;}
        public IList<IDateTimePeriod> ShiftingHolidays { get;}
        public IList<IDateTimePeriod> ExtraAvailability { get;}

        public ITimeTable Clone()
        {
            return new TimeTable(this);
        }

        protected bool Equals(TimeTable other)
        {
            return Equals(HostTimeZone, other.HostTimeZone) && Equals(RegularTimeTable, other.RegularTimeTable) && Equals(AnnualHolidays, other.AnnualHolidays) && Equals(ShiftingHolidays, other.ShiftingHolidays) && Equals(ExtraAvailability, other.ExtraAvailability); 
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TimeTable) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (HostTimeZone != null ? HostTimeZone.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RegularTimeTable != null ? RegularTimeTable.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (AnnualHolidays != null ? AnnualHolidays.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ShiftingHolidays != null ? ShiftingHolidays.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ExtraAvailability != null ? ExtraAvailability.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
