// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IFormatWriter : IDisposable
{
    IFLogFormattingAppender OwningAppender { get; }

    bool IsIOSynchronous { get; }
    bool InUse           { get; }
    
    void NotifyStartEntryAppend();
    void Append(string toWrite);
    void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue);
    void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue);
    void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue);
    void NotifyEntryAppendComplete();
}

public abstract class FormatWriter<T> : IFormatWriter where T : IFormatWriter
{
    protected FormatWriterReceivedHandler<T> OnWriteCompleteCallback;

    protected FormatWriter(IMutableFLogFormattingAppender owningAppender, FormatWriterReceivedHandler<T> onWriteCompleteCallback)
    {
        OwningAppender          = owningAppender;
        OnWriteCompleteCallback = onWriteCompleteCallback;
    }

    public IFLogFormattingAppender OwningAppender { get; }

    public bool InUse { get; set; }

    public abstract bool IsIOSynchronous { get; }

    public abstract void Append(string toWrite);

    public abstract void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue);

    public abstract void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue);

    public abstract void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue);

    public virtual void NotifyStartEntryAppend() { }

    public virtual void NotifyEntryAppendComplete()    { }

    public virtual void Dispose()
    {
        InUse = false;
        OnWriteCompleteCallback((T)(IFormatWriter)this);
    }
}
