// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public class KeyedCollectionFinalAppend<TToReturn, TCallerType> : CollectionFinalAppend<TToReturn, TCallerType>, IKeyedCollectionFinalAppend
    where TCallerType : FLogEntryMessageBuilderBase<TToReturn, TCallerType>, TToReturn where TToReturn : class, IFLogMessageBuilder
{
    public void AddKeyed<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null, string? keyFormatString = null) =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue>?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyDictionary<TKey, TValue>?, PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase 
        where TValue : TVBase? =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase 
        where TValue : TVBase? =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase 
        where TValue : TVBase? =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase 
        where TValue : TVBase? =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase?, TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKBase 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull  =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase?, TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull  =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyStyler)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase?, TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull  =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(IReadOnlyDictionary<TKey, TValue>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase, TVRevealBase 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyStyler).PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null, string? keyFormatString = null) =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TVRevealBase>(KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString).PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue>[]?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TVRevealBase>((KeyValuePair<TKey, TValue>[]?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue>[]? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyStyler).PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue>[]?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase 
        where TValue : TVBase? =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase 
        where TValue : TVBase? =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase 
        where TValue : TVBase? =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase 
        where TValue : TVBase? =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull  =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : TVFilterBase?, TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull  =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyStyler)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : TVFilterBase?, TVRevealBase?  
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);


    public void AddKeyed<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);
    
    public void AddKeyed<TKey, TValue, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>) valueTuple)
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(PreappendCheckGetStringAppender(value), value, valueStyler, keyStyler).PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase? =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase? =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase? =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?  =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKBase, TVFilterBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull  =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull  =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull  =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : TVFilterBase?, TVRevealBase?  
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(PreappendCheckGetStringAppender(value), value, filterPredicate, valueStyler, keyStyler)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyed<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : TVFilterBase?, TVRevealBase?  
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendFilteredKeyedCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AppendKeyedCollectionEnumerate(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedEnumerate<TKey, TValue, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedEnumerate<TKey, TValue, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>) valueTuple) 
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(PreappendCheckGetStringAppender(value), value, valueStyler, keyStyler).PostAppendCheckAndReturn(value, this);

    public void AddKeyedEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedIterate<TEnumtr, TKey, TValue>(TEnumtr value, string? valueFormatString = null, string? keyFormatString = null)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?  =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>(PreappendCheckGetStringAppender(value), value, valueFormatString, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyedIterate<TEnumtr, TKey, TValue>((TEnumtr, string?, string?) valueTuple)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>? =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedIterate<TEnumtr, TKey, TValue>((TEnumtr, string?) valueTuple)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?   =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedIterate<TEnumtr, TKey, TValue, TVRevealBase>
        (TEnumtr value, PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyedIterate<TEnumtr, TKey, TValue, TVRevealBase>(
        (TEnumtr, PalantírReveal<TVRevealBase>, string?) valueTuple) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedIterate<TEnumtr, TKey, TValue, TVRevealBase>(
        (TEnumtr, PalantírReveal<TVRevealBase>) valueTuple) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase? 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);
    
    public void AddKeyedIterate<TEnumtr, TKey, TValue>
    (TEnumtr value, PalantírReveal<TValue> valueStyler
      , string? keyFormatString = null) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>(PreappendCheckGetStringAppender(value), value, valueStyler, keyFormatString)
            .PostAppendCheckAndReturn(value, this);

    
    public void AddKeyedIterate<TEnumtr, TKey, TValue>(
        (TEnumtr, PalantírReveal<TValue>, string?) valueTuple) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);
    
    public void AddKeyedIterate<TEnumtr, TKey, TValue>(
        (TEnumtr, PalantírReveal<TValue>) valueTuple) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);
    
    public void AddKeyedIterate<TEnumtr, TKey, TValue, TKRevealBase>
    (TEnumtr value, PalantírReveal<TValue> valueRevealer,  PalantírReveal<TKRevealBase> keyRevealer) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TKRevealBase>(PreappendCheckGetStringAppender(value), value, valueRevealer, keyRevealer)
            .PostAppendCheckAndReturn(value, this);
    
    public void AddKeyedIterate<TEnumtr, TKey, TValue, TKRevealBase>(
        (TEnumtr, PalantírReveal<TValue>, PalantírReveal<TKRevealBase>) valueTuple) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
        =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TKRevealBase>(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);
    
    public void AddKeyedIterate<TEnumtr, TKey, TValue, TVRevealBase>
        (TEnumtr value, PalantírReveal<TVRevealBase> valueRevealer,  PalantírReveal<TKey> keyRevealer) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>(PreappendCheckGetStringAppender(value), value, valueRevealer, keyRevealer)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyedIterate<TEnumtr, TKey, TValue, TVRevealBase>(
        (TEnumtr, PalantírReveal<TVRevealBase>, PalantírReveal<TKey>) valueTuple)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
        =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);
    
    public void AddKeyedIterate<TEnumtr, TKey, TValue>
        (TEnumtr value, PalantírReveal<TValue> valueRevealer, PalantírReveal<TKey> keyRevealer) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct =>
        AppendKeyedCollectionIterate(PreappendCheckGetStringAppender(value), value, valueRevealer, keyRevealer)
            .PostAppendCheckAndReturn(value, this);
    
    public void AddKeyedIterate<TEnumtr,TKey, TValue>(
        (TEnumtr, PalantírReveal<TValue>, PalantírReveal<TKey>) valueTuple) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct
        where TValue : struct
        =>
        AppendKeyedCollectionIterate(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);

    public void AddKeyedIterate<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(TEnumtr value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(PreappendCheckGetStringAppender(value), value, valueStyler, keyStyler)
            .PostAppendCheckAndReturn(value, this);

    public void AddKeyedIterate<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(
        (TEnumtr, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase? 
        where TKRevealBase : notnull 
        where TVRevealBase : notnull =>
        AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(valueTuple, PreappendCheckGetStringAppender(valueTuple))
            .PostAppendCheckAndReturn(valueTuple, this);
}
