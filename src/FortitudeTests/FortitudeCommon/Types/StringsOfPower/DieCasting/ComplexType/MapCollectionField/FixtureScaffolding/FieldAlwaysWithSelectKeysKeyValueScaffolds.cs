using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArray
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromArray?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArray);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArray)
              , AlwaysWithSelectKeysFromArray
              , DisplayKeys?.ToArray()!
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpan
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromSpan?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpan)
              , AlwaysWithSelectKeysFromSpan
              , (DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty)
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpan
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromReadOnlySpan?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpan);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpan)
              , AlwaysWithSelectKeysFromReadOnlySpan
              , (DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty)
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromList
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromList?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromList);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromList)
              , AlwaysWithSelectKeysFromList
              , DisplayKeys!
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys?.ToArray()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys?.ToArray()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey 
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromListValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromListValueRevealerKeyFormatString?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>  
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey 
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromArrayBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArrayBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayBothRevealers)
              , AlwaysWithSelectKeysFromArrayBothRevealers
              , DisplayKeys?.ToArray()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>  
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey 
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromArrayBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromArrayBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromArrayBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayBothRevealers)
              , AlwaysWithSelectKeysFromArrayBothRevealers
              , DisplayKeys?.ToArray()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromSpanBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpanBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanBothRevealers)
              , AlwaysWithSelectKeysFromSpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromSpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromSpanBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromSpanBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanBothRevealers)
              , AlwaysWithSelectKeysFromSpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : Span<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer 
                | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromReadOnlySpanBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers)
              , AlwaysWithSelectKeysFromReadOnlySpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer 
                | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromReadOnlySpanBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers)
              , AlwaysWithSelectKeysFromReadOnlySpanBothRevealers
              , DisplayKeys != null ? DisplayKeys.ToArray().AsSpan() : ReadOnlySpan<TKSelectDerived>.Empty
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromListBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromListBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListBothRevealers)
              , AlwaysWithSelectKeysFromListBothRevealers
              , DisplayKeys!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysFromListBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromListBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromListBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListBothRevealers)
              , AlwaysWithSelectKeysFromListBothRevealers
              , DisplayKeys!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
