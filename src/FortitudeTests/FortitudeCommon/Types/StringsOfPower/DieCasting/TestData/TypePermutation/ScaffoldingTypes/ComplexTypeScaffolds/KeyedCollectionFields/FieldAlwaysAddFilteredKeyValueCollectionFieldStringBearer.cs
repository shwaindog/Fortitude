#region

using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;


    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothFormatStrings);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothFormatStrings);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothFormatStrings);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryValueRevealerKeyFormatString);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayValueRevealerKeyFormatString);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListValueRevealerKeyFormatString);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorValueRevealerKeyFormatString);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothRevealers);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public Delegate KeyRevealerDelegate
    {
        get => KeyRevealer;
        set => KeyRevealer = (PalantírReveal<TKRevealBase>)value;
    }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredDictionaryBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsArray | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothRevealers);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public Delegate KeyRevealerDelegate
    {
        get => KeyRevealer;
        set => KeyRevealer = (PalantírReveal<TKRevealBase>)value;
    }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsList | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothRevealers);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public Delegate KeyRevealerDelegate
    {
        get => KeyRevealer;
        set => KeyRevealer = (PalantírReveal<TKRevealBase>)value;
    }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerable | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public Delegate KeyRevealerDelegate
    {
        get => KeyRevealer;
        set => KeyRevealer = (PalantírReveal<TKRevealBase>)value;
    }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsEnumerator | AlwaysWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothRevealers);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public Delegate KeyRevealerDelegate
    {
        get => KeyRevealer;
        set => KeyRevealer = (PalantírReveal<TKRevealBase>)value;
    }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
