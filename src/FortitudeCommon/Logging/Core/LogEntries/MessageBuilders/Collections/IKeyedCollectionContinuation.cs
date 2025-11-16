// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
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
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple) where TKey : TKFilterBase where TValue : TVFilterBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase1, TVFilterBase, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase1, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase1, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull;
}
