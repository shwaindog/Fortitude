#region

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothFormatStrings
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothFormatStrings
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothFormatStrings
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new Dictionary<TKey, TValue>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current.Key, value.Current.Value);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new Dictionary<TKey, TValue>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current.Key, value.Current.Value);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().CachedCSharpNameWithConstraints()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new Dictionary<TKey, TValue>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current.Key, value.Current.Value);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}
