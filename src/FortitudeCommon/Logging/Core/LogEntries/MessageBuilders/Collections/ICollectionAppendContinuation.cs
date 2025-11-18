// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public interface ICollectionAppendContinuation<out TToReturn>
{
    [MustUseReturnValue("Use Final*, AndFinal* or WithOnly* to finish and send LogEntry")]
    TToReturn Add<TStyleObj>(TStyleObj[]? value) where TStyleObj : class, IStringBearer;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TStyleObj>(IReadOnlyList<TStyleObj>? value) where TStyleObj : class, IStringBearer;

    [MustUseReturnValue("Use Final*, AndFinal* or WithOnly* to finish and send LogEntry")]
    TToReturn AddFormat<TFmt>(TFmt[]? value, string? formatString = null) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn AddFormat<TFmt>((TFmt[]?, string?) valueTuple) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn AddFormat<TFmt>(IReadOnlyList<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn AddFormat<TFmt>((IReadOnlyList<TFmt>?, string?) valueTuple) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TCloaked, TRevealBase>(TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TCloaked, TRevealBase>((TCloaked?[]?, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TCloaked, TRevealBase>(IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TCloaked, TRevealBase>((IReadOnlyList<TCloaked?>?, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TStyleObj>(TStyleObj[]? value, OrderedCollectionPredicate<TStyleObj> filter) where TStyleObj : class, IStringBearer;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TStyleObj>(IReadOnlyList<TStyleObj>? value, OrderedCollectionPredicate<TStyleObj> filter)
        where TStyleObj : class, IStringBearer;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn AddFormat<TFmt, TBase>(TFmt[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where TFmt : ISpanFormattable, TBase;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn AddFormat<TFmt, TBase>(
        (TFmt[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where TFmt : ISpanFormattable, TBase;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn AddFormat<TFmt, TBase>(
        (TFmt[]?, OrderedCollectionPredicate<TBase>) valueTuple) where TFmt : ISpanFormattable, TBase;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn AddFormat<TFmt, TBase>(IReadOnlyList<TFmt>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where TFmt : ISpanFormattable, TBase;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn AddFormat<TFmt, TBase>(
        (IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where TFmt : ISpanFormattable, TBase;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn AddFormat<TFmt, TBase>(
        (IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>) valueTuple) where TFmt : ISpanFormattable, TBase;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TCloaked, TFilterBase, TRevealBase>(TCloaked[]? value
      , OrderedCollectionPredicate<TFilterBase> filter, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TCloaked, TFilterBase, TRevealBase>(
        (TCloaked[]?, OrderedCollectionPredicate<TFilterBase>, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TCloaked, TFilterBase, TRevealBase>(IReadOnlyList<TCloaked>? value
      , OrderedCollectionPredicate<TFilterBase> filter, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TCloaked, TFilterBase, TRevealBase>(
        (IReadOnlyList<TCloaked>?, OrderedCollectionPredicate<TFilterBase>, PalantírReveal<TRevealBase>) valueTuple)
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    TToReturn AddEnumerate<TStyleObj>(IEnumerable<TStyleObj>? value) where TStyleObj : class, IStringBearer;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddEnumerate<TStyleObj>(IEnumerator<TStyleObj>? value) where TStyleObj : class, IStringBearer;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddFormatEnumerate<TFmt>(IEnumerable<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddFormatEnumerate<TFmt>((IEnumerable<TFmt>?, string?) valueTuple) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddFormatEnumerate<TFmt>(IEnumerator<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddFormatEnumerate<TFmt>((IEnumerator<TFmt>?, string?) valueTuple) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TCloaked, TRevealBase>(IEnumerable<TCloaked>? value
      , PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TCloaked, TRevealBase>((IEnumerable<TCloaked?>?, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TCloaked, TRevealBase>(IEnumerator<TCloaked?>? value
      , PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TCloaked, TRevealBase>((IEnumerator<TCloaked>?, PalantírReveal<TRevealBase>) valueTuple)
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>(T[]? value, PalantírReveal<TBase> palantírReveal) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>((T[]?, PalantírReveal<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>(IReadOnlyList<T>? value, PalantírReveal<TBase> palantírReveal) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>((IReadOnlyList<T>?, PalantírReveal<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T>((IEnumerable<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase1, TBase2>(T[]? value
      , OrderedCollectionPredicate<TBase1> filter, PalantírReveal<TBase2> palantírReveal)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, PalantírReveal<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, PalantírReveal<TBase2> palantírReveal)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, PalantírReveal<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T, TBase>(IEnumerable<T>? value, PalantírReveal<TBase> palantírReveal) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T, TBase>((IEnumerable<T>?, PalantírReveal<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T, TBase>(IEnumerator<T>? value, PalantírReveal<TBase> palantírReveal) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T, TBase>((IEnumerator<T>?, PalantírReveal<TBase>) valueTuple) where T : class, TBase where TBase : class;
}
