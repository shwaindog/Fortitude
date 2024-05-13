using FortitudeCommon.OSWrapper.Memory;
using FortitudeCommon.Types;

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

public class BucketFactory<T> where T : IBucket, new()
{
    private static readonly Func<ShiftableMemoryMappedFileView, long, bool, T> ParamConstructor = 
        ReflectionHelper.CtorBinder<ShiftableMemoryMappedFileView, long, bool, T>();

    private int prefixMargin = 32;
    private int prefixPadding = 21;
    private int suffixMargin = 32;
    private int suffixPadding = 1024;

    public bool NoPatternOrPadding { get; set; }
    public byte[] BucketPrefixPattern { get; set; } = [ 0xB, 0xA, 0xD, 0xB, 0x0, 0x0, 0xB, 0x5, 0xD, 0xA, 0xD];
    public byte[] BucketSuffixPattern { get; set; } = [ 0xD, 0xA, 0xD, 0xB, 0x0, 0x0, 0xB, 0x5, 0xB, 0xA, 0xD];
    public int PrefixMargin
    {
        get => !NoPatternOrPadding ? prefixMargin : 0;
        set => prefixMargin = value;
    }

    public int PrefixPadding
    {
        get => !NoPatternOrPadding ? prefixPadding : 0;
        set => prefixPadding = value;
    }

    public int SuffixMargin
    {
        get => !NoPatternOrPadding ? suffixMargin : 0;
        set => suffixMargin = value;
    }

    public int SuffixPadding 
    {
        get => !NoPatternOrPadding ? suffixPadding : 0;
        set => suffixPadding = value;
    }
    public long BucketInfoStartFileOffset => PrefixMargin + BucketPrefixPattern.Length + PrefixPadding;
    public long LastEntryBucketEndFileOffset => SuffixPadding + BucketSuffixPattern.Length + SuffixMargin;

    public unsafe T CreateNewBucket(ShiftableMemoryMappedFileView bucketMappedFileView,
        long createStartingAt, bool isWritable, out long bucketLocation)
    {
        var currentFileOffset = createStartingAt;
        var ptr = bucketMappedFileView.FileCursorBufferPointer(createStartingAt);
        if (!NoPatternOrPadding)
        {
            for (var i = currentFileOffset; i < createStartingAt + PrefixMargin; i++)
            {
                *ptr++ = 0;
            }
            currentFileOffset += PrefixMargin;
            var prefixPattern = BucketPrefixPattern;
            for (var i = 0; i < prefixPattern.Length; i++)
            {
                *ptr++ = prefixPattern[i];
            }
            currentFileOffset += prefixPattern.Length;
            for (var i = currentFileOffset; i < createStartingAt + PrefixPadding; i++)
            {
                *ptr++ = 0;
            }
            currentFileOffset += PrefixPadding;
        }
        bucketLocation = currentFileOffset;
        return ParamConstructor(bucketMappedFileView, currentFileOffset, isWritable);
    }

    public T OpenExistingBucket(ShiftableMemoryMappedFileView bucketMappedFileView,
        long bucketFileCursorOffset, bool isWritable)
    {
        return ParamConstructor(bucketMappedFileView, bucketFileCursorOffset, isWritable);
    }
}
