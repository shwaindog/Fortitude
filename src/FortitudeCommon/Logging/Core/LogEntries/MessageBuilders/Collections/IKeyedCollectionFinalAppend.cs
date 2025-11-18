// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public interface IKeyedCollectionFinalAppend : ICollectionFinalAppend
{
    void AddKeyed<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null, string? keyFormatString = null);
    void AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);
    void AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);
    void AddKeyed<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null, string? keyFormatString = null);
    void AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple);
    void AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple);
    void AddKeyed<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null, string? keyFormatString = null);
    void AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);
    void AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void AddKeyedEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null);

    void AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);
    void AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void AddKeyedEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);
    void AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) 
        where TKey : TKBase 
        where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value,
        PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;
    
    void AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TVRevealBase>(IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>
          , PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>)valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;
}
