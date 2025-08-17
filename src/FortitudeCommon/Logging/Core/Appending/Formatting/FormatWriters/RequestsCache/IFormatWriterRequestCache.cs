using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

public interface IFormatWriterRequestCache : IFormatWriterResolver
{
    int FormatWriterRequestQueue { get; }

    void TryToReturnUsedFormatWriter(IFormatWriter toReturn);
}
