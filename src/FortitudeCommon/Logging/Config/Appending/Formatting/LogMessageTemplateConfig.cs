using FortitudeCommon.Config;
using FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Formatting;

public interface ILogMessageTemplateConfig : IFLogConfig, IInterfacesComparable<ILogMessageTemplateConfig>
, IConfigCloneTo<ILogMessageTemplateConfig>, IStyledToStringObject
{
    FLogLevel LogLevel { get; }

    string MessageTemplate { get; }

    string TemplateLoggerName { get; }
}

public interface IMutableLogMessageTemplateConfig : ILogMessageTemplateConfig, IMutableFLogConfig
{
    new FLogLevel LogLevel { get; set; }

    new string MessageTemplate { get; set; }

    new string TemplateLoggerName { get; set; }
}

public class LogMessageTemplateConfig : FLogConfig, IMutableLogMessageTemplateConfig
{
    public LogMessageTemplateConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public LogMessageTemplateConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public LogMessageTemplateConfig
    ( FLogLevel logLevel, string messageTemplate, string templateLoggerName)
        : this(InMemoryConfigRoot, InMemoryPath, logLevel, messageTemplate, templateLoggerName) { }

    public LogMessageTemplateConfig
    (IConfigurationRoot root, string path, FLogLevel logLevel, string messageTemplate, string templateLoggerName)
        : base(root, path)
    {
        LogLevel           = logLevel;
        MessageTemplate    = messageTemplate;
        TemplateLoggerName = templateLoggerName;
    }

    public LogMessageTemplateConfig(ILogMessageTemplateConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        LogLevel           = toClone.LogLevel;
        MessageTemplate    = toClone.MessageTemplate;
        TemplateLoggerName = toClone.TemplateLoggerName;
    }

    public LogMessageTemplateConfig(ILogMessageTemplateConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public FLogLevel LogLevel
    {
        get =>
            Enum.TryParse<FLogLevel>(this[nameof(LogLevel)], out var logLevel)
                ? logLevel
                : FLogLevel.Trace;
        set => this[nameof(LogLevel)] = value.ToString();
    }

    public string MessageTemplate
    {
        get => this[nameof(MessageTemplate)] = "";
        set => this[nameof(MessageTemplate)] = value;
    }

    public string TemplateLoggerName
    {
        get => this[nameof(TemplateLoggerName)] = "";
        set => this[nameof(TemplateLoggerName)] = value;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    ILogMessageTemplateConfig ICloneable<ILogMessageTemplateConfig>.    Clone() => Clone();

    ILogMessageTemplateConfig IConfigCloneTo<ILogMessageTemplateConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) => 
        new LogMessageTemplateConfig(configRoot, path);

    public virtual LogMessageTemplateConfig Clone() => new(this);

    public LogMessageTemplateConfig CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        new (this, configRoot, path);

    public virtual bool AreEquivalent(ILogMessageTemplateConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var logLevelSame        = LogLevel == other.LogLevel;
        var messageTemplateSame = MessageTemplate == other.MessageTemplate;
        var templateLoggerName  = TemplateLoggerName == other.TemplateLoggerName;
        

        var allAreSame = logLevelSame && messageTemplateSame && templateLoggerName;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ILogMessageTemplateConfig, true);

    public override int GetHashCode()
    {
        var hashCode = (int)LogLevel;
        hashCode = (hashCode * 397) ^ MessageTemplate.GetHashCode();
        hashCode = (hashCode * 397) ^ TemplateLoggerName.GetHashCode();
        return hashCode;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartComplexType(nameof(ExtractKeyExpressionConfig))
               .Field.AlwaysAdd(nameof(LogLevel), LogLevel, FLogLevelExtensions.FLogLevelFormatter)
               .Field.AlwaysAdd(nameof(MessageTemplate), MessageTemplate)
               .Field.AlwaysAdd(nameof(TemplateLoggerName), TemplateLoggerName)
               .Complete();
    }
}
