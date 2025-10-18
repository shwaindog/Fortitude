#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }
    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothFormatStrings);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }
    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothFormatStrings);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothFormatStrings);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothFormatStrings);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryValueRevealerKeyFormatString);
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
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayValueRevealerKeyFormatString);
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
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListValueRevealerKeyFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListValueRevealerKeyFormatStrings);
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
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString);
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
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString);
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
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothRevealers);
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
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllDictionaryBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothRevealers);
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
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllArrayBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothRevealers);
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
           .KeyedCollectionField.WhenNonNullAddAll
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllListBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers);
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
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumerableBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonNullWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers);
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
           .KeyedCollectionField.WhenNonNullAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}
