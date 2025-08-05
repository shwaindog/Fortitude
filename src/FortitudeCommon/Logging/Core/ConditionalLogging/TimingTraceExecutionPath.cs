// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Extensions;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.ConditionalLogging;

public class TimingTraceExecutionPath : RecyclableObject
{
    private readonly ExecutionTimingStart traceStarted;
    private readonly IMutableFLogEntry    finalLogEntry;

    public ExecutionDuration? ExecutionDuration { get; protected set; }

    private static readonly IStringBuilder IncompleteTraceEntry = new MutableString();

    static TimingTraceExecutionPath()
    {
        IncompleteTraceEntry.Append("Trace did not complete last trace entry");
    }

    private readonly TraceMessageLogEntry traceLogEntry;

    public TimingTraceExecutionPath(ExecutionTimingStart traceStarted, IMutableFLogEntry finalLogEntry)
    {
        this.traceStarted  = traceStarted;
        this.finalLogEntry = finalLogEntry;

        traceLogEntry = new TraceMessageLogEntry(this.finalLogEntry);
    }

    public FLogEntry AddTraceEntry => traceLogEntry;

    public LoggingLocation LoggingLocation => finalLogEntry.LogLocation;

    public ExecutionDuration StopGetDuration()
    {
        ExecutionDuration ??= traceStarted.StopClock(finalLogEntry.Logger);
        return ExecutionDuration.Value;
    }

    internal void Dispatch(IStringBuilder? warningToPrefix = null)
    {
        if (traceLogEntry.HasPartial)
        {
            traceLogEntry.OnMessageComplete(IncompleteTraceEntry);
        }

        finalLogEntry.OnMessageComplete(warningToPrefix);
    }

    private class TraceMessageLogEntry : FLogEntry
    {
        private readonly StringBuilder finalEntrySb;

        private readonly StringBuilder myTraceEntrySb;

        public TraceMessageLogEntry(IMutableFLogEntry finalLogEntry)
        {
            finalEntrySb   = ((MutableString)(finalLogEntry.Message)).BackingStringBuilder;
            myTraceEntrySb = ((MutableString)(Message)).BackingStringBuilder;
        }

        public bool HasPartial => myTraceEntrySb.Length > 0;

        protected override void SendToDispatchHandler(IFLogEntry me)
        {
            finalEntrySb.AppendFormat("{0:mm:ss.ffffff}", me.DispatchedAt).Append(" - ");
            finalEntrySb.AppendRange(myTraceEntrySb);
            myTraceEntrySb.Clear();
            DispatchedAt = null;
        }
    }
}
