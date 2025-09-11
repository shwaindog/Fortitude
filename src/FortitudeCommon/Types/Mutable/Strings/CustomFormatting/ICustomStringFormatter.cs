// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;

namespace FortitudeCommon.Types.Mutable.Strings.CustomFormatting;

public interface ICustomStringFormatter
{
    public string ItemSeparator { get; set; }

    int AddCollectionElementSeparator(Type collectionType, IStringBuilder sb, int nextItemNumber);

    int AddCollectionElementSeparator(Type collectionType, Span<char> charSpan, int atSpanOffset, int nextItemNumber);

    bool IgnoreNullableValues { get; set; }
    bool EmptyCollectionWritesNull { get; set; }
    bool IgnoreEmptyCollection { get; set; }

    int Transfer(char[] source, IStringBuilder sb);
    int Transfer(char[] source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue);
    int Transfer(char[] source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue);
    int Transfer(char[] source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue);

    int Transfer(ReadOnlySpan<char> source, IStringBuilder sb);
    int Transfer(ReadOnlySpan<char> source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue);

    int Transfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue);

    int Transfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue);


    int Transfer(StringBuilder source, IStringBuilder sb);
    int Transfer(StringBuilder source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue);
    int Transfer(StringBuilder source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue);
    int Transfer(StringBuilder source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue);


    int Transfer(ICharSequence source, IStringBuilder sb);
    int Transfer(ICharSequence source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue);
    int Transfer(ICharSequence source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue);
    int Transfer(ICharSequence source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue);


    int Format(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue);
    int Format(char[] source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue);
    int Format(StringBuilder source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue);

    int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue);

    int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int Format(char[] source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex, int maxTransferCount = int.MaxValue);

    int Format(StringBuilder source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int Format(ICharSequence source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue);

    int Format<TFmt>(TFmt? source, IStringBuilder sb, ReadOnlySpan<char> formatString) where TFmt : ISpanFormattable;
    int Format<TFmt>(TFmt? source, Span<char> destination, int destStartIndex, ReadOnlySpan<char> formatString) where TFmt : ISpanFormattable;

    int Format<TFmt>(TFmt? source, IStringBuilder sb, ReadOnlySpan<char> formatString) where TFmt : struct, ISpanFormattable;

    int Format<TFmt>(TFmt? source, Span<char> destination, int destStartIndex, ReadOnlySpan<char> formatString) where TFmt : struct, ISpanFormattable;


    int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(TFmt[] arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(TFmt[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(IEnumerable<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(IEnumerable<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(IEnumerator<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(IEnumerator<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    int CollectionStart(Type collectionType,  IStringBuilder sb, bool hasItems);
    int CollectionStart(Type collectionType, Span<char> destination, int destStartIndex, bool hasItems);

    int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString) where TFmt : ISpanFormattable;

    int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString)
        where TFmt : ISpanFormattable;

    int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString)
        where TFmt : struct, ISpanFormattable;

    int CollectionNextItemFormat<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString)
        where TFmt : struct, ISpanFormattable;

    int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb);
    int CollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex);


    int CollectionEnd(Type collectionType, IStringBuilder sb, int totalItemCount);
    
    int CollectionEnd(Type collectionType, Span<char> destination, int index, int totalItemCount);


    public static ICustomStringFormatter DefaultBufferFormatter { get; set; } = new DefaultStringFormatter();
}

public static class CustomStringFormatterExtensions
{
    public static int ReturnCharCount(this IStringBuilder sb, int charsAdded) => charsAdded;
}
