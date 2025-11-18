#region

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothFormatStrings
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothFormatStrings
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothFormatStrings
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListValueRevealerKeyFormatStrings
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListValueRevealerKeyFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothRevealers
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothRevealers
    {
        get => Value?.ToList();
        set => Value = value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value as Dictionary<TKey, TValue?> ?? value?.ToDictionary();
    }

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}
