// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.LoggerViews;

public delegate T CreateFLoggerView<out T>(IFLogger fLogger) where T : ISwitchFLoggerView;

public interface ISwitchFLoggerView
{
    T As<T>() where T : ISwitchFLoggerView;
}
