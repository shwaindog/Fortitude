// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.StringsOfPower.Forge;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using static FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout.FLogEntryLayoutTokens;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public class LogEntryDataTemplatePart : ITemplatePart, IStringBearer
{
    protected static IStringBuilder? SmallScratchBuffer;

    [ThreadStatic] protected static IStringBuilder? LargeScratchBuffer;

    private TokenFormatting tokenFormatting;

    public LogEntryDataTemplatePart(string tokenName, int paddingLength, int maxLength, string formattingString) =>
        tokenFormatting = new TokenFormatting(tokenName, paddingLength, maxLength, formattingString);

    public LogEntryDataTemplatePart(TokenFormatting tokenFormatting) => this.tokenFormatting = tokenFormatting;

    public int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        var charBufferSize = 512;
        if (tokenFormatting.TokenName.IsLargeBufferCheckRequiredTokenName())
        {
            var tokenName                                                                              = tokenFormatting.TokenName;
            if (tokenName.IsLogEntryMessageTokenName()) charBufferSize                                 = logEntry.Message.Length + 2;
            if (logEntry.Exception != null && tokenName.IsLogEntryExceptionTokenName()) charBufferSize = logEntry.Exception.Message.Length + 2;
        }
        var scratchBuffer = SourceCachedStringBuilder(charBufferSize);
        return Apply(formatWriter, logEntry, scratchBuffer);
    }

    public FormattingAppenderSinkType TargetingAppenderTypes => FormattingAppenderSinkType.Any;

    private IStringBuilder SourceCachedStringBuilder(int charSizeRequired = 512)
    {
        if (charSizeRequired <= 512)
        {
            SmallScratchBuffer ??= 512.SourceCharArrayStringBuilder();
            SmallScratchBuffer.Clear();
            return SmallScratchBuffer;
        }
        if (LargeScratchBuffer is not null)
        {
            if (LargeScratchBuffer.Length < charSizeRequired)
            {
                LargeScratchBuffer.DecrementRefCount();
                LargeScratchBuffer = charSizeRequired.SourceCharArrayStringBuilder();
            }
        }
        else
        {
            if (LargeScratchBuffer is not null && LargeScratchBuffer.Capacity < charSizeRequired)
            {
                LargeScratchBuffer.DecrementRefCount();
                LargeScratchBuffer = charSizeRequired.SourceCharArrayStringBuilder();
            }
            LargeScratchBuffer ??= charSizeRequired.SourceCharArrayStringBuilder();
        }
        LargeScratchBuffer.Clear();
        return LargeScratchBuffer;
    }

    private IStringBuilder SourceEphemeralStringBuilder(int charSizeRequired = 512)
    {
        var temporary = charSizeRequired.SourceCharArrayStringBuilder();
        temporary.Clear();
        return temporary;
    }

    private int Apply(IFormatWriter formatWriter, IFLogEntry logEntry, IStringBuilder partStringBuilder)
    {
        ApplyTokenToStringBuilder(partStringBuilder, logEntry);
        if (partStringBuilder is CharArrayStringBuilder charArrayStringBuilder)
        {
            var writtenCharArrayRange = charArrayStringBuilder.AsCharArrayRange;
            formatWriter.Append(writtenCharArrayRange.CharBuffer, writtenCharArrayRange.FromIndex, writtenCharArrayRange.Length);
        }
        else if (partStringBuilder is MutableString mutableString)
        {
            formatWriter.Append(mutableString.BackingStringBuilder, 0, mutableString.Length);
        }
        else
        {
            formatWriter.Append(partStringBuilder);
        }
        return partStringBuilder.Length;
    }

    // example toke "'%TS:yyyy-MM-dd HH:mm:SS.fff%' '%LVL,5%' '%THREADID,4%' '%THREADNAME[..10],10%' '%LGR,/././[^2..]%';:'%LLN,3:00%' '%MSG%'";
    protected virtual void ApplyTokenToStringBuilder(IStringBuilder sb, IFLogEntry logEntry)
    {
        switch (tokenFormatting.TokenName)
        {
            case $"{nameof(DATETIME)}":
            case $"{nameof(DATE)}":
            case $"{nameof(TIME)}":
            case $"{nameof(TS)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.LogDateTime);
                break;
            case $"{nameof(DATEONLY)}":
            case $"{nameof(DO)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.LogDateTime.Date);
                break;
            case $"{nameof(TIMEONLY)}":
            case $"{nameof(TO)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.LogDateTime - logEntry.LogDateTime.Date);
                break;
            case $"{nameof(DATETIME_MICROSECONDS)}":
            case $"{nameof(DATE_MICROS)}":
            case $"{nameof(TIME_MICROS)}":
            case $"{nameof(TS_US)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.LogDateTime.Microsecond);
                break;
            case $"{nameof(LOGLEVEL)}":
            case $"{nameof(LEVEL)}":
            case $"{nameof(LVL)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.LogLevel);
                break;
            case $"{nameof(CORRELATIONID)}":
            case $"{nameof(CID)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.CorrelationId);
                break;
            case $"{nameof(THREADID)}":
            case $"{nameof(TID)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.Thread.ManagedThreadId);
                break;
            case $"{nameof(THREADNAME)}":
            case $"{nameof(TNAME)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.Thread.Name ?? "".AsSpan());
                break;
            case $"{nameof(LOGGERNAME)}":
            case $"{nameof(LOGGER)}":
            case $"{nameof(LGRNAME)}":
            case $"{nameof(LGR)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.Logger.FullName);
                break;
            case $"{nameof(LOGLINENUMBER)}":
            case $"{nameof(LOGLINENUM)}":
            case $"{nameof(LLN)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.LogLocation.SourceLineNumber);
                break;
            case $"{nameof(LOGMEMBERNAME)}":
            case $"{nameof(LMN)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.LogLocation.MemberName);
                break;
            case $"{nameof(LOGFILENAME_WITHEXT)}":
            case $"{nameof(LFNE)}":
                sb.ExtractAppendFileNameWithExtension(logEntry.LogLocation);
                break;
            case $"{nameof(LOGFILENAME)}":
            case $"{nameof(LFN)}":
                sb.ExtractAppendFileNameNoExt(logEntry.LogLocation);
                break;
            case $"{nameof(LOGFULLFILEPATH)}":
            case $"{nameof(LFFP)}":
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.LogLocation.SourceFilePath);
                break;
            case $"{nameof(MESSAGE)}":
            case $"{nameof(MESG)}":
            case $"{nameof(MSG)}":
                sb.AppendRange(logEntry.Message);
                break;
            case $"{nameof(EXCEPTION)}":
            case $"{nameof(EXCEP)}":
            case $"{nameof(EX)}":
                if (logEntry.Exception is { } ex) sb.Append(ex);
                break;
            case $"{nameof(EXCEPTION_ONELINE)}":
            case $"{nameof(EXCEP_ONELINE)}":
            case $"{nameof(EX_ONELINE)}":
            case $"{nameof(EXCEPTION_1L)}":
            case $"{nameof(EXCEP_1L)}":
            case $"{nameof(EX_1L)}":
                if (logEntry.Exception is { } excep)
                {
                    var scratchBuffer = SourceEphemeralStringBuilder(4096);
                    scratchBuffer.Clear();
                    scratchBuffer.Append(excep);
                    scratchBuffer.Replace("\r", "");
                    scratchBuffer.Replace("\n", ",");
                    sb.AppendRange(scratchBuffer);
                    scratchBuffer.DecrementRefCount();
                }
                break;
            case $"{nameof(CALLERATTACHED)}":
            case $"{nameof(CALLERATTCH)}":
            case $"{nameof(CALLERAT)}":
                if (logEntry.CallerContextObject is { } obj) sb.Append(obj);
                break;
        }
    }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .AddBaseRevealStateFields(this).Complete();

    public override string ToString() => this.DefaultToString();
}
