using System.Globalization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConditionalFormattingCommands;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;
using static FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout.FLogEntryLayoutTokens;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface ITokenisedLogEntryFormatStringParser
{
    List<ITemplatePart> BuildTemplateParts(string tokenisedFormatString, List<ITemplatePart>? toPopulate = null);

    IEnumerable<ITemplatePart> GetAppenderTypeSubTokens(string tokenFormatting);

    List<ITemplatePart> EnsureConsoleColorsReset(List<ITemplatePart> checkIsReset);
}

public class TokenisedLogEntryFormatStringParser : ITokenisedLogEntryFormatStringParser
{
    private static ITokenisedLogEntryFormatStringParser? singletonInstance;

    private static readonly object SyncLock = new ();

    public static ITokenisedLogEntryFormatStringParser Instance
    {
        get
        {
            if (singletonInstance == null)
            {
                lock (SyncLock)
                {
                    if (singletonInstance == null)
                    {
                        singletonInstance = new TokenisedLogEntryFormatStringParser();
                    }
                }
            }
            return singletonInstance;
        }
    }

    static readonly string[] TokenReplaceDelimiters = ["{", "}"];

    public List<ITemplatePart> BuildTemplateParts(string tokenisedFormatString, List<ITemplatePart>? toPopulate = null)
    {
        toPopulate ??= new List<ITemplatePart>();

        var stringParts    = tokenisedFormatString.TokenSplit(IFLogEntryFormatter.TokenDelimiters, TokenReplaceDelimiters);
        foreach (var part in stringParts)
        {
            if (part[0] == '{' && part[^1] == '}')
            {
                toPopulate.Add(new LogEntryDataTemplatePart(part));
            }
            else
            {
                toPopulate.Add(new StringConstantTemplatePart(part));
            }
        }
        return toPopulate;
    }

    public IEnumerable<ITemplatePart> GetAppenderTypeSubTokens(string tokenFormatting)
    {
        var tokenSpan = tokenFormatting.AsSpan();
        tokenSpan.ExtractStringFormatStages(out var identifier, out var padding, out var subTokenFormatting);
        Span<char> upperParam = stackalloc char[identifier.Length];
        identifier.ToUpper(upperParam, CultureInfo.InvariantCulture);
        var identifierName = new String(upperParam);

        var capStringLength = int.MaxValue;
        var paddingInt = 0;
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
            paddingInt = padding.ExtractInt() ?? 0;
        }
        var commandOrFormatting     =  new String(subTokenFormatting);
        var templateParams =  new String(padding);
        if (IsLogEntryDataToken(identifierName))
        {
            yield return new LogEntryDataTemplatePart(identifierName, paddingInt, capStringLength, commandOrFormatting);
        }
        if (IsScopedFormattingToken(identifierName))
        {
            var scopedTemplatePart = SourceScopedFormattingTemplatePart(identifierName, templateParams, commandOrFormatting);
            if (scopedTemplatePart != null)
            {
                yield return scopedTemplatePart;
            }
            if (commandOrFormatting.Length > 0)
            {
                switch (scopedTemplatePart)
                {
                    case ConsoleBackgroundColorTemplatePart :
                    case ConsoleTextColorTemplatePart :
                    case ConsoleLogLevelTextColorMatchTemplatePart :
                    case ConsoleLogLevelBackgroundColorMatchTemplatePart :
                        foreach (var scopedPart in BuildTemplateParts(commandOrFormatting))
                        {
                            yield return scopedPart;
                        }
                        break;
                }
                yield return new ConsoleColorResetTemplatePart("");
            }
        }
    }

    public List<ITemplatePart> EnsureConsoleColorsReset(List<ITemplatePart> checkIsReset)
    {
        var isDefaultColors = true;
        foreach (var templatePart in checkIsReset)
        {
            switch (templatePart)
            {
                case ConsoleBackgroundColorTemplatePart :
                case ConsoleTextColorTemplatePart :
                case ConsoleLogLevelTextColorMatchTemplatePart :
                case ConsoleLogLevelBackgroundColorMatchTemplatePart :
                    isDefaultColors = false;
                    break;
                case ConsoleColorResetTemplatePart :
                    isDefaultColors = true;
                    break;
            }
        }
        if (!isDefaultColors)
        {
            checkIsReset.Add(new ConsoleColorResetTemplatePart(""));
        }
        return checkIsReset;
    }

    protected virtual bool IsLogEntryDataToken(string tokenName)
    {
        switch (tokenName)
        {
            case $"{nameof(DATETIME)}":
            case $"{nameof(DATE)}":
            case $"{nameof(TIME)}":
            case $"{nameof(TS)}":
            case $"{nameof(LOGLEVEL)}":
            case $"{nameof(LEVEL)}":
            case $"{nameof(LVL)}":
            case $"{nameof(CORRELATIONID)}":
            case $"{nameof(CID)}":
            case $"{nameof(THREADID)}":
            case $"{nameof(TID)}":
            case $"{nameof(THREADNAME)}":
            case $"{nameof(TNAME)}":
            case $"{nameof(LOGGERNAME)}":
            case $"{nameof(LOGGER)}":
            case $"{nameof(LGRNAME)}":
            case $"{nameof(LGR)}":
            case $"{nameof(MESSAGE)}":
            case $"{nameof(MESG)}":
            case $"{nameof(MSG)}":
            case $"{nameof(EXCEPTION)}":
            case $"{nameof(EXCEP)}":
            case $"{nameof(EX)}":
            case $"{nameof(EXCEPTION_ONELINE)}":
            case $"{nameof(EXCEP_ONELINE)}":
            case $"{nameof(EX_ONELINE)}":
            case $"{nameof(EXCEPTION_1L)}":
            case $"{nameof(EXCEP_1L)}":
            case $"{nameof(EX_1L)}":
            case $"{nameof(CALLERATTACHED)}":
            case $"{nameof(CALLERATTCH)}":
            case $"{nameof(CALLERAT)}":
                return true;
        }
        return false;
    }

    protected virtual bool IsScopedFormattingToken(string tokenName)
    {
        switch (tokenName)
        {
            case $"{nameof(LOGLEVEL_COND)}":
            case $"{nameof(LEVEL_COND)}":
            case $"{nameof(LVL_COND)}":
            case $"{nameof(EXCEPTION_COND)}":
            case $"{nameof(EXCEP_COND)}":
            case $"{nameof(EX_COND)}":
            case $"{nameof(COLOR)}":
            case $"{nameof(FOREGROUNDCOLOR)}":
            case $"{nameof(FORECOLOR)}":
            case $"{nameof(FCOLOR)}":
            case $"{nameof(BACKGROUNDCOLOR)}":
            case $"{nameof(BACKCOLOR)}":
            case $"{nameof(BCOLOR)}":
            case $"{nameof(COLOR_MATCH)}":
            case $"{nameof(FOREGROUNDCOLOR_MATCH)}":
            case $"{nameof(FORECOLOR_MATCH)}":
            case $"{nameof(FCOLOR_MATCH)}":
            case $"{nameof(BACKGROUNDCOLOR_MATCH)}":
            case $"{nameof(BACKCOLOR_MATCH)}":
            case $"{nameof(BCOLOR_MATCH)}":
            case $"{nameof(RESETCOLOR)}":
                return true;
        }
        return false;
    }

    protected virtual ITemplatePart? SourceScopedFormattingTemplatePart(string tokenName, string templateParams, string command)
    {
        switch (tokenName)
        {
            case $"{nameof(LOGLEVEL_COND)}":
            case $"{nameof(LEVEL_COND)}":
            case $"{nameof(LVL_COND)}":
                var logLevel = Enum.TryParse<FLogLevel>(templateParams, out var flogLevel) ? flogLevel : FLogLevel.None;
                return new LogLevelConditionalFormatTemplatePart(logLevel, command);
            case $"{nameof(EXCEPTION_COND)}":
            case $"{nameof(EXCEP_COND)}":
            case $"{nameof(EX_COND)}":
                return new ExceptionConditionalFormatTemplatePart(templateParams, command);
            case $"{nameof(COLOR)}":
            case $"{nameof(FOREGROUNDCOLOR)}":
            case $"{nameof(FORECOLOR)}":
            case $"{nameof(FCOLOR)}":
                return new ConsoleTextColorTemplatePart(templateParams);
            case $"{nameof(BACKGROUNDCOLOR)}":
            case $"{nameof(BACKCOLOR)}":
            case $"{nameof(BCOLOR)}":
                return new ConsoleBackgroundColorTemplatePart(templateParams);
            case $"{nameof(COLOR_MATCH)}":
            case $"{nameof(FOREGROUNDCOLOR_MATCH)}":
            case $"{nameof(FORECOLOR_MATCH)}":
            case $"{nameof(FCOLOR_MATCH)}":
                return new ConsoleLogLevelTextColorMatchTemplatePart(templateParams);
            case $"{nameof(BACKGROUNDCOLOR_MATCH)}":
            case $"{nameof(BACKCOLOR_MATCH)}":
            case $"{nameof(BCOLOR_MATCH)}":
                return new ConsoleLogLevelBackgroundColorMatchTemplatePart(templateParams);
            case $"{nameof(RESETCOLOR)}":
                return new ConsoleColorResetTemplatePart(templateParams);
                default: return null;
        }
    }
}