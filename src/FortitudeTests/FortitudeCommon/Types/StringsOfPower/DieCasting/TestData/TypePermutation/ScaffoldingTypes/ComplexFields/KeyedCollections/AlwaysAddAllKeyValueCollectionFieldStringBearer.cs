#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexFields.KeyedCollections;

public class KeyValueDictionaryFormatStringsAlwaysAddAllStringBearer<TKey, TValue>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysAddAllKeyValueDictionaryBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll(nameof(AlwaysAddAllKeyValueDictionaryBothFormatStrings), AlwaysAddAllKeyValueDictionaryBothFormatStrings, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue>
(
    KeyValuePair<TKey, TValue>[]? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public KeyValuePair<TKey, TValue>[]? AlwaysAddAllKeyValuePairArrayBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll(nameof(AlwaysAddAllKeyValuePairArrayBothFormatStrings), AlwaysAddAllKeyValuePairArrayBothFormatStrings, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairListBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? AlwaysAddAllKeyValuePairListBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll(nameof(AlwaysAddAllKeyValuePairListBothFormatStrings), AlwaysAddAllKeyValuePairListBothFormatStrings, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? AlwaysAddAllKeyValuePairEnumerableBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllKeyValuePairEnumerableBothFormatStrings), AlwaysAddAllKeyValuePairEnumerableBothFormatStrings, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorBothFormatStringsAlwaysAddAllStringBearer<TKey, TValue>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? AlwaysAddAllKeyValuePairEnumeratorBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllKeyValuePairEnumeratorBothFormatStrings), AlwaysAddAllKeyValuePairEnumeratorBothFormatStrings, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysAddAllKeyValueDictionaryValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll(nameof(AlwaysAddAllKeyValueDictionaryValueRevealerKeyFormatStrings), AlwaysAddAllKeyValueDictionaryValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? AlwaysAddAllKeyValuePairArrayValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll(nameof(AlwaysAddAllKeyValuePairArrayValueRevealerKeyFormatStrings), AlwaysAddAllKeyValuePairArrayValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairListValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? AlwaysAddAllKeyValuePairListValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll(nameof(AlwaysAddAllKeyValuePairListValueRevealerKeyFormatStrings), AlwaysAddAllKeyValuePairListValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? AlwaysAddAllKeyValuePairEnumerableValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllKeyValuePairEnumerableValueRevealerKeyFormatStrings), AlwaysAddAllKeyValuePairEnumerableValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorValueRevealerKeyFormatStringsAlwaysAddAllStringBearer<TKey, TValue, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? AlwaysAddAllKeyValuePairEnumeratorValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllKeyValuePairEnumeratorValueRevealerKeyFormatStrings), AlwaysAddAllKeyValuePairEnumeratorValueRevealerKeyFormatStrings, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysAddAllKeyValueDictionaryBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll(nameof(AlwaysAddAllKeyValueDictionaryBothRevealers), AlwaysAddAllKeyValueDictionaryBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairArrayBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? AlwaysAddAllKeyValuePairArrayBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll(nameof(AlwaysAddAllKeyValuePairArrayBothRevealers), AlwaysAddAllKeyValuePairArrayBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairListBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? AlwaysAddAllKeyValuePairListBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAll(nameof(AlwaysAddAllKeyValuePairListBothRevealers), AlwaysAddAllKeyValuePairListBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumerableBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? AlwaysAddAllKeyValuePairEnumerableBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllKeyValuePairEnumerableBothRevealers), AlwaysAddAllKeyValuePairEnumerableBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumeratorBothRevealersAlwaysAddAllStringBearer<TKey, TValue, TKRevealBase, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer where TKey : TKRevealBase where TValue : TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? AlwaysAddAllKeyValuePairEnumeratorBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddAllEnumerate(nameof(AlwaysAddAllKeyValuePairEnumeratorBothRevealers), AlwaysAddAllKeyValuePairEnumeratorBothRevealers, valueRevealer, keyRevealer)
           .Complete();
}
