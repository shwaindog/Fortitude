// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
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

    bool IsStruct { get; }
    string? FormatString { get; }

    bool HasDefault { get; }

    string ShortTestName { get; }

    bool HasIndexRangeLimiting { get; }

    string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null);

    IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry);
}

// Expect Key shortened to reduce obscuring declarative expect definition
public class EK : IEquatable<EK>
{
    private readonly ScaffoldingStringBuilderInvokeFlags matchScaff;
    private readonly StringStyle                         matchStyle;
    
    public EK(ScaffoldingStringBuilderInvokeFlags matchScaff
      , StringStyle matchStyle = StringStyle.Compact | StringStyle.Json | StringStyle.Log | StringStyle.Pretty)
    {
        this.matchScaff = matchScaff;
        this.matchStyle      = matchStyle;
    }
    
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
        var styleIsMatched                 = (style & MatchStyle) == style;
        var meetsWriteCondition            = (MatchScaff & OutputConditionMask & condition) > 0;
        var hasMatchingInputType           = (MatchScaff & AcceptsAnyGeneric & condition).HasAnyOf(MatchScaff & AcceptsAnyGeneric);
        var conditionIsSubSpanOnlyCallType = (condition & SubSpanCallMask) > 0;
        var meetsInputTypeCondition        = (hasMatchingInputType && !conditionIsSubSpanOnlyCallType);
        var isSameSubSpanCalType =
            ((condition & SubSpanCallMask) & (MatchScaff & SubSpanCallMask)) == (condition & SubSpanCallMask);
        var checkIsSubSpanOnlyCallType = (MatchScaff & SubSpanCallMask) > 0;
        var meetSubSpanCallType        = (conditionIsSubSpanOnlyCallType && checkIsSubSpanOnlyCallType && isSameSubSpanCalType);
        return styleIsMatched && meetsWriteCondition && (meetsInputTypeCondition || (meetSubSpanCallType));
    }

    public bool Equals(EK? other) => matchScaff == other?.matchScaff && matchStyle == other.matchStyle;

    public override bool Equals(object? obj) => obj is EK other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(matchScaff, (int)matchStyle);
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

public abstract class FieldExpectBase<T> : ITypedFormatExpectation<T>, IEnumerable<KeyValuePair<EK, string>>
{
    protected readonly List<KeyValuePair<EK, string>> ExpectedResults = new();

    protected FieldExpectBase(T? input, string? formatString = null, bool hasDefault = false, T? defaultValue = default)
    {
        Input        = input;
        FormatString = formatString;
        HasDefault   = hasDefault;
        DefaultValue = defaultValue.IfNullableGetNonNullableUnderlyingDefault();
    }

    public virtual Type InputType => typeof(T);

    public virtual Type CoreType => InputType.IfNullableGetUnderlyingTypeOrThis();

    public T? Input { get; init; }

    public string? FormatString { get; init; }

    public bool HasDefault { get; init; }

    public T? DefaultValue { get; init; }

    public virtual bool IsNullable => InputType.IsNullable() || Input == null;

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
                    result.Append(AsStringDelimiter)
                          .AppendFormat(ICustomStringFormatter.DefaultBufferFormatter, "{0}", Input)
                          .Append(AsStringDelimiter).Append("_").Append(FormatString);
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

    protected string AsStringDelimiter =>
        InputType.Name switch
        {
            "String"        => "\""
          , "MutableString" => "\""
          , "StringBuilder" => "\""
          , "Char"          => "'"
          , _               => ""
        };

    public virtual string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null)
    {
        for (var i = 0; i < ExpectedResults.Count; i++)
        {
            var existing = ExpectedResults[i];
            if (!existing.Key.IsMatchingScenario(condition, stringStyle.Style)) continue;
            var  rawInternal = existing.Value;
            return rawInternal;
        }
        return IFormatExpectation.NoResultExpectedValue;
    }

    public void Add(EK key, string value)
    {
        for (var i = 0; i < ExpectedResults.Count; i++)
        {
            var existing    = ExpectedResults[i];
            var existingKey = existing.Key.MatchScaff;
            if (!existing.Key.IsMatchingScenario(key.MatchScaff, key.MatchStyle)) continue;
            existingKey &= ~(key.MatchScaff);
            if (existingKey == None)
                ExpectedResults.RemoveAt(i);
            else
                ExpectedResults[i] = new KeyValuePair< EK, string>(new EK(existingKey, key.MatchStyle), existing.Value);
            break;
        }
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
    public abstract IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry);

    public override string ToString()
    {
        var sb = new MutableString();
        sb.Append(nameof(InputType)).Append(": ").Append(InputType.ShortNameInCSharpFormat()).Append(", ");
        sb.Append(nameof(Input)).Append(": ").Append(AsStringDelimiter).Append(Input).Append(AsStringDelimiter).Append(", ");
        sb.Append(nameof(FormatString)).Append(": ").Append(FormatString != null ? $"\"{FormatString}\"" : "null");
        if (HasDefault)
        {
            sb.Append(", ").Append(nameof(DefaultValue)).Append(": ");
            if (InputType.IsChar() || InputType.IsString() || InputType.IsCharSequence() || InputType.IsStringBuilder())
            {
                sb.Append('"').Append(DefaultValue).Append('"');
            }
            else { sb.Append(DefaultValue?.GetType().ShortNameInCSharpFormat() ?? "null"); }
        }
        return sb.ToString();
    }
}

public class FieldExpect<T> : FieldExpectBase<T>
{
    public int FromIndex { get; init; }
    public int Length { get; init; }

    public override bool HasIndexRangeLimiting => FromIndex != 0 || Length != int.MaxValue;

    public FieldExpect(T? input, string? formatString = null, bool hasDefault = false
      , T? defaultValue = default, int fromIndex = 0, int length = int.MaxValue) : base(input, formatString, hasDefault, defaultValue)
    {
        FromIndex = fromIndex;
        Length    = length;
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
                    result.Append(AsStringDelimiter)
                          .AppendFormat(ICustomStringFormatter.DefaultBufferFormatter, "{0}", Input)
                          .Append(AsStringDelimiter).Append("_").Append(FormatString);
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

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        if (InputType == typeof(string) && createdStringBearer is ISupportsSettingValueFromString supportsSettingValueFromString)
            supportsSettingValueFromString.StringValue = (string?)(object?)Input;
        else if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = Input;
        else
            ((IMoldSupportedValue<T?>)createdStringBearer).Value = Input;
        if (FormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = FormatString;
        if (FormatString != null && createdStringBearer is ISupportsIndexRangeLimiting indexRangeLimiting)
        {
            indexRangeLimiting.FromIndex = FromIndex;
            indexRangeLimiting.Length    = Length;
        }
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<T> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue ?? default(T)!;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
            supportsStringDefaultValue.DefaultValue = new MutableString().Append(DefaultValue).ToString();
        return createdStringBearer;
    }

    public override string ToString()
    {
        var sb = new MutableString(base.ToString());
        if (FromIndex != 0 || Length != int.MaxValue)
        {
            sb.Append(", ").Append(nameof(FromIndex)).Append(": ").Append(FromIndex).Append(", ");
            sb.Append(nameof(Length)).Append(": ").Append(Length);
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

    public ITypedFormatExpectation<TChildScaffoldType> FieldValueExpectation { get; }

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
                (Input.GetType().ShortNameInCSharpFormat(), $"CloakedRevealer{RevealerScaffold.PropertyName}", condition, FieldValueExpectation);
        }
        return expectValue;
    }

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        return scaffoldEntry.ScaffoldingFlags.IsNullableSpanFormattableOnly()
            ? scaffoldEntry.CreateStringBearerFunc(CoreType)()
            : scaffoldEntry.CreateStringBearerFunc(InputType, CoreType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry)
    {
        calledScaffoldingPart = new ScaffoldingPartEntry(typeof(TChildScaffold), scaffoldEntry.ScaffoldingFlags);
        RevealerScaffold      = calledScaffoldingPart.CreateTypedStringBearerFunc<TChildScaffold>()();
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        if (InputType == typeof(string) && createdStringBearer is ISupportsSettingValueFromString supportsSettingValueFromString)
            supportsSettingValueFromString.StringValue = (string?)(object?)Input;
        else if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = Input;
        else
            ((IMoldSupportedValue<TChildScaffoldType?>)createdStringBearer).Value = Input;
        if (FormatString != null && RevealerScaffold is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = FormatString;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<TChildScaffoldType> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue ?? default(TChildScaffoldType)!;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
            supportsStringDefaultValue.DefaultValue = new MutableString().Append(DefaultValue).ToString();
        if (createdStringBearer is ISupportsUnknownValueRevealer supportsValueRevealer)
        {
            supportsValueRevealer.ValueRevealerDelegate = RevealerScaffold.CreateRevealerDelegate;
        }
        return createdStringBearer;
    }
}

public class StringBearerExpect<T> : FieldExpect<T>, IComplexFieldFormatExpectation
    where T : IStringBearer
{
    public StringBearerExpect(T? input, string? formatString = null
      , bool hasDefault = false, T? defaultValue = default) : base(input, formatString, hasDefault, defaultValue)
    {
        // this.scaffoldType     = scaffoldType;
        FieldValueExpectation = new FieldExpect<T>(Input, FormatString, HasDefault, DefaultValue);
    }

    public ITypedFormatExpectation<T> FieldValueExpectation { get; }

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
                (Input.GetType().ShortNameInCSharpFormat(), ((ISinglePropertyTestStringBearer)Input).PropertyName, condition, FieldValueExpectation);
        }
        return expectValue;
    }

    public override IStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
    {
        return scaffoldEntry.CreateStringBearerFunc(InputType)();
    }

    public override IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry)
    {
        var createdStringBearer = CreateNewStringBearer(scaffoldEntry);
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = Input;
        else
            ((IMoldSupportedValue<T?>)createdStringBearer).Value = Input;
        if (FormatString != null && Input is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = FormatString;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<T> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue ?? default(T)!;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
            supportsStringDefaultValue.DefaultValue = new MutableString().Append(DefaultValue).ToString();
        return createdStringBearer;
    }
}
