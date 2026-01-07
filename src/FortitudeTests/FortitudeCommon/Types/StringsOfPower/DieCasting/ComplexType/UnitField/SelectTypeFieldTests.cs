// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.UnitFieldsContentTypes;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.UnitField;

public abstract class SelectTypeFieldTests : CommonExpectationTestBase
{
    public override string TestsCommonDescription => "Complex Type Single Value Field";
    
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


    public static IEnumerable<object[]> NonNullableBooleanExpect =>
        nonNullBoolScaffoldingExpectations ??=
        (from fe in BoolTestData.AllBoolExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg.IsAComplexType()
                   .IsNotContentType()
                   .ProcessesSingleValue()
                   .AcceptsBoolean()
                   .AcceptsNonNullables()
        select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> NullableBooleanExpect =>
        nullableBooleanExpectScaffoldingExpectations ??=
        (from fe in BoolTestData.AllBoolExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .AcceptsBoolean()
                .AcceptsNullableStructs()
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> NonNullableSpanFormattableExpect =>
        nonNullableSpanFormattableScaffoldingExpectations ??=
        (from fe in SpanFormattableTestData.AllSpanFormattableExpectations.Value
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .HasSpanFormattable()
                .NotHasSupportsValueRevealer()
                .AcceptsNonNullables()
        select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> NullableStructSpanFormattableExpect =>
        nullableStructSpanFormattableScaffoldingExpectations ??=
        (from fe in SpanFormattableTestData.AllSpanFormattableExpectations.Value
        where fe is { IsNullable: true, IsStruct: true }
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .HasSpanFormattable()
                .NotHasSupportsValueRevealer()
                .AcceptsNullableStructs()
        select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> StringExpect =>
        stringScaffoldingExpectations ??=
        (from fe in StringTestData.AllStringExpectations
        where fe.InputType.IsString()
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .AcceptsString()
                .NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> CharArrayExpect =>
        charArrayScaffoldingExpectations ??=
        (from fe in CharArrayTestData.AllCharArrayExpectations
        where fe.InputType.IsCharArray()
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .AcceptsCharArray()
                .NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> CharSequenceExpect =>
        charSequenceScaffoldingExpectations ??=
        (from fe in CharSequenceTestData.AllCharSequenceExpectations
        where fe.InputType.ImplementsInterface<ICharSequence>()
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .AcceptsCharSequence()
                .NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> StringBuilderExpect =>
        stringBuilderScaffoldingExpectations ??=
        (from fe in StringBuilderTestData.AllStringBuilderExpectations
        where fe.InputType.IsStringBuilder()
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .AcceptsStringBuilder()
                .NotHasSupportsValueRevealer()
        where !fe.HasIndexRangeLimiting || scaffoldToCall.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges)
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> NonNullCloakedBearerExpect =>
        nonNullCloakedBearerScaffoldingExpectations ??=
        (from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .AcceptsNonNullables()
                .HasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> NullCloakedBearerExpect =>
        nullCloakedBearerScaffoldingExpectations ??=
        (from fe in CloakedBearerTestData.AllCloakedBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .AcceptsNullableStructs()
                .HasSupportsValueRevealer()
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> NonNullStringBearerExpect =>
        nonNullStringBearerScaffoldingExpectations ??=
        (from fe in StringBearerTestData.AllStringBearerExpectations
        where !fe.IsNullable
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .AcceptsNonNullables()
                .NotHasSupportsValueRevealer()
                .HasAcceptsStringBearer()
        select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> NullStringBearerExpect =>
        nullStringBearerScaffoldingExpectations ??=
        (from fe in StringBearerTestData.AllStringBearerExpectations
        where fe.IsNullable
        from scaffoldToCall in
            scafReg
                .IsAComplexType()
                .IsNotContentType()
                .ProcessesSingleValue()
                .AcceptsNullableStructs()
                .NotHasSupportsValueRevealer()
                .HasAcceptsStringBearer()
        select new object[] { fe, scaffoldToCall }).ToList();
}
