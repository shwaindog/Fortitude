#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.
    FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsAnyGeneric
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsAnyGeneric
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueListWhenPopulatedAddAllStringBearer<TKey, TValue> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumerableWhenPopulatedAddAllStringBearer<TKey, TValue> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumeratorWhenPopulatedAddAllStringBearer<TKey, TValue> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerArrayWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerArrayWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerListWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerListWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerEnumerableWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerEnumerableWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableValueRevealerKeyFormatString
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueRevealerEnumeratorWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyStructValueRevealerEnumeratorWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerArrayWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldStructKeyRevealerValueRevealerArrayWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey?, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerArrayWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldStructKeyRevealerStructValueRevealerArrayWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public KeyValuePair<TKey?, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerListWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldStructKeyRevealerValueRevealerListWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerListWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsNullableStruct
                | KeyNullableStruct | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldStructKeyRevealerStructValueRevealerListWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerEnumerableWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldStructKeyRevealerValueRevealerEnumerableWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerEnumerableWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldStructKeyRevealerStructValueRevealerEnumerableWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerValueRevealerEnumeratorWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
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

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldStructKeyRevealerValueRevealerEnumeratorWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyRevealerStructValueRevealerEnumeratorWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerator | NonDefaultWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldStructKeyRevealerStructValueRevealerEnumeratorWhenPopulatedAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
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

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}
