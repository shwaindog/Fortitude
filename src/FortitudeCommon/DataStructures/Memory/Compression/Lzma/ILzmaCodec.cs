// This file was taken from 7-zip.org/sdk.html
// LZMA SDK is placed in the public domain.
// all credit and thanks to Igor Pavlov, Abraham Lempel and Jacob Ziv and thanks

namespace FortitudeCommon.DataStructures.Memory.Compression.Lzma;

/// <summary>
///     The exception that is thrown when an error in input stream occurs during decoding.
/// </summary>
internal class DataErrorException : ApplicationException
{
    public DataErrorException() : base("Data Error") { }
}

/// <summary>
///     The exception that is thrown when the value of an argument is outside the allowable range.
/// </summary>
internal class InvalidParamException : ApplicationException
{
    public InvalidParamException() : base("Invalid Parameter") { }
}

public interface ICodecProgress
{
    /// <summary>
    ///     Callback progress.
    /// </summary>
    /// <param name="inSize">
    ///     input size. -1 if unknown.
    /// </param>
    /// <param name="outSize">
    ///     output size. -1 if unknown.
    /// </param>
    void SetProgress(long inSize, long outSize);
};

public struct ByteStream
{
    public ByteStream(Stream stream) => Stream = stream;
    public ByteStream(IByteArray byteArray) => ByteArray = byteArray;
    public Stream?     Stream    { get; }
    public IByteArray? ByteArray { get; }
}

public struct LzmaEncoderParams
{
    public LzmaEncoderParams()
    {
        DictionarySize = 1 << 23;
        PosStateBits   = 2;
        LitContextBits = 3;
        LitPosBits     = 0;
        Algorithm      = 2;
        NumFastBytes   = 128;
        MatchFinder    = "bt4";
        HasEOS         = false;
        InputSize      = -1;
        OutputSize     = -1;
    }

    public int    DictionarySize { get; set; }
    public int    PosStateBits   { get; set; }
    public int    LitContextBits { get; set; }
    public int    LitPosBits     { get; set; }
    public int    Algorithm      { get; set; }
    public int    NumFastBytes   { get; set; }
    public string MatchFinder    { get; set; }
    public bool   HasEOS         { get; set; }
    public long   InputSize      { get; set; }
    public long   OutputSize     { get; set; }

    public ByteStream?     TrainStream    { get; set; }
    public ICodecProgress? CodecProgress  { get; set; }
}

/*
public interface ICoder2
{
     void Code(ISequentialInStream []inStreams,
            const UInt64 []inSizes,
            ISequentialOutStream []outStreams,
            UInt64 []outSizes,
            ICodeProgress progress);
};
*/

/// <summary>
///     Provides the fields that represent properties idenitifiers for compressing.
/// </summary>
public enum CoderPropID
{
    /// <summary>
    ///     Specifies default property.
    /// </summary>
    DefaultProp = 0

    /// <summary>
    ///     Specifies size of dictionary.
    /// </summary>
  , DictionarySize

    /// <summary>
    ///     Specifies size of memory for PPM*.
    /// </summary>
  , UsedMemorySize

    /// <summary>
    ///     Specifies order for PPM methods.
    /// </summary>
  , Order

    /// <summary>
    ///     Specifies Block Size.
    /// </summary>
  , BlockSize

    /// <summary>
    ///     Specifies number of postion state bits for LZMA (0 <= x <= 4).
    /// </summary>
  , PosStateBits

    /// <summary>
    ///     Specifies number of literal context bits for LZMA (0 <= x <= 8).
    /// </summary>
  , LitContextBits

    /// <summary>
    ///     Specifies number of literal position bits for LZMA (0 <= x <= 4).
    /// </summary>
  , LitPosBits

    /// <summary>
    ///     Specifies number of fast bytes for LZ*.
    /// </summary>
  , NumFastBytes

    /// <summary>
    ///     Specifies match finder. LZMA: "BT2", "BT4" or "BT4B".
    /// </summary>
  , MatchFinder

    /// <summary>
    ///     Specifies the number of match finder cyckes.
    /// </summary>
  , MatchFinderCycles

    /// <summary>
    ///     Specifies number of passes.
    /// </summary>
  , NumPasses

    /// <summary>
    ///     Specifies number of algorithm.
    /// </summary>
  , Algorithm

    /// <summary>
    ///     Specifies the number of threads.
    /// </summary>
  , NumThreads

    /// <summary>
    ///     Specifies mode with end marker.
    /// </summary>
  , EndMarker
};

public interface ILzmaEncoder
{
    void Compress(LzmaEncoderParams encoderParams, ByteStream inStream, ByteStream outStream);
}

public interface ILzmaDecoder
{
    void Decompress(ByteStream inStream, ByteStream outStream, ICodecProgress? progress = null);
    void SetDecoderProperties(byte[] properties);
}
