#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysAddAllStringBearer<TKey, TValue> : FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionary
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionary?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionary);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionary)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionary
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueArrayAlwaysAddAllStringBearer<TKey, TValue> : FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArray
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllArray?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArray)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArray?.ToArray()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueListAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllList
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllList?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllList)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllList
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              ,ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryStructValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryStructValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryStructValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryStructValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryStructValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
               ,ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerArrayAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerArrayAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayStructValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayStructValueRevealerKeyFormatString?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayStructValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayStructValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayStructValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerListAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerListAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}


[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerArrayAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerArrayAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey?, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey?, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerArrayAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealersStruct
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealersStruct?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | AcceptsNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerArrayAlwaysAddAllStringBearer<TKey, TValue> :
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct 
    where TValue : struct
{
    public KeyValuePair<TKey?, TValue?>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothStructRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothStructRevealers?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothStructRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothStructRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothStructRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerListAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerListAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStructKey
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStructKey?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStructKey);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStructKey)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStructKey
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerListAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase? 
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | AcceptsNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerListAlwaysAddAllStringBearer<TKey, TValue> :
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct 
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}