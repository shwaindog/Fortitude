using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Formatting;

public interface IFormattingAppenderConfig : IAppenderDefinitionConfig, ICloneable<IFormattingAppenderConfig>
{
    const string DefaultStringFormattingTemplate
        = "'%TS:yyyy-MM-dd HH:mm:SS.fff%' '%LVL,5%' ['%THREADNAME,10[..15]%' '%THREADID%'] '%LGR%' '%MSG%''%NL%'";

    string LogEntryFormatLayout { get; }

    IAppenderReferenceConfig? InheritsFrom { get; }

    IInheritingAppendersLookupConfig? Defines { get; }

    new IFormattingAppenderConfig Clone();
}

public interface IMutableFormattingAppenderConfig : IFormattingAppenderConfig, IMutableAppenderDefinitionConfig
{
    new string LogEntryFormatLayout { get; set; }

    new IMutableFormattingAppenderConfig? InheritsFrom { get; set; }

    new IAppendableInheritingAppendersLookupConfig? Defines { get; set; }
}

public class FormattingAppenderConfig : AppenderDefinitionConfig, IMutableFormattingAppenderConfig
{
    public FormattingAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FormattingAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FormattingAppenderConfig
    (string appenderName, string appenderType
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate, int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, appenderType, logEntryFormatLayout, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public FormattingAppenderConfig
    (string appenderName
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate, int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, logEntryFormatLayout, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public FormattingAppenderConfig
    (IConfigurationRoot root, string path, string appenderName, string appenderType
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, appenderType, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere)
    {
        LogEntryFormatLayout = logEntryFormatLayout;
    }

    public FormattingAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere)
    {
        LogEntryFormatLayout = logEntryFormatLayout;
    }

    public FormattingAppenderConfig(IFormattingAppenderConfig toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path)
    {
        LogEntryFormatLayout = toClone.LogEntryFormatLayout;
    }

    public FormattingAppenderConfig(IFormattingAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    IAppenderReferenceConfig? IFormattingAppenderConfig.InheritsFrom => InheritsFrom;

    public IMutableFormattingAppenderConfig? InheritsFrom
    {
        get => ParentConfig as IMutableFormattingAppenderConfig;
        set => ParentConfig = value;
    }

    public string LogEntryFormatLayout
    {
        get => this[nameof(LogEntryFormatLayout)] ?? IFormattingAppenderConfig.DefaultStringFormattingTemplate;
        set => this[nameof(LogEntryFormatLayout)] = value;
    }

    IInheritingAppendersLookupConfig? IFormattingAppenderConfig.Defines => Defines;

    public IAppendableInheritingAppendersLookupConfig? Defines
    {
        get
        {
            if (GetSection(nameof(Defines)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new InheritingAppendersLookupConfig(ConfigRoot, $"{Path}{Split}{nameof(Defines)}")
                {
                    ParentConfig = this
                };
            }
            return null;
        }
        set
        {
            if (value != null)
            {
                _ = new InheritingAppendersLookupConfig(value, ConfigRoot, $"{Path}{Split}{nameof(Defines)}");

                value.ParentConfig = this;
            }
        }
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IFormattingAppenderConfig ICloneable<IFormattingAppenderConfig>.Clone() => Clone();

    IFormattingAppenderConfig IFormattingAppenderConfig.Clone() => Clone();

    public override FormattingAppenderConfig Clone() => new(this);

    public override FormattingAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IFormattingAppenderConfig formattingAppender) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var formatLayoutSame = LogEntryFormatLayout == formattingAppender.LogEntryFormatLayout;
        var definesSame      = InheritsFrom?.AreEquivalent(formattingAppender.InheritsFrom, exactTypes) ?? formattingAppender.InheritsFrom == null;

        var allAreSame = baseSame && formatLayoutSame && definesSame;

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
        return tb.Field.AlwaysAdd(nameof(LogEntryFormatLayout), LogEntryFormatLayout)
                 .Field.WhenNonNullOrDefaultAdd(nameof(InheritsFrom), InheritsFrom)
                 .Complete();
    }
}
