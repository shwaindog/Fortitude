using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders;

public abstract partial class FLogEntryMessageBuilder 
{
    protected IStyledTypeStringAppender? AppendValueCollection<TFmt>
        (IStyledTypeStringAppender? toAppendTo, TFmt[]? value, string? formatString = null)
        where TFmt : ISpanFormattable
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollection<TFmt>
        ((TFmt[]?, string?) valueTuple, IStyledTypeStringAppender? appender)
        where TFmt : ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                  .AddAll(value, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollection<TFmt>
        (IReadOnlyList<TFmt>? value, IStyledTypeStringAppender? appender)
        where TFmt : ISpanFormattable
    {
        appender?.StartSimpleCollectionType("")
                .AddAll(value).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendValueCollection<TFmt>
        (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<TFmt>? value, string? formatString = null)
        where TFmt : ISpanFormattable
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollection<TFmt>
        ((IReadOnlyList<TFmt>?, string?) valueTuple, IStyledTypeStringAppender? appender)
        where TFmt : ISpanFormattable
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendValueCollection<TToStyle, TStylerType>
        (IStyledTypeStringAppender? toAppendTo, TToStyle[]? value, CustomTypeStyler<TStylerType> customTypeStyler)
        where TToStyle : TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollection<TToStyle, TStylerType>
        ((TToStyle[]?, CustomTypeStyler<TStylerType>) valueTuple, IStyledTypeStringAppender? appender)
        where TToStyle : TStylerType
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendValueCollection<TToStyle, TStylerType>
        (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollection<TToStyle, TStylerType>
        ((IReadOnlyList<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple, IStyledTypeStringAppender? appender)
        where TToStyle : TStylerType
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredValueCollection<TFmt, TBase>
    (IStyledTypeStringAppender? toAppendTo, TFmt[]? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where TFmt : ISpanFormattable, TBase
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredValueCollection<TFmt, TBase>
        ((TFmt[]?, OrderedCollectionPredicate<TFmt>, string?) valueTuple, IStyledTypeStringAppender? appender)
        where TFmt : ISpanFormattable, TBase
    {
        var (value, filter, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredValueCollection<TFmt, TBase>
        ((TFmt[]?, OrderedCollectionPredicate<TBase>) valueTuple, IStyledTypeStringAppender? appender)
        where TFmt : ISpanFormattable, TBase
    {
        var (value, filter) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredValueCollection<TFmt, TBase>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<TFmt>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where TFmt : ISpanFormattable, TBase
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredValueCollection<TFmt, TBase>
        ((IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>, string?) valueTuple, IStyledTypeStringAppender? appender)
        where TFmt : ISpanFormattable, TBase
    {
        var (value, filter, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredValueCollection<TFmt, TBase>
        ((IReadOnlyList<TFmt>?, OrderedCollectionPredicate<TBase>) valueTuple, IStyledTypeStringAppender? appender)
        where TFmt : ISpanFormattable, TBase
    {
        var (value, filter) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredValueCollection<TToStyle, TToStyleBase, TStylerType>
    (IStyledTypeStringAppender? toAppendTo, TToStyle[]? value, OrderedCollectionPredicate<TToStyleBase> filter
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredValueCollection<TToStyle, TToStyleBase, TStylerType>
        ((TToStyle[]?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple, IStyledTypeStringAppender? appender)
        where TToStyle : TToStyleBase, TStylerType
    {
        var (value, filter, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, structStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredValueCollection<TToStyle, TToStyleBase, TStylerType>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<TToStyle>? value, OrderedCollectionPredicate<TToStyleBase> filter
      , CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TToStyleBase, TStylerType 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredValueCollection<TToStyle, TToStyleBase, TStylerType>
        ((IReadOnlyList<TToStyle>?, OrderedCollectionPredicate<TToStyleBase>, CustomTypeStyler<TStylerType>) valueTuple
          , IStyledTypeStringAppender? appender) where TToStyle : TToStyleBase, TStylerType
    {
        var (value, filter, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, structStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendValueCollectionEnumerate<TFmt>
        (IStyledTypeStringAppender? toAppendTo, IEnumerable<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollectionEnumerate<TFmt>
        (IEnumerable<TFmt>? value, IStyledTypeStringAppender? appender) 
        where TFmt : ISpanFormattable 
    {
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollectionEnumerate<TFmt>
        ((IEnumerable<TFmt>?, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TFmt : ISpanFormattable 
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendValueCollectionEnumerate<TFmt>
        (IStyledTypeStringAppender? toAppendTo, IEnumerator<TFmt>? value, string? formatString = null) where TFmt : ISpanFormattable
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollectionEnumerate<TFmt>
        (IEnumerator<TFmt>? value, IStyledTypeStringAppender? appender) 
        where TFmt : ISpanFormattable 
    {
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollectionEnumerate<TFmt>
        ((IEnumerator<TFmt>?, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where TFmt : ISpanFormattable 
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendValueCollectionEnumerate<TToStyle, TStylerType>
        (IStyledTypeStringAppender? toAppendTo, IEnumerable<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollectionEnumerate<TToStyle, TStylerType>
        ((IEnumerable<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple, IStyledTypeStringAppender? appender) 
        where TToStyle : TStylerType 
    {
        var (value, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, structStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendValueCollectionEnumerate<TToStyle, TStylerType>
        (IStyledTypeStringAppender? toAppendTo, IEnumerator<TToStyle>? value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendValueCollectionEnumerate<TToStyle, TStylerType>
        ((IEnumerator<TToStyle>?, CustomTypeStyler<TStylerType>) valueTuple, IStyledTypeStringAppender? appender) 
        where TToStyle : TStylerType 
    {
        var (value, structStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, structStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendObjectCollection<T>
        (IStyledTypeStringAppender? toAppendTo, T[]? value, string? formatString = null) 
        where T : class 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllMatch(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollection<T>(T[]? value, IStyledTypeStringAppender? appender) 
        where T : class 
    {
        appender?.StartSimpleCollectionType("")
                .AddAllMatch(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollection<T>((T[]?, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class 
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllMatch(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendObjectCollection<T>
        (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<T>? value, string? formatString = null) where T : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllMatch(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollection<T>((IReadOnlyList<T>?, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class 
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllMatch(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase>
        (IStyledTypeStringAppender? toAppendTo, T[]? value, OrderedCollectionPredicate<TBase> filter, string? formatString = null)
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFilteredMatch(value, filter, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase>
        ((T[]?, OrderedCollectionPredicate<TBase>, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase where TBase : class 
    {
        var (value, filter, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase>
        ((T[]?, OrderedCollectionPredicate<TBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase where TBase : class 
    {
        var (value, filter) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase> filter
      , string? formatString = null) where T : class, TBase  where TBase : class 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFilteredMatch(value, filter, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase>
        ((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase where TBase : class 
    {
        var (value, filter, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter, formatString).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase>
        ((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase where TBase : class 
    {
        var (value, filter) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFilteredMatch(value, filter).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T>
        (IStyledTypeStringAppender? toAppendTo, IEnumerable<T>? value, string? formatString = null) where T : class 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllMatchEnumerate(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T>(IEnumerable<T>? value, IStyledTypeStringAppender? appender) 
        where T : class  
    {
        appender?.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T>((IEnumerable<T>?, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class 
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value, formatString).Complete();
        return appender;
    }
    
    public IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T>
        (IStyledTypeStringAppender? toAppendTo, IEnumerator<T>? value, string? formatString = null) 
        where T : class 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllMatchEnumerate(value, formatString).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T>
        (IEnumerator<T>? value, IStyledTypeStringAppender? appender) 
        where T : class  
    {
        appender?.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value).Complete();
        return appender;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T>
        ((IEnumerator<T>?, string?) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class 
    {
        var (value, formatString) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllMatchEnumerate(value, formatString).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendObjectCollection<T, TBase>
        (IStyledTypeStringAppender? toAppendTo, T?[]? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollection<T, TBase>
        ((T[]?, CustomTypeStyler<TBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase where TBase : class 
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, customTypeStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendObjectCollection<T, TBase>
        (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAll(value, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollection<T, TBase>
        ((IReadOnlyList<T>?, CustomTypeStyler<TBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase where TBase : class 
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAll(value, customTypeStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase1, TBase2>
        (IStyledTypeStringAppender? toAppendTo, T[]? value, OrderedCollectionPredicate<TBase1> filter, CustomTypeStyler<TBase2> customTypeStyler) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase1, TBase2>
        ((T[]?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class  
    {
        var (value, filter, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, customTypeStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase1, TBase2>
    (IStyledTypeStringAppender? toAppendTo, IReadOnlyList<T>? value, OrderedCollectionPredicate<TBase1> filter
      , CustomTypeStyler<TBase2> customTypeStyler) where T : class, TBase1, TBase2  where TBase1 : class  where TBase2 : class 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddFiltered(value, filter, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendFilteredObjectCollection<T, TBase1, TBase2>
        ((IReadOnlyList<T>?, OrderedCollectionPredicate<TBase1>, CustomTypeStyler<TBase2>) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase1, TBase2 where TBase1 : class where TBase2 : class 
    {
        var (value, filter, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddFiltered(value, filter, customTypeStyler).Complete();
        return appender;
    }
    
    protected IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T, TBase>
        (IStyledTypeStringAppender? toAppendTo, IEnumerable<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T, TBase>
        ((IEnumerable<T>?, CustomTypeStyler<TBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase where TBase : class  
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, customTypeStyler).Complete();
        return appender;
    }
    
    public IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T, TBase>
        (IStyledTypeStringAppender? toAppendTo, IEnumerator<T>? value, CustomTypeStyler<TBase> customTypeStyler) 
        where T : class, TBase where TBase : class 
    {
        toAppendTo?.StartSimpleCollectionType("")
                  .AddAllEnumerate(value, customTypeStyler).Complete();
        return toAppendTo;
    }
    
    protected static IStyledTypeStringAppender? AppendObjectCollectionEnumerate<T, TBase>
        ((IEnumerator<T>?, CustomTypeStyler<TBase>) valueTuple, IStyledTypeStringAppender? appender) 
        where T : class, TBase where TBase : class 
    {
        var (value, customTypeStyler) = valueTuple;
        appender?.StartSimpleCollectionType("")
                .AddAllEnumerate(value, customTypeStyler).Complete();
        return appender;
    }
    
}
