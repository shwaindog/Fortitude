// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public class KeyedCollectionContinuation<TToReturn, TCallerType> : CollectionAppendContinuation<TToReturn, TCallerType>
  , IKeyedCollectionContinuation<TToReturn>
    where TCallerType : FLogEntryMessageBuilderBase<TToReturn, TCallerType>, TToReturn where TToReturn : class, IFLogMessageBuilder
{
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null) =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null) =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AppendKeyedCollectionEnumerate(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) =>
        AppendKeyedCollectionEnumerate(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null)
        where TValue : TVRevealBase
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null)
        where TValue : TVRevealBase
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?
      , PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>((IEnumerable<KeyValuePair<TKey, TValue?>>?
      , PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , PalantírReveal<TVRevealBase> valueRevealer, PalantírReveal<TKRevealBase> keyRevealer) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueRevealer, keyRevealer).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyStyler).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(PreappendCheckGetStringAppender(value), value, valueStyler, keyStyler).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerator<KeyValuePair<TKey, TValue?>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(PreappendCheckGetStringAppender(value), value, valueStyler, keyStyler).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public TToReturn AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IEnumerator<KeyValuePair<TKey, TValue?>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase where TValue : TVFilterBase =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple)
        where TKey : TKFilterBase where TValue : TVFilterBase =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase 
        where TValue : TVBase =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple)
        where TKey : TKFilterBase
        where TValue : TVFilterBase =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple)
        where TKey : TKFilterBase
        where TValue : TVFilterBase =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple)
        where TKey : TKFilterBase
        where TValue : TVFilterBase =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKBase
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKBase
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKBase
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKBase
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKBase
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKBase
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKBase
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKBase
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKBase
        where TValue : TVFilterBase, TVRevealBase
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyStyler)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue?>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>,
            PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue?>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyStyler)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue?>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>)
            valueTuple)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue?>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyStyler)
            .PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue?>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase
        where TValue : TVFilterBase, TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public TToReturn AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue?>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKRevealBase
        where TValue : TVRevealBase
        where TKRevealBase : notnull
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyStyler).PostAppendCheckAndReturn(value, this);
}
