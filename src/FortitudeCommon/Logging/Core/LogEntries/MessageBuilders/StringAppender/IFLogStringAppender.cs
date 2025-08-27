using System.Numerics;
using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public interface IFLogStringAppender : IFLogMessageBuilder
{
    // ReSharper disable UnusedMember.Global
    int Count { get; }

    string Indent { get; set; }
    
    StringBuildingStyle Style { get; }

    IFLogStringAppender IncrementIndent();

    IFLogStringAppender DecrementIndent();


    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender AppendMatch<T>(T value);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(bool value);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(bool? value);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(string? value);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append((string?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append((string?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(char[]? value);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append((char[]?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(ICharSequence? value);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append((ICharSequence?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(StringBuilder? value);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append((StringBuilder?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(IStyledToStringObject? value);

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append(object? value);


    [MustUseReturnValue("Use FinalAppendFormat to finish and send LogEntry")]
    IFLogStringAppender AppendFormat<TFmtStruct>(TFmtStruct value, string formatString) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendFormat to finish and send LogEntry")]
    IFLogStringAppender AppendFormat<TFmtStruct>((TFmtStruct, string) value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendFormat to finish and send LogEntry")]
    IFLogStringAppender AppendFormat<TFmtStruct>(TFmtStruct? value, string formatString) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendFormat to finish and send LogEntry")]
    IFLogStringAppender AppendFormat<TFmtStruct>((TFmtStruct?, string) value) where TFmtStruct : struct, ISpanFormattable;

    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TToStyle, TStylerType>(TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TToStyle, TToStyleBase, TStylerType>(
        (TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TToStyle, TToStyleBase, TStylerType>(
        (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TFmtStruct>
        (IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value
      , string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TToStyle, TStylerType>((IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>? , CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerate<T>(IEnumerable<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerate<T>(IEnumerator<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerate<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase1, TBase2>(T[]? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) 
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) 
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;
    
    
    [MustUseReturnValue("Use FinalMatchAppend to finish and send LogEntry")]
    IFLogStringAppender AppendMatchLine<T>(T value);

    IFLogStringAppender AppendLine();

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(bool value);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(bool? value);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(string? value);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine((string?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine((string?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(char[]? value);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine((char[]?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(ICharSequence? value);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine((ICharSequence?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(StringBuilder? value);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine((StringBuilder?, int) valueTuple);
    
    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(IStyledToStringObject? value);

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine(object? value);


    [MustUseReturnValue("Use FinalAppendFormat to finish and send LogEntry")]
    IFLogStringAppender AppendFormatLine<TFmtStruct>(TFmtStruct value, string formatString) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendFormat to finish and send LogEntry")]
    IFLogStringAppender AppendFormatLine<TFmtStruct>((TFmtStruct, string) value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendFormat to finish and send LogEntry")]
    IFLogStringAppender AppendFormatLine<TFmtStruct>(TFmtStruct? value, string formatString) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendFormat to finish and send LogEntry")]
    IFLogStringAppender AppendFormatLine<TFmtStruct>((TFmtStruct?, string) value) where TFmtStruct : struct, ISpanFormattable;

    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    public IFLogStringAppender AppendValueCollectionLine<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TToStyle, TStylerType>(TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TToStyle, TToStyleBase, TStylerType>(
        (TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TToStyle, TToStyleBase, TStylerType>(
        (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TFmtStruct>
        (IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TFmtStruct>(IEnumerator<TFmtStruct>? value
      , string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TToStyle, TStylerType>(IEnumerable<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TToStyle, TStylerType>((IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TToStyle, TStylerType>(IEnumerator<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TToStyle, TStylerType>((IEnumerator<TToStyle>? , CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerateLine<T>(IEnumerable<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerateLine<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerateLine<T>(IEnumerator<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerateLine<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase1, TBase2>(T[]? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerateLine<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerateLine<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerateLine<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionEnumerateLine<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?
      , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase ;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase ;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase ;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase ;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase ;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase ;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase ;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase ;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase ;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;
    
    void FinalMatchAppend<T>(T value);
    void FinalAppend(bool value);
    void FinalAppend(bool? value);
    void FinalAppend<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;
    void FinalAppend<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;
    void FinalAppend<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;
    void FinalAppend<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;
    void FinalAppend(ReadOnlySpan<char> value);
    void FinalAppend(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);
    void FinalAppend(string? value);
    void FinalAppend((string?, int) valueTuple);
    void FinalAppend((string?, int, int) valueTuple);
    void FinalAppend(string? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(char[]? value);
    void FinalAppend((char[]?, int) valueTuple);
    void FinalAppend((char[]?, int, int) valueTuple);
    void FinalAppend(char[]? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(ICharSequence? value);
    void FinalAppend((ICharSequence?, int) valueTuple);
    void FinalAppend((ICharSequence?, int, int) valueTuple);
    void FinalAppend(ICharSequence? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(StringBuilder? value);
    void FinalAppend((StringBuilder?, int) valueTuple);
    void FinalAppend((StringBuilder?, int, int) valueTuple);
    void FinalAppend(StringBuilder? value, int startIndex, int count = int.MaxValue);
    void FinalAppend(IStyledToStringObject? value);
    void FinalAppend(object? value);

    void FinalAppendNumberFormatted<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum>;
    void FinalAppendNumberFormatted<TNum>((TNum, string) value) where TNum : struct, INumber<TNum>;
    void FinalAppendNumberFormatted<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum>;
    void FinalAppendNumberFormatted<TNum>((TNum?, string) value) where TNum : struct, INumber<TNum>;
    
    
    public void FinalAppendValueCollection<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;
    public void FinalAppendValueCollection<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    public void FinalAppendValueCollection<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;
    void FinalAppendValueCollection<TToStyle, TStylerType>(TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;
    void FinalAppendValueCollection<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;
    void FinalAppendValueCollection<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;
    void FinalAppendValueCollection<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    void FinalAppendValueCollection<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollection<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollection<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollection<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollection<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollection<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    void FinalAppendValueCollection<TToStyle, TToStyleBase, TStylerType>(
        (TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType;

    void FinalAppendValueCollection<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    void FinalAppendValueCollection<TToStyle, TToStyleBase, TStylerType>(
        (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TToStyleBase, TStylerType;

    void FinalAppendValueCollectionEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollectionEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollectionEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollectionEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollectionEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType;

    void FinalAppendValueCollectionEnumerate<TToStyle, TStylerType>((IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    void FinalAppendValueCollectionEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType;

    void FinalAppendValueCollectionEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>? , CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType;

    void FinalAppendObjectCollection<T>(T[]? value, string? formatString = null) where T : class;
    void FinalAppendObjectCollection<T>((T[]?, string?) valueTuple) where T : class;
    void FinalAppendObjectCollection<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;
    void FinalAppendObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    void FinalAppendObjectCollection<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class;

    void FinalAppendObjectCollection<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
        where T : class, TBase where TBase : class;

    void FinalAppendObjectCollection<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    void FinalAppendObjectCollection<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class;

    void FinalAppendObjectCollection<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where T : class, TBase where TBase : class;

    void FinalAppendObjectCollection<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    void FinalAppendObjectCollectionEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class;
    void FinalAppendObjectCollectionEnumerate<T>((IEnumerable<T>?, string?) valueTuple) where T : class;
    void FinalAppendObjectCollectionEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class;
    void FinalAppendObjectCollectionEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class;

    void FinalAppendObjectCollection<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;

    void FinalAppendObjectCollection<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    void FinalAppendObjectCollection<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;

    void FinalAppendObjectCollection<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    void FinalAppendObjectCollection<T, TBase1, TBase2>(T[]? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void FinalAppendObjectCollection<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void FinalAppendObjectCollection<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void FinalAppendObjectCollection<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void FinalAppendObjectCollectionEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    void FinalAppendObjectCollectionEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    void FinalAppendObjectCollectionEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class;

    void FinalAppendObjectCollectionEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class;

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollection<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]? , string?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]? , string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollection<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , string?) valueTuple);

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , string?, string?) valueTuple);

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , string?) valueTuple);

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple)
        where TValue : TVBase;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>
      , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void FinalAppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;
    
    
    // ReSharper restore UnusedMember.Global
}
