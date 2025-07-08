using System.Text;
using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using FortitudeIO.Config;
using Microsoft.Extensions.Configuration;

namespace FortitudeIO.Storage.Database.Config;

public enum DbType
{
    Unknown
  , SqlServer
  , SqlServerCompact
  , SqLite2
  , SqLite3
  , EfInMemory
  , Cosmos
  , Postgres
  , MySql
  , Maria
  , Oracle
  , MongoDb
  , Db2
  , BigCommerce
  , Dynamics365
  , FreshBooks
  , Magento
  , Mailchimp
  , Quickbooks
  , Salesforce
  , SugarCrm
  , ZohoCrm
  , ZohoBooks
  , MsAccess
  , GoogleSpanner
  , Terradata
  , FileContext
  , FileBase
  , OpenEdge
  , Snowflake
}

public enum MultiSelectionExpectations
{
    Default    = 0
  , OnlyOne    = 0x00_01
  , AtLeastOne = 0x00_02
  , All        = 0x00_04
}

[Flags]
public enum DbConnectionExpectation : uint
{
    Default                        = 0
  , MustExistAndConnect            = 0x00_01
  , OptionalConnectOk              = 0x00_02
  , IsFallbackCacheDb              = 0x00_04
  , ShouldCreateMissingFileDb      = 0x00_08
  , HasReadOnlyPermissions         = 0x00_10
  , CanCreateSchemaDefinitions     = 0x00_20
  , OnConnectAttemptCodeMigrations = 0x00_40
  , CanUpdateItemsRetrieved        = 0x00_80
  , IsTemporalDateTimeFileDb       = 0x01_00
  , IsRemoteLocationDb             = 0x02_00
  , DisableReattempts              = 0x04_00
}

public interface IDbConnectionConfig<T> : IRemoteCredentialsConfig, IAlternativeConfigLocationLookup<T>
where T : IDbConnectionConfig<T>
{
    string? HostName         { get; set; }
    string? DatabaseName     { get; set; }
    DbType  DatabaseType     { get; set; }
    string? Schema           { get; set; }
    string? DatabasePath     { get; set; }
    string? ConnectionString { get; set; }

    string ResolvedConnectionString { get; }

    Func<IDbConnectionConfig<T>, string> ConnectionStringBuilder { get; set; }
}

public interface IDbConnectionConfig : IDbConnectionConfig<IDbConnectionConfig>
{
}

public abstract class DbConnectionConfig<T> : RemoteCredentialsConfig, IDbConnectionConfig<T>
    where T : IDbConnectionConfig<T>
{
    protected T? LookupDbConfig;

    protected DbConnectionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    protected DbConnectionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    protected DbConnectionConfig
    (string hostName, bool enabled, string remoteAddress, string? username = null
      , string? password = null, string? apiToken = null, string? certificatePath = null)
    : this(InMemoryConfigRoot, InMemoryPath, hostName, enabled, remoteAddress, username, password, apiToken, certificatePath)
    {
    }

    protected DbConnectionConfig
    (IConfigurationRoot root, string path, string hostName, bool enabled, string remoteAddress, string? username = null
      , string? password = null, string? apiToken = null, string? certificatePath = null)
    : base(root, path)
    {
        HostName           = hostName;
        Enabled            = enabled;
        RemoteAddress      = remoteAddress;
        Username           = username;
        Password           = password;
        ApiToken           = apiToken;
        CertificatePath    = certificatePath;
        CertificatePayload = CertificatePayload;
    }

    protected DbConnectionConfig(IDbConnectionConfig<T> toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        ConfigLookupReferencePath = toClone.ConfigLookupReferencePath;

        HostName      = toClone.HostName;
        Enabled       = toClone.Enabled;
        RemoteAddress = toClone.RemoteAddress;
        Username      = toClone.Username;
        Password      = toClone.Password;
        ApiToken      = toClone.ApiToken;

        CertificatePath    = toClone.CertificatePath;
        CertificatePayload = toClone.CertificatePayload;
    }

    protected DbConnectionConfig(IDbConnectionConfig<T> toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override RemoteCredentialsFieldFlags DefaultObscureFieldFlags =>
        base.DefaultObscureFieldFlags
      | RemoteCredentialsFieldFlags.HostName
      | RemoteCredentialsFieldFlags.DatabaseName
      | RemoteCredentialsFieldFlags.Schema
      | RemoteCredentialsFieldFlags.Username
      | RemoteCredentialsFieldFlags.Password
      | RemoteCredentialsFieldFlags.ConnectionString;


    public string? ConfigLookupReferencePath
    {
        get => this[nameof(ConfigLookupReferencePath)];
        set => this[nameof(ConfigLookupReferencePath)] = value;
    }

    public bool HasFoundConfigLookup => ConfigLookupReferencePath.IsNotNullOrEmpty();

    public abstract T? LookupValue { get; }

    public string? DatabaseName
    {
        get => this[nameof(DatabaseName)] ?? LookupValue?.DatabaseName;
        set => this[nameof(DatabaseName)] = value;
    }

    public string? DatabasePath
    {
        get => this[nameof(HostName)] ?? LookupValue?.DatabasePath;
        set => this[nameof(HostName)] = value;
    }

    public DbType DatabaseType
    {
        get
        {
            var checkValue = this[nameof(DbType)];
            return checkValue.IsNotNullOrEmpty() ? Enum.Parse<DbType>(checkValue) : LookupValue?.DatabaseType ?? DbType.Unknown;
        }
        set => this[nameof(DbType)] = value.ToString();
    }

    public override ushort? Port
    {
        get
        {
            var checkValue = this[nameof(Port)];
            return checkValue.IsNotNullOrEmpty() ? ushort.Parse(checkValue) : LookupValue?.Port;
        }
        set => this[nameof(Port)] = value.ToString();
    }

    public string? ConnectionString
    {
        get => this[nameof(ConnectionString)] ?? LookupValue?.ConnectionString;
        set => this[nameof(ConnectionString)] = value;
    }

    public string ResolvedConnectionString => ConnectionString ?? LookupValue?.ResolvedConnectionString ?? ConnectionStringBuilder(this);

    public Func<IDbConnectionConfig<T>, string> ConnectionStringBuilder { get; set; } = _ => string.Empty;

    public override bool Enabled
    {
        get => bool.TryParse(this[nameof(Enabled)]!, out var result)? result : (LookupValue?.Enabled ?? true);
        set => this[nameof(Enabled)] = value.ToString();
    }

    public override string Name
    {
        get => this[nameof(Name)] ?? LookupValue!.Name;
        set => this[nameof(Name)] = value;
    }

    public string? Schema
    {
        get => this[nameof(HostName)] ?? LookupValue?.Schema;
        set => this[nameof(HostName)] = value;
    }

    public string? HostName
    {
        get => this[nameof(HostName)] ?? LookupValue?.HostName;
        set => this[nameof(HostName)] = value;
    }

    public override string? RemoteAddress
    {
        get => this[nameof(RemoteAddress)] ?? LookupValue?.RemoteAddress;
        set => this[nameof(RemoteAddress)] = value;
    }

    public override string? Username
    {
        get => this[nameof(Username)] ?? LookupValue?.Username;
        set => this[nameof(Username)] = value;
    }

    public override string? Password
    {
        get => this[nameof(Password)] ?? LookupValue?.Password;
        set => this[nameof(Password)] = value;
    }

    public override string? ApiToken
    {
        get => this[nameof(ApiToken)] ?? LookupValue?.ApiToken;
        set => this[nameof(ApiToken)] = value;
    }

    public override string? CertificatePath
    {
        get => this[nameof(CertificatePath)] ?? LookupValue?.CertificatePath;
        set => this[nameof(CertificatePath)] = value;
    }

    public override string? CertificatePayload
    {
        get => this[nameof(CertificatePayload)] ?? LookupValue?.CertificatePayload;
        set => this[nameof(CertificatePayload)] = value;
    }

    object ICloneable.Clone() => Clone();

    public override DbConnectionConfig<T> Clone() => throw new NotImplementedException("Derived classes must override");

    public override bool AreEquivalent(IRemoteCredentialsConfig? other, bool exactTypes = false)
    {
        var baseSame = base.AreEquivalent(other, exactTypes);
        if (other is not IDbConnectionConfig dbConnConfig) return baseSame;

        var hostNameSame   = HostName == dbConnConfig.HostName;
        var dbNameSame     = DatabaseName == dbConnConfig.DatabaseName;
        var dbTypeSame     = DatabaseType == dbConnConfig.DatabaseType;
        var schemaSame     = Schema == dbConnConfig.Schema;
        var dbPathSame     = DatabasePath == dbConnConfig.DatabasePath;
        var connStringSame = ConnectionString == dbConnConfig.ConnectionString;


        var allAreSame = baseSame && hostNameSame && dbNameSame && dbTypeSame && schemaSame && dbPathSame && connStringSame;

        return allAreSame;
    }

    public override string BuildToString(bool secureSensitive)
    {
        var sb = new StringBuilder();

        sb.Append(base.BuildToString(secureSensitive)).Append(", ");
        var secureFlags = ObscureFieldFlags;
        sb.Append(nameof(DatabaseType)).Append(": ").Append(DatabaseType).Append(", ");
        if (ConfigLookupReferencePath.IsNotNullOrEmpty())
        {
            sb.Append(nameof(ConfigLookupReferencePath)).Append(": ").Append(ConfigLookupReferencePath).Append(", ");
        }
        if (HostName.IsNotNullOrEmpty())
        {
            sb.Append(nameof(HostName)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecureHostName() ? Obscure(HostName) : HostName).Append(", ");
        }
        if (DatabaseName.IsNotNullOrEmpty())
        {
            sb.Append(nameof(DatabaseName)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecureDatabaseName() ? Obscure(DatabaseName) : DatabaseName).Append(", ");
        }
        if (Schema.IsNotNullOrEmpty())
        {
            sb.Append(nameof(Schema)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecureSchema() ? Obscure(Schema) : Schema).Append(", ");
        }
        if (DatabasePath.IsNotNullOrEmpty())
        {
            sb.Append(nameof(DatabasePath)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecureDatabasePath() ? Obscure(DatabasePath) : DatabasePath).Append(", ");
        }
        if (ConnectionString.IsNotNullOrEmpty())
        {
            sb.Append(nameof(ConnectionString)).Append(": ");
            sb.Append(secureSensitive && secureFlags.IsSecureConnectionString() ? Obscure(ConnectionString) : ConnectionString).Append(", ");
        }
        sb.Append(nameof(ResolvedConnectionString)).Append(": ").Append(secureSensitive && secureFlags.IsSecureResolvedConnectionString()
                                                                            ? Obscure(ResolvedConnectionString)
                                                                            : ResolvedConnectionString);

        return sb.ToString();
    }

    protected string DbConnectionConfigToStringMembers => $"{BuildToString(ObscureToString)}";

    public override string ToString() => $"{nameof(DbConnectionConfig<T>)}{{{DbConnectionConfigToStringMembers}}}";
}

public class DbConnectionConfig : DbConnectionConfig<IDbConnectionConfig>, IDbConnectionConfig
{
    public DbConnectionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public DbConnectionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }
    
    public DbConnectionConfig
    (string hostName, bool enabled, string remoteAddress, string? username = null
      , string? password = null, string? apiToken = null, string? certificatePath = null)
        : this(InMemoryConfigRoot, InMemoryPath, hostName, enabled, remoteAddress, username, password, apiToken, certificatePath)
    {
    }

    public DbConnectionConfig
    (IConfigurationRoot root, string path, string hostName, bool enabled, string remoteAddress, string? username = null
      , string? password = null, string? apiToken = null, string? certificatePath = null)
        : base(root, path, hostName, enabled, remoteAddress, username, password, apiToken, certificatePath)
    {
    }

    public DbConnectionConfig(IDbConnectionConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
    }

    public DbConnectionConfig(IDbConnectionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override IDbConnectionConfig? LookupValue
    {
        get
        {
            if (HasFoundConfigLookup && LookupDbConfig == null)
            {
                LookupDbConfig = new DbConnectionConfig(ConfigRoot, ConfigLookupReferencePath!);
            }
            return LookupDbConfig;
        }
    }

    object ICloneable.Clone() => Clone();

    public override DbConnectionConfig Clone() => new(this);

    public override string ToString() => $"{nameof(DbConnectionConfig)}{{{DbConnectionConfigToStringMembers}}}";
}
