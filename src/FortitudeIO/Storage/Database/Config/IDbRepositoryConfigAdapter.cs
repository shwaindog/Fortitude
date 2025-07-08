// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Extensions;
using Microsoft.Extensions.Configuration;
using static FortitudeIO.Storage.Database.Config.DbRepositoryConfigRetrieveStatus;

namespace FortitudeIO.Storage.Database.Config;

public interface IDbRepositoryConfigAdapter<TConfig>
    where TConfig : class, IConfigCombine<TConfig>
{
    DbRepositoryConfigRetrieveStatus RetrieveConfig(IDbConfigSourceConnectionConfig<TConfig> configDbConnection, out TConfig? result);
}

public interface IDbEmbeddedFallbackConfig<TConfig> : IDbRepositoryConfigAdapter<TConfig>
    where TConfig : class, IConfigCombine<TConfig>
{
    bool DisableEmbeddedConfig { get; set; }

    TConfig? EmbeddedDbConfigFallback { get; set; }
}

public class DbEmbeddedFallbackConfig<TConfig> : ConfigSection, IDbEmbeddedFallbackConfig<TConfig>, IAlternativeConfigLocationLookup<DbEmbeddedFallbackConfig<TConfig>>
    where TConfig : class, IConfigCombine<TConfig>
{
    public DbEmbeddedFallbackConfig<TConfig>? LookupEmbeddedConfig;

    public DbEmbeddedFallbackConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public DbEmbeddedFallbackConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public DbEmbeddedFallbackConfig(TConfig? embeddedDbConfigFallback = null, bool disableEmbeddedConfig = false,  string? configLookupReferencePath = null)
    {
        DisableEmbeddedConfig = disableEmbeddedConfig;
        EmbeddedDbConfigFallback = embeddedDbConfigFallback;

        ConfigLookupReferencePath = configLookupReferencePath;
    }

    public DbEmbeddedFallbackConfig(IDbEmbeddedFallbackConfig<TConfig> toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        DisableEmbeddedConfig      = toClone.DisableEmbeddedConfig;
        EmbeddedDbConfigFallback = toClone.EmbeddedDbConfigFallback?.Clone();
    }

    public DbEmbeddedFallbackConfig(IDbEmbeddedFallbackConfig<TConfig> toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string? ConfigLookupReferencePath
    {
        get => this[nameof(ConfigLookupReferencePath)];
        set => this[nameof(ConfigLookupReferencePath)] = value;
    }

    public bool HasFoundConfigLookup => ConfigLookupReferencePath.IsNotNullOrEmpty();
    
    public virtual DbEmbeddedFallbackConfig<TConfig>? LookupValue
    {
        get
        {
            if (HasFoundConfigLookup && LookupEmbeddedConfig == null)
            {
                LookupEmbeddedConfig = new DbEmbeddedFallbackConfig<TConfig>(ConfigRoot, ConfigLookupReferencePath!);
            }
            return LookupEmbeddedConfig;
        }
    }

    public bool DisableEmbeddedConfig
    {
        get => bool.TryParse(this[nameof(DisableEmbeddedConfig)], out var useEmbeddedConfig) ? useEmbeddedConfig : (LookupEmbeddedConfig?.DisableEmbeddedConfig ?? false);
        set => this[nameof(DisableEmbeddedConfig)] = value.ToString();
    }
    
    public TConfig? EmbeddedDbConfigFallback
    {
        get
        {
            if (GetSection(nameof(EmbeddedDbConfigFallback)).GetChildren().Any(cs => cs.Value.IsNotNullOrEmpty()))
            {
                var check = (TConfig)Activator.CreateInstance(typeof(TConfig), ConfigRoot, $"{Path}{Split}{nameof(EmbeddedDbConfigFallback)}")!;
                return check;
            }
            return LookupEmbeddedConfig?.EmbeddedDbConfigFallback;
        }
        set
        {
            if (value != null)
            {
                _ = (TConfig)Activator.CreateInstance(typeof(TConfig), value, ConfigRoot, $"{Path}{Split}{nameof(EmbeddedDbConfigFallback)}")!;
            }
        }
    }

    public DbRepositoryConfigRetrieveStatus RetrieveConfig(IDbConfigSourceConnectionConfig<TConfig> configDbConnection, out TConfig? result)
    {
        if (!DisableEmbeddedConfig && EmbeddedDbConfigFallback is {} embeddedConfigFallback)
        {
            result = embeddedConfigFallback;
            return HasConfigResults | UsedEmbeddedConfigResults;
        }
        result = null;
        return DbUnavailable;
    }

    protected string DbEmbeddedFallbackConfigToStringMembers => $"{nameof(ConfigLookupReferencePath)}: {ConfigLookupReferencePath}";
}