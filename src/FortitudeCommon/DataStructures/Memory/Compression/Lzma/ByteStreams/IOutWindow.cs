// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.OSWrapper.Streams;

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.ByteStreams;

public interface IOutWindow
{
    uint TrainSize { get; }
    void Create(uint windowSize);
    void Init(IStream stream, bool solid);
    void ReleaseStream();
    void Flush();
    void CopyBlock(uint distance, uint len);
    void PutByte(byte b);
    byte GetByte(uint distance);
}