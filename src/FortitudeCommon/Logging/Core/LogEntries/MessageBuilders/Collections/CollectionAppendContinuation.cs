// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public class CollectionAppendContinuation<TToReturn, TCallerType> : AddCollectionBase<TToReturn, TCallerType>
  , ICollectionAppendContinuation<TToReturn>
    where TCallerType : FLogEntryMessageBuilderBase<TToReturn, TCallerType>, TToReturn where TToReturn : class, IFLogMessageBuilder
{
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TStyleObj>(TStyleObj[]? value) where TStyleObj : class, IStringBearer =>
        AppendObjectCollection(PreappendCheckGetStringAppender(value), value).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TStyleObj>(IReadOnlyList<TStyleObj>? value) where TStyleObj : class, IStringBearer =>
        AppendObjectCollection(PreappendCheckGetStringAppender(value), value).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt>(TFmt[]? value, string? formatString = null) where TFmt : ISpanFormattable =>
        AppendValueCollection(PreappendCheckGetStringAppender(value), value, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt>((TFmt[]?, string?) valueTuple) where TFmt : ISpanFormattable =>
        AppendValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt>(IReadOnlyList<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable =>
        AppendValueCollection(PreappendCheckGetStringAppender(value), value, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt>((IReadOnlyList<TFmt>?, string?) valueTuple) where TFmt : ISpanFormattable =>
        AppendValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TToStyle, TStylerType>(TToStyle[]? value, StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TStylerType =>
        AppendValueCollection(PreappendCheckGetStringAppender(value), value, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TToStyle, TStylerType>((TToStyle[]?, StringBearerRevealState<TStylerType>) valueTuple) where TToStyle : TStylerType =>
        AppendValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType =>
        AppendValueCollection(PreappendCheckGetStringAppender(value), value, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, StringBearerRevealState<TStylerType>) valueTuple)
        where TToStyle : TStylerType =>
        AppendValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TStyleObj>(TStyleObj[]? value, OrderedCollectionPredicate<TStyleObj> filter)
        where TStyleObj : class, IStringBearer =>
        AppendFilteredObjectCollection(PreappendCheckGetStringAppender(value), value, filter).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TStyleObj>(IReadOnlyList<TStyleObj>? value, OrderedCollectionPredicate<TStyleObj> filter)
        where TStyleObj : class, IStringBearer =>
        AppendFilteredObjectCollection(PreappendCheckGetStringAppender(value), value, filter).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt, TBase>(TFmt[]? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where TFmt : ISpanFormattable, TBase =>
        AppendFilteredValueCollection(PreappendCheckGetStringAppender(value), value, filter, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt, TBase>((TFmt[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where TFmt : ISpanFormattable, TBase =>
        AppendFilteredValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt, TBase>(
        (TFmt[]?, OrderedCollectionPredicate<TBase>) valueTuple) where TFmt : ISpanFormattable, TBase =>
        AppendFilteredValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt, TBase>(IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where TFmt : ISpanFormattable, TBase =>
        AppendFilteredValueCollection(PreappendCheckGetStringAppender(value), value, filter, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt, TBase>(
        (IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where TFmt : ISpanFormattable, TBase =>
        AppendFilteredValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn AddFormat<TFmt, TBase>(
        (IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>) valueTuple) where TFmt : ISpanFormattable, TBase =>
        AppendFilteredValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TToStyleBase, TStylerType =>
        AppendFilteredValueCollection(PreappendCheckGetStringAppender(value), value, filter, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TToStyle, TToStyleBase, TStylerType>(
        (TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, StringBearerRevealState<TStylerType>) valueTuple)
        where TToStyle : TToStyleBase, TStylerType =>
        AppendFilteredValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, StringBearerRevealState<TStylerType> stringBearerRevealState) where TToStyle : TToStyleBase, TStylerType =>
        AppendFilteredValueCollection(PreappendCheckGetStringAppender(value), value, filter, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public TToReturn Add<TToStyle, TToStyleBase, TStylerType>(
        (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, StringBearerRevealState<TStylerType>) valueTuple)
        where TToStyle : TToStyleBase, TStylerType =>
        AppendFilteredValueCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddEnumerate<TStyleObj>(IEnumerable<TStyleObj>? value) where TStyleObj : class, IStringBearer =>
        AppendObjectCollectionEnumerate(PreappendCheckGetStringAppender(value), value).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddEnumerate<TStyleObj>(IEnumerator<TStyleObj>? value) where TStyleObj : class, IStringBearer =>
        AppendObjectCollectionEnumerate(PreappendCheckGetStringAppender(value), value).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddFormatEnumerate<TFmt>(IEnumerable<TFmt>? value
      , string? formatString = null) where TFmt : ISpanFormattable =>
        AppendValueCollectionEnumerate(PreappendCheckGetStringAppender(value), value, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddFormatEnumerate<TFmt>((IEnumerable<TFmt>?, string?) valueTuple)
        where TFmt : ISpanFormattable =>
        AppendValueCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddFormatEnumerate<TFmt>(IEnumerator<TFmt>? value, string? formatString = null)
        where TFmt : ISpanFormattable =>
        AppendValueCollectionEnumerate(PreappendCheckGetStringAppender(value), value, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddFormatEnumerate<TFmt>((IEnumerator<TFmt>?, string?) valueTuple) where TFmt : ISpanFormattable =>
        AppendValueCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType =>
        AppendValueCollectionEnumerate(PreappendCheckGetStringAppender(value), value, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddEnumerate<TToStyle, TStylerType>((IEnumerable<TToStyle>?, StringBearerRevealState<TStylerType>) valueTuple)
        where TToStyle : TStylerType =>
        AppendValueCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value, StringBearerRevealState<TStylerType> stringBearerRevealState)
        where TToStyle : TStylerType =>
        AppendValueCollectionEnumerate(PreappendCheckGetStringAppender(value), value, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public TToReturn AddEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>?, StringBearerRevealState<TStylerType>) valueTuple)
        where TToStyle : TStylerType =>
        AppendValueCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>(T[]? value, StringBearerRevealState<TBase> stringBearerRevealState) where T : class, TBase where TBase : class =>
        AppendObjectCollection(PreappendCheckGetStringAppender(value), value, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>((T[]?, StringBearerRevealState<TBase>) valueTuple) where T : class, TBase where TBase : class =>
        AppendObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>(IReadOnlyList<T>? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class =>
        AppendObjectCollection(PreappendCheckGetStringAppender(value), value, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>((IReadOnlyList<T>?, StringBearerRevealState<TBase>) valueTuple) where T : class, TBase where TBase : class =>
        AppendObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(PreappendCheckGetStringAppender(value), value, filter, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(PreappendCheckGetStringAppender(value), value, filter, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple)
        where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T>(T[]? value, string? formatString = null) where T : class =>
        AppendObjectCollection(PreappendCheckGetStringAppender(value), value, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T>((T[]?, string?) valueTuple) where T : class =>
        AppendObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class =>
        AppendObjectCollection(PreappendCheckGetStringAppender(value), value, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public TToReturn AddMatch<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class =>
        AppendObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public TToReturn AddMatchEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class =>
        AppendObjectCollectionEnumerate(PreappendCheckGetStringAppender(value), value, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public TToReturn AddMatchEnumerate<T>((IEnumerable<T>?, string?) valueTuple) where T : class =>
        AppendObjectCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public TToReturn AddMatchEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class =>
        AppendObjectCollectionEnumerate(PreappendCheckGetStringAppender(value), value, formatString).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public TToReturn AddMatchEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class =>
        AppendObjectCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    public TToReturn AddMatch<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> filter, StringBearerRevealState<TBase2> stringBearerRevealState)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class =>
        AppendFilteredObjectCollection(PreappendCheckGetStringAppender(value), value, filter, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    public TToReturn AddMatch<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>
      , StringBearerRevealState<TBase2>) valueTuple) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class =>
        AppendFilteredObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    public TToReturn AddMatch<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, StringBearerRevealState<TBase2> stringBearerRevealState)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class =>
        AppendFilteredObjectCollection(PreappendCheckGetStringAppender(value), value, filter, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    public TToReturn AddMatch<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, StringBearerRevealState<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class =>
        AppendFilteredObjectCollection(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    public TToReturn AddMatchEnumerate<T, TBase>(IEnumerable<T>? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class =>
        AppendObjectCollectionEnumerate(PreappendCheckGetStringAppender(value), value, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    public TToReturn AddMatchEnumerate<T, TBase>((IEnumerable<T>?, StringBearerRevealState<TBase>) valueTuple)
        where T : class, TBase where TBase : class =>
        AppendObjectCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    public TToReturn AddMatchEnumerate<T, TBase>(IEnumerator<T>? value, StringBearerRevealState<TBase> stringBearerRevealState)
        where T : class, TBase where TBase : class =>
        AppendObjectCollectionEnumerate(PreappendCheckGetStringAppender(value), value, stringBearerRevealState).PostAppendCheckAndReturn(value, this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    public TToReturn AddMatchEnumerate<T, TBase>((IEnumerator<T>?, StringBearerRevealState<TBase>) valueTuple)
        where T : class, TBase where TBase : class =>
        AppendObjectCollectionEnumerate(valueTuple, PreappendCheckGetStringAppender(valueTuple)).PostAppendCheckAndReturn(valueTuple, this);
}
