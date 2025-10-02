#region

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

#endregion

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation.ScaffoldingTypes.ComplexTypeScaffolds.KeyedCollectionFields;

public class KeyValueDictionaryFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysAddFilteredKeyValueDictionaryBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredKeyValueDictionaryBothFormatStrings), AlwaysAddFilteredKeyValueDictionaryBothFormatStrings, filterPredicate, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public KeyValuePair<TKey, TValue>[]? AlwaysAddFilteredKeyValuePairArrayBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredKeyValuePairArrayBothFormatStrings), AlwaysAddFilteredKeyValuePairArrayBothFormatStrings, filterPredicate, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairListBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? AlwaysAddFilteredKeyValuePairListBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredKeyValuePairListBothFormatStrings), AlwaysAddFilteredKeyValuePairListBothFormatStrings, filterPredicate, valueFormatString, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? AlwaysAddFilteredKeyValuePairEnumerableBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate(nameof(AlwaysAddFilteredKeyValuePairEnumerableBothFormatStrings), AlwaysAddFilteredKeyValuePairEnumerableBothFormatStrings, filterPredicate, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorBothFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , string? valueFormatString = null
  , string? keyFormatString = null) : IStringBearer
    where TKey : TKFilterBase where TValue : TVFilterBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? AlwaysAddFilteredKeyValuePairEnumeratorBothFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate(nameof(AlwaysAddFilteredKeyValuePairEnumeratorBothFormatStrings), AlwaysAddFilteredKeyValuePairEnumeratorBothFormatStrings, filterPredicate, valueFormatString
                                                     , keyFormatString)
           .Complete();
}

public class KeyValueDictionaryValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysAddFilteredKeyValueDictionaryValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredKeyValueDictionaryValueRevealerKeyFormatStrings), AlwaysAddFilteredKeyValueDictionaryValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairArrayValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? AlwaysAddFilteredKeyValuePairArrayValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredKeyValuePairArrayValueRevealerKeyFormatStrings), AlwaysAddFilteredKeyValuePairArrayValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairListValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? AlwaysAddFilteredKeyValuePairListValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredKeyValuePairListValueRevealerKeyFormatStrings), AlwaysAddFilteredKeyValuePairListValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumerableValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? AlwaysAddFilteredKeyValuePairEnumerableValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate(nameof(AlwaysAddFilteredKeyValuePairEnumerableValueRevealerKeyFormatStrings), AlwaysAddFilteredKeyValuePairEnumerableValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValuePairEnumeratorValueRevealerKeyFormatStringsAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , string? keyFormatString = null) : IStringBearer 
    where TKey : TKFilterBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? AlwaysAddFilteredKeyValuePairEnumeratorValueRevealerKeyFormatStrings { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate(nameof(AlwaysAddFilteredKeyValuePairEnumeratorValueRevealerKeyFormatStrings), AlwaysAddFilteredKeyValuePairEnumeratorValueRevealerKeyFormatStrings, filterPredicate, valueRevealer, keyFormatString)
           .Complete();
}

public class KeyValueDictionaryBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyDictionary<TKey, TValue>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyDictionary<TKey, TValue>? AlwaysAddFilteredKeyValueDictionaryBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredKeyValueDictionaryBothRevealers), AlwaysAddFilteredKeyValueDictionaryBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairArrayBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    KeyValuePair<TKey, TValue>[]? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public KeyValuePair<TKey, TValue>[]? AlwaysAddFilteredKeyValuePairArrayBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredKeyValuePairArrayBothRevealers), AlwaysAddFilteredKeyValuePairArrayBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairListBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IReadOnlyList<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IReadOnlyList<KeyValuePair<TKey, TValue>>? AlwaysAddFilteredKeyValuePairListBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFiltered(nameof(AlwaysAddFilteredKeyValuePairListBothRevealers), AlwaysAddFilteredKeyValuePairListBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumerableBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IEnumerable<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerable<KeyValuePair<TKey, TValue>>? AlwaysAddFilteredKeyValuePairEnumerableBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate(nameof(AlwaysAddFilteredKeyValuePairEnumerableBothRevealers), AlwaysAddFilteredKeyValuePairEnumerableBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}

public class KeyValuePairEnumeratorBothRevealersAlwaysAddFilteredStringBearer<TKey, TValue, TKFilterBase, TKRevealBase, TVFilterBase, TVRevealBase>
(
    IEnumerator<KeyValuePair<TKey, TValue>>? value
  , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
  , PalantírReveal<TVRevealBase> valueRevealer
  , PalantírReveal<TKRevealBase> keyRevealer) : IStringBearer 
    where TKey : TKFilterBase, TKRevealBase where TValue : TVFilterBase, TVRevealBase
{
    public IEnumerator<KeyValuePair<TKey, TValue>>? AlwaysAddFilteredKeyValuePairEnumeratorBothRevealers { get; } = value;

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .KeyedCollectionField.AlwaysAddFilteredEnumerate(nameof(AlwaysAddFilteredKeyValuePairEnumeratorBothRevealers), AlwaysAddFilteredKeyValuePairEnumeratorBothRevealers, filterPredicate, valueRevealer, keyRevealer)
           .Complete();
}
