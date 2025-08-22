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

#pragma warning disable SYSLIB0001

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FileSystem;

public interface IFLogFileAppender : IEncodedByteBufferingAppender, IMultiDestinationFormattingAppender
{
    string ConfigPath { get; }

    string ConfigFileName { get; }
}

public interface IMutableFLogFileAppender : IFLogFileAppender, IMutableEncodedByteBufferingAppender
{
    new string ConfigPath { get; set; }

    new string ConfigFileName { get; set; }
}

public class FLogFileAppender : FLogBufferingFormatAppender, IMutableFLogFileAppender
{
    protected readonly IRecycler FileAppenderRecycler = new Recycler();

    protected DateTimePartFlags FullFilePathTimeVariableFlags = DateTimePartFlags.None;

    private string combinedFullConfigFilePath;

    private Encoding? fileEncoding;
    private string   configPath;
    private string   configFileName;

    public FLogFileAppender(IFileAppenderConfig fileAppenderConfig, IFLogContext context) : base(fileAppenderConfig, context, false)
    {
        configPath         = fileAppenderConfig.FilePath;
        configFileName     = fileAppenderConfig.FileName;
        FileEncoding       = GetEncoding(fileAppenderConfig.FileEncoding);
        ExpiryToCloseDelay = TimeSpan.FromMilliseconds(fileAppenderConfig.CloseDelayMs);

        combinedFullConfigFilePath = Path.Combine(configPath, configFileName);

        FormatWriterRequestCache = new MultiDestLogEntryNamedFormatWriterRequestCache().Initialize(this, context);
    }

    public LogEntryPathResolver PathResolver =>
        FileAppenderRecycler.Borrow<LogEntryPathResolver>().Initialize(FileAppenderRecycler, combinedFullConfigFilePath);

    public string ConfigPath
    {
        get => configPath;
        set
        {
            if (configPath == value) return;
            configPath                 = value;
            combinedFullConfigFilePath = Path.Combine(configPath, ConfigFileName);
        }
    }

    public string ConfigFileName
    {
        get => configFileName;
        set
        {
            if (configFileName == value) return;
            configFileName             = value;
            combinedFullConfigFilePath = Path.Combine(configPath, configFileName);
        }
    }

    public TimeSpan ExpiryToCloseDelay { get; private set; }

    public Encoder FileEncoder { get; private set; } = null!;

    public Encoding FileEncoding
    {
        get => fileEncoding ??= Encoding.UTF8; 
        set
        {
            if (fileEncoding?.EncodingName == value.EncodingName) return;
            fileEncoding = value;
            FileEncoder  = fileEncoding.GetEncoder();
        }
    }

    protected Encoding GetEncoding(FileEncodingTypes fileEncodingType)
    {
        switch (fileEncodingType)
        {
            case FileEncodingTypes.Utf7:    return Encoding.UTF7;
            case FileEncodingTypes.Utf16:   return Encoding.Unicode;
            case FileEncodingTypes.Utf16Be: return Encoding.BigEndianUnicode;
            case FileEncodingTypes.Utf32:   return Encoding.UTF32;
            case FileEncodingTypes.Ascii:   return Encoding.ASCII;
            case FileEncodingTypes.Latin1:  return Encoding.Latin1;

            case FileEncodingTypes.Utf8:
            default: return Encoding.UTF8;
        }
    }

    public void ReceiveNotificationTargetClose(string targetNameClosed)
    {
        // to do compression if selected.
    }

    public override FormattingAppenderSinkType FormatAppenderType => FormattingAppenderSinkType.File;

    public SingleDestBufferedFormatWriterRequestCache GetWriterRequestCache(string targetDestination) =>
        new SingleDestBufferedFormatWriterRequestCache().Initialize(this, FLogContext.Context, targetDestination);
    
    public override IBufferedFormatWriter CreateBufferedFormatWriter(IBufferFlushingFormatWriter bufferFlushingFormatWriter, string targetName
      , int bufferNum, FormatWriterReceivedHandler<IFormatWriter> writeCompleteHandler) => 
        new EncodedByteBufferFormatWriter().Initialize(this, bufferFlushingFormatWriter, targetName, bufferNum, writeCompleteHandler);

    protected override IFormatWriter CreatedAppenderDirectFormatWriter
        (IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback) =>
        new FileFormatWriter().Initialize(this, context, targetName, onWriteCompleteCallback);

    private class FileFormatWriter : ByteBufferFlushingFormatWriter<FileFormatWriter>
    {
        private readonly object syncLock = new();
        private FLogFileAppender FileAppender => (FLogFileAppender)OwningAppender;
        private IStream? AppendDestinationStream { get; set; }

        public FileFormatWriter Initialize(FLogFileAppender owningAppender, IFLogContext context, string fullFileName
          , FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
        {
            base.Initialize(owningAppender, fullFileName, onWriteCompleteCallback);

            IsIOSynchronous = true;

            directWriteStagingFileEncoder = owningAppender.FileEncoder;
            try
            {
                var fs = context.FLogFileSystemController;
                fs.EnsureDirExists(fullFileName);
                AppendDestinationStream = fs.OpenOrCreateAppendWriteStream(fullFileName);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine($"Caught exception while trying to open FLogFileAppender file '{fullFileName}'.  Got {e}");
                AppendDestinationStream = new StreamWrapper(Console.OpenStandardOutput());
            }

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
            AppendDestinationStream!.Write(byteRange.ByteBuffer, 0, byteRange.Length);
        }

        public override void Append(StringBuilder toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            directWriteStagingBuffer!.Clear();
            directWriteStagingBuffer.Add(directWriteStagingFileEncoder, toWrite, fromIndex, length);
            var byteRange = directWriteStagingBuffer.AsByteArrayRange;
            AppendDestinationStream!.Write(byteRange.ByteBuffer, 0, byteRange.Length);
        }

        public override void Append(ReadOnlySpan<char> toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            directWriteStagingBuffer!.Clear();
            directWriteStagingBuffer.Add(directWriteStagingFileEncoder, toWrite, fromIndex, length);
            var byteRange = directWriteStagingBuffer.AsByteArrayRange;
            AppendDestinationStream!.Write(byteRange.ByteBuffer, 0, byteRange.Length);
        }

        public override void Append(char[] toWrite, int fromIndex = 0, int length = int.MaxValue)
        {
            directWriteStagingBuffer!.Clear();
            directWriteStagingBuffer.Add(directWriteStagingFileEncoder, toWrite, fromIndex, length);
            var byteRange = directWriteStagingBuffer.AsByteArrayRange;
            AppendDestinationStream!.Write(byteRange.ByteBuffer, 0, byteRange.Length);
        }

        public override void NotifyEntryAppendComplete()
        {
            FileAppender.IncrementLogEntriesProcessed();
            directWriteStagingBuffer!.Clear();
            AppendDestinationStream!.Flush();
        }

        public override void FlushBufferToAppender(IEncodedByteArrayBufferedFormatWriter toFlush)
        {
            var bufferAndRange = toFlush.FlushRange();
            lock (syncLock)
            {
                AppendDestinationStream!.Write(bufferAndRange.ByteBuffer, 0, bufferAndRange.Length);
                AppendDestinationStream.Flush();   
            }
            FileAppender.IncrementLogEntriesProcessed(toFlush.BufferedFormattedLogEntries);
            toFlush.Clear();
            FileAppender.FormatWriterRequestCache.TryToReturnUsedFormatWriter(toFlush);
        }

        public override void StateReset()
        {
            if (AppendDestinationStream != null)
            {
                AppendDestinationStream.Flush();
                AppendDestinationStream.Close();
                AppendDestinationStream = null;
            }
            base.StateReset();
        }
    }

    public override IFileAppenderConfig GetAppenderConfig() => (IFileAppenderConfig)AppenderConfig;
}
