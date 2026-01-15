using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public class PassThroughEncodingTransfer : RecyclableObject, IEncodingTransfer
{
    protected const char DblQtChar = '"';
    public string EncodingTransferConfigKey { get; private set; } = nameof(EncodingType.PassThrough);

    public EncodingType Type => EncodingType.PassThrough;

    public static PassThroughEncodingTransfer Instance { get; } = new();

    public int StringValueDelimiter(IStringBuilder sb) => sb.Append(DblQtChar).ReturnCharCount(1);

    public int StringValueDelimiter(Span<char> destSpan, int destStartIndex) => destSpan[destStartIndex].ReturnInt(1);

    public int StringFieldDelimiter(IStringBuilder sb) => sb.Append(DblQtChar).ReturnCharCount(1);

    public int StringFieldDelimiter(Span<char> destSpan, int destStartIndex) => destSpan[destStartIndex].ReturnInt(1);

    public int Transfer(Rune? source, IStringBuilder destSb)
    {
        var preAppendLength = destSb.Length;
        destSb.EnsureCapacity(2);
        destSb.Append(source);
        return destSb.Length - preAppendLength;
    }

    public int Transfer(Rune? source, Span<char> dest, int destIndex)
    {
        if (source is null) return 0;
        source.Value.TryEncodeToUtf16(dest[destIndex..], out var written);
        return written;
    }

    public virtual int TransferPrefix(bool encodePrefix, ReadOnlySpan<char> source, IStringBuilder destSb) => 
        UffixTransfer(source, destSb);

    public virtual int TransferPrefix(bool encodePrefix, ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex) => 
        UffixTransfer(source, destSpan, destStartIndex);

    public virtual int TransferSuffix(ReadOnlySpan<char> source, IStringBuilder destSb, bool encodeSuffix) => 
        UffixTransfer(source, destSb);

    public virtual int TransferSuffix(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex, bool encodeSuffix) => 
        UffixTransfer(source, destSpan, destStartIndex);
    
    
    protected int UffixTransfer(ReadOnlySpan<char> source, IStringBuilder destSb)
    {
        var cappedLength   = source.Length;
        
        destSb.EnsureCapacity(cappedLength);
        var lastCharWasBrcOpn = false;
        var lastCharWasBrcCls = false;
        for (int i = 0; i < cappedLength; i++)
        {
            var charToTransfer = source[i];
            if (charToTransfer.IsBrcOpn())
            {
                if (lastCharWasBrcOpn)
                {
                    lastCharWasBrcOpn = false;
                }
                else
                {
                    lastCharWasBrcOpn = true;
                    destSb.Append(charToTransfer);
                }
            }
            else if (charToTransfer.IsBrcCls())
            {
                if (lastCharWasBrcCls)
                {
                    lastCharWasBrcCls = false;
                }
                else
                {
                    lastCharWasBrcCls = true;
                    destSb.Append(charToTransfer);
                }
            }
            else
            {
                lastCharWasBrcOpn = false;
                lastCharWasBrcCls = false;
                destSb.Append(charToTransfer);
            } 
        }
        return cappedLength;
    }
    
    protected int UffixTransfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex)
    {
        var cappedLength =  source.Length;
        var to           = destStartIndex;
        
        var lastCharWasBrcOpn = false;
        var lastCharWasBrcCls = false;
        for (int i = 0; i < cappedLength; i++)
        {
            var charToTransfer = source[i];
            if (charToTransfer.IsBrcOpn())
            {
                if (lastCharWasBrcOpn)
                {
                    lastCharWasBrcOpn = false;
                }
                else
                {
                    lastCharWasBrcOpn = true;
                    destSpan[to++] = charToTransfer;
                }
            }
            else if (charToTransfer.IsBrcCls())
            {
                if (lastCharWasBrcCls)
                {
                    lastCharWasBrcCls = false;
                }
                else
                {
                    lastCharWasBrcCls = true;
                    destSpan[to++]    = charToTransfer;
                }
            }
            else
            {
                lastCharWasBrcOpn = false;
                lastCharWasBrcCls = false;
                destSpan[to++]    = charToTransfer;
            } 
        }
        return cappedLength;
    }

    public int CalculateEncodedLength(ReadOnlySpan<char> source, int sourceFrom = 0, int maxTransferCount = int.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, i, source.Length);
        return end - i;
    }

    public int CalculateEncodedLength(char[] source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, i, source.Length);
        return end - i;
    }

    public int CalculateEncodedLength(ICharSequence source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue)
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, i, source.Length);
        return end - i;
    }

    public int CalculateEncodedLength(StringBuilder source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue) 
    {
        var capLen = Math.Clamp(maxTransferCount, 0, source.Length);
        var i      = Math.Clamp(sourceFrom, 0, source.Length);
        var end    = Math.Clamp(i + capLen, i, source.Length);
        return end - i;
    }

    public int CalculateLengthForCappedEncodeLength(int cappedLength, ReadOnlySpan<char> source, int sourceFrom = 0
      , int maxTransferCount = int.MaxValue) => 
        CalculateEncodedLength(source, sourceFrom, cappedLength);

    public int CalculateLengthForCappedEncodeLength(int cappedLength, char[] source, int sourceFrom = 0, int maxTransferCount = Int32.MaxValue) => 
        CalculateEncodedLength(source, sourceFrom, cappedLength);

    public int CalculateLengthForCappedEncodeLength(int cappedLength, ICharSequence source, int sourceFrom = 0
      , int maxTransferCount = Int32.MaxValue) => 
        CalculateEncodedLength(source, sourceFrom, cappedLength);

    public int CalculateLengthForCappedEncodeLength(int cappedLength, StringBuilder source, int sourceFrom = 0
      , int maxTransferCount = Int32.MaxValue) => 
        CalculateEncodedLength(source, sourceFrom, cappedLength);

    public virtual int AppendTransfer(ReadOnlySpan<char> source, IStringBuilder destSb)
    {
        var preAppendLength = destSb.Length;
        
        
        destSb.EnsureCapacity(source.Length);
        
        destSb.Append(source);
        return destSb.Length - preAppendLength;
    }

    public virtual int InsertTransfer(ReadOnlySpan<char> source, IStringBuilder destSb, int destStartIndex)
    {
        destSb.EnsureCapacity(source.Length);
        destSb.Insert(destStartIndex, source);
        return source.Length;
    }

    public virtual int OverwriteTransfer(ReadOnlySpan<char> source, IStringBuilder destSb, int destStartIndex)
    {
        destSb.EnsureCapacity(source.Length);
        return destSb.Overwrite(destStartIndex, source);
    }

    public virtual int OverwriteTransfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue) =>
        OverwriteTransfer(source, 0, destSpan, destStartIndex, maxTransferCount);

    public virtual int InsertTransfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
       ,int currentEndIndex)
    {
        destSpan.ShiftByAmount(destStartIndex, currentEndIndex, source.Length);
        return OverwriteTransfer(source, 0, destSpan, destStartIndex, source.Length);
    }

    public virtual int OverwriteTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
        
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[cappedFrom + i];
        return cappedLength;
    }

    public virtual int InsertTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan
      , int destStartIndex, int currentEndIndex, int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
        
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[cappedFrom + i];
        return cappedLength;
    }

    public virtual int AppendTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
        
        destSb.EnsureCapacity(cappedLength);
        
        destSb.Append(source, cappedFrom, cappedLength);
        return cappedLength;
    }

    public virtual int InsertTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
        
        destSb.EnsureCapacity(cappedLength);
        
        destSb.Insert(destStartIndex, source, cappedFrom, cappedLength);
        return cappedLength;
    }

    public virtual int OverwriteTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
        
        destSb.EnsureCapacity(cappedLength);
        
        return destSb.Overwrite(destStartIndex, source, cappedFrom, cappedLength);
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, IStringBuilder destSb, int destStartIndex = int.MaxValue)
    {
        var preAppendLength = destSb.Length;
        
        destSb.EnsureCapacity(source.Length);
        destSb.Append(source);
        return destSb.Length - preAppendLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, Math.Min(source.Length, destSpan.Length - destStartIndex)));
        
        for (var i = 0; i < cappedLength; i++) destSpan[i + 1 + destStartIndex] = source[i];
        return cappedLength + 2;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
        
        destSb.EnsureCapacity(cappedLength);
        
        destSb.Append(source, cappedFrom, cappedLength);
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, Math.Min(source.Length - sourceFrom, destSpan.Length - destStartIndex)));
        for (var i = 0; i < cappedLength; i++) destSpan[i + 1 + destStartIndex] = source[sourceFrom + i];
        return cappedLength;
    }

    public virtual int Transfer(StringBuilder source, IStringBuilder destSb
      , int destStartIndex = int.MaxValue)
    {
        var preAppendLength = destSb.Length;
        
        destSb.EnsureCapacity(preAppendLength);
        destSb.Append(source);
        return destSb.Length - preAppendLength;
    }

    public virtual int Transfer(StringBuilder source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, Math.Min(source.Length, destSpan.Length - destStartIndex)));

        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public virtual int Transfer(StringBuilder source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        sourceFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - sourceFrom);
        
        destSb.EnsureCapacity(cappedLength);
        
        destSb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public virtual int Transfer(StringBuilder source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, Math.Min(source.Length - sourceFrom, destSpan.Length - destStartIndex)));
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[sourceFrom + i];
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, IStringBuilder destSb
      , int destStartIndex = int.MaxValue)
    {
        var preAppendLength = destSb.Length;
        
        destSb.EnsureCapacity(source.Length);
        destSb.Append(source);
        return destSb.Length - preAppendLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, Math.Min(source.Length, destSpan.Length - destStartIndex)));

        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, source.Length - sourceFrom));

        destSb.EnsureCapacity(cappedLength);
        destSb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, Math.Min(source.Length - sourceFrom, destSpan.Length - destStartIndex)));
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[sourceFrom + i];
        return cappedLength;
    }
}
