// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

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

    void AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value,
        StringBearerRevealState<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void AddKeyed<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, StringBearerRevealState<TVBase>, string?) valueTuple) where TValue : TVBase;

    void AddKeyed<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, StringBearerRevealState<TVBase>) valueTuple) where TValue : TVBase;

    void AddKeyed<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase;

    void AddKeyed<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, StringBearerRevealState<TVBase>, string?) valueTuple) where TValue : TVBase;
    void AddKeyed<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, StringBearerRevealState<TVBase>) valueTuple) where TValue : TVBase;

    void AddKeyed<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StringBearerRevealState<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase;

    void AddKeyed<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    void AddKeyed<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>) valueTuple) where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, string?) valueTuple) where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>) valueTuple) where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, string?) valueTuple) where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>) valueTuple) where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, StringBearerRevealState<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , StringBearerRevealState<TVBase> valueStyler, StringBearerRevealState<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, StringBearerRevealState<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, StringBearerRevealState<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StringBearerRevealState<TVBase> valueStyler, StringBearerRevealState<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StringBearerRevealState<TVBase>, StringBearerRevealState<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, StringBearerRevealState<TVBase2>, StringBearerRevealState<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, StringBearerRevealState<TVBase2>, StringBearerRevealState<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, StringBearerRevealState<TVBase2> valueStyler, StringBearerRevealState<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, StringBearerRevealState<TVBase2>, StringBearerRevealState<TKBase2>)
            valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;
}
