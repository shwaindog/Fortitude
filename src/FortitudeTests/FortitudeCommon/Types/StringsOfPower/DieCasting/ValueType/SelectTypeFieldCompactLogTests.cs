// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Net;
using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.ExampleConfig;
using FortitudeCommon.Logging.Core;
using FortitudeCommon.Logging.Core.LoggerViews;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.SingleFields;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.SimpleTypeScaffolds;
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
                   .AcceptsOnlyBoolean()
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
                   .AcceptsOnlyBoolean()
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
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsOnlyBoolean().OnlyAcceptsNullableStructs()
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
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsOnlyBoolean().OnlyAcceptsNullableStructs()
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

    private static IEnumerable<object[]> NonNullCloakedBearerExpectAsValue =>
        from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsSimpleType().ProcessesSingleValue().AcceptsNonNullables().HasSupportsValueRevealer()
                   .HasTreatedAsValueOut()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
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
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
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
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
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

    [TestMethod]
    public void CompactLogSingleTest()
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        SharedCompactLogAsValue
            (new FieldExpect<char[]>("But they were all of them deceived, for another string was made.".ToCharArray()
                              , "{0,0/,//[1..]}")
        {
            { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsValueOut), " for another string was made." }
          , { new EK(SimpleType | AcceptsChars | AcceptsCharArray | CallsAsSpan | DefaultTreatedAsStringOut), "\" for another string was made.\"" }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                       NonNullAndPopulatedWrites, Log | Compact | Pretty)
              , "[ for another string was made.]"
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                       NonNullAndPopulatedWrites, Json | Compact)
              , """[" ","f","o","r"," ","a","n","o","t","h","e","r"," ","s","t","r","i","n","g"," ","w","a","s"," ","m","a","d","e","."]"""
            }
           ,
            {
                new EK(AcceptsChars | AcceptsCharArray | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                       NonNullAndPopulatedWrites, Json | Pretty)
              , """
                [
                    " ",
                    "f",
                    "o",
                    "r",
                    " ",
                    "a",
                    "n",
                    "o",
                    "t",
                    "h",
                    "e",
                    "r",
                    " ",
                    "s",
                    "t",
                    "r",
                    "i",
                    "n",
                    "g",
                    " ",
                    "w",
                    "a",
                    "s",
                    " ",
                    "m",
                    "a",
                    "d",
                    "e",
                    "."
                  ]
                """.Dos2Unix()
            }
           ,
            {
                new EK(AcceptsChars | CallsAsSpan | AlwaysWrites | NonDefaultWrites | NonNullWrites |
                       NonNullAndPopulatedWrites)
              , "\" for another string was made.\""
            }
        }
           , new ScaffoldingPartEntry
                 (typeof(SimpleAsValueMatchOrDefaultSimpleValueTypeStringBearer<>)
                , SimpleType | AcceptsSingleValue  | AcceptsAnyGeneric | SupportsValueFormatString 
                | DefaultTreatedAsValueOut | DefaultBecomesNull));
    }

    private void SharedCompactLogAsValue(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
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
            // if (expectValue != IFormatExpectation.NoResultExpectedValue
            //  && !expectation.IsStringLike
            //  && (className.Contains("WithDefault") ||
            //      className.Contains("WithStringDefault"))
            //  && expectation.InputIsNull &&
            //     formatExpectation.DefaultAsString(tos.Settings.StyledTypeFormatter) !=
            //     null) { expectValue = formatExpectation.DefaultAsString(tos.Settings.StyledTypeFormatter); }
            // if ((expectValue is "null" or "\"\"" or "[]")
            //  && expectation.IsStringLike
            //  && (expectation.InputIsNull
            //   || (expectation.InputIsEmpty
            //    && condition.HasAnyOf(CallsAsSpan | CallsAsReadOnlySpan | SupportsIndexSubRanges | DefaultBecomesZero)
            //    && condition.HasNoneOf(DefaultBecomesNull)))
            //  && condition.HasAnyOf(SupportsSettingDefaultValue | DefaultBecomesZero)
            //  && formatExpectation.DefaultAsString(tos.Settings.StyledTypeFormatter) == "0") { expectValue = "0"; }
            // if ((expectValue is "\"\"" or "[]")
            //  && expectation.IsStringLike
            //  && (expectation.InputIsNull
            //   || (expectation.InputIsEmpty
            //    && condition.HasAnyOf(CallsAsSpan | CallsAsReadOnlySpan | SupportsIndexSubRanges | DefaultBecomesNull)
            //    && condition.HasNoneOf(DefaultBecomesZero)))
            //  && condition.HasAnyOf(SupportsSettingDefaultValue | DefaultBecomesNull)) { expectValue = "null"; }
            // if (condition.HasAllOf(SupportsValueRevealer | DefaultBecomesNull)
            //  && expectation.InputIsNull) { expectValue = "null"; }
            // if (!condition.HasAllOf(AcceptsAnyGeneric)
            //  && expectation.IsStringLike
            //  && ((expectValue.Length > 2 && expectValue[0] == '"' && expectValue[^1] == '"')
            //   || (expectValue.Length > 2 && expectValue[0] == '[' && expectValue[^1] == ']')))
            // {
            //     expectValue = expectValue.Substring(1, expectValue.Length - 2);
            // }
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
            // if (expectation.InputIsNull && expectValue == "null") return "null";
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

    private void SharedCompactLogAsString(IFormatExpectation formatExpectation, ScaffoldingPartEntry scaffoldingToCall)
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

        var tos = new TheOneString().Initialize(Compact | Log);

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
            // if (expectValue == "null")
            // {
            //     if (!expectation.IsStringLike
            //      && (className.Contains("WithDefault") ||
            //          className.Contains("WithStringDefault"))
            //      && expectation.InputIsNull &&
            //         formatExpectation.DefaultAsString(tos.Settings.StyledTypeFormatter) !=
            //         null) { expectValue = formatExpectation.DefaultAsString(tos.Settings.StyledTypeFormatter); }
            //     if (expectValue == "null" && expectation is { InputIsNull: true } or { InputIsEmpty: true }
            //           && condition.HasNoneOf(DefaultBecomesNull)) { expectValue = ""; }
            //     else if (expectValue == "null" && (condition.IsAcceptsAnyGeneric()) || condition.HasAnyOf(DefaultBecomesNull))
            //     {
            //         compactLogTemplate = compactLogTemplate.Replace("\"", "");
            //     }
            // }
            // if ((expectValue is "\"\"" )
            //  && expectation.IsStringLike
            //  && (expectation.InputIsNull
            //   || (expectation.InputIsEmpty
            //    && condition.HasAnyOf(CallsAsSpan | CallsAsReadOnlySpan | AcceptsChars | SupportsIndexSubRanges)
            //       && (condition.HasNoneOf(DefaultBecomesNull) || condition.HasAllOf(AcceptsAnyGeneric))))) 
            // { expectValue = ""; }
            // if ((expectValue is "\"\"" or "[]")
            //  && expectation.IsStringLike
            //  && (expectation.InputIsNull
            //   || (expectation.InputIsEmpty
            //    && condition.HasAnyOf(CallsAsSpan | CallsAsReadOnlySpan | SupportsIndexSubRanges | DefaultBecomesZero)
            //    && condition.HasNoneOf(DefaultBecomesNull)))
            //  && condition.HasAnyOf(SupportsSettingDefaultValue | DefaultBecomesNull)) { expectValue = ""; }
            // if (condition.HasAllOf(SupportsValueRevealer | DefaultBecomesNull)
            //  && expectation.InputIsNull) { expectValue = "null"; }
            
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
            // if (expectation.InputIsNull && expectValue == "null") return "null";
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
