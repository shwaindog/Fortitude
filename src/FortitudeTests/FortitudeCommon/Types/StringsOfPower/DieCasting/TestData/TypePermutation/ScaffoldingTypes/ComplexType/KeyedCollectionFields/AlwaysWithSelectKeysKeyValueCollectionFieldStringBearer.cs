using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexType.KeyedCollectionFields;

public class KeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayBothFormatStrings), AlwaysWithSelectKeysFromArrayBothFormatStrings, selectKeys
              , valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanBothFormatStrings), AlwaysWithSelectKeysFromSpanBothFormatStrings, selectKeys.AsSpan()
              , valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings), AlwaysWithSelectKeysFromReadOnlySpanBothFormatStrings
              , (ReadOnlySpan<TKSelectDerived>)selectKeys.AsSpan(), valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IReadOnlyList<TKSelectDerived> selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListBothFormatStrings), AlwaysWithSelectKeysFromListBothFormatStrings
              , selectKeys, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerable<TKSelectDerived> selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromEnumerableBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysFromEnumerableBothFormatStrings), AlwaysWithSelectKeysFromEnumerableBothFormatStrings
              , selectKeys, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothFormatStringsAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerator<TKSelectDerived> selectKeys
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings), AlwaysWithSelectKeysKeyValuePairEnumeratorBothFormatStrings
              , selectKeys, valueFormatString , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString), AlwaysWithSelectKeysFromArrayValueRevealerKeyFormatString
              , selectKeys, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString), AlwaysWithSelectKeysFromSpanValueRevealerKeyFormatString
              , selectKeys.AsSpan(), valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString), AlwaysWithSelectKeysFromReadOnlySpanValueRevealerKeyFormatString
              , (ReadOnlySpan<TKSelectDerived>)selectKeys.AsSpan(), valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IReadOnlyList<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListValueRevealerKeyFormatString { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListValueRevealerKeyFormatString), AlwaysWithSelectKeysFromListValueRevealerKeyFormatString
              , selectKeys, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerable<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumerableValueRevealerKeyFormatStrings, selectKeys, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerator<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer
    where TKSelectDerived : TKey where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings)
              , AlwaysWithSelectKeysKeyValuePairEnumeratorValueRevealerKeyFormatStrings, selectKeys, valueRevealer
              , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothRevealersAlwaysWithSelectKeysArrayStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromArrayBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromArrayBothRevealers), AlwaysWithSelectKeysFromArrayBothRevealers
              , selectKeys, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersAlwaysWithSelectKeysSpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromSpanBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromSpanBothRevealers), AlwaysWithSelectKeysFromSpanBothRevealers
              , selectKeys.AsSpan(), valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersAlwaysWithSelectKeysReadOnlySpanStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , TKSelectDerived[] selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromReadOnlySpanBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromReadOnlySpanBothRevealers), AlwaysWithSelectKeysFromReadOnlySpanBothRevealers
              , (ReadOnlySpan<TKSelectDerived>)selectKeys.AsSpan(), valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersAlwaysWithSelectKeysListStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IReadOnlyList<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysFromListBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeys
               (nameof(AlwaysWithSelectKeysFromListBothRevealers), AlwaysWithSelectKeysFromListBothRevealers
              , selectKeys, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersAlwaysWithSelectKeysEnumerableStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerable<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers)
              , AlwaysWithSelectKeysKeyValuePairEnumerableBothRevealers, selectKeys, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValueDictionaryBothRevealersAlwaysWithSelectKeysEnumeratorStringBearer<TKey, TValue, TKSelectDerived, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , IEnumerator<TKSelectDerived> selectKeys
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer
    where TKSelectDerived : TKey where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysWithSelectKeysEnumerate 
               (nameof(AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers), AlwaysWithSelectKeysKeyValuePairEnumeratorBothRevealers
              , selectKeys, valueRevealer, keyRevealer)
           .Complete();
}