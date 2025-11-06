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
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.SimpleTypeScaffolds;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;


public partial class ValueTypeMoldTests
{

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsString(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullFmtAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullFmtAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullFmtStructAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullFmtStructAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsString(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(StringExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(StringExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharArrayAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharArrayAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharSequenceAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogCharSequenceAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringBuilderAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogStringBuilderAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void PrettyLogNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue(formatExpectation, scaffoldingToCall);
    }

    // [TestMethod]
    public void PrettyLogSingleTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedPrettyLogAsValue
            (new FieldExpect<BigInteger>(Int128.MinValue * (BigInteger)50, "'{0:X33}'")
             {
                 { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut, Log | Compact | Pretty)
                   , "'E700000000000000000000000000000000'" }
               , { new EK(SimpleType | AcceptsSpanFormattable), "\"'E700000000000000000000000000000000'\"" }
               , {
                     new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                          , Log | Compact | Pretty) , "'E700000000000000000000000000000000'"
                 }
                ,
                 {
                     new EK(AcceptsSpanFormattable | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                          , Json | Compact | Pretty)
                   , "\"'E700000000000000000000000000000000'\""
                 }
             }
           , new ScaffoldingPartEntry
                 (typeof(SimpleAsValueSpanFormattableWithFieldSimpleValueTypeStringBearer<>)
                , SimpleType | AcceptsSingleValue | AcceptsStruct | AcceptsClass | AcceptsNullableClass | AcceptsSpanFormattable 
                | AcceptsIntegerNumber | AcceptsDecimalNumber | AcceptsDateTimeLike | SupportsValueFormatString | DefaultTreatedAsValueOut 
                | DefaultBecomesZero));
    }

    private void SharedPrettyLogAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
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
        var tos = new TheOneString().Initialize(Pretty | Log);
        tos.Settings.NewLineStyle = "\n";

        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string prettyLogTemplate = "{0}={1}{2}{3}";

            var maybeProperty = propertyName.IsNotEmpty() ? $"{propertyName}:" : "";
            var maybeIndent = "";
            var expectValue   = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                maybeIndent = expectValue.IsNotEmpty() && propertyName.IsNotEmpty() ? " " : "";
            }
            else { expectValue = ""; }

            return string.Format(prettyLogTemplate, className, maybeProperty, maybeIndent, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string prettyLogTemplate = "{0} {{{1}{2}{3}{1}}}";

            var maybeNewLine = "";
            var maybeIndent  = "";
            var expectValue  = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                maybeNewLine = "\n";
                maybeIndent  = "  ";
                expectValue  = propertyName + ": " + expectValue;
            }

            else { expectValue = ""; }

            return string.Format(prettyLogTemplate, className, maybeNewLine, maybeIndent, expectValue);
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
                (stringBearer.GetType().ShortNameInCSharpFormat()
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
        }
        else
        {
            logger.InfoAppend("Result Matched Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .FinalAppend("");
        }
        Assert.AreEqual(buildExpectedOutput, result);
    }

    private void SharedPrettyLogAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
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
        var tos = new TheOneString().Initialize(Pretty | Log);
        tos.Settings.NewLineStyle = "\n";


        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string prettyLogTemplate = "{0}={1}{2}{3}";

            var maybeProperty = propertyName.IsNotEmpty() ? $"{propertyName}:" : "";
            var maybeIndent   = "";
            var expectValue   = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                maybeIndent = expectValue.IsNotEmpty() && propertyName.IsNotEmpty() ? " " : "";
            }
            else { expectValue = ""; }

            return string.Format(prettyLogTemplate, className, maybeProperty, maybeIndent, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string prettyLogTemplate = "{0} {{{1}{2}{3}{1}}}";

            var maybeNewLine = "";
            var maybeIndent  = "";
            var expectValue  = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                maybeNewLine = "\n";
                maybeIndent  = "  ";
                expectValue  = propertyName + ": " + expectValue;
            }

            else { expectValue = ""; }

            return string.Format(prettyLogTemplate, className, maybeNewLine, maybeIndent, expectValue);
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
                (stringBearer.GetType().ShortNameInCSharpFormat()
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
