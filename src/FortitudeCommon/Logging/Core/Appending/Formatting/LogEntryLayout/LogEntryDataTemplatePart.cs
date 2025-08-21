// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using static FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout.FLogEntryLayoutTokens;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public class LogEntryDataTemplatePart : ITemplatePart, IStyledToStringObject
{
    [ThreadStatic] protected static StringBuilder? scratchBuffer;
    
    private TokenFormatting tokenFormatting;

    public LogEntryDataTemplatePart(string tokenName, int paddingLength, int maxLength, string formattingString)
    {
        tokenFormatting = new TokenFormatting(tokenName, paddingLength, maxLength, formattingString);
    }

    public LogEntryDataTemplatePart(TokenFormatting tokenFormatting) 
        
    {
        this.tokenFormatting = tokenFormatting;
    }
    
    
    public int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        scratchBuffer ??= new();
        scratchBuffer.Clear();
        ApplyTokenToStringBuilder(scratchBuffer, logEntry);
        if (scratchBuffer.Length > tokenFormatting.MaxLength)
        {
            scratchBuffer.Length = tokenFormatting.MaxLength;
        }
        formatWriter.Append(scratchBuffer);
        return scratchBuffer.Length;
    }
    
    public FormattingAppenderSinkType TargetingAppenderTypes => FormattingAppenderSinkType.Any;

    // example toke "%TS:yyyy-MM-dd HH:mm:SS.fff% %LVL,5% %THREADID,4% %THREADNAME,10[..10]% %LGR% %MSG%";
    protected virtual void ApplyTokenToStringBuilder(StringBuilder sb, IFLogEntry logEntry)
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
                sb.AppendFormat(tokenFormatting.FormatString, logEntry.Thread.Name);
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
                    scratchBuffer ??= new();
                    scratchBuffer.Clear();
                    scratchBuffer.Append(excep);
                    scratchBuffer.Replace("\r", "");
                    scratchBuffer.Replace("\n", ",");
                    sb.AppendRange(scratchBuffer);
                }
                break;
            case $"{nameof(CALLERATTACHED)}":
            case $"{nameof(CALLERATTCH)}":
            case $"{nameof(CALLERAT)}":
                if (logEntry.CallerContextObject is { } obj) sb.Append(obj);
                break;
        }
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        var tb =
            sbc.StartComplexType(nameof(LogEntryDataTemplatePart))
               .AddBaseFieldsStart();
        return tb.Complete();
    }

    public override string ToString() => this.DefaultToString();
}
