// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;

public record ScaffoldingPartEntry(Type ScaffoldingType, ScaffoldingStringBuilderInvokeFlags ScaffoldingFlags)
{
    private static readonly Type MoldSupportedDefaultValueType          = typeof(IMoldSupportedDefaultValue<>);
    private static readonly Type SupportsSubsetDisplayKeysType          = typeof(ISupportsSubsetDisplayKeys<>);
    private static readonly Type SupportsOrderedCollectionPredicateType = typeof(ISupportsOrderedCollectionPredicate<>);
    private static readonly Type SupportsKeyedCollectionPredicateType   = typeof(ISupportsKeyedCollectionPredicate<,>);
    private static readonly Type SupportsValueRevealerType              = typeof(ISupportsValueRevealer<>);
    private static readonly Type SupportsKeyRevealerType              = typeof(ISupportsKeyRevealer<>);

    public Type ValueType { get; } = ScaffoldingType.MoldSupportedValueGetValueType();


    public int RequiredNumOfTypeArguments => ScaffoldingType.IsGenericType ? ScaffoldingType.GenericTypeArguments.Length : 0;

    public bool IsValueMold => ScaffoldingFlags.HasAnyOf(SimpleType);

    public bool IsComplexMold => ScaffoldingFlags.HasAnyOf(ComplexType);

    public bool IsOrderedCollectionMold => ScaffoldingFlags.HasAnyOf(OrderedCollectionType);

    public bool IsKeyedCollectionMold => ScaffoldingFlags.HasAnyOf(KeyedCollectionType);

    public bool SupportsDefaultValue => ScaffoldingType.ImplementsGenericTypeInterface(MoldSupportedDefaultValueType);

    public bool SupportsValueFormatString => ScaffoldingType.ImplementsInterface<ISupportsValueFormatString>();

    public bool SupportsKeyFormatString => ScaffoldingType.ImplementsInterface<ISupportsKeyFormatString>();

    public bool SupportsSubSetKeys => ScaffoldingType.ImplementsGenericTypeInterface(SupportsSubsetDisplayKeysType);

    public bool SupportsOrderedCollectionPredicate => ScaffoldingType.ImplementsGenericTypeInterface(SupportsOrderedCollectionPredicateType);

    public bool SupportsKeyedCollectionPredicate => ScaffoldingType.ImplementsGenericTypeInterface(SupportsKeyedCollectionPredicateType);

    public bool SupportsValueRevealer => ScaffoldingType.ImplementsGenericTypeInterface(SupportsValueRevealerType);

    public bool SupportsKeyRevealer => ScaffoldingType.ImplementsGenericTypeInterface(SupportsKeyRevealerType);

    public bool SupportsIndexRangeLimiting => ScaffoldingType.ImplementsInterface<ISupportsIndexRangeLimiting>();

    public bool SupportsCustomFieldHandling => ScaffoldingType.ImplementsInterface<ISupportsFieldHandling>();

    public bool SupportsSettingValueFromString => ScaffoldingType.ImplementsInterface<ISupportsSettingValueFromString>();

    public string Name { get; } = ScaffoldingType.Name;

    public Func<IStringBearer> CreateStringBearerFunc(params Type[] genericTypeArguments)
        => ReflectionHelper.GenericTypeDefaultCtorBinder<IStringBearer>(ScaffoldingType, genericTypeArguments);
};

public static class ScaffoldingRegistry
{
    private static readonly List<ScaffoldingPartEntry> ScaffoldingTypes = new();

    private static readonly Type MoldSupportedValueInterfaceType = typeof(IMoldSupportedValue<>);

    static ScaffoldingRegistry()
    {
        var types =
            typeof(ScaffoldingPartEntry)
                .Assembly.GetAllTopLevelClassTypes()
                .Where(t => t.GetCustomAttributes(typeof(TypeGeneratePartAttribute)).Any() &&
                            t.ImplementsInterface<IStringBearer>())
                .ToList();

        foreach (var scaffoldType in types)
        {
            var typeGenerateAttrib = (TypeGeneratePartAttribute)scaffoldType.GetCustomAttributes(typeof(TypeGeneratePartAttribute)).First();
            var scaffoldPartEntry  = new ScaffoldingPartEntry(scaffoldType, typeGenerateAttrib.ScaffoldingFlags);
            ScaffoldingTypes.Add(scaffoldPartEntry);
        }

        AllScaffoldingTypes = ScaffoldingTypes.AsReadOnly();
    }

    public static IReadOnlyList<ScaffoldingPartEntry> AllScaffoldingTypes { get; }

    public static Type MoldSupportedValueGetValueType(this Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == MoldSupportedValueInterfaceType) return type.GenericTypeArguments.Single();
        var allInterfaces = type.GetInterfaces().ToList();

        var moldInterface  = allInterfaces.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == MoldSupportedValueInterfaceType);
        var genericMoldArg = moldInterface.GenericTypeArguments.Single();
        return genericMoldArg;
    }

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeFieldAlwaysAddFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesSingleValue().AlwaysWrites();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeFieldWhenNonDefaultAddFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesSingleValue().PopulatedWrites();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeFieldWhenNonNullAddFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesSingleValue().NonNullWrites();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeFieldWhenNonNullOrDefaultAddFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesSingleValue().NonNullAndPopulatedWrites();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeCollectionFieldAlwaysAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesCollection().AlwaysWrites().NoFilterPredicate();


    public static IEnumerable<ScaffoldingPartEntry>
        ComplexTypeCollectionFieldWhenNonNullAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesCollection().NonNullWrites().NoFilterPredicate();


    public static IEnumerable<ScaffoldingPartEntry>
        ComplexTypeCollectionFieldWhenPopulatedAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesCollection().NonNullAndPopulatedWrites().NoFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeCollectionFieldWhenNonNullAddFilteredFilter(
        this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesCollection().NonNullWrites().HasFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry>
        ComplexTypeCollectionFieldWhenPopulatedWithFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesCollection().NonNullAndPopulatedWrites().HasFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry>
        ComplexTypeCollectionFieldAlwaysAddFilteredFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesCollection().AlwaysWrites().HasFilterPredicate();


    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldAlwaysAddAllFilter
        (this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesKeyedCollection().AlwaysWrites().NoFilterPredicate().NoSubsetList();
    
    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldWhenNonNullAddAllFilter
        (this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesKeyedCollection().NonNullWrites().NoFilterPredicate().NoSubsetList();
    
    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldAlwaysAddFiltered(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesKeyedCollection().AlwaysWrites().HasFilterPredicate();
    
    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldWhenNonNullAddFiltered(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesKeyedCollection().NonNullWrites().HasFilterPredicate();
    
    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesKeyedCollection().PopulatedWrites().HasFilterPredicate();
    
    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldAlwaysAddSelectKeysFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesKeyedCollection().AlwaysWrites().NoFilterPredicate().HasSubsetList();
    
    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldWhenNonNullAddSelectKeysFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesKeyedCollection().NonNullWrites().NoFilterPredicate().HasSubsetList();
    
    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldPopulatedWithSelectKeysFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsComplexType().ProcessesKeyedCollection().PopulatedWrites().NoFilterPredicate().HasSubsetList();
    
    public static IEnumerable<ScaffoldingPartEntry> OrderedCollectionAlwaysAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsOrderedCollectionType().ProcessesCollection().NoFilterPredicate();
    
    public static IEnumerable<ScaffoldingPartEntry> OrderedCollectionAddFiltered(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsOrderedCollectionType().ProcessesCollection().HasFilterPredicate();
    
    
    public static IEnumerable<ScaffoldingPartEntry> KeyedCollectionAlwaysAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsKeyedCollectionType().ProcessesKeyedCollection().NoFilterPredicate().NoSubsetList();
    
    public static IEnumerable<ScaffoldingPartEntry> KeyedCollectionAddFiltered(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsKeyedCollectionType().ProcessesKeyedCollection().HasFilterPredicate();
    
    public static IEnumerable<ScaffoldingPartEntry> KeyedCollectionAddWithSelectedKeysFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.IsKeyedCollectionType().ProcessesKeyedCollection().NoFilterPredicate().HasSubsetList();

    public static IEnumerable<ScaffoldingPartEntry> IsSimpleType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SimpleType));

    public static IEnumerable<ScaffoldingPartEntry> IsComplexType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ComplexType));

    public static IEnumerable<ScaffoldingPartEntry> IsOrderedCollectionType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(OrderedCollectionType));

    public static IEnumerable<ScaffoldingPartEntry> IsKeyedCollectionType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(KeyedCollectionType));

    public static IEnumerable<ScaffoldingPartEntry> ProcessesSingleValue(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(AcceptsSingleValue));

    public static IEnumerable<ScaffoldingPartEntry> ProcessesCollection(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsCollection));
    
    public static IEnumerable<ScaffoldingPartEntry> ProcessesKeyedCollection(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(AcceptsKeyValueCollection));

    public static IEnumerable<ScaffoldingPartEntry> HasSubsetList(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SubsetListFilter));

    public static IEnumerable<ScaffoldingPartEntry> NoSubsetList(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(SubsetListFilter));

    public static IEnumerable<ScaffoldingPartEntry> HasFilterPredicate(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(FilterPredicate));

    public static IEnumerable<ScaffoldingPartEntry> NoFilterPredicate(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(FilterPredicate));

    public static IEnumerable<ScaffoldingPartEntry> NoEnumerableOrEnumerator(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(AcceptsEnumerable | AcceptsEnumerator) ||
                            (spe.ScaffoldingFlags.HasAllOf(AcceptsAny)
                          && !(spe.Name.Contains("MatchEnumerable")
                            || spe.Name.Contains("MatchEnumerator")
                            || spe.Name.Contains("ObjectEnumerable") 
                             || spe.Name.Contains("ObjectEnumerator"))));

    public static IEnumerable<ScaffoldingPartEntry> AlwaysWrites(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AlwaysWrites));

    public static IEnumerable<ScaffoldingPartEntry> NonNullWrites(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.NonNullWrites));

    public static IEnumerable<ScaffoldingPartEntry> NonNullAndPopulatedWrites(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.NonNullAndPopulatedWrites));

    public static IEnumerable<ScaffoldingPartEntry> PopulatedWrites(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(OnlyPopulatedWrites));


    public static IEnumerable<ScaffoldingPartEntry> AcceptsChars(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsChars));
    
    public static IEnumerable<ScaffoldingPartEntry> HasSupportsValueFormatString(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsValueFormatString));
    
    public static IEnumerable<ScaffoldingPartEntry> HasSupportsKeyFormatString(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsKeyFormatString));
    
    public static IEnumerable<ScaffoldingPartEntry> HasSupportsValueRevealer(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsValueRevealer));
    
    public static IEnumerable<ScaffoldingPartEntry> HasSupportsKeyRevealer(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsKeyRevealer));
    
    public static IEnumerable<ScaffoldingPartEntry> HasSupportsIndexSubRanges(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges));


    public static IEnumerable<ScaffoldingPartEntry> HasAcceptsAny(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsAny));

    public static IEnumerable<ScaffoldingPartEntry> NotHasAcceptsAny(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => !spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsAny));


    public static IEnumerable<ScaffoldingPartEntry> AcceptsStringBearer(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsStringBearer));

    


}
