// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Formatting.Files;

public interface IFileAppenderConfig : IFormattingAppenderConfig, ICloneable<IFileAppenderConfig>
{
    const string DefaultFileName = "{StartAssemblyName}.log";
    const string DefaultFilePath = ".";

    FileAppenderType FileAppenderType { get; }

    CompressionType CompressionType { get; }

    string FilePath { get; }

    string FileName { get; }

    new IFileAppenderConfig Clone();
}

public interface IMutableFileAppenderConfig : IFileAppenderConfig, IMutableFormattingAppenderConfig
{
    new FileAppenderType FileAppenderType { get; set; }

    new CompressionType CompressionType { get; set; }

    new string FilePath { get; set; }

    new string FileName { get; set; }
}

public class FileAppenderConfig : FormattingAppenderConfig, IMutableFileAppenderConfig
{
    public FileAppenderConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FileAppenderConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FileAppenderConfig
    (string appenderName, string appenderType
      , FileAppenderType fileAppenderType = FileAppenderType.Unbounded
      , string filename = IFileAppenderConfig.DefaultFileName
      , string filePath = IFileAppenderConfig.DefaultFilePath
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate, int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, appenderType, fileAppenderType, filename, filePath, logEntryFormatLayout
             , runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public FileAppenderConfig
    (string appenderName
      , FileAppenderType fileAppenderType = FileAppenderType.Unbounded
      , string filename = IFileAppenderConfig.DefaultFileName
      , string filePath = IFileAppenderConfig.DefaultFilePath
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate, int runOnAsyncQueueNumber = 0
      , string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false, bool deactivateHere = false)
        : this(InMemoryConfigRoot, InMemoryPath, appenderName, fileAppenderType, filename, filePath, logEntryFormatLayout, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere) { }

    public FileAppenderConfig
    (IConfigurationRoot root, string path, string appenderName, string appenderType
      , FileAppenderType fileAppenderType = FileAppenderType.Unbounded
      , string filename = IFileAppenderConfig.DefaultFileName
      , string filePath = IFileAppenderConfig.DefaultFilePath
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, logEntryFormatLayout, appenderType, runOnAsyncQueueNumber
             , inheritFromAppenderName, isTemplateOnlyDefinition, deactivateHere)
    {
        FileAppenderType = fileAppenderType;

        FileName = filename;
        FilePath = filePath;
    }

    public FileAppenderConfig
    (IConfigurationRoot root, string path, string appenderName
      , FileAppenderType fileAppenderType = FileAppenderType.Unbounded
      , string filename = IFileAppenderConfig.DefaultFileName
      , string filePath = IFileAppenderConfig.DefaultFilePath
      , string logEntryFormatLayout = IFormattingAppenderConfig.DefaultStringFormattingTemplate
      , int runOnAsyncQueueNumber = 0, string? inheritFromAppenderName = null, bool isTemplateOnlyDefinition = false
      , bool deactivateHere = false)
        : base(root, path, appenderName, logEntryFormatLayout, runOnAsyncQueueNumber, inheritFromAppenderName, isTemplateOnlyDefinition
             , deactivateHere)
    {
        FileAppenderType = fileAppenderType;

        FileName = filename;
        FilePath = filePath;
    }

    public FileAppenderConfig(IFileAppenderConfig toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path)
    {
        FileAppenderType = toClone.FileAppenderType;

        FileName = toClone.FileName;
        FilePath = toClone.FilePath;
    }

    public FileAppenderConfig(IFileAppenderConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public FileAppenderType FileAppenderType
    {
        get =>
            Enum.TryParse<FileAppenderType>(this[nameof(FileAppenderType)], out var poolScope)
                ? poolScope
                : FileAppenderType.Unbounded;
        set => this[nameof(FileAppenderType)] = value.ToString();
    }

    public CompressionType CompressionType
    {
        get =>
            Enum.TryParse<CompressionType>(this[nameof(CompressionType)], out var poolScope)
                ? poolScope
                : CompressionType.Uncompressed;
        set => this[nameof(CompressionType)] = value.ToString();
    }

    public string FileName
    {
        get => this[nameof(FileName)] ?? IFileAppenderConfig.DefaultFileName;
        set => this[nameof(FileName)] = value;
    }

    public string FilePath
    {
        get => this[nameof(FilePath)] ?? IFileAppenderConfig.DefaultFilePath;
        set => this[nameof(FilePath)] = value;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    object ICloneable.Clone() => Clone();

    IFileAppenderConfig ICloneable<IFileAppenderConfig>.Clone() => Clone();

    IFileAppenderConfig IFileAppenderConfig.Clone() => Clone();

    public override FileAppenderConfig Clone() => new(this);

    public override FileAppenderConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public override bool AreEquivalent(IAppenderReferenceConfig? other, bool exactTypes = false)
    {
        if (other is not IFileAppenderConfig fileAppender) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var fileAppendTypeSame = FileAppenderType == fileAppender.FileAppenderType;

        var fileNameSame = FileName == fileAppender.FileName;
        var filePathSame = FilePath == fileAppender.FilePath;

        var allAreSame = baseSame && fileAppendTypeSame && fileNameSame && filePathSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFormattingAppenderConfig, true);

    public override int GetHashCode()
    {
        var hashCode = base.GetHashCode();
        hashCode = (hashCode * 397) ^ (int)FileAppenderType;
        hashCode = (hashCode * 397) ^ FileName.GetHashCode();
        hashCode = (hashCode * 397) ^ FilePath.GetHashCode();
        return hashCode;
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        using var tb =
            sbc.StartComplexType(nameof(FileAppenderConfig))
               .Field.AlwaysAdd(nameof(FileAppenderType), FileAppenderType, FileAppenderTypeExtensions.FileAppenderTypeFormatter)
               .Field.AlwaysAdd(nameof(FileName), FileName)
               .Field.AlwaysAdd(nameof(FilePath), FilePath)
               .AddBaseFieldsStart();
        base.ToString(sbc);
        return tb.Complete();
    }
}
