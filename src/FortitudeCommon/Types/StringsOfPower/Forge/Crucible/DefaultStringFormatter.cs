// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormattingHandlingFlags;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public class DefaultStringFormatter : CustomStringFormatter, ICustomStringFormatter
{

    public override int ProcessAppendedRange(IStringBuilder sb, int fromIndex) => 0;

    public override int ProcessAppendedRange(Span<char> destSpan, int fromIndex, int length) => 0;

    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : default
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : default
    {
        var addedChars = 0;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) 
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : default
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : default
    {
        var addedChars = 0;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) 
    {
        var preAppendLen = sb.Length;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var addedChars = 0;
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public override int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : default
    {
        var preAppendLen = sb.Length;
        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Count);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : default
    {
        var addedChars = 0;
        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public override int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var preAppendLen = sb.Length;
        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Count);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var addedChars = 0;
        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmtStruct);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
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
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : default
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
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull)
                {
                    sb.Append(Options.NullString);
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

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : default
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
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull)
                {
                    addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, Options.NullString);
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

    public override int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var preAppendLen         = sb.Length;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmtStruct);
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
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull)
                {
                    sb.Append(Options.NullString);
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

    public override int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var addedChars           = 0;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmtStruct);
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
            if (!Options.IgnoreEmptyCollection)
            {
                if (Options.EmptyCollectionWritesNull)
                {
                    addedChars += destCharSpan.OverWriteAt(destStartIndex + addedChars, Options.NullString);
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

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : default
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !Options.IgnoreEmptyCollection)
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
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, itemCount);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    where TFmt : default
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !Options.IgnoreEmptyCollection)
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
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmtStruct);
        if (!hasNext || !Options.IgnoreEmptyCollection)
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
                CollectionNextItemFormat(item, itemCount, sb, formatString);
                itemCount++;
                hasNext = arg0.MoveNext();
            }
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, itemCount);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmtStruct);
        if (!hasNext || !Options.IgnoreEmptyCollection)
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
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount);
        }
        return addedChars;
    }

    public override int CollectionStart(Type collectionType,  IStringBuilder sb, bool hasItems
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        return sb.Append(SqBrktOpn).ReturnCharCount(1);
    }

    public override int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        return destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : default =>
        nextItem == null 
            ?  (Options.NullWritesNothing 
                    ? 0 
                    : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length))
             : Format(nextItem, sb, formatString);

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString, FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : default
        => 
            nextItem == null 
                ?  (Options.NullWritesNothing 
                    ? 0 
                    : destCharSpan.AppendReturnAddCount(Options.NullString))
                : Format(nextItem, destCharSpan, destStartIndex, formatString);

    public override int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)  => Format(nextItem, sb, formatString);

    public override int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString, FormattingHandlingFlags formatFlags = EncodeInnerContent) => 
        Format(nextItem, destCharSpan, destStartIndex, formatString);

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (nextItem == null)
        {
            return (Options.NullWritesNothing ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length));
        }
        switch (nextItem)
        {
            case ISpanFormattable sourceFmt:
                return CollectionNextItemFormat(sourceFmt, retrieveCount, sb, "");
            case string sourceString:
                return Format(sourceString, retrieveCount, sb, "");
            case char[] sourceCharArray:
                return Format(sourceCharArray, retrieveCount, sb, "");
            case ICharSequence sourceCharSequence:
                return Format(sourceCharSequence, retrieveCount, sb, "");
            case StringBuilder sourceStringBuilder:
                return Format(sourceStringBuilder, retrieveCount, sb, "");
        }
        var preAppendLen = sb.Length;
        sb.Append(nextItem);
        return sb.Length - preAppendLen;
    }

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (nextItem == null)
        {
            return Options.NullWritesNothing ? 0 : destCharSpan.AppendReturnAddCount(Options.NullString);
        }
        switch (nextItem)
        {
            case ISpanFormattable sourceFmt:
                return Format(sourceFmt, destCharSpan, destStartIndex, "");
            case string sourceString:
                return Format(sourceString, 0, destCharSpan, "", destStartIndex );
            case char[] sourceCharArray:
                return Format(sourceCharArray, 0, destCharSpan, "", destStartIndex);
            case ICharSequence sourceCharSequence:
                return Format(sourceCharSequence, 0, destCharSpan, "", destStartIndex);
            case StringBuilder sourceStringBuilder:
                return Format(sourceStringBuilder, 0, destCharSpan, "", destStartIndex);
        }
        CharSpanCollectionScratchBuffer ??= MutableString.MediumScratchBuffer;
        CharSpanCollectionScratchBuffer.Clear();
        CharSpanCollectionScratchBuffer.Append(nextItem);
        return destCharSpan.OverWriteAt(destStartIndex, CharSpanCollectionScratchBuffer);
    }

    public override int CollectionEnd(Type collectionType, IStringBuilder sb, int itemsCount
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        return sb.Append(SqBrktCls).ReturnCharCount(1);
    }

    public override int CollectionEnd(Type collectionType, Span<char> destSpan, int index, int itemsCount
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
    {
        if (formatFlags.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        return destSpan.OverWriteAt(index, SqBrktCls);
    }
}
