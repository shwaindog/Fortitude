using System.Text;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public class PassThroughEncodingTransfer : IEncodingTransfer
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
        Transfer(source, 0, destSb);

    public virtual int TransferPrefix(bool encodePrefix, ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex) => 
        Transfer(source, 0, destSpan, destStartIndex);

    public virtual int TransferSuffix(ReadOnlySpan<char> source, IStringBuilder destSb, bool encodeSuffix) => 
        Transfer(source, 0, destSb);

    public virtual int TransferSuffix(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex, bool encodeSuffix) => 
        Transfer(source, 0, destSpan, destStartIndex);

    public virtual int Transfer(ReadOnlySpan<char> source, IStringBuilder destSb
      , int destStartIndex = int.MaxValue)
    {
        var preAppendLength = destSb.Length;
        destSb.Append(source);
        return destSb.Length - preAppendLength;
    }

    public virtual int InsertTransfer(ReadOnlySpan<char> source, IStringBuilder destSb, int destStartIndex)
    {
        var preAppendLength = destSb.Length;
        destSb.InsertAt(source, destStartIndex);
        return destSb.Length - preAppendLength;
    }

    public virtual int Transfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue) =>
        Transfer(source, 0, destSpan, destStartIndex, maxTransferCount);

    public virtual int InsertTransfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
       ,int currentEndIndex)
    {
        destSpan.ShiftByAmount(destStartIndex, currentEndIndex, source.Length);
        return Transfer(source, 0, destSpan, destStartIndex, source.Length);
    }

    public virtual int Transfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[cappedFrom + i];
        return cappedLength;
    }

    public virtual int Transfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
        var end = cappedFrom + cappedLength;
        
        destSb.Append(source, cappedFrom, cappedLength);
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, IStringBuilder destSb, int destStartIndex = int.MaxValue)
    {
        var preAppendLength = destSb.Length;
        destSb.Append("[");
        destSb.Append(source);
        destSb.Append("]");
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
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, source.Length));
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
