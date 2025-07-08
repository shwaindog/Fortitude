// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config.Appending;


public interface IFLoggerMatchedAppenders
{
    INamedAppendersLookupConfig Appenders { get; }
}

public interface IMutableFLoggerMatchedAppenders : IFLoggerMatchedAppenders
{
    new IAppendableNamedAppendersLookupConfig Appenders { get; set; }
}