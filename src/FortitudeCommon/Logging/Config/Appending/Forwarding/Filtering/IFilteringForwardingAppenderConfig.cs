// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;

public interface IFilteringForwardingAppenderConfig : IForwardingAppenderConfig
{
    IMatchOperatorExpressionConfig? When { get; }
}
