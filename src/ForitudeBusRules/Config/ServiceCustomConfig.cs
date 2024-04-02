#region

using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeBusRules.Config;

public enum CustomConfigType
{
    ConfigSectionPath
    , ExternalFilePath
    , Json
    , StringContent
}

public interface IServiceCustomConfig
{
    IConfigurationRoot ConfigSectionConfigurationRoot { get; }
    CustomConfigType CustomConfigType { get; set; }
    string Content { get; set; }
}

public class ServiceCustomConfig : ConfigurationSection, IServiceCustomConfig
{
    public ServiceCustomConfig(IConfigurationRoot configRoot, string path) : base(configRoot, path) => ConfigSectionConfigurationRoot = configRoot;

    public ServiceCustomConfig(IServiceCustomConfig toClone, IConfigurationRoot configRoot, string path) : this(configRoot, path)
    {
        CustomConfigType = toClone.CustomConfigType;
        Content = toClone.Content;
    }

    public IConfigurationRoot ConfigSectionConfigurationRoot { get; }

    public CustomConfigType CustomConfigType
    {
        get => Enum.Parse<CustomConfigType>(this[nameof(CustomConfigType)]!);
        set => this[nameof(CustomConfigType)] = value.ToString();
    }

    public string Content
    {
        get => this[nameof(Content)]!;
        set => this[nameof(Content)] = value;
    }
}
