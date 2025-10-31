// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Net;
using System.Numerics;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.SimpleTypeScaffolds;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;


public partial class ValueTypeMoldTests
{

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsString(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullFmtAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullFmtAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullFmtStructAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullFmtStructAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsString(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(StringExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonStringAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(StringExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonStringAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharArrayAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharArrayAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharSequenceAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharSequenceAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonStringBuilderAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonStringBuilderAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsString(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsValue(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    public void CompactJsonSingleTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJsonAsString
            (new FieldExpect<char[]>("with".ToCharArray(), "\"{0[8..10]}\"")
        {
            { new EK(SimpleType | AcceptsAnyGeneric | AcceptsCharArray | CallsAsSpan, Log | Compact | Pretty), "\"\"" }
          , { new EK(SimpleType | AcceptsAnyGeneric | AcceptsCharArray | DefaultBecomesFallback | DefaultTreatedAsValueOut) , "\\u0022\\u0022" }
          , { new EK(SimpleType | AcceptsAnyGeneric | AcceptsCharArray | DefaultBecomesFallback) , "\"\\u0022\\u0022\"" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan, Log | Compact | Pretty), "\"\"" }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut, Log | Compact | Pretty),
                "\"\""
            }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan),
                """""
                "\u0022\u0022"
                """""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Log | Compact | Pretty)
              , "[\"\"]"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Compact)
              , """["\u0022","\u0022"]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites
                     , Json | Pretty)
              , """
                [
                    "\u0022",
                    "\u0022"
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites | NonNullAndPopulatedWrites)
              , """""
                """"
                """""
            }
        }
           , new ScaffoldingPartEntry
                 (typeof(SimpleAsStringMatchSimpleValueTypeStringBearer<>)
                , SimpleType | AcceptsSingleValue  | AcceptsAnyGeneric | SupportsValueFormatString
                | DefaultTreatedAsStringOut | DefaultBecomesNull));
    }

    private void SharedCompactJsonAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Complex Type Single Value Field  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(new MutableString().AppendFormat("{0}",  scaffoldingToCall.ScaffoldingFlags).ToString())
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");

        // ReSharper disable once RedundantArgumentDefaultValue
        var tos = new TheOneString().Initialize(Compact | Json);

        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var compactJsonTemplate = expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
                ? "{{\"{0}\":{1}}}"
                : "{1}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = "";
            }
            return string.Format(compactJsonTemplate, propertyName, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string compactJsonTemplate = "{{\"{0}\":{1}}}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = propertyName + ": " + expectValue + (expectValue.Length > 0 ? " " : "");
            }
            else { expectValue = ""; }
            return string.Format(compactJsonTemplate, propertyName, expectValue);
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
               , formatExpectation);
        var result = tos.WriteBuffer.ToString();
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

    private void SharedCompactJsonAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Complex Type Single Value Field  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(new MutableString().AppendFormat("{0}",  scaffoldingToCall.ScaffoldingFlags).ToString())
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");

        // ReSharper disable once RedundantArgumentDefaultValue
        var tos = new TheOneString().Initialize(Compact | Json);
        tos.Settings.NewLineStyle = "\n";

        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var compactJsonTemplate = expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
                ? "{{\"{0}\":{1}}}"
                : "{1}";

            var maybeSpace  = "";
            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                maybeSpace = expectValue.Trim().Length > 0 ? " " : "";
                if (maybeSpace.Length == 0)
                {
                    expectValue = "";
                }
            }
            else { expectValue = ""; }
            return string.Format(compactJsonTemplate, propertyName, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string compactJsonTemplate = "{{\"{0}\":{1}}}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = propertyName + ": " + expectValue + (expectValue.Length > 0 ? " " : "");
            }
            else { expectValue = ""; }
            return string.Format(compactJsonTemplate, propertyName, expectValue);
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
               , formatExpectation);
        var result = tos.WriteBuffer.ToString();
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
