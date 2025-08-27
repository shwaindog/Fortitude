using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;

public interface ICollectionAppendContinuation<out TToReturn>
{
    [MustUseReturnValue("Use Final*, AndFinal* or WithOnly* to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TToStyle, TStylerType>(TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TToStyle, TToStyleBase, TStylerType>(
        (TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    TToReturn Add<TToStyle, TToStyleBase, TStylerType>(
        (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TFmtStruct>
        (IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TToStyle, TStylerType>((IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>? , CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

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
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    TToReturn AddMatch<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    TToReturn AddMatchEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;
    
}
