using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
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
    IFLogAdditionalFormatterParameterEntry? And<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) 
      where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? And<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) 
      where TToStyle : TStylerType;

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
    
    IFLogAdditionalParamCollectionAppend AndCollection
    {
      [MustUseReturnValue("Use Add* to add a collection to the format parameters")] get;
    }
    
    
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

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?
    , CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TToStyle, TStylerType>(TToStyle[]? value
    , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple)
      where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value
    , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TToStyle, TStylerType>((IEnumerable<TToStyle>?
    , CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollectionEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>? 
    , CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(TFmtStruct[]? value
      , OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TFmtStruct>(
      (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;

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
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TToStyle, TToStyleBase, TStylerType>(
      (TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use AndFinalValueCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndValueCollection<TToStyle, TToStyleBase, TStylerType>(
      (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) 
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(
      (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(
      (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple)
      where T : class, TBase where TBase : class;

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
    
    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) 
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) 
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase1, TBase2>(T[]? value
    , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase1, TBase2>((T[]?
    , OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple)
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam to finish LogEntry")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase1, TBase2>(IReadOnlyList<T>? value
    , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler)
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollection<T, TBase1, TBase2>(
      (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndObjectCollectionEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
      where T : class, TBase where TBase : class;

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
    
    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
    , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(
      (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(
      (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
    , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TVBase>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
      (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionEnumerate if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) 
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple)
      where TKey : TKBase  where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
      where TKey : TKBase  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
      where TKey : TKBase  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
      where TKey : TKBase  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
      where TKey : TKBase  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
      where TKey : TKBase  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
      where TKey : TKBase  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
      IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
    , string? keyFormatString = null) where TKey : TKBase  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
      where TKey : TKBase  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
      where TKey : TKBase  where TValue : TVBase1, TVBase2;
    
    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
      IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
    , CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
      where TKey : TKBase1, TKBase2  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
      where TKey : TKBase1, TKBase2  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
      where TKey : TKBase1, TKBase2  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
      IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler
    , CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2  where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollection if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? AndKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>
      , CustomTypeStyler<TKBase2>) valueTuple) where TKey : TKBase1, TKBase2  where TValue : TVBase1, TVBase2;


    void AndFinalMatchParam<T>(T value);
    void AndFinalParam(bool value);
    void AndFinalParam(bool? value);
    void AndFinalParam<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;
    void AndFinalParam<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;
    void AndFinalParam<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;
    void AndFinalParam<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;
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

    IFinalCollectionAppend AndFinalCollectionParam
    {
      [MustUseReturnValue("Use Add* to add a collection to the format parameters")] get;
    }

    void AndFinalValueCollectionParam<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParam<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;
    
    void AndFinalValueCollectionParam<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
      where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParam<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable;
    
    void AndFinalValueCollectionParam<TToStyle, TStylerType>(TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;
    
    void AndFinalValueCollectionParam<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    void AndFinalValueCollectionParam<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;
    
    void AndFinalValueCollectionParam<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

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

    void AndFinalValueCollectionParam<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;
    
    void AndFinalValueCollectionParam<TToStyle, TToStyleBase, TStylerType>(
      (TToStyle[]? , OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType;

    void AndFinalValueCollectionParam<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    void AndFinalValueCollectionParam<TToStyle, TToStyleBase, TStylerType>(
      (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) 
      where TToStyle : TToStyleBase, TStylerType;

    void AndFinalValueCollectionParamEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null)
      where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParamEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParamEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null)
      where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParamEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
      where TFmtStruct : struct, ISpanFormattable;

    void AndFinalValueCollectionParamEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
      where TToStyle : TStylerType;

    void AndFinalValueCollectionParamEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
      where TToStyle : TStylerType;

    void AndFinalValueCollectionParamEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
      where TToStyle : TStylerType;

    void AndFinalObjectCollectionParam<T>(T[]? value, string? formatString = null) where T : class;
    
    void AndFinalObjectCollectionParam<T>((T[]?, string?) valueTuple) where T : class;

    void AndFinalObjectCollectionParam<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;
    
    void AndFinalObjectCollectionParam<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    void AndFinalObjectCollectionParam<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParam<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
      where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParam<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParam<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParam<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
      where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParam<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) 
      where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParamEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class;

    void AndFinalObjectCollectionParamEnumerate<T>((IEnumerable<T>?, string?) valueTuple) where T : class;

    void AndFinalObjectCollectionParamEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class;

    void AndFinalObjectCollectionParamEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class;
    
    void AndFinalObjectCollectionParam<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;
    
    void AndFinalObjectCollectionParam<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParam<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) where T : class, TBase where TBase : class;
    
    void AndFinalObjectCollectionParam<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParam<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void AndFinalObjectCollectionParam<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void AndFinalObjectCollectionParam<T, TBase1, TBase2>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
    , CustomTypeStyler<TBase2> customTypeStyler) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void AndFinalObjectCollectionParam<T, TBase1, TBase2>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    void AndFinalObjectCollectionParamEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
      where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParamEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParamEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
      where T : class, TBase where TBase : class;

    void AndFinalObjectCollectionParamEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class;

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

    void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple)
      where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple)
      where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple)
      where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple)
      where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple)
      where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple)
      where TValue : TVBase;
    
    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
    , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
    , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;
    
    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
    , CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
    , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
    , CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
    , CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>
    , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>
    , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>
    , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
    , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParamEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
    , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
    , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase;
    
    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, 
      KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;
    
    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
    , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, 
      KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, 
      KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, 
      KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, 
      string? keyFormatString = null) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) 
      where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, 
      KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?,
      KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>
    , CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>
    , CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>
    , CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>
    , CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple) 
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
    , KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?
    , KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    void AndFinalKeyedCollectionParam<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;


    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalMatchParamThenToAppender<T>(T? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(bool? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(bool value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender<TNum>(TNum value) where TNum : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender<TNum>(TNum? value) where TNum : struct, ISpanFormattable;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(string? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((string?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((string?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(char[]? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((char[]?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(char[]? value, int fromIndex, int count);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(ICharSequence? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(StringBuilder? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(IStyledToStringObject? value);

    [MustUseReturnValue("Use AndFinalParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AndFinalParamThenToAppender(object? value);
    
  
    IStringAppenderCollectionBuilder AndFinalCollectionParamThenToAppender
    {
      [MustUseReturnValue("Use Add* to add a collection to the format parameters")] get;
    }


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
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple)
      where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value
    , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

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
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

    [MustUseReturnValue("Use AndFinalValueCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamToStringAppender<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType;

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
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TToStyle, TStylerType>
      (IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TToStyle, TStylerType>(
      (IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TToStyle, TStylerType>(
      IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalValueCollectionParamEnumerateToStringAppender<TToStyle, TStylerType>(
      (IEnumerator<TToStyle>? , CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>(T[]? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>((T[]?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>(T[]? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>((T[]?
    , OrderedCollectionPredicate<TBase> , string?) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>((T[]?
    , OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase> filter, string? formatString = null) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>((IReadOnlyList<T>?
    , OrderedCollectionPredicate<TBase>, string?) valueTuple) where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>((IReadOnlyList<T>?
    , OrderedCollectionPredicate<TBase>) valueTuple) where T : class, TBase where TBase : class;

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
    
    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) 
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) 
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase1, TBase2>(
      T[]? value, OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase1, TBase2>(
      (T[]?, OrderedCollectionPredicate<TBase1> , CustomTypeStyler<TBase2>) valueTuple) 
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;
    

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase1, TBase2>(
      IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamToStringAppender<T, TBase1, TBase2>(
      (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
      where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamEnumerateToStringAppender<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamEnumerateToStringAppender<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamEnumerateToStringAppender<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler)
      where T : class, TBase where TBase : class;

    [MustUseReturnValue("Use AndFinalObjectCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalObjectCollectionParamEnumerateToStringAppender<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
      where T : class, TBase where TBase : class;
    
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
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
      IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString) where TValue : TVBase;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
      (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
      IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler, string? keyFormatString) where TValue : TVBase;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase;

    [MustUseReturnValue("Use AfterFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase;
    
    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
    , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
      where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TKBase, TVBase>(
      (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
      where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParamEnumerate if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamEnumerateToStringAppender<TKey, TValue, TKBase, TVBase>(
      (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
      where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string? , string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
    , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
    , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
    , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
      where TKey : TKBase where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase, TVBase1, TVBase2>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
      where TKey : TKBase where TValue : TVBase1, TVBase2;
    
    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
    , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
      (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
      (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
      IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    [MustUseReturnValue("Use AndFinalKeyedCollectionParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender AfterFinalKeyedCollectionParamToStringAppender<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
      (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
      where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2;

    // ReSharper restore UnusedMember.Global
}
