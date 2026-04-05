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


[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromKeyValueDictionaryAddWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterate
               (AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromKeyValueDictionaryAddWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterate
               (AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)(DisplayKeys?.GetEnumerator())
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromKeyValueDictionaryAddWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterate
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromKeyValueDictionaryAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterate
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived>
               (AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyValueRevealerDictionaryAddWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateValueRevealer
               (AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyValueRevealerDictionaryAddWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateValueRevealer
               (AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyValueRevealerDictionaryAddWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateValueRevealer
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyValueRevealerDictionaryAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateValueRevealer
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyStructValueRevealerDictionaryAddWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateNullValueRevealer
               (AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyStructValueRevealerDictionaryAddWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateNullValueRevealer
               (AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyStructValueRevealerDictionaryAddWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateNullValueRevealer
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString
                | SupportsValueFormatString)]
public class KeyedFromKeyStructValueRevealerDictionaryAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
    where TValue : struct
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateNullValueRevealer
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived>
               (AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer
                | SupportsValueFormatString)]
public class KeyedFromKeyRevealerValueRevealerDictionaryAddWithSelectKeysAnyNullEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateBothRevealers
               (AddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer
                | SupportsValueFormatString)]
public class KeyedFromKeyRevealerValueRevealerDictionaryAddWithSelectKeysAnyEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateBothRevealers
               (AddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer
                | SupportsValueFormatString)]
public class KeyedFromKeyRevealerValueRevealerDictionaryAddWithSelectKeysNullEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateBothRevealers
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (AddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer
                | SupportsValueFormatString)]
public class KeyedFromKeyRevealerValueRevealerDictionaryAddWithSelectKeysEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : TVRevealBase?
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateBothRevealers
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (AddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromKeyRevealerStructValueRevealerDictionaryAddWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateBothWithNullValueRevealers
               (AddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromKeyRevealerStructValueRevealerDictionaryAddWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateBothWithNullValueRevealers
               (AddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromKeyRevealerStructValueRevealerDictionaryAddWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateBothWithNullValueRevealers
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (AddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsKeyedCollectionType | IsContentType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator
                | AlwaysWrites | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class KeyedFromKeyRevealerStructValueRevealerDictionaryAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TKSelectDerived : TKey
    where TValue : struct
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType =>
        AddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysIterateBothWithNullValueRevealers
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (AddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}