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

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFieldCollection;

[TestClass]
public partial class SelectTypeCollectionFieldTests
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
                    .IsJustComplexType()
                    .ProcessesCollection()
                    .NoFilterPredicate()
                    .AcceptsBoolean()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
                where fe.ElementTypeIsNullable && !fe.HasRestrictingFilter   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .AcceptsBoolean()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(UnfilteredBooleanCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogBoolCollections(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredBooleanCollectionsExpect =>
        (from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
            where !fe.ElementTypeIsNullable && fe.HasRestrictingFilter   
            from scaffoldToCall in 
                scafReg
                    .IsJustComplexType()
                    .ProcessesCollection()
                    .HasFilterPredicate()
                    .AcceptsBoolean()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
                where fe.ElementTypeIsNullable && fe.HasRestrictingFilter   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .AcceptsBoolean()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredBooleanCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogBoolCollections(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }


    private static IEnumerable<object[]> UnfilteredFmtCollectionsExpect =>
        (from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
            where fe is {ElementTypeIsNullable: false, HasRestrictingFilter: false }   
            from scaffoldToCall in 
                scafReg
                    .IsJustComplexType()
                    .ProcessesCollection()
                    .NoFilterPredicate()
                    .HasSpanFormattable()
                    .NotHasSupportsValueRevealer()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
                where fe is { ElementTypeIsNullable: true, ElementTypeIsStruct: true, HasRestrictingFilter: false }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
                where fe is {ElementTypeIsClass : true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(UnfilteredFmtCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogFmtList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredFmtCollectionsExpect =>
        // Non nullables and classes
        (from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
            where fe is {ElementTypeIsNullable: false, HasRestrictingFilter: true }   
            from scaffoldToCall in 
                scafReg
                    .IsJustComplexType()
                    .ProcessesCollection()
                    .HasFilterPredicate()
                    .HasSpanFormattable()
                    .NotHasSupportsValueRevealer()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                // Nullable structs
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
                where fe is { ElementTypeIsNullable: true, ElementTypeIsStruct: true, HasRestrictingFilter: true }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                // classes
                from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations
                where fe is {ElementTypeIsClass : true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredFmtCollectionsExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogFmtList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> UnfilteredStringCollectionExpect =>
        (from fe in StringCollectionsTestData.AllStringCollectionExpectations
        where fe.ElementType.IsString() && fe is {ContainsNullElements : false,  HasRestrictingFilter: false } 
        from scaffoldToCall in 
            scafReg
                .IsJustComplexType()
                .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .AcceptsString()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                // classes
                from fe in StringCollectionsTestData.AllStringCollectionExpectations
                where fe is {ElementTypeIsClass : true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .AcceptsString()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });


    [TestMethod]
    [DynamicData(nameof(UnfilteredStringCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogStringList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredStringCollectionExpect =>
        (from fe in StringCollectionsTestData.AllStringCollectionExpectations
        where fe.ElementType.IsString() && fe is { ContainsNullElements : false,  HasRestrictingFilter: true } 
        from scaffoldToCall in 
            scafReg
                .IsJustComplexType()
                .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .AcceptsString()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                // classes
                from fe in StringCollectionsTestData.AllStringCollectionExpectations
                where fe is {ElementTypeIsClass : true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .AcceptsString()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });


    [TestMethod]
    [DynamicData(nameof(FilteredStringCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogStringList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> UnfilteredCharSequenceCollectionExpect =>
        (from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
        where fe is {ContainsNullElements : false,  HasRestrictingFilter: false } 
        from scaffoldToCall in 
            scafReg
                .IsJustComplexType()
                .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .AcceptsCharSequence()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                // classes
                from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
                where fe is {ElementTypeIsClass : true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .AcceptsCharSequence()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });


    [TestMethod]
    [DynamicData(nameof(UnfilteredCharSequenceCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogCharSequenceList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredCharSequenceCollectionExpect =>
        (from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
        where fe is { ContainsNullElements : false,  HasRestrictingFilter: true } 
        from scaffoldToCall in 
            scafReg
                .IsJustComplexType()
                .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .AcceptsCharSequence()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                // classes
                from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
                where fe is {ElementTypeIsClass : true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .AcceptsCharSequence()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });


    [TestMethod]
    [DynamicData(nameof(FilteredCharSequenceCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogCharSequenceList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }
    
    private static IEnumerable<object[]> UnfilteredStringBuilderCollectionExpect =>
        (from fe in StringBuilderCollectionsTestData.AllStringBuilderCollectionExpectations
        where fe is {ContainsNullElements : false,  HasRestrictingFilter: false } 
        from scaffoldToCall in 
            scafReg
                .IsJustComplexType()
                .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .AcceptsStringBuilder()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });


    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBuilderCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogStringBuilderList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredStringBuilderCollectionExpect =>
        (from fe in StringBuilderCollectionsTestData.AllStringBuilderCollectionExpectations
        where fe is {ContainsNullElements : false,  HasRestrictingFilter: true } 
        from scaffoldToCall in 
            scafReg
                .IsJustComplexType()
                .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .AcceptsStringBuilder()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredStringBuilderCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogStringBuilderList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }
    
    private static IEnumerable<object[]> UnfilteredCloakedBearerCollectionExpect =>
        (from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
            where fe is {ElementTypeIsClass: true, ContainsNullElements : false,  HasRestrictingFilter: false } 
            from scaffoldToCall in 
                scafReg
                    .IsJustComplexType()
                    .ProcessesCollection()
                    .AcceptsNonNullables()
                    .NoFilterPredicate()
                    .HasSupportsValueRevealer()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsClass: true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsNullableStruct: true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .HasSupportsValueRevealer()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });
    
    [TestMethod]
    [DynamicData(nameof(UnfilteredCloakedBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogCloakedBearerList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredCloakedBearerCollectionExpect =>
        (from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
            where fe is {ElementTypeIsNullable: false, ContainsNullElements : false,  HasRestrictingFilter: true } 
            from scaffoldToCall in 
                scafReg
                    .IsJustComplexType()
                    .ProcessesCollection()
                    .AcceptsNonNullables()
                    .HasFilterPredicate()
                    .HasSupportsValueRevealer()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsClass: true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsNullableStruct: true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .HasSupportsValueRevealer()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredCloakedBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogCloakedBearerList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }
    
    private static IEnumerable<object[]> UnfilteredStringBearerCollectionExpect =>
        (from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
            where fe is {ElementTypeIsClass: true, ContainsNullElements : false,  HasRestrictingFilter: false } 
            from scaffoldToCall in 
                scafReg
                    .IsJustComplexType()
                    .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .NoFilterPredicate()
                        .HasAcceptsStringBearer()
                        .NotHasSupportsValueRevealer()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(UnfilteredStringBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void UnfilteredCompactLogStringBearerList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> FilteredStringBearerCollectionExpect =>
        (from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
            where fe is {ElementTypeIsNullable: false, ContainsNullElements : false,  HasRestrictingFilter: true } 
            from scaffoldToCall in 
                scafReg
                    .IsJustComplexType()
                    .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
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
                        .IsJustComplexType()
                        .ProcessesCollection()
                        .HasFilterPredicate()
                        .HasAcceptsStringBearer()
                        .NotHasSupportsValueRevealer()
                        .OnlyAcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall });

    [TestMethod]
    [DynamicData(nameof(FilteredStringBearerCollectionExpect), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void FilteredCompactLogStringBearerList(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLog(formatExpectation, scaffoldingToCall);
    }

    [TestMethod]
    public void CompactLogListTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        //VVVVVVVVVVVVVVVVVVV  Paste Here VVVVVVVVVVVVVVVVVVVVVVVVVVVV//
        SharedCompactLog(StringBearerCollectionsTestData.AllStringBearerCollectionExpectations[3], ScaffoldingRegistry.AllScaffoldingTypes[693]);
    }

    private void SharedCompactLog(IOrderedListExpect formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        logger.InfoAppend("Complex Type Collection Field  Scaffolding Class - ")?
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
            const string compactLogTemplate = "{0} {{ {1}}}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue != IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = propertyName + ": " + expectValue + (expectValue.Length > 0 ? " " : "");
            }
            else { expectValue = ""; }
            return string.Format(compactLogTemplate, className, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var compactLogTemplate = className.IsNotEmpty() ? "({0}){1}" : "{1}";

            var expectValue = expectation.GetExpectedOutputFor(condition, tos.Settings, expectation.FormatString);
            if (expectValue == IFormatExpectation.NoResultExpectedValue)
            {
                expectValue = "";
            }
            return string.Format(compactLogTemplate, className, expectValue);
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
