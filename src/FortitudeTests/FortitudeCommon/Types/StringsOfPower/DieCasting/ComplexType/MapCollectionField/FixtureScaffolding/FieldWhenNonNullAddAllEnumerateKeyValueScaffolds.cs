// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueAnyEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBoth
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerAnyEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateValueRevealer<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerAnyEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateNullValueRevealer
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateNullValueRevealer
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : notnull
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateNullValueRevealer<IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerAnyEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothRevealers
               <IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerAnyEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey?, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothWithNullKeyRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothWithNullKeyRevealers
               <StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsAnyExceptNullableStruct
               | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerValueRevealerEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey?, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothWithNullKeyRevealers
               <IEnumerable<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerAnyEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothWithNullValueRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothWithNullValueRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerEnumerableWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothWithNullValueRevealers
               <IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerNullEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsEnumerable | NonNullWrites | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldStructKeyRevealerStructValueRevealerEnumerableWhenNonNullAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey?, TValue?>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerateBothNullRevealers
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
