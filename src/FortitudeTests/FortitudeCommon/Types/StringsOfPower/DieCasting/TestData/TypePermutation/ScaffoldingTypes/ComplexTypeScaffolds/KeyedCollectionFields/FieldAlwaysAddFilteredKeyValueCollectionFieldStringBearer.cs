#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothFormatStrings
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothFormatStrings
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothFormatStrings
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new Dictionary<TKey, TValue?>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current.Key, value.Current.Value);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase, TVRevealBase
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase, TVRevealBase
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase, TVRevealBase
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListValueRevealerKeyFormatString
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase, TVRevealBase
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase, TVRevealBase
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new Dictionary<TKey, TValue?>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current.Key, value.Current.Value);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase, TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothRevealers
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase, TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase, TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothRevealers
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase, TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase, TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new Dictionary<TKey, TValue?>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current.Key, value.Current.Value);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}
