using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using JetBrains.Annotations;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender
{
    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(bool? value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(bool value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable => 
        MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable => 
        MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>(TStruct value, Types.StyledToString.StyledTypes.StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        structStyler(value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>((TStruct, Types.StyledToString.StyledTypes.StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>(TStruct? value, Types.StyledToString.StyledTypes.StructStyler<TStruct> structStyler)
        where TStruct : struct
    {
        if (value != null) structStyler(value.Value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append<TStruct>((TStruct?, Types.StyledToString.StyledTypes.StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ReadOnlySpan<char> value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value[startIndex..(startIndex + count)]).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(string? value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((string?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(string? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(char[]? value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((char[]?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(char[]? value, int startIndex, int count = int.MaxValue) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ICharSequence? value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((ICharSequence?, int) value)
    {
        AppendFromRange(value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((ICharSequence?, int, int) value)
    {
        AppendFromToRange(value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(ICharSequence? value, int startIndex, int count = int.MaxValue) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(StringBuilder? value) => MessageSb.Append(value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((StringBuilder?, int) value)
    {
        AppendFromRange(value, MessageStsa);
        return this;
    }


    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append((StringBuilder?, int, int) value)
    {
        AppendFromToRange(value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(StringBuilder? value, int startIndex, int count = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Max(0, (value?.Length ?? 0) - startIndex), count);
        return MessageSb.Append(value, startIndex, Math.Clamp(count, 0, cappedLength))
                        .ToAppender(this);
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(IStyledToStringObject? value)
    {
        value?.ToString(MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender Append(object? value)
    {
        AppendObject(value, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormat<TFmtStruct>((TFmtStruct, string) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormat<TFmtStruct>((TFmtStruct?, string) valueTuple) where TFmtStruct : struct, ISpanFormattable
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        return this;
    }

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormat<TFmtStruct>(TFmtStruct value, string formatString) where TFmtStruct : struct, ISpanFormattable =>
        MessageSb.AppendFormat(formatString, value).ToAppender(this);

    [MustUseReturnValue("Use FinalAppend to finish LogEntry")]
    public IFLogStringAppender AppendFormat<TFmtStruct>(TFmtStruct? value, string formatString) where TFmtStruct : struct, ISpanFormattable =>
        MessageSb.AppendFormat(formatString, value).ToAppender(this);

}
