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

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromKeyValueDictionaryAddWithSelectKeysAnyNullEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromEnumerableBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysFromEnumerableBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysFromEnumerableBothFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate
               (AddWithSelectKeysFromEnumerableBothFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromKeyValueDictionaryAddWithSelectKeysAnyEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromEnumerableBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysFromEnumerableBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysFromEnumerableBothFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate
               (AddWithSelectKeysFromEnumerableBothFormatStrings
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromKeyValueDictionaryAddWithSelectKeysNullEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromEnumerableBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysFromEnumerableBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysFromEnumerableBothFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (AddWithSelectKeysFromEnumerableBothFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromKeyValueDictionaryAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromEnumerableBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysFromEnumerableBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysFromEnumerableBothFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate<TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived>
               (AddWithSelectKeysFromEnumerableBothFormatStrings
                // ReSharper disable once RedundantCast
              , (IEnumerable<TKSelectDerived>?)DisplayKeys
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyValueRevealerDictionaryAddWithSelectKeysAnyNullEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateValueRevealer
               (AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyValueRevealerDictionaryAddWithSelectKeysAnyEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateValueRevealer
               (AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyValueRevealerDictionaryAddWithSelectKeysNullEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateValueRevealer
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyValueRevealerDictionaryAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateValueRevealer<TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , (IEnumerable<TKSelectDerived>?)DisplayKeys
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyStructValueRevealerDictionaryAddWithSelectKeysAnyNullEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateNullValueRevealer
               (AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyStructValueRevealerDictionaryAddWithSelectKeysAnyEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateNullValueRevealer
               (AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyStructValueRevealerDictionaryAddWithSelectKeysNullEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateNullValueRevealer
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyStructValueRevealerDictionaryAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateNullValueRevealer<TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived>
               (AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
                // ReSharper disable once RedundantCast
              , ((IEnumerable<TKSelectDerived>?)(DisplayKeys))!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer
                | SupportsValueFormatString)]
public class KeyedFromKeyRevealerValueRevealerDictionaryAddWithSelectKeysAnyNullEnumerableStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateBothRevealers
               (AddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer
                | SupportsValueFormatString)]
public class KeyedFromKeyRevealerValueRevealerDictionaryAddWithSelectKeysAnyEnumerableStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateBothRevealers
               (AddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer
                | SupportsValueFormatString)]
public class KeyedFromKeyRevealerValueRevealerDictionaryAddWithSelectKeysNullEnumerableStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateBothRevealers
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (AddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer
                | SupportsValueFormatString)]
public class KeyedFromKeyRevealerValueRevealerDictionaryAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateBothRevealers
               <TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (AddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , (IEnumerable<TKSelectDerived>?)DisplayKeys
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromKeyRevealerStructValueRevealerDictionaryAddWithSelectKeysAnyNullEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateBothWithNullValueRevealers
               (AddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromKeyRevealerStructValueRevealerDictionaryAddWithSelectKeysAnyEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateBothWithNullValueRevealers
               (AddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , (IEnumerable?)DisplayKeys
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromKeyRevealerStructValueRevealerDictionaryAddWithSelectKeysNullEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateBothWithNullValueRevealers
               <TKey, TValue, StructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (AddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , DisplayKeys.ToNullableStructEnumerable<IReadOnlyList<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromKeyRevealerStructValueRevealerDictionaryAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumerableBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerateBothWithNullValueRevealers
               <TKey, TValue, IEnumerable<TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (AddWithSelectKeysKeyValuePairEnumerableBothRevealers
                // ReSharper disable once RedundantCast
              , (IEnumerable<TKSelectDerived>?)DisplayKeys
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}