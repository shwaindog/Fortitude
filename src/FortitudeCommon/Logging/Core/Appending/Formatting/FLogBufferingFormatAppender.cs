// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Formatting;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;
using FortitudeCommon.Logging.Core.Hub;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IFLogBufferingFormatAppender : IFLogFormattingAppender
{
    bool BufferingEnabled { get; }

    int CharBufferSize { get; }
}

public interface IMutableFLogBufferingFormatAppender : IFLogBufferingFormatAppender, IMutableFLogFormattingAppender
{
    new bool BufferingEnabled { get; set; }

    bool UsingDoubleBuffering { get; }

    new int CharBufferSize { get; set; }

    IBufferFlushAppenderAsyncClient BufferFlushingAsyncClient { get; }
    
    IBufferedFormatWriter CreateBufferedFormatWriter(IBufferFlushingFormatWriter bufferFlushingFormatWriter, string targetName, int bufferNum, FormatWriterReceivedHandler<IFormatWriter> writeCompleteHandler);
    
    new IBufferingFormatAppenderConfig GetAppenderConfig();
    new IBufferFlushingFormatWriter CreatedDirectFormatWriter(IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback);
}

public abstract class FLogBufferingFormatAppender : FLogFormattingAppender, IMutableFLogBufferingFormatAppender
{
    // protected IBufferedFormatWriter? ToFlush;

    protected FLogBufferingFormatAppender(IBufferingFormatAppenderConfig bufferingFormatAppenderConfig, IFLogContext context)
        : base(bufferingFormatAppenderConfig, context)
    {
        CharBufferSize       = bufferingFormatAppenderConfig.CharBufferSize;
        UsingDoubleBuffering = bufferingFormatAppenderConfig.EnableDoubleBufferToggling;
        
        FlushWhenBufferLength = (int)(bufferingFormatAppenderConfig.FlushConfig.WriteTriggeredAtBufferPercentage *
                                      bufferingFormatAppenderConfig.CharBufferSize);
    }

    public int CharBufferSize { get; set; }

    public bool UsingDoubleBuffering { get; set; }

    public int FlushWhenBufferLength { get; set; }

    protected override IFormatWriterRequestCache CreateFormatWriterRequestCache
        (IFormattingAppenderConfig formattingAppenderConfig, IFLogContext context) => 
        new SingleDestBufferedFormatWriterRequestCache().Initialize(this, context);

    public IBufferedFormatWriter CreateBufferedFormatWriter(IBufferFlushingFormatWriter bufferFlushingFormatWriter, string targetName
      , int bufferNum, FormatWriterReceivedHandler<IFormatWriter> writeCompleteHandler) => 
        new CharArrayBufferedFormatWriter().Initialize(this, bufferFlushingFormatWriter, targetName, bufferNum, writeCompleteHandler);

    IBufferFlushAppenderAsyncClient IMutableFLogBufferingFormatAppender.BufferFlushingAsyncClient => (IBufferFlushAppenderAsyncClient)AsyncClient;

    public bool BufferingEnabled
    {
        get => ((IBufferedFormatWriterRequestCache)FormatWriterRequestCache).BufferingEnabled;
        set => ((IBufferedFormatWriterRequestCache)FormatWriterRequestCache).BufferingEnabled = value;
    }

    protected override IBufferFlushAppenderAsyncClient CreateAppenderAsyncClient
        (IAppenderDefinitionConfig appenderDefinitionConfig, IFLoggerAsyncRegistry asyncRegistry)
    {
        var processAsync = appenderDefinitionConfig.RunOnAsyncQueueNumber;

        var bufferConfig = (IBufferingFormatAppenderConfig)appenderDefinitionConfig;

        var bufferAsync = bufferConfig.FlushAsyncQueueNumber;

        var bufferFlushingAsyncClient =
            new BufferFlushAppenderAsyncClient(this, processAsync, asyncRegistry, bufferAsync);
        return bufferFlushingAsyncClient;
    }
    
    IBufferFlushingFormatWriter IMutableFLogBufferingFormatAppender.CreatedDirectFormatWriter
        (IFLogContext context, string targetName, FormatWriterReceivedHandler<IFormatWriter> onWriteCompleteCallback)
    {
        return (IBufferFlushingFormatWriter)CreatedAppenderDirectFormatWriter(context, targetName, onWriteCompleteCallback);
    }

    public override IBufferingFormatAppenderConfig GetAppenderConfig() => (IBufferingFormatAppenderConfig)AppenderConfig;
}
