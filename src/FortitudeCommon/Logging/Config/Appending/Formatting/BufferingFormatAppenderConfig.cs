using System.Globalization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Formatting;

public interface IBufferingFormatAppenderConfig : IFormattingAppenderConfig, ICloneable<IBufferingFormatAppenderConfig>
{
    const int MinimumCharBufferSize = 1000;
    const int MaximumCharBufferSize = (int)NumberExtensions.GigaByte;
    const int DefaultCharBufferSize = 8000;

    static readonly IMutableFlushBufferConfig DefaultFlushBufferConfig =
        new FlushBufferConfig(IFlushBufferConfig.DefaultBufferSizeTriggerPercentage);

    bool DisableBuffering { get; }

    int CharBufferSize { get; }

    bool EnableDoubleBufferToggling { get; }

    FlushMechanism BufferFlushMechanism { get; }

    int FlushAsyncQueueNumber { get; }

    IFlushBufferConfig FlushConfig { get; }

    new IBufferingFormatAppenderConfig Clone();
}

public interface IMutableBufferingFormatAppenderConfig : IBufferingFormatAppenderConfig, IMutableFormattingAppenderConfig
{
    new bool DisableBuffering { get; set; }

    new int CharBufferSize { get; set; }

    new bool EnableDoubleBufferToggling { get; set; }

    new FlushMechanism BufferFlushMechanism { get; set; }

    new int FlushAsyncQueueNumber { get; set; }

    new IMutableFlushBufferConfig FlushConfig { get; set; }

    new IMutableBufferingFormatAppenderConfig Clone();
}

public class BufferingFormatAppenderConfig : FormattingAppenderConfig, IMutableBufferingFormatAppenderConfig
{
    public BufferingFormatAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public BufferingFormatAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public BufferingFormatAppenderConfig
    (string appenderName, string appenderType
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, appenderType, charBufferSize, flushBufferConfig, disableBuffering
             , logEntryFormatLayout, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public BufferingFormatAppenderConfig
    (string appenderName
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, charBufferSize, flushBufferConfig, disableBuffering
             , logEntryFormatLayout, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public BufferingFormatAppenderConfig
    (IConfigurationRoot root, string path, string appenderName, string appenderType
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, appenderType, logEntryFormatLayout, runOnAsyncQueueNumber, inheritFromAppenderName
             , isTemplateOnlyDefinition, deactivateHere)
    {
        DisableBuffering = disableBuffering;
        CharBufferSize   = charBufferSize;
        FlushConfig      = flushBufferConfig ?? IBufferingFormatAppenderConfig.DefaultFlushBufferConfig;
    }

    public BufferingFormatAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, logEntryFormatLayout, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition
             , deactivateHere)
    {
        DisableBuffering = disableBuffering;
        CharBufferSize   = charBufferSize;
        FlushConfig      = flushBufferConfig ?? IBufferingFormatAppenderConfig.DefaultFlushBufferConfig;
    }

    public BufferingFormatAppenderConfig(IBufferingFormatAppenderConfig toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path)
    {
        DisableBuffering = toClone.DisableBuffering;
        CharBufferSize   = toClone.CharBufferSize;
        FlushConfig      = (IMutableFlushBufferConfig)toClone.FlushConfig;
    }

    public BufferingFormatAppenderConfig(IBufferingFormatAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public virtual bool DisableBuffering
    {
        get => bool.TryParse(this[nameof(DisableBuffering)], out var enableDoubleBuffering) && enableDoubleBuffering;
        set => this[nameof(DisableBuffering)] = value.ToString();
    }

    public int CharBufferSize
    {
        get =>
            int.TryParse(this[nameof(CharBufferSize)], out var charBufferSize)
                ? Math.Clamp(charBufferSize
                           , IBufferingFormatAppenderConfig.MinimumCharBufferSize
                           , IBufferingFormatAppenderConfig.MaximumCharBufferSize)
                : IBufferingFormatAppenderConfig.DefaultCharBufferSize;
        set => this[nameof(CharBufferSize)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public FlushMechanism BufferFlushMechanism
    {
        get =>
            Enum.TryParse<FlushMechanism>(this[nameof(BufferFlushMechanism)], out var logLevel)
                ? logLevel
                : FlushMechanism.Default;
        set => this[nameof(BufferFlushMechanism)] = value.ToString();
    }

    public bool EnableDoubleBufferToggling
    {
        get => !bool.TryParse(this[nameof(EnableDoubleBufferToggling)], out var enableDoubleBuffering) || enableDoubleBuffering;
        set => this[nameof(EnableDoubleBufferToggling)] = value.ToString();
    }

    public int FlushAsyncQueueNumber
    {
        get => int.TryParse(this[nameof(FlushAsyncQueueNumber)], out var flushAsyncQueueNumber) ? flushAsyncQueueNumber : 0;
        set => this[nameof(FlushAsyncQueueNumber)] = value.ToString(CultureInfo.InvariantCulture);
    }

    IFlushBufferConfig IBufferingFormatAppenderConfig.FlushConfig => FlushConfig;

    public IMutableFlushBufferConfig FlushConfig
    {
        get
        {
            if (GetSection(nameof(FlushConfig)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new FlushBufferConfig(ConfigRoot, $"{Path}{Split}{nameof(FlushConfig)}");
            }
            return new FlushBufferConfig(IBufferingFormatAppenderConfig.DefaultFlushBufferConfig
                                       , ConfigRoot, $"{Path}{Split}{nameof(FlushConfig)}");
        }
        set => _ = new FlushBufferConfig(value, ConfigRoot, $"{Path}{Split}{nameof(FlushConfig)}");
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    IBufferingFormatAppenderConfig ICloneable<IBufferingFormatAppenderConfig>.Clone() => Clone();

    IBufferingFormatAppenderConfig IBufferingFormatAppenderConfig.Clone() => Clone();

    IMutableBufferingFormatAppenderConfig IMutableBufferingFormatAppenderConfig.Clone() => Clone();

    public override BufferingFormatAppenderConfig Clone() => new(this);

    public override BufferingFormatAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IBufferingFormatAppenderConfig flushBufferingFormatAppender) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var charBufferSizeSame = CharBufferSize == flushBufferingFormatAppender.CharBufferSize;
        var flushConfigSame    = FlushConfig.AreEquivalent(flushBufferingFormatAppender.FlushConfig, exactTypes);

        var allAreSame = baseSame && charBufferSizeSame && flushConfigSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFormattingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        hashCode = (hashCode * 397) ^ LogEntryFormatLayout.GetHashCode();
        hashCode = (hashCode * 397) ^ (InheritsFrom?.GetHashCode() ?? 0);
        return hashCode;
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        using var tb = sbc.StartComplexType(nameof(FormattingAppenderConfig))
                          .AddBaseFieldsStart();
        base.ToString(sbc);
        return tb.Field.AlwaysAdd(nameof(CharBufferSize), CharBufferSize)
                 .Field.AlwaysAdd(nameof(FlushConfig), FlushConfig)
                 .Complete();
    }
}
