// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType.FixtureScaffolding;

[ TypeGeneratePart( IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromAnyNullEnumerableKeyValueAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[ TypeGeneratePart( IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromAnyEnumerableKeyValueAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>
{
    public IEnumerable? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[ TypeGeneratePart( IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromNullEnumerableKeyValueAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate<StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[ TypeGeneratePart( IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyGeneric
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromEnumerableKeyValueAddAllStringBearer<TKey, TValue> : 
    FormattedKeyValueMoldScaffold<TKey, TValue>
{
    public List<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate<List<KeyValuePair<TKey, TValue>>, TKey, TValue>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumerableKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyEnumerableKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromNullEnumerableKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateValueRevealer<StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromEnumerableKeyValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    FormattedKeyValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateValueRevealer<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumerableKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateNullValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromAnyEnumerableKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TValue : struct
{
    public IEnumerable? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateNullValueRevealer
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromNullEnumerableKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateNullValueRevealer<StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class KeyedFromEnumerableKeyStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    FormattedKeyStructValueRevealerMoldScaffold<TKey, TValue>
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString?.GetType() 
     ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateNullValueRevealer<IEnumerable<KeyValuePair<TKey, TValue?>>, TKey, TValue>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
              | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumerableKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
              | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyEnumerableKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumerableKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue>>, KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
              | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumerableKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
    KeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase?
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothRevealers<IEnumerable<KeyValuePair<TKey, TValue>>, TKey, TValue, TKRevealBase, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumerableStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothWithNullKeyRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyEnumerableStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey?, TValue>>().ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothWithNullKeyRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumerableStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothWithNullKeyRevealers
               <StructEnumerable<List<KeyValuePair<TKey?, TValue>>, KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsAnyExceptNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumerableStructKeyRevealerValueRevealerAddAllStringBearer<TKey, TValue, TVRevealBase> : 
    StructKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TVRevealBase>
    where TKey : struct
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IEnumerable<KeyValuePair<TKey?, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothWithNullKeyRevealers<IEnumerable<KeyValuePair<TKey?, TValue>>, TKey, TValue, TVRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
               | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyNullEnumerableKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothWithNullValueRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
               | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromAnyEnumerableKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public IEnumerable? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.Cast<KeyValuePair<TKey, TValue?>>().ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothWithNullValueRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
                | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumerableKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothWithNullValueRevealers
               <StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
               | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumerableKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue, TKRevealBase> : 
    KeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKRevealBase>
    where TKey : TKRevealBase?
    where TValue : struct
    where TKRevealBase : notnull
{
    public List<KeyValuePair<TKey, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothWithNullValueRevealers<List<KeyValuePair<TKey, TValue?>>, TKey, TValue, TKRevealBase>
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromNullEnumerableStructKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public StructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>? 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value.ToNullableStructEnumerable<List<KeyValuePair<TKey?, TValue?>>, KeyValuePair<TKey?, TValue?>>();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() 
     ?? typeof(StructEnumerable<List<KeyValuePair<TKey, TValue?>>, KeyValuePair<TKey, TValue?>>?);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothNullRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | KeyValueCardinality | AcceptsEnumerable  | AcceptsNullableStruct
                | KeyNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromEnumerableStructKeyRevealerStructValueRevealerAddAllStringBearer<TKey, TValue> : 
    StructKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue>
    where TKey : struct
    where TValue : struct
{
    public IEnumerable<KeyValuePair<TKey?, TValue?>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers?.GetType() ?? typeof(IEnumerable<KeyValuePair<TKey?, TValue?>>);

    public override string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerateBothNullRevealers
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
