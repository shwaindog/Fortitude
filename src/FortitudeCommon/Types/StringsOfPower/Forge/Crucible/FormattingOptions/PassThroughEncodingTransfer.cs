using System.Text;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public class PassThroughEncodingTransfer : IEncodingTransfer
{
    protected const char DblQtChar = '"';
    public string EncodingTransferConfigKey { get; private set; } = "PassThrough";

    public static PassThroughEncodingTransfer Instance { get; } = new();

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


    public virtual int TransferPrefix(bool encodeFirst, ReadOnlySpan<char> source, IStringBuilder destSb)
    {
        if (encodeFirst)
        {
            if (source.Length > 0)
            {
                var firstChar      = source[0];
                var firstCharCount = 0;
                if (firstChar != DblQtChar)
                {
                    destSb.Append(firstChar);
                    firstCharCount = 1;
                }
                if (source.Length > 1)
                {
                    if (firstChar.IsTwoCharHighSurrogate())
                    {
                        destSb.Append(source[1]);
                        if (source.Length > 2)
                        {
                            return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 2, destSb) + 1 + firstCharCount;
                        }
                    }
                    return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 1, destSb) + firstCharCount;
                }
                return firstCharCount;
            }
            return 0;
        }
        else { return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 0, destSb); }
    }

    public virtual int TransferPrefix(bool encodeFirst, ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex)
    {
        if (encodeFirst)
        {
            if (source.Length > 0)
            {
                var firstChar      = source[0];
                var firstCharCount = 0;
                if (firstChar != DblQtChar)
                {
                    destSpan.OverWriteAt(destStartIndex, firstChar);
                    firstCharCount = 1;
                }
                if (source.Length > 1)
                {
                    if (firstChar.IsTwoCharHighSurrogate())
                    {
                        destSpan.OverWriteAt(destStartIndex + firstCharCount, source[1]);
                        if (source.Length > 2)
                        {
                            return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source
                                          , 2, destSpan, destStartIndex + 2) + 2;
                        }
                    }
                    return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 1, destSpan
                                  , destStartIndex + firstCharCount) + firstCharCount;
                }
                return firstCharCount;
            }
            return 0;
        }
        else { return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 0, destSpan, destStartIndex); }
    }

    public virtual int TransferSuffix(ReadOnlySpan<char> source, IStringBuilder destSb, bool encodeLast)
    {
        if (encodeLast)
        {
            if (source.Length > 0)
            {
                var lastChar = source[^1];
                if (source.Length > 1)
                {
                    var encodedCount = 0;
                    if (lastChar.IsTwoCharLowSurrogate())
                    {
                        // then definitely last char is not DblQty
                        return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 0, destSb);
                    }
                    encodedCount = Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 0, destSb
                                          , maxTransferCount: source.Length - 1);

                    if (lastChar != DblQtChar)
                    {
                        destSb.Append(lastChar);
                        return encodedCount + 1;
                    }
                    return encodedCount;
                }
                if (lastChar != DblQtChar)
                {
                    destSb.Append(lastChar);
                    return 1;
                }
            }
            return 0;
        }
        else { return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 0, destSb); }
    }

    public virtual int TransferSuffix(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex, bool encodeLast)
    {
        if (encodeLast)
        {
            if (source.Length > 0)
            {
                var lastChar = source[^1];
                if (source.Length > 1)
                {
                    if (lastChar.IsTwoCharLowSurrogate())
                    {
                        // then definitely last char is not DblQty
                        return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 0, destSpan, destStartIndex);
                    }
                    var encodedCount = Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 0, destSpan, destStartIndex
                                              , maxTransferCount: source.Length - 1);

                    if (lastChar != DblQtChar)
                    {
                        destSpan.OverWriteAt(destStartIndex + encodedCount, lastChar);
                        return encodedCount + 1;
                    }
                    return encodedCount;
                }
                if (lastChar != DblQtChar)
                {
                    destSpan.OverWriteAt(destStartIndex, lastChar);
                    return 1;
                }
            }
            return 0;
        }
        else { return Transfer(ICustomStringFormatter.DefaultAsciiEscapeFormatter, source, 0, destSpan, destStartIndex); }
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, IStringBuilder destSb
      , int destStartIndex = int.MaxValue)
    {
        var preAppendLength = destSb.Length;
        destSb.Append(source);
        return destSb.Length - preAppendLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue) =>
        Transfer(stringFormatter, source, 0, destSpan, destStartIndex, maxTransferCount);

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan
      , int destStartIndex, int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);

        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[cappedFrom + i];
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        var cappedFrom   = Math.Clamp(sourceFrom, 0, source.Length);
        var cappedLength = Math.Clamp(maxTransferCount, 0, source.Length - cappedFrom);
        var end = cappedFrom + cappedLength;
        
        // for (var i = cappedFrom; i < end; i++) destSb.Append(source[i]);
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
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, Math.Min(source.Length, destSpan.Length - destStartIndex)));

        for (var i = 0; i < cappedLength; i++) destSpan[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, int sourceFrom, IStringBuilder destSb
      , int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Clamp(maxTransferCount, 0, Math.Max(0, source.Length));
        destSb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public virtual int Transfer(ICustomStringFormatter stringFormatter, StringBuilder source, int sourceFrom, Span<char> destSpan, int destStartIndex
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
