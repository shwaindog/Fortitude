// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.OrderedLists;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;

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
                        .AcceptsNullableStructs()
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
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogBoolCollections(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }
    
    private static IEnumerable<object[]> UnfilteredFmtCollectionsExpect =>
        // Non nullables and classes
        (from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations.Value
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
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations.Value
                where fe is { ElementTypeIsNullable: true, ElementTypeIsStruct: true, HasRestrictingFilter: false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                // classes
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations.Value
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
    [DynamicData(nameof(UnfilteredFmtCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogFmtList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredFmtCollectionsExpect =>
        (from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations.Value
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
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations.Value
                where fe is { ElementTypeIsNullable: true, ElementTypeIsStruct: true, HasRestrictingFilter: true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations.Value
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

    private static IEnumerable<object[]> UnfilteredStringCollectionExpect =>
        (from fe in StringCollectionsTestData.AllStringCollectionExpectations
        where fe is {ContainsNullElements : false,  HasRestrictingFilter: false } 
        from scaffoldToCall in 
            scafReg
                .IsOrderedCollectionType()
                .AcceptsNonNullables()
                .NoFilterPredicate()
                .AcceptsString()
                .NotHasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in StringCollectionsTestData.AllStringCollectionExpectations
                where fe is {ContainsNullElements : true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .AcceptsString()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });


    [TestMethod]
    [DynamicData(nameof(UnfilteredStringCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredStringCollectionExpect =>
        (from fe in StringCollectionsTestData.AllStringCollectionExpectations
        where fe is {ContainsNullElements : false,  HasRestrictingFilter: true } 
        from scaffoldToCall in 
            scafReg
                .IsOrderedCollectionType()
                .AcceptsNonNullables()
                .HasFilterPredicate()
                .AcceptsString()
                .NotHasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in StringCollectionsTestData.AllStringCollectionExpectations
                where fe is {ContainsNullElements : true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .AcceptsString()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredStringCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogStringList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> UnfilteredCharSequenceCollectionExpect =>
        (from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
        where fe is {ContainsNullElements : false,  HasRestrictingFilter: false } 
        from scaffoldToCall in 
            scafReg
                .IsOrderedCollectionType()
                .AcceptsNonNullables()
                .NoFilterPredicate()
                .AcceptsCharSequence()
                .NotHasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
                where fe is {ContainsNullElements : true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .AcceptsCharSequence()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });
    
    [TestMethod]
    [DynamicData(nameof(UnfilteredCharSequenceCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
        
    }

    private static IEnumerable<object[]> FilteredCharSequenceCollectionExpect =>
        (from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
        where fe is { ContainsNullElements : false,  HasRestrictingFilter: true } 
        from scaffoldToCall in 
            scafReg
                .IsOrderedCollectionType()
                .AcceptsNonNullables()
                .HasFilterPredicate()
                .AcceptsCharSequence()
                .NotHasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
                where fe is {ContainsNullElements : true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .AcceptsCharSequence()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });
    
    [TestMethod]
    [DynamicData(nameof(FilteredCharSequenceCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogCharSequenceList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
        
    }
    
    private static IEnumerable<object[]> UnfilteredStringBuilderCollectionExpect =>
        (from fe in StringBuilderCollectionsTestData.AllStringBuilderCollectionExpectations
        where fe is {ContainsNullElements : false,  HasRestrictingFilter: false } 
        from scaffoldToCall in 
            scafReg
                .IsOrderedCollectionType()
                .AcceptsNonNullables()
                .NoFilterPredicate()
                .AcceptsStringBuilder()
                .NotHasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in StringBuilderCollectionsTestData.AllStringBuilderCollectionExpectations
                where fe is {ContainsNullElements : true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .AcceptsStringBuilder()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBuilderCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredStringBuilderCollectionExpect =>
        (from fe in StringBuilderCollectionsTestData.AllStringBuilderCollectionExpectations
        where fe is {ContainsNullElements : false,  HasRestrictingFilter: true } 
        from scaffoldToCall in 
            scafReg
                .IsOrderedCollectionType()
                .AcceptsNonNullables()
                .HasFilterPredicate()
                .AcceptsStringBuilder()
                .NotHasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in StringBuilderCollectionsTestData.AllStringBuilderCollectionExpectations
                where fe is {ContainsNullElements : true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .AcceptsStringBuilder()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredStringBuilderCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogStringBuilderList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }
    
    private static IEnumerable<object[]> UnfilteredCloakedBearerCollectionExpect =>
        (from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
            where fe is {ElementTypeIsClass: true, ContainsNullElements : false,  HasRestrictingFilter: false } 
            from scaffoldToCall in 
                scafReg
                    .IsOrderedCollectionType()
                    .AcceptsNonNullables()
                    .NoFilterPredicate()
                    .HasSupportsValueRevealer()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsClass: true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsNullableStruct: true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(UnfilteredCloakedBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredCloakedBearerCollectionExpect =>
        (from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
            where fe is {ElementTypeIsNullable: false, ContainsNullElements : false,  HasRestrictingFilter: true } 
            from scaffoldToCall in 
                scafReg
                    .IsOrderedCollectionType()
                    .AcceptsNonNullables()
                    .HasFilterPredicate()
                    .HasSupportsValueRevealer()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsClass: true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsNullableStruct: true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredCloakedBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogCloakedBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }
    
    private static IEnumerable<object[]> UnfilteredStringBearerCollectionExpect =>
        (from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
            where fe is {ElementTypeIsClass: true, ContainsNullElements : false,  HasRestrictingFilter: false } 
            from scaffoldToCall in 
                scafReg
                    .IsOrderedCollectionType()
                    .AcceptsNonNullables()
                    .NoFilterPredicate()
                    .HasAcceptsStringBearer()
                    .NotHasSupportsValueRevealer()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
                where fe is {ElementTypeIsClass: true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasAcceptsStringBearer()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
                where fe is {ElementTypeIsNullableStruct: true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasAcceptsStringBearer()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredStringBearerCollectionExpect =>
        (from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
            where fe is {ElementTypeIsNullable: false, ContainsNullElements : false,  HasRestrictingFilter: true } 
            from scaffoldToCall in 
                scafReg
                    .IsOrderedCollectionType()
                    .AcceptsNonNullables()
                    .HasFilterPredicate()
                    .HasAcceptsStringBearer()
                    .NotHasSupportsValueRevealer()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
                where fe is {ElementTypeIsClass: true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasAcceptsStringBearer()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
                where fe is {ElementTypeIsNullableStruct: true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasAcceptsStringBearer()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredStringBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogStringBearerList(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    // [TestMethod]
    public void CompactLogListTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        SharedCompactLog(CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations[2], ScaffoldingRegistry.AllScaffoldingTypes[28]);
    }

    private void SharedCompactLog(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Ordered Collection Type Field  Scaffolding Class - ")?
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

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = "";
            }
            return string.Format(compactLogTemplate, className, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.ValueFormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            { expectValue = ""; }
            return expectValue;
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
        }
        else
        {
            logger.InfoAppend("Result Matched Expected - ")?.AppendLine()
                  .Append(result).AppendLine()
                  .FinalAppend("");
        }
            
        logger.InfoAppend("To Debug Test past the following code into ")?
              .Append(nameof(CompactLogListTest)).Append("()\n\n")
              .Append("SharedCompactLog(")
              .Append(formatExpectation.ItemCodePath).Append(", ").Append(scaffoldingToCall.ItemCodePath).FinalAppend(");");
        Assert.AreEqual(buildExpectedOutput, result);
    }
}
