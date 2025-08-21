using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

public interface IBufferedFormatWriterRequestCache : IFormatWriterRequestCache
{
    bool BufferingEnabled { get; set; }

    void FlushBufferToAppender(IBufferedFormatWriter toFlush);
}
