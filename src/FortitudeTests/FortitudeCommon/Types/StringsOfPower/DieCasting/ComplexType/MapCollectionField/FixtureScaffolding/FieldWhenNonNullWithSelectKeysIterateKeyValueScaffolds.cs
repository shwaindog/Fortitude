// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterate
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterate
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerator)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateValueRevealer
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateValueRevealer
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateValueRevealer
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase>
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateValueRevealer
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateNullValueRevealer
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateNullValueRevealer
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateNullValueRevealer
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateNullValueRevealer
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysAnyNullEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateBothRevealers
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysAnyEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateBothRevealers
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysNullEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateBothRevealers
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    where TKey : TKRevealBase
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateBothRevealers
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateBothWithNullValueRevealers
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateBothWithNullValueRevealers
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateBothWithNullValueRevealers
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonNullWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase>
    where TKey : TKRevealBase
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysIterateBothWithNullValueRevealers
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

