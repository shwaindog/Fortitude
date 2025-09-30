using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexType.KeyedCollectionFields;

public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayBothFormatStrings), WhenPopulatedWithSelectKeysFromArrayBothFormatStrings, selectKeys
              , valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanBothFormatStrings), WhenPopulatedWithSelectKeysFromSpanBothFormatStrings, selectKeys.AsSpan()
              , valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothFormatStrings), WhenPopulatedWithSelectKeysFromReadOnlySpanBothFormatStrings
              , (ReadOnlySpan<TKSelectDerived>)selectKeys.AsSpan(), valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IReadOnlyList<TKSelectDerived> selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListBothFormatStrings), WhenPopulatedWithSelectKeysFromListBothFormatStrings
              , selectKeys, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerable<TKSelectDerived> selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromEnumerableBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysFromEnumerableBothFormatStrings), WhenPopulatedWithSelectKeysFromEnumerableBothFormatStrings
              , selectKeys, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerator<TKSelectDerived> selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothFormatStrings), WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
              , selectKeys, valueFormatString , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString), WhenPopulatedWithSelectKeysFromArrayValueRevealerKeyFormatString
              , selectKeys, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString), WhenPopulatedWithSelectKeysFromSpanValueRevealerKeyFormatString
              , selectKeys.AsSpan(), valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString), WhenPopulatedWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , (ReadOnlySpan<TKSelectDerived>)selectKeys.AsSpan(), valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IReadOnlyList<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString), WhenPopulatedWithSelectKeysFromListValueRevealerKeyFormatString
              , selectKeys, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerable<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings, selectKeys, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerator<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings, selectKeys, valueRevealer
              , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromArrayBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromArrayBothRevealers), WhenPopulatedWithSelectKeysFromArrayBothRevealers
              , selectKeys, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromSpanBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromSpanBothRevealers), WhenPopulatedWithSelectKeysFromSpanBothRevealers
              , selectKeys.AsSpan(), valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers), WhenPopulatedWithSelectKeysFromReadOnlySpanBothRevealers
              , (ReadOnlySpan<TKSelectDerived>)selectKeys.AsSpan(), valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IReadOnlyList<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysFromListBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeys
               (nameof(WhenPopulatedWithSelectKeysFromListBothRevealers), WhenPopulatedWithSelectKeysFromListBothRevealers
              , selectKeys, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerable<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , WhenPopulatedWithSelectKeysKeyValuePairEnumerableBothRevealers, selectKeys, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenPopulatedWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerator<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithSelectKeysEnumerate 
               (nameof(WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers), WhenPopulatedWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , selectKeys, valueRevealer, keyRevealer)
           .Complete();
}