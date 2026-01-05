#region

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.Expectations.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType.FixtureScaffolding;

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryKeyValueAddAllStringBearer<TKey, TValue> :
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromArrayKeyValueAddAllStringBearer<TKey, TValue> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromListKeyValueAddAllStringBearer<TKey, TValue> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromEnumerableKeyValueAddAllStringBearer<TKey, TValue> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromEnumeratorKeyValueAddAllStringBearer<TKey, TValue> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromArrayKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromArrayKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public KeyValuePair<TKey, TValue?>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromListKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromListKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromEnumerableKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromEnumerableKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromEnumeratorKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromEnumeratorKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromArrayKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromArrayStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey?, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromArrayKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsNullableStruct
              | KeyNullableStruct  | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromArrayStructKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public KeyValuePair<TKey?, TValue?>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromListKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromListStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromListKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromListStructKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
              | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumerableKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumerableStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey?, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
               | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumerableKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumerableStructKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey?, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumeratorKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumeratorStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey?, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey?, TValue>>();
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumeratorKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey, TValue?>>();
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

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumeratorStructKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey?, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
    {
        get => Value?.GetEnumerator();
        set
        {
            if (value == null)
            {
                Value = null;
                return;
            }
            var newValue       = new List<KeyValuePair<TKey?, TValue?>>();
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
