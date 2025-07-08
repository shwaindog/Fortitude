using System.Text;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;

namespace FortitudeIO.Storage.Database.Config;

[Flags]
public enum DbQueryExpectation : uint
{
    Default                    = 0
  , RunOnce                    = 0x00_01
  , RetrievesMandatoryConfig   = 0x00_02
  , RetrievesOptionalConfig    = 0x00_04
  , RefreshPeriodically        = 0x00_08
  , RunByApplication           = 0x00_10
  , OnFailureExit              = 0x00_20
  , OnFailureLogError          = 0x00_40
  , OnFailureLogWarn           = 0x01_00
  , OnFailureLogInfo           = 0x02_00
  , OnFailureLogDebug          = 0x04_00
  , OnFailureIgnore            = 0x08_00
  , IsReadOnlyQuery            = 0x10_00
  , IsUpsertOperation          = 0x20_00
  , CanCreateSchemaDefinitions = 0x40_00
  , CanUpdateItemsRetrieved    = 0x80_00
}

public interface IDbQueryConfig : IAlternativeConfigLocationLookup<IDbQueryConfig>, IInterfacesComparable<IDbQueryConfig>, ICloneable<IDbQueryConfig>
{
    string  Query       { get; set; }
    string? Name        { get; set; }
    string? Description { get; set; }

    DbQueryExpectation QueryExpectation { get; set; }

    IDbConnectionConfig? UsingDbConnectionConfig { get; set; }
}

public class DbQueryConfig : ConfigSection, IDbQueryConfig
{
    private DbQueryConfig? lookupDbQuery;

    public DbQueryConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public DbQueryConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public DbQueryConfig
    (string query, DbQueryExpectation queryExpectation, string? name = null, string? description = null
      , IDbConnectionConfig? usingDbConnectionConfig = null)
        : this(InMemoryConfigRoot, InMemoryPath, query, queryExpectation, name, description, usingDbConnectionConfig) { }

    public DbQueryConfig
    (IConfigurationRoot root, string path, string query, DbQueryExpectation queryExpectation, string? name = null
      , string? description = null, IDbConnectionConfig? usingDbConnectionConfig = null) : base(root, path)
    {
        Query       = query;
        Name        = name;
        Description = description;

        QueryExpectation        = queryExpectation;
        UsingDbConnectionConfig = usingDbConnectionConfig;
    }

    public DbQueryConfig(IDbQueryConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        Query       = toClone.Query;
        Name        = toClone.Name;
        Description = toClone.Description;

        QueryExpectation        = toClone.QueryExpectation;
        UsingDbConnectionConfig = toClone.UsingDbConnectionConfig;
    }

    public DbQueryConfig(IDbQueryConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string? ConfigLookupReferencePath
    {
        get => this[nameof(ConfigLookupReferencePath)];
        set => this[nameof(ConfigLookupReferencePath)] = value;
    }

    public bool HasFoundConfigLookup => ConfigLookupReferencePath.IsNotNullOrEmpty();

    public IDbQueryConfig? LookupValue
    {
        get
        {
            if (HasFoundConfigLookup && lookupDbQuery == null)
            {
                lookupDbQuery = new DbQueryConfig(ConfigRoot, ConfigLookupReferencePath!);
            }
            return lookupDbQuery;
        }
    }
    public string Query
    {
        get => this[nameof(Query)] ?? LookupValue?.Query!;
        set => this[nameof(Query)] = value;
    }

    public string? Name
    {
        get => this[nameof(Name)] ?? LookupValue?.Name;
        set => this[nameof(Name)] = value;
    }

    public string? Description
    {
        get => this[nameof(Description)] ?? LookupValue?.Description;
        set => this[nameof(Description)] = value;
    }

    public DbQueryExpectation QueryExpectation
    {
        get
        {
            var checkValue = this[nameof(QueryExpectation)];
            return checkValue.IsNotNullOrEmpty()
                ? Enum.Parse<DbQueryExpectation>(checkValue)
                : LookupValue?.QueryExpectation ?? DbQueryExpectation.Default;
        }
        set => this[nameof(QueryExpectation)] = value.ToString();
    }

    public IDbConnectionConfig? UsingDbConnectionConfig
    {
        get
        {
            if (GetSection(nameof(UsingDbConnectionConfig)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                return new DbConnectionConfig(ConfigRoot, $"{Path}{Split}{nameof(UsingDbConnectionConfig)}");
            }
            return LookupValue?.UsingDbConnectionConfig;
        }
        set => _ = value != null ? new DbConnectionConfig(value, ConfigRoot, $"{Path}{Split}{nameof(UsingDbConnectionConfig)}") : null;
    }

    object ICloneable.Clone() => Clone();

    IDbQueryConfig ICloneable<IDbQueryConfig>.Clone() => Clone();

    public virtual DbQueryConfig Clone() => new(this);

    public virtual bool AreEquivalent(IDbQueryConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;
        
        var querySame         = Query == other.Query;
        var nameSame         = Name == other.Name;
        var descriptionSame         = Description == other.Description;
        var queryExpectationSame         = QueryExpectation == other.QueryExpectation;
        var usingDbConnectionSame         = UsingDbConnectionConfig?.AreEquivalent(other.UsingDbConnectionConfig, exactTypes) ?? other.UsingDbConnectionConfig == null;

        var allAreSame = querySame && nameSame && descriptionSame && queryExpectationSame && usingDbConnectionSame;

        return allAreSame;
    }


    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IDbQueryConfig, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Query.GetHashCode();
            hashCode = (hashCode * 397) ^ QueryExpectation.GetHashCode();
            hashCode = (hashCode * 397) ^ (Name?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (Description?.GetHashCode() ?? 0);
            hashCode = (hashCode * 397) ^ (UsingDbConnectionConfig?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public virtual string BuildToString()
    {
        var sb = new StringBuilder();
        sb.Append(nameof(Query)).Append(": ").Append(Query).Append(", ");
        sb.Append(nameof(QueryExpectation)).Append(": ").Append(QueryExpectation);
        if(Name != null) sb.Append(", ").Append(nameof(Name)).Append(": ").Append(Name);
        if(Description != null) sb.Append(", ").Append(nameof(Description)).Append(": ").Append(Description);
        if(UsingDbConnectionConfig != null) sb.Append(", ").Append(nameof(UsingDbConnectionConfig)).Append(": ").Append(UsingDbConnectionConfig);
        return sb.ToString();
    }

    public override string ToString() => $"{nameof(DbQueryConfig)}{{{BuildToString()}}}";
}
