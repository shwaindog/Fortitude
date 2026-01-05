using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.
    FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArray
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArray)
              , AlwaysWithSelectKeysFromArray
              , DisplayKeys?.ToArray()!
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpan
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpan)
              , AlwaysWithSelectKeysFromSpan
              , (DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty)
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpan
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpan)
              , AlwaysWithSelectKeysFromReadOnlySpan
              , (DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty)
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromList
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromList)
              , AlwaysWithSelectKeysFromList
              , DisplayKeys!
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromEnumerable 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromEnumerable );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysFromEnumerable)
              , AlwaysWithSelectKeysFromEnumerable
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)(DisplayKeys))!
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerator );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerator)
              , AlwaysWithSelectKeysKeyValuePairEnumerator
              , DisplayKeys?.GetEnumerator()!
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys?.ToArray()!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys?.ToArray()!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey 
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey 
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>  
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey 
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArrayBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayBothRevealers)
              , AlwaysWithSelectKeysFromArrayBothRevealers
              , DisplayKeys?.ToArray()!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>  
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey 
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromArrayBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArrayBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayBothRevealers)
              , AlwaysWithSelectKeysFromArrayBothRevealers
              , DisplayKeys?.ToArray()!, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanBothRevealers)
              , AlwaysWithSelectKeysFromSpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromSpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanBothRevealers)
              , AlwaysWithSelectKeysFromSpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers)
              , AlwaysWithSelectKeysFromReadOnlySpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers)
              , AlwaysWithSelectKeysFromReadOnlySpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromListBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListBothRevealers)
              , AlwaysWithSelectKeysFromListBothRevealers
              , DisplayKeys!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromListBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromListBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListBothRevealers)
              , AlwaysWithSelectKeysFromListBothRevealers
              , DisplayKeys!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers), AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer, KeyRevealer)
           .Complete();
}
