// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using Microsoft.Extensions.Configuration;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering.Matching.MatchConditions;

public interface IMatchEntryContainsStringConfig : IMatchConditionConfig
{
    LogEntryField MatchOn { get; }

    bool IsRegEx { get; }

    string EntryContains { get; }
}

public interface IMutableMatchEntryContainsStringConfig : IMatchEntryContainsStringConfig, IMutableMatchConditionConfig
{
    new LogEntryField MatchOn { get; set; }

    new bool IsRegEx { get; set; }

    new string EntryContains { get; set; }
}

public class MatchEntryContainsStringConfig : MatchConditionConfig, IMutableMatchEntryContainsStringConfig
{
    public MatchEntryContainsStringConfig(IConfigurationRoot root, string path) 
        : base(root, path) { }

    public MatchEntryContainsStringConfig() 
        : this(InMemoryConfigRoot, InMemoryPath) { }

    public MatchEntryContainsStringConfig
    (string entryContains, LogEntryField matchOn = LogEntryField.MessageBody, bool isRegEx = false)
        : this(InMemoryConfigRoot, InMemoryPath, entryContains, matchOn, isRegEx)
    {
        EntryContains = entryContains;

        MatchOn = matchOn;
        IsRegEx = isRegEx;
    }

    public MatchEntryContainsStringConfig
    (IConfigurationRoot root, string path, string entryContains, LogEntryField matchOn = LogEntryField.MessageBody, bool isRegEx = false)
        : base(root, path, FLoggerEntryMatchType.LogLevelComparison)
    {
        EntryContains = entryContains;

        MatchOn = matchOn;
        IsRegEx = isRegEx;
    }

    public MatchEntryContainsStringConfig(IMatchEntryContainsStringConfig toClone, IConfigurationRoot root, string path) 
        : base(toClone, root, path)
    {
        EntryContains = toClone.EntryContains;

        MatchOn = toClone.MatchOn;
        IsRegEx = toClone.IsRegEx;
    }

    public MatchEntryContainsStringConfig(IMatchEntryContainsStringConfig toClone) 
        : this(toClone, InMemoryConfigRoot, InMemoryPath) { }

    public string EntryContains
    {
        get => this[nameof(EntryContains)] ?? "__No_EntryContains_Provided__";
        set => this[nameof(EntryContains)] = value;
    }

    public bool IsRegEx
    {
        get => bool.TryParse(this[nameof(IsRegEx)], out var disabled) && disabled;
        set => this[nameof(IsRegEx)] = value.ToString();
    }

    public LogEntryField MatchOn
    {
        get =>
            Enum.TryParse<LogEntryField>(this[nameof(MatchOn)], out var fullQueueHandling)
                ? fullQueueHandling
                : LogEntryField.MessageBody;
        set => this[nameof(MatchOn)] = value.ToString();
    }

    public override T Visit<T>(T visitor) => visitor.Accept(this);

    public override MatchEntryContainsStringConfig Clone() => new(this);

    public override MatchEntryContainsStringConfig CloneConfigTo(IConfigurationRoot configRoot, string path) =>
        new (this, configRoot, path);

    public override bool AreEquivalent(IMatchConditionConfig? other, bool exactTypes = false)
    {
        if (other is not IMatchEntryContainsStringConfig containsString) return false;

        var baseSame = base.AreEquivalent(other, exactTypes);

        var entryContainsSame = EntryContains == containsString.EntryContains;
        var isRegExSame       = IsRegEx == containsString.IsRegEx;
        var matchOnSame       = MatchOn == containsString.MatchOn;

        var allAreSame = baseSame && entryContainsSame && isRegExSame && matchOnSame;

        return allAreSame;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as IMatchConditionConfig, true);

    public override int GetHashCode()
    {
        var hashCode = (int)CheckConditionType;
        return hashCode;
    }

    public override StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartComplexType(nameof(MatchEntryContainsStringConfig))
               .Field.AlwaysAdd(nameof(CheckConditionType), CheckConditionType, FLoggerEntryMatchTypeExtensions.FLoggerEntryMatchTypeFormatter)
               .Field.AlwaysAdd(nameof(MatchOn), MatchOn, LogEntryFieldExtensions.FormatMatchOnLogEntryFieldFormatter)
               .Field.AlwaysAdd(nameof(EntryContains), EntryContains)
               .Field.WhenNonDefaultAdd(nameof(IsRegEx), IsRegEx)
               .Complete();
    }
}
