using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonDefaultWrites | SubsetListFilter
                | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArray
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromArray);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArray)
              , WhenPopulatedWithSelectKeysFromArray
              , DisplayKeys?.ToArray()!
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonDefaultWrites | SubsetListFilter
                | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpan
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromSpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpan)
              , WhenPopulatedWithSelectKeysFromSpan
              , (DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty)
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpan
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromReadOnlySpan);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpan)
              , WhenPopulatedWithSelectKeysFromReadOnlySpan
              , (DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty)
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromList
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromList);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromList)
              , WhenPopulatedWithSelectKeysFromList
              , DisplayKeys!
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromEnumerable 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromEnumerable );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysFromEnumerable)
              , WhenPopulatedWithSelectKeysFromEnumerable
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)(DisplayKeys))!
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerator
              , DisplayKeys?.GetEnumerator()!
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonDefaultWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys?.ToArray()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonDefaultWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys?.ToArray()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonDefaultWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonDefaultWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonDefaultWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromArrayBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayBothRevealers)
              , WhenPopulatedWithSelectKeysFromArrayBothRevealers
              , DisplayKeys?.ToArray()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonDefaultWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysFromArrayBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromArrayBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayBothRevealers)
              , WhenPopulatedWithSelectKeysFromArrayBothRevealers
              , DisplayKeys?.ToArray()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonDefaultWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromSpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanBothRevealers)
              , WhenPopulatedWithSelectKeysFromSpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonDefaultWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysFromSpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromSpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanBothRevealers)
              , WhenPopulatedWithSelectKeysFromSpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonDefaultWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer
                | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers)
              , WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonDefaultWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer 
                | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers)
              , WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromListBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListBothRevealers)
              , WhenPopulatedWithSelectKeysFromListBothRevealers
              , DisplayKeys!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysFromListBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysFromListBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListBothRevealers)
              , WhenPopulatedWithSelectKeysFromListBothRevealers
              , DisplayKeys!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
