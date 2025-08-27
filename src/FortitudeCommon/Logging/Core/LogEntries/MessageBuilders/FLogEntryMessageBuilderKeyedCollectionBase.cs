using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder
{
    protected IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        (IStyledTypeStringAppender? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, IStyledTypeStringAppender? appender) 
    {
        appender?.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender? toAppendTo, KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        (KeyValuePair<TKey, TValue>[]? value, IStyledTypeStringAppender? appender) 
    {
        appender?.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, IStyledTypeStringAppender? appender) 
    {
        appender?.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue>
    (IStyledTypeStringAppender? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, IStyledTypeStringAppender? appender) 
    {
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue>
    (IStyledTypeStringAppender? toAppendTo, IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple, IStyledTypeStringAppender? appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, IStyledTypeStringAppender? appender) 
    {
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null)  
        where TValue : TVBase 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple, IStyledTypeStringAppender? appender)
        where TValue : TVBase 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple, IStyledTypeStringAppender? appender)
        where TValue : TVBase 
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TValue : TVBase
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TVBase>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TValue : TVBase
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TValue : TVBase 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TValue : TVBase 
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TVBase>
    (IStyledTypeStringAppender? toAppendTo, KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TValue : TVBase 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TValue : TVBase 
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , string? keyFormatString = null) where TValue : TVBase 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TValue : TVBase 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TValue : TVBase 
    {
        var (value, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
    (IStyledTypeStringAppender? toAppendTo, KeyValuePair<TKey, TValue>[]? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAll(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple
          , IStyledTypeStringAppender? appender) where TKey : TKBase where TValue : TVBase 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IEnumerator<KeyValuePair<TKey, TValue>>? value, CustomTypeStyler<TVBase> valueStyler
      , CustomTypeStyler<TKBase> keyStyler) where TKey : TKBase where TValue : TVBase 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendKeyedCollectionEnumerate<TKey, TValue, TKBase, TVBase>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, CustomTypeStyler<TVBase>, CustomTypeStyler<TKBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple, IStyledTypeStringAppender? appender)  
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase>) valueTuple, IStyledTypeStringAppender? appender)  
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
    (IStyledTypeStringAppender? toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase
    {
        var (value, filterPredicate) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple
          , IStyledTypeStringAppender? appender) where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple
          , IStyledTypeStringAppender? appender) where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    (IStyledTypeStringAppender? toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple
          , IStyledTypeStringAppender? appender) where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple
          , IStyledTypeStringAppender? appender) where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, string? keyFormatString = null) 
        where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>, string?) valueTuple
          , IStyledTypeStringAppender? appender) 
        where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        var (value, filterPredicate, customValueStyler, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, customValueStyler, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase, TVBase1, TVBase2>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase, TVBase1>, CustomTypeStyler<TVBase2>) valueTuple
          , IStyledTypeStringAppender? appender) where TKey : TKBase where TValue : TVBase1, TVBase2 
    {
        var (value, filterPredicate, customTypeStyler) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, customTypeStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple
          , IStyledTypeStringAppender? appender) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (IStyledTypeStringAppender? toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple
          , IStyledTypeStringAppender? appender) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKBase1, TVBase1> filterPredicate
      , CustomTypeStyler<TVBase2> valueStyler, CustomTypeStyler<TKBase2> keyStyler) where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2  
    {
        toAppendTo?.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStyler, keyStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredKeyedCollection<TKey, TValue, TKBase1, TKBase2, TVBase1, TVBase2>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKBase1, TVBase1>, CustomTypeStyler<TVBase2>, CustomTypeStyler<TKBase2>) valueTuple
          , IStyledTypeStringAppender? appender) 
        where TKey : TKBase1, TKBase2 where TValue : TVBase1, TVBase2 
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender?.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }
}
