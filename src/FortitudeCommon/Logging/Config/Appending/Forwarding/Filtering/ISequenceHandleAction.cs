using FortitudeCommon.DataStructures.Maps;

namespace FortitudeCommon.Logging.Config.Appending.Forwarding.Filtering;


public enum TriggeringLogEntries
{
    None
  , All
  , First
  , Last
}

public interface ISequenceHandleAction
{
    ILogMessageTemplateConfig? SendMessage { get; }

    IAppenderReferenceConfig? SendToAppender { get; }

    TriggeringLogEntries SendTriggeringLogEntries { get; }
}