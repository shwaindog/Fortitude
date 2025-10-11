// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections;
using System.Net;
using System.Numerics;
using System.Text;
using FortitudeCommon.Types.StringsOfPower;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public interface IFormatExpectation
{
    Type InputType { get; }
    Type SpanFormattableType { get; }

    bool IsNullableStruct { get; }
    string? FormatString { get; }

    bool HasDefault { get; }

    string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition);

    IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry);
}

public interface ITypedFormatExpectation<out T> : IFormatExpectation
{
    T Input { get; }
}

public record FmtExpect<T>
    (T? Input, string? FormatString = null, bool HasDefault = false, T? DefaultValue = default)
    : ITypedFormatExpectation<T?>, IEnumerable<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>> where T : ISpanFormattable
{
    private readonly List<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>> expectedResults = new();

    public string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing = expectedResults[i];
            if ((existing.Key & condition) == 0) continue;
            return existing.Value;
        }
        return string.Empty;
    }

    public IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry)
    {
        var createdStringBearer = scaffoldEntry.CreateStringBearerFunc(InputType)();
        if (createdStringBearer is IMoldSupportedValue<object?> isObjectMold)
            isObjectMold.Value = Input;
        else
            ((IMoldSupportedValue<T?>)createdStringBearer).Value = Input;
        if (FormatString != null && createdStringBearer is ISupportsValueFormatString supportsValueFormatString)
            supportsValueFormatString.ValueFormatString = FormatString;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<object?> supportsObjectDefaultValue)
            supportsObjectDefaultValue.DefaultValue = DefaultValue;
        if (HasDefault && createdStringBearer is IMoldSupportedDefaultValue<T?> supportsDefaultValue)
            supportsDefaultValue.DefaultValue = DefaultValue;
        return createdStringBearer;
    }

    public Type InputType => typeof(T);
    public Type SpanFormattableType => typeof(T);
    public bool IsNullableStruct => false;

    public void Add(ScaffoldingStringBuilderInvokeFlags key, string value)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing    = expectedResults[i];
            var existingKey = existing.Key;
            if ((existingKey & key) == 0) continue;
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
            if ((existingKey & newExpectedResult.Key) == 0) continue;
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
};

public record NullFmtStructExpect<T>(T? Input, string? FormatString = null, bool HasDefault = false, T DefaultValue = default)
    : ITypedFormatExpectation<T?>, IEnumerable<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>>
    where T : struct, ISpanFormattable
{
    private readonly List<KeyValuePair<ScaffoldingStringBuilderInvokeFlags, string>> expectedResults = new();

    public string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing = expectedResults[i];
            if ((existing.Key & condition) == 0) continue;
            return existing.Value;
        }
        return string.Empty;
    }

    public IStringBearer CreateStringBearerWithValueFor(ScaffoldingPartEntry scaffoldEntry)
    {
        var createdStringBearer = scaffoldEntry.ScaffoldingFlags.IsNullableSpanFormattableOnly()
            ? scaffoldEntry.CreateStringBearerFunc(SpanFormattableType)()
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

    public Type InputType => typeof(T?);
    public Type SpanFormattableType => typeof(T);

    public bool IsNullableStruct => true;

    public void Add(ScaffoldingStringBuilderInvokeFlags key, string value)
    {
        for (var i = 0; i < expectedResults.Count; i++)
        {
            var existing    = expectedResults[i];
            var existingKey = existing.Key;
            if ((existingKey & key) == 0) continue;
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
            if ((existingKey & newExpectedResult.Key) == 0) continue;
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
}

public static class SpanFormattableTestData
{
    public static readonly IFormatExpectation[] AllSpanFormattableExpectations =
    [
        // byte
        new FmtExpect<byte>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<byte>(255) { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "255" } }
      , new FmtExpect<byte>(128, "C2") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "$128.00" } }
      , new FmtExpect<byte>(77, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"77                  \"" }
        }
      , new FmtExpect<byte>(255, "{0[..1]}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2" } }
      , new FmtExpect<byte>(255, "{0[1..2]}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "5" } }
      , new FmtExpect<byte>(255, "{0[1..]}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "55" } }
      
        // byte?
      , new NullFmtStructExpect<byte>(0, "{0}") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<byte>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<byte>(255) { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "255" } }
      , new NullFmtStructExpect<byte>(128, "C2")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "$128.00" }
        }
      , new NullFmtStructExpect<byte>(144, "\"{0,20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                 144\"" }
        }
      , new NullFmtStructExpect<byte>(255, "{0[..1]}")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2" }
        }
      , new NullFmtStructExpect<byte>(255, "{0[1..2]}")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "5" }
        }
      , new NullFmtStructExpect<byte>(255, "{0[1..]}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "55" } }
      
        // char
      , new FmtExpect<char>('\0', "") { { AlwaysWrites | NonNullWrites, "\0" } }
      , new FmtExpect<char>('A') { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "A" } }
      , new FmtExpect<char>(' ', "'{0}'") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "' '" } }
      , new FmtExpect<char>('z', "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"z                   \"" }
        }
      
        // char?
      , new NullFmtStructExpect<char>('\0', "") { { AlwaysWrites | NonNullWrites, "\0" } }
      , new NullFmtStructExpect<char>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<char>('A') { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "A" } }
      , new NullFmtStructExpect<char>(' ', "'{0}'") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "' '" } }
      , new NullFmtStructExpect<char>('z', "\"{0,20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                   z\"" }
        }
      
        // short
      , new FmtExpect<short>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<short>(32000, "N2") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "32,000.00" } }
      , new FmtExpect<short>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new FmtExpect<short>(-16328, "'{0}'") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-16328'" } }
      , new FmtExpect<short>(55, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }
      
        // short?
      , new NullFmtStructExpect<short>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<short>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<short>(32000, "N2") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "32,000.00" } }
      , new NullFmtStructExpect<short>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new NullFmtStructExpect<short>(-16328, "'{0}'") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-16328'" } }
      , new NullFmtStructExpect<short>(55, "\"{0,20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }
      
        // ushort
      , new FmtExpect<ushort>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<ushort>(32000, "N2") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "32,000.00" } }
      , new FmtExpect<ushort>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new FmtExpect<ushort>(ushort.MaxValue, "'{0:B16}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1111111111111111'" } }
      , new FmtExpect<ushort>(55, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }
      
        // ushort?
      , new NullFmtStructExpect<ushort>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<ushort>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<ushort>(32000, "N2") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "32,000.00" } }
      , new NullFmtStructExpect<ushort>(32, "C8", true, 32) { { AlwaysWrites | NonNullWrites, "$32.00000000" } }
      , new NullFmtStructExpect<ushort>(ushort.MaxValue, "'{0:B16}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1111111111111111'" } }
      , new NullFmtStructExpect<ushort>(55, "\"{0,20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }
      
        // Half
      , new FmtExpect<Half>(Half.Zero) { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<Half>(Half.MinValue / (Half)2.0, "R")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "-32750" }
        }
      , new FmtExpect<Half>(Half.One, "", true, Half.One) { { AlwaysWrites | NonNullWrites, "1" } }
      , new FmtExpect<Half>(Half.NaN, "", true, Half.NaN) { { AlwaysWrites | NonNullWrites, "NaN" } }
      , new FmtExpect<Half>(Half.NaN, "\"{0}\"") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new FmtExpect<Half>(Half.MaxValue, "'{0:G}'") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'65500'" } }
      , new FmtExpect<Half>(Half.MinValue, "'{0:c}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-$65,504.00'" } }
      , new FmtExpect<Half>((Half)(Math.E * 10.0), "N0")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "27" }
        }
      , new FmtExpect<float>((float)Math.PI, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.1415927           \"" }
        }
      
        // Half?
      , new NullFmtStructExpect<Half>(Half.Zero) { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<Half>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<Half>(Half.MinValue / (Half)2.0, "R")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "-32750" }
        }
      , new NullFmtStructExpect<Half>(Half.One, "", true, Half.One) { { AlwaysWrites | NonNullWrites, "1" } }
      , new NullFmtStructExpect<Half>(Half.NaN, "", true, Half.NaN) { { AlwaysWrites | NonNullWrites, "NaN" } }
      , new NullFmtStructExpect<Half>(Half.NaN, "\"{0}\"")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new NullFmtStructExpect<Half>(Half.MaxValue, "'{0:G}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'65500'" } }
      , new NullFmtStructExpect<Half>(Half.MinValue, "'{0:c}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-$65,504.00'" } }
      , new NullFmtStructExpect<Half>((Half)(Math.E * 10.0), "N0")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "27" }
        }
      , new NullFmtStructExpect<float>((float)Math.PI, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.1415927           \"" }
        }
      
        // int
      , new FmtExpect<int>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<int>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FmtExpect<int>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new FmtExpect<int>(int.MaxValue, "'{0:X8}'") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFF'" } }
      , new FmtExpect<int>(int.MinValue, "'{0:X9}'") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'080000000'" } }
      , new FmtExpect<int>(55, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }
      
        // int?
      , new NullFmtStructExpect<int>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<int>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<int>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new NullFmtStructExpect<int>(32, "C8", true, 32) { { AlwaysWrites | NonNullWrites, "$32.00000000" } }
      , new NullFmtStructExpect<int>(int.MaxValue, "'{0:X8}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFF'" } }
      , new NullFmtStructExpect<int>(int.MinValue, "'{0:X9}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'080000000'" } }
      , new NullFmtStructExpect<int>(55, "\"{0,20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }
      
        // uint
      , new FmtExpect<uint>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<uint>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FmtExpect<uint>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new FmtExpect<uint>(uint.MaxValue, "'{0:X8}'") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFF'" } }
      , new FmtExpect<uint>(uint.MinValue, "'{0:X9}'", true, 100)
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'000000000'" } }
      , new FmtExpect<uint>(55, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }
      
        // uint?
      , new NullFmtStructExpect<uint>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<uint>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<uint>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new NullFmtStructExpect<uint>(32, "C8", true, 32) { { AlwaysWrites | NonNullWrites, "$32.00000000" } }
      , new NullFmtStructExpect<uint>(uint.MaxValue, "'{0:X8}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFF'" } }
      , new NullFmtStructExpect<uint>(uint.MinValue, "'{0:X9}'", true, 100)
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'000000000'" } }
      , new NullFmtStructExpect<uint>(55, "\"{0,20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }
      
        // float
      , new FmtExpect<float>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<float>(1 - float.MinValue, "R")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "3.4028235E+38" } }
      , new FmtExpect<float>(1, "", true, 1) { { AlwaysWrites | NonNullWrites, "1" } }
      , new FmtExpect<float>(float.NaN, "", true, float.NaN) { { AlwaysWrites | NonNullWrites, "NaN" } }
      , new FmtExpect<float>(float.NaN, "\"{0}\"") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new FmtExpect<float>(float.MaxValue, "'{0:G}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'3.4028235E+38'" } }
      , new FmtExpect<float>(float.MinValue, "'{0:c}'")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'"
            }
        }
      , new FmtExpect<float>((float)Math.E * 1_000_000, "N0")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new FmtExpect<float>((float)Math.PI, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.1415927           \"" }
        }
      
        // float?
      , new NullFmtStructExpect<float>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<float>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<float>(1 - float.MinValue, "R")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "3.4028235E+38" } }
      , new NullFmtStructExpect<float>(1, "", true, 1) { { AlwaysWrites | NonNullWrites, "1" } }
      , new NullFmtStructExpect<float>(float.NaN, "", true, float.NaN) { { AlwaysWrites | NonNullWrites, "NaN" } }
      , new NullFmtStructExpect<float>(float.NaN, "\"{0}\"")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new NullFmtStructExpect<float>(float.MaxValue, "'{0:G}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'3.4028235E+38'" } }
      , new NullFmtStructExpect<float>(float.MinValue, "'{0:c}'")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$340,282,346,638,528,859,811,704,183,484,516,925,440.00'"
            }
        }
      , new NullFmtStructExpect<float>((float)Math.E * 1_000_000, "N0")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new NullFmtStructExpect<float>((float)Math.PI, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.1415927           \"" }
        }
      
        // long
      , new FmtExpect<long>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<long>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FmtExpect<long>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new FmtExpect<long>(long.MaxValue, "'{0:X16}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFFFFFFFFFF'" } }
      , new FmtExpect<long>(long.MinValue, "'{0:X17}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'08000000000000000'" } }
      , new FmtExpect<long>(55, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }
      
        // long?
      , new NullFmtStructExpect<long>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<long>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<long>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new NullFmtStructExpect<long>(32, "C8", true, 32) { { AlwaysWrites | NonNullWrites, "$32.00000000" } }
      , new NullFmtStructExpect<long>(long.MaxValue, "'{0:X16}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFFFFFFFFFF'" } }
      , new NullFmtStructExpect<long>(long.MinValue, "'{0:X17}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'08000000000000000'" } }
      , new NullFmtStructExpect<long>(55, "\"{0,20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }
      
        // ulong
      , new FmtExpect<ulong>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<ulong>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FmtExpect<ulong>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new FmtExpect<ulong>(ulong.MaxValue, "'{0:X16}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFFFFFFFFFF'" } }
      , new FmtExpect<ulong>(ulong.MinValue, "'{0:X17}'", true, 100)
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'00000000000000000'" } }
      , new FmtExpect<ulong>(55, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"55                  \"" }
        }
      
        // ulong?
      , new NullFmtStructExpect<ulong>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<ulong>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<ulong>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new NullFmtStructExpect<ulong>(32, "C8", true, 32) { { AlwaysWrites | NonNullWrites, "$32.00000000" } }
      , new NullFmtStructExpect<ulong>(ulong.MaxValue, "'{0:X16}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFFFFFFFFFF'" } }
      , new NullFmtStructExpect<ulong>(ulong.MinValue, "'{0:X17}'", true, 100)
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'00000000000000000'" } }
      , new NullFmtStructExpect<ulong>(55, "\"{0,20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                  55\"" }
        }
      
        // double
      , new FmtExpect<double>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<double>(1 - double.MinValue, "R")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.7976931348623157E+308" } }
      , new FmtExpect<double>(1, "", true, 1) { { AlwaysWrites | NonNullWrites, "1" } }
      , new FmtExpect<double>(double.NaN, "", true, double.NaN) { { AlwaysWrites | NonNullWrites, "NaN" } }
      , new FmtExpect<double>(double.NaN, "\"{0}\"") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new FmtExpect<double>(double.MaxValue, "'{0:G}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1.7976931348623157E+308'" } }
      , new FmtExpect<double>(double.MinValue, "'{0:c}'")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.00'"
            }
        }
      , new FmtExpect<double>(Math.E * 1_000_000, "N0")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new FmtExpect<double>(Math.PI, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.141592653589793   \"" }
        }
      
        // double?
      , new NullFmtStructExpect<double>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<double>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<double>(1 - double.MinValue, "R")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.7976931348623157E+308" } }
      , new NullFmtStructExpect<double>(1, "", true, 1) { { AlwaysWrites | NonNullWrites, "1" } }
      , new NullFmtStructExpect<double>(double.NaN, "", true, double.NaN) { { AlwaysWrites | NonNullWrites, "NaN" } }
      , new NullFmtStructExpect<double>(double.NaN, "\"{0}\"")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"NaN\"" } }
      , new NullFmtStructExpect<double>(double.MaxValue, "'{0:G}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1.7976931348623157E+308'" } }
      , new NullFmtStructExpect<double>(double.MinValue, "'{0:c}'")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'-$179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.00'"
            }
        }
      , new NullFmtStructExpect<double>(Math.E * 1_000_000, "N0")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new NullFmtStructExpect<double>(Math.PI, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.141592653589793   \"" }
        }
      
        // decimal
      , new FmtExpect<decimal>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<decimal>(decimal.MinValue, "R")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "-79228162514264337593543950335" } }
      , new FmtExpect<decimal>(1, "", true, 1) { { AlwaysWrites | NonNullWrites, "1" } }
      , new FmtExpect<decimal>(decimal.MaxValue, "'{0:G}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'79228162514264337593543950335'" } }
      , new FmtExpect<decimal>(decimal.MinValue, "'{0:c}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-$79,228,162,514,264,337,593,543,950,335.00'" } }
      , new FmtExpect<decimal>((decimal)Math.E * 1_000_000, "N0")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new FmtExpect<decimal>((decimal)Math.PI, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.14159265358979    \"" }
        }
      
        // decimal?
      , new NullFmtStructExpect<decimal>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<decimal>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<decimal>(decimal.MinValue, "R")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "-79228162514264337593543950335" } }
      , new NullFmtStructExpect<decimal>(1, "", true, 1) { { AlwaysWrites | NonNullWrites, "1" } }
      , new NullFmtStructExpect<decimal>(decimal.MaxValue, "'{0:G}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'79228162514264337593543950335'" } }
      , new NullFmtStructExpect<decimal>(decimal.MinValue, "'{0:c}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'-$79,228,162,514,264,337,593,543,950,335.00'" } }
      , new NullFmtStructExpect<decimal>((decimal)Math.E * 1_000_000, "N0")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2,718,282" }
        }
      , new NullFmtStructExpect<decimal>((decimal)Math.PI, "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"3.14159265358979    \"" }
        }
      
        // Int128
      , new FmtExpect<Int128>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<Int128>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FmtExpect<Int128>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new FmtExpect<Int128>(Int128.MaxValue, "'{0:X32}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'" } }
      , new FmtExpect<Int128>(Int128.MinValue, "'{0:X33}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'080000000000000000000000000000000'" } }
      , new FmtExpect<Int128>(Int128.MaxValue, "\"{0,-52:N0}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \"" }
        }
      
        // Int128?
      , new NullFmtStructExpect<Int128>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<Int128>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<Int128>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new NullFmtStructExpect<Int128>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new NullFmtStructExpect<Int128>(Int128.MaxValue, "'{0:X32}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'7FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'" } }
      , new NullFmtStructExpect<Int128>(Int128.MinValue, "'{0:X33}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'080000000000000000000000000000000'" } }
      , new NullFmtStructExpect<Int128>(Int128.MaxValue, "\"{0,-52:N0}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"170,141,183,460,469,231,731,687,303,715,884,105,727 \"" }
        }
      
        // UInt128
      , new FmtExpect<UInt128>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<UInt128>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FmtExpect<UInt128>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new FmtExpect<UInt128>(UInt128.MaxValue, "'{0:X32}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'" } }
      , new FmtExpect<UInt128>(UInt128.MinValue, "'{0:X33}'", true, (UInt128)100)
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'000000000000000000000000000000000'" } }
      , new FmtExpect<UInt128>(UInt128.MaxValue, "\"{0,-52:N0}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \"" }
        }
      
        // UInt128?
      , new NullFmtStructExpect<UInt128>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<UInt128>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<UInt128>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new NullFmtStructExpect<UInt128>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new NullFmtStructExpect<UInt128>(UInt128.MaxValue, "'{0:X32}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF'" } }
      , new NullFmtStructExpect<UInt128>(UInt128.MinValue, "'{0:X33}'", true, (UInt128)100)
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'000000000000000000000000000000000'" } }
      , new NullFmtStructExpect<UInt128>(UInt128.MaxValue, "\"{0,-52:N0}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"340,282,366,920,938,463,463,374,607,431,768,211,455 \"" }
        }
      
        // BigInteger
      , new FmtExpect<BigInteger>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new FmtExpect<BigInteger>(32000, "0x{0:X}") { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new FmtExpect<BigInteger>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new FmtExpect<BigInteger>(UInt128.MaxValue * (BigInteger)50, "'{0:X32}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'" } }
      , new FmtExpect<BigInteger>(Int128.MinValue * (BigInteger)50, "'{0:X33}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'E700000000000000000000000000000000'" } }
      , new FmtExpect<BigInteger>(UInt128.MaxValue * (BigInteger)100, "\"{0,-56:N0}\"")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
            }
        }
      
        // BigInteger?
      , new NullFmtStructExpect<BigInteger>(0, "") { { AlwaysWrites | NonNullWrites, "0" } }
      , new NullFmtStructExpect<BigInteger>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<BigInteger>(32000, "0x{0:X}")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0x7D00" } }
      , new NullFmtStructExpect<BigInteger>(32, "C0", true, 32) { { AlwaysWrites | NonNullWrites, "$32" } }
      , new NullFmtStructExpect<BigInteger>(UInt128.MaxValue * (BigInteger)50, "'{0:X32}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'31FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFCE'" }
        }
      , new NullFmtStructExpect<BigInteger>(Int128.MinValue * (BigInteger)50, "'{0:X33}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'E700000000000000000000000000000000'" }
        }
      , new NullFmtStructExpect<BigInteger>(UInt128.MaxValue * (BigInteger)100, "\"{0,-56:N0}\"")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"34,028,236,692,093,846,346,337,460,743,176,821,145,500  \""
            }
        }
      
        // Complex
      , new FmtExpect<Complex>(0, "") { { AlwaysWrites | NonNullWrites, "<0; 0>" } }
      , new FmtExpect<Complex>(32000, "{0:N0}")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "<32,000; 0>" }
        }
      , new FmtExpect<Complex>(new Complex(32.0d, 1), "N0", true, new Complex(32.0d, 1))
        {
            { AlwaysWrites | NonNullWrites, "<32; 1>" }
        }
      , new FmtExpect<Complex>(new Complex(999999.999, 999999.999), "'{0:N2}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'<1,000,000.00; 1,000,000.00>'" } }
      , new FmtExpect<Complex>(new Complex(double.MinValue, double.MinValue), "'{0:N9}'")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632" +
                ",766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389" +
                ",328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299" +
                ",881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476" +
                ",803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986" +
                ",049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797" +
                ",826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'"
            }
        }
      , new FmtExpect<Complex>(new Complex(Math.PI, Math.E), "\"{0-20}\"")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"<3.141592653589793; 2.718281828459045>\""
            }
        }
      
        // Complex?
      , new NullFmtStructExpect<Complex>(0, "") { { AlwaysWrites | NonNullWrites, "<0; 0>" } }
      , new NullFmtStructExpect<Complex>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<Complex>(32000, "{0:N0}")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "<32,000; 0>" }
        }
      , new NullFmtStructExpect<Complex>(new Complex(32.0d, 1), "N0", true, new Complex(32.0d, 1))
        {
            { AlwaysWrites | NonNullWrites, "<32; 1>" }
        }
      , new NullFmtStructExpect<Complex>(new Complex(999999.999, 999999.999), "'{0:N2}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'<1,000,000.00; 1,000,000.00>'" } }
      , new NullFmtStructExpect<Complex>(new Complex(double.MinValue, double.MinValue), "'{0:N9}'")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "'<-179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476,803,157,260,780,028,538,760,589,558,632" +
                ",766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986,049,910,576,551,282,076,245,490,090,389" +
                ",328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797,826,204,144,723,168,738,177,180,919,299" +
                ",881,250,404,026,184,124,858,368.000000000; -179,769,313,486,231,570,814,527,423,731,704,356,798,070,567,525,844,996,598,917,476" +
                ",803,157,260,780,028,538,760,589,558,632,766,878,171,540,458,953,514,382,464,234,321,326,889,464,182,768,467,546,703,537,516,986" +
                ",049,910,576,551,282,076,245,490,090,389,328,944,075,868,508,455,133,942,304,583,236,903,222,948,165,808,559,332,123,348,274,797" +
                ",826,204,144,723,168,738,177,180,919,299,881,250,404,026,184,124,858,368.000000000>'"
            }
        }
      , new NullFmtStructExpect<Complex>(new Complex(Math.PI, Math.E), "\"{0-20}\"")
        {
            {
                AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites
              , "\"<3.141592653589793; 2.718281828459045>\""
            }
        }
      
        // DateTime
      , new FmtExpect<DateTime>(DateTime.MinValue, "O")
            { { AlwaysWrites | NonNullWrites, "0001-01-01T00:00:00.0000000" } }
      , new FmtExpect<DateTime>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01T01:01:01.1111111" } }
      , new FmtExpect<DateTime>(new DateTime(2020, 2, 2)
                                    .AddTicks(2222222), "s", true
                              , new DateTime(2020, 2, 2).AddTicks(2222222))
            { { AlwaysWrites | NonNullWrites, "2020-02-02T00:00:00" } }
      , new FmtExpect<DateTime>(DateTime.MaxValue, "'{0:u}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31 23:59:59Z'" } }
      , new FmtExpect<DateTime>(DateTime.MinValue, "\"{0,30:u}\"", true, new DateTime(2020, 1, 1))
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"          0001-01-01 00:00:00Z\"" } }
      , new FmtExpect<DateTime>(new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980-07-31 11:48:13'" }
        }
      , new FmtExpect<DateTime>(new DateTime(2009, 11, 12, 19, 49, 0), "\"{0,-30:O}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"2009-11-12T19:49:00.0000000   \"" }
        }
      
        // DateTime?
      , new NullFmtStructExpect<DateTime>(DateTime.MinValue, "O")
            { { AlwaysWrites | NonNullWrites, "0001-01-01T00:00:00.0000000" } }
      , new NullFmtStructExpect<DateTime>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<DateTime>(new DateTime(2000, 1, 1, 1, 1, 1).AddTicks(1111111), "o")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01T01:01:01.1111111" } }
      , new NullFmtStructExpect<DateTime>(new DateTime(2020, 2, 2)
                                              .AddTicks(2222222), "s", true
                                        , new DateTime(2020, 2, 2).AddTicks(2222222))
            { { AlwaysWrites | NonNullWrites, "2020-02-02T00:00:00" } }
      , new NullFmtStructExpect<DateTime>(DateTime.MaxValue, "'{0:u}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31 23:59:59Z'" } }
      , new NullFmtStructExpect<DateTime>(DateTime.MinValue, "\"{0,30:u}\"", true, new DateTime(2020, 1, 1))
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"          0001-01-01 00:00:00Z\"" } }
      , new NullFmtStructExpect<DateTime>(new DateTime(1980, 7, 31, 11, 48, 13), "'{0:yyyy-MM-dd HH:mm:ss}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980-07-31 11:48:13'" }
        }
      , new NullFmtStructExpect<DateTime>(new DateTime(2009, 11, 12, 19, 49, 0), "\"{0,-30:O}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"2009-11-12T19:49:00.0000000   \"" }
        }
      
        // TimeSpan
      , new FmtExpect<TimeSpan>(TimeSpan.Zero, "g") { { AlwaysWrites | NonNullWrites, "0:00:00" } }
      , new FmtExpect<TimeSpan>(new TimeSpan(1, 1, 1, 1, 111, 111), "c")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.01:01:01.1111110" } }
      , new FmtExpect<TimeSpan>(new TimeSpan(-2, -22, -22, -22, -222, -222), "G", true
                              , new TimeSpan(-2, -22, -22, -22, -222, -222))
            { { AlwaysWrites | NonNullWrites, "-2:22:22:22.2222220" } }
      , new FmtExpect<TimeSpan>(TimeSpan.MaxValue, "'{0:G}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'10675199:02:48:05.4775807'" } }
      , new FmtExpect<TimeSpan>(TimeSpan.MinValue, "\"{0,30:c}\"", true, TimeSpan.Zero)
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"    -10675199.02:48:05.4775808\"" } }
      , new FmtExpect<TimeSpan>(new TimeSpan(3, 3, 33, 33, 333, 333),
                                "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-03-33-33.333'" }
        }
      , new FmtExpect<TimeSpan>(new TimeSpan(-4, -4, -44, -44, -444, -444), "\"{0,-30:G}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"-4:04:44:44.4444440           \"" }
        }
      
        // TimeSpan?
      , new NullFmtStructExpect<TimeSpan>(TimeSpan.Zero, "g") { { AlwaysWrites | NonNullWrites, "0:00:00" } }
      , new NullFmtStructExpect<TimeSpan>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<TimeSpan>(new TimeSpan(1, 1, 1, 1, 111, 111), "c")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.01:01:01.1111110" } }
      , new NullFmtStructExpect<TimeSpan>(new TimeSpan(-2, -22, -22, -22, -222, -222), "G", true
                                        , new TimeSpan(-2, -22, -22, -22, -222, -222))
            { { AlwaysWrites | NonNullWrites, "-2:22:22:22.2222220" } }
      , new NullFmtStructExpect<TimeSpan>(TimeSpan.MaxValue, "'{0:G}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'10675199:02:48:05.4775807'" } }
      , new NullFmtStructExpect<TimeSpan>(TimeSpan.MinValue, "\"{0,30:c}\"", true, TimeSpan.Zero)
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"    -10675199.02:48:05.4775808\"" } }
      , new NullFmtStructExpect<TimeSpan>(new TimeSpan(3, 3, 33, 33, 333, 333),
                                          "'{0:dd\\-hh\\-mm\\-ss\\.fff}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-03-33-33.333'" }
        }
      , new NullFmtStructExpect<TimeSpan>(new TimeSpan(-4, -4, -44, -44, -444, -444)
                                        , "\"{0,-30:G}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"-4:04:44:44.4444440           \"" }
        }
      
        // DateOnly
      , new FmtExpect<DateOnly>(DateOnly.MinValue, "o")
            { { AlwaysWrites | NonNullWrites, "0001-01-01" } }
      , new FmtExpect<DateOnly>(new DateOnly(2000, 1, 1), "o")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01" } }
      , new FmtExpect<DateOnly>(new DateOnly(2020, 2, 2), "o", true
                              , new DateOnly(2020, 2, 2))
            { { AlwaysWrites | NonNullWrites, "2020-02-02" } }
      , new FmtExpect<DateOnly>(DateOnly.MaxValue, "'{0:o}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31'" } }
      , new FmtExpect<DateOnly>(DateOnly.MinValue, "\"{0,30:o}\"", true
                              , new DateOnly(2020, 1, 1))
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                    0001-01-01\"" } }
      , new FmtExpect<DateOnly>(new DateOnly(1980, 7, 31), "'{0:yyyy\\\\MM\\\\dd}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980\\07\\31'" }
        }
      , new FmtExpect<DateOnly>(new DateOnly(2009, 11, 12), "\"{0,-30:o}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"2009-11-12                    \"" }
        }
      
        // DateOnly?
      , new NullFmtStructExpect<DateOnly>(DateOnly.MinValue, "o")
            { { AlwaysWrites | NonNullWrites, "0001-01-01" } }
      , new NullFmtStructExpect<DateOnly>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<DateOnly>(new DateOnly(2000, 1, 1), "o")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "2000-01-01" } }
      , new NullFmtStructExpect<DateOnly>(new DateOnly(2020, 2, 2), "o", true
                                        , new DateOnly(2020, 2, 2))
            { { AlwaysWrites | NonNullWrites, "2020-02-02" } }
      , new NullFmtStructExpect<DateOnly>(DateOnly.MaxValue, "'{0:o}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'9999-12-31'" } }
      , new NullFmtStructExpect<DateOnly>(DateOnly.MinValue, "\"{0,30:o}\"", true, new DateOnly(2020, 1, 1))
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                    0001-01-01\"" } }
      , new NullFmtStructExpect<DateOnly>(new DateOnly(1980, 7, 31), "'{0:yyyy\\\\MM\\\\dd}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1980\\07\\31'" }
        }
      , new NullFmtStructExpect<DateOnly>(new DateOnly(2009, 11, 12), "\"{0,-30:o}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"2009-11-12                    \"" }
        }
      
        // TimeOnly
      , new FmtExpect<TimeOnly>(TimeOnly.FromTimeSpan(TimeSpan.Zero), "r") { { AlwaysWrites | NonNullWrites, "00:00:00" } }
      , new FmtExpect<TimeOnly>(new TimeOnly(1, 1, 1, 111, 111), "o")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "01:01:01.1111110" } }
      , new FmtExpect<TimeOnly>(new TimeOnly(22, 22, 22, 222, 222), "O", true
                              , new TimeOnly(22, 22, 22, 222, 222))
            { { AlwaysWrites | NonNullWrites, "22:22:22.2222220" } }
      , new FmtExpect<TimeOnly>(TimeOnly.MaxValue, "'{0:o}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'23:59:59.9999999'" } }
      , new FmtExpect<TimeOnly>(TimeOnly.MinValue, "\"{0,30:r}\"", true
                              , TimeOnly.FromTimeSpan(TimeSpan.FromHours(1)))
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                      00:00:00\"" } }
      , new FmtExpect<TimeOnly>(new TimeOnly(3, 33, 33, 333, 333),
                                "'{0:hh\\-mm\\-ss\\.fff}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-33-33.333'" }
        }
      , new FmtExpect<TimeOnly>(new TimeOnly(4, 44, 44, 444, 444), "\"{0,-30:O}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"04:44:44.4444440              \"" }
        }
      
        // TimeOnly?
      , new NullFmtStructExpect<TimeOnly>(TimeOnly.FromTimeSpan(TimeSpan.Zero), "r")
        {
            { AlwaysWrites | NonNullWrites, "00:00:00" }
        }
      , new NullFmtStructExpect<TimeOnly>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<TimeOnly>(new TimeOnly(1, 1, 1, 111, 111), "o")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "01:01:01.1111110" } }
      , new NullFmtStructExpect<TimeOnly>(new TimeOnly(22, 22, 22, 222, 222), "O", true
                                        , new TimeOnly(22, 22, 22, 222, 222))
            { { AlwaysWrites | NonNullWrites, "22:22:22.2222220" } }
      , new NullFmtStructExpect<TimeOnly>(TimeOnly.MaxValue, "'{0:o}'")
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'23:59:59.9999999'" } }
      , new NullFmtStructExpect<TimeOnly>(TimeOnly.MinValue, "\"{0,30:r}\"", true
                                        , TimeOnly.FromTimeSpan(TimeSpan.FromHours(1)))
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"                      00:00:00\"" } }
      , new NullFmtStructExpect<TimeOnly>(new TimeOnly(3, 33, 33, 333, 333),
                                          "'{0:hh\\-mm\\-ss\\.fff}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'03-33-33.333'" }
        }
      , new NullFmtStructExpect<TimeOnly>(new TimeOnly(4, 44, 44, 444, 444), "\"{0,-30:O}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"04:44:44.4444440              \"" }
        }
      
        // Rune
      , new FmtExpect<Rune>(Rune.GetRuneAt("\0", 0)) { { AlwaysWrites | NonNullWrites, "\0" } }
      , new FmtExpect<Rune>(Rune.GetRuneAt("𝄞", 0))
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "𝄞" }
        }
      , new FmtExpect<Rune>(Rune.GetRuneAt("𝄢", 0), "'{0}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'𝄢'" }
        }
      , new FmtExpect<Rune>(Rune.GetRuneAt("𝅘𝅥𝅮", 0), "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"𝅘𝅥𝅮                  \"" }
        }
      
        // Rune?
      , new NullFmtStructExpect<Rune>(Rune.GetRuneAt("\0", 0)) { { AlwaysWrites | NonNullWrites, "\0" } }
      , new NullFmtStructExpect<Rune>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<Rune>(Rune.GetRuneAt("𝄞", 0))
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "𝄞" }
        }
      , new NullFmtStructExpect<Rune>(Rune.GetRuneAt("𝄢", 0), "'{0}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'𝄢'" }
        }
      , new NullFmtStructExpect<Rune>(Rune.GetRuneAt("𝅘𝅥𝅮", 0), "\"{0,-20}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"𝅘𝅥𝅮                  \"" }
        }
      
        // Guid
      , new FmtExpect<Guid>(Guid.Empty) { { AlwaysWrites | NonNullWrites, "00000000-0000-0000-0000-000000000000" } }
      , new FmtExpect<Guid>(Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"))
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "beefca4e-beef-ca4e-beef-c0ffeebabe51" }
        }
      , new FmtExpect<Guid>(Guid.ParseExact("C0FFEEFE-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"), "'{0}'") 
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'" } }
      , new FmtExpect<Guid>(Guid.ParseExact("BEEEEEEF-BEEF-BEEF-BEEF-CAAAAAAAAA4E", "D"), "\"{0,40}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\"" }
        }
      
        // Guid?
      , new NullFmtStructExpect<Guid>(Guid.Empty) { { AlwaysWrites | NonNullWrites, "00000000-0000-0000-0000-000000000000" } }
      , new NullFmtStructExpect<Guid>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<Guid>(Guid.ParseExact("BEEFCA4E-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"))
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "beefca4e-beef-ca4e-beef-c0ffeebabe51" }
        }
      , new NullFmtStructExpect<Guid>(Guid.ParseExact("C0FFEEFE-BEEF-CA4E-BEEF-C0FFEEBABE51", "D"), "'{0}'") 
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'c0ffeefe-beef-ca4e-beef-c0ffeebabe51'" } }
      , new NullFmtStructExpect<Guid>(Guid.ParseExact("BEEEEEEF-BEEF-BEEF-BEEF-CAAAAAAAAA4E", "D"), "\"{0,40}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"    beeeeeef-beef-beef-beef-caaaaaaaaa4e\"" }
        }
      
        // IPNetwork
      , new FmtExpect<IPNetwork>(new IPNetwork())
        {
            { AlwaysWrites | NonNullWrites, "0.0.0.0/0" }
        }
      , new FmtExpect<IPNetwork>(new IPNetwork(IPAddress.Loopback, 32))
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "127.0.0.1/32" }
        }
      , new FmtExpect<IPNetwork>(IPNetwork.Parse("255.255.255.254/31"), "'{0}'") 
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'255.255.255.254/31'" } }
      , new FmtExpect<IPNetwork>(IPNetwork.Parse("255.255.0.0/16"), "\"{0,17}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"   255.255.0.0/16\"" }
        }
      
        // IPNetwork?
      , new NullFmtStructExpect<IPNetwork>(new IPNetwork())
        {
            { AlwaysWrites | NonNullWrites, "0.0.0.0/0" }
        }
      , new NullFmtStructExpect<IPNetwork>(null, "null", true) { { AlwaysWrites | NonEmptyWrites, "null" } }
      , new NullFmtStructExpect<IPNetwork>(new IPNetwork(IPAddress.Loopback, 32))
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "127.0.0.1/32" }
        }
      , new NullFmtStructExpect<IPNetwork>(IPNetwork.Parse("255.255.255.254/31"), "'{0}'") 
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'255.255.255.254/31'" } }
      , new NullFmtStructExpect<IPNetwork>(IPNetwork.Parse("255.255.0.0/16"), "\"{0,17}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"   255.255.0.0/16\"" }
        }
      
        // Version and Version?  (Class)
      , new FmtExpect<Version>(new Version())
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0.0" }
        }
      , new FmtExpect<Version>(null, "null", true) { { AlwaysWrites, "null" } }
      , new FmtExpect<Version>(new Version(1, 1))
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "1.1" }
        }
      , new FmtExpect<Version>(new Version("1.2.3.4"), "'{0}'") 
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'1.2.3.4'" } }
      , new FmtExpect<Version>(null, "null", true) { { AlwaysWrites, "null" } }
      , new FmtExpect<Version>(new Version(1,0), "'{0}'", true, new Version(1, 0)) 
            { { AlwaysWrites | NonNullWrites, "'1.0'" } }
      , new FmtExpect<Version>(new Version("5.6.7.8"), "\"{0,17}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"          5.6.7.8\"" }
        }
      
        //  IPAddress and IPAddress?
      , new FmtExpect<IPAddress>(new IPAddress("\0\0\0\0"u8))
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "0.0.0.0" }
        }
      , new FmtExpect<IPAddress>(null, "null", true) { { AlwaysWrites, "null" } }
      , new FmtExpect<IPAddress>(IPAddress.Loopback)
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "127.0.0.1" }
        }
      , new FmtExpect<IPAddress>(new IPAddress([192,168,0,1]), "'{0}'", true, new IPAddress([192,168,0,1]))
        {
            { AlwaysWrites | NonNullWrites, "'192.168.0.1'" }
        }
      , new FmtExpect<IPAddress>(IPAddress.Parse("255.255.255.254"), "'{0}'") 
            { { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'255.255.255.254'" } }
      , new FmtExpect<IPAddress>(IPAddress.Parse("255.255.0.0"), "\"{0,17}\"")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "\"      255.255.0.0\"" }
        }
      
        //  Uri and Uri?
      , new FmtExpect<Uri>(new Uri("https://learn.microsoft.com/en-us/dotnet/api"))
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "https://learn.microsoft.com/en-us/dotnet/api" }
        }
      , new FmtExpect<Uri>(null, "null", true) { { AlwaysWrites, "null" } }
      , new FmtExpect<Uri>(new Uri("https://github.com/shwaindog/Fortitude"), "'{0}'", true, new Uri("https://github.com/shwaindog/Fortitude"))
        {
            { AlwaysWrites | NonNullWrites, "'https://github.com/shwaindog/Fortitude'" }
        }
      , new FmtExpect<Uri>(new Uri("https://github.com/shwaindog/Fortitude/tree/main/src/FortitudeTests/FortitudeCommon/Types/StringsOfPower/DieCasting/TestData"), "{0[..38]}")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "https://github.com/shwaindog/Fortitude" }
        }
      , new FmtExpect<Uri>(new Uri("https://en.wikipedia.org/wiki/Rings_of_Power"), "'{0,-40}'")
        {
            { AlwaysWrites | NonEmptyWrites | NonNullWrites | NonNullAndPopulatedWrites, "'https://en.wikipedia.org/wiki/Rings_of_Power'" }
        }
    ];
}
