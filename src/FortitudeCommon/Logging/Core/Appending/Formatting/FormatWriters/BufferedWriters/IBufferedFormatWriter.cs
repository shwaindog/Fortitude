namespace FortitudeCommon.Logging.Core.Appending.Formatting.FormatWriters.BufferedWriters;

public interface IBufferedFormatWriter : IFormatWriter
{
    int BufferNum { get; }

    int BufferCharCapacity { get; }

    int BufferRemainingCharCapacity { get; }

    int BufferedChars { get; }

    void Flush();

    void Clear();
}
