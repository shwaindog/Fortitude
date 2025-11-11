using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ScaffoldingStringBuilderInvokeFlags;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonDefaultWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;

    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromArrayBothFormatStrings);
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonDefaultWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromSpanBothFormatStrings);
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothFormatStrings);
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromListBothFormatStrings);
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromEnumerableBothFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromEnumerableBothFormatStrings );
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueFormatString, ISupportsKeyFormatString where TKSelectDerived : TKey
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothFormatStrings );
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

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonDefaultWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString
              , DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonDefaultWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString)
              , WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings
              , DisplayKeys, ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString 
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyFormatString)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsArray | NonDefaultWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromArrayBothRevealers );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayBothRevealers)
              , WhenPopulatedWithSelectKeysFromArrayBothRevealers
              , DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsSpan | NonDefaultWrites | SubsetListFilter
                | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromSpanBothRevealers );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanBothRevealers)
              , WhenPopulatedWithSelectKeysFromSpanBothRevealers
              , DisplayKeys.ToArray().AsSpan(), ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | CallsAsReadOnlySpan | NonDefaultWrites 
                | SubsetListFilter | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers)
              , WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers
              , (ReadOnlySpan<TKSelectDerived>)DisplayKeys.ToArray(), ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysFromListBothRevealers );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListBothRevealers)
              , WhenPopulatedWithSelectKeysFromListBothRevealers
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerable | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers
              , DisplayKeys, ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}

[TypeGeneratePart(ComplexType | KeyValueCardinality | AcceptsDictionary | AcceptsList | AcceptsEnumerator | NonDefaultWrites
                | SubsetListFilter | AcceptsClass | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
    : IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsSubsetDisplayKeys<TKSelectDerived>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase> 
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    private IReadOnlyList<TKSelectDerived>? displayKeys;
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers 
    {
        get => Value;
        set => Value = value;
    }

    public string PropertyName => nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers );
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
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers), WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , DisplayKeys.GetEnumerator(), ValueRevealer, KeyRevealer)
           .Complete();

    public override string ToString() => $"{GetType().ShortNameInCSharpFormat()}({Value})";
}
