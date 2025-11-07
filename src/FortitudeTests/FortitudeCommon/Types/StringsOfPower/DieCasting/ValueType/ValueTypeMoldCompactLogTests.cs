// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Net;
using System.Numerics;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.Expectations.SingleField;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ValueTypeScaffolds;
using static FortitudeCommon.Types.StringsOfPower.Options.StringStyle;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ValueType;

[TestClass]
public partial class ValueTypeMoldTests
{
    private static IReadOnlyList<ScaffoldingPartEntry> scafReg = ScaffoldingRegistry.AllScaffoldingTypes;


    private static IVersatileFLogger logger = null!;

    [ClassInitialize]
    public static void AllTestsInClassStaticSetup(TestContext testContext)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        FLogConfigExamples.SyncColoredTestConsoleExample.LoadExampleAsCurrentContext();

        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
        logger ??= FLog.FLoggerForType.As<IVersatileFLogger>();
    }

    public static string CreateDataDrivenTestName(MethodInfo methodInfo, object[] data)
    {
        return $"{methodInfo.Name}_{(((IFormatExpectation)data[0]).ShortTestName)}_{((ScaffoldingPartEntry)data[1]).Name}";
    }

    private static IEnumerable<object[]> NonNullableBooleanSimpleExpectAsValue =>
        from fe in BoolTestData.AllBoolExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsSimpleType()
                   .ProcessesSingleValue()
                   .AcceptsBoolean()
                   .AcceptsNonNullables()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullableBooleanSimpleExpectAsString =>
        from fe in BoolTestData.AllBoolExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsSimpleType()
                   .ProcessesSingleValue()
                   .AcceptsBoolean()
                   .AcceptsNonNullables()
                   .HasTreatedAsStringOut()
        select new object[] { fe, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(NonNullableBooleanSimpleExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsString(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullableBooleanExpectAsValue =>
        from fe in BoolTestData.AllBoolExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsBoolean().OnlyAcceptsNullableStructs()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullBoolAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullableBooleanExpectAsString =>
        from fe in BoolTestData.AllBoolExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsBoolean().OnlyAcceptsNullableStructs()
                   .HasTreatedAsStringOut()
        select new object[] { fe, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(NullableBooleanExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullBoolAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsString(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullableSpanFormattableExpectAsValue
    {
        get
        {
            var structJoins = from fe in SpanFormattableTestData.AllSpanFormattableExpectations
                where fe.InputType.IsValueType && !fe.IsNullable
                from scaffoldToCall in
                    scafReg.IsSimpleType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().AcceptsNonNullStructs()
                           .HasTreatedAsValueOut()
                select new object[] { fe, scaffoldToCall };

            var classJoins = from fe in SpanFormattableTestData.AllSpanFormattableExpectations
                where !fe.InputType.IsValueType
                from scaffoldToCall in
                    scafReg.IsSimpleType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().AcceptsClasses()
                           .HasTreatedAsValueOut()
                select new object[] { fe, scaffoldToCall };

            return structJoins.Concat(classJoins);
        }
    }


    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullFmtAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullableSpanFormattableExpectAsString
    {
        get
        {
            var structJoins = from fe in SpanFormattableTestData.AllSpanFormattableExpectations
                where fe.InputType.IsValueType && !fe.IsNullable
                from scaffoldToCall in
                    scafReg.IsSimpleType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().AcceptsNonNullStructs()
                           .HasTreatedAsStringOut()
                select new object[] { fe, scaffoldToCall };

            var classJoins = from fe in SpanFormattableTestData.AllSpanFormattableExpectations
                where !fe.InputType.IsValueType
                from scaffoldToCall in
                    scafReg.IsSimpleType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().AcceptsClasses()
                           .HasTreatedAsStringOut()
                select new object[] { fe, scaffoldToCall };

            return structJoins.Concat(classJoins);
        }
    }


    [TestMethod]
    [DynamicData(nameof(NonNullableSpanFormattableExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullFmtAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsString(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullableStructSpanFormattableExpectAsValue =>
        from fe in SpanFormattableTestData.AllSpanFormattableExpectations
        where fe is { IsNullable: true, IsStruct: true }
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().OnlyAcceptsNullableStructs()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullFmtStructAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullableStructSpanFormattableExpectAsString =>
        from fe in SpanFormattableTestData.AllSpanFormattableExpectations
        where fe is { IsNullable: true, IsStruct: true }
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().OnlyAcceptsNullableStructs()
                   .HasTreatedAsStringOut()
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(NullableStructSpanFormattableExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullFmtStructAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsString(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> StringExpectAsValue
    {
        get
        {
            var acceptsFormatString = from fe in StringLikeTestData.AllStringLikeExpectations
                where fe.InputType.IsString()
                from scaffoldToCall in
                    scafReg.IsSimpleType().ProcessesSingleValue().AcceptsString().HasSupportsValueFormatString().NotHasSupportsValueRevealer()
                           .HasTreatedAsValueOut()
                where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
                select new object[] { fe, scaffoldToCall };

            var noFormatStringFiltering =
                StringLikeTestData
                    .AllStringLikeExpectations
                    .Where(fe =>
                    {
                        var noFormatStringModifications = fe.FormatString.IsNullOrEmpty();
                        if (!noFormatStringModifications) { noFormatStringModifications = fe.FormatString is "" or "{0}"; }
                        return fe.InputType.IsString() && noFormatStringModifications;
                    })
                    .SelectMany(_ =>
                                    scafReg
                                        .IsSimpleType()
                                        .ProcessesSingleValue()
                                        .AcceptsString()
                                        .HasNotSupportsValueFormatString()
                                        .NotHasSupportsValueRevealer()
                                        .HasTreatedAsValueOut()
                              , (fe, scaffoldToCall) => new { fe, scaffoldToCall })
                    .Where(join => !join.fe.HasIndexRangeLimiting ||
                                   join.scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges))
                    .Select(join => new object[] { join.fe, join.scaffoldToCall });

            return acceptsFormatString.Concat(noFormatStringFiltering);
        }
    }


    [TestMethod]
    [DynamicData(nameof(StringExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogStringAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> StringExpectAsString
    {
        get
        {
            var acceptsFormatString = from fe in StringLikeTestData.AllStringLikeExpectations
                where fe.InputType.IsString()
                from scaffoldToCall in
                    scafReg.IsSimpleType().ProcessesSingleValue().AcceptsString().HasSupportsValueFormatString().NotHasSupportsValueRevealer()
                           .HasTreatedAsStringOut()
                where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
                select new object[] { fe, scaffoldToCall };

            var noFormatStringFiltering =
                StringLikeTestData
                    .AllStringLikeExpectations
                    .Where(fe =>
                    {
                        var noFormatStringModifications = fe.FormatString.IsNullOrEmpty();
                        if (!noFormatStringModifications) { noFormatStringModifications = fe.FormatString is "" or "{0}"; }
                        return fe.InputType.IsString() && noFormatStringModifications;
                    })
                    .SelectMany(_ =>
                                    scafReg
                                        .IsSimpleType()
                                        .ProcessesSingleValue()
                                        .AcceptsString()
                                        .HasNotSupportsValueFormatString()
                                        .NotHasSupportsValueRevealer()
                                        .HasTreatedAsStringOut()
                              , (fe, scaffoldToCall) => new { fe, scaffoldToCall })
                    .Where(join => !join.fe.HasIndexRangeLimiting ||
                                   join.scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges))
                    .Select(join => new object[] { join.fe, join.scaffoldToCall });

            return acceptsFormatString.Concat(noFormatStringFiltering);
        }
    }


    [TestMethod]
    [DynamicData(nameof(StringExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogStringAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsString(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> CharArrayExpectAsValue
    {
        get
        {
            var acceptsFormatString = from fe in StringLikeTestData.AllStringLikeExpectations
                where fe.InputType.IsCharArray()
                from scaffoldToCall in
                    scafReg.IsSimpleType().ProcessesSingleValue().AcceptsCharArray().HasSupportsValueFormatString().NotHasSupportsValueRevealer()
                           .HasTreatedAsValueOut()
                where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
                select new object[] { fe, scaffoldToCall };

            var noFormatStringFiltering =
                StringLikeTestData
                    .AllStringLikeExpectations
                    .Where(fe =>
                    {
                        var noFormatStringModifications = fe.FormatString.IsNullOrEmpty();
                        if (!noFormatStringModifications) { noFormatStringModifications = fe.FormatString is "" or "{0}"; }
                        return fe.InputType.IsCharArray() && noFormatStringModifications;
                    })
                    .SelectMany(_ =>
                                    scafReg
                                        .IsSimpleType()
                                        .ProcessesSingleValue()
                                        .AcceptsCharArray()
                                        .HasNotSupportsValueFormatString()
                                        .NotHasSupportsValueRevealer()
                                        .HasTreatedAsValueOut()
                              , (fe, scaffoldToCall) => new { fe, scaffoldToCall })
                    .Where(join => !join.fe.HasIndexRangeLimiting ||
                                   join.scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges))
                    .Select(join => new object[] { join.fe, join.scaffoldToCall });

            return acceptsFormatString.Concat(noFormatStringFiltering);
        }
    }


    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogCharArrayAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> CharArrayExpectAsString
    {
        get
        {
            var acceptsFormatString = from fe in StringLikeTestData.AllStringLikeExpectations
                where fe.InputType.IsCharArray()
                from scaffoldToCall in
                    scafReg.IsSimpleType().ProcessesSingleValue().AcceptsCharArray().HasSupportsValueFormatString().NotHasSupportsValueRevealer()
                           .HasTreatedAsStringOut()
                where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
                select new object[] { fe, scaffoldToCall };

            var noFormatStringFiltering =
                StringLikeTestData
                    .AllStringLikeExpectations
                    .Where(fe =>
                    {
                        var noFormatStringModifications = fe.FormatString.IsNullOrEmpty();
                        if (!noFormatStringModifications) { noFormatStringModifications = fe.FormatString is "" or "{0}"; }
                        return fe.InputType.IsCharArray() && noFormatStringModifications;
                    })
                    .SelectMany(_ =>
                                    scafReg
                                        .IsSimpleType()
                                        .ProcessesSingleValue()
                                        .AcceptsCharArray()
                                        .HasNotSupportsValueFormatString()
                                        .NotHasSupportsValueRevealer()
                                        .HasTreatedAsStringOut()
                              , (fe, scaffoldToCall) => new { fe, scaffoldToCall })
                    .Where(join => !join.fe.HasIndexRangeLimiting ||
                                   join.scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges))
                    .Select(join => new object[] { join.fe, join.scaffoldToCall });

            return acceptsFormatString.Concat(noFormatStringFiltering);
        }
    }
    
    [TestMethod]
    [DynamicData(nameof(CharArrayExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogCharArrayAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsString(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> CharSequenceExpectAsValue =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.ImplementsInterface<ICharSequence>()
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsCharSequence().NotHasSupportsValueRevealer()
                   .HasTreatedAsValueOut()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogCharSequenceAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> CharSequenceExpectAsString =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.ImplementsInterface<ICharSequence>()
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsCharSequence().NotHasSupportsValueRevealer()
                   .HasTreatedAsStringOut()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(CharSequenceExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogCharSequenceAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsString(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> StringBuilderExpectAsValue =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.IsStringBuilder()
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsStringBuilder().NotHasSupportsValueRevealer()
                   .HasTreatedAsValueOut()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogStringBuilderAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> StringBuilderExpectAsString =>
        from fe in StringLikeTestData.AllStringLikeExpectations
        where fe.InputType.IsStringBuilder()
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsStringBuilder().NotHasSupportsValueRevealer()
                   .HasTreatedAsStringOut()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(StringBuilderExpectAsString), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogStringBuilderAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsString(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullCloakedBearerExpectAsValue =>
        from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsNonNullables().HasSupportsValueRevealer()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(NonNullCloakedBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullCloakedBearerExpectAsValue =>
        from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().OnlyAcceptsNullableStructs().HasSupportsValueRevealer()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(NullCloakedBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullCloakedBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NonNullStringBearerExpectAsValue =>
        from fe in StringBearerTestData.AllStringBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsNonNullables()
                   .NotHasSupportsValueRevealer().HasAcceptsStringBearer()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall };


    [TestMethod]
    [DynamicData(nameof(NonNullStringBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNonNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    private static IEnumerable<object[]> NullStringBearerExpectAsValue =>
        from fe in StringBearerTestData.AllStringBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().OnlyAcceptsNullableStructs()
                   .NotHasSupportsValueRevealer().HasAcceptsStringBearer()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall };

    [TestMethod]
    [DynamicData(nameof(NullStringBearerExpectAsValue), DynamicDataDisplayName = nameof(CreateDataDrivenTestName))]
    public void CompactLogNullStringBearerAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue(formatExpectation, scaffoldingToCall);
    }

    // [TestMethod]
    public void CompactLogSingleTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue
            (new FieldExpect<IPAddress, string>(null, "", true, "")
             {
                 { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesNull), "null" }
               , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsValueOut | DefaultBecomesFallback
                        , Log | Compact | Pretty), "" }
               , { new EK(SimpleType | AcceptsAnyGeneric | DefaultBecomesFallback), "\"\"" }
               , { new EK(SimpleType | AcceptsAnyGeneric | DefaultTreatedAsStringOut | DefaultBecomesFallback), "\"\"" }
                 // Some SpanFormattable Scaffolds have both DefaultBecomesNull and DefaultBecomesFallback for when their default is TFmt?
                 // So the following will only match when both the scaffold and the following have both.
               , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull | DefaultBecomesFallback), "null" }
               , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesZero
                        , Log | Compact | Pretty) , "0" }
               , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesZero )
                   , "\"0\"" }
               , { new EK(SimpleType | AcceptsSpanFormattable | DefaultTreatedAsValueOut | DefaultBecomesEmpty | DefaultBecomesFallback
                        , Log | Compact | Pretty) , "" }
               , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesEmpty | DefaultBecomesEmpty | DefaultBecomesFallback), "\"\"" }
                 // The following covers the others that would return null.
               , { new EK(SimpleType | AcceptsSpanFormattable | DefaultBecomesNull), "null" }
               , { new EK(AcceptsSpanFormattable | AlwaysWrites | DefaultTreatedAsValueOut | DefaultTreatedAsStringOut), "null" }
             }
           , new ScaffoldingPartEntry
                 (typeof(SimpleAsValueNullableSpanFormattableClassWithStringDefaultWithFieldSimpleValueTypeStringBearer<>)
                , SimpleType | SingleValueCardinality | AcceptsOnlyNullableClassSpanFormattable | SupportsSettingDefaultValue 
                | SupportsValueFormatString | DefaultTreatedAsValueOut | DefaultBecomesFallback));
    }

    private void SharedCompactLogAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
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
        var tos = new TheOneString().Initialize(Compact | Log);

        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var compactLogTemplate = expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
                ? (propertyName.IsNotEmpty() ? "{0} {{ {1}:{2}{3}{2}}}" : "{0} {{ {3} }}")
                : expectation.InputType.IsEnum
                    ? (propertyName.IsNotEmpty() ? "{0}.{1}:{2}{3}" : "{0}.{3}")
                    : (propertyName.IsNotEmpty() ? "{0}={1}:{2}{3}" : "{0}={3}");

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
            return string.Format(compactLogTemplate, className, propertyName, maybeSpace, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
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

    private void SharedCompactLogAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
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
        var tos = new TheOneString().Initialize(Compact | Log);
        tos.Settings.NewLineStyle = "\n";

        string BuildExpectedOutput(string className, string propertyName
          , ScaffoldingStringBuilderInvokeFlags condition, IFormatExpectation expectation)
        {
            var compactLogTemplate = expectation.GetType().ExtendsGenericBaseType(typeof(NullableStringBearerExpect<>))
                ? (propertyName.IsNotEmpty() ? "{0} {{ {1}:{2}{3} }}" : "{0} {{ {3} }}")
                : expectation.InputType.IsEnum
                    ? (propertyName.IsNotEmpty() ? "{0}.{1}:{2}{3}" : "{0}.{3}")
                    : (propertyName.IsNotEmpty() ? "{0}={1}:{2}{3}" : "{0}={3}");

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
            return string.Format(compactLogTemplate, className, propertyName, maybeSpace, expectValue);
        }

        string BuildChildExpectedOutput(string className, string propertyName
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
