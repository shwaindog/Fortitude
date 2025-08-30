// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public interface IKeyedCollectionContinuation<out TToReturn> : ICollectionAppendContinuation<TToReturn>
{
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>)
            valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;
}
