using System.Text;
using FortitudeCommon.Logging.Core.Appending.Formatting.FileSystem;
using FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

namespace FortitudeCommon.Logging.Core.Appending.Formatting;

public interface IMultiDestinationFormattingAppender : IMutableFLogBufferingFormatAppender
{
    LogEntryPathResolver PathResolver { get; }
    
    TimeSpan ExpiryToCloseDelay { get; }

    SingleDestBufferedFormatWriterRequestCache GetWriterRequestCache(string targetDestination);

    void ReceiveNotificationTargetClose(string targetNameClosed);

    void IncrementLogEntriesDropped(uint byAmount = 1);
}

public interface IEncodedByteBufferingAppender : IFLogBufferingFormatAppender
{
    Encoding FileEncoding { get; }

    Encoder FileEncoder { get; }
}

public interface IMutableEncodedByteBufferingAppender : IEncodedByteBufferingAppender, IMutableFLogBufferingFormatAppender
{
    new Encoding FileEncoding { get; set; }
}