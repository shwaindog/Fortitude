// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;

public interface IFormatWriter : IDisposable
{
    IFLogFormattingAppender OwningAppender { get; }

    bool IsIOSynchronous { get; }
    bool InUse { get; set; }

    string TargetName { get; }

    bool NotifyStartEntryAppend(IFLogEntry forEntry);
    void Append(string toWrite);
    void Append(ICharSequence toWrite, int fromIndex = 0, int length = int.MaxValue);
    void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue);
    void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue);
    void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue);
    void NotifyEntryAppendComplete();
}

public abstract class FormatWriter<T> : RecyclableObject, IFormatWriter where T : IFormatWriter
{
    protected FormatWriterReceivedHandler<T> OnWriteCompleteCallback = null!;

    protected virtual FormatWriter<T> Initialize(IMutableFLogFormattingAppender owningAppender, string targetName
      , FormatWriterReceivedHandler<T> onWriteCompleteCallback)
    {
        TargetName     = targetName;
        OwningAppender = owningAppender;

        OnWriteCompleteCallback = onWriteCompleteCallback;

        return this;
    }

    public IFLogFormattingAppender OwningAppender { get; private set; } = null!;

    public string TargetName { get; private set; } = null!;

    public bool InUse { get; set; }

    public bool IsIOSynchronous { get; protected set; }

    public abstract void Append(string toWrite);

    public abstract void Append(ICharSequence toWrite, int fromIndex = 0, int length = int.MaxValue);

    public abstract void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue);

    public abstract void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue);

    public abstract void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue);

    public virtual bool NotifyStartEntryAppend(IFLogEntry forEntry) => true; // can continue with this writer

    public virtual void NotifyEntryAppendComplete() { }

    public virtual void Dispose()
    {
        if (InUse)
        {
            InUse = false;
            OnWriteCompleteCallback((T)(IFormatWriter)this);
        }
    }

    public override void StateReset()
    {
        OnWriteCompleteCallback = null!;

        OwningAppender = null!;
        TargetName     = null!;

        base.StateReset();
    }
}
