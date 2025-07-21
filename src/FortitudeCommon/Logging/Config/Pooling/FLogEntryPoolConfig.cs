using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
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

    const int DefaultLogEntryCharsCapacity = 512;
    const int DefaultLargeLogEntryCharsCapacity = 8000;
    const int DefaultVeryLargeLogEntryCharsCapacity = 40_000; // Large Object Heap (LOH) ~ 85k / 2 bytes per char
                                                               // keeps backing buffer out of the LOH

    const int DefaultLogEntryBatchSize = 32;

    const PoolScope DefaultLogEntryScope = PoolScope.Default;

    string PoolName { get; }

    PoolScope PoolScope { get; }

    int NewItemCapacity { get; }

    int ItemBatchSize { get; }
}

public interface IMutableFLogEntryPoolConfig : IFLogEntryPoolConfig, IMutableFLogConfig
{
    new string PoolName { get; set; }

    new PoolScope PoolScope { get; set; }

    new int NewItemCapacity { get; set; }

    new int ItemBatchSize { get; set; }
}

public class FLogEntryPoolConfig : FLogConfig, IMutableFLogEntryPoolConfig
{
    public FLogEntryPoolConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public FLogEntryPoolConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public FLogEntryPoolConfig
        (string poolName, PoolScope poolScope = PoolScope.Default
          , int newItemCapacity = IFLogEntryPoolConfig.DefaultLogEntryCharsCapacity
          , int itemBatchSize = IFLogEntryPoolConfig.DefaultLogEntryBatchSize)
        : this(InMemoryConfigRoot, InMemoryPath, poolName, poolScope, newItemCapacity, itemBatchSize) { }

    public FLogEntryPoolConfig
    (IConfigurationRoot root, string path, string poolName, PoolScope poolScope = PoolScope.Default
      , int newItemCapacity = IFLogEntryPoolConfig.DefaultLogEntryCharsCapacity
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

    public override T Visit<T>(T visitor) => visitor.Accept(this);

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

    public IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
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
