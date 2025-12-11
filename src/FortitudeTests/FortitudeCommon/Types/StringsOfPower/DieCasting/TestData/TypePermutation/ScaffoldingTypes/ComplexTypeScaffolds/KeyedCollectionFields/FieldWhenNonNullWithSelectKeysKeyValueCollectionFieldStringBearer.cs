using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonNullWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromArrayBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromArrayBothFormatStrings)
              , WhenNonNullAddWithSelectKeysFromArrayBothFormatStrings
              , DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonNullWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromSpanBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromSpanBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromSpanBothFormatStrings)
              , WhenNonNullAddWithSelectKeysFromSpanBothFormatStrings
              , DisplayKeys.ToArray().AsSpan(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromReadOnlySpanBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanBothFormatStrings)
              , WhenNonNullAddWithSelectKeysFromReadOnlySpanBothFormatStrings
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromListBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromListBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromListBothFormatStrings)
              , WhenNonNullAddWithSelectKeysFromListBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromEnumerableBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysFromEnumerableBothFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysFromEnumerableBothFormatStrings)
              , WhenNonNullAddWithSelectKeysFromEnumerableBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
              , DisplayKeys.GetEnumerator(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonNullWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonNullWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonNullWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonNullWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonNullWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers), WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyRevealer)
           .Complete();
}
