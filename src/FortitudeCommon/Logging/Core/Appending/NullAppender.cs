using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending;

public class NullAppender(INullAppenderConfig appenderDefinitionConfig, IFLogContext context) 
    : FLogAppender(appenderDefinitionConfig, context)
{
    public override void Append(IReusableList<IFLogEntry> batchFLogEntries) { }

    public override void Append(IFLogEntry logEntry) { }

    public override INullAppenderConfig GetAppenderConfig() => (INullAppenderConfig)AppenderConfig;
}