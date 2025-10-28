#region

using FortitudeCommon.DataStructures.MemoryPools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

#endregion

namespace FortitudeCommon.Config;

public abstract class ConfigSection(IConfigurationRoot root, string path) : ConfigurationSection(root, path)
{
    public const string InMemoryPath = "InMemory";

    protected readonly IConfigurationRoot ConfigRoot = root;

    private IRecycler? recycler;

    protected static string Split = ConfigurationPath.KeyDelimiter;
    
    public static string KeySeparator = ConfigurationPath.KeyDelimiter;

    protected ConfigSection() : this(InMemoryConfigRoot, InMemoryPath) { }

    protected static IConfigurationRoot InMemoryConfigRoot => new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build();

    protected IRecycler Recycler => recycler ??= new Recycler();
}
