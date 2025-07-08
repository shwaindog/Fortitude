// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config.Appending;

public interface INamedAppendersLookupConfig : IStickyKeyValueDictionary<string, IAppenderReferenceConfig> { }

public interface IAppendableNamedAppendersLookupConfig : INamedAppendersLookupConfig, IAppendableDictionary<string, IMutableAppenderReferenceConfig>
{
    new IEnumerable<string> Keys { get; }

    new IEnumerable<IMutableAppenderReferenceConfig> Values { get; }

    new int Count { get; }

    new IMutableAppenderReferenceConfig this[string key] { get; set; }

    new bool ContainsKey(string key);
    
    new IEnumerator<KeyValuePair<string, IMutableAppenderReferenceConfig>> GetEnumerator();
}