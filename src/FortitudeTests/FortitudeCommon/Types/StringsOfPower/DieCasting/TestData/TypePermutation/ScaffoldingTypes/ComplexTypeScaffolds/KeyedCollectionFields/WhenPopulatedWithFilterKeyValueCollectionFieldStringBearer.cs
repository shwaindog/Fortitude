#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.KeyedCollectionFields;

public class KeyValueDictionaryFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithFilterKeyValueDictionaryBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterKeyValueDictionaryBothFormatStrings), WhenPopulatedWithFilterKeyValueDictionaryBothFormatStrings, filterPredicate, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public KeyValuePair<TKey, TValue>[]? WhenPopulatedWithFilterKeyValuePairArrayBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterKeyValuePairArrayBothFormatStrings), WhenPopulatedWithFilterKeyValuePairArrayBothFormatStrings, filterPredicate, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairListBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenPopulatedWithFilterKeyValuePairListBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterKeyValuePairListBothFormatStrings), WhenPopulatedWithFilterKeyValuePairListBothFormatStrings, filterPredicate, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenPopulatedWithFilterKeyValuePairEnumerableBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate(nameof(WhenPopulatedWithFilterKeyValuePairEnumerableBothFormatStrings), WhenPopulatedWithFilterKeyValuePairEnumerableBothFormatStrings, filterPredicate, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorBothFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenPopulatedWithFilterKeyValuePairEnumeratorBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate(nameof(WhenPopulatedWithFilterKeyValuePairEnumeratorBothFormatStrings), WhenPopulatedWithFilterKeyValuePairEnumeratorBothFormatStrings, filterPredicate, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithFilterKeyValueDictionaryValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterKeyValueDictionaryValueRevealerKeyFormatStrings), WhenPopulatedWithFilterKeyValueDictionaryValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? WhenPopulatedWithFilterKeyValuePairArrayValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterKeyValuePairArrayValueRevealerKeyFormatStrings), WhenPopulatedWithFilterKeyValuePairArrayValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairListValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenPopulatedWithFilterKeyValuePairListValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterKeyValuePairListValueRevealerKeyFormatStrings), WhenPopulatedWithFilterKeyValuePairListValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenPopulatedWithFilterKeyValuePairEnumerableValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate(nameof(WhenPopulatedWithFilterKeyValuePairEnumerableValueRevealerKeyFormatStrings), WhenPopulatedWithFilterKeyValuePairEnumerableValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorValueRevealerKeyFormatStringsWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenPopulatedWithFilterKeyValuePairEnumeratorValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate(nameof(WhenPopulatedWithFilterKeyValuePairEnumeratorValueRevealerKeyFormatStrings), WhenPopulatedWithFilterKeyValuePairEnumeratorValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? WhenPopulatedWithFilterKeyValueDictionaryBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterKeyValueDictionaryBothRevealers), WhenPopulatedWithFilterKeyValueDictionaryBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairArrayBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? WhenPopulatedWithFilterKeyValuePairArrayBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterKeyValuePairArrayBothRevealers), WhenPopulatedWithFilterKeyValuePairArrayBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairListBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? WhenPopulatedWithFilterKeyValuePairListBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilter(nameof(WhenPopulatedWithFilterKeyValuePairListBothRevealers), WhenPopulatedWithFilterKeyValuePairListBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumerableBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? WhenPopulatedWithFilterKeyValuePairEnumerableBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate(nameof(WhenPopulatedWithFilterKeyValuePairEnumerableBothRevealers), WhenPopulatedWithFilterKeyValuePairEnumerableBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumeratorBothRevealersWhenPopulatedWithFilterStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? WhenPopulatedWithFilterKeyValuePairEnumeratorBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.WhenPopulatedWithFilterEnumerate(nameof(WhenPopulatedWithFilterKeyValuePairEnumeratorBothRevealers), WhenPopulatedWithFilterKeyValuePairEnumeratorBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}
