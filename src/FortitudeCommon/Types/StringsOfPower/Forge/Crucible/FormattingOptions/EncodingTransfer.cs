using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;



public interface IEncodingTransfer : IRecyclableObject
{
    string EncodingTransferConfigKey { get; }

    EncodingType Type { get; }

    int StringValueDelimiter(IStringBuilder sb);
    int StringValueDelimiter(Span<char> destSpan, int destStartIndex);
    int StringFieldDelimiter(IStringBuilder sb);
    int StringFieldDelimiter(Span<char> destSpan, int destStartIndex);

    int Transfer(Rune? source, IStringBuilder destSb);
    int Transfer(Rune? source, Span<char> dest, int destIndex);

    int CalculateEncodedLength(ReadOnlySpan<char> source, int sourceFrom = 0, int maxTransferCount = int.MaxValue);
    int CalculateEncodedLength(char[] source, int sourceFrom = 0, int maxTransferCount = int.MaxValue);
    int CalculateEncodedLength(ICharSequence source, int sourceFrom = 0, int maxTransferCount = int.MaxValue);
    int CalculateEncodedLength(StringBuilder source, int sourceFrom = 0, int maxTransferCount = int.MaxValue);

    int CalculateLengthForCappedEncodeLength(int cappedLength, ReadOnlySpan<char> source, int sourceFrom = 0, int maxTransferCount = int.MaxValue);
    int CalculateLengthForCappedEncodeLength(int cappedLength, char[] source, int sourceFrom = 0, int maxTransferCount = int.MaxValue);
    int CalculateLengthForCappedEncodeLength(int cappedLength, ICharSequence source, int sourceFrom = 0, int maxTransferCount = int.MaxValue);
    int CalculateLengthForCappedEncodeLength(int cappedLength, StringBuilder source, int sourceFrom = 0, int maxTransferCount = int.MaxValue);

    int Transfer(ICustomStringFormatter stringFormatter, char[] source, IStringBuilder destSb, int destStartIndex = int.MaxValue);
    int Transfer(ICustomStringFormatter stringFormatter, char[] source, Span<char> destSpan, int destStartIndex, int maxTransferCount = int.MaxValue);

    int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, IStringBuilder destSb, int destStartIndex = int.MaxValue
      , int maxTransferCount = int.MaxValue);

    int Transfer(ICustomStringFormatter stringFormatter, char[] source, int sourceFrom, Span<char> destSpan, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue);

    int TransferPrefix(bool encodePrefix, ReadOnlySpan<char> source, IStringBuilder destSb);
    int TransferPrefix(bool encodePrefix, ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex);

    int TransferSuffix(ReadOnlySpan<char> source, IStringBuilder destSb, bool encodeSuffix);
    int TransferSuffix(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex, bool encodeSuffix);

    int AppendTransfer(ReadOnlySpan<char> source, IStringBuilder destSb);
    int InsertTransfer(ReadOnlySpan<char> source, IStringBuilder destSb, int destStartIndex);
    int OverwriteTransfer(ReadOnlySpan<char> source, IStringBuilder destSb, int destStartIndex);
  
    int OverwriteTransfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int InsertTransfer(ReadOnlySpan<char> source, Span<char> destSpan, int destStartIndex, int currentEndIndex);

    int AppendTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb, int maxTransferCount = int.MaxValue);

    int OverwriteTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb, int destStartIndex, int maxTransferCount = int.MaxValue);

    int InsertTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder destSb, int destStartIndex, int maxTransferCount = int.MaxValue);

    int OverwriteTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int InsertTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destSpan, int destStartIndex
    , int currentEndIndex, int maxTransferCount = int.MaxValue);


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