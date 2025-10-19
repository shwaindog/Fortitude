// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
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

    string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition);

    IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry);
}

public interface IComplexFieldFormatExpectation : IFormatExpectation
{
    BuildExpectedOutput WhenValueExpectedOutput { get; set; }
}

public interface ITypedFormatExpectation<out T> : IFormatExpectation
{
    T? Input { get; }

    void ClearExpectations();

    void Add(ScaffoldingStringBuilderInvokeFlags key, string value);
    void Add(KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string> newExpectedResult);
}

public abstract class FieldExpectBase<T> : ITypedFormatExpectation<T>, IEnumerable<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>>
{
    protected readonly List<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>> ExpectedResults = new();

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

    public virtual string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition)
    {
        for (var i = 0; i < ExpectedResults.Count; i++)
        {
            var existing = ExpectedResults[i];
            if (!IsMatchingScenario(existing.Key, condition)) continue;
            return existing.Value;
        }
        return IFormatExpectation.NoResultExpectedValue;
    }

    protected bool IsMatchingScenario(ScaffoldingStringBuilderInvokeFlags check, ScaffoldingStringBuilderInvokeFlags condition)
    {
        var meetsWriteCondition            = (check & OutputConditionMask & condition) > 0;
        var hasMatchingInputType           = (check & AcceptsAnyGeneric & condition).HasAnyOf(check & AcceptsAnyGeneric);
        var conditionIsSubSpanOnlyCallType = (condition & SubSpanCallMask) > 0;
        var meetsInputTypeCondition        = (hasMatchingInputType && !conditionIsSubSpanOnlyCallType);
        var isSameSubSpanCalType =
            ((condition & SubSpanCallMask) & (check & SubSpanCallMask)) == (condition & SubSpanCallMask);
        var checkIsSubSpanOnlyCallType = (check & SubSpanCallMask) > 0;
        var meetSubSpanCallType        = (conditionIsSubSpanOnlyCallType && checkIsSubSpanOnlyCallType && isSameSubSpanCalType);
        return meetsWriteCondition && (meetsInputTypeCondition || (meetSubSpanCallType));
    }

    public void Add(ScaffoldingStringBuilderInvokeFlags key, string value)
    {
        for (var i = 0; i < ExpectedResults.Count; i++)
        {
            var existing    = ExpectedResults[i];
            var existingKey = existing.Key;
            if (!IsMatchingScenario(existing.Key, key)) continue;
            existingKey &= ~key;
            if (existingKey == None)
                ExpectedResults.RemoveAt(i);
            else
                ExpectedResults[i] = new KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>(existingKey, existing.Value);
            break;
        }
        ExpectedResults.Add(new KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>(key, value));
    }

    public void Add(KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string> newExpectedResult)
    {
        for (var i = 0; i < ExpectedResults.Count; i++)
        {
            var existing    = ExpectedResults[i];
            var existingKey = existing.Key;
            if (!IsMatchingScenario(existing.Key, newExpectedResult.Key)) continue;
            existingKey &= ~newExpectedResult.Key;
            if (existingKey == None)
                ExpectedResults.RemoveAt(i);
            else
                ExpectedResults[i] = new KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>(existingKey, existing.Value);
            break;
        }
        ExpectedResults.Add(newExpectedResult);
    }

    public void ClearExpectations()
    {
        ExpectedResults.Clear();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>> GetEnumerator() => ExpectedResults.GetEnumerator();

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

    public virtual bool IsNullable => InputType.IsNullable();
    
    public TChildScaffold RevealerScaffold { get; set; }

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        if (Input is string || Input is char[] || Input is ICharSequence || Input is StringBuilder)
        {
            condition |= AcceptsChars | AcceptsString | AcceptsCharArray | AcceptsCharSequence | AcceptsStringBuilder;
        }
        var expectValue = FieldValueExpectation.GetExpectedOutputFor(condition);
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
        RevealerScaffold        = calledScaffoldingPart.CreateTypedStringBearerFunc<TChildScaffold>()();
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

    public virtual bool IsNullable => InputType.IsNullable();

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        condition |= AcceptsSpanFormattable | AcceptsChars | AcceptsString;
        var expectValue = FieldValueExpectation.GetExpectedOutputFor(condition);
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
