using System.Text;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public class PassThroughEncodingTransfer : IEncodingTransfer
{
    public string EncodingTransferConfigKey { get; private set; } = "PassThrough";

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

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, IStringBuilder destSb
      , int destStartIndex = int.MaxValue)
    {
        var preAppendLength = destSb.Length;
        destSb.Append(source);
        return destSb.Length - preAppendLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(maxTransferCount, Math.Min(source.Length, destSpan.Length - destStartIndex));
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan
      , int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destSpan.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        destSb.Append(source, sourceFrom, cappedLength);
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
        destSpan.OverWriteAt(destStartIndex, "[");
        var cappedLength = Math.Min(source.Length, destSpan.Length - destStartIndex - 2);

        for (var i = 0; i < cappedLength; i++) destSpan[i + 1 + destStartIndex] = source[i];
        destSpan.OverWriteAt(destStartIndex + cappedLength + 1, "]");
        return cappedLength + 2;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        destSb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destSpan.Length - destStartIndex - 2);
        for (var i = 0; i < cappedLength; i++) destSpan[i + 1 + destStartIndex] = source[i];
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, IStringBuilder destSb
      , int destStartIndex = int.MaxValue)
    {
        var preAppendLength = destSb.Length;
        destSb.Append(source);
        return destSb.Length - preAppendLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength                                                    = Math.Min(source.Length, destSpan.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        destSb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destSpan.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[i];
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
        var cappedLength                                                    = Math.Min(source.Length, destSpan.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        destSb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destSpan.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[i];
        return cappedLength;
    }
}
