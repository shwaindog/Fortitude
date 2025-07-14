// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public class TokenKeyTemplatePart : ITemplatePart
{
    [ThreadStatic] private static StringBuilder? scratchBuffer;

    private readonly string sbTokenFormatting;

    private readonly int capStringLength = int.MaxValue;

    public TokenKeyTemplatePart(string tokenFormatting)
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
        sbTokenFormatting = zeroPosParam.BuildStringBuilderFormatting(padding, formatting);
    }

    public String TokenName { get; }

    public void Apply(StringBuilder sb, IFLogEntry logEntry)
    {
        if (capStringLength != int.MaxValue)
        {
            ApplyTokenToStringBuilder(sb, logEntry);
        }
        else
        {
            scratchBuffer ??= new();
            scratchBuffer.Clear();
            ApplyTokenToStringBuilder(scratchBuffer, logEntry);
            if (scratchBuffer.Length > capStringLength)
            {
                scratchBuffer.Length = capStringLength;
            }
            sb.AppendRange(scratchBuffer);
        }
    }

    // example toke "%TS:yyyy-MM-dd HH:mm:SS.fff% %LVL,5% %THREADID,4% %THREADNAME,10[..10]% %LGR% %MSG%";
    protected virtual void ApplyTokenToStringBuilder(StringBuilder sb, IFLogEntry logEntry)
    {
        switch (TokenName)
        {
            case "DATETIME":
            case "DATE":
            case "TIME":
            case "TS":
                sb.AppendFormat(sbTokenFormatting, logEntry.LogDateTime);
                break;
            case "LOGLEVEL":
            case "LEVEL":
            case "LVL":
                sb.AppendFormat(sbTokenFormatting, logEntry.LogLevel);
                break;
            case "THREADID":
            case "TID":
                sb.AppendFormat(sbTokenFormatting, logEntry.Thread.ManagedThreadId);
                break;
            case "THREADNAME":
            case "TNAME":
                sb.AppendFormat(sbTokenFormatting, logEntry.Thread.Name);
                break;
            case "LOGGERNAME":
            case "LOGGER":
            case "LGRNAME":
            case "LGR":
                sb.AppendFormat(sbTokenFormatting, logEntry.Logger.FullName);
                break;
            case "MESSAGE":
            case "MESG":
            case "MSG":
                sb.AppendRange(logEntry.Message);
                break;
        }
    }
}
