// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config.LoggersHierarchy;

public interface INamedChildLoggersLookupConfig : IStickyKeyValueDictionary<string, IFLoggerDescendantConfig>
{
}

public interface IMutableNamedChildLoggersLookupConfig : INamedChildLoggersLookupConfig, IAppendableDictionary<string, IMutableFLoggerDescendantConfig> 
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<IMutableFLoggerDescendantConfig> Values { get; }

    new int Count { get; }

    new IMutableFLoggerDescendantConfig this[string key] { get; set; }

    new bool ContainsKey(string key);
    
    new IEnumerator<KeyValuePair<string, IMutableFLoggerDescendantConfig>> GetEnumerator();
}