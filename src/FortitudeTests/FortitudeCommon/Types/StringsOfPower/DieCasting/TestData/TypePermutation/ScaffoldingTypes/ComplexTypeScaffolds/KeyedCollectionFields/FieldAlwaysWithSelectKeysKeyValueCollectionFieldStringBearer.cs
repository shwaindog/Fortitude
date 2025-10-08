using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayBothFormatStrings
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayBothFormatStrings)
              , AlwaysWithSelectKeysFromArrayBothFormatStrings
              , DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanBothFormatStrings
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanBothFormatStrings)
              , AlwaysWithSelectKeysFromSpanBothFormatStrings
              , DisplayKeys.ToArray().AsSpan(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings)
              , AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListBothFormatStrings
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListBothFormatStrings)
              , AlwaysWithSelectKeysFromListBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromEnumerableBothFormatStrings 
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
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysFromEnumerableBothFormatStrings)
              , AlwaysWithSelectKeysFromEnumerableBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
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
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
              , DisplayKeys.GetEnumerator(), ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString 
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString 
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListValueRevealerKeyFormatString 
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString)
              , AlwaysWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
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
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
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
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayBothRevealers 
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayBothRevealers)
              , AlwaysWithSelectKeysFromArrayBothRevealers
              , DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanBothRevealers 
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanBothRevealers)
              , AlwaysWithSelectKeysFromSpanBothRevealers
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanBothRevealers 
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers)
              , AlwaysWithSelectKeysFromReadOnlySpanBothRevealers
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListBothRevealers 
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
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListBothRevealers)
              , AlwaysWithSelectKeysFromListBothRevealers
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers 
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
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers 
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
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers), AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyRevealer)
           .Complete();
}
