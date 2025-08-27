using System.Numerics;
using System.Text;
using FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.Collections;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
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
    IFLogStringAppender Append<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppend to finish and send LogEntry")]
    IFLogStringAppender Append<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

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
    
    IStringAppenderCollectionBuilder AppendCollection { get; }
    
    
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
    IFLogStringAppender AppendLine<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;

    [MustUseReturnValue("Use FinalAppendLine to finish and send LogEntry")]
    IFLogStringAppender AppendLine<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;

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
    
    IStringAppenderCollectionBuilder AppendLineCollection { get; }
        
    void FinalMatchAppend<T>(T value);
    void FinalAppend(bool value);
    void FinalAppend(bool? value);
    void FinalAppend<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable;
    void FinalAppend<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable;
    void FinalAppend<TToStyle, TStylerType>(TToStyle value, CustomTypeStyler<TStylerType> customTypeStyler) where TToStyle : TStylerType;
    void FinalAppend<TToStyle, TStylerType>((TToStyle, CustomTypeStyler<TStylerType>) valueTuple) where TToStyle : TStylerType;
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

    void FinalAppendFormat<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum>;
    void FinalAppendFormat<TNum>((TNum, string) value) where TNum : struct, INumber<TNum>;
    void FinalAppendFormat<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum>;
    void FinalAppendFormat<TNum>((TNum?, string) value) where TNum : struct, INumber<TNum>;
    
    IFinalCollectionAppend FinalAppendCollection { get; }
    
    // ReSharper restore UnusedMember.Global
}
