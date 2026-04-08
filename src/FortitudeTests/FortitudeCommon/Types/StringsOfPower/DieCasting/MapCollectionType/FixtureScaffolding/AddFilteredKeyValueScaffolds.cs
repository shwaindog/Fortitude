// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.


namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType.FixtureScaffolding;

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryKeyValueAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothFormatStrings
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothFormatStrings);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromArrayKeyValueAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase
    where TValue : TVFilterBase?
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothFormatStrings
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothFormatStrings?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromListKeyValueAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase
    where TValue : TVFilterBase?
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothFormatStrings?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromDictionaryKeyValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromDictionaryKeyStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase> :
    FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : TKFilterBase
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromArrayKeyValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString?.GetType() 
     ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromArrayKeyStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase> :
    FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : TKFilterBase
    where TValue : struct
{
    public KeyValuePair<TKey, TValue?>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString?.GetType() 
     ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromListKeyValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromListKeyStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromDictionaryKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromDictionaryKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers?.GetType() 
     ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromArrayKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey, TValue>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromArrayStructKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public KeyValuePair<TKey?, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey?, TValue>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromArrayKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public KeyValuePair<TKey, TValue?>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey, TValue?>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromArrayStructKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public KeyValuePair<TKey?, TValue?>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
    {
        get => Value?.ToArray();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers?.GetType() ?? typeof(KeyValuePair<TKey?, TValue?>[]);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromListKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromListStructKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromListKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyList<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromListStructKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IReadOnlyList<KeyValuePair<TKey?, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers?.GetType() ?? typeof(IReadOnlyList<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}