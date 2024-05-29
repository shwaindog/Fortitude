// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using FortitudeCommon.OSWrapper.Streams;

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.ByteStreams;

public interface IInWindow
{
    IByteArray BufferBase { get; }
    uint BufferOffset { get; }
    uint Pos { get; }
    uint StreamPos { get; }
    void MoveBlock();
    void ReadBlock();
    void Create(uint keepSizeBefore, uint keepSizeAfter, uint keepSizeReserv);
    void SetStream(IStream stream);
    void ReleaseStream();
    void Init();
    void MovePos();
    byte GetIndexByte(int index);
    uint GetMatchLen(int index, uint distance, uint limit);
    uint GetNumAvailableBytes();
    void ReduceOffsets(int subValue);
}