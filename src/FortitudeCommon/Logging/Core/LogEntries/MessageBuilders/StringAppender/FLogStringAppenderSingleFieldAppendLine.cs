using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender
{
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine() => MessageSb.AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(bool value) => MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(bool? value) => MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable => 
        MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable => 
        MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>(TStruct value, Types.StyledToString.StyledTypes.StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        structStyler(value, MessageStsa);
        return MessageSb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>((TStruct, Types.StyledToString.StyledTypes.StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, MessageStsa);
        return MessageSb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>(TStruct? value, Types.StyledToString.StyledTypes.StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        if (value != null) structStyler(value.Value, MessageStsa);
        return MessageSb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine<TStruct>((TStruct?, Types.StyledToString.StyledTypes.StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, MessageStsa);
        return MessageSb.AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ReadOnlySpan<char> value) => MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value[startIndex..(startIndex + count)]).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value) => MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((string?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(string? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char[]? value) => MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((char[]?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(char[]? value, int startIndex, int count = int.MaxValue) => MessageSb.Append(value).AppendLine(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ICharSequence? value) => MessageSb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((ICharSequence?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((ICharSequence?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(StringBuilder? value) => MessageSb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((StringBuilder?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine((StringBuilder?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value).AppendLine().ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(IStyledToStringObject? value)
    {
        value?.ToString(MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendLine(object? value) => MessageSb.Append(value).AppendLine(this);


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormatLine<TFmtStruct>(TFmtStruct value, string formatString) where TFmtStruct : struct, ISpanFormattable =>
        MessageSb.AppendFormat(formatString, value).AppendLine(this);
    

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormatLine<TFmtStruct>((TFmtStruct, string) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        return AppendLine(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormatLine<TFmtStruct>(TFmtStruct? value, string formatString) where TFmtStruct : struct, ISpanFormattable =>
        MessageSb.AppendFormat(formatString, value).ToAppender(this);
    
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormatLine<TFmtStruct>((TFmtStruct?, string) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        return AppendLine(this);
    }
}
