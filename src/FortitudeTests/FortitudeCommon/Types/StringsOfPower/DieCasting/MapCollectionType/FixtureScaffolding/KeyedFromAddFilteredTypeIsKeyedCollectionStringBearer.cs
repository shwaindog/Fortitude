#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;
#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

#endregion

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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothFormatStrings);
    
    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromEnumerableKeyValueAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase
    where TValue : TVFilterBase?
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromEnumeratorKeyValueAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : TKFilterBase
    where TValue : TVFilterBase?
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromEnumerableKeyValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromEnumerableKeyStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromEnumeratorKeyValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromEnumeratorKeyStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsDictionary  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsArray  | FilterPredicate | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsList  | FilterPredicate | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumerableKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumerableStructKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey?, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumerableKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumerableStructKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey?, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumeratorKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumeratorStructKeyRevealerValueRevealerAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey?, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumeratorKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerator<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerator  | FilterPredicate | AcceptsNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromEnumeratorStructKeyRevealerStructValueRevealerAddFilteredStringBearer<TKey, TValue>
    : FilteredKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerator<KeyValuePair<TKey?, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
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

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}