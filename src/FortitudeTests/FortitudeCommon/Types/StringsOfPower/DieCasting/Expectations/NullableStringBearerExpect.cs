// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;

public class NullableStringBearerExpect<TInput> : NullableStringBearerExpect<TInput, TInput>
    where TInput : struct, IStringBearer
{
    // ReSharper disable twice ExplicitCallerInfoArgument
    public NullableStringBearerExpect(TInput? input, string? valueFormatString = null
      , bool hasDefault = false, TInput? defaultValue = null
      , FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0)
        : base(input, valueFormatString, hasDefault, defaultValue, formatFlags, name, srcFile, srcLine)
    {
        FieldValueExpectation = new FieldExpect<TInput?>(Input, ValueFormatString, HasDefault, DefaultValue);
    }
}

public class NullableStringBearerExpect<TInput, TDefault> : FieldExpect<TInput?, TDefault?>, IComplexFieldFormatExpectation
    where TInput : struct, IStringBearer
    where TDefault : struct
{
    public ITypedFormatExpectation<TInput?> FieldValueExpectation { get; protected init; }

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override bool IsNullable => InputType.IsNullable();

    // ReSharper disable twice ExplicitCallerInfoArgument
    public NullableStringBearerExpect(TInput? input, string? valueFormatString = null
      , bool hasDefault = false, TDefault? defaultValue = null, FormatFlags formatFlags = DefaultCallerTypeFlags
      , string? name = null, [CallerFilePath] string srcFile = "", [CallerLineNumber] int srcLine = 0)
        : base(input, valueFormatString, hasDefault, defaultValue, formatFlags, name, srcFile, srcLine)
    {
        // ReSharper disable twice ExplicitCallerInfoArgument
        FieldValueExpectation = new FieldExpect<TInput?, TDefault?>(Input, ValueFormatString, HasDefault, DefaultValue, formatFlags
                                                                  , name, srcFile, srcLine);
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
                ((Input?.GetType() ?? typeof(TInput)).CachedCSharpNameNoConstraints(), ((ISinglePropertyTestStringBearer)Input!.Value).PropertyName
               , condition
               , FieldValueExpectation);
        }
        return expectValue;
    }

    public override ISinglePropertyTestStringBearer CreateNewStringBearer(ScaffoldingPartEntry scaffoldEntry)
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
                scaffoldEntry.ScaffoldingFlags.HasAnyOf(DefaultTreatedAsValueOut |
                                                        DefaultTreatedAsStringOut)
             && !InputType.IsSpanFormattableOrNullable()
                    ? expectedDefaultString
                    : new MutableString().Append(DefaultValue).ToString();
        }
        var stringBearerInput = Input;
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (ValueFormatString != null && stringBearerInput is ISupportsValueFormatString supportsValueFormatString)
        {
            supportsValueFormatString.ValueFormatString = ValueFormatString;

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
