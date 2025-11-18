// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public interface ICollectionFinalAppend
{
    void Add<TStyleObj>(TStyleObj[]? value) where TStyleObj : class, IStringBearer;
    void Add<TStyleObj>(IReadOnlyList<TStyleObj>? value) where TStyleObj : class, IStringBearer;
    void AddFormat<TFmt>(TFmt[]? value, string? formatString = null) where TFmt : ISpanFormattable;
    void AddFormat<TFmt>((TFmt[]?, string?) valueTuple) where TFmt : ISpanFormattable;
    void AddFormat<TFmt>(IReadOnlyList<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable;
    void AddFormat<TFmt>((IReadOnlyList<TFmt>?, string?) valueTuple) where TFmt : ISpanFormattable;
    
    void Add<TCloaked, TRevealBase>(TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;
    
    void Add<TCloaked, TRevealBase>((TCloaked?[]?, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;
    
    void Add<TCloaked, TRevealBase>(IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;
    
    void Add<TCloaked, TRevealBase>((IReadOnlyList<TCloaked?>?, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    void Add<TStyleObj>(TStyleObj[]? value, OrderedCollectionPredicate<TStyleObj> filter) where TStyleObj : class, IStringBearer;

    void Add<TStyleObj>(IReadOnlyList<TStyleObj>? value, OrderedCollectionPredicate<TStyleObj> filter) where TStyleObj : class, IStringBearer;

    void AddFormat<TFmt, TBase>(TFmt[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where TFmt : ISpanFormattable, TBase;

    void AddFormat<TFmt, TBase>((TFmt[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where TFmt : ISpanFormattable, TBase;
    void AddFormat<TFmt, TBase>((TFmt[]?, OrderedCollectionPredicate<TBase>) valueTuple) where TFmt : ISpanFormattable, TBase;

    void AddFormat<TFmt, TBase>(IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where TFmt : ISpanFormattable, TBase;

    void AddFormat<TFmt, TBase>((IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where TFmt : ISpanFormattable, TBase;

    void AddFormat<TFmt, TBase>((IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>) valueTuple) where TFmt : ISpanFormattable, TBase;

    void Add<TCloaked, TFilterBase, TRevealBase>(TCloaked?[]? value
      , OrderedCollectionPredicate<TFilterBase> filter, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull;

    void Add<TCloaked, TFilterBase, TRevealBase>(
        (TCloaked?[]?, OrderedCollectionPredicate<TFilterBase>, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull;

    void Add<TCloaked, TFilterBase, TRevealBase>(IReadOnlyList<TCloaked?>? value
      , OrderedCollectionPredicate<TFilterBase> filter, PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull;

    void Add<TCloaked, TFilterBase, TRevealBase>((IReadOnlyList<TCloaked>?, OrderedCollectionPredicate<TFilterBase>
      , PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull;

    void AddEnumerate<TStyleObj>(IEnumerable<TStyleObj>? value) where TStyleObj : class, IStringBearer;
    void AddEnumerate<TStyleObj>(IEnumerator<TStyleObj>? value) where TStyleObj : class, IStringBearer;

    void AddFormatEnumerate<TFmt>(IEnumerable<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable;
    void AddFormatEnumerate<TFmt>((IEnumerable<TFmt>?, string?) valueTuple) where TFmt : ISpanFormattable;
    void AddFormatEnumerate<TFmt>(IEnumerator<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable;
    void AddFormatEnumerate<TFmt>((IEnumerator<TFmt>?, string?) valueTuple) where TFmt : ISpanFormattable;

    void AddEnumerate<TCloaked, TRevealBase>(IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    void AddEnumerate<TCloaked, TRevealBase>((IEnumerable<TCloaked?>?, PalantírReveal<TRevealBase>) valueTuple) 
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    void AddEnumerate<TCloaked, TRevealBase>(IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    void AddEnumerate<TCloaked, TRevealBase>((IEnumerator<TCloaked>?, PalantírReveal<TRevealBase>) valueTuple)
        where TCloaked : TRevealBase
        where TRevealBase : notnull;

    void AddMatch<T>(T[]? value, string? formatString = null) where T : class;
    void AddMatch<T>((T[]?, string?) valueTuple) where T : class;
    void AddMatch<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;
    void AddMatch<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;
    void AddMatch<T, TBase>(T[]? value, PalantírReveal<TBase> palantírReveal) where T : class, TBase where TBase : class;
    void AddMatch<T, TBase>((T[]?, PalantírReveal<TBase>) valueTuple) where T : class, TBase where TBase : class;
    void AddMatch<T, TBase>(IReadOnlyList<T>? value, PalantírReveal<TBase> palantírReveal) where T : class, TBase where TBase : class;
    void AddMatch<T, TBase>((IReadOnlyList<T>?, PalantírReveal<TBase>) valueTuple) where T : class, TBase where TBase : class;

    void AddMatch<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    void AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class;
    void AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class;

    void AddMatch<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    void AddMatch<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class;
    void AddMatch<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class;
    void AddMatchEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class;
    void AddMatchEnumerate<T>((IEnumerable<T>?, string?) valueTuple) where T : class;
    void AddMatchEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class;
    void AddMatchEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class;

    void AddMatch<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> filter, PalantírReveal<TBase2> palantírReveal)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void AddMatch<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, PalantírReveal<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void AddMatch<T, TBase1, TBase2>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter, PalantírReveal<TBase2> palantírReveal)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void AddMatch<T, TBase1, TBase2>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, PalantírReveal<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void AddMatchEnumerate<T, TBase>(IEnumerable<T>? value, PalantírReveal<TBase> palantírReveal) where T : class, TBase where TBase : class;
    void AddMatchEnumerate<T, TBase>((IEnumerable<T>?, PalantírReveal<TBase>) valueTuple) where T : class, TBase where TBase : class;
    void AddMatchEnumerate<T, TBase>(IEnumerator<T>? value, PalantírReveal<TBase> palantírReveal) where T : class, TBase where TBase : class;
    void AddMatchEnumerate<T, TBase>((IEnumerator<T>?, PalantírReveal<TBase>) valueTuple) where T : class, TBase where TBase : class;
}
