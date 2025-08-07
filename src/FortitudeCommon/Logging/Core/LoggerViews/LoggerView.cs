// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LoggerViews;

public interface ILoggerView : ISwitchFLoggerView
{
    IFLogger Logger { get; }
}

public class LoggerView : ILoggerView
{
    public LoggerView(IFLogger logger) => Logger = logger;

    public T As<T>() where T : ISwitchFLoggerView
    {
        return Logger.As<T>();
    }

    public IFLogger Logger { get; set; }
}
