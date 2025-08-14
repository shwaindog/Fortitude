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
    IFLogStringAppender Append<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    
    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;

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
    IFLogStringAppender AppendValueCollection<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

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
    IFLogStringAppender AppendValueCollection<TStruct>(TStruct[]? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TStruct>(
        (TStruct[]?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollection<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>(T[]? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollection<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

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
    IFLogStringAppender AppendValueCollectionEnumerate<TStruct>((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TStruct>(IEnumerator<TStruct>? value
      , StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerate<TStruct>((IEnumerator<TStruct>? , StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

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
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;
    
    
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
    IFLogStringAppender AppendLine<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;

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
    IFLogStringAppender AppendValueCollectionLine<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

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
    IFLogStringAppender AppendValueCollectionLine<TStruct>(TStruct[]? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TStruct>(
        (TStruct[]?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollection to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionLine<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>(T[]? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    IFLogStringAppender AppendObjectCollectionLine<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

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
    IFLogStringAppender AppendValueCollectionEnumerateLine<TStruct>((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TStruct>(IEnumerator<TStruct>? value
      , StructStyler<TStruct> structStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendValueCollectionEnumerateLine<TStruct>((IEnumerator<TStruct>? , StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

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
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionEnumerateLine<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish and send LogEntry")]
    IFLogStringAppender AppendKeyedCollectionLine<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;
    
    void FinalMatchAppend<T>(T value);
    void FinalAppend(bool value);
    void FinalAppend(bool? value);
    void FinalAppend<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;
    void FinalAppend<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;
    void FinalAppend<TStruct>(TStruct value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void FinalAppend<TStruct>((TStruct, StructStyler<TStruct>) valueTuple) where TStruct : struct;
    void FinalAppend<TStruct>(TStruct? value, StructStyler<TStruct> structStyler) where TStruct : struct;
    void FinalAppend<TStruct>((TStruct?, StructStyler<TStruct>) valueTuple) where TStruct : struct;
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

    public void FinalAppendValueCollection<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollection<TStruct>(TStruct[]? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    void FinalAppendValueCollection<TStruct>((TStruct[]?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    void FinalAppendValueCollection<TStruct>(IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler)
        where TStruct : struct;

    void FinalAppendValueCollection<TStruct>((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

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

    void FinalAppendValueCollection<TStruct>(TStruct[]? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    void FinalAppendValueCollection<TStruct>(
        (TStruct[]?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    void FinalAppendValueCollection<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, StructStyler<TStruct> structStyler) where TStruct : struct;

    void FinalAppendValueCollection<TStruct>(
        (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple) where TStruct : struct;

    void FinalAppendObjectCollection<T>(T[]? value, string? formatString = null) where T : class;

    void FinalAppendObjectCollection<T>((T[]?, string?) valueTuple) where T : class;

    void FinalAppendObjectCollection<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    void FinalAppendObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    void FinalAppendObjectCollection<T>(T[]? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    void FinalAppendObjectCollection<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    void FinalAppendObjectCollection<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    void FinalAppendObjectCollection<T>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    void FinalAppendObjectCollection<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    void FinalAppendObjectCollection<T>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    void FinalAppendValueCollectionEnumerate<TFmtStruct>
    (IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollectionEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollectionEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value
      , string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollectionEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable;

    void FinalAppendValueCollectionEnumerate<TStruct>((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    void FinalAppendValueCollectionEnumerate<TStruct>(IEnumerator<TStruct>? value
      , StructStyler<TStruct> structStyler)
        where TStruct : struct;

    void FinalAppendValueCollectionEnumerate<TStruct>((IEnumerator<TStruct>? , StructStyler<TStruct>) valueTuple)
        where TStruct : struct;

    void FinalAppendObjectCollectionEnumerate<T>(IEnumerable<T>? value, string? formatString = null)
        where T : class;

    void FinalAppendObjectCollectionEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
        where T : class;

    void FinalAppendObjectCollectionEnumerate<T>(IEnumerator<T>? value, string? formatString = null)
        where T : class;

    void FinalAppendObjectCollectionEnumerate<T>((IEnumerator<T>?, string?) valueTuple)
        where T : class;

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , string?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
      , string?) valueTuple);

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

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]? , KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple)
        where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple)
        where TValue : struct;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple) where TValue : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler)
        where TValue : struct where TKey : struct;

    void FinalAppendKeyedCollection<TKey, TValue>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple)
        where TValue : struct where TKey : struct;
    
    
    // ReSharper restore UnusedMember.Global
}
