using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using JetBrains.Annotations;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.FormatBuilder;

public interface IFLogFirstFormatterParameterEntry : IFLogFormatterParameterEntry
{
    // ReSharper disable UnusedMember.Global


    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithMatchParams<T>(T value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(bool? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(bool value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TFmt>(TFmt value) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((string?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((string?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((char[]?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    IFLogAdditionalFormatterParameterEntry? WithParams(IStyledToStringObject? value);

    [MustUseReturnValue("Use WithOnlyParam if only one Parameter is required")]
    [CallsObjectToString]
    IFLogAdditionalFormatterParameterEntry? WithObjectParams(object? value);
    
    IFLogAdditionalParamCollectionAppend WithParamsCollection
    {
        [MustUseReturnValue("Use Add* to add a collection to the format parameters")] get;
    }

    void WithOnlyMatchParam<T>(T value);
    void WithOnlyParam(bool? value);
    void WithOnlyParam(bool value);
    void WithOnlyParam<TFmt>(TFmt value) where TFmt : ISpanFormattable;
    void WithOnlyEnumParam<TEnum>(TEnum value) where TEnum : Enum;
    void WithOnlyParam<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;
    void WithOnlyParam<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;
    void WithOnlyParam(ReadOnlySpan<char> value);
    void WithOnlyParam(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(string? value);
    void WithOnlyParam((string?, int) valueTuple);
    void WithOnlyParam((string?, int, int) valueTuple);
    void WithOnlyParam(string? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(char[]? value);
    void WithOnlyParam((char[]?, int) valueTuple);
    void WithOnlyParam((char[]?, int, int) valueTuple);
    void WithOnlyParam(char[]? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(ICharSequence? value);
    void WithOnlyParam((ICharSequence?, int) valueTuple);
    void WithOnlyParam((ICharSequence?, int, int) valueTuple);
    void WithOnlyParam(ICharSequence? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(StringBuilder? value);
    void WithOnlyParam((StringBuilder?, int) valueTuple);
    void WithOnlyParam((StringBuilder?, int, int) valueTuple);
    void WithOnlyParam(StringBuilder? value, int startIndex, int count = int.MaxValue);
    void WithOnlyParam(IStyledToStringObject? value);
    
    [CallsObjectToString]
    void WithOnlyObjectParam(object? value);
    
    IFinalCollectionAppend WithOnlyParamCollection
    {
        [MustUseReturnValue("Use Add* to add a collection to the format parameters")] get;
    }

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamMatchThenToAppender<T>(T value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(bool? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(bool value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender<TFmt>(TFmt value) where TFmt : ISpanFormattable;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) 
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) 
        where TToStyle : TStylerType;

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(ReadOnlySpan<char> value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(string? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender((string?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender((string?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(string? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(char[]? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender((char[]?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender((char[]?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(char[]? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(ICharSequence? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender((ICharSequence?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender((ICharSequence?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(ICharSequence? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(StringBuilder? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender((StringBuilder?, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender((StringBuilder?, int, int) valueTuple);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(StringBuilder? value, int startIndex, int count = int.MaxValue);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    IFLogStringAppender WithOnlyParamThenToAppender(IStyledToStringObject? value);

    [MustUseReturnValue("Use WithOnlyParam if you do not plan on using the returned StringAppender")]
    [CallsObjectToString]
    IFLogStringAppender WithOnlyObjectParamThenToAppender(object? value);
    
    IStringAppenderCollectionBuilder WithOnlyParamCollectionThenToAppender
    {
        [MustUseReturnValue("Use Add* to add a collection to the format parameters")] get;
    }
        // ReSharper restore UnusedMember.Global
}
