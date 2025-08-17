using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface IFormatWriterResolver
{
    bool IsOpen { get; }
    IBlockingFormatWriterResolverHandle FormatWriterResolver(IFLogEntry logEntry);

    void Close();
}

public class CharSpanReturningLogEntryFormatter : RecyclableObject, IFormatWriter, IFormatWriterResolver, IBlockingFormatWriterResolverHandle
{
    private bool isDisposed;
    private bool wasTaken;

    private IFLogEntry? requestingLogEntry;
    
    public CharSpanReturningLogEntryFormatter Initialize(int minCharSize, IFLogEntry? logEntry = null)
    {
        isDisposed = false;
        wasTaken   = false;

        if (minCharSize > (Buffer?.Length ?? 0))
        {
            Buffer?.DecrementRefCount();
            Buffer = minCharSize.SourceRecyclingCharArray();
        }
        requestingLogEntry = logEntry;

        return this;
    }

    private RecyclingCharArray? Buffer { get; set; }

    public void Clear() => Buffer?.Clear();

    public string TargetName => $"{nameof(CharSpanReturningLogEntryFormatter)}";

    public Span<char> AsSpan
    {
        get
        {
            var arraySpan = Buffer!.WrittenAsSpan();
            return arraySpan[..Buffer.Length];
        }
    }

    public IBlockingFormatWriterResolverHandle FormatWriterResolver(IFLogEntry logEntry)
    {
        RequestingLogEntry = logEntry;
        Buffer!.Clear();
        return this;
    }

    public IFLogEntry RequestingLogEntry
    {
        get => requestingLogEntry!;
        private set => requestingLogEntry = value;
    }

    public void Dispose()
    {
        if (!isDisposed)
        {
            isDisposed = true;
        }
    }

    IFLogFormattingAppender IFormatWriter.OwningAppender => null!;

    public bool IsIOSynchronous => false;

    public bool InUse { get; set; }

    public bool NotifyStartEntryAppend(IFLogEntry forEntry) => true;

    public void Append(string toWrite)
    {
        Buffer!.Add(toWrite);
    }

    public void Append(StringBuilder toWrite, int fromIndex = 0, int length = Int32.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = Int32.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public void Append(char[] toWrite, int fromIndex = 0, int length = Int32.MaxValue)
    {
        Buffer!.Add(toWrite, fromIndex, length);
    }

    public void NotifyEntryAppendComplete() { }

    public bool IsDisposed => isDisposed;
    public bool IsAvailable => true;
    public bool WasTaken => wasTaken;

    public bool TryGetFormatWriter(out IFormatWriter? formatWriter)
    {
        formatWriter = this;
        return true;
    }

    public IFormatWriter GetOrWaitForFormatWriter(int timeout = 2000)
    {
        return this;
    }

    public void ReceiveFormatWriterHandler(IFormatWriter writer) { }

    public void IssueRequestAborted()
    {
    }

    public bool IsOpen => true;

    public void Close()
    {
        Buffer?.Clear();
    }

    public override void StateReset()
    {
        Dispose();
        Buffer?.Clear();
        base.StateReset();
    }
}
