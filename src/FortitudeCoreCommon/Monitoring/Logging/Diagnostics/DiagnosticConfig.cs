#region

using Microsoft.Extensions.Configuration;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics;

public delegate void ConfigKeyEventHandler(object sender, ConfigKeyEventArgs e);

public class ConfigKeyEventArgs : EventArgs
{
    public ConfigKeyEventArgs(string keyName, string keyValue)
    {
        KeyName = keyName;
        KeyValue = keyValue;
    }

    public string KeyName { get; }

    public string KeyValue { get; }
}

internal class DiagnosticConfig
{
    private readonly string diagnosticName;
    private IConfigurationSection? configurationSection;

    public DiagnosticConfig(string diagnosticName,
        ConfigurationSection? configurationSection = null)
    {
        this.diagnosticName = diagnosticName;
        this.configurationSection = configurationSection;
    }

    public IConfigurationSection? ConfigurationSection
    {
        get => configurationSection;
        set
        {
            var changes = CompareConfigChanges(configurationSection, value).ToList();
            configurationSection = value;
            foreach (var configChange in changes)
                HierarchicalConfigurationUpdater.Instance.DiagnosticsUpdate(configChange.diagnosticName,
                    configChange.keyValue, configChange.changeType);
        }
    }

    public IEnumerable<ConfigChange> CompareConfigChanges(IConfigurationSection? original,
        IConfigurationSection? change)
    {
        if (original != null && change == null)
            yield return new ConfigChange(diagnosticName,
                new KeyValuePair<string, string?>(original.Key, original.Value), UpdateType.Removed);
        else if (original == null && change != null)
            yield return new ConfigChange(diagnosticName,
                new KeyValuePair<string, string?>(change.Key, change.Value), UpdateType.Added);
        else if (original != null && change != null && original.Value != change.Value)
            yield return new ConfigChange(diagnosticName,
                new KeyValuePair<string, string?>(change.Key, change.Value), UpdateType.Updated);

        if (original != null && change != null)
        {
            var origChildren = original.GetChildren().ToList();
            foreach (var origChild in origChildren)
            foreach (var subKeyChange in CompareConfigChanges(origChild, change.GetSection(origChild.Key)))
                yield return subKeyChange;

            var origKeys = origChildren.Select(cs => cs.Key).ToList();
            var rhsAdditionalKeys = change.GetChildren()
                .Select(cs => cs.Key)
                .Where(k => !origKeys.Contains(k));
            foreach (var changeAdded in rhsAdditionalKeys)
            foreach (var subKeyChange in CompareConfigChanges(null, change.GetSection(changeAdded)))
                yield return subKeyChange;
        }
    }

    public bool SameConfiguration(IConfigurationSection? other) => SameConfiguration(configurationSection, other);

    private static bool SameConfiguration(IConfigurationSection? lhs, IConfigurationSection? rhs)
    {
        if ((lhs == null && rhs == null) || (lhs == null && rhs != null) || (lhs != null && rhs == null)) return false;

        var lhsChildren = lhs!.GetChildren().ToList();
        foreach (var lhsChild in lhsChildren)
            if (!SameConfiguration(lhsChild, rhs!.GetSection(lhsChild.Key)))
                return false;
        var rhsChildren = rhs!.GetChildren().ToList();

        return lhsChildren.Count() == rhsChildren.Count() && lhs.Value == rhs.Value;
    }

    public record ConfigChange(string diagnosticName, KeyValuePair<string, string?> keyValue, UpdateType changeType);
}
