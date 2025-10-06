#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using static FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.TypeGeneratePartFlags;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.
    KeyedCollectionFields;

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : IStringBearer
  , IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;


    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : IStringBearer
  , IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairListBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : IStringBearer
  , IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : IStringBearer
  , IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueFormatString | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase> : IStringBearer
  , IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueFormatString, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothFormatStrings
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public string? ValueFormatString { get; set; }

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothFormatStrings)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothFormatStrings
              , KeyValuePredicate, ValueFormatString, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValueDictionaryValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairArrayValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase> :
    IStringBearer, IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
  , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairListValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumerableValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyFormatString)]
public class FieldKeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyFormatString where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorValueRevealerKeyFormatString
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public string? KeyFormatString { get; set; }

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorValueRevealerKeyFormatString)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorValueRevealerKeyFormatString
              , KeyValuePredicate, ValueRevealer, KeyFormatString)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsDictionary | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValueDictionaryBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyDictionary<TKey, TValue>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyDictionary<TKey, TValue>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredDictionaryBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsArray | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairArrayBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<KeyValuePair<TKey, TValue>[]?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public KeyValuePair<TKey, TValue>[]? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredArrayBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsList | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairListBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IReadOnlyList<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public IReadOnlyList<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredListBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerable | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumerableBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IEnumerable<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerable<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumerableBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}

[TypeGeneratePart(ComplexType | AcceptsKeyValueCollection | AcceptsEnumerator | NonNullWrites | FilterPredicate | AcceptsClass
                | AcceptsNullableClass | SupportsValueRevealer | SupportsKeyRevealer)]
public class FieldKeyValuePairEnumeratorBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
    : IStringBearer, IMoldSupportedValue<IEnumerator<KeyValuePair<TKey, TValue>>?>, ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>
      , ISupportsValueRevealer<TVRevealBase>, ISupportsKeyRevealer<TKRevealBase>
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothRevealers
    {
        get => Value;
        set => Value = value;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>>? Value { get; set; }

    public PalantírReveal<TKRevealBase> KeyRevealer { get; set; } = null!;

    public PalantírReveal<TVRevealBase> ValueRevealer { get; set; } = null!;

    public KeyValuePredicate<TKFilterBase, TVFilterBase> KeyValuePredicate { get; set; } =
        ISupportsKeyedCollectionPredicate<TKFilterBase, TVFilterBase>.GetNoFilterPredicate;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate
               (nameof(ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothRevealers)
              , ComplexTypeKeyedCollectionFieldWhenNonNullAddFilteredEnumeratorBothRevealers
              , KeyValuePredicate, ValueRevealer, KeyRevealer)
           .Complete();
}
