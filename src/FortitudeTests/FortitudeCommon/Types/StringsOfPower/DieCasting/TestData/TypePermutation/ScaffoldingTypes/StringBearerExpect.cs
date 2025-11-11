// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Runtime.CompilerServices;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields.FieldContentHandling;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;


// ReSharper disable twice ExplicitCallerInfoArgument
public class StringBearerExpect<TInput>
(
    TInput? input
  , string? formatString = null
  , bool hasDefault = false
  , TInput? defaultValue = default
  , FieldContentHandling contentHandling = DefaultCallerTypeFlags
  , [CallerFilePath] string srcFile = ""
  , [CallerLineNumber] int srcLine = 0)
    : StringBearerExpect<TInput, TInput>(input, formatString, hasDefault, defaultValue, contentHandling, srcFile, srcLine)
    where TInput : IStringBearer;


public class StringBearerExpect<TInput, TDefault> : FieldExpect<TInput, TDefault>, IComplexFieldFormatExpectation
    where TInput : IStringBearer
{
    // ReSharper disable twice ExplicitCallerInfoArgument
    public StringBearerExpect(TInput? input, string? formatString = null
      , bool hasDefault = false, TDefault? defaultValue = default
      , FieldContentHandling contentHandling = DefaultCallerTypeFlags
      , [CallerFilePath] string srcFile = ""
      , [CallerLineNumber] int srcLine = 0) : base(input, formatString, hasDefault, defaultValue, contentHandling, srcFile, srcLine)
    {
        FieldValueExpectation = new FieldExpect<TInput?, TDefault?>(Input, FormatString, HasDefault, DefaultValue, contentHandling, srcFile, srcLine);
    }

    public ITypedFormatExpectation<TInput?> FieldValueExpectation { get; }

    public override bool IsNullable => InputType.IsNullable();

    public BuildExpectedOutput WhenValueExpectedOutput { get; set; } = null!;

    public override string GetExpectedOutputFor(ScaffoldingStringBuilderInvokeFlags condition, StyleOptions stringStyle, string? formatString = null)
    {
        FieldValueExpectation.ClearExpectations();
        foreach (var expectedResult in ExpectedResults) { FieldValueExpectation.Add(expectedResult); }
        condition |= ScaffoldingStringBuilderInvokeFlags.AcceptsSpanFormattable | ScaffoldingStringBuilderInvokeFlags.AcceptsChars | ScaffoldingStringBuilderInvokeFlags.AcceptsString;
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
                scaffoldEntry.ScaffoldingFlags.HasAnyOf(ScaffoldingStringBuilderInvokeFlags.DefaultTreatedAsValueOut | ScaffoldingStringBuilderInvokeFlags.DefaultTreatedAsStringOut)
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
