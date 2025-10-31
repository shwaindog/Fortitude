// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MoldCrucible;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface IFormatExpectation
{
    public const string NoResultExpectedValue = "No ResultExpected Value";

    Type InputType { get; }
    Type CoreType { get; }

    bool IsNullable { get; }
    bool InputIsNull { get; }
    bool InputIsEmpty { get; }

    bool IsStruct { get; }
    string? FormatString { get; }

    string? DefaultAsString(IStyledTypeFormatting styleFormatting);

    string? FormattedDefault { get; }
    bool IsStringLike { get; }

    bool HasDefault { get; }

    string ShortTestName { get; }

    bool HasIndexRangeLimiting { get; }

    string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null);

    IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle);
}

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
        var meetsMoldTypeCondition = (MatchScaff & MoldTypeConditionMask) == 0 || (MatchScaff & MoldTypeConditionMask & condition) > 0;
        var meetsWriteCondition    = (MatchScaff & OutputConditionMask & condition) > 0 || condition.HasAnyOf((SimpleType));
        var conditionHasBothBecomesNullAndFallback = (condition & OutputBecomesMask) == (DefaultBecomesNull | DefaultBecomesFallback)
                                                  && (condition & SupportsSettingDefaultValue) > 0;
        var scaffHasBothBecomesNullAndFallback = (MatchScaff & OutputBecomesMask) == (DefaultBecomesNull | DefaultBecomesFallback);
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
        return styleIsMatched && bothGenericOrNotScaff && meetsMoldTypeCondition && meetsWriteCondition && meetsOutputType
            && (meetsInputTypeCondition || (meetSubSpanCallType));
    }

    public bool Equals(EK? other) => matchScaff == other?.matchScaff && matchStyle == other.matchStyle;

    public override bool Equals(object? obj) => obj is EK other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(matchScaff, (int)matchStyle);

    public override string ToString() => $"{{ {nameof(MatchScaff)}: {MatchScaff}, {nameof(MatchStyle)}: {MatchStyle} }}";
}

public interface IComplexFieldFormatExpectation : IFormatExpectation
{
    BuildExpectedOutput WhenValueExpectedOutput { get; set; }
}

public interface ITypedFormatExpectation<out T> : IFormatExpectation
{
    T? Input { get; }

    void ClearExpectations();

    void Add(EK key, string value);
    void Add(KeyValuePair<EK, string> newExpectedResult);
}

public abstract class FieldExpectBase<TInput, TDefault> : ITypedFormatExpectation<TInput>, IEnumerable<KeyValuePair<EK, string>>
{
    private static readonly IVersatileFLogger Logger = FLog.FLoggerForType.As<IVersatileFLogger>();

    protected readonly List<KeyValuePair<EK, string>> ExpectedResults = new();

    // ReSharper disable once ConvertToPrimaryConstructor
    protected FieldExpectBase(TInput? input, string? formatString = null, bool hasDefault = false, TDefault? defaultValue = default)
    {
        Input        = input;
        FormatString = formatString;
        HasDefault   = hasDefault;
        DefaultValue = !typeof(TInput).IfNullableGetUnderlyingTypeOrThis().ImplementsInterface<IStringBearer>()
            ? defaultValue.IfNullableGetNonNullableUnderlyingDefault()
            : defaultValue;
    }

    public virtual Type InputType => typeof(TInput);


    public bool IsStringLike => InputType.IsAnyTypeHoldingChars();

    public virtual Type CoreType => InputType.IfNullableGetUnderlyingTypeOrThis();

    public TInput? Input { get; set; }

    public string? FormatString { get; init; }

    public string DefaultAsString(IStyledTypeFormatting styleFormatting)
    {
        // if (!HasDefault && !InputType.IsValueType && InputIsNull) return DefaultValue?.ToString() ?? "null";
        if (!HasDefault
         && ((InputType.IsValueType && InputType.IsNullable())
          || !InputType.IsValueType && Equals(DefaultValue, null)))
            return "null";
        if (!HasDefault && InputType.IsValueType && Equals(DefaultValue, default(TInput))) return "0";
        var sb = new MutableString();
        switch (DefaultValue)
        {
            case bool boolDefault:             styleFormatting.Format(boolDefault, sb, FormatString ?? ""); break;
            case ISpanFormattable spanDefault: styleFormatting.Format(spanDefault, sb, FormatString ?? ""); break;
            case string stringDefault:         styleFormatting.Format(stringDefault, 0, sb, FormatString ?? ""); break;
            case char[] charArrayDefault:      styleFormatting.Format(charArrayDefault, 0, sb, FormatString ?? ""); break;
            case ICharSequence charSeqDefault: styleFormatting.Format(charSeqDefault, 0, sb, FormatString ?? ""); break;
            case StringBuilder sbDefault:      styleFormatting.Format(sbDefault, 0, sb, FormatString ?? ""); break;
            default:                           styleFormatting.Format(DefaultValue?.ToString() ?? "null", 0, sb, FormatString ?? ""); break;
        }
        return sb.ToString();
    }

    public bool HasDefault { get; init; }

    public TDefault? DefaultValue { get; init; }

    public string? FormattedDefault { get; protected set; }

    public virtual bool IsNullable => InputType.IsNullable() || InputIsNull;

    public bool InputIsNull => Input == null;
    public abstract bool InputIsEmpty { get; }

    public bool IsStruct => InputType.IsValueType;

    public virtual bool HasIndexRangeLimiting => false;

    public virtual string ShortTestName
    {
        get
        {
            {
                var result = new MutableString();
                result.Append(InputType.ShortNameInCSharpFormat());
                if (Input == null) { result.Append("=null"); }
                else
                {
                    result.Append(AsStringDelimiterOpen)
                          .Append(new MutableString().Append(Input).ToString())
                          .Append(AsStringDelimiterClose).Append("_").Append(FormatString);
                }
                if (HasDefault)
                {
                    var defaultString = DefaultValue?.ToString() ?? "null";
                    if (defaultString.IsNotEmpty()) { result.Append($"_{defaultString}"); }
                }

                return result.ToString();
            }
        }
    }

    protected string AsStringDelimiterOpen =>
        InputType.Name switch
        {
            "String"        => "\""
          , "MutableString" => "\""
          , "StringBuilder" => "\""
          , "Char"          => "'"
          , "Char[]"        => "["
          , _               => "("
        };

    protected string AsStringDelimiterClose =>
        InputType.Name switch
        {
            "String"        => "\""
          , "MutableString" => "\""
          , "StringBuilder" => "\""
          , "Char"          => "'"
          , "Char[]"        => "]"
          , _               => ")"
        };

    public virtual string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null)
    {
        for (var i = 0; i < ExpectedResults.Count; i++)
        {
            var existing = ExpectedResults[i];
            if (!existing.Key.IsMatchingScenario(condition, stringStyle.Style))
            {
                Logger.DebugAppend("Rejected -")?.FinalAppend(i);
                continue;
            }
            Logger.WarnAppend("Selected -")?.FinalAppend(i);
            var rawInternal = existing.Value;
            return rawInternal;
        }
        Logger.Error("No Match Found !");
        return IFormatExpectation.NoResultExpectedValue;
    }

    public void Add(EK key, string value)
    {
        ExpectedResults.Add(new KeyValuePair<EK, string>(key, value));
    }

    public void Add(KeyValuePair<EK, string> newExpectedResult)
    {
        Add(newExpectedResult.Key, newExpectedResult.Value);
    }

    public void ClearExpectations()
    {
        ExpectedResults.Clear();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<EK, string>> GetEnumerator() => ExpectedResults.GetEnumerator();

    public abstract IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry);
    public abstract IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle);

    public override string ToString()
    {
        var sb = new MutableString();
        sb.Append(nameof(InputType)).Append(": ").Append(InputType.ShortNameInCSharpFormat()).Append(", ");
        sb.Append(nameof(Input)).Append(": ");
        if (InputIsNull) { sb.Append("null"); }
        else { sb.Append(AsStringDelimiterOpen).Append(new MutableString().Append(Input).ToString()).Append(AsStringDelimiterClose); }
        sb.Append(", ").Append(nameof(FormatString)).Append(": ").Append(FormatString != null ? $"\"{FormatString}\"" : "null");
        sb.Append(", ").Append(nameof(HasDefault)).Append(": ").Append(HasDefault ? "true" : "false");
        sb.Append(", ").Append(nameof(DefaultValue)).Append(": ");
        if (DefaultValue == null) { sb.Append("null"); }
        else { sb.Append(AsStringDelimiterOpen).Append(new MutableString().Append(DefaultValue).ToString()).Append(AsStringDelimiterClose); }
        sb.Append(", ").Append(nameof(DefaultAsString)).Append(": ")
          .Append(AsStringDelimiterOpen)
          .Append(new MutableString()
                  .Append(DefaultAsString(new CompactJsonTypeFormatting())).ToString())
          .Append(AsStringDelimiterClose);
        return sb.ToString();
    }
}

public class FieldExpect<TInput>
(
    TInput? input
  , string? formatString = null
  , bool hasDefault = false
  , TInput? defaultValue = default
  , int fromIndex = 0
  , int length = Int32.MaxValue)
    : FieldExpect<TInput, TInput>(input, formatString, hasDefault, defaultValue, fromIndex, length);

public class FieldExpect<TInput, TDefault> : FieldExpectBase<TInput?, TDefault?>
{
    public int FromIndex { get; init; }
    public int Length { get; init; }

    public override bool HasIndexRangeLimiting => FromIndex != 0 || Length != int.MaxValue;

    // ReSharper disable once ConvertToPrimaryConstructor
    public FieldExpect(TInput? input, string? formatString = null, bool hasDefault = false
      , TDefault? defaultValue = default, int fromIndex = 0, int length = int.MaxValue) : base(input, formatString, hasDefault, defaultValue)
    {
        FromIndex = fromIndex;
        Length    = length;
    }

    public override bool InputIsEmpty
    {
        get
        {
            switch (Input)
            {
                case string stringValue:
                    return stringValue.Length == 0
                        || FromIndex >= stringValue.Length || Length <= 0;
                case char[] charArrayValue:
                    return charArrayValue.Length == 0
                        || FromIndex >= charArrayValue.Length || Length <= 0;
                case ICharSequence charSeqValue:
                    return charSeqValue.Length == 0
                        || FromIndex >= charSeqValue.Length || Length <= 0;
                case StringBuilder sbValue:
                    return sbValue.Length == 0
                        || FromIndex >= sbValue.Length || Length <= 0;
                default: return Input != null && Equals(Input, default(TInput));
            }
        }
    }

    public override string ShortTestName
    {
        get
        {
            {
                var result = new MutableString();
                result.Append(InputType.ShortNameInCSharpFormat());
                if (Input == null) { result.Append("=null"); }
                else
                {
                    result.Append(AsStringDelimiterOpen)
                          .AppendFormat(ICustomStringFormatter.DefaultBufferFormatter, "{0}", Input)
                          .Append(AsStringDelimiterClose).Append("_").Append(FormatString);
                }
                if (HasDefault)
                {
                    if (DefaultValue is string defaultString)
                    {
                        if (defaultString.Length > 0) { result.Append("_").Append(DefaultValue); }
                    }
                    else if (DefaultValue is char[] defaultCharArray)
                    {
                        if (defaultCharArray.Length > 0) { result.Append("_").Append(DefaultValue); }
                    }
                    else if (DefaultValue is ICharSequence defaultCharSeq)
                    {
                        if (defaultCharSeq.Length > 0) { result.Append("_").Append(DefaultValue); }
                    }
                    else if (DefaultValue is StringBuilder defaultSb)
                    {
                        if (defaultSb.Length > 0) { result.Append("_").Append(DefaultValue); }
                    }
                    else { result.Append("_").Append(DefaultValue); }
                }

                return result.ToString();
            }
        }
    }

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        return scaffoldEntry.ScaffoldingFlags.IsNullableSpanFormattableOnly()
            ? scaffoldEntry.CreateStringBearerFunc(CoreType)()
            : scaffoldEntry.CreateStringBearerFunc(InputType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        if (InputType == typeof(string) && createdStringBearer is ISupportsSettingValueFromString supportsSettingValueFromString)
            supportsSettingValueFromString.StringValue = (string?)(object?)Input;
        else if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = Input;
        else
            ((IMoldSupportedValue<TInput?>)createdStringBearer).Value = Input;
        if (FormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = FormatString;
        if (FormatString != null && createdStringBearer is ISupportsIndexRangeLimiting indexRangeLimiting)
        {
            indexRangeLimiting.FromIndex = FromIndex;
            indexRangeLimiting.Length    = Length;
        }
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<TDefault> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue ?? default(TDefault)!;
        if (createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
        {
            var expectedDefaultString = DefaultAsString(stringStyle.StyledTypeFormatter);
            FormattedDefault = new MutableString().Append(expectedDefaultString).ToString();
            supportsStringDefaultValue.DefaultValue =
                scaffoldEntry.ScaffoldingFlags.HasAnyOf(DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
             && !InputType.IsAnyTypeHoldingChars() && !InputType.IsSpanFormattableOrNullable()
                    ? expectedDefaultString
                    : new MutableString().Append(DefaultValue).ToString();

            if (InputType.IsTimeFormattableOrNullable())
            {
                supportsStringDefaultValue.DefaultValue = expectedDefaultString.Replace("\"", "").Replace("\'", "").Trim();
            }
        }
        return createdStringBearer;
    }

    public override string ToString()
    {
        var sb = new MutableString();
        sb.AppendLine(GetType().ShortNameInCSharpFormat());
        sb.Append(base.ToString());
        if (FromIndex != 0 || Length != int.MaxValue)
        {
            sb.Append(", ").Append(nameof(FromIndex)).Append(": ").Append(FromIndex).Append(", ");
            sb.Append(nameof(Length)).Append(": ").Append(Length);
        }
        sb.AppendLine();
        sb.AppendLine("ExpectedResults");
        var count = 0;
        foreach (var keyValuePair in ExpectedResults)
        {
            sb.Append(count++).Append(" - ").Append("{ ").Append(keyValuePair.Key).Append(", >").Append(keyValuePair.Value).AppendLine("< }");
        }
        return sb.ToString();
    }
};

public delegate ITypedFormatExpectation<T?> CreateFieldExpectation<T>(T? input, string? formatString = null
  , bool hasDefault = false, T? defaultValue = default);

public delegate string BuildExpectedOutput(string className, string propertyName, ScaffoldingStringBuilderInvokeFlags condition
  , IFormatExpectation expectation);

public class CloakedBearerExpect<TChildScaffoldType, TChildScaffold> : FieldExpect<TChildScaffoldType>, IComplexFieldFormatExpectation
    where TChildScaffold : ISinglePropertyTestStringBearer, IUnknownPalantirRevealerFactory
{
    private ScaffoldingPartEntry? calledScaffoldingPart;

    public CloakedBearerExpect(TChildScaffoldType? input, string? formatString = null
      , bool hasDefault = false, TChildScaffoldType? defaultValue = default) : base(input, formatString, hasDefault, defaultValue)
    {
        FieldValueExpectation = new FieldExpect<TChildScaffoldType>(Input, FormatString, HasDefault, DefaultValue);
    }

    public ITypedFormatExpectation<TChildScaffoldType?> FieldValueExpectation { get; }

    public override bool IsNullable => InputType.IsNullable();

    public TChildScaffold RevealerScaffold { get; set; } = default!;

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        if (Input is string || Input is char[] || Input is ICharSequence || Input is StringBuilder)
        {
            condition |= AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder;
        }
        var expectValue = FieldValueExpectation.GetExpectedOutputFor(condition, stringStyle, formatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue && Input != null)
        {
            expectValue = WhenValueExpectedOutput
                ((Input?.GetType() ?? typeof(TChildScaffoldType)).ShortNameInCSharpFormat()
               , $"CloakedRevealer{RevealerScaffold.PropertyName}", condition, FieldValueExpectation);
        }
        return expectValue;
    }

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        return scaffoldEntry.ScaffoldingFlags.IsNullableSpanFormattableOnly()
            ? scaffoldEntry.CreateStringBearerFunc(CoreType)()
            : scaffoldEntry.CreateStringBearerFunc(InputType, CoreType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        calledScaffoldingPart = new ScaffoldingPartEntry(typeof(TChildScaffold), scaffoldEntry.ScaffoldingFlags);
        RevealerScaffold      = calledScaffoldingPart.CreateTypedStringBearerFunc<TChildScaffold>()();
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<TChildScaffoldType> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue ?? default(TChildScaffoldType)!;
        if (createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
        {
            var expectedDefaultString = DefaultAsString(stringStyle.StyledTypeFormatter);
            FormattedDefault = new MutableString().Append(expectedDefaultString).ToString();
            supportsStringDefaultValue.DefaultValue =
                scaffoldEntry.ScaffoldingFlags.HasAnyOf(DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
             && !InputType.IsSpanFormattableOrNullable()
                    ? expectedDefaultString
                    : new MutableString().Append(DefaultValue).ToString();

            createdStringBearer = (IStringBearer)supportsStringDefaultValue;
        }
        if (FormatString != null && RevealerScaffold is ISupportsValueFormatString supportsValueFormatString)
        {
            supportsValueFormatString.ValueFormatString = FormatString;

            RevealerScaffold = (TChildScaffold)supportsValueFormatString;
        }
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = Input;
        else { ((IMoldSupportedValue<TChildScaffoldType?>)createdStringBearer).Value = Input; }
        if (createdStringBearer is ISupportsUnknownValueRevealer supportsValueRevealer)
        {
            supportsValueRevealer.ValueRevealerDelegate = RevealerScaffold.CreateRevealerDelegate;
        }
        return createdStringBearer;
    }
}

public class StringBearerExpect<TInput>
(
    TInput? input
  , string? formatString = null
  , bool hasDefault = false
  , TInput? defaultValue = default)
    : StringBearerExpect<TInput, TInput>(input, formatString, hasDefault, defaultValue)
    where TInput : IStringBearer;

public class StringBearerExpect<TInput, TDefault> : FieldExpect<TInput, TDefault>, IComplexFieldFormatExpectation
    where TInput : IStringBearer
{
    public StringBearerExpect(TInput? input, string? formatString = null
      , bool hasDefault = false, TDefault? defaultValue = default) : base(input, formatString, hasDefault, defaultValue)
    {
        FieldValueExpectation = new FieldExpect<TInput?, TDefault?>(Input, FormatString, HasDefault, DefaultValue);
    }

    public ITypedFormatExpectation<TInput?> FieldValueExpectation { get; }

    public override bool IsNullable => InputType.IsNullable();

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        condition |= AcceptsSpanFormattable | AcceptsChars | AcceptsString;
        var expectValue = FieldValueExpectation.GetExpectedOutputFor(condition, stringStyle, formatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue && Input != null)
        {
            expectValue = WhenValueExpectedOutput
                ((Input?.GetType() ?? typeof(TInput)).ShortNameInCSharpFormat(), ((ISinglePropertyTestStringBearer)Input!).PropertyName, condition
               , FieldValueExpectation);
        }
        return expectValue;
    }

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        return scaffoldEntry.CreateStringBearerFunc(InputType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        if (createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (createdStringBearer is IMoldSupportedDefaultValue<TDefault> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue ?? default(TDefault)!;
        if (createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
        {
            var expectedDefaultString = DefaultAsString(stringStyle.StyledTypeFormatter);
            FormattedDefault = new MutableString().Append(expectedDefaultString).ToString();
            supportsStringDefaultValue.DefaultValue =
                scaffoldEntry.ScaffoldingFlags.HasAnyOf(DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
             && !InputType.IsSpanFormattableOrNullable()
                    ? expectedDefaultString
                    : new MutableString().Append(DefaultValue).ToString();

            createdStringBearer = (IStringBearer)supportsStringDefaultValue;
        }
        var stringBearerInput = Input;
        if (FormatString != null && stringBearerInput is ISupportsValueFormatString supportsValueFormatString)
        {
            supportsValueFormatString.ValueFormatString = FormatString;

            stringBearerInput = (TInput)(supportsValueFormatString);
        }
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = stringBearerInput;
        else if (createdStringBearer is IMoldSupportedValue<TInput?> nullableMoldBearer)
        {
            nullableMoldBearer.Value = stringBearerInput;

            createdStringBearer = nullableMoldBearer;
        }
        else if (createdStringBearer is IMoldSupportedValue<TInput> moldBearer)
        {
            moldBearer.Value = stringBearerInput ?? throw new ArgumentNullException(nameof(stringBearerInput));

            createdStringBearer = moldBearer;
        }
        return createdStringBearer;
    }
}

public class NullableStringBearerExpect<TInput> : NullableStringBearerExpect<TInput, TInput>
    where TInput : struct, IStringBearer
{
    public NullableStringBearerExpect(TInput? input, string? formatString = null
      , bool hasDefault = false, TInput? defaultValue = null) : base(input, formatString, hasDefault, defaultValue)
    {
        FieldValueExpectation = new FieldExpect<TInput?>(Input, FormatString, HasDefault, DefaultValue);
    }
}

public class NullableStringBearerExpect<TInput, TDefault> : FieldExpect<TInput?, TDefault?>, IComplexFieldFormatExpectation
    where TInput : struct, IStringBearer
    where TDefault : struct
{
    public ITypedFormatExpectation<TInput?> FieldValueExpectation { get; protected init; }

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override bool IsNullable => InputType.IsNullable();

    public NullableStringBearerExpect(TInput? input, string? formatString = null
      , bool hasDefault = false, TDefault? defaultValue = null) : base(input, formatString, hasDefault, defaultValue)
    {
        FieldValueExpectation = new FieldExpect<TInput?, TDefault?>(Input, FormatString, HasDefault, DefaultValue);
    }

    public override string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        condition |= AcceptsSpanFormattable | AcceptsChars | AcceptsString;
        var expectValue = FieldValueExpectation.GetExpectedOutputFor(condition, stringStyle, formatString);
        if (expectValue != IFormatExpectation.NoResultExpectedValue && Input != null)
        {
            expectValue = WhenValueExpectedOutput
                ((Input?.GetType() ?? typeof(TInput)).ShortNameInCSharpFormat(), ((ISinglePropertyTestStringBearer)Input!.Value).PropertyName
               , condition
               , FieldValueExpectation);
        }
        return expectValue;
    }

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        return scaffoldEntry.ScaffoldingFlags.HasAcceptsNullableStruct() && !scaffoldEntry.ScaffoldingFlags.IsAcceptsAnyGeneric()
            ? scaffoldEntry.CreateStringBearerFunc(CoreType)()
            : scaffoldEntry.CreateStringBearerFunc(InputType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry, StyleOptions stringStyle)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        if (createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (createdStringBearer is IMoldSupportedDefaultValue<TDefault?> supportsDefaultValue) { supportsDefaultValue.DefaultValue = DefaultValue; }
        if (createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
        {
            var expectedDefaultString = DefaultAsString(stringStyle.StyledTypeFormatter);
            FormattedDefault = new MutableString().Append(expectedDefaultString).ToString();
            supportsStringDefaultValue.DefaultValue =
                scaffoldEntry.ScaffoldingFlags.HasAnyOf(DefaultTreatedAsValueOut | DefaultTreatedAsStringOut)
             && !InputType.IsSpanFormattableOrNullable()
                    ? expectedDefaultString
                    : new MutableString().Append(DefaultValue).ToString();
        }
        var stringBearerInput = Input;
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (FormatString != null && stringBearerInput is ISupportsValueFormatString supportsValueFormatString)
        {
            supportsValueFormatString.ValueFormatString = FormatString;

            stringBearerInput = (TInput)(supportsValueFormatString);
        }
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = stringBearerInput;
        else if (createdStringBearer is IMoldSupportedValue<TInput?> nullableMoldBearer)
        {
            nullableMoldBearer.Value = stringBearerInput;

            createdStringBearer = nullableMoldBearer;
        }
        else if (createdStringBearer is IMoldSupportedValue<TInput> moldBearer)
        {
            moldBearer.Value = stringBearerInput ?? throw new ArgumentNullException(nameof(stringBearerInput));

            createdStringBearer = moldBearer;
        }
        return createdStringBearer;
    }
}
