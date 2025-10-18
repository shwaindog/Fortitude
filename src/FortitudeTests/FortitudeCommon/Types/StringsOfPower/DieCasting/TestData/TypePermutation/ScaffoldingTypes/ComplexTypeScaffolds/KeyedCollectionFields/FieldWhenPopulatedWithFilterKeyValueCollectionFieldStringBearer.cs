#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;


    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothFormatStrings);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothFormatStrings);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothFormatStrings);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothFormatStrings);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString);
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
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    IStringBearer, IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString);
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
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString);
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
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString);
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
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString);
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
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers);
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
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterDictionaryBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers);
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
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers);
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
           .KeyedCollectionField.WhenPopulatedWithFilter
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers);
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
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonEmptyWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers);
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
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedWithFilterEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}
