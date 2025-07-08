// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;

public interface IExtractedMessageKeyValues : IStickyKeyValueDictionary<string, IExtractKeyExpressionConfig> { }

public interface IExtractKeyExpressionConfig
{
    string Key                { get; }
    string ExtractRegEx       { get; }
    int    ExtractGroupNumber { get; }
}
