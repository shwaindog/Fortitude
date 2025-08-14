using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder 
{
    
    protected IStyledTypeStringAppender AppendValueCollection<TFmtStruct>
        (IStyledTypeStringAppender toAppendTo, TFmtStruct[]? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAll(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollection<TFmtStruct>
        (TFmtStruct[]? value, IStyledTypeStringAppender appender)
        where TFmtStruct : struct, ISpanFormattable
    {
        appender.StartSimpleCollectionType("")
                  .AddAll(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollection<TFmtStruct>
        ((TFmtStruct[]?, string?) valueTuple, IStyledTypeStringAppender appender)
        where TFmtStruct : struct, ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                  .AddAll(value, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollection<TFmtStruct>
        (IReadOnlyList<TFmtStruct>? value, IStyledTypeStringAppender appender)
        where TFmtStruct : struct, ISpanFormattable
    {
        appender.StartSimpleCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendValueCollection<TFmtStruct>
        (IStyledTypeStringAppender toAppendTo, IReadOnlyList<TFmtStruct>? value, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAll(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollection<TFmtStruct>
        ((IReadOnlyList<TFmtStruct>?, string?) valueTuple, IStyledTypeStringAppender appender)
        where TFmtStruct : struct, ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendValueCollection<TStruct>
        (IStyledTypeStringAppender toAppendTo, TStruct[]? value, StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAll(value, structStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollection<TStruct>
        ((TStruct[]?, StructStyler<TStruct>) valueTuple, IStyledTypeStringAppender appender)
        where TStruct : struct
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendValueCollection<TStruct>
        (IStyledTypeStringAppender toAppendTo, IReadOnlyList<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAll(value, structStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollection<TStruct>
        ((IReadOnlyList<TStruct>?, StructStyler<TStruct>) valueTuple, IStyledTypeStringAppender appender)
        where TStruct : struct
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredValueCollection<TFmtStruct>
    (IStyledTypeStringAppender toAppendTo, TFmtStruct[]? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredValueCollection<TFmtStruct>
        ((TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple, IStyledTypeStringAppender appender)
        where TFmtStruct : struct, ISpanFormattable
    {
        var (value, filter, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFiltered(value, filter, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredValueCollection<TFmtStruct>
        ((TFmtStruct[]?, OrderedCollectionPredicate<TFmtStruct>) valueTuple, IStyledTypeStringAppender appender)
        where TFmtStruct : struct, ISpanFormattable
    {
        var (value, filter) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFiltered(value, filter).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredValueCollection<TFmtStruct>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyList<TFmtStruct>? value, OrderedCollectionPredicate<TFmtStruct> filter
      , string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredValueCollection<TFmtStruct>
        ((IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>, string?) valueTuple, IStyledTypeStringAppender appender)
        where TFmtStruct : struct, ISpanFormattable
    {
        var (value, filter, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFiltered(value, filter, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredValueCollection<TFmtStruct>
        ((IReadOnlyList<TFmtStruct>?, OrderedCollectionPredicate<TFmtStruct>) valueTuple, IStyledTypeStringAppender appender)
        where TFmtStruct : struct, ISpanFormattable
    {
        var (value, filter) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFiltered(value, filter).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredValueCollection<TStruct>
    (IStyledTypeStringAppender toAppendTo, TStruct[]? value, OrderedCollectionPredicate<TStruct> filter
      , StructStyler<TStruct> structStyler) where TStruct : struct
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, structStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredValueCollection<TStruct>
        ((TStruct[]?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple, IStyledTypeStringAppender appender)
        where TStruct : struct
    {
        var (value, filter, structStyler) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFiltered(value, filter, structStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredValueCollection<TStruct>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyList<TStruct>? value, OrderedCollectionPredicate<TStruct> filter
      , StructStyler<TStruct> structStyler) where TStruct : struct 
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, structStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredValueCollection<TStruct>
        ((IReadOnlyList<TStruct>?, OrderedCollectionPredicate<TStruct>, StructStyler<TStruct>) valueTuple, IStyledTypeStringAppender appender)
        where TStruct : struct
    {
        var (value, filter, structStyler) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFiltered(value, filter, structStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendObjectCollection<T>
        (IStyledTypeStringAppender toAppendTo, T[]? value, string? formatString = null) 
        where T : class 
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAllMatch(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendObjectCollection<T>
        (T[]? value, IStyledTypeStringAppender appender) 
        where T : class 
    {
        appender.StartSimpleCollectionType("")
                .AddAllMatch(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendObjectCollection<T>
        ((T[]?, string?) valueTuple, IStyledTypeStringAppender appender) 
        where T : class 
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAllMatch(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendObjectCollection<T>
        (IStyledTypeStringAppender toAppendTo, IReadOnlyList<T>? value, string? formatString = null) where T : class
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAllMatch(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendObjectCollection<T>
        ((IReadOnlyList<T>?, string?) valueTuple, IStyledTypeStringAppender appender) 
        where T : class 
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAllMatch(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredObjectCollection<T>
        (IStyledTypeStringAppender toAppendTo, T[]? value, OrderedCollectionPredicate<T> filter, string? formatString = null) where T : class
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddFilteredMatch(value, filter, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredObjectCollection<T>
        ((T[]?, OrderedCollectionPredicate<T>, string?) valueTuple, IStyledTypeStringAppender appender) 
        where T : class 
    {
        var (value, filter, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredObjectCollection<T>
        ((T[]?, OrderedCollectionPredicate<T>) valueTuple, IStyledTypeStringAppender appender) 
        where T : class 
    {
        var (value, filter) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendFilteredObjectCollection<T>
    (IStyledTypeStringAppender toAppendTo, IReadOnlyList<T>? value, OrderedCollectionPredicate<T> filter
      , string? formatString = null) where T : class 
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddFilteredMatch(value, filter, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredObjectCollection<T>
        ((IReadOnlyList<T>?, OrderedCollectionPredicate<T>, string?) valueTuple, IStyledTypeStringAppender appender) 
        where T : class 
    {
        var (value, filter, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendFilteredObjectCollection<T>
        ((IReadOnlyList<T>?, OrderedCollectionPredicate<T>) valueTuple, IStyledTypeStringAppender appender) 
        where T : class 
    {
        var (value, filter) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendValueCollectionEnumerate<TFmtStruct>
        (IStyledTypeStringAppender toAppendTo, IEnumerable<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable 
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollectionEnumerate<TFmtStruct>
        (IEnumerable<TFmtStruct>? value, IStyledTypeStringAppender appender) 
        where TFmtStruct : struct, ISpanFormattable 
    {
        appender.StartSimpleCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollectionEnumerate<TFmtStruct>
        ((IEnumerable<TFmtStruct>?, string?) valueTuple, IStyledTypeStringAppender appender) 
        where TFmtStruct : struct, ISpanFormattable 
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAllEnumerate(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendValueCollectionEnumerate<TFmtStruct>
        (IStyledTypeStringAppender toAppendTo, IEnumerator<TFmtStruct>? value, string? formatString = null) where TFmtStruct : struct, ISpanFormattable
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollectionEnumerate<TFmtStruct>
        (IEnumerator<TFmtStruct>? value, IStyledTypeStringAppender appender) 
        where TFmtStruct : struct, ISpanFormattable 
    {
        appender.StartSimpleCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollectionEnumerate<TFmtStruct>
        ((IEnumerator<TFmtStruct>?, string?) valueTuple, IStyledTypeStringAppender appender) 
        where TFmtStruct : struct, ISpanFormattable 
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAllEnumerate(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendValueCollectionEnumerate<TStruct>
        (IStyledTypeStringAppender toAppendTo, IEnumerable<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, structStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollectionEnumerate<TStruct>
        ((IEnumerable<TStruct>?, StructStyler<TStruct>) valueTuple, IStyledTypeStringAppender appender) 
        where TStruct : struct 
    {
        var (value, structStyler) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAllEnumerate(value, structStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendValueCollectionEnumerate<TStruct>
        (IStyledTypeStringAppender toAppendTo, IEnumerator<TStruct>? value, StructStyler<TStruct> structStyler) where TStruct : struct
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, structStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendValueCollectionEnumerate<TStruct>
        ((IEnumerator<TStruct>?, StructStyler<TStruct>) valueTuple, IStyledTypeStringAppender appender) 
        where TStruct : struct 
    {
        var (value, structStyler) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAllEnumerate(value, structStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender AppendObjectCollectionEnumerate<T>
        (IStyledTypeStringAppender toAppendTo, IEnumerable<T>? value, string? formatString = null) where T : class 
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAllMatchEnumerate(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendObjectCollectionEnumerate<T>
        (IEnumerable<T>? value, IStyledTypeStringAppender appender) 
        where T : class  
    {
        appender.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendObjectCollectionEnumerate<T>
        ((IEnumerable<T>?, string?) valueTuple, IStyledTypeStringAppender appender) 
        where T : class 
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value, formatString).Complete();
        return appender;
    }
    
    public IStyledTypeStringAppender AppendObjectCollectionEnumerate<T>
        (IStyledTypeStringAppender toAppendTo, IEnumerator<T>? value, string? formatString = null) 
        where T : class 
    {
        toAppendTo.StartSimpleCollectionType("")
                  .AddAllMatchEnumerate(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender AppendObjectCollectionEnumerate<T>
        (IEnumerator<T>? value, IStyledTypeStringAppender appender) 
        where T : class  
    {
        appender.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender AppendObjectCollectionEnumerate<T>
        ((IEnumerator<T>?, string?) valueTuple, IStyledTypeStringAppender appender) 
        where T : class 
    {
        var (value, formatString) = valueTuple;
        appender.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value, formatString).Complete();
        return appender;
    }
    
}
