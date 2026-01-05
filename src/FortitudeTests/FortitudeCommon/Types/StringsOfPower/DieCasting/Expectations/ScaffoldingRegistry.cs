// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Reflection;
using FortitudeCommon.DataStructures.Lists.PositionAware;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;

public record ScaffoldingPartEntry(Type ScaffoldingType, ScaffoldingStringBuilderInvokeFlags ScaffoldingFlags)
    : IComparable<ScaffoldingPartEntry>, ICodeLocationAwareListItem
{
    private static readonly Type MoldSupportedDefaultValueType          = typeof(IMoldSupportedDefaultValue<>);
    private static readonly Type SupportsSubsetDisplayKeysType          = typeof(ISupportsSubsetDisplayKeys<>);
    private static readonly Type SupportsOrderedCollectionPredicateType = typeof(ISupportsOrderedCollectionPredicate<>);
    private static readonly Type SupportsKeyedCollectionPredicateType   = typeof(ISupportsKeyedCollectionPredicate<,>);
    private static readonly Type SupportsValueRevealerType              = typeof(ISupportsValueRevealer<>);
    private static readonly Type SupportsKeyRevealerType                = typeof(ISupportsKeyRevealer<>);

    public Type ValueType { get; } = ScaffoldingType.MoldSupportedValueGetValueType();

    public int RequiredNumOfTypeArguments => ScaffoldingType.IsGenericType ? ScaffoldingType.GenericTypeArguments.Length : 0;

    public bool IsValueMold => ScaffoldingFlags.HasAnyOf(IsContentType);

    public bool IsAComplexMold => ScaffoldingFlags.HasAnyOf(IsComplexType);

    public bool IsOrderedCollectionMold => ScaffoldingFlags.HasAnyOf(IsOrderedCollectionType);

    public bool IsKeyedCollectionMold => ScaffoldingFlags.HasAnyOf(IsKeyedCollectionType);

    public bool SupportsDefaultValue => ScaffoldingType.ImplementsGenericTypeInterface(MoldSupportedDefaultValueType);

    public bool SupportsValueFormatString => ScaffoldingType.ImplementsInterface<ISupportsValueFormatString>();

    public bool SupportsKeyFormatString => ScaffoldingType.ImplementsInterface<ISupportsKeyFormatString>();

    public bool SupportsSubSetKeys => ScaffoldingType.ImplementsGenericTypeInterface(SupportsSubsetDisplayKeysType);

    public bool SupportsOrderedCollectionPredicate => ScaffoldingType.ImplementsGenericTypeInterface(SupportsOrderedCollectionPredicateType);

    public bool SupportsKeyedCollectionPredicate => ScaffoldingType.ImplementsGenericTypeInterface(SupportsKeyedCollectionPredicateType);

    public bool SupportsValueRevealer => ScaffoldingType.ImplementsGenericTypeInterface(SupportsValueRevealerType);

    public bool SupportsKeyRevealer => ScaffoldingType.ImplementsGenericTypeInterface(SupportsKeyRevealerType);

    public bool SupportsIndexRangeLimiting => ScaffoldingType.ImplementsInterface<ISupportsIndexRangeLimiting>();

    public bool SupportsSettingValueFromString => ScaffoldingType.ImplementsInterface<ISupportsSettingValueFromString>();

    public string Name { get; } = ScaffoldingType.CachedCSharpNameNoConstraints();

    public int AtIndex { get; set; }

    public Type? ListOwningType { get; set; }

    public string? ListMemberName { get; set; }

    public string ItemCodePath => ListOwningType != null
        ? $"{ListOwningType.Name}.{ListMemberName}[{AtIndex}]"
        : $"UnsetListOwnerType.UnknownListMemberName[{AtIndex}]";

    public Func<ISinglePropertyTestStringBearer> CreateStringBearerFunc(params Type[] genericTypeArguments)
    {
        try
        {
            if (ScaffoldingType.IsGenericType)
            {
                return ReflectionHelper.GenericTypeDefaultCtorBinder<ISinglePropertyTestStringBearer>(ScaffoldingType, genericTypeArguments);
            }
            return ReflectionHelper.DefaultCtorFunc<ISinglePropertyTestStringBearer>(ScaffoldingType);
        }
        catch (Exception e)
        {
            Console.Out.WriteLine
                ($"Problem trying to create {Name} with generic arguments " +
                 $"{string.Join(",", genericTypeArguments.Select(t => t.CachedCSharpNameWithConstraints()))}." +
                 $"  Got {e}");
            throw;
        }
    }

    public Func<TScaffold> CreateTypedStringBearerFunc<TScaffold>()
    {
        try
        {
            if (ScaffoldingType.IsGenericType) { return ReflectionHelper.GenericTypeDefaultCtorBinder<TScaffold>(); }
            return ReflectionHelper.DefaultCtorFunc<TScaffold>();
        }
        catch (Exception e)
        {
            Console.Out.WriteLine($"Problem trying to create {Name}.  Got {e}");
            throw;
        }
    }

    public int CompareTo(ScaffoldingPartEntry? other) => String.Compare(Name, other?.Name ?? "none", StringComparison.InvariantCulture);
};

public static class ScaffoldingRegistry
{
    // ReSharper disable once ExplicitCallerInfoArgument
    private static readonly PositionUpdatingList<ScaffoldingPartEntry> ScaffoldingTypes =
        new(typeof(ScaffoldingRegistry), "AllScaffoldingTypes");

    private static readonly Type MoldSupportedValueInterfaceType = typeof(IMoldSupportedValue<>);

    static ScaffoldingRegistry()
    {
        var types =
            typeof(ScaffoldingPartEntry)
                .Assembly.GetAllTopLevelClassTypes()
                .Where(t =>
                           !t.IsAbstract &&
                           t.GetCustomAttributes(typeof(TypeGeneratePartAttribute)).Any() &&
                           t.ImplementsInterface<IStringBearer>())
                .ToList();

        for (var i = 0; i < types.Count; i++)
        {
            var scaffoldType       = types[i];
            var typeGenerateAttrib = (TypeGeneratePartAttribute)scaffoldType.GetCustomAttributes(typeof(TypeGeneratePartAttribute)).First();
            var scaffoldPartEntry  = new ScaffoldingPartEntry(scaffoldType, typeGenerateAttrib.ScaffoldingFlags);
            ScaffoldingTypes.Add(scaffoldPartEntry);
        }

        AllScaffoldingTypes = ScaffoldingTypes.AsReadOnly();
    }

    public static ScaffoldingPartEntry GetScaffoldPartEntry(int key) => ScaffoldingTypes[key];

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
        subSet
            .IsAComplexType()
            .IsNotContentType()
            .ProcessesSingleValue()
            .AlwaysWrites();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeFieldWhenNonDefaultAddFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .IsNotContentType()
            .ProcessesSingleValue()
            .PopulatedWrites();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeFieldWhenNonNullAddFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .IsNotContentType()
            .ProcessesSingleValue()
            .NonNullWrites();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeFieldWhenNonNullOrDefaultAddFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .IsNotContentType()
            .ProcessesSingleValue()
            .NonNullAndPopulatedWrites();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeCollectionFieldAlwaysAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesCollection()
            .AlwaysWrites()
            .NoFilterPredicate();
    
    public static IEnumerable<ScaffoldingPartEntry>
        ComplexTypeCollectionFieldWhenNonNullAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesCollection()
            .NonNullWrites()
            .NoFilterPredicate();
    
    public static IEnumerable<ScaffoldingPartEntry>
        ComplexTypeCollectionFieldWhenPopulatedAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesCollection()
            .NonNullAndPopulatedWrites()
            .NoFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeCollectionFieldWhenNonNullAddFilteredFilter(
        this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesCollection()
            .NonNullWrites()
            .HasFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry>
        ComplexTypeCollectionFieldWhenPopulatedWithFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesCollection()
            .NonNullAndPopulatedWrites()
            .HasFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry>
        ComplexTypeCollectionFieldAlwaysAddFilteredFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesCollection()
            .AlwaysWrites()
            .HasFilterPredicate();
    
    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldAlwaysAddAllFilter
        (this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesKeyedCollection()
            .AlwaysWrites()
            .NoFilterPredicate()
            .NoSubsetList();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldWhenNonNullAddAllFilter
        (this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesKeyedCollection()
            .NonNullWrites()
            .NoFilterPredicate()
            .NoSubsetList();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldAlwaysAddFiltered(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesKeyedCollection()
            .AlwaysWrites()
            .HasFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldWhenNonNullAddFiltered(
        this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesKeyedCollection()
            .NonNullWrites()
            .HasFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilter(
        this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesKeyedCollection()
            .PopulatedWrites()
            .HasFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldAlwaysAddSelectKeysFilter(
        this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesKeyedCollection()
            .AlwaysWrites()
            .NoFilterPredicate()
            .HasSubsetListFilter();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldWhenNonNullAddSelectKeysFilter(
        this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesKeyedCollection()
            .NonNullWrites()
            .NoFilterPredicate()
            .HasSubsetListFilter();

    public static IEnumerable<ScaffoldingPartEntry> ComplexTypeKeyedCollectionFieldPopulatedWithSelectKeysFilter(
        this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAComplexType()
            .ProcessesKeyedCollection()
            .PopulatedWrites()
            .NoFilterPredicate()
            .HasSubsetListFilter();

    public static IEnumerable<ScaffoldingPartEntry> OrderedCollectionAlwaysAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAnOrderedCollectionType()
            .ProcessesCollection()
            .NoFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry> OrderedCollectionAddFiltered(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsAnOrderedCollectionType()
            .ProcessesCollection()
            .HasFilterPredicate();
    
    public static IEnumerable<ScaffoldingPartEntry> KeyedCollectionAlwaysAddAllFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsKeyedCollectionType()
            .ProcessesKeyedCollection()
            .NoFilterPredicate()
            .NoSubsetList();

    public static IEnumerable<ScaffoldingPartEntry> KeyedCollectionAddFiltered(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsKeyedCollectionType()
            .ProcessesKeyedCollection()
            .HasFilterPredicate();

    public static IEnumerable<ScaffoldingPartEntry> KeyedCollectionAddWithSelectedKeysFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet
            .IsKeyedCollectionType()
            .ProcessesKeyedCollection()
            .NoFilterPredicate()
            .HasSubsetListFilter();

    public static IEnumerable<ScaffoldingPartEntry> IsContentType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.IsContentType));

    public static IEnumerable<ScaffoldingPartEntry> IsNotContentType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(ScaffoldingStringBuilderInvokeFlags.IsContentType));

    public static IEnumerable<ScaffoldingPartEntry> IsAComplexType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(IsComplexType));

    public static IEnumerable<ScaffoldingPartEntry> IsJustComplexType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasComplexTypeFlag()
                         && spe.ScaffoldingFlags.HasNoneOf(ScaffoldingStringBuilderInvokeFlags.IsContentType | IsOrderedCollectionType | ScaffoldingStringBuilderInvokeFlags.IsKeyedCollectionType));

    public static IEnumerable<ScaffoldingPartEntry> IsAnOrderedCollectionType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasOrderedCollectionTypeFlag());

    public static IEnumerable<ScaffoldingPartEntry> IsNotOrderedCollectionType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.DoesNotHaveOrderedCollectionTypeFlag());

    public static IEnumerable<ScaffoldingPartEntry> IsKeyedCollectionType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.IsKeyedCollectionType));

    public static IEnumerable<ScaffoldingPartEntry> IsNotKeyedCollectionType(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(ScaffoldingStringBuilderInvokeFlags.IsKeyedCollectionType));

    public static IEnumerable<ScaffoldingPartEntry> ProcessesSingleValue(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SingleValueCardinality));

    public static IEnumerable<ScaffoldingPartEntry> ProcessesCollection(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(CollectionCardinality));

    public static IEnumerable<ScaffoldingPartEntry> ProcessesKeyedCollection(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(KeyValueCardinality));

    public static IEnumerable<ScaffoldingPartEntry> HasSubsetListFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SubsetListFilter));

    public static IEnumerable<ScaffoldingPartEntry> NoSubsetList(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(SubsetListFilter));

    public static IEnumerable<ScaffoldingPartEntry> HasFilterPredicate(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasFilterPredicate());

    public static IEnumerable<ScaffoldingPartEntry> NoFilterPredicate(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.DoesNotHaveFilterPredicate());

    public static IEnumerable<ScaffoldingPartEntry> NoSubsetListFilter(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(SubsetListFilter));

    public static IEnumerable<ScaffoldingPartEntry> NoEnumerableOrEnumerator(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(AcceptsEnumerable | AcceptsEnumerator) ||
                            (spe.ScaffoldingFlags.HasAllOf(AcceptsAnyGeneric)
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
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(NonDefaultWrites));


    public static IEnumerable<ScaffoldingPartEntry> AcceptsChars(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsChars));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsString(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsString));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsCharArray(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsCharArray));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsCharSequence(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsCharSequence));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsStringBuilder(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(ScaffoldingStringBuilderInvokeFlags.AcceptsStringBuilder));

    public static IEnumerable<ScaffoldingPartEntry> HasSupportsValueFormatString(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsValueFormatString));

    public static IEnumerable<ScaffoldingPartEntry> HasNotSupportsValueFormatString(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(SupportsValueFormatString));

    public static IEnumerable<ScaffoldingPartEntry> HasSupportsKeyFormatString(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsKeyFormatString));

    public static IEnumerable<ScaffoldingPartEntry> HasSupportsValueRevealer(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsValueRevealer));

    public static IEnumerable<ScaffoldingPartEntry> NotHasSupportsValueRevealer(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(SupportsValueRevealer));

    public static IEnumerable<ScaffoldingPartEntry> HasSupportsKeyRevealer(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsKeyRevealer));

    public static IEnumerable<ScaffoldingPartEntry> NotHasSupportsKeyRevealer(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(SupportsKeyRevealer));

    public static IEnumerable<ScaffoldingPartEntry> HasSupportsIndexSubRanges(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(SupportsIndexSubRanges));

    public static IEnumerable<ScaffoldingPartEntry> HasSpanFormattable(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(AcceptsSpanFormattable));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsBoolean(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.IsAcceptsBool());

    public static IEnumerable<ScaffoldingPartEntry> AcceptsNullables(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsNullableClass | AcceptsNullableStruct));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsNullableStructs(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsNullableStruct));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsKeyNullableStruct(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(KeyNullableStruct));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsKeyIsNotNullableStruct(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(KeyNullableStruct));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsNullableClasses(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsNullableClass));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsOnlyNonNullables(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsClass | AcceptsStruct)
                         && spe.ScaffoldingFlags.HasNoneOf(AcceptsNullableClass | AcceptsNullableStruct));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsNonNullables(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsClass | AcceptsStruct));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsNonNullStructs(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsStruct));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsStructClassNullableClass(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsClass | AcceptsStruct | AcceptsNullableClass));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsAllButNullStructs(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsClass | AcceptsStruct | AcceptsNullableClass)
                         && spe.ScaffoldingFlags.HasNoneOf(AcceptsNullableStruct));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsNonNullClasses(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsClass) && spe.ScaffoldingFlags.HasNoneOf(AcceptsNullableClass));

    public static IEnumerable<ScaffoldingPartEntry> AcceptsClasses(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAnyOf(AcceptsClass | AcceptsNullableClass));

    public static IEnumerable<ScaffoldingPartEntry> NoExplicitAcceptsNullables(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(AcceptsNullableClass | AcceptsNullableStruct) ||
                            spe.ScaffoldingFlags.HasAllOf(AcceptsAnyGeneric));

    public static IEnumerable<ScaffoldingPartEntry> NoExplicitAcceptsNonNullables(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasNoneOf(AcceptsClass | AcceptsStruct) || spe.ScaffoldingFlags.IsAcceptsAnyGeneric());


    public static IEnumerable<ScaffoldingPartEntry> HasAcceptsAny(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(AcceptsAnyGeneric));

    public static IEnumerable<ScaffoldingPartEntry> NotHasAcceptsAny(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => !spe.ScaffoldingFlags.HasAllOf(AcceptsAnyMask));


    public static IEnumerable<ScaffoldingPartEntry> HasAcceptsStringBearer(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(AcceptsStringBearer));

    public static IEnumerable<ScaffoldingPartEntry> HasTreatedAsValueOut(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(DefaultTreatedAsValueOut));

    public static IEnumerable<ScaffoldingPartEntry> HasTreatedAsStringOut(this IEnumerable<ScaffoldingPartEntry> subSet) =>
        subSet.Where(spe => spe.ScaffoldingFlags.HasAllOf(DefaultTreatedAsStringOut));
}
