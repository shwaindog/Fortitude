#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    KeyedCollectionScaffolds;

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsDictionary  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;


    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsArray  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairArrayBothFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothFormatStrings);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsList  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairListBothFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothFormatStrings);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerable  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairEnumerableBothFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothFormatStrings);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerator  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairEnumeratorBothFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsDictionary  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryValueRevealerKeyFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString);
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
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsArray  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairArrayValueRevealerKeyFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString);
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
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsList  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairListValueRevealerKeyFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString);
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
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerable  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairEnumerableValueRevealerKeyFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableValueRevealerKeyFormatString);
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
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerator  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairEnumeratorValueRevealerKeyFormatStringsAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString);
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
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsDictionary  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryBothRevealersAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers);
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
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredDictionaryBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsArray  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairArrayBothRevealersAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers);
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
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsList  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairListBothRevealersAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers);
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
        tos.StartKeyedCollectionType(this)
           .AddFiltered
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerable  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairEnumerableBothRevealersAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers);
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
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerator  | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairEnumeratorBothRevealersAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers);
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
        tos.StartKeyedCollectionType(this)
           .AddFilteredEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}
