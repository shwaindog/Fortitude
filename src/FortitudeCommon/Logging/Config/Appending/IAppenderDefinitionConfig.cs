// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Config.Appending;

public interface IAppenderDefinitionConfig : IAppenderReferenceConfig
{
    const string DefaultStringFormattingTemplate = "%TS:yyyy-MM-dd HH:mm:SS.fff% %LVL,5% %THREADID,4% %THREADNAME,10[..10]% %LGR% %MSG%";

    IAppenderReferenceConfig? InheritingParentReference { get; }

    bool IsTemplateOnlyDefinition { get; }

    new string AppenderName { get; }

    int RunOnAsyncQueueNumber { get; }

    FloggerAppenderType AppenderType { get; }

    string LogEntryFormatLayout { get; }
}

public interface IMutableAppenderDefinitionConfig : IAppenderDefinitionConfig, IMutableAppenderReferenceConfig
{
    new string  AppenderName           { get; set; }

    new bool IsTemplateOnlyDefinition { get; set; }

    new FloggerAppenderType AppenderType { get; set; }

    new int RunOnAsyncQueueNumber { get; set; }

    new string LogEntryFormatLayout { get; set; }
}