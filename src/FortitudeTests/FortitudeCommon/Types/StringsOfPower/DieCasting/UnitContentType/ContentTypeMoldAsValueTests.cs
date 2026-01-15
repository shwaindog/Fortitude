// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.UnitContentType;

public abstract class ContentTypeMoldAsValueTests : CommonScaffoldExpectationTestBase
{
    public override string TestsCommonDescription => "Unit Content AsValue";
    
    private static List<object[]>? nonNullBoolScaffoldingExpectations;
    private static List<object[]>? nullableBooleanExpectScaffoldingExpectations;
    private static List<object[]>? nonNullableSpanFormattableScaffoldingExpectations;
    private static List<object[]>? nullableStructSpanFormattableScaffoldingExpectations;
    private static List<object[]>? stringScaffoldingExpectations;
    private static List<object[]>? charArrayScaffoldingExpectations;
    private static List<object[]>? charSequenceScaffoldingExpectations;
    private static List<object[]>? stringBuilderScaffoldingExpectations;
    private static List<object[]>? nonNullCloakedBearerScaffoldingExpectations;
    private static List<object[]>? nullCloakedBearerScaffoldingExpectations;
    private static List<object[]>? nonNullStringBearerScaffoldingExpectations;
    private static List<object[]>? nullStringBearerScaffoldingExpectations;
    
    public static IEnumerable<object[]> NonNullableBooleanSimpleExpectAsValue =>
        nonNullBoolScaffoldingExpectations ??=
        (from fe in BoolTestData.AllBoolExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            ScafReg
                .IsContentType()
                .ProcessesSingleValue()
                .AcceptsBoolean()
                .AcceptsNonNullables()
                .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> NullableBooleanExpectAsValue =>
        nullableBooleanExpectScaffoldingExpectations ??=
        (from fe in BoolTestData.AllBoolExpectations
        where fe.IsNullable
        from scaffoldToCall in
            ScafReg.IsContentType().ProcessesSingleValue().AcceptsBoolean().AcceptsNullableStructs()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> NonNullableSpanFormattableExpectAsValue
    {
        get
        {
            if (nonNullableSpanFormattableScaffoldingExpectations?.Any() ?? false) return nonNullableSpanFormattableScaffoldingExpectations;
            var structJoins = from fe in SpanFormattableTestData.AllSpanFormattableExpectations.Value
                where fe.InputType.IsValueType && !fe.IsNullable
                from scaffoldToCall in
                    ScafReg.IsContentType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().AcceptsNonNullStructs()
                           .HasTreatedAsValueOut()
                select new object[] { fe, scaffoldToCall };

            var classJoins = from fe in SpanFormattableTestData.AllSpanFormattableExpectations.Value
                where !fe.InputType.IsValueType
                from scaffoldToCall in
                    ScafReg.IsContentType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().AcceptsClasses()
                           .HasTreatedAsValueOut()
                select new object[] { fe, scaffoldToCall };

            return nonNullableSpanFormattableScaffoldingExpectations = structJoins.Concat(classJoins).ToList();
        }
    }
    
    public static IEnumerable<object[]> NullableStructSpanFormattableExpectAsValue =>
        nullableStructSpanFormattableScaffoldingExpectations ??=
        (from fe in SpanFormattableTestData.AllSpanFormattableExpectations.Value
        where fe is { IsNullable: true, IsStruct: true }
        from scaffoldToCall in
            ScafReg.IsContentType().ProcessesSingleValue().HasSpanFormattable().NotHasSupportsValueRevealer().AcceptsNullableStructs()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> StringExpectAsValue
    {
        get
        {
            if (stringScaffoldingExpectations?.Any() ?? false) return stringScaffoldingExpectations;
            var acceptsFormatString = from fe in StringTestData.AllStringExpectations
                where fe.InputType.IsString()
                from scaffoldToCall in
                    ScafReg.IsContentType().ProcessesSingleValue().AcceptsString().HasSupportsValueFormatString().NotHasSupportsValueRevealer()
                           .HasTreatedAsValueOut()
                where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
                select new object[] { fe, scaffoldToCall };

            var noFormatStringFiltering =
                StringTestData
                    .AllStringExpectations
                    .Where(fe =>
                    {
                        var noFormatStringModifications = fe.ValueFormatString.IsNullOrEmpty();
                        if (!noFormatStringModifications) { noFormatStringModifications = fe.ValueFormatString is "" or "{0}"; }
                        return fe.InputType.IsString() && noFormatStringModifications;
                    })
                    .SelectMany(_ =>
                                    ScafReg
                                        .IsContentType()
                                        .ProcessesSingleValue()
                                        .AcceptsString()
                                        .HasNotSupportsValueFormatString()
                                        .NotHasSupportsValueRevealer()
                                        .HasTreatedAsValueOut()
                              , (fe, scaffoldToCall) => new { fe, scaffoldToCall })
                    .Where(join => !join.fe.HasIndexRangeLimiting ||
                                   join.scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges))
                    .Select(join => new object[] { join.fe, join.scaffoldToCall });

            return stringScaffoldingExpectations = acceptsFormatString.Concat(noFormatStringFiltering).ToList();
        }
    }

    public static IEnumerable<object[]> CharArrayExpectAsValue
    {
        get
        {
            if (charArrayScaffoldingExpectations?.Any() ?? false) return charArrayScaffoldingExpectations;
            
            var acceptsFormatString = from fe in CharArrayTestData.AllCharArrayExpectations
                where fe.InputType.IsCharArray()
                from scaffoldToCall in
                    ScafReg.IsContentType().ProcessesSingleValue().AcceptsCharArray().HasSupportsValueFormatString().NotHasSupportsValueRevealer()
                           .HasTreatedAsValueOut()
                where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
                select new object[] { fe, scaffoldToCall };

            var noFormatStringFiltering =
                CharArrayTestData
                    .AllCharArrayExpectations
                    .Where(fe =>
                    {
                        var noFormatStringModifications = fe.ValueFormatString.IsNullOrEmpty();
                        if (!noFormatStringModifications) { noFormatStringModifications = fe.ValueFormatString is "" or "{0}"; }
                        return fe.InputType.IsCharArray() && noFormatStringModifications;
                    })
                    .SelectMany(_ =>
                                    ScafReg
                                        .IsContentType()
                                        .ProcessesSingleValue()
                                        .AcceptsCharArray()
                                        .HasNotSupportsValueFormatString()
                                        .NotHasSupportsValueRevealer()
                                        .HasTreatedAsValueOut()
                              , (fe, scaffoldToCall) => new { fe, scaffoldToCall })
                    .Where(join => !join.fe.HasIndexRangeLimiting ||
                                   join.scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges))
                    .Select(join => new object[] { join.fe, join.scaffoldToCall });

            return charArrayScaffoldingExpectations = acceptsFormatString.Concat(noFormatStringFiltering).ToList();
        }
    }

    public static IEnumerable<object[]> CharSequenceExpectAsValue =>
        charSequenceScaffoldingExpectations ??=
        (from fe in CharSequenceTestData.AllCharSequenceExpectations
        where fe.InputType.ImplementsInterface<ICharSequence>()
        from scaffoldToCall in
            ScafReg.IsContentType().ProcessesSingleValue().AcceptsCharSequence().NotHasSupportsValueRevealer()
                   .HasTreatedAsValueOut()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> StringBuilderExpectAsValue =>
        stringBuilderScaffoldingExpectations ??=
        (from fe in StringBuilderTestData.AllStringBuilderExpectations
        where fe.InputType.IsStringBuilder()
        from scaffoldToCall in
            ScafReg.IsContentType().ProcessesSingleValue().AcceptsStringBuilder().NotHasSupportsValueRevealer()
                   .HasTreatedAsValueOut()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> NonNullCloakedBearerExpectAsValue =>
        nonNullCloakedBearerScaffoldingExpectations ??=
        (from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            ScafReg.IsContentType().ProcessesSingleValue().AcceptsNonNullables().HasSupportsValueRevealer()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> NullCloakedBearerExpectAsValue =>
        nullCloakedBearerScaffoldingExpectations ??=
        (from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            ScafReg
                .IsContentType()
                .ProcessesSingleValue()
                .AcceptsNullableStructs()
                .HasSupportsValueRevealer()
                .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> NonNullStringBearerExpectAsValue =>
        nonNullStringBearerScaffoldingExpectations ??=
        (from fe in StringBearerTestData.AllStringBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            ScafReg.IsContentType().ProcessesSingleValue().AcceptsNonNullables()
                   .NotHasSupportsValueRevealer().HasAcceptsStringBearer()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> NullStringBearerExpectAsValue =>
        nullStringBearerScaffoldingExpectations ??=
        (from fe in StringBearerTestData.AllStringBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            ScafReg.IsContentType().ProcessesSingleValue().AcceptsNullableStructs()
                   .NotHasSupportsValueRevealer().HasAcceptsStringBearer()
                   .HasTreatedAsValueOut()
        select new object[] { fe, scaffoldToCall }).ToList();
}
