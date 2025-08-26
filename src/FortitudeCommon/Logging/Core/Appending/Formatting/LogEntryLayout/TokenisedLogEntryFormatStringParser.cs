using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Config;
using FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConditionalFormattingCommands;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;
using static FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout.FLogEntryLayoutTokens;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface ITokenisedLogEntryFormatStringParser
{
    List<ITemplatePart> BuildTemplateParts
    (string tokenisedFormatString, List<ITemplatePart>? toPopulate = null, ITokenFormattingValidator? tokenFormattingValidator = null);

    void GetAppenderTypeSubTokens(string tokenFormatting, List<ITemplatePart> toPopulate
      , ITokenFormattingValidator? tokenFormattingValidator = null);

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
                    singletonInstance ??= new TokenisedLogEntryFormatStringParser();
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
                        GetAppenderTypeSubTokens(part, toPopulate, tokenFormattingValidator);
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

    public void GetAppenderTypeSubTokens(string tokenFormatting, List<ITemplatePart> toPopulate
      , ITokenFormattingValidator? tokenFormattingValidator = null)
    {
        var tokenFormattingReplace = new TokenFormatting(tokenFormatting, tokenFormattingValidator);
        if (tokenFormattingReplace.TokenName.IsLogEntryDatumTokenName())
        {
            toPopulate.Add(new LogEntryDataTemplatePart(tokenFormattingReplace));
        } else if (tokenFormattingReplace.TokenName.IsEnvironmentTokenName())
        {
            toPopulate.Add(new EnvironmentDataTemplatePart(tokenFormattingReplace));
        } else if (tokenFormattingReplace.TokenName.IsScopedFormattingTokenName())
        {
            var scopedTemplatePart = SourceScopedFormattingTemplatePart
                (tokenFormattingReplace.TokenName , tokenFormattingReplace.Layout, tokenFormattingReplace.Format);
            if (scopedTemplatePart != null)
            {
                toPopulate.Add(scopedTemplatePart);
            }
            if (tokenFormattingReplace.Format.Length > 0)
            {
                switch (scopedTemplatePart)
                {
                    case ConsoleBackgroundColorTemplatePart:
                    case ConsoleTextColorTemplatePart:
                    case ConsoleLogLevelTextColorMatchTemplatePart:
                    case ConsoleLogLevelBackgroundColorMatchTemplatePart:
                        BuildTemplateParts(tokenFormattingReplace.Format, toPopulate, tokenFormattingValidator);
                        
                        break;
                }
                if (scopedTemplatePart is ConsoleBackgroundColorTemplatePart colorTemplatePart)
                {
                    colorTemplatePart.WasScopeClosed = true;
                }
                toPopulate.Add(new ConsoleColorResetTemplatePart(""));
            }
        }
        else
        {
            toPopulate.Add(new StringConstantTemplatePart(tokenFormatting));
        }
    }

    public List<ITemplatePart> EnsureConsoleColorsReset(List<ITemplatePart> checkIsReset)
    {
        var isDefaultColors = true;
        foreach (var templatePart in checkIsReset)
        {
            switch (templatePart)
            {
                case ConsoleAppenderColorChangeTemplatePart consoleColorChangeCommand:
                    isDefaultColors = consoleColorChangeCommand.WasScopeClosed;
                    break;
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
