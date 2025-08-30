// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.RequestsCache;

public interface IFormatWriterRequestCache : IFormatWriterResolver
{
    int FormatWriterRequestQueue { get; }

    void TryToReturnUsedFormatWriter(IFormatWriter toReturn);
}
