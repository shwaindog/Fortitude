using System.Globalization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConditionalFormattingCommands;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;
using static FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout.FLogEntryLayoutTokens;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface ITokenisedLogEntryFormatStringParser
{
    List<ITemplatePart> BuildTemplateParts(string tokenisedFormatString, List<ITemplatePart>? toPopulate = null
                                                                        , ITokenFormattingValidator? tokenFormattingValidator = null);

    IEnumerable<ITemplatePart> GetAppenderTypeSubTokens(string tokenFormatting);

    List<ITemplatePart> EnsureConsoleColorsReset(List<ITemplatePart> checkIsReset);
}

public class TokenisedLogEntryFormatStringParser : ITokenisedLogEntryFormatStringParser
{
    private static ITokenisedLogEntryFormatStringParser? singletonInstance;

    private static readonly object SyncLock = new();

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

    static readonly (string Open, string Close) TokenReplaceDelimiters = ("{", "}");

    private const string NewLine    = $"{{{nameof(NEWLINE)}}}";
    private const string Nl         = $"{{{nameof(NL)}}}";
    private const string UnixNewLine = $"{{{nameof(UNIXNEWLINE)}}}";
    private const string UnixNl     = $"{{{nameof(UNIXNL)}}}";
    private const string WindowsNewLine = $"{{{nameof(WINDOWSNEWLINE)}}}";
    private const string WindowsNl     = $"{{{nameof(WINDOWSNL)}}}";
    private const string WinNewLine = $"{{{nameof(WINNEWLINE)}}}";
    private const string WinNl     = $"{{{nameof(WINNL)}}}";
    private const string Comma      = $"{{{nameof(COMMA)}}}";
    private const string Co          = $"{{{nameof(C)}}}";
    private const string CommaSpace = $"{{{nameof(COMMASPACE)}}}";
    private const string Cs         = $"{{{nameof(CS)}}}";

    public List<ITemplatePart> BuildTemplateParts(string tokenisedFormatString, List<ITemplatePart>? toPopulate = null
      , ITokenFormattingValidator? tokenFormattingValidator = null)
    {
        toPopulate ??= new List<ITemplatePart>();

        var stringParts = tokenisedFormatString.TokenSplit(IFLogEntryFormatter.TokenDelimiters, TokenReplaceDelimiters);
        foreach (var part in stringParts)
        {
            switch (part)
            {
                case Nl:
                case NewLine:
                    toPopulate.Add(new StringConstantTemplatePart(Environment.NewLine));
                    break;
                case UnixNewLine:
                case UnixNl:
                    toPopulate.Add(new StringConstantTemplatePart("\n"));
                    break;
                case WindowsNewLine:
                case WinNewLine:
                case WindowsNl:
                case WinNl:
                    toPopulate.Add(new StringConstantTemplatePart("\n\r"));
                    break;
                case Co:
                case Comma:
                    toPopulate.Add(new StringConstantTemplatePart(","));
                    break;
                case Cs:
                case CommaSpace:
                    toPopulate.Add(new StringConstantTemplatePart(", "));
                    break;
                default:
                    if (part[0] == '{' && part[^1] == '}')
                    {
                        var tokenFormattingReplace = new TokenFormatting(part, tokenFormattingValidator);
                        toPopulate.Add(tokenFormattingReplace.TokenName.IsEnvironmentTokenName()
                                       ? new EnvironmentDataTemplatePart(tokenFormattingReplace)
                                       : new LogEntryDataTemplatePart(tokenFormattingReplace));
                    }
                    else
                    {
                        toPopulate.Add(new StringConstantTemplatePart(part));
                    }
                    break;
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
        var paddingInt      = 0;
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
        var commandOrFormatting = new String(subTokenFormatting);
        var templateParams      = new String(padding);
        if (identifierName.IsLogEntryDatumTokenName())
        {
            yield return new LogEntryDataTemplatePart(identifierName, paddingInt, capStringLength, commandOrFormatting);
        } else if (identifierName.IsEnvironmentTokenName())
        {
            yield return new EnvironmentDataTemplatePart(identifierName, paddingInt, capStringLength, commandOrFormatting);
        } else if (identifierName.IsScopedFormattingTokenName())
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
                    case ConsoleBackgroundColorTemplatePart:
                    case ConsoleTextColorTemplatePart:
                    case ConsoleLogLevelTextColorMatchTemplatePart:
                    case ConsoleLogLevelBackgroundColorMatchTemplatePart:
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
                case ConsoleBackgroundColorTemplatePart:
                case ConsoleTextColorTemplatePart:
                case ConsoleLogLevelTextColorMatchTemplatePart:
                case ConsoleLogLevelBackgroundColorMatchTemplatePart:
                    isDefaultColors = false;
                    break;
                case ConsoleColorResetTemplatePart: isDefaultColors = true; break;
            }
        }
        if (!isDefaultColors)
        {
            checkIsReset.Add(new ConsoleColorResetTemplatePart(""));
        }
        return checkIsReset;
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
            case $"{nameof(FC)}":
                return new ConsoleTextColorTemplatePart(templateParams);
            case $"{nameof(BACKGROUNDCOLOR)}":
            case $"{nameof(BACKCOLOR)}":
            case $"{nameof(BCOLOR)}":
            case $"{nameof(BC)}":
                return new ConsoleBackgroundColorTemplatePart(templateParams);
            case $"{nameof(COLOR_MATCH)}":
            case $"{nameof(FOREGROUNDCOLOR_MATCH)}":
            case $"{nameof(FORECOLOR_MATCH)}":
            case $"{nameof(FCOLOR_MATCH)}":
            case $"{nameof(FCM)}":
                return new ConsoleLogLevelTextColorMatchTemplatePart(templateParams);
            case $"{nameof(BACKGROUNDCOLOR_MATCH)}":
            case $"{nameof(BACKCOLOR_MATCH)}":
            case $"{nameof(BCOLOR_MATCH)}":
            case $"{nameof(BCM)}":
                return new ConsoleLogLevelBackgroundColorMatchTemplatePart(templateParams);
            case $"{nameof(RESETCOLOR)}": 
            case $"{nameof(RC)}": 
                return new ConsoleColorResetTemplatePart(templateParams);
            
            default: return null;
        }
    }
}
