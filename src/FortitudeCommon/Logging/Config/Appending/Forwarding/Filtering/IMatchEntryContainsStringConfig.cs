// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;

public enum MatchOnEntryField
{
    MessageBody
  , LoggerName
  , SourceFile
  , MemberName
}

public interface IMatchEntryContainsStringConfig : IMatchConditionConfig
{
    MatchOnEntryField MatchOn { get; }

    string Match { get; }
}
