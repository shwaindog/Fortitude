using System.Numerics;
using System.Text;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;

namespace FortitudeCommon.Logging.Core.LogEntries.MessageBuilders.StringAppender;

public partial class FLogStringAppender
{
    public void FinalAppend(bool value)  => MessageSb.Append(value).ToAppender(this).CallOnComplete();
    public void FinalAppend(bool? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TFmtStruct>(TFmtStruct value) where TFmtStruct : struct, ISpanFormattable => 
        MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TFmtStruct>(TFmtStruct? value) where TFmtStruct : struct, ISpanFormattable => 
        MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TStruct>(TStruct value, Types.StyledToString.StyledTypes.StructStyler<TStruct> structStyler) where TStruct : struct =>
        MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TStruct>((TStruct, Types.StyledToString.StyledTypes.StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend<TStruct>(TStruct? value, Types.StyledToString.StyledTypes.StructStyler<TStruct> structStyler) where TStruct : struct =>
        MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend<TStruct>((TStruct?, Types.StyledToString.StyledTypes.StructStyler<TStruct>) valueTuple)
        where TStruct : struct
    {
        AppendStruct(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(ReadOnlySpan<char> value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend(ReadOnlySpan<char> value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value[startIndex..(startIndex + count)]).ToAppender(this).CallOnComplete();

    public void FinalAppend(string? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((string?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend((string?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(string? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(char[]? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((char[]?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend((char[]?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(char[]? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(ICharSequence? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((ICharSequence?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend((ICharSequence?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(ICharSequence? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(StringBuilder? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppend((StringBuilder?, int) valueTuple)
    {
        AppendFromRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend((StringBuilder?, int, int) valueTuple)
    {
        AppendFromToRange(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(StringBuilder? value, int startIndex, int count = int.MaxValue) =>
        MessageSb.Append(value, startIndex, count).ToAppender(this).CallOnComplete();

    public void FinalAppend(IStyledToStringObject? value)
    {
        AppendStyledObject(value, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppend(object? value) => MessageSb.Append(value).ToAppender(this).CallOnComplete();

    public void FinalAppendNumberFormatted<TNum>(TNum value, string formatString) where TNum : struct, INumber<TNum>
    {
        MessageSb.AppendFormat(formatString, value);
        CallOnComplete();
    }

    public void FinalAppendNumberFormatted<TNum>((TNum, string) valueTuple) where TNum : struct, INumber<TNum>
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        CallOnComplete();
    }

    public void FinalAppendNumberFormatted<TNum>(TNum? value, string formatString) where TNum : struct, INumber<TNum>
    {
        MessageSb.AppendFormat(formatString, value);
        CallOnComplete();
    }

    public void FinalAppendNumberFormatted<TNum>((TNum?, string) valueTuple) where TNum : struct, INumber<TNum>
    {
        AppendSpanFormattable(valueTuple, MessageStsa);
        CallOnComplete();
    }
}
