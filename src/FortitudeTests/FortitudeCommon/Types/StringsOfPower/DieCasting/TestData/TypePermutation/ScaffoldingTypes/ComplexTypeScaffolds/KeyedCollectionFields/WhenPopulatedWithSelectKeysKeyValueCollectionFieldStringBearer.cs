using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsArray | OnlyPopulatedWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayBothFormatStrings)
              , WhenPopulatedWithSelectKeysFromArrayBothFormatStrings
              , DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsSpan | OnlyPopulatedWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanBothFormatStrings)
              , WhenPopulatedWithSelectKeysFromSpanBothFormatStrings
              , DisplayKeys.ToArray().AsSpan(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothFormatStrings)
              , WhenPopulatedWithSelectKeysFromReadOnlySpanBothFormatStrings
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListBothFormatStrings)
              , WhenPopulatedWithSelectKeysFromListBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerable | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromEnumerableBothFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysFromEnumerableBothFormatStrings)
              , WhenPopulatedWithSelectKeysFromEnumerableBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerator | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
              , DisplayKeys.GetEnumerator(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsArray | OnlyPopulatedWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsSpan | OnlyPopulatedWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerable | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerator | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsArray | OnlyPopulatedWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }
    

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayBothRevealers)
              , WhenPopulatedWithSelectKeysFromArrayBothRevealers
              , DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsSpan | OnlyPopulatedWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }
    

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanBothRevealers)
              , WhenPopulatedWithSelectKeysFromSpanBothRevealers
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | OnlyPopulatedWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }
    

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers)
              , WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }
    

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListBothRevealers)
              , WhenPopulatedWithSelectKeysFromListBothRevealers
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerable | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }
    

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerator | OnlyPopulatedWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }
    

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers), WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyRevealer)
           .Complete();
}
