// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

public class JsEscapingFormatter : CustomStringFormatter, ICustomStringFormatter
{
    private static string[] jsEscapeChars = null!;

    private byte previousByteUnusedBits;
    private byte previousByteBitCount;

    public JsEscapingFormatter()
    {
        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
        jsEscapeChars ??= BuildEscapedChars(new string[128]);
    }

    protected virtual string[] BuildEscapedChars(string[] toBuild)
    {
        var hexBuffer = stackalloc char[6].ResetMemory();
        for (int i = 0; i < 128; i++)
        {
            var iChar = (char)i;
            if (i < 32)
            {
                switch (i)
                {
                    case '\b': toBuild[i] = @"\b"; break;
                    case '\t': toBuild[i] = @"\t"; break;
                    case '\n': toBuild[i] = @"\n"; break;
                    case '\v': toBuild[i] = @"\v"; break;
                    case '\f': toBuild[i] = @"\f"; break;
                    case '\r': toBuild[i] = @"\r"; break;
                    default:
                        hexBuffer[0] = '\\';
                        hexBuffer[1] = 'u';
                        hexBuffer.AppendAsLowerHex(i, 2);
                        toBuild[i] = new string(hexBuffer);
                        break;
                }
            }
            else
            {
                switch (i)
                {
                    case '\"': toBuild[i] = @"\"; break;
                    case '\'': toBuild[i] = @"\'"; break;
                    case '\\': toBuild[i] = @"\\"; break;
                    default:   toBuild[i] = iChar.ToString(); break;
                }
            }
        }
        return toBuild;
    }

    public bool CharArrayWritesString { get; set; }

    public bool ByteArrayWritesBase64String { get; set; } = true;

    public override int Transfer(ReadOnlySpan<char> source, IStringBuilder sb)
    {
        return JsEscapingTransfer(source, 0, sb);
    }

    public override int Transfer(ReadOnlySpan<char> source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, 0, destination, destStartIndex, maxTransferCount);
    }

    public override int Transfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, sb, maxTransferCount);
    }

    public override int Transfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destination, destStartIndex, maxTransferCount);
    }

    protected virtual int Transfer(char? source, IStringBuilder sb)
    {
        if (source == null) return 0;
        sb.Append(jsEscapeChars[source.Value]);
        return 1;
    }

    protected virtual int Transfer(char? source, Span<char> dest, int destIndex)
    {
        if (source == null) return 0;
        return dest.OverWriteAt(destIndex, jsEscapeChars[source.Value]);
    }

    protected virtual int NextBase64Chars(byte source, IStringBuilder sb)
    {
        var b = source;

        int firstChar;
        switch (previousByteBitCount)
        {
            case 0:
                previousByteUnusedBits = (byte)((0x3 & b) << 4);
                previousByteBitCount   = 2;
                firstChar              = b >> 2;
                sb.Append(Base64LookupTable[firstChar]);
                return 1;
            case 2:
                firstChar              = previousByteUnusedBits | ((0xF0 & b) >> 4);
                previousByteUnusedBits = (byte)((0xF & b) << 2);
                previousByteBitCount   = 4;
                sb.Append(Base64LookupTable[firstChar]);
                return 1;
            case 4:
                var secondChar = (byte)((0x3F & b));
                firstChar = previousByteUnusedBits | ((0xC0 & b) >> 6);
                sb.Append(Base64LookupTable[firstChar]);
                sb.Append(Base64LookupTable[secondChar]);
                previousByteBitCount = 0;
                return 2;
            default: throw new ApplicationException("Unexpected base 64 remaining bit state!");
        }
    }

    protected virtual int CompleteBase64Sequence(IStringBuilder sb)
    {
        int firstChar;
        switch (previousByteBitCount)
        {
            case 0: return 0;
            case 2:
                firstChar = previousByteUnusedBits;
                sb.Append(Base64LookupTable[firstChar]);
                sb.Append('=');
                sb.Append('=');
                previousByteBitCount = 0;
                return 3;
            case 4:
                previousByteBitCount = 0;
                firstChar            = previousByteUnusedBits;
                sb.Append(Base64LookupTable[firstChar]);
                sb.Append('=');
                return 2;
            default: throw new ApplicationException("Unexpected base 64 remaining bit state!");
        }
    }

    protected virtual int NextBase64Chars(byte source, Span<char> dest, int destIndex)
    {
        var b = source;

        int firstChar;
        switch (previousByteBitCount)
        {
            case 0:
                previousByteUnusedBits = (byte)((0x3 & b) << 4);
                previousByteBitCount   = 2;
                firstChar              = b >> 2;
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                return 1;
            case 2:
                firstChar              = previousByteUnusedBits | ((0xF0 & b) >> 4);
                previousByteUnusedBits = (byte)((0xF & b) << 2);
                previousByteBitCount   = 4;
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                return 1;
            case 4:
                var secondChar = (byte)((0x3F & b));
                firstChar = previousByteUnusedBits | ((0xC0 & b) >> 6);
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                dest.OverWriteAt(destIndex + 1, Base64LookupTable[firstChar]);
                previousByteBitCount = 0;
                return 2;
            default: throw new ApplicationException("Unexpected base 64 remaining bit state!");
        }
    }

    protected virtual int CompleteBase64Sequence(Span<char> dest, int destIndex)
    {
        int firstChar;
        switch (previousByteBitCount)
        {
            case 0: return 0;
            case 2:
                firstChar = previousByteUnusedBits;
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                dest.OverWriteAt(destIndex + 1, '=');
                dest.OverWriteAt(destIndex + 2, '=');
                previousByteBitCount = 0;
                return 3;
            case 4:
                previousByteBitCount = 0;
                firstChar            = previousByteUnusedBits;
                dest.OverWriteAt(destIndex, Base64LookupTable[firstChar]);
                dest.OverWriteAt(destIndex + 1, '=');
                return 2;
            default: throw new ApplicationException("Unexpected base 64 remaining bit state!");
        }
    }

    protected int JsEscapingTransfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue)
    {
        var i         = sourceFrom;
        var end       = Math.Min(source.Length, maxTransferCount + sourceFrom);
        var hexBuffer = stackalloc char[6].ResetMemory();
        for (; i < end; i++)
        {
            var iChar = (int)source[i];
            if (iChar < 128) { sb.Append(jsEscapeChars[iChar]); }
            else
            {
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendAsLowerHex(i, 2);
                sb.Append(hexBuffer);
            }
        }
        return i - sourceFrom;
    }

    protected int JsEscapingTransfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destination
      , int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var i     = sourceFrom;
        var desti = destStartIndex;
        var end   = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end; i++)
        {
            var iChar = (int)source[i];
            if (iChar < 128)
            {
                var jsEscapeChar = jsEscapeChars[iChar];
                switch (jsEscapeChar.Length)
                {
                    case 1: destination[desti++] = jsEscapeChar[0]; break;
                    case 2:
                        destination[desti++] = jsEscapeChar[0];
                        destination[desti++] = jsEscapeChar[1];
                        break;
                    case 6:
                        destination[desti++] = jsEscapeChar[0];
                        destination[desti++] = jsEscapeChar[1];
                        destination[desti++] = jsEscapeChar[2];
                        destination[desti++] = jsEscapeChar[3];
                        destination[desti++] = jsEscapeChar[4];
                        destination[desti++] = jsEscapeChar[5];
                        break;
                    default:
                        for (int j = 0; j < jsEscapeChar.Length; j++) { destination[desti++] = jsEscapeChar[j]; }
                        break;
                }
            }
            else
            {
                destination[desti++] =  '\\';
                destination[desti++] =  'u';
                desti                += destination.AppendAsLowerHex(i, desti);
            }
        }
        return i - sourceFrom;
    }

    public override int Transfer(char[] source, IStringBuilder sb)
    {
        return JsEscapingTransfer(source, 0, sb);
    }

    public override int Transfer(char[] source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, 0, destination, destStartIndex, maxTransferCount);
    }

    public override int Transfer(char[] source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, sb, maxTransferCount);
    }

    public override int Transfer(char[] source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destination, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(char[] source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue)
    {
        var i         = sourceFrom;
        var hexBuffer = stackalloc char[6].ResetMemory();
        var end       = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end; i++)
        {
            var iChar = (int)source[i];
            if (iChar < 128) { sb.Append(jsEscapeChars[iChar]); }
            else
            {
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendAsLowerHex(i, 2);
                sb.Append(hexBuffer);
            }
        }
        return i - sourceFrom;
    }

    protected int JsEscapingTransfer(char[] source, int sourceFrom, Span<char> destination
      , int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var i     = sourceFrom;
        var desti = destStartIndex;
        var end   = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end; i++)
        {
            var iChar = (int)source[i];
            if (iChar < 128)
            {
                var jsEscapeChar = jsEscapeChars[iChar];
                switch (jsEscapeChar.Length)
                {
                    case 1: destination[desti++] = jsEscapeChar[0]; break;
                    case 2:
                        destination[desti++] = jsEscapeChar[0];
                        destination[desti++] = jsEscapeChar[1];
                        break;
                    case 6:
                        destination[desti++] = jsEscapeChar[0];
                        destination[desti++] = jsEscapeChar[1];
                        destination[desti++] = jsEscapeChar[2];
                        destination[desti++] = jsEscapeChar[3];
                        destination[desti++] = jsEscapeChar[4];
                        destination[desti++] = jsEscapeChar[5];
                        break;
                    default:
                        for (int j = 0; j < jsEscapeChar.Length; j++) { destination[desti++] = jsEscapeChar[j]; }
                        break;
                }
            }
            else
            {
                destination[desti++] = '\\';
                destination[desti++] = 'u';

                desti += destination.AppendAsLowerHex(i, desti);
            }
        }
        return i - sourceFrom;
    }

    public override int Transfer(StringBuilder source, IStringBuilder sb)
    {
        return JsEscapingTransfer(source, 0, sb);
    }

    public override int Transfer(StringBuilder source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, 0, destination, destStartIndex, maxTransferCount);
    }

    public override int Transfer(StringBuilder source, int sourceFrom, IStringBuilder sb
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, sb, maxTransferCount);
    }

    public override int Transfer(StringBuilder source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destination, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(StringBuilder source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue)
    {
        var i         = sourceFrom;
        var hexBuffer = stackalloc char[6].ResetMemory();
        var end       = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end; i++)
        {
            var iChar = (int)source[i];
            if (iChar < 128) { sb.Append(jsEscapeChars[iChar]); }
            else
            {
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendAsLowerHex(i, 2);
                sb.Append(hexBuffer);
            }
        }
        return i - sourceFrom;
    }

    protected int JsEscapingTransfer(StringBuilder source, int sourceFrom, Span<char> destination
      , int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var i     = sourceFrom;
        var desti = destStartIndex;
        var end   = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end; i++)
        {
            var iChar = (int)source[i];
            if (iChar < 128)
            {
                var jsEscapeChar = jsEscapeChars[iChar];
                switch (jsEscapeChar.Length)
                {
                    case 1: destination[desti++] = jsEscapeChar[0]; break;
                    case 2:
                        destination[desti++] = jsEscapeChar[0];
                        destination[desti++] = jsEscapeChar[1];
                        break;
                    case 6:
                        destination[desti++] = jsEscapeChar[0];
                        destination[desti++] = jsEscapeChar[1];
                        destination[desti++] = jsEscapeChar[2];
                        destination[desti++] = jsEscapeChar[3];
                        destination[desti++] = jsEscapeChar[4];
                        destination[desti++] = jsEscapeChar[5];
                        break;
                    default:
                        for (int j = 0; j < jsEscapeChar.Length; j++) { destination[desti++] = jsEscapeChar[j]; }
                        break;
                }
            }
            else
            {
                destination[desti++] = '\\';
                destination[desti++] = 'u';

                desti += destination.AppendAsLowerHex(i, desti);
            }
        }
        return i - sourceFrom;
    }

    public override int Transfer(ICharSequence source, IStringBuilder sb)
    {
        return JsEscapingTransfer(source, 0, sb);
    }

    public override int Transfer(ICharSequence source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, 0, destination, destStartIndex, maxTransferCount);
    }

    public override int Transfer(ICharSequence source, int sourceFrom, IStringBuilder sb
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, sb, maxTransferCount);
    }

    public override int Transfer(ICharSequence source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        return JsEscapingTransfer(source, sourceFrom, destination, destStartIndex, maxTransferCount);
    }

    protected int JsEscapingTransfer(ICharSequence source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue)
    {
        var i         = sourceFrom;
        var hexBuffer = stackalloc char[6].ResetMemory();
        var end       = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end; i++)
        {
            var iChar = (int)source[i];
            if (iChar < 128) { sb.Append(jsEscapeChars[iChar]); }
            else
            {
                hexBuffer[0] = '\\';
                hexBuffer[1] = 'u';
                hexBuffer.AppendAsLowerHex(i, 2);
                sb.Append(hexBuffer);
            }
        }
        return i - sourceFrom;
    }

    protected int JsEscapingTransfer(ICharSequence source, int sourceFrom, Span<char> destination
      , int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var i     = sourceFrom;
        var desti = destStartIndex;
        var end   = Math.Min(source.Length, maxTransferCount + sourceFrom);
        for (; i < end; i++)
        {
            var iChar = (int)source[i];
            if (iChar < 128)
            {
                var jsEscapeChar = jsEscapeChars[iChar];
                switch (jsEscapeChar.Length)
                {
                    case 1: destination[desti++] = jsEscapeChar[0]; break;
                    case 2:
                        destination[desti++] = jsEscapeChar[0];
                        destination[desti++] = jsEscapeChar[1];
                        break;
                    case 6:
                        destination[desti++] = jsEscapeChar[0];
                        destination[desti++] = jsEscapeChar[1];
                        destination[desti++] = jsEscapeChar[2];
                        destination[desti++] = jsEscapeChar[3];
                        destination[desti++] = jsEscapeChar[4];
                        destination[desti++] = jsEscapeChar[5];
                        break;
                    default:
                        for (int j = 0; j < jsEscapeChar.Length; j++) { destination[desti++] = jsEscapeChar[j]; }
                        break;
                }
            }
            else
            {
                destination[desti++] = '\\';
                destination[desti++] = 'u';

                desti += destination.AppendAsLowerHex(i, desti);
            }
        }
        return i - sourceFrom;
    }


    public override int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;

        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Length); }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars  = 0;
        var elementType = typeof(TFmt);
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;

        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Length); }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars  = 0;
        var elementType = typeof(TFmt);
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(TFmt[] arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Length); }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(TFmt[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull) { return destCharSpan.OverWriteAt(destStartIndex, NullString); }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Length); }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull) { return destCharSpan.OverWriteAt(destStartIndex, NullString); }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection) { addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0); }
        for (var i = 0; i < arg0.Length; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }


    public override int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;

        if (arg0.Count == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Count > 0); }
        for (var i = 0; i < arg0.Count; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Count); }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars  = 0;
        var elementType = typeof(TFmt);
        for (var i = 0; i < arg0.Count; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;

        if (arg0.Count == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !IgnoreEmptyCollection) { CollectionStart(elementType, sb, arg0.Count > 0); }
        for (var i = 0; i < arg0.Count; i++)
        {
            var item = arg0[i];
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, i, sb); }
                else { CollectionNextItemFormat(item, i, sb, formatString); }
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection) { CollectionEnd(elementType, sb, arg0.Count); }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars  = 0;
        var elementType = typeof(TFmt);
        for (var i = 0; i < arg0.Count; i++)
        {
            var item              = arg0[i];
            if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, i, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString); }
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(IEnumerable<TFmt> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen         = sb.Length;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                CollectionStart(elementType, sb, true);
                hasStartedCollection = true;
            }
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, itemCount, sb); }
                else { CollectionNextItemFormat(item, itemCount, sb, formatString); }
            }
            itemCount++;
        }
        if (itemCount > 0) { CollectionEnd(elementType, sb, itemCount); }
        else
        {
            if (!IgnoreEmptyCollection)
            {
                if (EmptyCollectionWritesNull) { sb.Append(NullString); }
                else
                {
                    CollectionStart(elementType, sb, false);
                    CollectionEnd(elementType, sb, 0);
                }
            }
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IEnumerable<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars           = 0;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                hasStartedCollection =  true;
            }
            if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString); }
            }
            itemCount++;
        }
        if (itemCount > 0) { addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount); }
        else
        {
            if (!IgnoreEmptyCollection)
            {
                if (EmptyCollectionWritesNull) { addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, NullString); }
                else
                {
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                }
            }
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen         = sb.Length;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                CollectionStart(elementType, sb, true);
                hasStartedCollection = true;
            }
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, itemCount, sb); }
                else { CollectionNextItemFormat(item, itemCount, sb, formatString); }
            }
            itemCount++;
        }
        if (itemCount > 0) { CollectionEnd(elementType, sb, itemCount); }
        else
        {
            if (!IgnoreEmptyCollection)
            {
                if (EmptyCollectionWritesNull) { sb.Append(NullString); }
                else
                {
                    CollectionStart(elementType, sb, false);
                    CollectionEnd(elementType, sb, 0);
                }
            }
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars           = 0;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                hasStartedCollection =  true;
            }
            if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString); }
            }
            itemCount++;
        }
        if (itemCount > 0) { addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount); }
        else
        {
            if (!IgnoreEmptyCollection)
            {
                if (EmptyCollectionWritesNull) { addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, NullString); }
                else
                {
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                }
            }
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(IEnumerator<TFmt> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && !IgnoreEmptyCollection && EmptyCollectionWritesNull) { return sb.Append(NullString).ReturnCharCount(NullString.Length); }
        var elementType = typeof(TFmt);
        if (!hasNext || !IgnoreEmptyCollection) { CollectionStart(elementType, sb, hasNext); }
        var itemCount = 0;
        while (hasNext)
        {
            var item = arg0.Current;
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, itemCount, sb); }
                else { CollectionNextItemFormat(item, itemCount, sb, formatString); }
            }
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !IgnoreEmptyCollection) { CollectionEnd(elementType, sb, itemCount); }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IEnumerator<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && !IgnoreEmptyCollection && EmptyCollectionWritesNull) { return destCharSpan.OverWriteAt(destStartIndex, NullString); }
        var elementType = typeof(TFmt);
        if (!hasNext || !IgnoreEmptyCollection) { addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext); }
        var itemCount = 0;
        while (hasNext)
        {
            var item                      = arg0.Current;
            if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString); }
            }
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && !IgnoreEmptyCollection && EmptyCollectionWritesNull) { return sb.Append(NullString).ReturnCharCount(NullString.Length); }
        var elementType = typeof(TFmt);
        if (!hasNext || !IgnoreEmptyCollection) { CollectionStart(elementType, sb, hasNext); }
        var itemCount = 0;
        while (hasNext)
        {
            var item = arg0.Current;
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
            if (item is char iChar && CharArrayWritesString) { Transfer(iChar, sb); }
            else if (item is byte iByte && ByteArrayWritesBase64String) { NextBase64Chars(iByte, sb); }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString) { CollectionNextItem(item, itemCount, sb); }
                else { CollectionNextItemFormat(item, itemCount, sb, formatString); }
            }
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !IgnoreEmptyCollection) { CollectionEnd(elementType, sb, itemCount); }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && !IgnoreEmptyCollection && EmptyCollectionWritesNull) { return destCharSpan.OverWriteAt(destStartIndex, NullString); }
        var elementType = typeof(TFmt);
        if (!hasNext || !IgnoreEmptyCollection) { addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext); }
        var itemCount = 0;
        while (hasNext)
        {
            var item                      = arg0.Current;
            if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
            if (item is char iChar && CharArrayWritesString) { addedChars += Transfer(iChar, destCharSpan, destStartIndex + addedChars); }
            else if (item is byte iByte && ByteArrayWritesBase64String)
            {
                addedChars += NextBase64Chars(iByte, destCharSpan, destStartIndex + addedChars);
            }
            else
            {
                if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
                {
                    addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                }
                else { addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString); }
            }
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int CollectionStart(Type elementType, IStringBuilder sb, bool hasItems)
    {
        if (elementType == typeof(char) && CharArrayWritesString) return sb.Append("\"").ReturnCharCount(1);
        if (elementType == typeof(byte) && ByteArrayWritesBase64String) return sb.Append("\"").ReturnCharCount(1);
        return sb.Append("[").ReturnCharCount(1);
    }

    public override int CollectionStart(Type elementType, Span<char> destination, int destStartIndex, bool hasItems)
    {
        if (elementType == typeof(char) && CharArrayWritesString) return destination.OverWriteAt(destStartIndex, "\"");
        if (elementType == typeof(byte) && ByteArrayWritesBase64String) return destination.OverWriteAt(destStartIndex, "\"");
        return destination.OverWriteAt(destStartIndex, "[");
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, IStringBuilder sb, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && ByteArrayWritesBase64String) return 0;
        return sb.Append(",").ReturnCharCount(1);
    }

    public override int AddCollectionElementSeparator(Type collectionElementType, Span<char> charSpan, int atIndex, int nextItemNumber)
    {
        if (collectionElementType == typeof(char) && CharArrayWritesString) return 0;
        if (collectionElementType == typeof(byte) && ByteArrayWritesBase64String) return 0;
        return charSpan.OverWriteAt(atIndex, ",");
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString)
    {
        switch (nextItem)
        {
            case char charItem:
                if (CharArrayWritesString) { return sb.Append(charItem).ReturnCharCount(1); }
                if (formatString.IsNullOrEmpty()) { return sb.Append("\"").Append(charItem).Append("\"").ReturnCharCount(3); }
                break;
            case byte byteItem:
                if (ByteArrayWritesBase64String) return NextBase64Chars(byteItem, sb);
                break;
            case DateTime dateTimeItem:
                if (formatString.IsNullOrEmpty()) return Format(dateTimeItem, sb, "yyyy-MM-ddTHH:mm:ss");
                break;
        }
        return Format(nextItem, sb, formatString);
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString)
    {
        switch (nextItem)
        {
            case char charItem:
                if (CharArrayWritesString)
                {
                    destCharSpan[destStartIndex] = charItem;
                    return 1;
                }
                if (formatString.IsNullOrEmpty())
                {
                    destCharSpan.OverWriteAt(destStartIndex, "\"");
                    destCharSpan.OverWriteAt(destStartIndex + 1, charItem);
                    destCharSpan.OverWriteAt(destStartIndex + 2, "\"");
                    return 3;
                }
                break;
            case byte byteItem:
                if (ByteArrayWritesBase64String) return NextBase64Chars(byteItem, destCharSpan, destStartIndex);
                break;
            case DateTime dateTimeItem:
                if (formatString.IsNullOrEmpty()) return Format(dateTimeItem, destCharSpan, destStartIndex, "yyyy-MM-ddTHH:mm:ss");
                break;
        }
        return Format(nextItem, destCharSpan, destStartIndex, formatString);
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString)
    {
        switch (nextItem)
        {
            case char charItem:
                if (CharArrayWritesString) { return sb.Append(charItem).ReturnCharCount(1); }
                if (formatString.IsNullOrEmpty()) { return sb.Append("\"").Append(charItem).Append("\"").ReturnCharCount(3); }
                break;
            case byte byteItem:
                if (ByteArrayWritesBase64String) return NextBase64Chars(byteItem, sb);
                break;
            case DateTime dateTimeItem:
                if (formatString.IsNullOrEmpty()) return Format(dateTimeItem, sb, "yyyy-MM-ddTHH:mm:ss");
                break;
        }
        return Format(nextItem, sb, formatString);
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString)
    {
        switch (nextItem)
        {
            case char charItem:
                if (CharArrayWritesString)
                {
                    destCharSpan[destStartIndex] = charItem;
                    return 1;
                }
                if (formatString.IsNullOrEmpty())
                {
                    destCharSpan.OverWriteAt(destStartIndex, "\"");
                    destCharSpan.OverWriteAt(destStartIndex + 1, charItem);
                    destCharSpan.OverWriteAt(destStartIndex + 2, "\"");
                    return 3;
                }
                break;
            case byte byteItem:
                if (ByteArrayWritesBase64String) return NextBase64Chars(byteItem, destCharSpan, destStartIndex);
                break;
            case DateTime dateTimeItem:
                if (formatString.IsNullOrEmpty()) return Format(dateTimeItem, destCharSpan, destStartIndex, "yyyy-MM-ddTHH:mm:ss");
                break;
        }
        return Format(nextItem, destCharSpan, destStartIndex, formatString);
    }

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb)
    {
        var preAppendLen = sb.Length;
        switch (nextItem)
        {
            case char charItem:
                return CharArrayWritesString
                    ? sb.Append(charItem).ReturnCharCount(1)
                    : sb.Append("\"").Append(charItem).Append("\"").ReturnCharCount(3);
            case byte byteItem:
                return ByteArrayWritesBase64String
                    ? NextBase64Chars(byteItem, sb)
                    : sb.Append(byteItem).ReturnCharCount(3);
            case DateTime dateTimeItem: return Format(dateTimeItem, sb, "yyyy-MM-ddTHH:mm:ss");
        }
        return sb.Append(nextItem).ReturnCharCount(sb.Length - preAppendLen);
    }

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex)
    {
        switch (nextItem)
        {
            case char charItem:
                if (CharArrayWritesString)
                {
                    destCharSpan[destStartIndex] = charItem;
                    return 1;
                }
                destCharSpan.OverWriteAt(destStartIndex, "\"");
                destCharSpan.OverWriteAt(destStartIndex + 1, charItem);
                destCharSpan.OverWriteAt(destStartIndex + 2, "\"");
                return 3;
            case byte byteItem:
                if (ByteArrayWritesBase64String) return NextBase64Chars(byteItem, destCharSpan, destStartIndex);
                break;
            case DateTime dateTimeItem: return Format(dateTimeItem, destCharSpan, destStartIndex, "yyyy-MM-ddTHH:mm:ss");
        }
        CharSpanCollectionScratchBuffer ??= MutableString.MediumScratchBuffer;
        CharSpanCollectionScratchBuffer.Clear();
        CharSpanCollectionScratchBuffer.Append(nextItem);
        return destCharSpan.OverWriteAt(destStartIndex, CharSpanCollectionScratchBuffer);
    }


    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount)
    {
        if (elementType == typeof(char) && CharArrayWritesString) return sb.Append("\"").ReturnCharCount(1);
        if (elementType == typeof(byte) && ByteArrayWritesBase64String)
        {
            var addedChars = CompleteBase64Sequence(sb);
            return sb.Append("\"").ReturnCharCount(1 + addedChars);
        }
        return sb.Append("]").ReturnCharCount(1);
    }

    public override int CollectionEnd(Type elementType, Span<char> destination, int index, int itemsCount)
    {
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        if (elementType == typeof(char) && CharArrayWritesString) return destination.OverWriteAt(index, "\"");
        if (elementType == typeof(byte) && ByteArrayWritesBase64String)
        {
            var addedChars = CompleteBase64Sequence(destination, index);
            return destination.OverWriteAt(index + addedChars, "\"") + addedChars;
        }
        return destination.OverWriteAt(index, "]");
    }
}
