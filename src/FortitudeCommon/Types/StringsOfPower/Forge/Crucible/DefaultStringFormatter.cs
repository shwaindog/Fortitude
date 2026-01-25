// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormatSwitches;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public class DefaultStringFormatter : CustomStringFormatter, ICustomStringFormatter
{

    public override int ProcessAppendedRange(IStringBuilder sb, int fromIndex) => 0;

    public override int ProcessAppendedRange(Span<char> destSpan, int fromIndex, int length) => 0;

    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : default
    {
        var preAppendLen = sb.Length;
        formatString ??= "";
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Length > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i, formatSwitches);
            CollectionNextItemFormat(arg0[i], i, sb, formatString, formatSwitches);
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatFlags = EncodeInnerContent) where TFmt : default
    {
        var addedChars = 0;
        formatString ??= "";
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars  += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0, formatFlags);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            if (i > 0)
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, i, formatFlags);
            addedChars += CollectionNextItemFormat(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString, formatFlags);
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length, formatFlags);
        }
        return addedChars;
    }

    public override int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) 
    {
        var preAppendLen = sb.Length;
        formatString ??= "";
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType       = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Length > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i, formatSwitches);
            var item = arg0[i];
            CollectionNextItemFormat(item, i, sb, formatString, formatSwitches);
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormatSwitches formatFlags = EncodeInnerContent)
    {
        var addedChars = 0;
        formatString ??= "";
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType       = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars  += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0, formatFlags);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            if (i > 0)
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, i, formatFlags);
            var item = arg0[i];
            addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatFlags);
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length, formatFlags);
        }
        return addedChars;
    }

    public override int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : default
    {
        var preAppendLen = sb.Length;
        formatString ??= "";
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType       = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Length > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i, formatSwitches);
            var item = arg0[i];
            CollectionNextItemFormat(item, i, sb, formatString, formatSwitches);
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : default
    {
        var addedChars = 0;
        formatString ??= "";
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars  += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            if (i > 0) 
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            var item = arg0[i];
            addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) 
    {
        var preAppendLen = sb.Length;
        formatString ??= "";
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType       = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Length > 0, formatSwitches);
        }
        if (elementType.IsEnum() && !Options.EnumsDefaultAsNumber && formatString.PrefixSuffixLength() == 0 && formatString.EnumFormatStringIsNotAsNumberFormatSequence())
        {
            formatSwitches |= EnsureFormattedDelimited;
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i, formatSwitches);
            var item = arg0[i];
            CollectionNextItemFormat(item, i, sb, formatString, formatSwitches);
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Length, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var addedChars = 0;
        formatString ??= "";
        if (arg0.Length == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType       = typeof(TFmtStruct);
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Length > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Length; i++)
        {
            if (i > 0) 
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            var item = arg0[i];
            addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
        }
        if (arg0.Length > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Length, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : default
    {
        var preAppendLen = sb.Length;
        formatString ??= "";
        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Count > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Count; i++)
        {
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i, formatSwitches);
            var item = arg0[i];
            CollectionNextItemFormat(item, i, sb, formatString, formatSwitches);
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Count, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : default
    {
        var addedChars = 0;
        formatString ??= "";
        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars  += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Count > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Count; i++)
        {
            if (i > 0) 
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            var item = arg0[i];
            addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var preAppendLen = sb.Length;
        formatString ??= "";
        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType       = typeof(TFmtStruct);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, arg0.Count > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Count; i++)
        {
            if (i > 0) AddCollectionElementSeparator(elementType, sb, i, formatSwitches);
            var item = arg0[i];
            CollectionNextItemFormat(item, i, sb, formatString, formatSwitches);
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, arg0.Count, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        var addedChars = 0;
        formatString ??= "";
        if (arg0.Count == 0 && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType       = typeof(TFmtStruct);
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, arg0.Count > 0, formatSwitches);
        }
        for (var i = 0; i < arg0.Count; i++)
        {
            if (i > 0) 
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, i, formatSwitches);
            var item = arg0[i];
            addedChars += CollectionNextItemFormat(item, i, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
        }
        if (arg0.Count > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, arg0.Count, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : default
    {
        formatString ??= "";
        var preAppendLen         = sb.Length;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                CollectionStart(elementType, sb, true, formatSwitches);
                hasStartedCollection = true;
            }
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount, formatSwitches);
            CollectionNextItemFormat(item, itemCount, sb, formatString, formatSwitches);
            itemCount++;
        }
        if (itemCount > 0)
        {
            CollectionEnd(elementType, sb, itemCount, formatSwitches);
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
                    CollectionStart(elementType, sb, false, formatSwitches);
                    CollectionEnd(elementType, sb, 0, formatSwitches);
                }
            }
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : default
    {
        formatString ??= "";
        var addedChars           = 0;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmt);
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true, formatSwitches);
                hasStartedCollection =  true;
            }
            if (itemCount > 0) 
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
            addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
            itemCount++;
        }
        if (itemCount > 0)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
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
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false, formatSwitches);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
                }
            }
        }
        return addedChars;
    }

    public override int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        formatString ??= "";
        var preAppendLen         = sb.Length;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmtStruct);
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                CollectionStart(elementType, sb, true);
                hasStartedCollection = true;
            }
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount, formatSwitches);
            CollectionNextItemFormat(item, itemCount, sb, formatString, formatSwitches);
            itemCount++;
        }
        if (itemCount > 0)
        {
            CollectionEnd(elementType, sb, itemCount, formatSwitches);
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
                    CollectionStart(elementType, sb, false, formatSwitches);
                    CollectionEnd(elementType, sb, 0, formatSwitches);
                }
            }
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        formatString ??= "";
        var addedChars           = 0;
        var hasStartedCollection = false;
        var itemCount            = 0;
        var elementType          = typeof(TFmtStruct);
        foreach (var item in arg0)
        {
            if (!hasStartedCollection)
            {
                addedChars           += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, true, formatSwitches);
                hasStartedCollection =  true;
            }
            if (itemCount > 0) 
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
            addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
            itemCount++;
        }
        if (itemCount > 0)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
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
                    addedChars += CollectionStart(elementType, destCharSpan, destStartIndex + addedChars, false, formatSwitches);
                    addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
                }
            }
        }
        return addedChars;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : default
    {
        formatString ??= "";
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !Options.IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, hasNext, formatSwitches);
        }
        var itemCount   = 0;
        while (hasNext)
        {
            var item = arg0.Current;
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount, formatSwitches);
            CollectionNextItemFormat(item, itemCount, sb, formatString, formatSwitches);
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, itemCount, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
    where TFmt : default
    {
        formatString ??= "";
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType = typeof(TFmt);
        if (!hasNext || !Options.IgnoreEmptyCollection)
        {
            addedChars  += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext, formatSwitches);
        }
        var itemCount   = 0;
        while (hasNext)
        {
            var item                      = arg0.Current;
            if (itemCount > 0) 
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
            addedChars += CollectionNextItemFormat(item, itemCount++, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
        }
        return addedChars;
    }

    public override int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        formatString ??= "";
        var preAppendLen = sb.Length;
        var hasNext      = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length);
        }
        var elementType       = typeof(TFmtStruct);
        if (!hasNext || !Options.IgnoreEmptyCollection)
        {
            CollectionStart(elementType, sb, hasNext, formatSwitches);
        }
        var itemCount   = 0;
        while (hasNext)
        {
            var item = arg0.Current;
            if (itemCount > 0) AddCollectionElementSeparator(elementType, sb, itemCount, formatSwitches);
            CollectionNextItemFormat(item, itemCount, sb, formatString, formatSwitches);
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            CollectionEnd(elementType, sb, itemCount, formatSwitches);
        }
        return sb.Length - preAppendLen;
    }

    public override int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex
      , string? formatString = null, FormatSwitches formatSwitches = EncodeInnerContent)
    {
        formatString ??= "";
        var addedChars = 0;
        var hasNext    = arg0.MoveNext();

        if (!hasNext && Options is { IgnoreEmptyCollection: false, EmptyCollectionWritesNull: true })
        {
            return destCharSpan.OverWriteAt(destStartIndex, Options.NullString);
        }
        var elementType       = typeof(TFmtStruct);
        if (!hasNext || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionStart(elementType, destCharSpan, destStartIndex, !hasNext, formatSwitches);
        }
        var itemCount   = 0;
        while (hasNext)
        {
            var item                      = arg0.Current;
            if (itemCount > 0) 
                addedChars += AddCollectionElementSeparatorAndPadding
                    (elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
            addedChars += CollectionNextItemFormat(item, itemCount, destCharSpan, destStartIndex + addedChars, formatString, formatSwitches);
            itemCount++;
            hasNext = arg0.MoveNext();
        }
        if (itemCount > 0 || !Options.IgnoreEmptyCollection)
        {
            addedChars += CollectionEnd(elementType, destCharSpan, destStartIndex + addedChars, itemCount, formatSwitches);
        }
        return addedChars;
    }

    public override int CollectionStart(Type collectionType,  IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        return sb.Append(SqBrktOpn).ReturnCharCount(1);
    }

    public override int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && collectionType.IsCharArray())
        {
            return 0;
        }
        return destSpan.OverWriteAt(destStartIndex, SqBrktOpn);
    }

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : default =>
        nextItem == null 
            ?  (Options.NullWritesEmpty 
                    ? 0 
                    : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length))
             : Format(nextItem, sb, formatString, formatSwitches);

    public override int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString, FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : default
        => 
            nextItem == null 
                ?  (Options.NullWritesEmpty 
                    ? 0 
                    : destCharSpan.AppendReturnAddCount(Options.NullString))
                : Format(nextItem, destCharSpan, destStartIndex, formatString);

    public override int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormatSwitches formatSwitches = EncodeInnerContent)  => Format(nextItem, sb, formatString);

    public override int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
      , string formatString, FormatSwitches formatSwitches = EncodeInnerContent) => 
        Format(nextItem, destCharSpan, destStartIndex, formatString);

    public override int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (nextItem == null)
        {
            return (Options.NullWritesEmpty ? 0 : sb.Append(Options.NullString).ReturnCharCount(Options.NullString.Length));
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
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (nextItem == null)
        {
            return Options.NullWritesEmpty ? 0 : destCharSpan.AppendReturnAddCount(Options.NullString);
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

    public override int CollectionEnd(Type elementType, IStringBuilder sb, int itemsCount
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray())
        {
            return 0;
        }
        return sb.Append(SqBrktCls).ReturnCharCount(1);
    }

    public override int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int itemsCount
      , FormatSwitches formatSwitches = EncodeInnerContent)
    {
        if (formatSwitches.TreatCharArrayAsString() && elementType.IsCharArray())
        {
            return 0;
        }
        CharSpanCollectionScratchBuffer?.DecrementRefCount();
        CharSpanCollectionScratchBuffer = null;
        return destSpan.OverWriteAt(destIndex, SqBrktCls);
    }

    public override ICustomStringFormatter Clone() => AlwaysRecycler.Borrow<DefaultStringFormatter>().CopyFrom(this);

    public override DefaultStringFormatter CopyFrom(ICustomStringFormatter source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        base.CopyFrom(source, copyMergeFlags);

        return this;
    }
}
