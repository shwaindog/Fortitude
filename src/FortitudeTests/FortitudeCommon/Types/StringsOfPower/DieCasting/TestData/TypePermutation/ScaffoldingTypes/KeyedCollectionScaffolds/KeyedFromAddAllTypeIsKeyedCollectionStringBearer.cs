#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    KeyedCollectionScaffolds;

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsDictionary  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryFormatStringsAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }
    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothFormatStrings , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsArray  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairArrayBothFormatStringsAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothFormatStrings);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }
    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsList  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairListBothFormatStringsAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListBothFormatStrings);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerable  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairEnumerableBothFormatStringsAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerator  | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromPairEnumeratorBothFormatStringsAddAllStringBearer<TKey, TValue> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsDictionary  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;


    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsArray  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairArrayValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsList  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairListValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerable  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairEnumerableValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerator  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromPairEnumeratorValueRevealerKeyFormatStringsAddAllStringBearer<TKey, TValue, TVRevealBase> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsDictionary  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllDictionaryBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsArray  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairArrayBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers);
    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllArrayBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsList  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairListBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers);
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAll
               (KeyedCollectionTypeKeyedCollectionFieldAddAllListBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerable  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairEnumerableBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers);
    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumerableBothRevealers, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(KeyedCollectionType | AcceptsKeyValueCollection | AcceptsEnumerator  | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromPairEnumeratorBothRevealersAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : 
  IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers);
    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddAllEnumerate
               (KeyedCollectionTypeKeyedCollectionFieldAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}
