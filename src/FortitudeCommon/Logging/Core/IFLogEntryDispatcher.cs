// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core;

public interface IFLogEntryDispatcher
{
    void SendToAppenders(IFLogEntry logEntry);
}


public interface IFLogEntrySourceSink : IFLogEntryDispatcher, IFLogEntryFactory
{
}
