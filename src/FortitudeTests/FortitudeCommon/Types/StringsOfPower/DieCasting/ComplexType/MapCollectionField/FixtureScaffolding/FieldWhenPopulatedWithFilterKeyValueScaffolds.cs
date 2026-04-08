#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.
    ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBoth
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBoth?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueArrayWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBoth
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBoth?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueListWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBoth
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBoth?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase> :
    FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerArrayWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString?.GetType() 
     ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerArrayWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase> :
    FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString?.GetType() 
     ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerListWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerListWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase
                                                                                      , TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerArrayWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase
                                                                                 , TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers?.GetType() 
     ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerArrayWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey?, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers?.GetType() 
     ?? typeof(KeyValuePair<TKey?, TValue>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerArrayWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers?.GetType() 
     ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsArray | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerArrayWhenPopulatedWithFilterStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public KeyValuePair<TKey?, TValue?>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers?.GetType() 
     ?? typeof(KeyValuePair<TKey?, TValue?>[]);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerListWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase
                                                                                , TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerListWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerListWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsList | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerListWhenPopulatedWithFilterStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
