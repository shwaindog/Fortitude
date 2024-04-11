#region

using FortitudeCommon.DataStructures.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;

#endregion

namespace FortitudeCommon.Configuration;

public abstract class ConfigSection : ConfigurationSection
{
    public const string InMemoryPath = "InMemory";
    protected readonly IConfigurationRoot ConfigRoot;
    private IRecycler? recycler;

    protected ConfigSection(IConfigurationRoot root, string path) : base(root, path) => ConfigRoot = root;

    protected ConfigSection() : this(InMemoryConfigRoot, InMemoryPath) { }

    protected static IConfigurationRoot InMemoryConfigRoot => new ConfigurationBuilder().Add(new MemoryConfigurationSource()).Build();

    protected IRecycler Recycler => recycler ??= new Recycler();
}
