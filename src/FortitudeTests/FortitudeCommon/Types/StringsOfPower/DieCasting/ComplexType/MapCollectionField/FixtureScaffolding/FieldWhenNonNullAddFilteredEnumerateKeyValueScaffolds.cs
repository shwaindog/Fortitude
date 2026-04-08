// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyGeneric 
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               <IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase 
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateValueRevealer
               <IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : struct
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateNullValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase 
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateNullValueRevealer
               <IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothRevealers
               <IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey?, TValue>>().ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothWithNullKeyRevealers
               <StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothWithNullKeyRevealers
               <IEnumerable<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothWithNullValueRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothWithNullValueRevealers
               <IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerNullEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerEnumerableWhenNonNullAddFilteredStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

