// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using System.Collections;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestExpectations.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.ComplexType.MapCollectionField.FixtureScaffolding;

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterate
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyGeneric | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyValueFieldMoldScaffold<TKey, TValue, TKSelectDerived>
    where TKey : notnull
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerator 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumerator?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterate
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerator)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerator
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueFormatString
              , KeyFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateValueRevealer
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateValueRevealer
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateValueRevealer
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : SelectFormattedKeyValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TVRevealBase> 
    where TKey : notnull
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateValueRevealer
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TVRevealBase>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateNullValueRevealer
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateNullValueRevealer
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateNullValueRevealer
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyFormatString | SupportsValueFormatString)]
public class FieldKeyStructValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : SelectFormattedKeyStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived> 
    where TKey : notnull
    where TValue : struct
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateNullValueRevealer
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyFormatString
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysAnyNullEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateBothRevealers
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysAnyEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateBothRevealers
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator?)DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysNullEnumeratorStringBearer
    <TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateBothRevealers
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsAnyExceptNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : SelectKeyRevealerValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase> 
    where TKey : TKRevealBase 
    where TValue : TVRevealBase?
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
    where TVRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateBothRevealers
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TKRevealBase, TVRevealBase>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysAnyNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysAnyEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , (IEnumerator)DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysNullEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers
               <TKey, TValue, StructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator().ToNullableStructEnumerator<IEnumerator<TKSelectDerived>, TKSelectDerived>()
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

[TypeGeneratePart(IsComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsNullableStruct | SupportsValueRevealer | SupportsKeyRevealer | SupportsValueFormatString)]
public class FieldKeyRevealerStructValueRevealerDictionaryWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase>
    : SelectKeyRevealerStructValueRevealerFieldMoldScaffold<TKey, TValue, TKSelectDerived, TKRevealBase> 
    where TKey : TKRevealBase 
    where TValue : struct
    where TKSelectDerived : TKey
    where TKRevealBase : notnull
{
    public IReadOnlyDictionary<TKey, TValue?>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value?.ToDictionary();
        set => Value = value?.ToList();
    }

    public override Type KeyedCollectionType => 
        WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers?.GetType() ?? typeof(IReadOnlyDictionary<TKey, TValue?>);

    public override string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );

    public override AppendSummary RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysIterateBothWithNullValueRevealers
               <TKey, TValue, IEnumerator<TKSelectDerived>, TKSelectDerived, TKRevealBase>
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
                // ReSharper disable once GenericEnumeratorNotDisposed
              , DisplayKeys?.GetEnumerator()!
              , ValueRevealer
              , KeyRevealer
              , ValueFormatString, FormattingFlags)
           .Complete();
}

