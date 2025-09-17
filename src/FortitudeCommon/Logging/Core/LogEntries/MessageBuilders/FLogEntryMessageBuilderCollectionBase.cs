// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

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

    protected ITheOneString? AppendValueCollection<TToStyle, TStylerType>
        (ITheOneString? toAppendTo, TToStyle[]? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollection<TToStyle, TStylerType>
        ((TToStyle[]?, StringBearerRevealState<TStylerType>) valueTuple, ITheOneString? appender)
        where TToStyle : TStylerType
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollection<TToStyle, TStylerType>
        (ITheOneString? toAppendTo, IReadOnlyList<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollection<TToStyle, TStylerType>
        ((IReadOnlyList<TToStyle>?, StringBearerRevealState<TStylerType>) valueTuple, ITheOneString? appender)
        where TToStyle : TStylerType
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
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

    protected ITheOneString? AppendFilteredValueCollection<TToStyle, TToStyleBase, TStylerType>
    (ITheOneString? toAppendTo, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filter
      , StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TToStyleBase, TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredValueCollection<TToStyle, TToStyleBase, TStylerType>
        ((TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, StringBearerRevealState<TStylerType>) valueTuple, ITheOneString? appender)
        where TToStyle : TToStyleBase, TStylerType
    {
        var (value, filter, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, structStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredValueCollection<TToStyle, TToStyleBase, TStylerType>
    (ITheOneString? toAppendTo, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filter
      , StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TToStyleBase, TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredValueCollection<TToStyle, TToStyleBase, TStylerType>
    ((IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, StringBearerRevealState<TStylerType>) valueTuple
      , ITheOneString? appender) where TToStyle : TToStyleBase, TStylerType
    {
        var (value, filter, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, structStyler).Complete();
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
                  .AddAllEnumerate(value, formatString).Complete();
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
                .AddAllEnumerate(value, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollectionEnumerate<TToStyle, TStylerType>
        (ITheOneString? toAppendTo, IEnumerable<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollectionEnumerate<TToStyle, TStylerType>
        ((IEnumerable<TToStyle>?, StringBearerRevealState<TStylerType>) valueTuple, ITheOneString? appender)
        where TToStyle : TStylerType
    {
        var (value, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, structStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendValueCollectionEnumerate<TToStyle, TStylerType>
        (ITheOneString? toAppendTo, IEnumerator<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendValueCollectionEnumerate<TToStyle, TStylerType>
        ((IEnumerator<TToStyle>?, StringBearerRevealState<TStylerType>) valueTuple, ITheOneString? appender)
        where TToStyle : TStylerType
    {
        var (value, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, structStyler).Complete();
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
                  .AddAllMatchEnumerate(value, formatString).Complete();
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
                .AddAllMatchEnumerate(value, formatString).Complete();
        return appender;
    }

    protected ITheOneString? AppendObjectCollection<T, TBase>
        (ITheOneString? toAppendTo, T?[]? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollection<T, TBase>
        ((T[]?, StringBearerRevealState<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendObjectCollection<T, TBase>
        (ITheOneString? toAppendTo, IReadOnlyList<T>? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollection<T, TBase>
        ((IReadOnlyList<T>?, StringBearerRevealState<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredObjectCollection<T, TBase1, TBase2>
        (ITheOneString? toAppendTo, T[]? value, OrderedCollectionPredicate<TBase1> filter, StringBearerRevealState<TBase2> stringBearerRevealState)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredObjectCollection<T, TBase1, TBase2>
        ((T[]?, OrderedCollectionPredicate<TBase1>, StringBearerRevealState<TBase2>) valueTuple, ITheOneString? appender)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        var (value, filter, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendFilteredObjectCollection<T, TBase1, TBase2>
    (ITheOneString? toAppendTo, IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
      , StringBearerRevealState<TBase2> stringBearerRevealState) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendFilteredObjectCollection<T, TBase1, TBase2>
        ((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, StringBearerRevealState<TBase2>) valueTuple, ITheOneString? appender)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        var (value, filter, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, customTypeStyler).Complete();
        return appender;
    }

    protected ITheOneString? AppendObjectCollectionEnumerate<T, TBase>
        (ITheOneString? toAppendTo, IEnumerable<T>? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollectionEnumerate<T, TBase>
        ((IEnumerable<T>?, StringBearerRevealState<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, customTypeStyler).Complete();
        return appender;
    }

    public ITheOneString? AppendObjectCollectionEnumerate<T, TBase>
        (ITheOneString? toAppendTo, IEnumerator<T>? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, stringBearerRevealState).Complete();
        return toAppendTo;
    }

    protected static ITheOneString? AppendObjectCollectionEnumerate<T, TBase>
        ((IEnumerator<T>?, StringBearerRevealState<TBase>) valueTuple, ITheOneString? appender)
        where T : class, TBase where TBase : class
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, customTypeStyler).Complete();
        return appender;
    }
}
