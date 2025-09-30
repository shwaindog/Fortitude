using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexType.KeyedCollectionFields;

public class KeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromArrayBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromArrayBothFormatStrings), WhenNonNullAddWithSelectKeysFromArrayBothFormatStrings, selectKeys
              , valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromSpanBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromSpanBothFormatStrings), WhenNonNullAddWithSelectKeysFromSpanBothFormatStrings, selectKeys.AsSpan()
              , valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromReadOnlySpanBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanBothFormatStrings), WhenNonNullAddWithSelectKeysFromReadOnlySpanBothFormatStrings
              , (ReadOnlySpan<TKSelectDerived>)selectKeys.AsSpan(), valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IReadOnlyList<TKSelectDerived> selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromListBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromListBothFormatStrings), WhenNonNullAddWithSelectKeysFromListBothFormatStrings
              , selectKeys, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerable<TKSelectDerived> selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromEnumerableBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysFromEnumerableBothFormatStrings), WhenNonNullAddWithSelectKeysFromEnumerableBothFormatStrings
              , selectKeys, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerator<TKSelectDerived> selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings), WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
              , selectKeys, valueFormatString , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString), WhenNonNullAddWithSelectKeysFromArrayValueRevealerKeyFormatString
              , selectKeys, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString), WhenNonNullAddWithSelectKeysFromSpanValueRevealerKeyFormatString
              , selectKeys.AsSpan(), valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString), WhenNonNullAddWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , (ReadOnlySpan<TKSelectDerived>)selectKeys.AsSpan(), valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IReadOnlyList<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString), WhenNonNullAddWithSelectKeysFromListValueRevealerKeyFormatString
              , selectKeys, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerable<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings, selectKeys, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerator<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings, selectKeys, valueRevealer
              , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromArrayBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromArrayBothRevealers), WhenNonNullAddWithSelectKeysFromArrayBothRevealers
              , selectKeys, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromSpanBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromSpanBothRevealers), WhenNonNullAddWithSelectKeysFromSpanBothRevealers
              , selectKeys.AsSpan(), valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers), WhenNonNullAddWithSelectKeysFromReadOnlySpanBothRevealers
              , (ReadOnlySpan<TKSelectDerived>)selectKeys.AsSpan(), valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IReadOnlyList<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysFromListBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeys
               (nameof(WhenNonNullAddWithSelectKeysFromListBothRevealers), WhenNonNullAddWithSelectKeysFromListBothRevealers
              , selectKeys, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerable<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , WhenNonNullAddWithSelectKeysKeyValuePairEnumerableBothRevealers, selectKeys, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenNonNullAddWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerator<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddWithSelectKeysEnumerate 
               (nameof(WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers), WhenNonNullAddWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , selectKeys, valueRevealer, keyRevealer)
           .Complete();
}