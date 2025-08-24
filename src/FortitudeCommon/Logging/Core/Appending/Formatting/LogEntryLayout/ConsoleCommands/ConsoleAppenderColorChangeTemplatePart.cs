namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout.ConsoleCommands;

public abstract class ConsoleAppenderColorChangeTemplatePart : AppenderCommandTemplatePart
{
    
    public bool WasScopeClosed { get; set; }
    
    
    protected ConsoleAppenderColorChangeTemplatePart(FormattingAppenderSinkType targetingAppenderType, string command)
        : base(targetingAppenderType, command)
    {
        
    }
}
