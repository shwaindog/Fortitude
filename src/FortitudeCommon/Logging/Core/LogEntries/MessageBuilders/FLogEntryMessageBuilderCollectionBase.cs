// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder
{
    protected ITheOneString? AppendValueCollection<TFmt>
        (ITheOneString? toAppendTo, TFmt[]? value, string? formatString = null)
        where TFmt : ISpanFormattable
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollection<TFmt>
        ((TFmt[]?, string?) valueTuple, ITheOneString? appender)
        where TFmt : ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendValueCollection<TFmt>
        (IReadOnlyList<TFmt>? value, ITheOneString? appender)
        where TFmt : ISpanFormattable
    {
        appender?.StartSimpleCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollection<TFmt>
        (ITheOneString? toAppendTo, IReadOnlyList<TFmt>? value, string? formatString = null)
        where TFmt : ISpanFormattable
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollection<TFmt>
        ((IReadOnlyList<TFmt>?, string?) valueTuple, ITheOneString? appender)
        where TFmt : ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollection<TCloaked, TRevealBase>
        (ITheOneString? toAppendTo, TCloaked?[]? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealAll(value, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollection<TCloaked, TRevealBase>
        ((TCloaked?[]?, PalantírReveal<TRevealBase>) valueTuple, ITheOneString? appender)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealAll(value, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollection<TCloaked, TRevealBase>
        (ITheOneString? toAppendTo, IReadOnlyList<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealAll(value, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollection<TCloaked, TRevealBase>
        ((IReadOnlyList<TCloaked?>?, PalantírReveal<TRevealBase>) valueTuple, ITheOneString? appender)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealAll(value, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredValueCollection<TFmt, TBase>
    (ITheOneString? toAppendTo, TFmt[]? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where TFmt : ISpanFormattable, TBase
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredValueCollection<TFmt, TBase>
        ((TFmt[]?, OrderedCollectionPredicate<TFmt>, string?) valueTuple, ITheOneString? appender)
        where TFmt : ISpanFormattable, TBase
    {
        var (value, filter, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, formatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredValueCollection<TFmt, TBase>
        ((TFmt[]?, OrderedCollectionPredicate<TBase>) valueTuple, ITheOneString? appender)
        where TFmt : ISpanFormattable, TBase
    {
        var (value, filter) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredValueCollection<TFmt, TBase>
    (ITheOneString? toAppendTo, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where TFmt : ISpanFormattable, TBase
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredValueCollection<TFmt, TBase>
        ((IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>, string?) valueTuple, ITheOneString? appender)
        where TFmt : ISpanFormattable, TBase
    {
        var (value, filter, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, formatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredValueCollection<TFmt, TBase>
        ((IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>) valueTuple, ITheOneString? appender)
        where TFmt : ISpanFormattable, TBase
    {
        var (value, filter) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredValueCollection<TCloaked, TFilterBase, TRevealBase>
    (ITheOneString? toAppendTo, TCloaked?[]? value, OrderedCollectionPredicate<TFilterBase> filter
      , PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealFiltered(value, filter, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredValueCollection<TCloaked, TFilterBase, TRevealBase>
        ((TCloaked?[]?, OrderedCollectionPredicate<TFilterBase>, PalantírReveal<TRevealBase>) valueTuple, ITheOneString? appender)
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull
    {
        var (value, filter, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealFiltered(value, filter, structStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredValueCollection<TCloaked, TFilterBase, TRevealBase>
    (ITheOneString? toAppendTo, IReadOnlyList<TCloaked?>? value, OrderedCollectionPredicate<TFilterBase> filter
      , PalantírReveal<TRevealBase> palantírReveal) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealFiltered(value, filter, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredValueCollection<TCloaked, TFilterBase, TRevealBase>
    ((IReadOnlyList<TCloaked>?, OrderedCollectionPredicate<TFilterBase>, PalantírReveal<TRevealBase>) valueTuple
      , ITheOneString? appender) 
        where TCloaked : TFilterBase, TRevealBase
        where TRevealBase : notnull
    {
        var (value, filter, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealFiltered(value, filter, structStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollectionEnumerate<TFmt>
        (ITheOneString? toAppendTo, IEnumerable<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollectionEnumerate<TFmt>
        (IEnumerable<TFmt>? value, ITheOneString? appender)
        where TFmt : ISpanFormattable
    {
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }

    protected static ITheOneString? AppendValueCollectionEnumerate<TFmt>
        ((IEnumerable<TFmt>?, string?) valueTuple, ITheOneString? appender)
        where TFmt : ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollectionEnumerate<TFmt>
        (ITheOneString? toAppendTo, IEnumerator<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, null, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollectionEnumerate<TFmt>
        (IEnumerator<TFmt>? value, ITheOneString? appender)
        where TFmt : ISpanFormattable
    {
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }

    protected static ITheOneString? AppendValueCollectionEnumerate<TFmt>
        ((IEnumerator<TFmt>?, string?) valueTuple, ITheOneString? appender)
        where TFmt : ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, null, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollectionEnumerate<TCloaked, TRevealBase>
        (ITheOneString? toAppendTo, IEnumerable<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealAllEnumerate(value, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollectionEnumerate<TCloaked, TRevealBase>
        ((IEnumerable<TCloaked?>?, PalantírReveal<TRevealBase>) valueTuple, ITheOneString? appender)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var (value, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealAllEnumerate(value, structStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollectionEnumerate<TCloaked, TRevealBase>
        (ITheOneString? toAppendTo, IEnumerator<TCloaked?>? value, PalantírReveal<TRevealBase> palantírReveal)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealAllEnumerate(value, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollectionEnumerate<TCloaked, TRevealBase>
        ((IEnumerator<TCloaked>?, PalantírReveal<TRevealBase>) valueTuple, ITheOneString? appender)
        where TCloaked : TRevealBase
        where TRevealBase : notnull
    {
        var (value, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealAllEnumerate(value, structStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendObjectCollection<T>
        (ITheOneString? toAppendTo, T[]? value, string? formatString = null)
        where T : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllMatch(value, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollection<T>(T[]? value, ITheOneString? appender)
        where T : class
    {
        appender?.StartSimpleCollectionType("")
                .AddAllMatch(value).Complete();
        return appender;
    }

    protected static ITheOneString? AppendObjectCollection<T>((T[]?, string?) valueTuple, ITheOneString? appender)
        where T : class
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllMatch(value, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendObjectCollection<T>
        (ITheOneString? toAppendTo, IReadOnlyList<T>? value, string? formatString = null) where T : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllMatch(value, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple
      , ITheOneString? appender)
        where T : class
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllMatch(value, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredObjectCollection<T, TBase>
        (ITheOneString? toAppendTo, T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFilteredMatch(value, filter, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredObjectCollection<T, TBase>
        ((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, filter, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter, formatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredObjectCollection<T, TBase>
        ((T[]?, OrderedCollectionPredicate<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, filter) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredObjectCollection<T, TBase>
    (ITheOneString? toAppendTo, IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFilteredMatch(value, filter, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredObjectCollection<T, TBase>
        ((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, filter, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter, formatString).Complete();
        return appender;
    }

    protected static ITheOneString? AppendFilteredObjectCollection<T, TBase>
        ((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, filter) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter).Complete();
        return appender;
    }

    protected ITheOneString? AppendObjectCollectionEnumerate<T>
        (ITheOneString? toAppendTo, IEnumerable<T>? value, string? formatString = null) where T : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllMatchEnumerate(value, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollectionEnumerate<T>(IEnumerable<T>? value, ITheOneString? appender)
        where T : class
    {
        appender?.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value).Complete();
        return appender;
    }

    protected static ITheOneString? AppendObjectCollectionEnumerate<T>((IEnumerable<T>?, string?) valueTuple
      , ITheOneString? appender)
        where T : class
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value, formatString).Complete();
        return appender;
    }

    public ITheOneString? AppendObjectCollectionEnumerate<T>
        (ITheOneString? toAppendTo, IEnumerator<T>? value, string? formatString = null)
        where T : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllMatchEnumerate(value, null, formatString).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollectionEnumerate<T>
        (IEnumerator<T>? value, ITheOneString? appender)
        where T : class
    {
        appender?.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value).Complete();
        return appender;
    }

    protected static ITheOneString? AppendObjectCollectionEnumerate<T>
        ((IEnumerator<T>?, string?) valueTuple, ITheOneString? appender)
        where T : class
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value, null, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendObjectCollection<T, TBase>
        (ITheOneString? toAppendTo, T?[]? value, PalantírReveal<TBase> palantírReveal)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealAll(value, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollection<T, TBase>
        ((T[]?, PalantírReveal<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealAll(value, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendObjectCollection<T, TBase>
        (ITheOneString? toAppendTo, IReadOnlyList<T>? value, PalantírReveal<TBase> palantírReveal)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealAll(value, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollection<T, TBase>
        ((IReadOnlyList<T>?, PalantírReveal<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealAll(value, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredObjectCollection<T, TBase1, TBase2>
        (ITheOneString? toAppendTo, T[]? value, OrderedCollectionPredicate<TBase1> filter, PalantírReveal<TBase2> palantírReveal)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealFiltered(value, filter, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredObjectCollection<T, TBase1, TBase2>
        ((T[]?, OrderedCollectionPredicate<TBase1>, PalantírReveal<TBase2>) valueTuple, ITheOneString? appender)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        var (value, filter, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealFiltered(value, filter, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredObjectCollection<T, TBase1, TBase2>
    (ITheOneString? toAppendTo, IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
      , PalantírReveal<TBase2> palantírReveal) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealFiltered(value, filter, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredObjectCollection<T, TBase1, TBase2>
        ((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, PalantírReveal<TBase2>) valueTuple, ITheOneString? appender)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        var (value, filter, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealFiltered(value, filter, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendObjectCollectionEnumerate<T, TBase>
        (ITheOneString? toAppendTo, IEnumerable<T>? value, PalantírReveal<TBase> palantírReveal)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealAllEnumerate(value, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollectionEnumerate<T, TBase>
        ((IEnumerable<T>?, PalantírReveal<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealAllEnumerate(value, customTypeStyler).Complete();
        return appender;
    }

    public ITheOneString? AppendObjectCollectionEnumerate<T, TBase>
        (ITheOneString? toAppendTo, IEnumerator<T>? value, PalantírReveal<TBase> palantírReveal)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .RevealAllEnumerate(value, palantírReveal).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollectionEnumerate<T, TBase>
        ((IEnumerator<T>?, PalantírReveal<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .RevealAllEnumerate(value, customTypeStyler).Complete();
        return appender;
    }
}
