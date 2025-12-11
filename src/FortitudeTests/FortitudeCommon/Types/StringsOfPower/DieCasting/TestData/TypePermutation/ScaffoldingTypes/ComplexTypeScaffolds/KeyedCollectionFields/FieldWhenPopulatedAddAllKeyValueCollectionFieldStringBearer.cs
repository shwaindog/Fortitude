#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothFormatStrings
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}
