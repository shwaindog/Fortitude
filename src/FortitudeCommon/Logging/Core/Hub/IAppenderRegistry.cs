// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Pooling;
using NLog;

namespace FortitudeCommon.Logging.Core.Hub;


public delegate void HandleAppenderConfigUpdate(IAppenderDefinitionConfig appenderConfig);

public delegate void NotifyAppenderHandler(IFLoggerAppender appender);

public delegate void NotifyNewAppenderHandler(IMutableFLoggerAppender appender);

public record AppenderContainer
{
    public AppenderContainer(IMutableFLoggerAppender Appender)
    {
        this.Appender = Appender;

        ConfigUpdated += Appender.HandleConfigUpdate;
    }

    public event HandleAppenderConfigUpdate? ConfigUpdated;

    public IFLogEntryForwarder Appender { get; init; }

    public void OnUpdated(IAppenderDefinitionConfig newConfig)
    {
        ConfigUpdated?.Invoke(newConfig);
    }

    public void Deconstruct(out IFLogEntryForwarder appender)
    {
        appender = Appender;
    }
}

public interface IAppenderRegistry
{
    void RegistryAppenderInterest(NotifyAppenderHandler onRegisteredCallback, string? appenderName,  string? appenderConfigNodePath);

    NotifyNewAppenderHandler RegisterAppenderCallback { get; }

    FLogEntryPoolRegistry AppenderFLogEntryPoolRegistry { get; }
}
