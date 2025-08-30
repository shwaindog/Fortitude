// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;

public interface IBufferedFormatWriter : IFormatWriter
{
    int BufferNum { get; }

    int BufferCharCapacity { get; }

    int BufferRemainingCharCapacity { get; }

    int BufferedChars { get; }
    uint BufferedFormattedLogEntries { get; }

    void Flush();

    void Clear();
}
