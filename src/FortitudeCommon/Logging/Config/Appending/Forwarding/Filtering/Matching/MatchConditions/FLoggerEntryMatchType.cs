// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Configuration;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
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
    public static CustomTypeStyler<FLoggerEntryMatchType> FLoggerEntryMatchTypeFormatter
        = FormatEntryMatchTypeAppender;

    public static StyledTypeBuildResult FormatEntryMatchTypeAppender(this FLoggerEntryMatchType entryMatchType, IStyledTypeStringAppender sbc)
    {
        var tb = sbc.StartSimpleValueType(nameof(FLoggerEntryMatchType));
        using (var sb = tb.StartDelimitedStringBuilder())
        {
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

        return tb.Complete();
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
