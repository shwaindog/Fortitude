using FortitudeCommon.OSWrapper.Memory;
using FortitudeCommon.Types;

namespace FortitudeIO.TimeSeries.FileSystem.File.Buckets;

public class BucketFactory<TBucket> where TBucket : class, IBucketNavigation<TBucket>, IMutableBucket
{
    private static readonly Func<IBucketTrackingTimeSeriesFile, long, bool, TBucket> ParamConstructor = 
        ReflectionHelper.CtorBinder<IBucketTrackingTimeSeriesFile, long, bool, TBucket>();
    
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
    public long BucketInfoStartFileOffset => PrefixMargin + BucketPrefixPattern.Length + PrefixPadding;
    public long LastEntryBucketEndFileOffset => SuffixPadding + BucketSuffixPattern.Length + SuffixMargin;

    public unsafe TBucket CreateNewBucket(IBucketTrackingTimeSeriesFile containingFile,
        long createStartingAtFileCursorOffset, DateTime containingTime, bool isWritable, IMutableSubBucketContainerBucket? parentBucket = null)
    {
        var currentFileOffset = createStartingAtFileCursorOffset;
        var ptr = containingFile.ActiveBucketAppenderFileView.FileCursorBufferPointer(createStartingAtFileCursorOffset, isWritable);
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
        var newBucket = ParamConstructor(containingFile, currentFileOffset, isWritable);
        newBucket.BucketFactory = this;
        newBucket.InitializeNewBucket(containingTime, parentBucket);
        return newBucket;
    }

    public unsafe long AppendCloseBucketDelimiter(ShiftableMemoryMappedFileView bucketView, long fileCursorPosition)
    {
        var currentFileOffset = fileCursorPosition;
        if (!NoPatternOrPadding)
        {
            var ptr = bucketView.FileCursorBufferPointer(fileCursorPosition, true);
            
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

    public TBucket OpenExistingBucket(IBucketTrackingTimeSeriesFile containingFile,
        long bucketFileCursorOffset, bool isWritable)
    {
        return ParamConstructor(containingFile, bucketFileCursorOffset, isWritable);
    }
}
