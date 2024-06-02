using FortitudeCommon.DataStructures.Memory.UnmanagedMemory.MemoryMappedFiles;
using FortitudeCommon.Types;

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

public interface IBucketFactory<out TBucket> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket
{
    bool   IsFileRootBucketType { get; }
    bool   NoPatternOrPadding   { get; set; }
    byte[] BucketPrefixPattern  { get; set; }

    byte[] BucketSuffixPattern { get; set; }

    int PrefixMargin  { get; set; }
    int PrefixPadding { get; set; }
    int SuffixMargin  { get; set; }
    int SuffixPadding { get; set; }

    TBucket CreateNewBucket(IMutableBucketContainer bucketContainer,
        long createStartingAtFileCursorOffset, DateTime containingTime, bool isWritable);

    long AppendCloseBucketDelimiter(ShiftableMemoryMappedFileView bucketView, long fileCursorPosition);

    TBucket OpenExistingBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool isWritable, ShiftableMemoryMappedFileView? alternativeMappedFileView = null);
}


public class BucketFactory<TBucket> : IBucketFactory<TBucket> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket
{
    private static readonly Func<IMutableBucketContainer, long, bool, ShiftableMemoryMappedFileView?, TBucket> ParamConstructor = 
        ReflectionHelper.CtorBinder<IMutableBucketContainer, long, bool, ShiftableMemoryMappedFileView?, TBucket>();
    
    private int prefixMargin = 32;
    private int prefixPadding = 21;
    private int suffixMargin = 32;
    private int suffixPadding = 1024;

    public BucketFactory(bool isFileRootBucketType = false)
    {
        this.IsFileRootBucketType = isFileRootBucketType;
    }

    public bool IsFileRootBucketType { get; }

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

    public unsafe TBucket CreateNewBucket(IMutableBucketContainer bucketContainer,
        long createStartingAtFileCursorOffset, DateTime containingTime, bool isWritable)
    {
        var currentFileOffset = createStartingAtFileCursorOffset;
        var ptr = bucketContainer.ContainingSession.ActiveBucketDataFileView.FileCursorBufferPointer(createStartingAtFileCursorOffset, 0, isWritable);
        if (!NoPatternOrPadding)
        {
            for (var i = currentFileOffset; i < PrefixMargin; i++)
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
            for (var i = currentFileOffset; i < PrefixPadding; i++)
            {
                *ptr++ = 0;
            }
            currentFileOffset += PrefixPadding;
        }
        var newBucket = NewBucketObject(bucketContainer, currentFileOffset, isWritable);
        newBucket.BucketFactory = this;
        newBucket.InitializeNewBucket(containingTime);
        return newBucket;
    }

    protected virtual TBucket NewBucketObject(IMutableBucketContainer bucketContainer,
        long currentFileOffset, bool isWritable, ShiftableMemoryMappedFileView? alternativeMappedFileView = null)
    {
        return ParamConstructor(bucketContainer, currentFileOffset, isWritable, null);
    }

    public unsafe long AppendCloseBucketDelimiter(ShiftableMemoryMappedFileView bucketView, long fileCursorPosition)
    {
        var currentFileOffset = fileCursorPosition;
        if (!NoPatternOrPadding)
        {
            var ptr = bucketView.FileCursorBufferPointer(fileCursorPosition, 0, true);
            
            for (var i = currentFileOffset; i < SuffixPadding; i++)
            {
                *ptr++ = 0;
            }
            currentFileOffset += SuffixPadding;
            var suffixPattern = BucketSuffixPattern;
            for (var i = 0; i < suffixPattern.Length; i++)
            {
                *ptr++ = suffixPattern[i];
            }
            currentFileOffset += suffixPattern.Length;
            for (var i = currentFileOffset; i < SuffixMargin; i++)
            {
                *ptr++ = 0;
            }
            currentFileOffset += SuffixMargin;
        }
        return currentFileOffset;
    }

    public TBucket OpenExistingBucket(IMutableBucketContainer bucketContainer,
        long bucketFileCursorOffset, bool isWritable, ShiftableMemoryMappedFileView? alternativeMappedFileView = null)
    {
        return NewBucketObject(bucketContainer, bucketFileCursorOffset, isWritable, alternativeMappedFileView);
    }
}
