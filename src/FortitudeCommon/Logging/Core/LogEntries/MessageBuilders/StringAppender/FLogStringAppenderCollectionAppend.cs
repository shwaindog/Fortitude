using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender
{
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>(TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollection(MessageStsa, value, formatString).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>((TFmtStruct[]?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollection(MessageStsa, value, formatString).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TToStyle, TStylerType>(TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType =>
        AppendValueCollection(MessageStsa, value, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType =>
        AppendValueCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType =>
        AppendValueCollection(MessageStsa, value, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType =>
        AppendValueCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>(TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(MessageStsa, value, filter, formatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>(
        (TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>(IReadOnlyList<TFmtStruct>? value
      , OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable  =>
        AppendFilteredValueCollection(MessageStsa, value, filter, formatString).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TFmtStruct>(
        (IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) where TFmtStruct : struct, ISpanFormattable =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType =>
        AppendFilteredValueCollection(MessageStsa, value, filter, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TToStyle, TToStyleBase, TStylerType>(
        (TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TToStyleBase, TStylerType =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value
      , OrderedCollectionPredicate<TToStyleBase> filter, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType =>
        AppendFilteredValueCollection(MessageStsa, value, filter, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollection to finish LogEntry")]
    public IFLogStringAppender AppendValueCollection<TToStyle, TToStyleBase, TStylerType>(
        (IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TToStyleBase, TStylerType =>
        AppendFilteredValueCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollectionEnumerate(MessageStsa, value, formatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollectionEnumerate(MessageStsa, value, formatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable =>
        AppendValueCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        AppendValueCollectionEnumerate(MessageStsa, value, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerate<TToStyle, TStylerType>((IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType =>
        AppendValueCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType =>
        AppendValueCollectionEnumerate(MessageStsa, value, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendValueCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendValueCollectionEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType =>
        AppendValueCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class =>
        AppendObjectCollection(MessageStsa, value, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class =>
        AppendObjectCollection(valueTuple, MessageStsa).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class =>
        AppendObjectCollection(MessageStsa, value, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class =>
        AppendObjectCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null) 
        where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(MessageStsa, value, filter, formatString).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple)
        where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(MessageStsa, value, filter, formatString).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple)
        where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) 
        where T : class, TBase where TBase : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T>(T[]? value, string? formatString = null) where T : class =>
        AppendObjectCollection(MessageStsa, value, formatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T>((T[]?, string?) valueTuple) where T : class =>
        AppendObjectCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class =>
        AppendObjectCollection(MessageStsa, value, formatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class =>
        AppendObjectCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerate<T>(IEnumerable<T>? value, string? formatString = null) where T : class =>
        AppendObjectCollectionEnumerate(MessageStsa, value, formatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerate<T>((IEnumerable<T>?, string?) valueTuple) where T : class =>
        AppendObjectCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class =>
        AppendObjectCollectionEnumerate(MessageStsa, value, formatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class =>
        AppendObjectCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase1, TBase2>(T[]? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler)
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class  =>
        AppendFilteredObjectCollection(MessageStsa, value, filter, customTypeStyler).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>
      , CustomTypeStyler<TBase2>) valueTuple) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase1, TBase2>(IReadOnlyList<T>? value
      , OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class =>
        AppendFilteredObjectCollection(MessageStsa, value, filter, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollection to finish and send LogEntry")]
    public IFLogStringAppender AppendObjectCollection<T, TBase1, TBase2>(
        (IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class =>
        AppendFilteredObjectCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class  =>
        AppendObjectCollectionEnumerate(MessageStsa, value, customTypeStyler).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class  =>
        AppendObjectCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler)
        where T : class, TBase where TBase : class =>
        AppendObjectCollectionEnumerate(MessageStsa, value, customTypeStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendObjectCollectionEnumerate to finish and send LogEntry")]
    public IFLogStringAppender AppendObjectCollectionEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple)
        where T : class, TBase where TBase : class =>
        AppendObjectCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AppendKeyedCollection(MessageStsa, value, valueFormatString, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value
      , string? valueFormatString = null, string? keyFormatString = null)  =>
        AppendKeyedCollection(MessageStsa, value, valueFormatString, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null)  =>
        AppendKeyedCollection(MessageStsa, value, valueFormatString, keyFormatString).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null)  =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueFormatString, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , string? valueFormatString = null, string? keyFormatString = null) =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueFormatString, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple) =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase  =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase  =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
    (IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueStyler, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
    (IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase  =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueStyler, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple) where TValue : TVBase =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) where TValue : TVBase =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollection(MessageStsa, value, valueStyler, keyStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueStyler, keyStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollectionEnumerate(MessageStsa, value, valueStyler, keyStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollectionEnumerate to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>(
        (IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple)
        where TKey : TKBase where TValue : TVBase =>
        AppendKeyedCollectionEnumerate(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueFormatString, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) 
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) 
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple) 
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null)  
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueFormatString, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) 
        where TKey : TKBase where TValue : TVBase  =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) 
        where TKey : TKBase where TValue : TVBase  =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple) 
        where TKey : TKBase where TValue : TVBase  =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase   =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueFormatString, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) 
        where TKey : TKBase where TValue : TVBase  =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) 
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple) 
        where TKey : TKBase where TValue : TVBase =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null)
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyFormatString).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple)
        where TKey : TKBase where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(MessageStsa, value, filterPredicate, valueStyler, keyStyler).ToAppender(this);

    [MustUseReturnValue("Use FinalAppendKeyedCollection to finish LogEntry")]
    public IFLogStringAppender AppendKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 =>
        AppendFilteredKeyedCollection(valueTuple, MessageStsa).ToAppender(this);
    
}
