#region

using System.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

#endregion

namespace FortitudeCommon.Configuration;

public class PocoConfigurationSection : IConfigurationSection, IEnumerable<KeyValuePair<string, string?>>
{
    private readonly Dictionary<string, IConfigurationSection> subSections = new();

    public PocoConfigurationSection(string key, string path, string? value)
    {
        Key = key;
        Path = path;
        Value = value;
    }

    public IEnumerable<IConfigurationSection> GetChildren() => subSections.Values;

    public IChangeToken GetReloadToken() => throw new NotImplementedException();

    public IConfigurationSection GetSection(string key) => subSections[key];

    public string? this[string key]
    {
        get => subSections[key]?.Value;
        set
        {
            if (subSections.ContainsKey(key))
                GetSection(key).Value = value;
            else
                subSections.Add(key, new PocoConfigurationSection(key, $"{Path}.{key}", value));
        }
    }

    public string Key { get; set; }
    public string Path { get; set; }
    public string? Value { get; set; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<string, string?>> GetEnumerator() =>
        subSections.Select(kvp =>
            new KeyValuePair<string, string?>(kvp.Key, kvp.Value.Value)).GetEnumerator();

    public void Add(string key, string value)
    {
        this[key] = value;
    }
}
