using System.Numerics;
using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public interface IFLogAdditionalFormatterParameterEntry : IFLogFormatterParameterEntry
{
    // ReSharper disable UnusedMember.Global

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndMatch<T>(T value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(bool value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(bool? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TStruct>(TStruct value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TStruct>((TStruct, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TStruct>(TStruct? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TStruct>((TStruct?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ReadOnlySpan<char> value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(string? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((string?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((string?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(char[]? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((char[]?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(char[]? value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(ICharSequence? value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(StringBuilder? value, int fromIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(IStyledToStringObject? value);

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And(object? value);
    

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(
      (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>((IReadOnlyList<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
      where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(
      (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(
      (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(
      (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(TStruct[]? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(
      (TStruct[]?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(
      (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(T[]? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(
      (IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(
      (IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TFmtStruct>
      (IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple) 
      where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>(IEnumerable<TStruct>? value
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>((IEnumerable<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
      where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>(IEnumerator<TStruct>? value
      , CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TStruct>((IEnumerator<TStruct>? , CustomTypeStyler<TStruct>) valueTuple)
      where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T>(IEnumerable<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T>((IEnumerable<T>?, string?) valueTuple)
      where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T>(IEnumerator<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T>((IEnumerator<T>?, string?) valueTuple)
      where T : class;
    
    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>((TFmtStruct[]?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    public IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>((TStruct[]?, CustomTypeStyler<TStruct>) valueTuple)
      where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler)
      where TStruct : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
    , string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
    , string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
    , string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
    , string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
    , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate, CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
    , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
      (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple) where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
      (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple) where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
    , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple) where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple) where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
      (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;


    void AndFinalMatchParam<T>(T value);
    void AndFinalParam(bool value);
    void AndFinalParam(bool? value);
    void AndFinalParam<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;
    void AndFinalParam<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;
    void AndFinalParam<TStruct>(TStruct value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;
    void AndFinalParam<TStruct>((TStruct, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;
    void AndFinalParam<TStruct>(TStruct? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;
    void AndFinalParam<TStruct>((TStruct?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;
    void AndFinalParam(ReadOnlySpan<char> value);
    void AndFinalParam(ReadOnlySpan<char> value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(string? value);
    void AndFinalParam((string?, int) valueTuple);
    void AndFinalParam((string?, int, int) valueTuple);
    void AndFinalParam(string? value, int startIndex, int count = int.MaxValue);
    void AndFinalParam(char[]? value);
    void AndFinalParam((char[]?, int) valueTuple);
    void AndFinalParam((char[]?, int, int) valueTuple);
    void AndFinalParam(char[]? value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(ICharSequence? value);
    void AndFinalParam((ICharSequence?, int) valueTuple);
    void AndFinalParam((ICharSequence?, int, int) valueTuple);
    void AndFinalParam(ICharSequence? value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(StringBuilder? value);
    void AndFinalParam((StringBuilder?, int) valueTuple);
    void AndFinalParam((StringBuilder?, int, int) valueTuple);
    void AndFinalParam(StringBuilder? value, int fromIndex, int count = int.MaxValue);
    void AndFinalParam(IStyledToStringObject? value);
    void AndFinalParam(object? value);


    void AndFinalValueCollectionParam<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;
    
    void AndFinalValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
      where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;
    
    void AndFinalValueCollectionParam<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;
    
    void AndFinalValueCollectionParam<TStruct>((TStruct[]?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    void AndFinalValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;
    
    void AndFinalValueCollectionParam<TStruct>((IReadOnlyList<TStruct>?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    void AndFinalValueCollectionParam<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;
    
    void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?
    , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;
    
    void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?
    , OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?
    , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?
    , OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParam<TStruct>(TStruct[]? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;
    
    void AndFinalValueCollectionParam<TStruct>(
      (TStruct[]? , OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    void AndFinalValueCollectionParam<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    void AndFinalValueCollectionParam<TStruct>(
      (IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    void AndFinalObjectCollectionParam<T>(T[]? value, string? formatString = null) where T : class;
    
    void AndFinalObjectCollectionParam<T>((T[]?, string?) valueTuple) where T : class;

    void AndFinalObjectCollectionParam<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;
    
    void AndFinalObjectCollectionParam<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    void AndFinalObjectCollectionParam<T>(T[]? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    void AndFinalObjectCollectionParam<T>((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    void AndFinalObjectCollectionParam<T>((T[]?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    void AndFinalObjectCollectionParam<T>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    void AndFinalObjectCollectionParam<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    void AndFinalObjectCollectionParam<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    void AndFinalValueCollectionParamEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParamEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParamEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParamEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParamEnumerate<TStruct>(IEnumerable<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    void AndFinalValueCollectionParamEnumerate<TStruct>((IEnumerator<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
      where TStruct : struct;

    void AndFinalValueCollectionParamEnumerate<TStruct>(IEnumerator<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    void AndFinalObjectCollectionParamEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class;

    void AndFinalObjectCollectionParamEnumerate<T>((IEnumerable<T>?, string?) valueTuple) where T : class;

    void AndFinalObjectCollectionParamEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class;

    void AndFinalObjectCollectionParamEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class;

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple);
    
    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple);
    
    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);
    
    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple);

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);
    
    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);
    
    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
    , CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
    , CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>
    , CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>
    , CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;
    
    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
    , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple) where TValue : struct;
    
    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple) where TValue : struct;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
    , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null) where TValue : struct;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple) where TValue : struct;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple) where TValue : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
    , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?
    , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>
    , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>
    , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    void AndFinalKeyedCollectionParam<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>
    , CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;


    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalMatchParamToStringAppender<T>(T? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(bool? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(bool value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TNum>(TNum value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TNum>(TNum? value) where TNum : struct, INumber<TNum>;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TStruct>(TStruct value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TStruct>((TStruct, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TStruct>(TStruct? value, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender<TStruct>((TStruct?, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(string? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender((string?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender((string?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(char[]? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender((char[]?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(char[]? value, int fromIndex, int count);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ICharSequence? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(StringBuilder? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(IStyledToStringObject? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalParamToStringAppender(object? value);


    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>(TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;
    
    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>((TFmtStruct[]?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;
    
    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TStruct>(TStruct[]? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TStruct>((TStruct[]?, CustomTypeStyler<TStruct>) valueTuple)
      where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TStruct>(IReadOnlyList<TStruct>? value, CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TStruct>((IReadOnlyList<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
      where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>((TFmtStruct[]?
    , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>((TFmtStruct[]?
    , OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>((IReadOnlyList<TFmtStruct>?
    , OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TFmtStruct>((IReadOnlyList<TFmtStruct>?
    , OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TStruct>(TStruct[]? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TStruct>((TStruct[]?
    , OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TStruct>(IReadOnlyList<TStruct>? value
      , OrderedCollectionPredicate<TStruct> filter, CustomTypeStyler<TStruct> customTypeStyler) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TStruct>((IReadOnlyList<TStruct>?
    , OrderedCollectionPredicate<TStruct>, CustomTypeStyler<TStruct>) valueTuple) where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>(T[]? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>((T[]?
    , OrderedCollectionPredicate<T> , string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>((T[]?
    , OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?
    , OrderedCollectionPredicate<T>, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TStruct>(IEnumerable<TStruct>? value
      , CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TStruct>((IEnumerable<TStruct>?, CustomTypeStyler<TStruct>) valueTuple)
      where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TStruct>(IEnumerator<TStruct>? value
      , CustomTypeStyler<TStruct> customTypeStyler)
        where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TStruct>((IEnumerator<TStruct>?
    , CustomTypeStyler<TStruct>) valueTuple)
      where TStruct : struct;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamEnumerateToStringAppender<T>(IEnumerable<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamEnumerateToStringAppender<T>((IEnumerable<T>?, string?) valueTuple)
      where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamEnumerateToStringAppender<T>(IEnumerator<T>? value, string? formatString = null)
        where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamEnumerateToStringAppender<T>((IEnumerator<T>?, string?) valueTuple)
      where T : class;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?
    , string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((KeyValuePair<TKey, TValue>[]?
    , string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?
    , string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?
    , string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string? , string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKey, TValue>, string?) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKey, TValue>) valueTuple);

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?
    , KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?
    , KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString = null)
        where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple)
      where TValue : struct;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, string? keyFormatString)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, string?) valueTuple)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>) valueTuple)
        where TValue : struct where TKey : struct;
    
    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
    , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
      (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;
    
    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
    , KeyValuePredicate<TKey, TValue> filterPredicate
    , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKey, TValue> filterPredicate
      , CustomTypeStyler<TValue> valueStyler, CustomTypeStyler<TKey> keyStyler)
        where TValue : struct where TKey : struct;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, CustomTypeStyler<TValue>, CustomTypeStyler<TKey>) valueTuple)
      where TValue : struct where TKey : struct;

    // ReSharper restore UnusedMember.Global
}
