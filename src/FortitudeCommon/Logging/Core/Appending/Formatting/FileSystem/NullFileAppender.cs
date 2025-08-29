using System.Text;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.DataStructures.Memory.Buffers.ByteBuffers;
using FortitudeCommon.Logging.Config.Appending.Formatting.Files;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.OSWrapper.Streams;
using FortitudeCommon.Types.Mutable.Strings;

#pragma warning disable SYSLIB0001

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FileSystem;

public class NullFileAppender(IFileAppenderConfig fileAppenderConfig, IFLogContext context) : FLogFileAppender(fileAppenderConfig, context)
{
    public override void ReceiveNotificationTargetClose(string targetNameClosed) { }
    
    protected override IFormatWriter CreatedAppenderDirectFormatWriter
        (IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback) =>
        new NullFileFormatWriter().Initialize(this, context, targetName, onWriteCompleteCallback);

    private class NullFileFormatWriter : ByteBufferFlushingFormatWriter<NullFileFormatWriter>
    {
        private readonly object syncLock = new();

        private RecyclingByteArray? dummyFileBuffer;
        private NullFileAppender FileAppender => (NullFileAppender)OwningAppender;

        public NullFileFormatWriter Initialize(NullFileAppender owningAppender, IFLogContext context, string fullFileName
          , FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
        {
            base.Initialize(owningAppender, fullFileName, onWriteCompleteCallback);
            dummyFileBuffer = (64 * 1024).SourceRecyclingByteArray();

            IsIOSynchronous = true;

            directWriteStagingFileEncoder = owningAppender.FileEncoder;

            return this;
        }

        private RecyclingByteArray? directWriteStagingBuffer;

        private Encoder directWriteStagingFileEncoder = null!;

        public override bool NotifyStartEntryAppend(IFLogEntry forEntry)
        {
            if ((directWriteStagingBuffer?.Capacity ?? 0) < (3 * forEntry.Message.Length) + 256)
            {
                directWriteStagingBuffer?.DecrementRefCount();
                directWriteStagingBuffer = (forEntry.Message.Length * 4 + 256).SourceRecyclingByteArray();
            }
            return true;
        }

        public override void Append(string toWrite)
        {
            directWriteStagingBuffer!.Clear();
            directWriteStagingBuffer.Add(directWriteStagingFileEncoder, toWrite);
            var byteRange = directWriteStagingBuffer.AsByteArrayRange;
            dummyFileBuffer!.Add(byteRange.ByteBuffer, 0, byteRange.Length);
            dummyFileBuffer.Count = 0;
        }

        public override void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
            var cappedLength = Math.Clamp(length, 0, toWrite.Length);
            directWriteStagingBuffer!.Clear();
            directWriteStagingBuffer.Add(directWriteStagingFileEncoder, toWrite, cappedFrom, cappedLength);
            var byteRange = directWriteStagingBuffer.AsByteArrayRange;
            dummyFileBuffer!.Add(byteRange.ByteBuffer, 0, byteRange.Length);
            dummyFileBuffer.Count = 0;
        }

        public override void Append(ICharSequence toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            if (toWrite is CharArrayStringBuilder charArrayBuilder)
            {
                var writtenCharArrayRange = charArrayBuilder.AsCharArrayRange;
                var cappedFrom            = Math.Clamp(fromIndex, writtenCharArrayRange.FromIndex, writtenCharArrayRange.Length - 1);
                var cappedLength          = Math.Clamp(length, 0, writtenCharArrayRange.Length);
                directWriteStagingBuffer!.Clear();
                directWriteStagingBuffer.Add(directWriteStagingFileEncoder, writtenCharArrayRange.CharBuffer, cappedFrom, cappedLength);
                var byteRange = directWriteStagingBuffer.AsByteArrayRange;

                dummyFileBuffer!.Add(byteRange.ByteBuffer, 0, byteRange.Length);
                dummyFileBuffer.Count = 0;
            }
            else if (toWrite is MutableString mutableString)
            {
                var sb = mutableString.BackingStringBuilder;
                Append(sb, fromIndex, length);
            }
            else
            {
                var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
                var cappedLength = Math.Clamp(length, 0, toWrite.Length);
                var charArray    = cappedLength.SourceRecyclingCharArray();
                charArray.Add(toWrite, cappedFrom, cappedLength);
                directWriteStagingBuffer!.Clear();
                directWriteStagingBuffer.Add(directWriteStagingFileEncoder, charArray.BackingArray, 0, cappedLength);
                var byteRange = directWriteStagingBuffer.AsByteArrayRange;
                dummyFileBuffer!.Add(byteRange.ByteBuffer, 0, byteRange.Length);
                dummyFileBuffer.Count = 0;
                charArray.DecrementRefCount();
            }
        }

        public override void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
            var cappedLength = Math.Clamp(length, 0, toWrite.Length);
            directWriteStagingBuffer!.Clear();
            directWriteStagingBuffer.Add(directWriteStagingFileEncoder, toWrite, cappedFrom, cappedLength);
            var byteRange = directWriteStagingBuffer.AsByteArrayRange;
            dummyFileBuffer!.Add(byteRange.ByteBuffer, 0, byteRange.Length);
            dummyFileBuffer.Count = 0;
        }

        public override void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            var cappedFrom   = Math.Clamp(fromIndex, 0, toWrite.Length - 1);
            var cappedLength = Math.Clamp(length, 0, toWrite.Length);
            directWriteStagingBuffer!.Clear();
            directWriteStagingBuffer.Add(directWriteStagingFileEncoder, toWrite, cappedFrom, cappedLength);
            var byteRange = directWriteStagingBuffer.AsByteArrayRange;
            dummyFileBuffer!.Add(byteRange.ByteBuffer, 0, byteRange.Length);
            dummyFileBuffer.Count = 0;
        }

        public override void NotifyEntryAppendComplete()
        {
            FileAppender.IncrementLogEntriesProcessed();
            directWriteStagingBuffer!.Clear();
        }

        public override void FlushBufferToAppender(IEncodedByteArrayBufferedFormatWriter toFlush)
        {
            var bufferAndRange = toFlush.FlushRange();
            lock (syncLock)
            {
                dummyFileBuffer!.Add(bufferAndRange.ByteBuffer, 0, bufferAndRange.Length);
                dummyFileBuffer.Count = 0;
            }
            FileAppender.IncrementLogEntriesProcessed(toFlush.BufferedFormattedLogEntries);
            toFlush.Clear();
            FileAppender.FormatWriterRequestCache.TryToReturnUsedFormatWriter(toFlush);
        }

        public override void StateReset()
        {
            if (dummyFileBuffer != null)
            {
                dummyFileBuffer.DecrementRefCount();
                dummyFileBuffer = null!;
            }
            base.StateReset();
        }
    }

    public override IFileAppenderConfig GetAppenderConfig() => (IFileAppenderConfig)AppenderConfig;
}
