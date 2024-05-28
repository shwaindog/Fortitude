using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma.Compress.Lz;

public interface IOutWindow
{
    uint TrainSize { get; }
    void Create(uint windowSize);
    void Init(Stream stream, bool solid);
    void ReleaseStream();
    void Flush();
    void CopyBlock(uint distance, uint len);
    void PutByte(byte b);
    byte GetByte(uint distance);
}