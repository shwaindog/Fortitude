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
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TypeOrderedCollection;

[TestClass]
public partial class OrderedCollectionMoldTests
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

    private static IEnumerable<object[]> UnfilteredBooleanCollectionsExpect =>
        (from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
            where !fe.ElementTypeIsNullable && !fe.HasRestrictingFilter   
            from scaffoldToCall in 
                scafReg
                    .IsOrderedCollectionType()
                    .NoFilterPredicate()
                    .AcceptsBoolean()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
                where fe.ElementTypeIsNullable && !fe.HasRestrictingFilter   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .AcceptsBoolean()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(UnfilteredBooleanCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredBooleanCollectionsExpect =>
        (from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
            where !fe.ElementTypeIsNullable && fe.HasRestrictingFilter   
            from scaffoldToCall in 
                scafReg
                    .IsOrderedCollectionType()
                    .HasFilterPredicate()
                    .AcceptsBoolean()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
                where fe.ElementTypeIsNullable && fe.HasRestrictingFilter   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .AcceptsBoolean()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }
    
    private static IEnumerable<object[]> UnfilteredNonNullFmtCollectionsExpect =>
        // Non nullables and classes
        (from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
            where fe is {ElementTypeIsNullable: false, HasRestrictingFilter: false }   
            from scaffoldToCall in 
                scafReg
                    .IsOrderedCollectionType()
                    .NoFilterPredicate()
                    .HasSpanFormattable()
                    .NotHasSupportsValueRevealer()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                // Nullable structs
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
                where fe is { ElementTypeIsNullable: true, ElementTypeIsStruct: true, HasRestrictingFilter: false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                // classes
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
                where fe is {ElementTypeIsClass : true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(UnfilteredNonNullFmtCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredFmtCollectionsExpect =>
        (from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
            where fe is {ElementTypeIsNullable: false, HasRestrictingFilter: true }   
            from scaffoldToCall in 
                scafReg
                    .IsOrderedCollectionType()
                    .HasFilterPredicate()
                    .HasSpanFormattable()
                    .NotHasSupportsValueRevealer()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
                where fe is { ElementTypeIsNullable: true, ElementTypeIsStruct: true, HasRestrictingFilter: true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
                where fe is {ElementTypeIsClass : true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredFmtCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> StringExpect =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.IsString()
        from scaffoldToCall in scafReg.IsOrderedCollectionType().AcceptsString().NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    // [TestMethod]
    [DynamicData(nameof(StringExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> CharSequenceExpect =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.ImplementsInterface<ICharSequence>()
        from scaffoldToCall in
            scafReg.IsOrderedCollectionType().AcceptsCharSequence().NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    // [TestMethod]
    [DynamicData(nameof(CharSequenceExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> StringBuilderExpect =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.IsStringBuilder()
        from scaffoldToCall in
            scafReg.IsOrderedCollectionType().AcceptsStringBuilder().NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    // [TestMethod]
    [DynamicData(nameof(StringBuilderExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullCloakedBearerExpect =>
        from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsOrderedCollectionType().AcceptsNonNullables().HasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall };


    // [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullCloakedBearerExpect =>
        from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg.IsOrderedCollectionType().OnlyAcceptsNullableStructs().HasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall };


    // [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullStringBearerExpect =>
        from fe in StringBearerTestData.AllStringBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsOrderedCollectionType().AcceptsNonNullables()
                   .NotHasSupportsValueRevealer().HasAcceptsStringBearer()
        select new object[] { fe, scaffoldToCall };


    // [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullStringBearerExpect =>
        from fe in StringBearerTestData.AllStringBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg.IsOrderedCollectionType().OnlyAcceptsNullableStructs()
                   .NotHasSupportsValueRevealer().HasAcceptsStringBearer()
        select new object[] { fe, scaffoldToCall };

    // [TestMethod]
    [DynamicData(nameof(NullStringBearerExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    public void CompactLogListTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        SharedCompactLog(BoolCollectionsTestData.AllBoolCollectionExpectations[3], ScaffoldingRegistry.AllScaffoldingTypes[1]);
    }

    private void SharedCompactLog(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Ordered Collection Type Single Value Field  Scaffolding Class - ")?
              .AppendLine(scaffoldingToCall.Name)
              .AppendLine()
              .AppendLine("Scaffolding Flags -")
              .AppendLine(scaffoldingToCall.ScaffoldingFlags.ToString("F").Replace(",", " |"))
              .FinalAppend("\n");

        logger.WarnAppend("FormatExpectation - ")?
              .AppendLine(formatExpectation.ToString())
              .FinalAppend("");


        // ReSharper disable once RedundantArgumentDefaultValue
        var tos = new TheOneString().Initialize(Compact | Log);

        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            const string compactLogTemplate = "({0}){1}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = "";
            }
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
            
            logger.InfoAppend("To Debug Test past the following code into ")?
                  .Append(nameof(CompactLogListTest)).Append("()\n\n")
                  .Append("SharedCompactLog(")
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
