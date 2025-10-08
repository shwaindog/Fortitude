#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsAlwaysAddAllStringBearer<TKey, TValue> : IStringBearer
  , IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }
    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue> : IStringBearer
  , IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }
    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue> : IStringBearer
  , IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue> : IStringBearer
  , IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueFormatString, ISupportsKeyFormatString
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothFormatStrings
              , ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;


    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListValueRevealerKeyFormatStrings
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>
  , ISupportsKeyFormatString where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorValueRevealerKeyFormatString
              , ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllDictionaryBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllArrayBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllListBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumerableBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | AlwaysWrites | AcceptsClass | AcceptsNullableClass
                | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase> : IStringBearer
  , IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldAlwaysAddAllEnumeratorBothRevealers
              , ValueRevealer, KeyRevealer)
           .Complete();
}
