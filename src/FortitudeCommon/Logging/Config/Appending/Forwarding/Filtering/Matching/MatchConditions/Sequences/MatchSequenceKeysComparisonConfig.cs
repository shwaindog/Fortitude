// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions.Sequences;

public interface IMatchSequenceKeysComparisonConfig : IMatchConditionConfig
{
    string Lhs { get; }

    ComparisonOperatorType Is { get; }

    string Rhs { get; }
}

public interface IMutableMatchSequenceKeysComparisonConfig : IMatchSequenceKeysComparisonConfig, IMutableMatchConditionConfig
{
    new string Lhs { get; set; }

    new ComparisonOperatorType Is { get; set; }

    new string Rhs { get; set; }
}

public class MatchSequenceKeysComparisonConfig : MatchConditionConfig, IMutableMatchSequenceKeysComparisonConfig
{
    public MatchSequenceKeysComparisonConfig(IConfigurationRoot root, string path) : base(root, path) { }

    public MatchSequenceKeysComparisonConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    public MatchSequenceKeysComparisonConfig
    (string lhs
      , ComparisonOperatorType comparisonOperatorIs = ComparisonOperatorType.Equals, string rhs = "")
        : this(InMemoryConfigRoot, InMemoryPath, lhs, comparisonOperatorIs, rhs) { }

    public MatchSequenceKeysComparisonConfig
    (IConfigurationRoot root, string path, string lhs, ComparisonOperatorType comparisonOperatorIs = ComparisonOperatorType.Equals
      , string rhs = "")
        : base(root, path, FLoggerEntryMatchType.SequenceKeysComparison)
    {
        Lhs = lhs;
        Rhs = rhs;

        Is = comparisonOperatorIs;
    }

    public MatchSequenceKeysComparisonConfig(IMatchSequenceKeysComparisonConfig toClone, IConfigurationRoot root, string path) :
        base(toClone, root, path)
    {
        Lhs = toClone.Lhs;
        Rhs = toClone.Rhs;

        Is = toClone.Is;
    }

    public MatchSequenceKeysComparisonConfig(IMatchSequenceKeysComparisonConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public ComparisonOperatorType Is
    {
        get =>
            Enum.TryParse<ComparisonOperatorType>(this[nameof(Is)], out var fullQueueHandling)
                ? fullQueueHandling
                : ComparisonOperatorType.Equals;
        set => this[nameof(Is)] = value.ToString();
    }

    public string Lhs
    {
        get => this[nameof(Lhs)] ?? "";
        set => this[nameof(Lhs)] = value;
    }

    public string Rhs
    {
        get => this[nameof(Rhs)] ?? "";
        set => this[nameof(Rhs)] = value;
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public override MatchSequenceKeysComparisonConfig CloneConfigTo(IConfigurationRoot configRoot, string path) => new(this, configRoot, path);

    public override MatchSequenceKeysComparisonConfig Clone() => new(this);

    public override bool AreEquivalent(IMatchConditionConfig? other, bool exactTypes = false)
    {
        if (other is not IMatchSequenceKeysComparisonConfig matchSeqComparison) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var lhsSame = Lhs == matchSeqComparison.Lhs;
        var isSame  = Is == matchSeqComparison.Is;
        var rhsSame = Rhs == matchSeqComparison.Rhs;

        var allAreSame = baseSame && lhsSame && isSame && rhsSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMatchConditionConfig, true);

    public override int GetHashCode()
    {
        var hashCode = (int)CheckConditionType;
        return hashCode;
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc) =>
        sbc.StartComplexType(nameof(MatchEntryContainsStringConfig))
           .Field.AlwaysAdd(nameof(CheckConditionType), CheckConditionType)
           .Field.AlwaysAdd(nameof(Lhs), Lhs)
           .Field.AlwaysAdd(nameof(Is), Is)
           .Field.AlwaysAdd(nameof(Rhs), Rhs)
           .Complete();
}
