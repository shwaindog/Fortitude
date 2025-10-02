#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.KeyedCollectionFields;

public class KeyValueDictionaryFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddAllKeyValueDictionaryBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllKeyValueDictionaryBothFormatStrings), WhenNonNullAddAllKeyValueDictionaryBothFormatStrings, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue>
(
    KeyValuePair<TKey, TValue>[]? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public KeyValuePair<TKey, TValue>[]? WhenNonNullAddAllKeyValuePairArrayBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllKeyValuePairArrayBothFormatStrings), WhenNonNullAddAllKeyValuePairArrayBothFormatStrings, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairListBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenNonNullAddAllKeyValuePairListBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllKeyValuePairListBothFormatStrings), WhenNonNullAddAllKeyValuePairListBothFormatStrings, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenNonNullAddAllKeyValuePairEnumerableBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllKeyValuePairEnumerableBothFormatStrings), WhenNonNullAddAllKeyValuePairEnumerableBothFormatStrings, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorBothFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenNonNullAddAllKeyValuePairEnumeratorBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllKeyValuePairEnumeratorBothFormatStrings), WhenNonNullAddAllKeyValuePairEnumeratorBothFormatStrings, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddAllKeyValueDictionaryValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllKeyValueDictionaryValueRevealerKeyFormatStrings), WhenNonNullAddAllKeyValueDictionaryValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? WhenNonNullAddAllKeyValuePairArrayValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllKeyValuePairArrayValueRevealerKeyFormatStrings), WhenNonNullAddAllKeyValuePairArrayValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairListValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenNonNullAddAllKeyValuePairListValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllKeyValuePairListValueRevealerKeyFormatStrings), WhenNonNullAddAllKeyValuePairListValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenNonNullAddAllKeyValuePairEnumerableValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllKeyValuePairEnumerableValueRevealerKeyFormatStrings), WhenNonNullAddAllKeyValuePairEnumerableValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenNonNullAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenNonNullAddAllKeyValuePairEnumeratorValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllKeyValuePairEnumeratorValueRevealerKeyFormatStrings), WhenNonNullAddAllKeyValuePairEnumeratorValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddAllKeyValueDictionaryBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllKeyValueDictionaryBothRevealers), WhenNonNullAddAllKeyValueDictionaryBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairArrayBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? WhenNonNullAddAllKeyValuePairArrayBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllKeyValuePairArrayBothRevealers), WhenNonNullAddAllKeyValuePairArrayBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairListBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenNonNullAddAllKeyValuePairListBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAll(nameof(WhenNonNullAddAllKeyValuePairListBothRevealers), WhenNonNullAddAllKeyValuePairListBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumerableBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenNonNullAddAllKeyValuePairEnumerableBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllKeyValuePairEnumerableBothRevealers), WhenNonNullAddAllKeyValuePairEnumerableBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumeratorBothRevealersWhenNonNullAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenNonNullAddAllKeyValuePairEnumeratorBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddAllEnumerate(nameof(WhenNonNullAddAllKeyValuePairEnumeratorBothRevealers), WhenNonNullAddAllKeyValuePairEnumeratorBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}
