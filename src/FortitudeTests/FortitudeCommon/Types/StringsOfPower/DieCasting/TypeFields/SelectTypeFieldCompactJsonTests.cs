// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

public partial class SelectTypeFieldTests
{
    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullBool(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullBool(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullFmt(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullFmtStruct(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(StringExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(CharArrayExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharArray(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(CharSequenceExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonCharSequence(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(StringBuilderExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonStringBuilder(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullCloakedBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNullCloakedBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactJsonNonNullStringBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson(formatExpectation, scaffoldingToCall);
    }

    // [TestMethod]
    public void CompactJsonSingleTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactJson
            ( new FieldExpect<MutableString>
                 (new MutableString
                      ("And nine, nine strings were gifted to the race of C++ coders, " +
                       "who above all else desired unchecked memory access power. "), "***\"{0[1..^1]}\"***"
                , fromIndex: 9, length: 41)
                 {
                     {
                         new EK(AcceptsChars | CallsAsReadOnlySpan | CallsAsSpan | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                                NonNullAndPopulatedWrites)
                       , "***\"nine strings were gifted to the race of\"***"
                     }
                    ,
                     {
                         new EK(AcceptsChars | AlwaysWrites | NonEmptyWrites | NonNullWrites |
                                NonNullAndPopulatedWrites, Json | Compact | Pretty)
                       , """
                         ["*","*","*","\u0022","n","i","n","e"," ","s","t","r","i","n","g","s"," ","w","e","r","e"," ","g","i","f","t","e","d",
                         " ","t","o"," ","t","h","e"," ","r","a","c","e"," ","o","f","\u0022","*","*","*"]
                         """.RemoveLineEndings()
                     }
                 }, new ScaffoldingPartEntry
                 (typeof(FieldCharSequenceRangeAlwaysAddStringBearer<>)
                , ComplexType | AcceptsSingleValue | AlwaysWrites | AcceptsCharSequence | SupportsValueFormatString | SupportsIndexSubRanges));
    }

    private void SharedCompactJson(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Complex Type Single Value Field  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .FinalAppend("");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");


        var tos          = new TheOneString().Initialize(Compact | Json);
        var expectString = formatExpectation.InputType.IsAnyTypeHoldingChars() || formatExpectation.InputType.IsChar();
        
        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string compactJsonTemplate = "{{{0}}}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = "\"" + propertyName + "\":" + expectValue;
            }
            else { expectValue = ""; }
            return string.Format(compactJsonTemplate, expectValue);
        }

        if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
        {
            complexFieldExpectation.WhenValueExpectedOutput = BuildExpectedOutput;
        }
        tos.Clear();
        var stringBearer = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall);
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
