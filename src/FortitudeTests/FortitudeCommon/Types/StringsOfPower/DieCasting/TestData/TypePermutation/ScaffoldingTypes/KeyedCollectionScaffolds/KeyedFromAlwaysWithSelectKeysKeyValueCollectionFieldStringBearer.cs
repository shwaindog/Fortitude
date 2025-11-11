using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.
    KeyedCollectionScaffolds;

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryBothFormatStringsAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromArrayBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromArrayBothFormatStrings
              , DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryBothFormatStringsAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromSpanBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromSpanBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromSpanBothFormatStrings
              , DisplayKeys.ToArray().AsSpan(), ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryBothFormatStringsAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromReadOnlySpanBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromReadOnlySpanBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromReadOnlySpanBothFormatStrings
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryBothFormatStringsAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromListBothFormatStrings);
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromListBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryBothFormatStringsAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromEnumerableBothFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromEnumerableBothFormatStrings );
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate
               (AddWithSelectKeysFromEnumerableBothFormatStrings
              , DisplayKeys, ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class KeyedFromDictionaryBothFormatStringsAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings );
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate
               (AddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
              , DisplayKeys.GetEnumerator(), ValueFormatString, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryValueRevealerAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromArrayValueRevealerKeyFormatString );
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryValueRevealerAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromSpanValueRevealerKeyFormatString );
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryValueRevealerAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryValueRevealerAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromListValueRevealerKeyFormatString );
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryValueRevealerAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate
               (AddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class KeyedFromDictionaryValueRevealerAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );
    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public Delegate ValueRevealerDelegate
    {
        get => ValueRevealer;
        set => ValueRevealer = (PalantírReveal<TVRevealBase>)value;
    }

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate
               (AddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryBothRevealersAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromArrayBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromArrayBothRevealers );
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

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromArrayBothRevealers
              , DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | AlwaysWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryBothRevealersAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromSpanBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromSpanBothRevealers );
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

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromSpanBothRevealers
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | AlwaysWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryBothRevealersAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromReadOnlySpanBothRevealers );
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

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromReadOnlySpanBothRevealers
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryBothRevealersAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysFromListBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysFromListBothRevealers );
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

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeys
               (AddWithSelectKeysFromListBothRevealers
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryBothRevealersAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumerableBothRevealers );
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

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate
               (AddWithSelectKeysKeyValuePairEnumerableBothRevealers
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(KeyedCollectionType | SimpleType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | AlwaysWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class KeyedFromDictionaryBothRevealersAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? AddWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(AddWithSelectKeysKeyValuePairEnumeratorBothRevealers );
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

    public IReadOnlyList<TKSelectDerived> DisplayKeys
    {
        get => displayKeys ??= Value?.Keys.Select(key => (TKSelectDerived)(object?)key!).ToList() ?? [];
        set => displayKeys = value;
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartKeyedCollectionType(this)
           .AddWithSelectKeysEnumerate
               (AddWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
