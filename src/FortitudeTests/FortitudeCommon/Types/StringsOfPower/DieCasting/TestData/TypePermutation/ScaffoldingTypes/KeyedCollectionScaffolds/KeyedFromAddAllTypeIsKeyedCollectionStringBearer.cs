#region

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    KeyedCollectionScaffolds;

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryFormatStringsAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothFormatStrings , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairArrayBothFormatStringsAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothFormatStrings
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairListBothFormatStringsAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListBothFormatStrings
    {
        get => Value?.ToList();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairEnumerableBothFormatStringsAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairEnumeratorBothFormatStringsAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairArrayValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairListValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings
    {
        get => Value?.ToList();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairEnumerableValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairEnumeratorValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();

}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairArrayBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairListBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
    {
        get => Value?.ToList();
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairEnumerableBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairEnumeratorBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}
