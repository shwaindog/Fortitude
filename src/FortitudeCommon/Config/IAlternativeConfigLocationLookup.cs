using System.Text;
using FortitudeCommon.Extensions;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Config;

public interface IAlternativeConfigLocationLookup<out T> where T : IAlternativeConfigLocationLookup<T>
{
    string? ConfigLookupReferencePath { get; set; }

    bool HasFoundConfigLookup { get; }

    T? LookupValue { get; } 
}


public abstract class AlternativeConfigLocationLookup<T> : ConfigSection, IAlternativeConfigLocationLookup<T>
    where T : IAlternativeConfigLocationLookup<T>
{
    protected AlternativeConfigLocationLookup(IConfigurationRoot root, string path) : base(root, path) { }


    protected AlternativeConfigLocationLookup() : this(InMemoryConfigRoot, InMemoryPath) { }

    protected AlternativeConfigLocationLookup(string? configLookupReferencePath = null)
    {
        ConfigLookupReferencePath = configLookupReferencePath;
    }

    protected AlternativeConfigLocationLookup(IAlternativeConfigLocationLookup<T> toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        ConfigLookupReferencePath =  toClone.ConfigLookupReferencePath;
    }

    protected AlternativeConfigLocationLookup(IAlternativeConfigLocationLookup<T> toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string? ConfigLookupReferencePath
    {
        get => this[nameof(ConfigLookupReferencePath)];
        set => this[nameof(ConfigLookupReferencePath)] = value;
    }

    public bool HasFoundConfigLookup => ConfigLookupReferencePath.IsNotNullOrEmpty();

    public abstract T? LookupValue { get; }

    protected string AlternativeConfigLocationLookupToStringMembers => $"{nameof(ConfigLookupReferencePath)}: {ConfigLookupReferencePath}";
}