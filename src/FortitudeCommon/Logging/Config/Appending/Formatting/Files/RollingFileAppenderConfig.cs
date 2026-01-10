// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.Files;

public interface IRollingFileAppenderConfig : IFileAppenderConfig
{
    const string DefaultRollAtSize         = "100MB";
    const string DefaultRollFileNameFormat = "{ToRollFilePath}{ToRollFileName}.{RollNumPlaceholder}.{ToRollFileExt}{CompressionTypeExt}";

    string RollAtSize { get; }

    ulong RollAtBytes { get; }

    string RollFileNameFormat { get; }
}

public interface IMutableRollingFileAppenderConfig : IRollingFileAppenderConfig, IMutableFileAppenderConfig
{
    new string RollAtSize { get; set; }

    new string RollFileNameFormat { get; set; }
}

public class RollingFileAppenderConfig : FileAppenderConfig, IMutableRollingFileAppenderConfig
{
    public RollingFileAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public RollingFileAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public RollingFileAppenderConfig
    (string appenderName, string appenderType
      , FileAppenderType fileAppenderType = FileAppenderType.Unbounded
      , string filename = IFileAppenderConfig.DefaultFileName
      , string filePath = IFileAppenderConfig.DefaultFilePath
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , string rollAtSize = IRollingFileAppenderConfig.DefaultRollAtSize
      , uint closeDelayMs = IFileAppenderConfig.DefaultCloseDelayMs
      , string rollFileNameFormat = IRollingFileAppenderConfig.DefaultRollFileNameFormat
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, appenderType, fileAppenderType, filename, filePath, logEntryFormatLayout
             , rollAtSize, closeDelayMs, rollFileNameFormat, charBufferSize, flushBufferConfig, disableBuffering, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public RollingFileAppenderConfig
    (string appenderName
      , FileAppenderType fileAppenderType = FileAppenderType.Unbounded
      , string filename = IFileAppenderConfig.DefaultFileName
      , string filePath = IFileAppenderConfig.DefaultFilePath
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , string rollAtSize = IRollingFileAppenderConfig.DefaultRollAtSize
      , uint closeDelayMs = IFileAppenderConfig.DefaultCloseDelayMs
      , string rollFileNameFormat = IRollingFileAppenderConfig.DefaultRollFileNameFormat
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, fileAppenderType, filename, filePath, logEntryFormatLayout, rollAtSize
             , closeDelayMs, rollFileNameFormat, charBufferSize, flushBufferConfig, disableBuffering, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public RollingFileAppenderConfig
    (IConfigurationRoot root, string path, string appenderName, string appenderType
      , FileAppenderType fileAppenderType = FileAppenderType.Unbounded
      , string filename = IFileAppenderConfig.DefaultFileName
      , string filePath = IFileAppenderConfig.DefaultFilePath
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , string rollAtSize = IRollingFileAppenderConfig.DefaultRollAtSize
      , uint closeDelayMs = IFileAppenderConfig.DefaultCloseDelayMs
      , string rollFileNameFormat = IRollingFileAppenderConfig.DefaultRollFileNameFormat
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, appenderType, fileAppenderType, filename, filePath, logEntryFormatLayout, charBufferSize
             , flushBufferConfig, disableBuffering, closeDelayMs, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition
             , deactivateHere)
    {
        RollAtSize = rollAtSize;

        RollFileNameFormat = rollFileNameFormat;
    }

    public RollingFileAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , FileAppenderType fileAppenderType = FileAppenderType.Unbounded
      , string filename = IFileAppenderConfig.DefaultFileName
      , string filePath = IFileAppenderConfig.DefaultFilePath
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , string rollAtSize = IRollingFileAppenderConfig.DefaultRollAtSize
      , uint closeDelayMs = IFileAppenderConfig.DefaultCloseDelayMs
      , string rollFileNameFormat = IRollingFileAppenderConfig.DefaultRollFileNameFormat
      , int charBufferSize = IBufferingFormatAppenderConfig.DefaultCharBufferSize
      , IMutableFlushBufferConfig? flushBufferConfig = null
      , bool disableBuffering = false
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, fileAppenderType, filename, filePath, logEntryFormatLayout, charBufferSize, flushBufferConfig
             , disableBuffering, closeDelayMs, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere)
    {
        RollAtSize = rollAtSize;

        RollFileNameFormat = rollFileNameFormat;
    }

    public RollingFileAppenderConfig(IRollingFileAppenderConfig toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path)
    {
        RollAtSize = toClone.RollAtSize;

        RollFileNameFormat = toClone.RollFileNameFormat;
    }

    public RollingFileAppenderConfig(IRollingFileAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ulong RollAtBytes
    {
        get
        {
            var lowerRollAtSize = RollAtSize.ToLower();
            var (fileSize, unitSuffix) = lowerRollAtSize.SplitFormattedULongFromString();
            if (fileSize != null) return fileSize.Value.ToSizeInBytes(unitSuffix);
            return 100ul.AsMegaBytes();
        }
    }

    public string RollAtSize
    {
        get => this[nameof(RollAtSize)] ?? IRollingFileAppenderConfig.DefaultRollAtSize;
        set => this[nameof(RollAtSize)] = value;
    }

    public string RollFileNameFormat
    {
        get => this[nameof(RollFileNameFormat)] ?? IRollingFileAppenderConfig.DefaultRollFileNameFormat;
        set => this[nameof(RollFileNameFormat)] = value;
    }

    public override T Accept<T>(T visitor) => visitor.Visit(this);

    public override RollingFileAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    object ICloneable.Clone() => Clone();

    IFormattingAppenderConfig ICloneable<IFormattingAppenderConfig>.Clone() => Clone();

    IFormattingAppenderConfig IFormattingAppenderConfig.Clone() => Clone();

    public override RollingFileAppenderConfig Clone() => new(this);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IRollingFileAppenderConfig rollingFileAppender) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var rollAtSame = RollAtSize == rollingFileAppender.RollAtSize;

        var rollFileNameSame = RollFileNameFormat == rollingFileAppender.RollFileNameFormat;

        var allAreSame = baseSame && rollAtSame && rollFileNameSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFormattingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        hashCode = (hashCode * 397) ^ RollAtSize.GetHashCode();
        hashCode = (hashCode * 397) ^ RollFileNameFormat.GetHashCode();
        return hashCode;
    }

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(RollAtSize), RollAtSize)
           .Field.AlwaysAdd(nameof(RollFileNameFormat), RollFileNameFormat)
           .AddBaseRevealStateFields(this).Complete();
}
