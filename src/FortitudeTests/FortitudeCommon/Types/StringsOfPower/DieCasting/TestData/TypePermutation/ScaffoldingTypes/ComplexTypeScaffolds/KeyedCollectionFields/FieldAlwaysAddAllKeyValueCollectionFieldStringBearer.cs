#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothFormatStrings
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothFormatStrings!.ToArray()
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothFormatStrings
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothFormatStrings
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}
