// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> :
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               <IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBoth
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateValueRevealer
               <IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateNullValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateNullValueRevealer
               <IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyNullEnumerableWhenPopulatedWithFilterStringBearer
    <TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyEnumerableWhenPopulatedWithFilterStringBearer
    <TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerNullEnumerableWhenPopulatedWithFilterStringBearer
    <TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerEnumerableWhenPopulatedWithFilterStringBearer
    <TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothRevealers
               <IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey?, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers
               <StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothWithNullKeyRevealers
               <IEnumerable<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothWithNullValueRevealers
               <IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerNullEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonDefaultWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerEnumerableWhenPopulatedWithFilterStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}