using System.Globalization;
using FortitudeCommon.Config;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.Console;

public interface IConsoleAppenderConfig : IBufferingFormatAppenderConfig, IConfigCloneTo<IConsoleAppenderConfig>
{
    bool DisableColoredConsole { get; }

    new IConsoleAppenderConfig Clone();

    new IConsoleAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path);
}

public interface IMutableConsoleAppenderConfig : IConsoleAppenderConfig, IMutableBufferingFormatAppenderConfig
{
    new bool DisableColoredConsole { get; set; }

    new IMutableConsoleAppenderConfig Clone();

    new IMutableConsoleAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path);
}

public class ConsoleAppenderConfig : BufferingFormatAppenderConfig, IMutableConsoleAppenderConfig
{
    public const string DefaultConsoleAppenderName = $"DefaultColoredConsoleAppender";
    public const string ConsoleAppenderType = $"{nameof(FLoggerBuiltinAppenderType.ConsoleOut)}";

    public ConsoleAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public ConsoleAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }
    
    public ConsoleAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , bool disableColoredConsole = false
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName,  ConsoleAppenderType, charBufferSize, flushBufferConfig, disableBuffering, logEntryFormatLayout, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere)
    {
        DisableColoredConsole    = disableColoredConsole;
    }

    public ConsoleAppenderConfig(IConsoleAppenderConfig toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path)
    {
        DisableColoredConsole = toClone.DisableColoredConsole;
    }

    public ConsoleAppenderConfig(IConsoleAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public static readonly ConsoleAppenderConfig DefaultConsoleAppenderConfig = new (){ AppenderName = DefaultConsoleAppenderName };

    public bool DisableColoredConsole
    {
        get => bool.TryParse(this[nameof(DisableColoredConsole)], out var charBufferSize) && charBufferSize;
        set => this[nameof(DisableColoredConsole)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public override string AppenderType
    {
        get => this[nameof(AppenderType)] ?? ConsoleAppenderType;
        set => this[nameof(AppenderType)] = value;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    IConsoleAppenderConfig ICloneable<IConsoleAppenderConfig>.Clone() => Clone();

    IConsoleAppenderConfig IConsoleAppenderConfig.Clone() => Clone();

    IMutableConsoleAppenderConfig IMutableConsoleAppenderConfig.Clone() => Clone();

    public override ConsoleAppenderConfig Clone() => new(this);

    IConsoleAppenderConfig IConfigCloneTo<IConsoleAppenderConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    IConsoleAppenderConfig IConsoleAppenderConfig.CloneConfigTo(IConfigurationRoot configRoot, string path) => CloneConfigTo(configRoot, path);

    IMutableConsoleAppenderConfig IMutableConsoleAppenderConfig.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public override ConsoleAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IConsoleAppenderConfig consoleAppenderConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var disableColorSame = DisableColoredConsole == consoleAppenderConfig.DisableColoredConsole;

        var allAreSame = baseSame && disableColorSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFormattingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        hashCode = (hashCode * 397) ^ DisableColoredConsole.GetHashCode();
        return hashCode;
    }

    public override IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(ConsoleAppenderConfig))
           .AddTypeStart()
           .AddBaseFieldsStart();
        return base.ToString(sbc)
                   .AddBaseFieldsEnd()
                   .AddField(nameof(DisableColoredConsole), DisableColoredConsole)
                   .AddTypeEnd();
    }
}
