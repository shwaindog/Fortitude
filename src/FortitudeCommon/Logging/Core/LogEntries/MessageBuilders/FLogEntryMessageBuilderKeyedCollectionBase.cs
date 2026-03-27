// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using FortitudeCommon.Types.StringsOfPower.DieCasting.MapCollectionType;
using static FortitudeCommon.Types.StringsOfPower.DieCasting.FormatFlags;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder
{
    protected ITheOneString? AppendKeyedCollection<TKey, TValue>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, ITheOneString? appender)
    {
        appender?.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((IReadOnlyDictionary<TKey, TValue>?, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((IReadOnlyDictionary<TKey, TValue>?, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
        ((IReadOnlyDictionary<TKey, TValue>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple, ITheOneString? appender)
        where TKey : TKRevealBase 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase?
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase?
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase?
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase 
        where TValue : TVFilterBase?
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase, TKRevealBase 
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        (KeyValuePair<TKey, TValue>[]? value, ITheOneString? appender)
    {
        appender?.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((KeyValuePair<TKey, TValue>[]?, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((KeyValuePair<TKey, TValue>[]?, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
        ((KeyValuePair<TKey, TValue>[]?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple, ITheOneString? appender)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, ITheOneString? appender)
    {
        appender?.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TVRevealBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollection<TKey, TValue, TKRevealBase, TVRevealBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?, string?) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, string?) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>) valueTuple, ITheOneString? appender)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, string? keyFormatString = null)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>, string?) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, customValueStyler, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, customValueStyler, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TVRevealBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>) valueTuple
      , ITheOneString? appender) 
        where TKey : TKFilterBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, filterPredicate, customTypeStyler) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKFilterBase, TVFilterBase> filterPredicate
      , PalantírReveal<TVRevealBase> valueStyler, PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredKeyedCollection<TKey, TValue, TKFilterBase, TVFilterBase, TKRevealBase, TVRevealBase>
    ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKFilterBase, TVFilterBase>, PalantírReveal<TVRevealBase>
          , PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKFilterBase?, TKRevealBase? 
        where TValue : TVFilterBase?, TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
    (ITheOneString? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple, ITheOneString? appender)
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, ITheOneString? appender)
    {
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerateValueRevealer(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVRevealBase>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerateValueRevealer(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TVRevealBase>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerateValueRevealer(value, valueFormatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKRevealBase> keyStyler) 
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerateBothRevealers(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionEnumerate<TKey, TValue, TKRevealBase, TVRevealBase>
    ((IEnumerable<KeyValuePair<TKey, TValue>>?, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueRevealer, keyRevealer) = valueTuple;
        appender?
            .StartKeyedCollectionType("")
            .AddAllEnumerateBothRevealers(value, valueRevealer, keyRevealer)
            .Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>
    (ITheOneString? toAppendTo, TEnumtr value, string? valueFormatString = null
      , string? keyFormatString = null)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    {
        (toAppendTo?.StartKeyedCollectionType(""))
            ?.AddAllIterate<IEnumerator<KeyValuePair<TKey, TValue>>,TKey,TValue>(value, valueFormatString, keyFormatString)
            .Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>
        ((TEnumtr, string?, string?) valueTuple, ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        (appender?.StartKeyedCollectionType(""))
            ?.AddAllIterate<IEnumerator<KeyValuePair<TKey, TValue>>,TKey,TValue>(value, valueFormatString, keyFormatString)
            .Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>
        ((TEnumtr, string?) valueTuple, ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    {
        var (value, valueFormatString) = valueTuple;
        (appender?.StartKeyedCollectionType(""))
            ?.AddAllIterate<IEnumerator<KeyValuePair<TKey, TValue>>,TKey,TValue>(value, valueFormatString)
            .Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>
        (TEnumtr value, ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
    {
        appender?
            .StartKeyedCollectionType("")
            .AddAllIterate<IEnumerator<KeyValuePair<TKey, TValue>>,TKey,TValue>(value)
            .Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, TEnumtr value, PalantírReveal<TVRevealBase> valueStyler
      , string? keyFormatString = null) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        toAppendTo?
            .StartKeyedCollectionType("")
            .AddAllIterateValueRevealer<IEnumerator<KeyValuePair<TKey, TValue>>,TKey,TValue, TVRevealBase>(value, valueStyler, keyFormatString)
            .Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>
        ((TEnumtr, PalantírReveal<TVRevealBase>, string?) valueTuple, ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?
            .StartKeyedCollectionType("")
            .AddAllIterateValueRevealer<IEnumerator<KeyValuePair<TKey, TValue>>,TKey,TValue, TVRevealBase>(value, valueFormatString, keyFormatString)
            .Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>
        ((TEnumtr, PalantírReveal<TVRevealBase>) valueTuple, ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueFormatString) = valueTuple;
        appender?
            .StartKeyedCollectionType("")
            .AddAllIterateValueRevealer<IEnumerator<KeyValuePair<TKey, TValue>>,TKey,TValue, TVRevealBase>(value, valueFormatString)
            .Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>
    (ITheOneString? toAppendTo, TEnumtr value, PalantírReveal<TValue> valueStyler
      , string? keyFormatString = null) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        toAppendTo?
            .StartKeyedCollectionType("")
            .AddAllIterateNullValueRevealer<IEnumerator<KeyValuePair<TKey, TValue?>>,TKey,TValue>(value, valueStyler, keyFormatString)
            .Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>
        ((TEnumtr, PalantírReveal<TValue>) valueTuple, ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var (value, valueFormatString) = valueTuple;
        appender?
            .StartKeyedCollectionType("")
            .AddAllIterateNullValueRevealer<TEnumtr,TKey,TValue>(value, valueFormatString)
            .Complete();
        return appender;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>
        ((TEnumtr, PalantírReveal<TValue>, string?) valueTuple, ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TValue : struct
    {
        var (value, valueRevealer, keyFormatString) = valueTuple;
        appender?
            .StartKeyedCollectionType("")
            .AddAllIterateNullValueRevealer<TEnumtr,TKey,TValue>(value, valueRevealer, keyFormatString)
            .Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionIterateNoNullables<TEnumtr, TKey, TValue>
    (ITheOneString? toAppendTo, TEnumtr value, PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKey> keyRevealer) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : notnull 
        where TValue : notnull
    {
        toAppendTo?
            .StartKeyedCollectionType("")
            .AddAllIterateBothRevealers(value, valueRevealer, keyRevealer)
            .Complete();
        return toAppendTo;
    }

    protected ITheOneString? AppendKeyedCollectionIterateNoNullables<TEnumtr, TKey, TValue>
        ((TEnumtr, PalantírReveal<TValue>, PalantírReveal<TKey>) valueTuple, ITheOneString? toAppendTo) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : notnull 
        where TValue : notnull
    {
        var (value, valueRevealer, keyRevealer) = valueTuple;
        toAppendTo?
            .StartKeyedCollectionType("")
            .AddAllIterateBothRevealers(value, valueRevealer, keyRevealer)
            .Complete();
        return toAppendTo;
    }

    protected ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
    (ITheOneString? toAppendTo, TEnumtr value, PalantírReveal<TVRevealBase> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        toAppendTo?
            .StartKeyedCollectionType("")
            .AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(value, valueRevealer, keyRevealer)
            .Complete();
        return toAppendTo;
    }

    protected ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>
    ((TEnumtr, PalantírReveal<TVRevealBase>, PalantírReveal<TKRevealBase>) valueTuple, ITheOneString? toAppendTo) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue>>?
        where TKey : TKRevealBase? 
        where TValue : TVRevealBase?
        where TKRevealBase : notnull
        where TVRevealBase : notnull
    {
        var (value, valueRevealer, keyRevealer) = valueTuple;
        toAppendTo?
            .StartKeyedCollectionType("")
            .AddAllIterateBothRevealers<TEnumtr, TKey, TValue, TKRevealBase, TVRevealBase>(value, valueRevealer, keyRevealer)
            .Complete();
        return toAppendTo;
    }

    protected ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TKRevealBase>
    (ITheOneString? toAppendTo, TEnumtr value, PalantírReveal<TValue> valueRevealer
      , PalantírReveal<TKRevealBase> keyRevealer) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase? 
        where TValue : struct
        where TKRevealBase : notnull
    {
        toAppendTo?
            .StartKeyedCollectionType("")
            .AddAllIterateBothWithNullValueRevealersExplicit<TEnumtr, TKey, TValue, TKRevealBase>(value, valueRevealer, keyRevealer)
            .Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TKRevealBase>
    ((TEnumtr, PalantírReveal<TValue>, PalantírReveal<TKRevealBase>) valueTuple
      , ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey, TValue?>>?
        where TKey : TKRevealBase?
        where TValue : struct
        where TKRevealBase : notnull
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllIterateBothWithNullValueRevealersExplicit<TEnumtr, TKey, TValue, TKRevealBase>(value, valueFormatString, keyFormatString)
                .Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>
    (ITheOneString? toAppendTo, TEnumtr value, PalantírReveal<TVRevealBase> valueStyler
      , PalantírReveal<TKey> keyStyler) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        toAppendTo?
            .StartKeyedCollectionType("")
            .AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(value, valueStyler, keyStyler)
            .Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue, TVRevealBase>
    ((TEnumtr, PalantírReveal<TVRevealBase>, PalantírReveal<TKey>) valueTuple
      , ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue>>?
        where TKey : struct 
        where TValue : TVRevealBase?
        where TVRevealBase : notnull
    {
        var (value, valueRevealer, keyRevealer) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllIterateBothWithNullKeyRevealers<TEnumtr, TKey, TValue, TVRevealBase>(value, valueRevealer, keyRevealer)
                .Complete();
        return appender;
    }

    protected ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>
    (ITheOneString? toAppendTo, TEnumtr value, PalantírReveal<TValue> valueStyler
      , PalantírReveal<TKey> keyStyler) 
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct 
        where TValue : struct
    {
        toAppendTo?
            .StartKeyedCollectionType("")
            .AddAllIterateBothNullRevealers(value, valueStyler, keyStyler)
            .Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendKeyedCollectionIterate<TEnumtr, TKey, TValue>
    ((TEnumtr, PalantírReveal<TValue>, PalantírReveal<TKey>) valueTuple
      , ITheOneString? appender)
        where TEnumtr : IEnumerator<KeyValuePair<TKey?, TValue?>>?
        where TKey : struct 
        where TValue : struct
    {
        var (value, valueRevealer, keyRevealer) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllIterateBothNullRevealers(value, valueRevealer, keyRevealer)
                .Complete();
        return appender;
    }
}
