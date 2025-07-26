// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core.LogEntries;
using static FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout.FLogEntryLayoutTokens;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public class LogEntryDataTemplatePart : ITemplatePart
{
    [ThreadStatic] private static StringBuilder? scratchBuffer;

    private readonly string stringBuilderFormatting;

    private readonly int capStringLength = int.MaxValue;


    public LogEntryDataTemplatePart(string tokenName, int paddingLength, int capStringLength, string formattingString)
    {
        TokenName       = tokenName;
        this.capStringLength = capStringLength;
        stringBuilderFormatting = 0.BuildStringBuilderFormatting(paddingLength, formattingString);
    }

    public LogEntryDataTemplatePart(string tokenFormatting)
    {
        var tokenSpan = tokenFormatting.AsSpan();
        tokenSpan.ExtractStringFormatStages(out var param, out var padding, out var formatting);
        Span<char> upperParam = stackalloc char[param.Length];
        param.ToUpper(upperParam, CultureInfo.InvariantCulture);
        TokenName = new String(upperParam);

        if (padding.Length > 0)
        {
            var foundCapStringLength = padding.IndexOf("[..");
            if (foundCapStringLength >= 0)
            {
                var digitsSlice = padding.ExtractDigitsSlice(foundCapStringLength + 3);
                if (digitsSlice.Length > 0)
                {
                    capStringLength = int.TryParse(digitsSlice, out var attempt) ? attempt : int.MaxValue;
                }
                padding = padding.Slice(0, foundCapStringLength);
            }
        }
        var zeroPosParam = "0".AsSpan();
        stringBuilderFormatting = zeroPosParam.BuildStringBuilderFormatting(padding, formatting);
    }

    public String TokenName { get; private set; }

    public FormattingAppenderSinkType TargetingAppenderTypes => FormattingAppenderSinkType.Any;

    public int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        scratchBuffer ??= new();
        scratchBuffer.Clear();
        ApplyTokenToStringBuilder(scratchBuffer, logEntry);
        if (scratchBuffer.Length > capStringLength)
        {
            scratchBuffer.Length = capStringLength;
        }
        formatWriter.Append(scratchBuffer);
        return scratchBuffer.Length;
    }

    // example toke "%TS:yyyy-MM-dd HH:mm:SS.fff% %LVL,5% %THREADID,4% %THREADNAME,10[..10]% %LGR% %MSG%";
    protected virtual void ApplyTokenToStringBuilder(StringBuilder sb, IFLogEntry logEntry)
    {
        switch (TokenName)
        {
            case $"{nameof(DATETIME)}":
            case $"{nameof(DATE)}":
            case $"{nameof(TIME)}":
            case $"{nameof(TS)}":
                sb.AppendFormat(stringBuilderFormatting, logEntry.LogDateTime);
                break;
            case $"{nameof(LOGLEVEL)}":
            case $"{nameof(LEVEL)}":
            case $"{nameof(LVL)}":
                sb.AppendFormat(stringBuilderFormatting, logEntry.LogLevel);
                break;
            case $"{nameof(CORRELATIONID)}":
            case $"{nameof(CID)}":
                sb.AppendFormat(stringBuilderFormatting, logEntry.CorrelationId);
                break;
            case $"{nameof(THREADID)}":
            case $"{nameof(TID)}":
                sb.AppendFormat(stringBuilderFormatting, logEntry.Thread.ManagedThreadId);
                break;
            case $"{nameof(THREADNAME)}":
            case $"{nameof(TNAME)}":
                sb.AppendFormat(stringBuilderFormatting, logEntry.Thread.Name);
                break;
            case $"{nameof(LOGGERNAME)}":
            case $"{nameof(LOGGER)}":
            case $"{nameof(LGRNAME)}":
            case $"{nameof(LGR)}":
                sb.AppendFormat(stringBuilderFormatting, logEntry.Logger.FullName);
                break;
            case $"{nameof(MESSAGE)}":
            case $"{nameof(MESG)}":
            case $"{nameof(MSG)}":
                sb.AppendRange(logEntry.Message);
                break;
            case $"{nameof(EXCEPTION)}":
            case $"{nameof(EXCEP)}":
            case $"{nameof(EX)}":
                if(logEntry.Exception is {} ex)
                    sb.Append(ex);
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
                if(logEntry.CallerContextObject is {} obj)
                    sb.Append(obj);
                break;
        }
    }
}
