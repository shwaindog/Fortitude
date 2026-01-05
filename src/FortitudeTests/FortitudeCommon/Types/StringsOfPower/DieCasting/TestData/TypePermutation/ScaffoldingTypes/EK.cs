// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

// Expect Key shortened to reduce obscuring declarative expect definition
// ReSharper disable once InconsistentNaming
public class EK 
(
    ScaffoldingStringBuilderInvokeFlags matchScaff
  , StringStyle matchStyle = StringStyle.Compact | StringStyle.Json | StringStyle.Log | StringStyle.Pretty)
    : IEquatable<EK>, IOffsetAwareListItem
{
    private readonly ScaffoldingStringBuilderInvokeFlags matchScaff = matchScaff;
    private readonly StringStyle                         matchStyle = matchStyle;

    public ScaffoldingStringBuilderInvokeFlags MatchScaff
    {
        get => matchScaff;
        init => matchScaff = value;
    }

    public StringStyle MatchStyle
    {
        get => matchStyle;
        init => matchStyle = value;
    }

    public bool IsMatchingScenario(ScaffoldingStringBuilderInvokeFlags condition, StringStyle style)
    {
        var styleIsMatched        = (style & MatchStyle) == style;
        var bothCallViaObjectOrNeither = true;
        if (MatchScaff.HasCallsUsingObject())
        {
            bothCallViaObjectOrNeither = condition.HasCallsUsingObject();
        }
        else if (MatchScaff.HasNeverWhenCallingViaObject() && condition.HasCallsUsingObject())
        {
            bothCallViaObjectOrNeither = false;
        }
        var bothCallViaMatchOrNeither = true;
        if (MatchScaff.HasCallsViaMatch()) { bothCallViaMatchOrNeither = condition.HasCallsViaMatch(); }
        
        var meetsMoldTypeCondition = (MatchScaff & MoldTypeConditionMask) == 0 
                                  // || (style.IsLog() 
                                  //        ? ((MatchScaff & MoldTypeConditionMask) == (MoldTypeConditionMask & condition))
                                  //        : ((MatchScaff & MoldTypeConditionMask & condition) > 0));
                                  ||  ((MatchScaff & MoldTypeConditionMask & condition) > 0);
        
        var meetsWriteCondition    = 
            (MatchScaff & AllOutputConditionsMask & condition) > 0 
         || condition.HasSimpleTypeFlag() || condition.HasOrderedCollectionTypeFlag() || condition.HasKeyedCollectionTypeFlag();
        var conditionHasBothBecomesNullAndFallback = (condition & OutputBecomesMask) == (DefaultBecomesNull | DefaultBecomesFallbackValue)
                                                  && (condition & SupportsSettingDefaultValue) > 0;
        var scaffHasBothBecomesNullAndFallback = (MatchScaff & OutputBecomesMask) == (DefaultBecomesNull | DefaultBecomesFallbackValue);
        var meetsOutputType =
            ((MatchScaff.HasNoneOf(OutputTreatedMask)
           || condition.HasNoneOf(OutputTreatedMask)
           || (MatchScaff & condition).HasAnyOf(OutputTreatedMask))
           &&
             ((!scaffHasBothBecomesNullAndFallback || conditionHasBothBecomesNullAndFallback)
           && (MatchScaff.HasNoneOf(OutputBecomesMask)
            || condition.HasNoneOf(OutputBecomesMask)
            || (MatchScaff & condition).HasAnyOf(OutputBecomesMask))));
        var hasMatchingInputType           = (MatchScaff & AcceptsAnyGeneric & condition).HasAnyOf(MatchScaff & AcceptsAnyGeneric);
        var conditionIsSubSpanOnlyCallType = (condition & SubSpanCallMask) > 0;
        var meetsInputTypeCondition        = (hasMatchingInputType && !conditionIsSubSpanOnlyCallType);
        var isSameSubSpanCalType =
            ((condition & SubSpanCallMask) & (MatchScaff & SubSpanCallMask)) == (condition & SubSpanCallMask);
        var checkIsSubSpanOnlyCallType = (MatchScaff & SubSpanCallMask) > 0;
        var meetSubSpanCallType        = (conditionIsSubSpanOnlyCallType && checkIsSubSpanOnlyCallType && isSameSubSpanCalType);
        return styleIsMatched && bothCallViaObjectOrNeither && bothCallViaMatchOrNeither && meetsMoldTypeCondition && meetsWriteCondition 
            && meetsOutputType && (meetsInputTypeCondition || (meetSubSpanCallType));
    }

    public bool Equals(EK? other) => matchScaff == other?.matchScaff && matchStyle == other.matchStyle;

    public override bool Equals(object? obj) => obj is EK other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(matchScaff, (int)matchStyle);

    public int AtIndex { get; set; }

    public override string ToString() => $"{{ {nameof(AtIndex)}: {AtIndex}, {nameof(MatchScaff)}: {MatchScaff}, " +
                                         $"{nameof(MatchStyle)}: {MatchStyle} }}";
}
