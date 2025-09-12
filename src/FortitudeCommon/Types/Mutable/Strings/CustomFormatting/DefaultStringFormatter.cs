// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

public class DefaultStringFormatter : CustomStringFormatter, ICustomStringFormatter
{
    public override int Transfer(ReadOnlySpan<char> source, IStringBuilder sb)
    {
        var preAppendLength = sb.Length;
        sb.Append(source);
        return sb.Length - preAppendLength;
    }

    public override int Transfer(ReadOnlySpan<char> source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(maxTransferCount, Math.Min(source.Length, destination.Length - destStartIndex));
        for (var i = 0; i < cappedLength; i++) destination[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public override int Transfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destination[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public override int Transfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public override int Transfer(char[] source, IStringBuilder sb)
    {
        var preAppendLength = sb.Length;
        sb.Append(source);
        return sb.Length - preAppendLength;
    }

    public override int Transfer(char[] source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength                                                       = Math.Min(source.Length, destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destination[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public override int Transfer(char[] source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public override int Transfer(char[] source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destination[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public override int Transfer(StringBuilder source, IStringBuilder sb)
    {
        var preAppendLength = sb.Length;
        sb.Append(source);
        return sb.Length - preAppendLength;
    }

    public override int Transfer(StringBuilder source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength                                                       = Math.Min(source.Length, destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destination[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public override int Transfer(StringBuilder source, int sourceFrom, IStringBuilder sb
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public override int Transfer(StringBuilder source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destination[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public override int Transfer(ICharSequence source, IStringBuilder sb)
    {
        var preAppendLength = sb.Length;
        sb.Append(source);
        return sb.Length - preAppendLength;
    }

    public override int Transfer(ICharSequence source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength                                                       = Math.Min(source.Length, destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destination[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public override int Transfer(ICharSequence source, int sourceFrom, IStringBuilder sb
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public override int Transfer(ICharSequence source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++) destination[i + destStartIndex] = source[i];
        return cappedLength;
    }

    public override int ProcessAppendedRange(IStringBuilder sb, int fromIndex) => 0;

    public override int ProcessAppendedRange(Span<char> destSpan, int fromIndex, int length) => 0;

    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItemFormat(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItemFormat(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItemFormat(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItemFormat(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatArray<TFmt>(TFmt[] arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItemFormat(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatArray<TFmt>(TFmt[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItemFormat(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItemFormat(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItemFormat(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatList<TFmt>(IReadOnlyList<TFmt> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        if (arg0.Count == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Count > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItemFormat(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Count);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatList<TFmt>(IReadOnlyList<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Count == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Count > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItemFormat(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public override int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        if (arg0.Count == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Count > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                CollectionNextItemFormat(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Count);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Count == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Count > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += CollectionNextItemFormat(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen         = sb.Length;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    CollectionStart(elementType, sb, true);
                    hasStartedCollection = true;
                }
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItem(item, itemCount, sb);
                itemCount++;
            }
        }
        else
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    CollectionStart(elementType, sb, true);
                    hasStartedCollection = true;
                }
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItemFormat(item, itemCount, sb, formatString);
                itemCount++;
            }
        }
        if (itemCount > 0)
        {
            CollectionEnd(elementType, sb, itemCount);
        }
        else
        {
            if (!IgnoreEmptyCollection)
            {
                if (EmptyCollectionWritesNull)
                {
                    sb.Append(NullString);
                }
                else
                {
                    CollectionStart(elementType, sb, false);
                    CollectionEnd(elementType, sb, 0);
                }
            }
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars           = 0;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                    hasStartedCollection =  true;
                }
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                itemCount++;
            }
        }
        else
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                    hasStartedCollection =  true;
                }
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString);
                itemCount++;
            }
        }
        if (itemCount > 0)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        else
        {
            if (!IgnoreEmptyCollection)
            {
                if (EmptyCollectionWritesNull)
                {
                    addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, NullString);
                }
                else
                {
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                }
            }
        }
        return addedChars;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen         = sb.Length;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    CollectionStart(elementType, sb, true);
                    hasStartedCollection = true;
                }
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItem(item, itemCount, sb);
                itemCount++;
            }
        }
        else
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    CollectionStart(elementType, sb, true);
                    hasStartedCollection = true;
                }
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItemFormat(item, itemCount, sb, formatString);
                itemCount++;
            }
        }
        if (itemCount > 0)
        {
            CollectionEnd(elementType, sb, itemCount);
        }
        else
        {
            if (!IgnoreEmptyCollection)
            {
                if (EmptyCollectionWritesNull)
                {
                    sb.Append(NullString);
                }
                else
                {
                    CollectionStart(elementType, sb, false);
                    CollectionEnd(elementType, sb, 0);
                }
            }
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars           = 0;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                    hasStartedCollection =  true;
                }
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                itemCount++;
            }
        }
        else
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                    hasStartedCollection =  true;
                }
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString);
                itemCount++;
            }
        }
        if (itemCount > 0)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        else
        {
            if (!IgnoreEmptyCollection)
            {
                if (EmptyCollectionWritesNull)
                {
                    addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, NullString);
                }
                else
                {
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                }
            }
        }
        return addedChars;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, hasNext);
        }
        var itemCount   = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItem(item, itemCount, sb);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        else
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItem(item, itemCount, sb);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, itemCount);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext);
        }
        var itemCount   = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item                      = arg0.Current;
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        else
        {
            while (hasNext)
            {
                var item                      = arg0.Current;
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, hasNext);
        }
        var itemCount   = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItem(item, itemCount, sb);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        else
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                CollectionNextItem(item, itemCount, sb);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, itemCount);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext);
        }
        var itemCount   = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item                      = arg0.Current;
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += CollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        else
        {
            while (hasNext)
            {
                var item                      = arg0.Current;
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int CollectionStart(Type collectionType,  IStringBuilder sb, bool hasItems) => 
        sb.Append(SqBrktOpn).ReturnCharCount(1);

    public override int CollectionStart(Type collectionType, Span<char> destination, int destStartIndex, bool hasItems)
    {
        return destination.OverWriteAt(destStartIndex, SqBrktOpn);
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString) =>
        Format(nextItem, sb, formatString);

    public override int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString)
        => Format(nextItem, destCharSpan, destStartIndex, formatString);

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString) =>
        Format(nextItem, sb, formatString);

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString)
        => Format(nextItem, destCharSpan, destStartIndex, formatString);

    public override int CollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb)
    {
        var preAppendLen = sb.Length;
        sb.Append(nextItem);
        return sb.Length - preAppendLen;
    }

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destination, int destStartIndex)
    {
        CharSpanCollectionScratchBuffer ??= MutableString.MediumScratchBuffer;
        CharSpanCollectionScratchBuffer.Clear();
        CharSpanCollectionScratchBuffer.Append(nextItem);
        return destination.OverWriteAt(destStartIndex, CharSpanCollectionScratchBuffer);
    }

    public override int CollectionEnd(Type collectionType, IStringBuilder sb, int itemsCount) => sb.Append(SqBrktCls).ReturnCharCount(1);

    public override int CollectionEnd(Type collectionType, Span<char> destination, int index, int itemsCount)
    {
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        return destination.OverWriteAt(index, SqBrktCls);
    }
}
