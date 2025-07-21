// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Types;
using Microsoft.Extensions.Configuration;
using static FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.FLoggerEntryMatchType;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;

public enum FLoggerEntryMatchType
{
    Unknown
  , EntryContainsString
  , LogLevelComparison
  , MessageSequenceCompletes
  , MessageSequenceExpiresIncomplete
  , SequenceKeysComparison
}

public static class FLoggerEntryMatchTypeExtensions
{
    public static Action<FLoggerEntryMatchType, IStyledTypeStringAppender> FLoggerEntryMatchTypeFormatter
        = FormatEntryMatchTypeAppender;

    public static void FormatEntryMatchTypeAppender(this FLoggerEntryMatchType entryMatchType, IStyledTypeStringAppender sbc)
    {
        var sb = sbc.BackingStringBuilder;

        switch (entryMatchType)
        {
            case Unknown: sb.Append($"{nameof(Unknown)}"); break;

            case EntryContainsString:      sb.Append($"{nameof(EntryContainsString)}"); break;
            case MessageSequenceCompletes: sb.Append($"{nameof(MessageSequenceCompletes)}"); break;
            case SequenceKeysComparison:   sb.Append($"{nameof(SequenceKeysComparison)}"); break;

            case MessageSequenceExpiresIncomplete: sb.Append($"{nameof(MessageSequenceExpiresIncomplete)}"); break;

            default: sb.Append($"{nameof(Unknown)}"); break;
        }
    }

    public static string? GetCheckConditionType(this IConfigurationRoot configRoot, string configPath) =>
        configRoot.GetSection($"{configPath}{ConfigurationPath.KeyDelimiter}{nameof(IMatchConditionConfig.CheckConditionType)}").Value;

    public static IMutableMatchConditionConfig? GetMatchConditionConfig(this IConfigurationRoot configRoot, string configPath, IFLogConfig? parent = null)
    {
        var checkType = GetCheckConditionType(configRoot, configPath);
        switch (checkType)
        {
            case nameof(Unknown):             throw new ConfigurationErrorsException($"Expected {nameof(IMatchConditionConfig.CheckConditionType)}");
            case nameof(EntryContainsString): return new MatchEntryContainsStringConfig(configRoot, configPath)
            {
                ParentConfig = parent
            };
            case nameof(LogLevelComparison):  return new MatchLogLevelConfigConfig(configRoot, configPath)
            {
                ParentConfig = parent
            };
            default:                          return null;
        }
    }

    public static IMutableMatchConditionConfig? GetMatchConditionConfigNoParent(this IConfigurationRoot configRoot, string configPath)
    {
        var checkType = GetCheckConditionType(configRoot, configPath);
        switch (checkType)
        {
            case nameof(Unknown):             throw new ConfigurationErrorsException($"Expected {nameof(IMatchConditionConfig.CheckConditionType)}");
            case nameof(EntryContainsString): return new MatchEntryContainsStringConfig(configRoot, configPath);
            case nameof(LogLevelComparison):  return new MatchLogLevelConfigConfig(configRoot, configPath);
            default:                          return null;
        }
    }
}
