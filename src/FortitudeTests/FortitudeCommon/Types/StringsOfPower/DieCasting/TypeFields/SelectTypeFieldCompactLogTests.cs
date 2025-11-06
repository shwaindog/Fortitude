// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

[TestClass]
public partial class SelectTypeFieldTests
{
    private static IReadOnlyList<ScaffoldingPartEntry> scafReg = ScaffoldingRegistry.AllScaffoldingTypes;


    private static IVersatileFLogger logger = null!;

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

        logger = FLog.FLoggerForType.As<IVersatileFLogger>();
    }

    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data)
    {
        return $"{methodInfo.Name}_{(((IFormatExpectation)data[0]).ShortTestName)}_{((ScaffoldingPartEntry)data[1]).Name}";
    }

    private static IEnumerable<object[]> NonNullableBooleanExpect =>
        from fe in BoolTestData.AllBoolExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsComplexType()
                   .ProcessesSingleValue()
                   .AcceptsOnlyBoolean()
                   .AcceptsNonNullables()
        select new object[] { fe, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullBool(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullableBooleanExpect =>
        from fe in BoolTestData.AllBoolExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().AcceptsOnlyBoolean().OnlyAcceptsNullableStructs()
        select new object[] { fe, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullBool(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullableSpanFormattableExpect =>
        from fe in SpanFormattableTestData.AllSpanFormattableExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().AcceptsNonNullables()
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullFmt(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullableStructSpanFormattableExpect =>
        from fe in SpanFormattableTestData.AllSpanFormattableExpectations
        where fe is { IsNullable: true, IsStruct: true }
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().OnlyAcceptsNullableStructs()
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullFmtStruct(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> StringExpect =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.IsString()
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().AcceptsString().NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(StringExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> CharArrayExpect =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.IsCharArray()
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().AcceptsCharArray().NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(CharArrayExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogCharArray(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> CharSequenceExpect =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.ImplementsInterface<ICharSequence>()
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().AcceptsCharSequence().NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(CharSequenceExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogCharSequence(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> StringBuilderExpect =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.IsStringBuilder()
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().AcceptsStringBuilder().NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogStringBuilder(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullCloakedBearerExpect =>
        from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().AcceptsNonNullables().HasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullCloakedBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullCloakedBearerExpect =>
        from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().OnlyAcceptsNullableStructs().HasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullCloakedBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullStringBearerExpect =>
        from fe in StringBearerTestData.AllStringBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().AcceptsNonNullables()
                   .NotHasSupportsValueRevealer().HasAcceptsStringBearer()
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullStringBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullStringBearerExpect =>
        from fe in StringBearerTestData.AllStringBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg.IsComplexType().ProcessesSingleValue().OnlyAcceptsNullableStructs()
                   .NotHasSupportsValueRevealer().HasAcceptsStringBearer()
        select new object[] { fe, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullStringBearer(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    // [TestMethod]
    public void CompactLogSingleTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog
            (new FieldExpect<MutableString>
            (new MutableString
                 ("One string to use in all, one string to find text in, One string to replace them all and in the dustbins of " +
                  "time confine them"), "{0[^40..^0]}")
            {
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsValueOut)
                  , "and in the dustbins of time confine them"
                }
               ,
                {
                    new EK(SimpleType | AcceptsChars | AcceptsCharSequence | DefaultTreatedAsStringOut)
                  , "\"and in the dustbins of time confine them\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Log | Compact | Pretty)
                  , "\"and in the dustbins of time confine them\""
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Compact)
                  , """
                    ["a","n","d"," ","i","n"," ","t","h","e"," ","d","u","s","t","b","i","n","s"," ","o","f"," ","t","i","m","e"," ",
                    "c","o","n","f","i","n","e"," ","t","h","e","m"]
                    """.RemoveLineEndings()
                }
               ,
                {
                    new EK(AcceptsChars | AcceptsCharSequence | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                           NonNullAndPopulatedWrites, Json | Pretty)
                  , """
                    [
                        "a",
                        "n",
                        "d",
                        " ",
                        "i",
                        "n",
                        " ",
                        "t",
                        "h",
                        "e",
                        " ",
                        "d",
                        "u",
                        "s",
                        "t",
                        "b",
                        "i",
                        "n",
                        "s",
                        " ",
                        "o",
                        "f",
                        " ",
                        "t",
                        "i",
                        "m",
                        "e",
                        " ",
                        "c",
                        "o",
                        "n",
                        "f",
                        "i",
                        "n",
                        "e",
                        " ",
                        "t",
                        "h",
                        "e",
                        "m"
                      ]
                    """.Dos2Unix()
                }
            }, new ScaffoldingPartEntry
                 (typeof(FieldCharSequenceWhenNonNullStringBearer<>)
                , ComplexType | AcceptsSingleValue | NonDefaultWrites | AcceptsString | SupportsValueFormatString | SupportsIndexSubRanges |
                  SupportsCustomHandling));
    }

    private void SharedCompactLog(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Complex Type Single Value Field  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(scaffoldingToCall.ScaffoldingFlags.ToString("F").Replace(",", " |"))
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");


        var tos = new TheOneString().Initialize(Compact | Log);

        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string compactLogTemplate = "{0} {{ {1}}}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = propertyName + ": " + expectValue + (expectValue.Length > 0 ? " " : "");
            }
            else { expectValue = ""; }
            return string.Format(compactLogTemplate, className, expectValue);
        }

        if (formatExpectation is IComplexFieldFormatExpectation complexFieldExpectation)
        {
            complexFieldExpectation.WhenValueExpectedOutput = BuildExpectedOutput;
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
