using System.Text;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

public interface IEncodingTransfer
{
    string EncodingTransferConfigKey { get; }


    int StringValueDelimiter(IStringBuilder sb);
    int StringValueDelimiter(Span<char> destSpan, int destStartIndex);
    int StringFieldDelimiter(IStringBuilder sb);
    int StringFieldDelimiter(Span<char> destSpan, int destStartIndex);

    int Transfer(Rune? source, IStringBuilder destSb);
    int Transfer(Rune? source, Span<char> dest, int destIndex);

    int Transfer(ICustomStringFormatter stringFormatter, char[] source, IStringBuilder destSb, int destStartIndex = int.MaxValue);
    int Transfer(ICustomStringFormatter stringFormatter, char[] source, Span<char> destSpan, int destStartIndex, int maxTransferCount = int.MaxValue);

    int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, IStringBuilder destSb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue);

    int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, Span<char> destSpan, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue);

    int TransferPrefix(bool encodeFirst, ReadOnlySpan<char> source, IStringBuilder destSb);
    int TransferPrefix(bool encodeFirst, ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex);

    int TransferSuffix(ReadOnlySpan<char> source, IStringBuilder destSb, bool encodeLast);
    int TransferSuffix(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex, bool encodeLast);

    int Transfer(ReadOnlySpan<char> source, IStringBuilder destSb, int destStartIndex = int.MaxValue);

    int Transfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int Transfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb
    , int destStartIndex = int.MaxValue, int maxTransferCount = int.MaxValue);

    int Transfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue);


    int Transfer(StringBuilder source, IStringBuilder destSbb, int destStartIndex = int.MaxValue);
    
    int Transfer(StringBuilder source, Span<char> destSpan, int destStartIndex
    , int maxTransferCount = int.MaxValue);

    int Transfer(StringBuilder source, int sourceFrom, IStringBuilder destSbb
    , int destStartIndex = int.MaxValue , int maxTransferCount = int.MaxValue);

    int Transfer(StringBuilder source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue);


    int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, IStringBuilder destSb, int destStartIndex = int.MaxValue);
    
    int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, Span<char> destSpan, int destStartIndex
    , int maxTransferCount = int.MaxValue);

    int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, int sourceFrom, IStringBuilder destSb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue);

    int Transfer(ICustomStringFormatter stringFormatter, ICharSequence source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue);
}