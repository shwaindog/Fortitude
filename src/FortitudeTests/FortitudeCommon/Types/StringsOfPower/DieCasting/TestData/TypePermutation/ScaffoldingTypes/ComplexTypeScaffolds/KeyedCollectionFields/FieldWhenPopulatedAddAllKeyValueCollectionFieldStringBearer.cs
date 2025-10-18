#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }
    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }
    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothFormatStrings);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothFormatStrings);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }


    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableValueRevealerKeyFormatString);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers);
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllDictionaryBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers);
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllArrayBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers);
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllListBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers);
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumerableBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonEmptyWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers);
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

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenPopulatedAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}
