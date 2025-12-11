#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothFormatStrings
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothFormatStrings);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothFormatStrings
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue>>();
            var shouldContinue = value.MoveNext();
            while (shouldContinue)
            {
                newValue.Add(value.Current);
                shouldContinue = value.MoveNext();
            }
            Value = newValue;
        }
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}
