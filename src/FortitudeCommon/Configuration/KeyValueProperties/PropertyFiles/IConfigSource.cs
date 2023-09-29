using System;

namespace FortitudeCommon.Configuration.KeyValueProperties.PropertyFiles
{
    public interface IConfigSource
    {
        ConfigCollection Configs { get; }
        bool AutoSave { get; set; }
        AliasText Alias { get; }
        void Merge(IConfigSource source);
        void Save();
        void Reload();
        IConfig AddConfig(string name);
        string GetExpanded(IConfig config, string key);
        void ExpandKeyValues();
        void ReplaceKeyValues();
        event EventHandler Reloaded;
        event EventHandler Saved;
    }
}