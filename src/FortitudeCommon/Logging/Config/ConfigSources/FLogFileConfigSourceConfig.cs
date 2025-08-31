// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.ConfigSources;

public interface IFLogFileConfigSourceConfig : IFlogConfigSource
{
    bool FileSystemMonitored { get; }

    int PollInterval { get; }

    string FilePath { get; }

    new bool Optional { get; }
}

public interface IMutableFLogFileConfigSourceConfig : IFLogFileConfigSourceConfig, IMutableFlogConfigSource
{
    new bool FileSystemMonitored { get; set; }

    new int PollInterval { get; set; }

    new string FilePath { get; set; }

    new bool Optional { get; set; }
}

public class FLogFileConfigSourceConfig : FLogConfigSource, IMutableFLogFileConfigSourceConfig
{
    public FLogFileConfigSourceConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogFileConfigSourceConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLogFileConfigSourceConfig
    (ushort configPriorityOrder, FLogConfigSourceType sourceType, bool optional = false
      , string configSourceName = "", TimeSpanConfig? recheckConfigIntervalTimeSpan = null)
        : this(InMemoryConfigRoot, InMemoryPath, configPriorityOrder, sourceType, optional, configSourceName, recheckConfigIntervalTimeSpan) { }

    public FLogFileConfigSourceConfig
    (IConfigurationRoot root, string path, ushort configPriorityOrder, FLogConfigSourceType sourceType, bool optional = false
      , string? configSourceName = null, TimeSpanConfig? recheckConfigIntervalTimeSpan = null) : base(root, path)
    {
        ConfigPriorityOrder = configPriorityOrder;
        SourceType          = sourceType;
        Optional            = optional;
        ConfigSourceName    = configSourceName;
        if (recheckConfigIntervalTimeSpan != null) RecheckConfigIntervalTimeSpan = recheckConfigIntervalTimeSpan;
    }

    public FLogFileConfigSourceConfig(IFLogFileConfigSourceConfig toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path) { }

    public FLogFileConfigSourceConfig(IFLogFileConfigSourceConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string FilePath
    {
        get => this[nameof(FilePath)] ?? throw new ArgumentException("You must specify a file!");
        set => this[nameof(FilePath)] = value;
    }

    public bool FileSystemMonitored
    {
        get => bool.TryParse(this[nameof(FileSystemMonitored)], out var optional) && optional;
        set => this[nameof(FileSystemMonitored)] = value.ToString();
    }

    public int PollInterval
    {
        get => int.TryParse(this[nameof(PollInterval)], out var priorityOrder) ? priorityOrder : 0;
        set => this[nameof(PollInterval)] = value.ToString();
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public override IFlogConfigSource CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        new FLogFileConfigSourceConfig(this, configRoot, path);

    object ICloneable.Clone() => Clone();

    IFlogConfigSource ICloneable<IFlogConfigSource>.Clone() => Clone();

    public override FLogFileConfigSourceConfig Clone() => new(this);

    public override bool AreEquivalent(IFlogConfigSource? other, bool exactTypes = false)
    {
        if (other is not IFLogFileConfigSourceConfig fileConfigSource) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var filePathSame          = FilePath == fileConfigSource.FilePath;
        var fileSystemMonitorSame = FileSystemMonitored == fileConfigSource.FileSystemMonitored;
        var pollSame              = PollInterval == fileConfigSource.PollInterval;

        var allAreSame = baseSame && filePathSame && fileSystemMonitorSame && pollSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFlogConfigSource, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = base.GetHashCode();
            hashCode = (hashCode * 397) ^ FilePath.GetHashCode();
            hashCode = (hashCode * 397) ^ FileSystemMonitored.GetHashCode();
            hashCode = (hashCode * 397) ^ PollInterval.GetHashCode();
            return hashCode;
        }
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc) =>
        sbc.StartComplexType(nameof(FLogFileConfigSourceConfig))
           .AddBaseStyledToStringFields(this)
           .Field.AlwaysAdd(nameof(FilePath), FilePath)
           .Field.AlwaysAdd(nameof(FileSystemMonitored), FileSystemMonitored)
           .Field.AlwaysAdd(nameof(PollInterval), PollInterval)
           .Complete();
}
