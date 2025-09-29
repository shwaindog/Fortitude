#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.KeyedCollections;

public class KeyValueDictionaryFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedAddAllKeyValueDictionaryBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllKeyValueDictionaryBothFormatStrings), WhenPopulatedAddAllKeyValueDictionaryBothFormatStrings, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue>
(
    KeyValuePair<TKey, TValue>[]? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public KeyValuePair<TKey, TValue>[]? WhenPopulatedAddAllKeyValuePairArrayBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllKeyValuePairArrayBothFormatStrings), WhenPopulatedAddAllKeyValuePairArrayBothFormatStrings, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairListBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenPopulatedAddAllKeyValuePairListBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllKeyValuePairListBothFormatStrings), WhenPopulatedAddAllKeyValuePairListBothFormatStrings, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenPopulatedAddAllKeyValuePairEnumerableBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllKeyValuePairEnumerableBothFormatStrings), WhenPopulatedAddAllKeyValuePairEnumerableBothFormatStrings, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorBothFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenPopulatedAddAllKeyValuePairEnumeratorBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllKeyValuePairEnumeratorBothFormatStrings), WhenPopulatedAddAllKeyValuePairEnumeratorBothFormatStrings, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedAddAllKeyValueDictionaryValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllKeyValueDictionaryValueRevealerKeyFormatStrings), WhenPopulatedAddAllKeyValueDictionaryValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? WhenPopulatedAddAllKeyValuePairArrayValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllKeyValuePairArrayValueRevealerKeyFormatStrings), WhenPopulatedAddAllKeyValuePairArrayValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairListValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenPopulatedAddAllKeyValuePairListValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllKeyValuePairListValueRevealerKeyFormatStrings), WhenPopulatedAddAllKeyValuePairListValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenPopulatedAddAllKeyValuePairEnumerableValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllKeyValuePairEnumerableValueRevealerKeyFormatStrings), WhenPopulatedAddAllKeyValuePairEnumerableValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenPopulatedAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenPopulatedAddAllKeyValuePairEnumeratorValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllKeyValuePairEnumeratorValueRevealerKeyFormatStrings), WhenPopulatedAddAllKeyValuePairEnumeratorValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedAddAllKeyValueDictionaryBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllKeyValueDictionaryBothRevealers), WhenPopulatedAddAllKeyValueDictionaryBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairArrayBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? WhenPopulatedAddAllKeyValuePairArrayBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllKeyValuePairArrayBothRevealers), WhenPopulatedAddAllKeyValuePairArrayBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairListBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenPopulatedAddAllKeyValuePairListBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAll(nameof(WhenPopulatedAddAllKeyValuePairListBothRevealers), WhenPopulatedAddAllKeyValuePairListBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumerableBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenPopulatedAddAllKeyValuePairEnumerableBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllKeyValuePairEnumerableBothRevealers), WhenPopulatedAddAllKeyValuePairEnumerableBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumeratorBothRevealersWhenPopulatedAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenPopulatedAddAllKeyValuePairEnumeratorBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedAddAllEnumerate(nameof(WhenPopulatedAddAllKeyValuePairEnumeratorBothRevealers), WhenPopulatedAddAllKeyValuePairEnumeratorBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}
