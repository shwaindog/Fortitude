// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;

public interface IForwardingAppendersLookupConfig : INamedAppendersLookupConfig, IStickyKeyValueDictionary<string, IAppenderForwardingReferenceConfig>
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<IAppenderForwardingReferenceConfig> Values { get; }

    new int Count { get; }

    new IAppenderForwardingReferenceConfig this[string key] { get; set; }

    new bool ContainsKey(string key);
    
    new IEnumerator<KeyValuePair<string, IAppenderForwardingReferenceConfig>> GetEnumerator();
}


public interface IAppendableForwardingAppendersLookupConfig : IForwardingAppendersLookupConfig, IAppendableDictionary<string, IMutableAppenderForwardingReferenceConfig>, IAppendableNamedAppendersLookupConfig
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<IMutableAppenderForwardingReferenceConfig> Values { get; }

    new int Count { get; }

    new IMutableAppenderForwardingReferenceConfig this[string key] { get; set; }

    new bool ContainsKey(string key);
    
    new IEnumerator<KeyValuePair<string, IMutableAppenderForwardingReferenceConfig>> GetEnumerator();
}