// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;


[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerator)
              , AlwaysWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerator)
              , AlwaysWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}
[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterate
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerator)
              , AlwaysWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterate<TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerator)
              , AlwaysWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateValueRevealer
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateValueRevealer
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateValueRevealer
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateValueRevealer<TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateNullValueRevealer
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateNullValueRevealer
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateNullValueRevealer
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateNullValueRevealer<TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateBothRevealers
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateBothRevealers
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateBothRevealers
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase 
    where TValue : TVRevealBase 
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateBothRevealers
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateBothWithNullValueRevealers
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers), AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateBothWithNullValueRevealers
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers), AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateBothWithNullValueRevealers
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers), AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey  
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysIterateBothWithNullValueRevealers
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers), AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}
