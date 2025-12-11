using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayBothFormatStrings)
              , AlwaysWithSelectKeysFromArrayBothFormatStrings
              , DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpanBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanBothFormatStrings)
              , AlwaysWithSelectKeysFromSpanBothFormatStrings
              , DisplayKeys.ToArray().AsSpan(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings)
              , AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromListBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListBothFormatStrings)
              , AlwaysWithSelectKeysFromListBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromEnumerableBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromEnumerableBothFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysFromEnumerableBothFormatStrings)
              , AlwaysWithSelectKeysFromEnumerableBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings );

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
              , DisplayKeys.GetEnumerator(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
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
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
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
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers), AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyRevealer)
           .Complete();
}
