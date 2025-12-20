using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonNullWrites | SubsetListFilter
                | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromArray
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromArray)
              , WhenNonNullAddWithSelectKeysFromArray
              , DisplayKeys?.ToArray()!
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonNullWrites | SubsetListFilter
                | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromSpan
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromSpan)
              , WhenNonNullAddWithSelectKeysFromSpan
              , (DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty)
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonNullWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromReadOnlySpan
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpan)
              , WhenNonNullAddWithSelectKeysFromReadOnlySpan
              , (DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty)
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonNullWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromList
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromList)
              , WhenNonNullAddWithSelectKeysFromList
              , DisplayKeys!
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonNullWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromEnumerable 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromEnumerable );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysFromEnumerable)
              , WhenNonNullAddWithSelectKeysFromEnumerable
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)(DisplayKeys))!
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerator
              , DisplayKeys?.GetEnumerator()!
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonNullWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys?.ToArray()!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonNullWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys?.ToArray()!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonNullWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull

{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonNullWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey

{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString)
              , WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString)
              , WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonNullWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromArrayBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromArrayBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromArrayBothRevealers)
              , WhenNonNullAddWithSelectKeysFromArrayBothRevealers
              , DisplayKeys?.ToArray()!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonNullWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysFromArrayBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromArrayBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromArrayBothRevealers)
              , WhenNonNullAddWithSelectKeysFromArrayBothRevealers
              , DisplayKeys?.ToArray()!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonNullWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromSpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromSpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromSpanBothRevealers)
              , WhenNonNullAddWithSelectKeysFromSpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonNullWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysFromSpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromSpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromSpanBothRevealers)
              , WhenNonNullAddWithSelectKeysFromSpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonNullWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers)
              , WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonNullWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers)
              , WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromListBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromListBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromListBothRevealers)
              , WhenNonNullAddWithSelectKeysFromListBothRevealers
              , DisplayKeys!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysFromListBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromListBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromListBothRevealers)
              , WhenNonNullAddWithSelectKeysFromListBothRevealers
              , DisplayKeys!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer, KeyRevealer)
           .Complete();
}
