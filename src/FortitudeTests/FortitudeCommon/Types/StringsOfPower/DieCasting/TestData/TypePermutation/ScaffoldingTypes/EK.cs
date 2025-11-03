// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

// Expect Key shortened to reduce obscuring declarative expect definition
// ReSharper disable once InconsistentNaming
public class EK
(
    ScaffoldingStringBuilderInvokeFlags matchScaff
  , StringStyle matchStyle = StringStyle.Compact | StringStyle.Json | StringStyle.Log | StringStyle.Pretty)
    : IEquatable<EK>
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
        var bothGenericOrNotScaff = true;
        if (MatchScaff.IsAcceptsAnyGeneric()) { bothGenericOrNotScaff = condition.IsAcceptsAnyGeneric(); }
        var meetsMoldTypeCondition = (MatchScaff & ScaffoldingStringBuilderInvokeFlags.MoldTypeConditionMask) == 0 || (MatchScaff & ScaffoldingStringBuilderInvokeFlags.MoldTypeConditionMask & condition) > 0;
        var meetsWriteCondition    = (MatchScaff & ScaffoldingStringBuilderInvokeFlags.OutputConditionMask & condition) > 0 || condition.HasAnyOf((ScaffoldingStringBuilderInvokeFlags.SimpleType));
        var conditionHasBothBecomesNullAndFallback = (condition & ScaffoldingStringBuilderInvokeFlags.OutputBecomesMask) == (ScaffoldingStringBuilderInvokeFlags.DefaultBecomesNull | ScaffoldingStringBuilderInvokeFlags.DefaultBecomesFallback)
                                                  && (condition & ScaffoldingStringBuilderInvokeFlags.SupportsSettingDefaultValue) > 0;
        var scaffHasBothBecomesNullAndFallback = (MatchScaff & ScaffoldingStringBuilderInvokeFlags.OutputBecomesMask) == (ScaffoldingStringBuilderInvokeFlags.DefaultBecomesNull | ScaffoldingStringBuilderInvokeFlags.DefaultBecomesFallback);
        var meetsOutputType =
            ((MatchScaff.HasNoneOf(ScaffoldingStringBuilderInvokeFlags.OutputTreatedMask)
           || condition.HasNoneOf(ScaffoldingStringBuilderInvokeFlags.OutputTreatedMask)
           || (MatchScaff & condition).HasAnyOf(ScaffoldingStringBuilderInvokeFlags.OutputTreatedMask))
           &&
             ((!scaffHasBothBecomesNullAndFallback || conditionHasBothBecomesNullAndFallback)
           && (MatchScaff.HasNoneOf(ScaffoldingStringBuilderInvokeFlags.OutputBecomesMask)
            || condition.HasNoneOf(ScaffoldingStringBuilderInvokeFlags.OutputBecomesMask)
            || (MatchScaff & condition).HasAnyOf(ScaffoldingStringBuilderInvokeFlags.OutputBecomesMask))));
        var hasMatchingInputType           = (MatchScaff & ScaffoldingStringBuilderInvokeFlags.AcceptsAnyGeneric & condition).HasAnyOf(MatchScaff & ScaffoldingStringBuilderInvokeFlags.AcceptsAnyGeneric);
        var conditionIsSubSpanOnlyCallType = (condition & ScaffoldingStringBuilderInvokeFlags.SubSpanCallMask) > 0;
        var meetsInputTypeCondition        = (hasMatchingInputType && !conditionIsSubSpanOnlyCallType);
        var isSameSubSpanCalType =
            ((condition & ScaffoldingStringBuilderInvokeFlags.SubSpanCallMask) & (MatchScaff & ScaffoldingStringBuilderInvokeFlags.SubSpanCallMask)) == (condition & ScaffoldingStringBuilderInvokeFlags.SubSpanCallMask);
        var checkIsSubSpanOnlyCallType = (MatchScaff & ScaffoldingStringBuilderInvokeFlags.SubSpanCallMask) > 0;
        var meetSubSpanCallType        = (conditionIsSubSpanOnlyCallType && checkIsSubSpanOnlyCallType && isSameSubSpanCalType);
        return styleIsMatched && bothGenericOrNotScaff && meetsMoldTypeCondition && meetsWriteCondition && meetsOutputType
            && (meetsInputTypeCondition || (meetSubSpanCallType));
    }

    public bool Equals(EK? other) => matchScaff == other?.matchScaff && matchStyle == other.matchStyle;

    public override bool Equals(object? obj) => obj is EK other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(matchScaff, (int)matchStyle);

    public override string ToString() => $"{{ {nameof(MatchScaff)}: {MatchScaff}, {nameof(MatchStyle)}: {MatchStyle} }}";
}
