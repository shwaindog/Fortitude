// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;

public interface IMatchLogLevelConfig : IMatchConditionConfig
{
    FLogLevel CheckLogLevel { get; }

    ComparisonOperatorType Is { get; }
}
