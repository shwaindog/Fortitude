// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.Lz;

internal interface IInWindowStream
{
    void SetStream(ByteStream inStream);
    void Init();
    void ReleaseStream();
    byte GetIndexByte(int index);
    uint GetMatchLen(int index, uint distance, uint limit);
    uint GetNumAvailableBytes();
}

internal interface IMatchFinder : IInWindowStream
{
    void Create(uint historySize, uint keepAddBufferBefore,
        uint matchMaxLen, uint keepAddBufferAfter);

    uint GetMatches(uint[] distances);
    void Process(uint num, IInWindow? contextInput = null);
}
