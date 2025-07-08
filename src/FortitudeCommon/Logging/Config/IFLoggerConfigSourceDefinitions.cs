// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config;

public interface IFLoggerConfigSourceDefinitions : IStickyKeyValueDictionary<uint, IFloggerConfigSource> { }


public interface IMutableFLoggerConfigSourceDefinitions : IFLoggerConfigSourceDefinitions, IAppendableDictionary<uint, IFloggerConfigSource>
{
}
