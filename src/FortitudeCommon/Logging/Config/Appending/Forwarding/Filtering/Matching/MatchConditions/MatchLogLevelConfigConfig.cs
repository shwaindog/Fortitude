// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;

public interface IMatchLogLevelConfig : IMatchConditionConfig
{
    FLogLevel CheckLogLevel { get; }

    ComparisonOperatorType Is { get; }
}

public interface IMutableMatchLogLevelConfig : IMatchLogLevelConfig, IMutableMatchConditionConfig
{
    new FLogLevel CheckLogLevel { get; set; }

    new ComparisonOperatorType Is { get; set; }
}

public class MatchLogLevelConfigConfig : MatchConditionConfig, IMutableMatchLogLevelConfig
{
    public MatchLogLevelConfigConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MatchLogLevelConfigConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public MatchLogLevelConfigConfig
        (FLoggerEntryMatchType checkConditionType, FLogLevel logLevel, ComparisonOperatorType compareOperationIs)
        : this(InMemoryConfigRoot, InMemoryPath, checkConditionType, logLevel, compareOperationIs)
    { }

    public MatchLogLevelConfigConfig
    (IConfigurationRoot root, string path, FLoggerEntryMatchType checkConditionType
      , FLogLevel checkLogLevel, ComparisonOperatorType compareOperationIs)
        : base(root, path, checkConditionType)
    {
        CheckLogLevel = checkLogLevel;

        Is = compareOperationIs;
    }

    public MatchLogLevelConfigConfig(IMatchLogLevelConfig toClone, IConfigurationRoot root, string path) : base(toClone, root, path)
    {
        CheckLogLevel = toClone.CheckLogLevel;

        Is = toClone.Is;
    }

    public MatchLogLevelConfigConfig(IMatchLogLevelConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public FLogLevel CheckLogLevel
    {
        get =>
            Enum.TryParse<FLogLevel>(this[nameof(CheckLogLevel)], out var fullQueueHandling)
                ? fullQueueHandling
                : FLogLevel.Trace;
        set => this[nameof(CheckLogLevel)] = value.ToString();
    }

    public ComparisonOperatorType Is
    {
        get =>
            Enum.TryParse<ComparisonOperatorType>(this[nameof(Is)], out var fullQueueHandling)
                ? fullQueueHandling
                : ComparisonOperatorType.GreaterThanOrEqualTo;
        set => this[nameof(Is)] = value.ToString();
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public override MatchLogLevelConfigConfig Clone() => new(this);

    public override MatchLogLevelConfigConfig CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        new (this, configRoot, path);

    public override bool AreEquivalent(IMatchConditionConfig? other, bool exactTypes = false)
    {
        if (other is not IMatchLogLevelConfig logLevelConfig) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var logLevelSame = CheckLogLevel == logLevelConfig.CheckLogLevel;
        var operatorIsSame = Is == logLevelConfig.Is;

        var allAreSame = baseSame && logLevelSame && operatorIsSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMatchConditionConfig, true);

    public override int GetHashCode()
    {
        var hashCode = (int)CheckConditionType;
        return hashCode;
    }

    public override IStyledTypeStringAppender ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.AddTypeName(nameof(MatchEntryContainsStringConfig))
               .AddTypeStart()
               .AddField(nameof(CheckConditionType), CheckConditionType, FLoggerEntryMatchTypeExtensions.FLoggerEntryMatchTypeFormatter)
               .AddField(nameof(CheckLogLevel), CheckLogLevel, FLogLevelExtensions.FLogLevelFormatter)
               .AddField(nameof(Is), Is, ComparisonOperatorTypedExtensions.ComparisonOperatorTypeFormatter)
               .AddTypeEnd();
    }
}
