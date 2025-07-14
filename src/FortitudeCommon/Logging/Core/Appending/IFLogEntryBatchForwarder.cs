// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending;

public interface IFLogEntryBatchForwarder : Core.IFLogEntryForwarder
{
    BatchForwardLogEntry AppenderBatchForwardCallBack { get; }

    void BatchForwardTo(IReusableList<IFLogEntry> batchLogEntries);
}
