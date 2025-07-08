// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding;


public interface IForwardingAppenderConfig : IAppenderDefinitionConfig
{
    IForwardingAppendersLookupConfig ForwardTo { get; }
}