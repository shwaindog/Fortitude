// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using FortitudeCommon.Config;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;
using static FortitudeCommon.Logging.Config.Appending.Formatting.Console.IConsoleAppenderConfig;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.Console;

public interface IConsoleAppenderConfig : IBufferingFormatAppenderConfig, IConfigCloneTo<IConsoleAppenderConfig>
{
    const string DefaultConsoleAppenderName = $"DefaultColoredConsoleAppender";
    const string ConsoleAppenderType        = $"{nameof(FLoggerBuiltinAppenderType.ConsoleOut)}";

    bool DisableColoredConsole { get; }

    new IConsoleAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path);

    new IConsoleAppenderConfig Clone();
}

public interface IMutableConsoleAppenderConfig : IConsoleAppenderConfig, IMutableBufferingFormatAppenderConfig
{
    new bool DisableColoredConsole { get; set; }

    new IMutableConsoleAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path);

    new IMutableConsoleAppenderConfig Clone();
}

public class ConsoleAppenderConfig : BufferingFormatAppenderConfig, IMutableConsoleAppenderConfig
{
    public static readonly ConsoleAppenderConfig DefaultConsoleAppenderConfig = new() { AppenderName = DefaultConsoleAppenderName };

    public ConsoleAppenderConfig(IConfigurationRoot root, string path) : base(root, path) => AppenderType = ConsoleAppenderType;

    public ConsoleAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public ConsoleAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , bool disableColoredConsole = false
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, ConsoleAppenderType, logEntryFormatLayout, charBufferSize, flushBufferConfig, disableBuffering
             , runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) =>
        DisableColoredConsole = disableColoredConsole;

    public ConsoleAppenderConfig(IConsoleAppenderConfig toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path) =>
        DisableColoredConsole = toClone.DisableColoredConsole;

    public ConsoleAppenderConfig(IConsoleAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public bool DisableColoredConsole
    {
        get => bool.TryParse(this[nameof(DisableColoredConsole)], out var charBufferSize) && charBufferSize;
        set => this[nameof(DisableColoredConsole)] = value.ToString(CultureInfo.InvariantCulture);
    }

    public override bool DisableBuffering
    {
        get => !bool.TryParse(this[nameof(DisableBuffering)], out var enableDoubleBuffering) || enableDoubleBuffering;
        set => this[nameof(DisableBuffering)] = value.ToString();
    }

    public override string AppenderType
    {
        get => this[nameof(AppenderType)] ?? ConsoleAppenderType;
        set => this[nameof(AppenderType)] = value;
    }

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    IConsoleAppenderConfig IConfigCloneTo<IConsoleAppenderConfig>.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    IConsoleAppenderConfig IConsoleAppenderConfig.CloneConfigTo(IConfigurationRoot configRoot, string path) => CloneConfigTo(configRoot, path);

    IMutableConsoleAppenderConfig IMutableConsoleAppenderConfig.CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        CloneConfigTo(configRoot, path);

    public override ConsoleAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    IConsoleAppenderConfig ICloneable<IConsoleAppenderConfig>.Clone() => Clone();

    IConsoleAppenderConfig IConsoleAppenderConfig.Clone() => Clone();

    IMutableConsoleAppenderConfig IMutableConsoleAppenderConfig.Clone() => Clone();

    public override ConsoleAppenderConfig Clone() => new(this);

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

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .AddBaseRevealStateFields(this)
           .Field.AlwaysAdd(nameof(DisableColoredConsole), DisableColoredConsole)
           .Complete();
}
