// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString;

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

    public override int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, IStringBuilder sb, string? formatString = null)
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionStart(elementType, sb, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
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
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionStart(elementType, sb, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
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
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionStart(elementType, sb, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(TFmt[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
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
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionStart(elementType, sb, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Length > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
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
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionStart(elementType, sb, arg0.Count > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionEnd(elementType, sb, arg0.Count);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Count == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex, arg0.Count > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
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
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionStart(elementType, sb, arg0.Count > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddCollectionElementSeparator(elementType, sb, i);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionEnd(elementType, sb, arg0.Count);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
    {
        var addedChars = 0;
        if (arg0.Count == 0 && !IgnoreEmptyCollection && EmptyCollectionWritesNull)
        {
            return destCharSpan.OverWriteAt(destStartIndex, NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex, arg0.Count > 0);
        }
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
            }
        }
        else
        {
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, i);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
        }
        if (arg0.Count > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(IEnumerable<TFmt> arg0, IStringBuilder sb, string? formatString = null)
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
                    FormatCollectionStart(elementType, sb, true);
                    hasStartedCollection = true;
                }
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                FormatCollectionNextItem(item, itemCount, sb);
                itemCount++;
            }
        }
        else
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    FormatCollectionStart(elementType, sb, true);
                    hasStartedCollection = true;
                }
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                FormatCollectionNextItem(item, itemCount, sb, formatString);
                itemCount++;
            }
        }
        if (itemCount > 0)
        {
            FormatCollectionEnd(elementType, sb, itemCount);
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
                    FormatCollectionStart(elementType, sb, false);
                    FormatCollectionEnd(elementType, sb, 0);
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
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    addedChars           += FormatCollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                    hasStartedCollection =  true;
                }
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += FormatCollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                itemCount++;
            }
        }
        else
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    addedChars           += FormatCollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                    hasStartedCollection =  true;
                }
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += FormatCollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString);
                itemCount++;
            }
        }
        if (itemCount > 0)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
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
                    addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false);
                    addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
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
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    FormatCollectionStart(elementType, sb, true);
                    hasStartedCollection = true;
                }
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                FormatCollectionNextItem(item, itemCount, sb);
                itemCount++;
            }
        }
        else
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    FormatCollectionStart(elementType, sb, true);
                    hasStartedCollection = true;
                }
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                FormatCollectionNextItem(item, itemCount, sb, formatString);
                itemCount++;
            }
        }
        if (itemCount > 0)
        {
            FormatCollectionEnd(elementType, sb, itemCount);
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
                    FormatCollectionStart(elementType, sb, false);
                    FormatCollectionEnd(elementType, sb, 0);
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
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    addedChars           += FormatCollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                    hasStartedCollection =  true;
                }
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += FormatCollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
                itemCount++;
            }
        }
        else
        {
            foreach (var item in arg0)
            {
                if (!hasStartedCollection)
                {
                    addedChars           += FormatCollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true);
                    hasStartedCollection =  true;
                }
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += FormatCollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString);
                itemCount++;
            }
        }
        if (itemCount > 0)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
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
                    addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false);
                    addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                }
            }
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(IEnumerator<TFmt> arg0, IStringBuilder sb, string? formatString = null)
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
            FormatCollectionStart(elementType, sb, hasNext);
        }
        var itemCount   = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                FormatCollectionNextItem(item, itemCount, sb);
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
                FormatCollectionNextItem(item, itemCount, sb);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionEnd(elementType, sb, itemCount);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IEnumerator<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
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
            addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex, !hasNext);
        }
        var itemCount   = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item                      = arg0.Current;
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += FormatCollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
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
                addedChars += FormatCollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int FormatCollection<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
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
            FormatCollectionStart(elementType, sb, hasNext);
        }
        var itemCount   = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item = arg0.Current;
                if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount);
                FormatCollectionNextItem(item, itemCount, sb);
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
                FormatCollectionNextItem(item, itemCount, sb);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            FormatCollectionEnd(elementType, sb, itemCount);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatCollection<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
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
            addedChars += FormatCollectionStart(elementType, destCharSpan, destStartIndex, !hasNext);
        }
        var itemCount   = 0;
        if (formatString.IsNullOrEmpty() || formatString == NoFormatFormatString)
        {
            while (hasNext)
            {
                var item                      = arg0.Current;
                if (itemCount > 0) addedChars += AddCollectionElementSeparator(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
                addedChars += FormatCollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars);
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
                addedChars += FormatCollectionNextItem(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !IgnoreEmptyCollection)
        {
            addedChars += FormatCollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int FormatCollectionStart(Type collectionType,  IStringBuilder sb, bool hasItems) => 
        sb.Append("[").ReturnCharCount(1);

    public override int FormatCollectionStart(Type collectionType, Span<char> destination, int destStartIndex, bool hasItems) =>
        destination.OverWriteAt(destStartIndex, "[");

    public override int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString) =>
        Format(nextItem, sb, formatString);

    public override int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString)
        => Format(nextItem, destCharSpan, destStartIndex, formatString);

    public override int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString) =>
        Format(nextItem, sb, formatString);

    public override int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString)
        => Format(nextItem, destCharSpan, destStartIndex, formatString);

    public override int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb)
    {
        var preAppendLen = sb.Length;
        sb.Append(nextItem);
        return sb.Length - preAppendLen;
    }

    public override int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex)
    {
        destination = destStartIndex > 0 ? destination[destStartIndex..] : destination;
        if (nextItem.TryFormat(destination, out var charsWritten, "", null)) return charsWritten;
        return 0;
    }

    public override int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb)
    {
        var preAppendLen = sb.Length;
        sb.Append(nextItem);
        return sb.Length - preAppendLen;
    }

    public override int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex)
    {
        if (nextItem == null) return IgnoreNullableValues ? 0 : destination.OverWriteAt(destStartIndex, NullString);
        return FormatCollectionNextItem(nextItem.Value, retrieveCount, destination, destStartIndex);
    }

    public override int FormatCollectionEnd(Type collectionType, IStringBuilder sb, int itemsCount) => sb.Append("]").ReturnCharCount(1);

    public override int FormatCollectionEnd(Type collectionType, Span<char> destination, int index, int itemsCount) =>
        destination.OverWriteAt(index, "]");
}
