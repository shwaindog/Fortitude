// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;

public enum FLoggerEntryMatchType
{
    EntryContainsString
  , MessageSequenceCompletes
  , MessageSequenceExpiresIncomplete
  , SequenceKeysComparison
}

public interface IMatchConditionConfig
{
    FLoggerEntryMatchType CheckConditionType { get; }
}
