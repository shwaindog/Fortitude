// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Pooling;

public enum PoolScope
{
    Default
  , PrivateNamed
  , SharedNamed
  , Global
  , LargeMessage
  , VeryLargeMessage
  , LoggersGlobal
  , LoggerAndDescendants
  , AppendersGlobal
}

public interface IFLogEntryPoolConfig : IInterfacesComparable<IFLogEntryPoolConfig>, ICloneable<IFLogEntryPoolConfig>
  , IStyledToStringObject, IFLogConfig
{
    const string Default          = "Default";
    const string Global           = "Global";
    const string LoggersGlobal    = "LoggersGlobal";
    const string AppendersGlobal  = "AppendersGlobal";
    const string LargeMessage     = "LargeMessage";
    const string VeryLargeMessage = "VeryLargeMessage";

    const int DefaultLogEntryCharsCapacity          = 512;
    const int DefaultLargeLogEntryCharsCapacity     = 8000;
    const int DefaultVeryLargeLogEntryCharsCapacity = 40_000;

    // Large Object Heap (LOH) ~ 85k / 2 bytes per char
    // keeps backing buffer out of the LOH

    const int DefaultLogEntryBatchSize = 32;

    const PoolScope DefaultLogEntryScope = PoolScope.Default;

    string PoolName { get; }

    PoolScope PoolScope { get; }

    int LogEntryCharCapacity { get; }

    int LogEntriesBatchSize { get; }
}

public interface IMutableFLogEntryPoolConfig : IFLogEntryPoolConfig, IMutableFLogConfig
{
    new string PoolName { get; set; }

    new PoolScope PoolScope { get; set; }

    new int LogEntryCharCapacity { get; set; }

    new int LogEntriesBatchSize { get; set; }
}

public class FLogEntryPoolConfig : FLogConfig, IMutableFLogEntryPoolConfig
{
    public FLogEntryPoolConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogEntryPoolConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLogEntryPoolConfig
    (string poolName, PoolScope poolScope = PoolScope.Default
      , int logEntryCharCapacity = IFLogEntryPoolConfig.DefaultLogEntryCharsCapacity
      , int logEntriesBatchSize = IFLogEntryPoolConfig.DefaultLogEntryBatchSize)
        : this(InMemoryConfigRoot, InMemoryPath, poolName, poolScope, logEntryCharCapacity, logEntriesBatchSize) { }

    public FLogEntryPoolConfig
    (IConfigurationRoot root, string path, string poolName, PoolScope poolScope = PoolScope.Default
      , int logEntryCharCapacity = IFLogEntryPoolConfig.DefaultLogEntryCharsCapacity
      , int logEntriesBatchSize = IFLogEntryPoolConfig.DefaultLogEntryBatchSize) : base(root, path)
    {
        PoolName  = poolName;
        PoolScope = poolScope;

        LogEntryCharCapacity = logEntryCharCapacity;
        LogEntriesBatchSize  = logEntriesBatchSize;
    }

    public FLogEntryPoolConfig(IFLogEntryPoolConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        PoolName  = toClone.PoolName;
        PoolScope = toClone.PoolScope;

        LogEntryCharCapacity = toClone.LogEntryCharCapacity;
        LogEntriesBatchSize  = toClone.LogEntriesBatchSize;
    }

    public FLogEntryPoolConfig(IFLogEntryPoolConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string PoolName
    {
        get => this[nameof(PoolName)] ?? IFLogEntryPoolConfig.Global;
        set => this[nameof(PoolName)] = value;
    }

    public PoolScope PoolScope
    {
        get =>
            Enum.TryParse<PoolScope>(this[nameof(PoolScope)], out var poolScope)
                ? poolScope
                : IFLogEntryPoolConfig.DefaultLogEntryScope;
        set => this[nameof(PoolScope)] = value.ToString();
    }

    public int LogEntriesBatchSize
    {
        get => int.TryParse(this[nameof(LogEntriesBatchSize)], out var timePart) ? timePart : SourcePoolsInitConfig().DefaultLogEntryBatchSize;
        set => this[nameof(LogEntriesBatchSize)] = value.ToString();
    }

    public int LogEntryCharCapacity
    {
        get => int.TryParse(this[nameof(LogEntryCharCapacity)], out var timePart) ? timePart : SourcePoolsInitConfig().DefaultLogEntryCharCapacity;
        set => this[nameof(LogEntryCharCapacity)] = value.ToString();
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    protected virtual ILogEntryPoolsInitializationConfig SourcePoolsInitConfig()
    {
        if (ParentConfig is IMutableLogEntryPoolsInitializationConfig poolsInitializationConfig) return poolsInitializationConfig;
        return FLogContext.Context.LogEntryPoolRegistry.LogEntryPoolInitConfig;
    }

    object ICloneable.Clone() => Clone();

    IFLogEntryPoolConfig ICloneable<IFLogEntryPoolConfig>.Clone() => Clone();

    public FLogEntryPoolConfig Clone() => new(this);

    public bool AreEquivalent(IFLogEntryPoolConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var nameSame  = PoolName == other.PoolName;
        var scopeSame = PoolScope == other.PoolScope;

        var entrySizeSame = LogEntryCharCapacity == other.LogEntryCharCapacity;
        var batchSizeSame = LogEntriesBatchSize == other.LogEntriesBatchSize;

        var allAreSame = nameSame && scopeSame && entrySizeSame && batchSizeSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IFLogEntryPoolConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = PoolName.GetHashCode();
            hashCode = (hashCode * 397) ^ PoolScope.GetHashCode();
            hashCode = (hashCode * 397) ^ LogEntryCharCapacity;
            hashCode = (hashCode * 397) ^ LogEntriesBatchSize;
            return hashCode;
        }
    }

    public StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .Field.AlwaysAdd(nameof(PoolName), PoolName)
           .Field.AlwaysAdd(nameof(PoolScope), PoolScope.ToString())
           .Field.AlwaysAdd(nameof(LogEntryCharCapacity), LogEntryCharCapacity)
           .Field.AlwaysAdd(nameof(LogEntriesBatchSize), LogEntriesBatchSize)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
