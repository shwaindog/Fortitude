// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public interface ICustomStringFormatter : IRecyclableObject
{
    int AddCollectionElementSeparator(Type collectionType, IStringBuilder sb, int nextItemNumber);

    int AddCollectionElementSeparator(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber);

    IFormattingOptions Options { get; set; }

    int ProcessAppendedRange(IStringBuilder sb, int fromIndex);
    int ProcessAppendedRange(Span<char> destSpan, int fromIndex, int length);

    int Format(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue);
    int Format(char[] source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue);
    int Format(StringBuilder source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue);

    int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue);

    int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int Format(char[] source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int Format(StringBuilder source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int Format(ICharSequence source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int Format<TFmt>(TFmt? source, IStringBuilder sb, ReadOnlySpan<char> formatString) where TFmt : ISpanFormattable;
    int Format<TFmt>(TFmt? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString) where TFmt : ISpanFormattable;

    int Format<TFmtStruct>(TFmtStruct? source, IStringBuilder sb, ReadOnlySpan<char> formatString) where TFmtStruct : struct, ISpanFormattable;

    int Format<TFmtStruct>(TFmtStruct? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString) where TFmtStruct : struct, ISpanFormattable;

    int TryFormat<TAny>(TAny source, IStringBuilder sb, string formatString);

    int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, IStringBuilder sb, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null) where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmtStruct : struct, ISpanFormattable;


    int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems);
    int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems);

    int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString) where TFmt : ISpanFormattable;

    int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString)
        where TFmt : ISpanFormattable;

    int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, IStringBuilder sb, string formatString)
        where TFmtStruct : struct, ISpanFormattable;

    int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString)
        where TFmtStruct : struct, ISpanFormattable;

    int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb);
    int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex);


    int CollectionEnd(Type collectionType, IStringBuilder sb, int totalItemCount);

    int CollectionEnd(Type collectionType, Span<char> destSpan, int index, int totalItemCount);


    public static ICustomStringFormatter DefaultBufferFormatter { get; set; } =
        new DefaultStringFormatter
        {
            Options = new FormattingOptions.FormattingOptions()
        };
}

public static class CustomStringFormatterExtensions
{
    public static int ReturnCharCount(this IStringBuilder sb, int charsAdded) => charsAdded;
}
