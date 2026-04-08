// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;


[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate 
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate 
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate 
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase> 
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
    FilteredFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings);
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase> 
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : FilteredFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    where TKey : notnull, TKFilterBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateValueRevealer
               <IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateNullValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase>
    : FilteredFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase>
    where TKey : notnull, TKFilterBase
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateNullValueRevealer
               <IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>
                 , TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    : FilteredKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : TVFilterBase?, TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothRevealers
               <IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
              | KeyNullableStruct  | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
              | KeyNullableStruct  | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey?, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct  | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothWithNullKeyRevealers
               <StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsAnyExceptNullableStruct
              | KeyNullableStruct  | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TVFilterBase, TVRevealBase>
    : FilteredStructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVFilterBase, TVRevealBase>
    where TKey : struct
    where TValue : TVFilterBase?, TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothWithNullKeyRevealers
               <IEnumerable<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVFilterBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothWithNullValueRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerEnumerableAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase>
    : FilteredKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKFilterBase, TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothWithNullValueRevealers
               <IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKFilterBase, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerNullEnumerableAlwaysAddFilteredStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerEnumerableAlwaysAddFilteredStringBearer<TKey, TValue>
    : FilteredStructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}