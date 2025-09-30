#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexType.KeyedCollectionFields;

public class KeyValueDictionaryFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddFilteredKeyValueDictionaryBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredKeyValueDictionaryBothFormatStrings), WhenNonNullAddFilteredKeyValueDictionaryBothFormatStrings, filterPredicate, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public KeyValuePair<TKey, TValue>[]? WhenNonNullAddFilteredKeyValuePairArrayBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredKeyValuePairArrayBothFormatStrings), WhenNonNullAddFilteredKeyValuePairArrayBothFormatStrings, filterPredicate, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairListBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenNonNullAddFilteredKeyValuePairListBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredKeyValuePairListBothFormatStrings), WhenNonNullAddFilteredKeyValuePairListBothFormatStrings, filterPredicate, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenNonNullAddFilteredKeyValuePairEnumerableBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate(nameof(WhenNonNullAddFilteredKeyValuePairEnumerableBothFormatStrings), WhenNonNullAddFilteredKeyValuePairEnumerableBothFormatStrings, filterPredicate, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorBothFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenNonNullAddFilteredKeyValuePairEnumeratorBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate(nameof(WhenNonNullAddFilteredKeyValuePairEnumeratorBothFormatStrings), WhenNonNullAddFilteredKeyValuePairEnumeratorBothFormatStrings, filterPredicate, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddFilteredKeyValueDictionaryValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredKeyValueDictionaryValueRevealerKeyFormatStrings), WhenNonNullAddFilteredKeyValueDictionaryValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? WhenNonNullAddFilteredKeyValuePairArrayValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredKeyValuePairArrayValueRevealerKeyFormatStrings), WhenNonNullAddFilteredKeyValuePairArrayValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairListValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenNonNullAddFilteredKeyValuePairListValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredKeyValuePairListValueRevealerKeyFormatStrings), WhenNonNullAddFilteredKeyValuePairListValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenNonNullAddFilteredKeyValuePairEnumerableValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate(nameof(WhenNonNullAddFilteredKeyValuePairEnumerableValueRevealerKeyFormatStrings), WhenNonNullAddFilteredKeyValuePairEnumerableValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenNonNullAddFilteredKeyValuePairEnumeratorValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate(nameof(WhenNonNullAddFilteredKeyValuePairEnumeratorValueRevealerKeyFormatStrings), WhenNonNullAddFilteredKeyValuePairEnumeratorValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenNonNullAddFilteredKeyValueDictionaryBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredKeyValueDictionaryBothRevealers), WhenNonNullAddFilteredKeyValueDictionaryBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairArrayBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? WhenNonNullAddFilteredKeyValuePairArrayBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredKeyValuePairArrayBothRevealers), WhenNonNullAddFilteredKeyValuePairArrayBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairListBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenNonNullAddFilteredKeyValuePairListBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFiltered(nameof(WhenNonNullAddFilteredKeyValuePairListBothRevealers), WhenNonNullAddFilteredKeyValuePairListBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumerableBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenNonNullAddFilteredKeyValuePairEnumerableBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate(nameof(WhenNonNullAddFilteredKeyValuePairEnumerableBothRevealers), WhenNonNullAddFilteredKeyValuePairEnumerableBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumeratorBothRevealersWhenNonNullAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenNonNullAddFilteredKeyValuePairEnumeratorBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenNonNullAddFilteredEnumerate(nameof(WhenNonNullAddFilteredKeyValuePairEnumeratorBothRevealers), WhenNonNullAddFilteredKeyValuePairEnumeratorBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}
