#region

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBoth
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBoth?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueArrayWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBoth
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBoth?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueListWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBoth
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBoth?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct 
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase> :
    FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerArrayWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString?.GetType() 
     ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerArrayWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase> :
    FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : struct
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString?.GetType() 
     ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerListWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerListWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerArrayWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerArrayWhenNonNullAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey?, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey?, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerArrayWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerArrayWhenNonNullAddFilteredStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public KeyValuePair<TKey?, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey?, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerListWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerListWhenNonNullAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerListWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerListWhenNonNullAddFilteredStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
