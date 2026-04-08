// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyNullEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}
[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueNullEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerable
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyNullEnumerableAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyEnumerableAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerNullEnumerableAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerEnumerableAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateValueRevealer<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyNullEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerNullEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateNullValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateNullValueRevealer<IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyNullEnumerableAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyEnumerableAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerNullEnumerableAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerEnumerableAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> :
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase? 
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothRevealers<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyNullEnumerableAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyEnumerableAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey?, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerNullEnumerableAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothWithNullKeyRevealers
               <StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsAnyExceptNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerEnumerableAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> :
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct 
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothWithNullKeyRevealers
               <IEnumerable<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyNullEnumerableAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase? 
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyEnumerableAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase? 
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerNullEnumerableAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase? 
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothWithNullValueRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerEnumerableAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase> :
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase? 
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothWithNullValueRevealers<IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealersStruct
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerNullEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct 
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | AcceptsNullableStruct | KeyNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerEnumerableAlwaysAddAllStringBearer<TKey, TValue> :
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct 
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothStructRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
