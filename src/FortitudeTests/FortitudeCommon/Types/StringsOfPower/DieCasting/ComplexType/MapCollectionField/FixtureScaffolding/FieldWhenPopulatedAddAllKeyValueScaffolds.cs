#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString?.GetType()
     ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString?.GetType()
     ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString?.GetType()
     ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString?.GetType()
     ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings?.GetType()
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings?.GetType()
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers?.GetType()
     ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers?.GetType()
     ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers?.GetType()
     ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers?.GetType()
     ?? typeof(KeyValuePair<TKey?, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers?.GetType()
     ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers?.GetType()
     ?? typeof(KeyValuePair<TKey?, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers?.GetType()
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers?.GetType()
     ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers?.GetType()
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | AcceptsNullableStruct
                | KeyNullableStruct | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
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

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers?.GetType()
     ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
