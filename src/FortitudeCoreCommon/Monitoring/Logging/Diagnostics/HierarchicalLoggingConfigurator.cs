#region

using FortitudeCommon.Monitoring.Logging.Diagnostics.CallStats;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Latency;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Tracing;

#endregion

namespace FortitudeCommon.Monitoring.Logging.Diagnostics;

public class HierarchicalLoggingConfigurator<T> where T : class, IHierarchicalLogger
{
    private static readonly SortedList<string, List<T>> trackingItems;

    static HierarchicalLoggingConfigurator()
    {
        trackingItems ??= new SortedList<string, List<T>>(new LengthLexComparer());
        loggingActivations ??= new SortedList<string, string>(new LengthLexComparer());
    }

    public static IEnumerable<string> ActivationKeywords =>
        cachedActivationWords ??=
            CallStatsLogger.ActivationKeywords.Concat(LatencyLogger.ActivationKeywords)
                .Concat(TraceLogger.ActivationKeywords).Distinct().ToArray();

    public static void Register(T trackThis)
    {
        var lowercaseFullName = trackThis.FullNameOfLogger.ToLower();
        lock (TrackingSyncLock)
        {
            if (trackingItems.TryGetValue(lowercaseFullName, out var collectionOfT))
            {
                collectionOfT.Add(trackThis);
            }
            else
            {
                collectionOfT = new List<T>(12) { trackThis };
                trackingItems[lowercaseFullName] = collectionOfT;
            }
        }

        ProcessRulesForLogger(trackThis);
    }

    public static bool Unregister(T trackThis)
    {
        lock (TrackingSyncLock)
        {
            if (trackingItems.TryGetValue(trackThis.FullNameOfLogger.ToLower(), out var collectionOfT))
                return collectionOfT.Remove(trackThis);
        }

        return false;
    }

    public static void AddRule(string ruleHierarchy, string value)
    {
        ruleHierarchy = ruleHierarchy.ToLower();
        lock (ConfigSyncLock)
        {
            loggingActivations[ruleHierarchy] = value.ToLower();
        }

        Logger.Info("Activating {0} for full logger path {1} with value {2}", typeof(T).Name, ruleHierarchy, value);
        List<T> itemsMatchingRuleName;
        lock (TrackingSyncLock)
        {
            itemsMatchingRuleName = trackingItems.Keys.Where(key => key.StartsWith(ruleHierarchy))
                .SelectMany(trackingItemKey => trackingItems[trackingItemKey])
                .ToList();
        }

        foreach (var item in itemsMatchingRuleName) ProcessRulesForLogger(item);
    }

    public static void RemoveRule(string ruleHierarchy)
    {
        if (ruleHierarchy == null) return;
        ruleHierarchy = ruleHierarchy.ToLower();
        lock (ConfigSyncLock)
        {
            loggingActivations.Remove(ruleHierarchy);
        }

        Logger.Info("Deactivating {0} for full logger path {1}", typeof(T).Name, ruleHierarchy);
        List<T> itemsMatchingRuleName;
        lock (TrackingSyncLock)
        {
            itemsMatchingRuleName = trackingItems.Keys.Where(key => key.StartsWith(ruleHierarchy))
                .SelectMany(trackingItemKey => trackingItems[trackingItemKey])
                .ToList();
        }

        foreach (var item in itemsMatchingRuleName) ProcessRulesForLogger(item);
    }

    private static void ProcessRulesForLogger(T logger)
    {
        string? currentApplied = null;
        var lowerCaseLoggerFullName = logger.FullNameOfLogger.ToLower();

        lock (ConfigSyncLock)
        {
            foreach (var rule in loggingActivations)
            {
                if (lowerCaseLoggerFullName.StartsWith(rule.Key)) currentApplied = rule.Value;
                if (lowerCaseLoggerFullName.Length < rule.Key.Length) break;
            }
        }

        currentApplied = currentApplied ?? logger.DefaultStringValue;
        logger.SettingTranslation(logger, currentApplied);
    }

    public static bool AggregateLoggerShouldActivate(string typeNameSpace)
    {
        string? setting = null;

        lock (ConfigSyncLock)
        {
            if (loggingActivations.ContainsKey(typeNameSpace)) setting = loggingActivations[typeNameSpace];
        }

        if (setting == null) return false;
        var settings = setting.Split('-');
        if (settings.Any(aSettingValue => ActivationKeywords.Contains(aSettingValue.ToLower()))) return true;
        if (int.TryParse(setting, out var btchSize)) return btchSize > 1;
        return false;
    }

    private class LengthLexComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            var diff = x.Length - y.Length;
            if (diff != 0) return diff;
            return string.Compare(x, y, StringComparison.InvariantCulture);
        }
    }

    // ReSharper disable StaticFieldInGenericType
    private static readonly SortedList<string, string> loggingActivations;
    private static readonly object TrackingSyncLock = new();
    private static readonly object ConfigSyncLock = new();
    private static IEnumerable<string>? cachedActivationWords;

    private static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger("DiagnosticSettings");
    // ReSharper restore StaticFieldInGenericType
}
