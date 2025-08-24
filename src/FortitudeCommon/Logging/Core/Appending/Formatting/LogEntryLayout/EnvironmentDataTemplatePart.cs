using System.Collections;
using System.Reflection;
using FortitudeCommon.DataStructures.Maps;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable.Strings;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using static FortitudeCommon.Logging.Config.Appending.Formatting.LogEntryLayout.FLogEntryLayoutTokens;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public class EnvironmentDataTemplatePart : ITemplatePart, IStyledToStringObject
{
    [ThreadStatic] protected static IStringBuilder? ScratchBuffer;

    private CharSpanAcceptingStringMap<string>? envVariables;

    private readonly TokenFormatting tokenFormatting;

    public EnvironmentDataTemplatePart(string tokenName, int paddingLength, int maxLength, string formattingString) => 
        tokenFormatting = new TokenFormatting(tokenName, paddingLength, maxLength, formattingString);

    public EnvironmentDataTemplatePart(TokenFormatting tokenFormatting) => 
        this.tokenFormatting = tokenFormatting;

    private IStringBuilder SourceEphemeralStringBuilder(int charSizeRequired = 512)
    {
        ScratchBuffer ??= charSizeRequired.SourceCharArrayStringBuilder();
        ScratchBuffer.Clear();
        return ScratchBuffer;
    }

    public int Apply(IFormatWriter formatWriter, IFLogEntry logEntry)
    {
        var  formatBuffer = SourceEphemeralStringBuilder();
        ApplyTokenToStringBuilder(formatBuffer, logEntry);
        if (formatBuffer is CharArrayStringBuilder charArrayStringBuilder)
        {
            var writtenCharArrayRange = charArrayStringBuilder.AsCharArrayRange;
            formatWriter.Append(writtenCharArrayRange.CharBuffer, writtenCharArrayRange.FromIndex, writtenCharArrayRange.Length);
        }
        else if (formatBuffer is MutableString mutableString)
        {
            formatWriter.Append(mutableString.BackingStringBuilder, 0, mutableString.Length);
        }
        else
        {
            formatWriter.Append(formatBuffer);
        }
        return formatBuffer.Length;
    }

    public FormattingAppenderSinkType TargetingAppenderTypes => FormattingAppenderSinkType.Any;

    protected virtual void ApplyTokenToStringBuilder(IStringBuilder sb, IFLogEntry logEntry)
    {
        switch (tokenFormatting.TokenName)
        {
            case $"{nameof(STARTASSEMBLYNAME)}":
                sb.Append(Assembly.GetEntryAssembly()?.GetName().Name ?? "UnmanagedLaunched");
                break;
            case $"{nameof(STARTASSEMBLYDIRPATH)}": sb.Append(Assembly.GetEntryAssembly()?.Location ?? "."); break;
            case $"{nameof(HOSTNAME)}":             sb.Append(Environment.MachineName); break;
            case $"{nameof(LOGINDOMAINNAME)}":      sb.Append(Environment.UserDomainName); break;
            case $"{nameof(LOGIN)}":                sb.Append(Environment.UserName); break;
            case $"{nameof(STARTCOMMANDARG0)}":     AppendCommandLineArg(sb, 0); break;
            case $"{nameof(STARTCOMMANDARG1)}":     AppendCommandLineArg(sb, 1); break;
            case $"{nameof(STARTCOMMANDARG2)}":     AppendCommandLineArg(sb, 2); break;
            case $"{nameof(STARTCOMMANDARG3)}":     AppendCommandLineArg(sb, 3); break;
            case $"{nameof(STARTCOMMANDARG4)}":     AppendCommandLineArg(sb, 4); break;
            case $"{nameof(STARTCOMMANDARG5)}":     AppendCommandLineArg(sb, 5); break;
            case $"{nameof(STARTCOMMANDARG6)}":     AppendCommandLineArg(sb, 6); break;
            case $"{nameof(STARTCOMMANDARG7)}":     AppendCommandLineArg(sb, 7); break;
            case $"{nameof(STARTCOMMANDARG8)}":     AppendCommandLineArg(sb, 8); break;
            case $"{nameof(STARTCOMMANDARG9)}":     AppendCommandLineArg(sb, 9); break;
            case $"{nameof(ENV)}":
                var formatAsSpan = tokenFormatting.FormatString.AsSpan();
                var envVarName   = formatAsSpan.ExtractStringFormatStageOnly();
                envVariables ??= BuildExpandedEnvironmentVariables();
                var envVarValue = envVariables.GetValue(envVarName);
                sb.Append(envVarValue ?? "");
                break;
        }
    }

    private static string[]? commandLineArgs;

    private void AppendCommandLineArg(IStringBuilder sb, int argNum)
    {
        commandLineArgs ??= Environment.GetCommandLineArgs();
        if (argNum >= 0 && commandLineArgs.Length > argNum)
        {
            sb.Append(commandLineArgs[argNum]);
        }
    }

    private CharSpanAcceptingStringMap<string> BuildExpandedEnvironmentVariables()
    {
        var allEnvVariablesExpanded = new CharSpanAcceptingStringMap<string>();
        foreach (DictionaryEntry envVar in Environment.GetEnvironmentVariables())
        {
            allEnvVariablesExpanded.TryAdd(envVar.Key.ToString(), Environment.ExpandEnvironmentVariables(envVar.Value?.ToString() ?? ""));
        }
        return allEnvVariablesExpanded;
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartComplexType(nameof(LogEntryDataTemplatePart))
               .Field.AlwaysAdd(nameof(tokenFormatting), tokenFormatting).Complete();
    }

    public override string ToString() => this.DefaultToString();
}
