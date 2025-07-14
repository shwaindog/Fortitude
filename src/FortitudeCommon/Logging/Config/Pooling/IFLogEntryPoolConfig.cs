using FortitudeCommon.Config;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Pooling;

public enum PoolScope
{
    Default
  , Private
  , Global
  , Named
  , LoggersGlobal
  , LoggerDescendants
  , AppendersGlobal
  , AppendersAllFile
}

public interface IFLogEntryPoolConfig : IInterfacesComparable<IFLogEntryPoolConfig>, ICloneable<IFLogEntryPoolConfig>
, IStyledToStringObject
{
    const string Global        = "Global";
    const string LoggersGlobal = "LoggersGlobal";

    const int DefaultLogEntryStringCapacity = 512;

    const int DefaultLogEntryBatchSize = 32;

    const PoolScope DefaultLogEntryScope = PoolScope.Default;

    string PoolName { get; }

    PoolScope PoolScope { get; }

    int NewItemCapacity { get; }

    int ItemBatchSize { get; }
}

public interface IMutableFLogEntryPoolConfig : IFLogEntryPoolConfig
{
    new string PoolName { get; set; }

    new PoolScope PoolScope { get; set; }

    new int NewItemCapacity { get; set; }

    new int ItemBatchSize { get; set; }
}

public class FLogEntryPoolConfig : ConfigSection, IMutableFLogEntryPoolConfig
{
    public FLogEntryPoolConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogEntryPoolConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLogEntryPoolConfig
        (string poolName, PoolScope poolScope = PoolScope.Default
          , int newItemCapacity = IFLogEntryPoolConfig.DefaultLogEntryStringCapacity
          , int itemBatchSize = IFLogEntryPoolConfig.DefaultLogEntryBatchSize)
        : this(InMemoryConfigRoot, InMemoryPath, poolName, poolScope, newItemCapacity, itemBatchSize) { }

    public FLogEntryPoolConfig
    (IConfigurationRoot root, string path, string poolName, PoolScope poolScope = PoolScope.Default
      , int newItemCapacity = IFLogEntryPoolConfig.DefaultLogEntryStringCapacity
      , int itemBatchSize = IFLogEntryPoolConfig.DefaultLogEntryBatchSize) : base(root, path)
    {
        PoolName        = poolName;
        PoolScope       = poolScope;
        NewItemCapacity = newItemCapacity;
        ItemBatchSize   = itemBatchSize;
    }

    public FLogEntryPoolConfig(IFLogEntryPoolConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        PoolName        = toClone.PoolName;
        PoolScope       = toClone.PoolScope;
        NewItemCapacity = toClone.NewItemCapacity;
        ItemBatchSize   = toClone.ItemBatchSize;
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

    public int ItemBatchSize
    {
        get => int.TryParse(this[nameof(ItemBatchSize)], out var timePart) ? timePart : 0;
        set => this[nameof(ItemBatchSize)] = value.ToString();
    }

    public int NewItemCapacity
    {
        get => int.TryParse(this[nameof(NewItemCapacity)], out var timePart) ? timePart : 0;
        set => this[nameof(NewItemCapacity)] = value.ToString();
    }

    object ICloneable.Clone() => Clone();

    IFLogEntryPoolConfig ICloneable<IFLogEntryPoolConfig>.Clone() => Clone();

    public FLogEntryPoolConfig Clone() => new(this);

    public bool AreEquivalent(IFLogEntryPoolConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var nameSame = PoolName == other.PoolName;
        var scopeSame = PoolScope == other.PoolScope;
        var entrySizeSame = NewItemCapacity == other.NewItemCapacity;
        var batchSizeSame = ItemBatchSize == other.ItemBatchSize;

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
            hashCode = (hashCode * 397) ^ NewItemCapacity;
            hashCode = (hashCode * 397) ^ ItemBatchSize;
            return hashCode;
        }
    }

    public void ToString(IStyledTypeStringAppender sbc)
    {
        sbc.AddTypeName(nameof(FLogEntryPoolConfig))
           .AddTypeStart()
           .AddField(nameof(PoolName), PoolName)
           .AddField(nameof(PoolScope), PoolScope.ToString())
           .AddField(nameof(NewItemCapacity), NewItemCapacity)
           .AddField(nameof(ItemBatchSize), ItemBatchSize)
           .AddTypeEnd();
    }

    public override string ToString() => this.DefaultToString();
}
