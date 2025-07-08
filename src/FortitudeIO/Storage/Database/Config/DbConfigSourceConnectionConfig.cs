using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using Microsoft.Extensions.Configuration;
using static FortitudeIO.Storage.Database.Config.DbRepositoryConfigRetrieveStatus;

namespace FortitudeIO.Storage.Database.Config;

public interface IDbConfigSourceConnectionConfig<TConfig> : IDbConnectionConfig<IDbConfigSourceConnectionConfig<TConfig>>
  , IExternallySourcedWithGuaranteedConfig<TConfig> where TConfig : class, IConfigCombine<TConfig>
{
    bool IsRequiredStartupConfig { get; set; }

    string? DbQuery { get; set; }

    string? EntityQuery { get; set; }

    IReadOnlyDictionary<string, string>? Params { get; set; }

    bool Succeeded { get; }

    DbRepositoryConfigRetrieveStatus DbRetrievalStatus { get; set; }

    string? ConfigAdapterTypeFullName { get; set; }

    IDbRepositoryConfigAdapter<TConfig>? ConfigRepositoryAdapter { get; }

    bool AddParamIfMissing(KeyValuePair<string, string> checkAdd);
}

public class DbConfigSourceConnectionConfig<TConfig> : DbConnectionConfig<IDbConfigSourceConnectionConfig<TConfig>>
  , IDbConfigSourceConnectionConfig<TConfig>
    where TConfig : class, IConfigCombine<TConfig>
{
    protected Dictionary<string, string>           QueryParameterDictionary = new();
    protected IDbRepositoryConfigAdapter<TConfig>? ResolveDbRepositoryConfigAdapter;

    public DbConfigSourceConnectionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public DbConfigSourceConnectionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public DbConfigSourceConnectionConfig
    (string hostName, bool enabled, string remoteAddress, string? username = null
      , string? password = null, bool isRequiredStartupConfig = false, IReadOnlyDictionary<string, string>? dbParams = null
      , string? dbQuery = null, string? entityQuery = null, string? configAdapterTypeFullName = null
      , bool disableMustIncludeResults = false, TConfig? mustIncludeResults = null
      , string? apiToken = null, string? certificatePath = null)
        : this(InMemoryConfigRoot, InMemoryPath, hostName, enabled, remoteAddress, username, password, apiToken, certificatePath)
    {
        IsRequiredStartupConfig         = isRequiredStartupConfig;
        ConfigAdapterTypeFullName       = configAdapterTypeFullName;
        DisableIncludeGuaranteedResults = disableMustIncludeResults;
        GuaranteedResults               = mustIncludeResults;

        Params      = dbParams;
        DbQuery     = dbQuery;
        EntityQuery = entityQuery;
    }

    public DbConfigSourceConnectionConfig
    (IConfigurationRoot root, string path, string hostName, bool enabled, string remoteAddress, string? username = null
      , string? password = null, string? apiToken = null, string? certificatePath = null)
        : base(root, path, hostName, enabled, remoteAddress, username, password, apiToken, certificatePath) { }

    public DbConfigSourceConnectionConfig(IDbConfigSourceConnectionConfig<TConfig> toClone, IConfigurationRoot root, string path)
        : base(toClone, root, path)
    {
        IsRequiredStartupConfig         = toClone.IsRequiredStartupConfig;
        ConfigAdapterTypeFullName       = toClone.ConfigAdapterTypeFullName;
        DisableIncludeGuaranteedResults = toClone.DisableIncludeGuaranteedResults;
        GuaranteedResults               = toClone.GuaranteedResults;

        Params      = toClone.Params;
        DbQuery     = toClone.DbQuery;
        EntityQuery = toClone.EntityQuery;

        RetrievedExternalConfig = toClone.RetrievedExternalConfig?.Clone();
        ResolvedConfig          = toClone.ResolvedConfig?.Clone();
        ConfigRepositoryAdapter = toClone.ConfigRepositoryAdapter;

        Succeeded = toClone.Succeeded;
    }

    public DbConfigSourceConnectionConfig(IDbConfigSourceConnectionConfig<TConfig> toClone)
        : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public override IDbConfigSourceConnectionConfig<TConfig>? LookupValue
    {
        get
        {
            if (HasFoundConfigLookup && LookupDbConfig == null)
            {
                LookupDbConfig = new DbConfigSourceConnectionConfig<TConfig>(ConfigRoot, ConfigLookupReferencePath!);
            }
            return LookupDbConfig;
        }
    }

    public bool IsRequiredStartupConfig
    {
        get => bool.TryParse(this[nameof(IsRequiredStartupConfig)]!, out var result) ? result : (LookupValue?.IsRequiredStartupConfig ?? false);
        set => this[nameof(IsRequiredStartupConfig)] = value.ToString();
    }

    public IReadOnlyDictionary<string, string>? Params
    {
        get
        {
            if (!QueryParameterDictionary.Any() && LookupValue is { Params.Count: > 0 })
            {
                foreach (var kvp in LookupValue.Params)
                {
                    var key        = kvp.Key;
                    var paramValue = kvp.Value;
                    if (paramValue.IsNullOrEmpty()) continue;
                    if (!QueryParameterDictionary.ContainsKey(key))
                        QueryParameterDictionary.TryAdd(key, paramValue);
                    else
                        QueryParameterDictionary[key] = paramValue;
                }
            }
            if (!QueryParameterDictionary.Any() && GetSection(nameof(Params)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                foreach (var configurationSection in GetSection(nameof(Params)).GetChildren())
                {
                    var key        = configurationSection.Key;
                    var paramValue = configurationSection.Value;
                    if (paramValue.IsNullOrEmpty()) continue;
                    if (!QueryParameterDictionary.ContainsKey(key))
                        QueryParameterDictionary.TryAdd(key, paramValue);
                    else
                        QueryParameterDictionary[key] = paramValue;
                }
            }

            return QueryParameterDictionary;
        }
        set
        {
            var oldKeys = QueryParameterDictionary.Keys.ToHashSet();
            QueryParameterDictionary.Clear();
            if (value != null)
            {
                foreach (var kvp in value)
                {
                    var key        = kvp.Key;
                    var paramValue = kvp.Value;
                    this[$"{Params}{Split}{key}"] = paramValue;
                    if (!QueryParameterDictionary.ContainsKey(key))
                        QueryParameterDictionary.TryAdd(key, paramValue);
                    else
                        QueryParameterDictionary[key] = paramValue;
                }
            }

            var deletedKeys = oldKeys.Except(value?.Keys.ToHashSet() ?? []);
            foreach (var deletedKey in deletedKeys)
            {
                var key = deletedKey;
                this[$"{Params}{Split}{key}"] = null;
            }
        }
    }

    public string? DbQuery
    {
        get => this[nameof(DbQuery)] ?? LookupDbConfig?.DbQuery;
        set => this[nameof(DbQuery)] = value;
    }

    public string? EntityQuery
    {
        get => this[nameof(EntityQuery)] ?? LookupDbConfig?.EntityQuery;
        set => this[nameof(EntityQuery)] = value;
    }

    public virtual string? ConfigAdapterTypeFullName
    {
        get => this[nameof(ConfigAdapterTypeFullName)] ?? LookupDbConfig?.ConfigAdapterTypeFullName;
        set => this[nameof(ConfigAdapterTypeFullName)] = value;
    }

    public virtual IDbRepositoryConfigAdapter<TConfig>? ConfigRepositoryAdapter
    {
        get
        {
            var configAdapterTypeName = ConfigAdapterTypeFullName;
            if (ResolveDbRepositoryConfigAdapter == null && configAdapterTypeName != null)
            {
                var resolvedAdapterType = Type.GetType(configAdapterTypeName);
                if (resolvedAdapterType != null)
                {
                    ResolveDbRepositoryConfigAdapter = (IDbRepositoryConfigAdapter<TConfig>?)Activator.CreateInstance(resolvedAdapterType);
                }
            }
            ResolveDbRepositoryConfigAdapter ??= LookupDbConfig?.ConfigRepositoryAdapter;
            if (ResolveDbRepositoryConfigAdapter == null 
             && GetSection(nameof(ConfigRepositoryAdapter))
                .GetChildren()
                .Any(cs => cs.Key is nameof(IDbEmbeddedFallbackConfig<TConfig>.DisableEmbeddedConfig) 
                                  or nameof(IDbEmbeddedFallbackConfig<TConfig>.EmbeddedDbConfigFallback)))
            {
                return new DbEmbeddedFallbackConfig<TConfig>(ConfigRoot, $"{Path}{Split}{nameof(ConfigRepositoryAdapter)}");
            }
            return ResolveDbRepositoryConfigAdapter;
        }
        set => ResolveDbRepositoryConfigAdapter = value;
    }

    public bool Succeeded { get; protected set; }

    public DbRepositoryConfigRetrieveStatus DbRetrievalStatus
    {
        get =>
            Enum.TryParse<DbRepositoryConfigRetrieveStatus>(this[nameof(DbRepositoryConfigRetrieveStatus)], out var retrieveStatus)
                ? retrieveStatus
                : LookupValue?.DbRetrievalStatus ?? NeverAttempted;
        set => this[nameof(DbRetrievalStatus)] = value.ToString();
    }

    public TConfig? RetrievedExternalConfig { get; protected set; }

    public bool DisableIncludeGuaranteedResults
    {
        get =>
            bool.TryParse(this[nameof(DisableIncludeGuaranteedResults)]!, out var result)
                ? result
                : (LookupValue?.DisableIncludeGuaranteedResults ?? false);
        set => this[nameof(DisableIncludeGuaranteedResults)] = value.ToString();
    }

    public TConfig? GuaranteedResults
    {
        get
        {
            if (GetSection(nameof(GuaranteedResults)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                var check = (TConfig)Activator.CreateInstance(typeof(TConfig), ConfigRoot, $"{Path}{Split}{nameof(GuaranteedResults)}")!;
                return check;
            }
            return LookupDbConfig?.GuaranteedResults;
        }
        set
        {
            if (value != null)
            {
                _ = (TConfig)Activator.CreateInstance(typeof(TConfig), value, ConfigRoot, $"{Path}{Split}{nameof(GuaranteedResults)}")!;
            }
        }
    }

    public TConfig? ResolvedConfig { get; protected set; }

    public bool AddParamIfMissing(KeyValuePair<string, string> checkAdd)
    {
        if (!QueryParameterDictionary.Any())
        {
            _ = Params;
        }
        if (!QueryParameterDictionary.ContainsKey(checkAdd.Key))
        {
            QueryParameterDictionary.Add(checkAdd.Key, checkAdd.Value);
            return true;
        }
        return false;
    }

    public virtual bool ResolvedExternalConfig()
    {
        var configRepositoryAdapter = ConfigRepositoryAdapter;
        if (configRepositoryAdapter == null)
        {
            Succeeded = HandleNoDbRepositoryConfigAdapter();
            return Succeeded;
        }
        var adapterResult = configRepositoryAdapter.RetrieveConfig(this, out var dbResult);
        DbRetrievalStatus       |= adapterResult;
        RetrievedExternalConfig =  dbResult;
        if (adapterResult.HasAnyOf(InsufficientParameters | InvalidDbQuery | InvalidEntityQuery | DbUnavailable | DbPermissionsError |
                                   NoResultsReturned | MissingDbDefinition))
        {
            Succeeded = HandleNoResultsCheckMustInclude();
        }
        if (adapterResult.HasDbRetrieveSucceededFlag() && dbResult != null)
        {
            var mustIncludeResult = GuaranteedResults;
            if (mustIncludeResult != null && !DisableIncludeGuaranteedResults)
            {
                DbRetrievalStatus |= UsedMustIncludedResults;
                ResolvedConfig    =  dbResult.Combine(mustIncludeResult);
            }
            else
            {
                DbRetrievalStatus |= HasConfigResults;
                ResolvedConfig    =  dbResult;
            }
            Succeeded = true;
        }

        return Succeeded;
    }

    protected virtual bool HandleNoDbRepositoryConfigAdapter()
    {
        DbRetrievalStatus |= InvalidAdapterType;
        return HandleNoResultsCheckMustInclude();
    }

    protected virtual bool HandleNoResultsCheckMustInclude()
    {
        var mustIncludeResult = GuaranteedResults;
        if (mustIncludeResult != null && !DisableIncludeGuaranteedResults)
        {
            DbRetrievalStatus |= UsedMustIncludedResults | HasConfigResults;
            ResolvedConfig    =  mustIncludeResult;
            return true;
        }
        return false;
    }
}
