// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Net;
using System.Numerics;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ValueTypeScaffolds;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;


public partial class ValueTypeMoldTests
{

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsString(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullFmtAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullFmtAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullFmtStructAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullFmtStructAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsString(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(StringExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonStringAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(StringExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonStringAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharArrayAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharArrayAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharSequenceAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonCharSequenceAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonStringBuilderAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonStringBuilderAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyJsonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    // [TestMethod]
    public void PrettyJsonSingleTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        SharedCompactJsonAsValue(SpanFormattableTestData.AllSpanFormattableExpectations.Value[209], ScaffoldingRegistry.AllScaffoldingTypes[1139]);
    }

    private void SharedPrettyJsonAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Simple Value Type Single Value Field  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(new MutableString().AppendFormat("{0}",  scaffoldingToCall.ScaffoldingFlags).ToString().Replace(",", " |"))
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");

        // ReSharper disable once RedundantArgumentDefaultValue
        var tos = new TheOneString().Initialize(Pretty | Json);
        tos.Settings.NewLineStyle = "\n";

        string BuildExpectedOutput(string _, string _1
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var expectValue  = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            { expectValue = ""; }
            return expectValue;
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var maybeNewLine = "";
            var maybeIndent  = "";
            var expectValue  = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                maybeNewLine = "\n";
                maybeIndent  = "  ";
                expectValue  = "\"" + propertyName + "\": " + expectValue;
            }

            else { expectValue = ""; }

            // return string.Format(prettyJsonTemplate, maybeNewLine, maybeIndent, expectValue);
            return $"{{{maybeNewLine}{maybeIndent}{expectValue}{maybeNewLine}}}";
        }

        if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
        {
            complexFieldExpectation.WhenValueExpectedOutput = BuildChildExpectedOutput;
        }
        tos.Clear();
        var stringBearer = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall, tos.Settings);
        stringBearer.RevealState(tos);
        var buildExpectedOutput =
            BuildExpectedOutput
                (stringBearer.GetType().CachedCSharpNameNoConstraints()
               , ((ISinglePropertyTestStringBearer)stringBearer).PropertyName
               , scaffoldingToCall.ScaffoldingFlags
               , formatExpectation).MakeWhiteSpaceVisible();
        var result = tos.WriteBuffer.ToString().MakeWhiteSpaceVisible();
        if (buildExpectedOutput != result)
        {
            logger.ErrorAppend("Result Did not match Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .AppendLine("Expected it to match -")
                  .AppendLine(buildExpectedOutput)
                  .FinalAppend("");
            
            logger.InfoAppend("To Debug Test past the following code into ")?
                  .Append(nameof(PrettyJsonSingleTest)).Append("()\n\n")
                  .Append("SharedPrettyJsonAsValue(")
                  .Append(formatExpectation.ItemCodePath).Append(", ").Append(scaffoldingToCall.ItemCodePath).FinalAppend(");");
        }
        else
        {
            logger.InfoAppend("Result Matched Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .FinalAppend("");
        }
        Assert.AreEqual(buildExpectedOutput, result);
    }

    private void SharedPrettyJsonAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Simple Value Type Single Value Field  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(new MutableString().AppendFormat("{0}",  scaffoldingToCall.ScaffoldingFlags).ToString().Replace(",", " |"))
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");

        // ReSharper disable once RedundantArgumentDefaultValue
        var tos = new TheOneString().Initialize(Pretty | Json);
        tos.Settings.NewLineStyle = "\n";


        string BuildExpectedOutput(string _, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var prettyJsonTemplate = expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
                ? (propertyName.IsNotEmpty() ? "{{{0}{1}{2}{0}}}" : "{2}")
                : "{2}";

            var maybeNewLine = "";
            var maybeIndent  = "";
            var expectValue  = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                maybeNewLine = "\n";
                maybeIndent  = "  ";
            }

            else { expectValue = ""; }

            return string.Format(prettyJsonTemplate, maybeNewLine, maybeIndent, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string prettyJsonTemplate = "{{{0}{1}{2}{0}}}";

            var maybeNewLine = "";
            var maybeIndent  = "";
            var expectValue  = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                maybeNewLine = "\n";
                maybeIndent  = "  ";
                expectValue  = "\"" + propertyName + "\": " + expectValue.IndentSubsequentLines();
            }

            else { expectValue = ""; }

            return string.Format(prettyJsonTemplate, maybeNewLine, maybeIndent, expectValue);
        }

        if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
        {
            complexFieldExpectation.WhenValueExpectedOutput = BuildChildExpectedOutput;
        }
        tos.Clear();
        var stringBearer = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall, tos.Settings);
        stringBearer.RevealState(tos);
        var buildExpectedOutput =
            BuildExpectedOutput
                (stringBearer.GetType().CachedCSharpNameNoConstraints()
               , ((ISinglePropertyTestStringBearer)stringBearer).PropertyName
               , scaffoldingToCall.ScaffoldingFlags
               , formatExpectation).MakeWhiteSpaceVisible();
        var result = tos.WriteBuffer.ToString().MakeWhiteSpaceVisible();
        if (buildExpectedOutput != result)
        {
            logger.ErrorAppend("Result Did not match Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .AppendLine("Expected it to match -")
                  .AppendLine(buildExpectedOutput)
                  .FinalAppend("");
            
            logger.InfoAppend("To Debug Test past the following code into ")?
                  .Append(nameof(PrettyJsonSingleTest)).Append("()\n\n")
                  .Append("SharedPrettyJsonAsString(")
                  .Append(formatExpectation.ItemCodePath).Append(", ").Append(scaffoldingToCall.ItemCodePath).FinalAppend(");");
        }
        else
        {
            logger.InfoAppend("Result Matched Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .FinalAppend("");
        }
        Assert.AreEqual(buildExpectedOutput, result);
    }
}
