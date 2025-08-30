// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface IFormatWriterResolver
{
    bool IsOpen { get; }
    IBlockingFormatWriterResolverHandle FormatWriterResolver(IFLogEntry logEntry);

    void Close();
}

public class CharSpanReturningLogEntryFormatter : RecyclableObject, IFormatWriter, IFormatWriterResolver, IBlockingFormatWriterResolverHandle
{
    private IFLogEntry? requestingLogEntry;

    public CharSpanReturningLogEntryFormatter Initialize(int minCharSize, IFLogEntry? logEntry = null)
    {
        IsDisposed = false;
        WasTaken   = false;

        if (minCharSize > (Buffer?.Length ?? 0))
        {
            Buffer?.DecrementRefCount();
            Buffer = minCharSize.SourceRecyclingCharArray();
        }
        requestingLogEntry = logEntry;

        return this;
    }

    private RecyclingCharArray? Buffer { get; set; }

    public Span<char> AsSpan
    {
        get
        {
            var arraySpan = Buffer!.WrittenAsSpan();
            return arraySpan[..Buffer.Length];
        }
    }

    public IFLogEntry RequestingLogEntry
    {
        get => requestingLogEntry!;
        private set => requestingLogEntry = value;
    }

    public bool IsDisposed { get; private set; }
    public bool IsAvailable => true;
    public bool WasTaken { get; private set; }

    public bool TryGetFormatWriter(out IFormatWriter? formatWriter)
    {
        formatWriter = this;
        return true;
    }

    public IFormatWriter GetOrWaitForFormatWriter(int timeout = 2000) => this;

    public void ReceiveFormatWriterHandler(IFormatWriter writer) { }

    public void IssueRequestAborted() { }

    public string TargetName => $"{nameof(CharSpanReturningLogEntryFormatter)}";

    public void Dispose()
    {
        if (!IsDisposed) IsDisposed = true;
    }

    IFLogFormattingAppender IFormatWriter.OwningAppender => null!;

    public bool IsIOSynchronous => false;

    public bool InUse { get; set; }

    public bool NotifyStartEntryAppend(IFLogEntry forEntry) => true;

    public void Append(string toWrite)
    {
        Buffer!.Add(toWrite);
    }

    public void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public void Append(ICharSequence toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public void NotifyEntryAppendComplete() { }

    public IBlockingFormatWriterResolverHandle FormatWriterResolver(IFLogEntry logEntry)
    {
        RequestingLogEntry = logEntry;
        Buffer!.Clear();
        return this;
    }

    public bool IsOpen => true;

    public void Close()
    {
        Buffer?.Clear();
    }

    public void Clear() => Buffer?.Clear();

    public override void StateReset()
    {
        Dispose();
        Buffer?.Clear();
        base.StateReset();
    }
}
