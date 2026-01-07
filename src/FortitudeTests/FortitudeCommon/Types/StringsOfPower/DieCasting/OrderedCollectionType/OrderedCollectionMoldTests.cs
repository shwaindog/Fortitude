// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.OrderedCollectionFieldsTypes;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.OrderedCollectionType;

public abstract class OrderedCollectionMoldTests : CommonExpectationTestBase
{
    public override string TestsCommonDescription => "Ordered Collection Type";
    
    private static List<object[]>? boolUnfilteredScaffoldingExpectations;
    private static List<object[]>? boolFilteredScaffoldingExpectations;
    private static List<object[]>? spanFormattableUnfilteredScaffoldingExpectations;
    private static List<object[]>? spanFormattableFilteredScaffoldingExpectations;
    private static List<object[]>? stringUnfilteredScaffoldingExpectations;
    private static List<object[]>? stringFilteredScaffoldingExpectations;
    private static List<object[]>? charSequenceUnfilteredScaffoldingExpectations;
    private static List<object[]>? charSequenceFilteredScaffoldingExpectations;
    private static List<object[]>? stringBuilderUnfilteredScaffoldingExpectations;
    private static List<object[]>? stringBuilderFilteredScaffoldingExpectations;
    private static List<object[]>? cloakedBearerUnfilteredScaffoldingExpectations;
    private static List<object[]>? cloakedBearerFilteredScaffoldingExpectations;
    private static List<object[]>? stringBearerUnfilteredScaffoldingExpectations;
    private static List<object[]>? stringBearerFilteredScaffoldingExpectations;
    
    public static IEnumerable<object[]> UnfilteredBooleanCollectionsExpect =>
        boolUnfilteredScaffoldingExpectations ??=
        (from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
            where !fe.ElementTypeIsNullable && !fe.HasRestrictingFilter   
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
                    .NoFilterPredicate()
                    .AcceptsBoolean()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
                where fe.ElementTypeIsNullable && !fe.HasRestrictingFilter   
                from scaffoldToCall in 
                    scafReg
                        .IsAnOrderedCollectionType()
                        .NoFilterPredicate()
                        .AcceptsBoolean()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall }).ToList();
    

    public static IEnumerable<object[]> FilteredBooleanCollectionsExpect =>
        boolFilteredScaffoldingExpectations ??=
        (from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
            where !fe.ElementTypeIsNullable && fe.HasRestrictingFilter   
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
                    .HasFilterPredicate()
                    .AcceptsBoolean()
                    .AcceptsNonNullables()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in BoolCollectionsTestData.AllBoolCollectionExpectations
                where fe.ElementTypeIsNullable && fe.HasRestrictingFilter   
                from scaffoldToCall in 
                    scafReg
                        .IsAnOrderedCollectionType()
                        .HasFilterPredicate()
                        .AcceptsBoolean()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall }).ToList();
    
    
    public static IEnumerable<object[]> UnfilteredFmtCollectionsExpect =>
        spanFormattableUnfilteredScaffoldingExpectations ??=
        // Non nullables and classes
        (from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations.Value
            where fe is {ElementTypeIsNullable: false, HasRestrictingFilter: false }   
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall }).ToList();
    

    public static IEnumerable<object[]> FilteredFmtCollectionsExpect =>
        spanFormattableFilteredScaffoldingExpectations ??=
        (from fe in SpanFormattableCollectionTestData.AllSpanFormattableCollectionExpectations.Value
            where fe is {ElementTypeIsNullable: false, HasRestrictingFilter: true }   
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasSpanFormattable()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> UnfilteredStringCollectionExpect =>
        stringUnfilteredScaffoldingExpectations ??=
        (from fe in StringCollectionsTestData.AllStringCollectionExpectations
            where fe is {ContainsNullElements : false,  HasRestrictingFilter: false } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .NoFilterPredicate()
                        .AcceptsString()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> FilteredStringCollectionExpect =>
        stringFilteredScaffoldingExpectations ??=
        (from fe in StringCollectionsTestData.AllStringCollectionExpectations
            where fe is {ContainsNullElements : false,  HasRestrictingFilter: true } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .HasFilterPredicate()
                        .AcceptsString()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> UnfilteredCharSequenceCollectionExpect =>
        charSequenceUnfilteredScaffoldingExpectations ??=
        (from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
            where fe is {ContainsNullElements : false,  HasRestrictingFilter: false } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .NoFilterPredicate()
                        .AcceptsCharSequence()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> FilteredCharSequenceCollectionExpect =>
        charSequenceFilteredScaffoldingExpectations ??=
        (from fe in CharSequenceCollectionsTestData.AllCharSequenceCollectionExpectations
            where fe is { ContainsNullElements : false,  HasRestrictingFilter: true } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .HasFilterPredicate()
                        .AcceptsCharSequence()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> UnfilteredStringBuilderCollectionExpect =>
        stringBuilderUnfilteredScaffoldingExpectations ??=
        (from fe in StringBuilderCollectionsTestData.AllStringBuilderCollectionExpectations
            where fe is {ContainsNullElements : false,  HasRestrictingFilter: false } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .NoFilterPredicate()
                        .AcceptsStringBuilder()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> FilteredStringBuilderCollectionExpect =>
        stringBuilderFilteredScaffoldingExpectations ??=
        (from fe in StringBuilderCollectionsTestData.AllStringBuilderCollectionExpectations
            where fe is {ContainsNullElements : false,  HasRestrictingFilter: true } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .HasFilterPredicate()
                        .AcceptsStringBuilder()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> UnfilteredCloakedBearerCollectionExpect =>
        cloakedBearerUnfilteredScaffoldingExpectations ??=
        (from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
            where fe is {ElementTypeIsClass: true, ContainsNullElements : false,  HasRestrictingFilter: false } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
                    .AcceptsNonNullables()
                    .NoFilterPredicate()
                    .HasSupportsValueRevealer()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsClass: true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsAnOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsNullableStruct: true, HasRestrictingFilter : false }   
                from scaffoldToCall in 
                    scafReg
                        .IsAnOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> FilteredCloakedBearerCollectionExpect =>
        cloakedBearerFilteredScaffoldingExpectations ??=
        (from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
            where fe is {ElementTypeIsNullable: false, ContainsNullElements : false,  HasRestrictingFilter: true } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
                    .AcceptsNonNullables()
                    .HasFilterPredicate()
                    .HasSupportsValueRevealer()
            select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsClass: true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsAnOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableClasses()
                select new object[] { fe, scaffoldToCall })
        .Concat( 
                from fe in CloakedBearerCollectionsTestData.AllCloakedBearerCollectionExpectations
                where fe is {ElementTypeIsNullableStruct: true, HasRestrictingFilter : true }   
                from scaffoldToCall in 
                    scafReg
                        .IsAnOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall }).ToList();
    
    public static IEnumerable<object[]> UnfilteredStringBearerCollectionExpect =>
        stringBearerUnfilteredScaffoldingExpectations ??=
        (from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
            where fe is {ElementTypeIsClass: true, ContainsNullElements : false,  HasRestrictingFilter: false } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .NoFilterPredicate()
                        .HasAcceptsStringBearer()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> FilteredStringBearerCollectionExpect =>
        stringBearerFilteredScaffoldingExpectations ??=
        (from fe in StringBearerCollectionsTestData.AllStringBearerCollectionExpectations
            where fe is {ElementTypeIsNullable: false, ContainsNullElements : false,  HasRestrictingFilter: true } 
            from scaffoldToCall in 
                scafReg
                    .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
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
                        .IsAnOrderedCollectionType()
                        .HasFilterPredicate()
                        .HasAcceptsStringBearer()
                        .NotHasSupportsValueRevealer()
                        .AcceptsNullableStructs()
                select new object[] { fe, scaffoldToCall }).ToList();
}
