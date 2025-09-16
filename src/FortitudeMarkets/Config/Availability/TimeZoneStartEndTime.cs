using FortitudeCommon.Config;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeMarkets.Config.Availability;

public interface ITimeZoneStartStopTimeConfig : IInterfacesComparable<ITimeZoneStartStopTimeConfig>, IStyledToStringObject
{
    public TimeZoneInfo? OverrideTimeZone { get; set; }

    public TimeSpan StartTime { get; set; }
    public TimeSpan StopTime  { get; set; }
}

public class TimeZoneStartStopTimeConfig : ConfigSection, ITimeZoneStartStopTimeConfig
{
    public TimeZoneStartStopTimeConfig() { }

    public TimeZoneStartStopTimeConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public TimeZoneStartStopTimeConfig
    (TimeSpan startTime, TimeSpan stopTime
      , TimeZoneInfo? overrideTimeZone)
        : this(startTime, stopTime, InMemoryConfigRoot, InMemoryPath, overrideTimeZone) { }

    public TimeZoneStartStopTimeConfig
        (TimeSpan startTime, TimeSpan stopTime, IConfigurationRoot root, string path, TimeZoneInfo? overrideTimeZone) : base(root, path)
    {
        StartTime = startTime;
        StopTime  = stopTime;

        OverrideTimeZone = overrideTimeZone;
    }


    public TimeZoneStartStopTimeConfig(ITimeZoneStartStopTimeConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        if (toClone is TimeZoneStartStopTimeConfig timeZoneStartStopTimeConfig)
        {
            this[nameof(OverrideTimeZone)] = timeZoneStartStopTimeConfig[nameof(OverrideTimeZone)];
            ParentTimeZone                 = timeZoneStartStopTimeConfig.ParentTimeZone;
        }
        else
        {
            OverrideTimeZone = toClone.OverrideTimeZone;
        }


        StartTime = toClone.StartTime;
        StopTime  = toClone.StopTime;
    }

    public TimeZoneStartStopTimeConfig(ITimeZoneStartStopTimeConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public TimeZoneInfo? OverrideTimeZone
    {
        get
        {
            var checkValue = this[nameof(OverrideTimeZone)];
            return checkValue != null ? TimeZoneInfo.FindSystemTimeZoneById(checkValue) : ParentTimeZone;
        }
        set => this[nameof(OverrideTimeZone)] = value?.Id;
    }

    public TimeSpan StartTime
    {
        get => TimeSpan.Parse(this[nameof(StartTime)]!);
        set => this[nameof(StartTime)] = value.ToString();
    }

    public TimeSpan StopTime
    {
        get => TimeSpan.Parse(this[nameof(StopTime)]!);
        set => this[nameof(StopTime)] = value.ToString();
    }

    public TimeZoneInfo? ParentTimeZone { get; set; }

    public bool AreEquivalent(ITimeZoneStartStopTimeConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        var overrideTzSame = OverrideTimeZone == other.OverrideTimeZone;
        var startTimeSame = StartTime == other.StartTime;
        var stopTimeSame = StopTime == other.StopTime;

        var allAreSame = overrideTzSame && startTimeSame && stopTimeSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => (ReferenceEquals(this, obj)) || AreEquivalent(obj as ITimeZoneStartStopTimeConfig, true);


    public override int GetHashCode() => HashCode.Combine(StartTime, StopTime, OverrideTimeZone);

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc) => 
        sbc.StartComplexType(this)
           .Field.AlwaysAdd(nameof(OverrideTimeZone), this[nameof(OverrideTimeZone)])
           .Field.AlwaysAdd(nameof(ParentTimeZone), ParentTimeZone?.Id)
           .Field.AlwaysAdd(nameof(StartTime), StartTime)
           .Field.AlwaysAdd(nameof(StopTime), StopTime)
           .Complete();
    
    public override string ToString() => 
        $"{nameof(TimeZoneStartStopTimeConfig)}{{{nameof(OverrideTimeZone)}: {this[nameof(OverrideTimeZone)]}, {nameof(ParentTimeZone)}: {ParentTimeZone}, " +
        $"{nameof(StartTime)}: {StartTime}, {nameof(StopTime)}: {StopTime}}}";
}


public static class TimeZoneStartStopTimeConfigExtensions
{
    public static DateTimeOffset NextStartTimeFromDate(this ITimeZoneStartStopTimeConfig startStopTime, DateTimeOffset fromThisDate)
    {
        var convertedDate = new DateTimeOffset(TimeZoneInfo.ConvertTime(fromThisDate, startStopTime.OverrideTimeZone!).Date
                                             , startStopTime.OverrideTimeZone!.GetUtcOffset(fromThisDate));
        var convertedTime = convertedDate + startStopTime.StartTime;
        if (convertedTime < fromThisDate)
        {
            convertedTime = convertedDate.AddDays(1);
        }
        return convertedTime;
    }

    public static DateTimeOffset NextStopTimeFromDate(this ITimeZoneStartStopTimeConfig startStopTime, DateTimeOffset fromThisDate)
    {
        var convertedDate = new DateTimeOffset(TimeZoneInfo.ConvertTime(fromThisDate, startStopTime.OverrideTimeZone!).Date
                                             , startStopTime.OverrideTimeZone!.GetUtcOffset(fromThisDate));
        if (startStopTime.StopTime < startStopTime.StartTime)
        {
            convertedDate = convertedDate.AddDays(1);
        }
        var convertedTime     = convertedDate + startStopTime.StopTime;
        if (convertedTime < fromThisDate)
        {
            convertedTime = convertedDate.AddDays(1);
        }
        return convertedTime;
    }
}
