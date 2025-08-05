using System.Text;
using System.Text.Json.Serialization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Config;

public interface ITimeSpanConfig:  IInterfacesComparable<ITimeSpanConfig>, ICloneable<ITimeSpanConfig>, IStyledToStringObject
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int Micros { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int Millis { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int Seconds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int Minutes { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int Hours { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    int Days { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    ITimeSpanConfig? Add { get; set; }

    TimeSpan ToTimeSpan();
}


public class TimeSpanConfig: ConfigSection, ITimeSpanConfig
{
    public TimeSpanConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public TimeSpanConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public TimeSpanConfig(int millis = 0, int seconds = 0, int minutes = 0, int hours = 0, int days = 0, int micros = 0, ITimeSpanConfig? toAdd = null)
    : this(InMemoryConfigRoot, InMemoryPath, millis, seconds, minutes, hours, days, micros, toAdd)
    {
    }

    public TimeSpanConfig(IConfigurationRoot root, string path, int millis = 0, int seconds = 0, int minutes = 0, int hours = 0
      , int days = 0, int micros = 0, ITimeSpanConfig? toAdd = null) : base(root, path)
    {
        Micros  = micros;
        Millis  = millis;
        Seconds = seconds;
        Minutes = minutes;
        Hours   = hours;
        Days    = days;

        Add = toAdd;
    }

    public TimeSpanConfig(ITimeSpanConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Micros  = toClone.Micros;
        Millis  = toClone.Millis;
        Seconds = toClone.Seconds;
        Minutes = toClone.Minutes;
        Hours   = toClone.Hours;
        Days    = toClone.Days;
        Add     = toClone.Add?.Clone();
    }

    public TimeSpanConfig(ITimeSpanConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }


    public int Micros
    {
        get => int.TryParse(this[nameof(Micros)], out var timePart) ? timePart : 0;
        set => this[nameof(Micros)] = value.ToString();
    }

    public int Millis
    {
        get => int.TryParse(this[nameof(Millis)], out var timePart) ? timePart : 0;
        set => this[nameof(Millis)] = value.ToString();
    }

    public int Seconds
    {
        get => int.TryParse(this[nameof(Seconds)], out var timePart) ? timePart : 0;
        set => this[nameof(Seconds)] = value.ToString();
    }

    public int Minutes
    {
        get => int.TryParse(this[nameof(Minutes)], out var timePart) ? timePart : 0;
        set => this[nameof(Minutes)] = value.ToString();
    }

    public int Hours
    {
        get => int.TryParse(this[nameof(Hours)], out var timePart) ? timePart : 0;
        set => this[nameof(Hours)] = value.ToString();
    }

    public int Days
    {
        get => int.TryParse(this[nameof(Days)], out var timePart) ? timePart : 0;
        set => this[nameof(Days)] = value.ToString();
    }

    public ITimeSpanConfig? Add
    {
        get
        {
            if (GetSection(nameof(Add)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new TimeSpanConfig(ConfigRoot, $"{Path}{Split}{nameof(Add)}");
            }
            return null;
        }
        set => _ = value != null ? new TimeSpanConfig(value, ConfigRoot, $"{Path}{Split}{nameof(Add)}") : null;
    }

    public TimeSpan ToTimeSpan()
    {
        var result = TimeSpan.Zero;
        result =
            result
                .Add(TimeSpan.FromMicroseconds(Micros))
                .Add(TimeSpan.FromMilliseconds(Millis))
                .Add(TimeSpan.FromSeconds(Seconds))
                .Add(TimeSpan.FromMinutes(Minutes))
                .Add(TimeSpan.FromHours(Hours))
                .Add(TimeSpan.FromDays(Days));
        if (Add is { } toAdd)
        {
            result = result.Add(toAdd.ToTimeSpan());
        }
        return result;
    }

    object ICloneable.     Clone() => Clone();

    ITimeSpanConfig ICloneable<ITimeSpanConfig>.Clone() => Clone();

    public TimeSpanConfig Clone() => new (this);


    public bool AreEquivalent(ITimeSpanConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var microsSame = Micros == other.Micros;
        var millisSame = Millis == other.Millis;
        var secondsSame = Seconds == other.Seconds;
        var minutesSame = Minutes == other.Minutes;
        var hoursSame = Hours == other.Hours;
        var daysSame = Days == other.Days;
        var addSame = Add?.AreEquivalent(other.Add) ?? other.Add == null;

        var allAreSame = microsSame && millisSame && secondsSame && minutesSame && hoursSame && daysSame && addSame;

        return allAreSame;
    }


    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITimeSpanConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Micros;
            hashCode = (hashCode * 397) ^ Millis;
            hashCode = (hashCode * 397) ^ Seconds;
            hashCode = (hashCode * 397) ^ Minutes;
            hashCode = (hashCode * 397) ^ Hours;
            hashCode = (hashCode * 397) ^ Days;
            hashCode = (hashCode * 397) ^ (Add?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return sbc.StartComplexType(nameof(TimeSpanConfig))
           .Field.AlwaysAdd(nameof(Days), Days)
           .Field.AlwaysAdd(nameof(Hours), Hours)
           .Field.AlwaysAdd(nameof(Minutes), Minutes)
           .Field.AlwaysAdd(nameof(Seconds), Seconds)
           .Field.AlwaysAdd(nameof(Millis), Millis)
           .Field.AlwaysAdd(nameof(Micros), Micros)
           .Complete();
    }

    public override string ToString() => this.DefaultToString();
}