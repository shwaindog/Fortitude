// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.MapCollectionsFieldsTypes;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;

public abstract class SelectTypeKeyedCollectionFieldTests : CommonExpectationTestBase
{
    public override string TestsCommonDescription => "Complex Keyed Collection Type Field";
    
    private static List<object[]>? simpleUnfilteredScaffoldingExpectations;
    private static List<object[]>? simplePredicateFilteredScaffoldingExpectations;
    private static List<object[]>? simpleSubListFilteredScaffoldingExpectations;
    private static List<object[]>? valueRevealerUnfilteredScaffoldingExpectations;
    private static List<object[]>? valueRevealerPredicateFilteredScaffoldingExpectations;
    private static List<object[]>? valueRevealerSubListFilteredScaffoldingExpectations;
    private static List<object[]>? bothRevealersUnfilteredScaffoldingExpectations;
    private static List<object[]>? bothRevealersPredicateFilteredScaffoldingExpectations;
    private static List<object[]>? bothRevealersSubListFilteredScaffoldingExpectations;
    
    public static IEnumerable<object[]> SimpleUnfilteredDictExpect =>
        simpleUnfilteredScaffoldingExpectations ??=
        (from kce in SimpleDictTestData.AllUnfilteredSimpleDictExpectations
        where !kce.HasRestrictingPredicateFilter
        from scaffoldToCall in 
            ScafReg
                .IsAComplexType()
                .ProcessesKeyedCollection()
                .NotHasSupportsKeyRevealer()
                .NotHasSupportsValueRevealer()
                .NoFilterPredicate()
                .NoSubsetListFilter()
        where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()    
        select new object[] { kce, scaffoldToCall }).ToList();
    

    public static IEnumerable<object[]> SimplePredicateFilteredDictExpect =>
        simplePredicateFilteredScaffoldingExpectations ??=
        (from kce in SimpleDictTestData.AllPredicateFilteredSimpleDictExpectations
        from scaffoldToCall in
            ScafReg
                .IsAComplexType()
                .ProcessesKeyedCollection()
                .NotHasSupportsKeyRevealer()
                .NotHasSupportsValueRevealer()
                .HasFilterPredicate()
                .NoSubsetListFilter()
        where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()    
        select new object[] { kce, scaffoldToCall }).ToList();
    

    public static IEnumerable<object[]> SimpleSubListFilteredDictExpect =>
        simpleSubListFilteredScaffoldingExpectations ??=
        (from kce in SimpleDictTestData.AllSubListFilteredDictExpectations
        from scaffoldToCall in
            ScafReg
                .IsAComplexType()
                .ProcessesKeyedCollection()
                .NotHasSupportsKeyRevealer()
                .NotHasSupportsValueRevealer()
                .HasSubsetListFilter()
                .NoFilterPredicate()
        select new object[] { kce, scaffoldToCall }).ToList();

    public static IEnumerable<object[]> ValueRevealerUnfilteredDict =>
        valueRevealerUnfilteredScaffoldingExpectations ??=
        (from kce in ValueRevealerDictTestData.AllValueRevealerUnfilteredDictExpectations
            where kce.ValueTypeIsNotNullableStruct
            from scaffoldToCall in
                ScafReg
                    .IsAComplexType()
                    .ProcessesKeyedCollection()
                    .NotHasSupportsKeyRevealer()
                    .HasSupportsValueRevealer()
                    .NoFilterPredicate()
                    .AcceptsStructClassNullableClass()
                    .NoSubsetListFilter()
            where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
            select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in ValueRevealerDictTestData.AllValueRevealerUnfilteredDictExpectations
                where kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .NotHasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .AcceptsNullableStructs()
                        .NoSubsetListFilter()
                where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               ).ToList();

    public static IEnumerable<object[]> ValueRevealerPredicateFilteredDictExpect =>
        valueRevealerPredicateFilteredScaffoldingExpectations ??=
        (from kce in ValueRevealerDictTestData.AllPredicateFilteredDictExpectations
            where kce.ValueTypeIsNotNullableStruct
            from scaffoldToCall in
                ScafReg
                    .IsAComplexType()
                    .ProcessesKeyedCollection()
                    .NotHasSupportsKeyRevealer()
                    .HasSupportsValueRevealer()
                    .HasFilterPredicate()
                    .AcceptsStructClassNullableClass()
                    .NoSubsetListFilter()
            where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()    
            select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in ValueRevealerDictTestData.AllPredicateFilteredDictExpectations
                where kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .NotHasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasFilterPredicate()
                        .AcceptsNullableStructs()
                        .NoSubsetListFilter()
                where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               ).ToList();

    public static IEnumerable<object[]> ValueRevealerSubListFilteredDictExpect =>
        valueRevealerSubListFilteredScaffoldingExpectations ??=
        (from kce in ValueRevealerDictTestData.AllValueRevealerSubListFilteredDictExpectations
            where kce.ValueTypeIsNotNullableStruct
            from scaffoldToCall in
                ScafReg
                    .IsAComplexType()
                    .ProcessesKeyedCollection()
                    .NotHasSupportsKeyRevealer()
                    .HasSupportsValueRevealer()
                    .HasSubsetListFilter()
                    .NoFilterPredicate()
                    .AcceptsStructClassNullableClass()
            select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in ValueRevealerDictTestData.AllValueRevealerSubListFilteredDictExpectations
                where kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .NotHasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasSubsetListFilter()
                        .NoFilterPredicate()
                        .AcceptsNullableStructs()
                select new object[] { kce, scaffoldToCall }
               ).ToList();

    public static IEnumerable<object[]> BothRevealersUnfilteredDictExpect =>
        bothRevealersUnfilteredScaffoldingExpectations ??=
        (from kce in BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations
         where kce.KeyTypeIsNotNullableStruct && kce.ValueTypeIsNotNullableStruct
         from scaffoldToCall in
            ScafReg
                .IsAComplexType()
                .ProcessesKeyedCollection()
                .HasSupportsKeyRevealer()
                .HasSupportsValueRevealer()
                .NoFilterPredicate()
                .NoSubsetListFilter()
                .AcceptsStructClassNullableClass()
                .AcceptsKeyIsNotNullableStruct()
         where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
        select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNotNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsStructClassNullableClass()
                where scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations
                where kce.KeyTypeIsNotNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyIsNotNullableStruct()
                        .AcceptsNullableStructs()
                where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersUnfilteredDictExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsNullableStructs()
                where scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               ).ToList();

    public static IEnumerable<object[]> BothRevealersPredicateFilteredDictExpect =>
        bothRevealersPredicateFilteredScaffoldingExpectations ??=
        (from kce in BothRevealersDictTestData.AllPredicateFilteredKeyedCollectionsExpectations
         where kce.KeyTypeIsNotNullableStruct && kce.ValueTypeIsNotNullableStruct
         from scaffoldToCall in
            ScafReg
                .IsAComplexType()
                .ProcessesKeyedCollection()
                .HasSupportsKeyRevealer()
                .HasSupportsValueRevealer()
                .HasFilterPredicate()
                .NoSubsetListFilter()
                .AcceptsStructClassNullableClass()
                .AcceptsKeyIsNotNullableStruct()
         where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()   
        select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in BothRevealersDictTestData.AllPredicateFilteredKeyedCollectionsExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNotNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsStructClassNullableClass()
                where scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()   
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllPredicateFilteredKeyedCollectionsExpectations
                where kce.KeyTypeIsNotNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyIsNotNullableStruct()
                        .AcceptsNullableStructs()
                where !kce.KeyTypeIsNullable || scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllPredicateFilteredKeyedCollectionsExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .HasFilterPredicate()
                        .NoSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsNullableStructs()
                where scaffoldToCall.ScaffoldingFlags.DoesNotHaveAcceptsDictionary()   
                select new object[] { kce, scaffoldToCall }
               ).ToList();

    public static IEnumerable<object[]> BothRevealersSubListFilteredDictExpect =>
        bothRevealersSubListFilteredScaffoldingExpectations ??=
        (from kce in BothRevealersDictTestData.AllBothRevealersSubListFilteredDictExpectations
         where kce.KeyTypeIsNotNullableStruct && kce.ValueTypeIsNotNullableStruct
         from scaffoldToCall in
            ScafReg
                .IsAComplexType()
                .ProcessesKeyedCollection()
                .HasSupportsKeyRevealer()
                .HasSupportsValueRevealer()
                .NoFilterPredicate()
                .HasSubsetListFilter()
                .AcceptsStructClassNullableClass()
                .AcceptsKeyIsNotNullableStruct()
        select new object[] { kce, scaffoldToCall })
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersSubListFilteredDictExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNotNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .HasSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsStructClassNullableClass()
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersSubListFilteredDictExpectations
                where kce.KeyTypeIsNotNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .HasSubsetListFilter()
                        .AcceptsKeyIsNotNullableStruct()
                        .AcceptsNullableStructs()
                select new object[] { kce, scaffoldToCall }
               )
        .Concat(
                from kce in BothRevealersDictTestData.AllBothRevealersSubListFilteredDictExpectations
                where kce.KeyTypeIsNullableStruct  && kce.ValueTypeIsNullableStruct
                from scaffoldToCall in
                    ScafReg
                        .IsAComplexType()
                        .ProcessesKeyedCollection()
                        .HasSupportsKeyRevealer()
                        .HasSupportsValueRevealer()
                        .NoFilterPredicate()
                        .HasSubsetListFilter()
                        .AcceptsKeyNullableStruct()
                        .AcceptsNullableStructs()
                select new object[] { kce, scaffoldToCall }
               ).ToList();
}
