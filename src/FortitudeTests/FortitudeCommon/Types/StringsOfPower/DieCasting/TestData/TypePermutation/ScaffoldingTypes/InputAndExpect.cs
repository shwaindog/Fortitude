// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
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

public static class FormatExpectationExtensions
{
    public static bool IsMatchingScenario(this ScaffoldingStringBuilderInvokeFlags check, ScaffoldingStringBuilderInvokeFlags condition)
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
}

public interface ITypedFormatExpectation<out T> : IFormatExpectation
{
    T Input { get; }
}

public record FieldExpect<T>
    (T? Input, string? FormatString = null, bool HasDefault = false, T? DefaultValue = default, int FromIndex = 0, int Length = int.MaxValue)
    : ITypedFormatExpectation<T?>, IEnumerable<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>>
{
    private readonly List<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>> expectedResults = new();


    public Type InputType => typeof(T);
    public Type CoreType => typeof(T);
    public bool IsNullable => !IsStruct && Input == null;

    public bool IsStruct => InputType.IsValueType;
    
    public bool HasIndexRangeLimiting => FromIndex != 0 || Length != int.MaxValue;

    public string ShortTestName
    {
        get
        {
            {
                var result = new MutableString();
                result.Append(InputType.ShortNameInCSharpFormat());
                if (Input == null)
                {
                    result.Append("=null");
                }
                else
                {
                    result.Append(AsStringDelimiter)
                          .AppendFormat(ICustomStringFormatter.DefaultBufferFormatter, "{0}", Input)
                          .Append(AsStringDelimiter).Append("_").Append(FormatString);
                }
                if (HasDefault)
                {
                    var defaultString = DefaultValue?.ToString() ?? (!InputType.IsValueType ? "null" : "");
                    if (defaultString.IsNotEmpty())
                    {
                        result.Append($"_{defaultString}");
                    }
                }

                return result.ToString();
            }
        }
    }

    private string AsStringDelimiter =>
        InputType.Name switch
        {
            "String"        => "\""
          , "MutableString" => "\""
          , "StringBuilder" => "\""
          , "Char"          => "'"
          , _               => ""
        };


    public string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing = expectedResults[i];
            if (!existing.Key.IsMatchingScenario(condition)) continue;
            return existing.Value;
        }
        return IFormatExpectation.NoResultExpectedValue;
    }

    public IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry)
    {
        var createdStringBearer = scaffoldEntry.CreateStringBearerFunc(InputType)();
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
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<T?> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<string?> supportsStringDefaultValue)
            supportsStringDefaultValue.DefaultValue = new MutableString().Append(DefaultValue).ToString();
        return createdStringBearer;
    }

    public void Add(ScaffoldingStringBuilderInvokeFlags key, string value)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing    = expectedResults[i];
            var existingKey = existing.Key;
            if (!existing.Key.IsMatchingScenario(key)) continue;
            existingKey &= ~key;
            if (existingKey == None)
                expectedResults.RemoveAt(i);
            else
                expectedResults[i] = new KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>(existingKey, existing.Value);
            break;
        }
        expectedResults.Add(new KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>(key, value));
    }

    public void Add(KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string> newExpectedResult)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing    = expectedResults[i];
            var existingKey = existing.Key;
            if (!existing.Key.IsMatchingScenario(newExpectedResult.Key)) continue;
            existingKey &= ~newExpectedResult.Key;
            if (existingKey == None)
                expectedResults.RemoveAt(i);
            else
                expectedResults[i] = new KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>(existingKey, existing.Value);
            break;
        }
        expectedResults.Add(newExpectedResult);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>> GetEnumerator() => expectedResults.GetEnumerator();

    public override string ToString()
    {
        var sb = new MutableString();
        sb.Append(nameof(InputType)).Append(": ").Append(InputType).Append(", ");
        sb.Append(nameof(Input)).Append(": ").Append(AsStringDelimiter).Append(Input).Append(AsStringDelimiter).Append(", ");
        sb.Append(nameof(FormatString)).Append(": ").Append(FormatString != null ? $"\"{FormatString}\"" : "null");
        if (HasDefault)
        {
            sb.Append(", ").Append(nameof(DefaultValue)).Append(": ");
            if (InputType.IsChar() || InputType.IsString() || InputType.IsCharSequence() || InputType.IsStringBuilder())
            {
                sb.Append('"').Append(DefaultValue).Append('"');
            }
            else { sb.Append(DefaultValue); }
        }
        if (FromIndex != 0 || Length != int.MaxValue)
        {
            sb.Append(", ").Append(nameof(FromIndex)).Append(": ").Append(FromIndex).Append(", ");
            sb.Append(nameof(Length)).Append(": ").Append(Length);
        }
        return sb.ToString();
    }
};

public record NullableStructFieldExpect<T>(T? Input, string? FormatString = null, bool HasDefault = false, T DefaultValue = default)
    : ITypedFormatExpectation<T?>, IEnumerable<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>>
    where T : struct
{
    private readonly List<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>> expectedResults = new();

    public Type InputType => typeof(T?);
    public Type CoreType => typeof(T);

    public bool IsNullable => true;

    public bool IsStruct => true;

    public string ShortTestName
    {
        get
        {
            {
                var result = new MutableString();
                result.Append(InputType.ShortNameInCSharpFormat());
                if (Input == null)
                {
                    result.Append("=null");
                }
                else
                {
                    result.Append(AsStringDelimiter)
                          .AppendFormat(ICustomStringFormatter.DefaultBufferFormatter, "{0}", Input)
                          .Append(AsStringDelimiter).Append("_").Append(FormatString);
                }
                if (HasDefault)
                {
                    var defaultString = DefaultValue.ToString()!;
                    if (defaultString.IsNotEmpty())
                    {
                        result.Append($"_{defaultString}");
                    }
                }

                return result.ToString();
            }
        }
    }

    private string AsStringDelimiter =>
        InputType.Name switch
        {
            "String"        => "\""
          , "MutableString" => "\""
          , "StringBuilder" => "\""
          , "Char"          => "'"
          , _               => ""
        };

    public bool HasIndexRangeLimiting => false;

    public string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing = expectedResults[i];
            if (!existing.Key.IsMatchingScenario(condition)) continue;
            return existing.Value;
        }
        return IFormatExpectation.NoResultExpectedValue;
    }

    public IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry)
    {
        var createdStringBearer = scaffoldEntry.ScaffoldingFlags.IsNullableSpanFormattableOnly()
            ? scaffoldEntry.CreateStringBearerFunc(CoreType)()
            : scaffoldEntry.CreateStringBearerFunc(InputType)();
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = Input;
        else
            ((IMoldSupportedValue<T?>)createdStringBearer).Value = Input;
        if (FormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = FormatString;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<T?> supportsNullableDefaultValue)
            supportsNullableDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<T> supportsDefaultValue) supportsDefaultValue.DefaultValue = DefaultValue;
        return createdStringBearer;
    }

    public void Add(ScaffoldingStringBuilderInvokeFlags key, string value)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing    = expectedResults[i];
            var existingKey = existing.Key;
            if (!existing.Key.IsMatchingScenario(key)) continue;
            existingKey &= ~key;
            if (existingKey == None)
                expectedResults.RemoveAt(i);
            else
                expectedResults[i] = new KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>(existingKey, existing.Value);
            break;
        }
        expectedResults.Add(new KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>(key, value));
    }

    public void Add(KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string> newExpectedResult)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing    = expectedResults[i];
            var existingKey = existing.Key;
            if (!existing.Key.IsMatchingScenario(newExpectedResult.Key)) continue;
            existingKey &= ~newExpectedResult.Key;
            if (existingKey == None)
                expectedResults.RemoveAt(i);
            else
                expectedResults[i] = new KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>(existingKey, existing.Value);
            break;
        }
        expectedResults.Add(newExpectedResult);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>> GetEnumerator() => expectedResults.GetEnumerator();
    

    public override string ToString()
    {
        var sb = new MutableString();
        sb.Append(nameof(InputType)).Append(": ").Append(InputType).Append(", ");
        sb.Append(nameof(Input)).Append(": ").Append(AsStringDelimiter).Append(Input).Append(AsStringDelimiter).Append(", ");
        sb.Append(nameof(FormatString)).Append(": ").Append(FormatString != null ? $"\"{FormatString}\"" : "null");
        if (HasDefault)
        {
            sb.Append(", ").Append(nameof(DefaultValue)).Append(": ");
            if (InputType.IsChar() || InputType.IsString() || InputType.IsCharSequence() || InputType.IsStringBuilder())
            {
                sb.Append('"').Append(DefaultValue).Append('"');
            }
            else { sb.Append(DefaultValue); }
        }
        return sb.ToString();
    }
}
