// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Config;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeMarkets.Config.Availability;

public interface IWeeklyTimesConfig : IInterfacesComparable<IWeeklyTimesConfig>
{
    TimeZoneInfo?  OverrideTimeZone   { get; set; }
    DayOfWeekFlags OverrideDaysOfWeek { get; set; }
    TimeSpan       TimeOfDay          { get; set; }

    IReadOnlyList<IWeeklyTimesConfig>?  OverrideWeeklyTimes { get; set; }
    ReusableList<TimeZonedWeekDayTime> TimeZonedWeekDayTimes();
}

public readonly struct WeeklyTimes
(
    DayOfWeekFlags overrideDaysOfWeek
  , TimeSpan timeOfDay
  , TimeZoneInfo? overrideTimeZone = null
  , IReadOnlyList<WeeklyTimes>? overrideWeeklyTimes = null)
{
    public TimeZoneInfo?  OverrideTimeZone   { get; } = overrideTimeZone;
    public DayOfWeekFlags OverrideDaysOfWeek { get; } = overrideDaysOfWeek;
    public TimeSpan       TimeOfDay          { get; } = timeOfDay;

    public IReadOnlyList<WeeklyTimes>? OverrideWeeklyTimes { get; } = overrideWeeklyTimes;
}

public class WeeklyTimesConfig : ConfigSection, IWeeklyTimesConfig
{
    public WeeklyTimesConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public WeeklyTimesConfig() { }

    public WeeklyTimesConfig
    (DayOfWeekFlags daysOfWeek, TimeSpan timeOfDay, IConfigurationRoot root, string path,
        IReadOnlyList<IWeeklyTimesConfig>? overrideWeeklyTimes = null) : base(root, path)
    {
        OverrideDaysOfWeek = daysOfWeek;
        TimeOfDay          = timeOfDay;

        if (overrideWeeklyTimes != null) OverrideWeeklyTimes = overrideWeeklyTimes;
    }

    public WeeklyTimesConfig(DayOfWeekFlags daysOfWeek, TimeSpan timeOfDay, IReadOnlyList<IWeeklyTimesConfig>? overrideWeeklyTimes = null)
        : this(daysOfWeek, timeOfDay, InMemoryConfigRoot, InMemoryPath, overrideWeeklyTimes) { }

    public WeeklyTimesConfig
    (TimeZoneInfo overrideTimeZone, DayOfWeekFlags daysOfWeek, TimeSpan timeOfDay
      , IReadOnlyList<IWeeklyTimesConfig>? overrideWeeklyTimes = null)
        : this(daysOfWeek, timeOfDay, InMemoryConfigRoot, InMemoryPath, overrideWeeklyTimes) =>
        OverrideTimeZone = overrideTimeZone;

    public WeeklyTimesConfig(IWeeklyTimesConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        if (toClone is WeeklyTimesConfig weeklyTimesConfig)
        {
            this[nameof(OverrideTimeZone)]    = weeklyTimesConfig[nameof(OverrideTimeZone)];
            this[nameof(OverrideWeeklyTimes)] = weeklyTimesConfig[nameof(OverrideWeeklyTimes)];

            ParentDaysOfWeek = weeklyTimesConfig.ParentDaysOfWeek;
            ParentTimeZone   = weeklyTimesConfig.ParentTimeZone;
        }
        else
        {
            OverrideDaysOfWeek  = toClone.OverrideDaysOfWeek;
            OverrideWeeklyTimes = toClone.OverrideWeeklyTimes;
        }

        TimeOfDay = toClone.TimeOfDay;
    }

    public WeeklyTimesConfig(IWeeklyTimesConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TimeZoneInfo? OverrideTimeZone
    {
        get
        {
            var checkValue = this[nameof(OverrideTimeZone)];
            return checkValue != null ? TimeZoneInfo.FindSystemTimeZoneById(checkValue) : ParentTimeZone;
        }
        set => this[nameof(OverrideTimeZone)] = value?.Id;
    }

    public TimeZoneInfo? ParentTimeZone { get; set; }

    public DayOfWeekFlags OverrideDaysOfWeek
    {
        get
        {
            var checkValue = this[nameof(OverrideDaysOfWeek)];
            return StringExtensions.IsNotNullOrEmpty(checkValue) ? Enum.Parse<DayOfWeekFlags>(checkValue!) : ParentDaysOfWeek;
        }
        set => this[nameof(OverrideDaysOfWeek)] = value.ToString();
    }

    public DayOfWeekFlags ParentDaysOfWeek { get; set; }

    public TimeSpan TimeOfDay
    {
        get
        {
            var checkValue = this[nameof(TimeOfDay)];
            return checkValue != null ? TimeSpan.Parse(checkValue) : TimeSpan.Zero;
        }
        set => this[nameof(TimeOfDay)] = value.ToString();
    }

    public IReadOnlyList<IWeeklyTimesConfig>? OverrideWeeklyTimes
    {
        get
        {
            var autoRecycleList = Recycler.Borrow<ReusableList<IWeeklyTimesConfig>>();
            foreach (var configurationSection in GetSection(nameof(OverrideWeeklyTimes)).GetChildren())
                if (StringExtensions.IsNotNullOrEmpty(configurationSection[nameof(OverrideDaysOfWeek)]))
                {
                    var dayOverride = new WeeklyTimesConfig(ConfigRoot, configurationSection.Path);
                    dayOverride.ParentDaysOfWeek = OverrideDaysOfWeek;
                    dayOverride.ParentTimeZone   = OverrideTimeZone;
                    autoRecycleList.Add(dayOverride);
                }
            return autoRecycleList;
        }
        set
        {
            var oldCount = OverrideWeeklyTimes?.Count() ?? 0;
            var i        = 0;
            if (value != null)
                foreach (var remoteServiceConfig in value)
                {
                    _ = new WeeklyTimesConfig(remoteServiceConfig, ConfigRoot
                                            , $"{Path}{Split}{nameof(OverrideWeeklyTimes)}{Split}{i}");
                    if (remoteServiceConfig is WeeklyTimesConfig valueWeeklyTimesConfig)
                    {
                        valueWeeklyTimesConfig.ParentDaysOfWeek = OverrideDaysOfWeek;
                        valueWeeklyTimesConfig.ParentTimeZone   = OverrideTimeZone;
                    }
                    i++;
                }

            for (var j = i; j < oldCount; j++) ClearValues(ConfigRoot, $"{Path}{Split}{nameof(OverrideWeeklyTimes)}{Split}{i}");
        }
    }

    public TimeZonedWeekDayTime TimeZonedDayOfWeekWeekTime(TimeZoneInfo defaultTimeZone, DayOfWeekFlags dayOfWeekRequested)
    {
        var foundOverride = OverrideWeeklyTimes?.FirstOrDefault(owt => owt.OverrideDaysOfWeek.HasAnyOf(dayOfWeekRequested));
        if (foundOverride != null)
        {
            return new TimeZonedWeekDayTime(foundOverride.OverrideTimeZone ?? defaultTimeZone, dayOfWeekRequested, foundOverride.TimeOfDay);
        }

        return new TimeZonedWeekDayTime(OverrideTimeZone ?? defaultTimeZone, dayOfWeekRequested, TimeOfDay);
    }

    public static void ClearValues(IConfigurationRoot root, string path)
    {
        root[$"{path}{Split}{nameof(OverrideTimeZone)}"]   = null;
        root[$"{path}{Split}{nameof(TimeOfDay)}"]          = null;
        root[$"{path}{Split}{nameof(OverrideDaysOfWeek)}"] = null;
    }

    public ReusableList<TimeZonedWeekDayTime> TimeZonedWeekDayTimes()
    {
        var addToThis = Recycler.Borrow<ReusableList<TimeZonedWeekDayTime>>();
        if (OverrideDaysOfWeek.HasSunday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(OverrideTimeZone!, DayOfWeekFlags.Sunday));
        if (OverrideDaysOfWeek.HasMonday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(OverrideTimeZone!, DayOfWeekFlags.Monday));
        if (OverrideDaysOfWeek.HasTuesday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(OverrideTimeZone!, DayOfWeekFlags.Tuesday));
        if (OverrideDaysOfWeek.HasWednesday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(OverrideTimeZone!, DayOfWeekFlags.Wednesday));
        if (OverrideDaysOfWeek.HasThursday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(OverrideTimeZone!, DayOfWeekFlags.Thursday));
        if (OverrideDaysOfWeek.HasFriday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(OverrideTimeZone!, DayOfWeekFlags.Friday));
        if (OverrideDaysOfWeek.HasSaturday()) addToThis.Add(TimeZonedDayOfWeekWeekTime(OverrideTimeZone!, DayOfWeekFlags.Saturday));

        return addToThis;
    }

    public bool AreEquivalent(IWeeklyTimesConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var overrideTzSame         = Equals(OverrideTimeZone, other.OverrideTimeZone);
        var overrideDaysOfWeekSame = Equals(OverrideDaysOfWeek, other.OverrideDaysOfWeek);
        var timeOfDaySame          = TimeOfDay == other.TimeOfDay;

        var count            = OverrideWeeklyTimes?.Count() ?? 0;
        var countSame        = (count) == (other.OverrideWeeklyTimes?.Count() ?? 0);
        var allOverridesSame = countSame;
        if (countSame && count > 0)
        {
            var thisOverridesList = OverrideWeeklyTimes!;
            var otherOverrideList = other.OverrideWeeklyTimes!;
            for (var i = 0; i < count && allOverridesSame; i++)
            {
                allOverridesSame &= thisOverridesList[i].AreEquivalent(otherOverrideList[i], exactTypes);
            }
            if (thisOverridesList is ReusableList<IWeeklyTimesConfig> reusableList)
            {
                reusableList.DecrementRefCount();
            }
            if (otherOverrideList is ReusableList<IWeeklyTimesConfig> reusableListOther)
            {
                reusableListOther.DecrementRefCount();
            }
        }

        var allAreSame = overrideTzSame && overrideDaysOfWeekSame && timeOfDaySame && countSame && allOverridesSame;
        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IWeeklyTimesConfig, true);


    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = TimeOfDay.GetHashCode();
            hashCode = (hashCode * 397) ^ (OverrideTimeZone != null ? OverrideTimeZone.GetHashCode() : 0);
            hashCode = (hashCode * 397) ^ OverrideDaysOfWeek.GetHashCode();
            hashCode = (hashCode * 397) ^ (OverrideWeeklyTimes != null ? OverrideWeeklyTimes.GetHashCode() : 0);
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(WeeklyTimesConfig)}{{{nameof(TimeOfDay)}: {TimeOfDay}, {nameof(OverrideTimeZone)}: {OverrideTimeZone}, {nameof(OverrideDaysOfWeek)}: {OverrideDaysOfWeek}, " +
        $"{nameof(OverrideWeeklyTimes)}: {OverrideWeeklyTimes}, {nameof(ParentTimeZone)}: {ParentTimeZone}, {nameof(ParentDaysOfWeek)}: {ParentDaysOfWeek}}}";
}
