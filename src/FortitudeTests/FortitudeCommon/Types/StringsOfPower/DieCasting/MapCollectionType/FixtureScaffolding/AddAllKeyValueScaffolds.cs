#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll 
               (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothFormatStrings 
               , ValueFormatString
              , KeyFormatString, FormattingFlags)
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothFormatStrings?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothFormatStrings
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllListBothFormatStrings?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListBothFormatStrings
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags  )
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey?, TValue>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | AcceptsNullableStruct
              | KeyNullableStruct  | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey?, TValue?>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
