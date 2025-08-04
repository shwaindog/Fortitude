// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Config;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;

public interface IMatchConditionConfig : IFLogConfig, IConfigCloneTo<IMatchConditionConfig>
  , IInterfacesComparable<IMatchConditionConfig>, IStyledToStringObject
{
    FLoggerEntryMatchType CheckConditionType { get; set; }
}

public interface IMutableMatchConditionConfig : IMutableFLogConfig, IMatchConditionConfig
{
    new FLoggerEntryMatchType CheckConditionType { get; set; }
}

public abstract class MatchConditionConfig : FLogConfig, IMutableMatchConditionConfig
{
    protected MatchConditionConfig(IConfigurationRoot root, string path) : base(root, path) { }

    protected MatchConditionConfig() : this(InMemoryConfigRoot, InMemoryPath) { }

    protected MatchConditionConfig(FLoggerEntryMatchType checkConditionType)
        : this(InMemoryConfigRoot, InMemoryPath, checkConditionType) { }

    protected MatchConditionConfig
        (IConfigurationRoot root, string path, FLoggerEntryMatchType checkConditionType)
        : base(root, path)
    {
        CheckConditionType = checkConditionType;
    }

    protected MatchConditionConfig(IMatchConditionConfig toClone, IConfigurationRoot root, string path) : base(root, path)
    {
        CheckConditionType = toClone.CheckConditionType;
    }

    protected MatchConditionConfig(IMatchConditionConfig toClone) : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public FLoggerEntryMatchType CheckConditionType
    {
        get =>
            Enum.TryParse<FLoggerEntryMatchType>(this[nameof(CheckConditionType)], out var fullQueueHandling)
                ? fullQueueHandling
                : FLoggerEntryMatchType.Unknown;
        set => this[nameof(CheckConditionType)] = value.ToString();
    }

    object ICloneable.Clone() => Clone();

    IMatchConditionConfig ICloneable<IMatchConditionConfig>.Clone() => Clone();

    public abstract MatchConditionConfig Clone();

    public abstract IMatchConditionConfig CloneConfigTo(IConfigurationRoot configRoot, string path);

    public virtual bool AreEquivalent(IMatchConditionConfig? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var conditionTypeSame = CheckConditionType == other.CheckConditionType;

        var allAreSame = conditionTypeSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMatchConditionConfig, true);

    public override int GetHashCode()
    {
        var hashCode = (int)CheckConditionType;
        return hashCode;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartComplexType(nameof(MatchConditionConfig))
               .Field.AddAlways(nameof(CheckConditionType), CheckConditionType, FLoggerEntryMatchTypeExtensions.FLoggerEntryMatchTypeFormatter)
               .Complete();
    }
}
