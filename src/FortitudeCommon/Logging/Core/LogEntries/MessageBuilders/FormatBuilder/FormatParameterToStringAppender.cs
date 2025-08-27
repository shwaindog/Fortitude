using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public class FormatParameterToStringAppender : RecyclableObject, IStringAppenderCollectionBuilder
{
    private IFinalCollectionAppend  wrappedCollectionApppender = null!;
    private IStringBuilder          formattedString = null!;
    private FLogEntry               logEntry = null!;
    private Action<IStringBuilder?> onComplete = null!;
    

    public FormatParameterToStringAppender Initialize(FLogEntry fLogEntry, Action<IStringBuilder?> callWhenComplete,
        IFinalCollectionAppend finalCollectionAppend, IStringBuilder stringBuilder)
    {
        wrappedCollectionApppender =  finalCollectionAppend;
        formattedString = stringBuilder;
        logEntry = fLogEntry;
        onComplete = callWhenComplete;

        return this;
    }

    private IFLogStringAppender ConvertToStringAppender()
    {
        var styleTypeStringAppender = (Recycler?.Borrow<StyledTypeStringAppender>() ?? new StyledTypeStringAppender(logEntry.Style))
            .Initialize(formattedString, logEntry.Style);

        var convertedToStringAppender = (Recycler?.Borrow<FLogStringAppender>() ?? new FLogStringAppender())
            .Initialize(logEntry, styleTypeStringAppender, onComplete);

        DecrementRefCount();
        return convertedToStringAppender;
    }
    
    public IFLogStringAppender Add<TFmtStruct>(TFmtStruct[]? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TFmtStruct>((TFmtStruct[]?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TFmtStruct>((IReadOnlyList<TFmtStruct>?, string?) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TStylerType>(TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.Add(value, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TStylerType>((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TStylerType>(IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.Add(value, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TStylerType>((IReadOnlyList<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TFmtStruct>(TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(value, filter, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TFmtStruct>((TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TFmtStruct>((TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TFmtStruct>(IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filter, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(value, filter, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TFmtStruct>((IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TFmtStruct>((IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple) 
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TToStyleBase, TStylerType>(TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filter
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        wrappedCollectionApppender.Add(value, filter, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TToStyleBase, TStylerType>((TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, 
        CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TToStyleBase, TStylerType>(IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filter
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        wrappedCollectionApppender.Add(value, filter, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender Add<TToStyle, TToStyleBase, TStylerType>((IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, 
        CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TToStyleBase, TStylerType
    {
        wrappedCollectionApppender.Add(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TFmtStruct>(IEnumerable<TFmtStruct>? value, string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.AddEnumerate(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TFmtStruct>((IEnumerable<TFmtStruct>?, string?) 
        valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.AddEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TFmtStruct>(IEnumerator<TFmtStruct>? value, string? formatString = null) 
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.AddEnumerate(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TFmtStruct>((IEnumerator<TFmtStruct>?, string?) valueTuple)
        where TFmtStruct : struct, ISpanFormattable
    {
        wrappedCollectionApppender.AddEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TToStyle, TStylerType>(IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.AddEnumerate(value, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TToStyle, TStylerType>((IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.AddEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TToStyle, TStylerType>(IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.AddEnumerate(value, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddEnumerate<TToStyle, TStylerType>((IEnumerator<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple)
        where TToStyle : TStylerType
    {
        wrappedCollectionApppender.AddEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T>(T[]? value, string? formatString = null) where T : class
    {
        wrappedCollectionApppender.AddMatch(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T>((T[]?, string?) valueTuple) where T : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T>(IReadOnlyList<T>? value, string? formatString = null) where T : class
    {
        wrappedCollectionApppender.AddMatch(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T>((IReadOnlyList<T>?, string?) valueTuple) where T : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>(T[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((T[]?, CustomTypeStyler<TBase>) valueTuple) where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>(IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>(T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((T[]?, OrderedCollectionPredicate<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T>(IEnumerable<T>? value, string? formatString = null) 
        where T : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T>((IEnumerable<T>?, string?) valueTuple) 
        where T : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T>(IEnumerator<T>? value, string? formatString = null) where T : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, formatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T>((IEnumerator<T>?, string?) valueTuple) where T : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>(T[]? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>((T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>(IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(value, filter, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatch<T, TBase1, TBase2>((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>
      , CustomTypeStyler<TBase2>) valueTuple) where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class
    {
        wrappedCollectionApppender.AddMatch(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>(IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>(IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(value, customTypeStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddMatchEnumerate<T, TBase>((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple) 
        where T : class, TBase where TBase : class
    {
        wrappedCollectionApppender.AddMatchEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>(IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyed(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>(KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyed(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((KeyValuePair<TKey, TValue>[]?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyed(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>(IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue>((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple)
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>
      , string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>
      , string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null
      , string? keyFormatString = null) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>
      , string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>
      , string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple) 
        where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase> filterPredicate, string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueFormatString, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>, string?) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>(IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple) 
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple) 
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple) 
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple) 
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple)
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) 
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>
      , string?) valueTuple) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) 
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, string? keyFormatString = null) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>
      , string?) valueTuple) where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple) 
        where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyDictionary<TKey, TValue>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple) 
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>
      , CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value,  filterPredicate, valueStyler, keyFormatString);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase1, TVBase2>((IReadOnlyList<KeyValuePair<TKey, TValue>>?
      , KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple) where TKey : TKBase where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>
      , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>
      , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase, TVBase>((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>
      , CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerable<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerable<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>(IEnumerator<KeyValuePair<TKey, TValue>>? value
      , CustomTypeStyler<TVBase> valueStyler, CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(value, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyedEnumerate<TKey, TValue, TKBase, TVBase>((IEnumerator<KeyValuePair<TKey, TValue>>?
      , CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple) where TKey : TKBase where TValue : TVBase
    {
        wrappedCollectionApppender.AddKeyedEnumerate(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyDictionary<TKey, TValue>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(KeyValuePair<TKey, TValue>[]? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(IReadOnlyList<KeyValuePair<TKey, TValue>>? value
      , KeyValuePredicate<TKBase1, TVBase1> filterPredicate, CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(value, filterPredicate, valueStyler, keyStyler);
        return ConvertToStringAppender();
    }

    public IFLogStringAppender AddKeyed<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>(
        (IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple)
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2
    {
        wrappedCollectionApppender.AddKeyed(valueTuple);
        return ConvertToStringAppender();
    }
}
