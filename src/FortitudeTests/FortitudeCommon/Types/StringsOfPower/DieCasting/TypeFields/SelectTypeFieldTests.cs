// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower.Options;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

[TestClass]
public class SelectTypeFieldTests
{
    private static IReadOnlyList<ScaffoldingPartEntry> scafReg = ScaffoldingRegistry.AllScaffoldingTypes;


    private static IVersatileFLogger logger     = null!;
    private const  string            BulletList = "    * ";

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
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
    public void WithCompactLogNonNullBool(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
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
    public void WithCompactLogNullBool(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
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
    public void WithCompactLogNonNullFmt(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
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
    public void WithCompactLogNullFmtStruct(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
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
    public void WithCompactLogString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
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
    public void WithCompactLogCharArray(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
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
    public void WithCompactLogCharSequence(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
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
    public void WithCompactLogStringBuilder(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private void SharedCompactLog(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Complex Type Single Value Field  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .FinalAppend("");
        
        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");

        string BuildExpectedOutput(ISinglePropertyTestStringBearer testStringBearer, ScaffoldingPartEntry entry, IFormatExpectation expectation)
        {
            const string compactLogTemplate = "{0} {{ {1}}}";

            var expectValue = expectation.GetExpectedOutputFor(entry.ScaffoldingFlags);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = testStringBearer.PropertyName + ": " + expectValue + (expectValue.Length > 0 ? " " : "");
            }
            else { expectValue = ""; }
            return string.Format(compactLogTemplate, testStringBearer.GetType().ShortNameInCSharpFormat(), expectValue);
        }

        var tos = new TheOneString().Initialize(StringStyle.Compact | StringStyle.Log);
        tos.Clear();
        var stringBearer = formatExpectation.CreateStringBearerWithValueFor(scaffoldingToCall);
        stringBearer.RevealState(tos);
        var buildExpectedOutput = BuildExpectedOutput((ISinglePropertyTestStringBearer)stringBearer, scaffoldingToCall, formatExpectation);
        var result              = tos.WriteBuffer.ToString();
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
