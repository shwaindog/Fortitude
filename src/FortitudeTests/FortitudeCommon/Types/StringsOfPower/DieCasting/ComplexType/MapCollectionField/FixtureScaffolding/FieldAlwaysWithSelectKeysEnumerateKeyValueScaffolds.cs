// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;


[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysAnyNullEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromEnumerable 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromEnumerable?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromEnumerable );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysFromEnumerable)
              , AlwaysWithSelectKeysFromEnumerable
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysAnyEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromEnumerable 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromEnumerable?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromEnumerable );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysFromEnumerable)
              , AlwaysWithSelectKeysFromEnumerable
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysNullEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromEnumerable 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromEnumerable?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromEnumerable );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (nameof(AlwaysWithSelectKeysFromEnumerable)
              , AlwaysWithSelectKeysFromEnumerable
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromEnumerable 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysFromEnumerable?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysFromEnumerable );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate<TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived>
               (nameof(AlwaysWithSelectKeysFromEnumerable)
              , AlwaysWithSelectKeysFromEnumerable
                // ReSharper disable once RedundantCast
              , (IEnumerable<TKSelectDerived>?)DisplayKeys
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysAnyNullEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey 
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateValueRevealer
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysAnyEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey 
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateValueRevealer
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysNullEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey 
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateValueRevealer
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey 
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateValueRevealer<TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysAnyNullEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateNullValueRevealer
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysAnyEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateNullValueRevealer
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysNullEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateNullValueRevealer
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)DisplayKeys)!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysAnyNullEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateBothRevealers
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysAnyEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateBothRevealers
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysNullEnumerableStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateBothRevealers
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateBothRevealers
               <TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , DisplayKeys
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysAnyNullEnumerableStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateBothWithNullValueRevealers
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysAnyEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateBothWithNullValueRevealers
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysNullEnumerableStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateBothWithNullValueRevealers
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers );
    
    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerateBothWithNullValueRevealers
               <TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , DisplayKeys
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
