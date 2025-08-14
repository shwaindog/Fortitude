using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder
{
    protected IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        (IStyledTypeStringAppender toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, string?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        (IReadOnlyDictionary<TKey, TValue>? value, IStyledTypeStringAppender appender) 
    {
        appender.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, KeyValuePair<TKey, TValue>[]? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, string?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        (KeyValuePair<TKey, TValue>[]? value, IStyledTypeStringAppender appender) 
    {
        appender.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAll(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        (IReadOnlyList<KeyValuePair<TKey, TValue>>? value, IStyledTypeStringAppender appender) 
    {
        appender.StartKeyedCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerable<KeyValuePair<TKey, TValue>>? value, IStyledTypeStringAppender appender) 
    {
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IEnumerator<KeyValuePair<TKey, TValue>>? value, string? valueFormatString = null
      , string? keyFormatString = null) 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, string?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        (IEnumerator<KeyValuePair<TKey, TValue>>? value, IStyledTypeStringAppender appender) 
    {
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, filterPredicate) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, filterPredicate) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , string? valueFormatString = null, string? keyFormatString = null) 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, string?) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>) valueTuple, IStyledTypeStringAppender appender) 
    {
        var (value, filterPredicate) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null)  where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStructStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple, IStyledTypeStringAppender appender) where TValue : struct
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple, IStyledTypeStringAppender appender) where TValue : struct
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStructStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple, IStyledTypeStringAppender appender) where TValue : struct
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple, IStyledTypeStringAppender appender) where TValue : struct
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStructStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStructStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, string? keyFormatString = null) where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStructStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, string?) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, filterPredicate, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, filterPredicate, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filterPredicate, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAll(value, valueStructStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, string?) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAll(value, valueStructStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, string?) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , string? keyFormatString = null) where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAll(value, valueStructStyler, keyFormatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, string?) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>) valueTuple, IStyledTypeStringAppender appender) 
        where TValue : struct 
    {
        var (value, valueFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAll(value, valueStructStyler, keyStructStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple, IStyledTypeStringAppender appender) 
        where TKey : struct where TValue : struct 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, KeyValuePair<TKey, TValue>[]? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAll(value, valueStructStyler, keyStructStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, StructStyler<TValue>, StructStyler<TKey>) valueTuple, IStyledTypeStringAppender appender) 
        where TKey : struct where TValue : struct 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAll(value, valueStructStyler, keyStructStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple, IStyledTypeStringAppender appender) 
        where TKey : struct where TValue : struct 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAll(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IEnumerable<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStructStyler, keyStructStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerable<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple, IStyledTypeStringAppender appender) 
        where TKey : struct where TValue : struct 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IEnumerator<KeyValuePair<TKey, TValue>>? value, StructStyler<TValue> valueStructStyler
      , StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddAllEnumerate(value, valueStructStyler, keyStructStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendKeyedCollectionEnumerate<TKey, TValue>
        ((IEnumerator<KeyValuePair<TKey, TValue>>?, StructStyler<TValue>, StructStyler<TKey>) valueTuple, IStyledTypeStringAppender appender) 
        where TKey : struct where TValue : struct 
    {
        var (value, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddAllEnumerate(value, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyDictionary<TKey, TValue>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStructStyler, keyStructStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyDictionary<TKey, TValue>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple
          , IStyledTypeStringAppender appender) 
        where TKey : struct where TValue : struct 
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, KeyValuePair<TKey, TValue>[]? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct 
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStructStyler, keyStructStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((KeyValuePair<TKey, TValue>[]?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple
          , IStyledTypeStringAppender appender) 
        where TKey : struct where TValue : struct 
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyList<KeyValuePair<TKey, TValue>>? value, KeyValuePredicate<TKey, TValue> filterPredicate
      , StructStyler<TValue> valueStructStyler, StructStyler<TKey> keyStructStyler) where TKey : struct where TValue : struct  
    {
        toAppendTo.StartKeyedCollectionType("")
                  .AddFiltered(value, filterPredicate, valueStructStyler, keyStructStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredKeyedCollection<TKey, TValue>
        ((IReadOnlyList<KeyValuePair<TKey, TValue>>?, KeyValuePredicate<TKey, TValue>, StructStyler<TValue>, StructStyler<TKey>) valueTuple
          , IStyledTypeStringAppender appender) 
        where TKey : struct where TValue : struct 
    {
        var (value, filter, valueFormatString, keyFormatString) = valueTuple;
        appender.StartKeyedCollectionType("")
                .AddFiltered(value, filter, valueFormatString, keyFormatString).Complete();
        return appender;
    }
}
