// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormattingHandlingFlags;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public interface ICustomStringFormatter : IRecyclableObject
{
    int AddCollectionElementSeparator(Type collectionType, IStringBuilder sb, int nextItemNumber
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    int AddCollectionElementPadding(Type collectionType, IStringBuilder sb, int nextItemNumber
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    int AddCollectionElementSeparatorAndPadding(Type collectionType, IStringBuilder sb, int nextItemNumber
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int AddCollectionElementSeparator(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int AddCollectionElementPadding(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int AddCollectionElementSeparatorAndPadding(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    int AppendDelimiterStart(Type maybeDelimited, IStringBuilder sb);
    int AppendDelimiterEnd(Type maybeDelimited, IStringBuilder sb);
    int AppendDelimiterStart(Type maybeDelimited, Span<char> destSpan, int fromIndex);
    int AppendDelimiterEnd(Type maybeDelimited, Span<char> destSpan, int fromIndex);

    IFormattingOptions Options { get; set; }
    IEncodingTransfer ContentEncoder { get; set; }
    IEncodingTransfer LayoutEncoder { get; set; }
    
    FormattingHandlingFlags ResolveStringFormattingFlags<T>(char lastNonWhiteSpace, T input, FormattingHandlingFlags callerFormattingFlags
      , string formatString = "");

    int ProcessAppendedRange(IStringBuilder sb, int fromIndex);
    int ProcessAppendedRange(Span<char> destSpan, int fromIndex, int length);

    int Format(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    int Format(char[] source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    int Format(StringBuilder source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int Format(char[] source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int Format(StringBuilder source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int Format(ICharSequence source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue, FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int Format<TFmt>(TFmt source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?;
    
    int Format<TFmt>(TFmt source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?;

    int Format(bool source, IStringBuilder sb, ReadOnlySpan<char> formatString, FormattingHandlingFlags formatFlags = DefaultCallerTypeFlags);

    int Format(bool source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = DefaultCallerTypeFlags);

    int Format(bool? source, IStringBuilder sb, ReadOnlySpan<char> formatString, FormattingHandlingFlags formatFlags = DefaultCallerTypeFlags);

    int Format(bool? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = DefaultCallerTypeFlags);

    int FormatBoolEnumerator(IEnumerator<bool> arg0, IStringBuilder sb, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int FormatBoolEnumerator(IEnumerator<bool> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int FormatBoolEnumerator(IEnumerator<bool?> arg0, IStringBuilder sb, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int FormatBoolEnumerator(IEnumerator<bool?> arg0, Span<char> destCharSpan, int destStartIndex
    , string? formatString = null, FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int Format<TFmtStruct>(TFmtStruct? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int Format<TFmtStruct>(TFmtStruct? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int TryFormat<TAny>(TAny source, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, IStringBuilder sb, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormattingHandlingFlags formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;


    int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int CollectionNextItemFormat(bool nextItem, int retrieveCount, IStringBuilder sb, string formatString
    , FormattingHandlingFlags formatFlags = DefaultCallerTypeFlags);

    int CollectionNextItemFormat(bool nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString
    , FormattingHandlingFlags formatFlags = DefaultCallerTypeFlags);
    
    int CollectionNextItemFormat(bool? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormattingHandlingFlags formatFlags = DefaultCallerTypeFlags);

    int CollectionNextItemFormat(bool? nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString
    , FormattingHandlingFlags formatFlags = DefaultCallerTypeFlags);

    int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?;

    int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?;

    int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, IStringBuilder sb, string formatString
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, Span<char> destination, int destStartIndex
      , string formatString, FormattingHandlingFlags formatFlags = EncodeInnerContent) 
        where TFmtStruct : struct, ISpanFormattable;

    int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb, FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);


    int CollectionEnd(Type collectionType, IStringBuilder sb, int totalItemCount
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);

    int CollectionEnd(Type collectionType, Span<char> destSpan, int destIndex, int totalItemCount
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);


    public static ICustomStringFormatter DefaultBufferFormatter { get; set; } =
        new DefaultStringFormatter
        {
            Options = new FormattingOptions.FormattingOptions()
        };
    
    public static ICustomStringFormatter DefaultPassThroughFormatter { get; set; } =
        new DefaultStringFormatter
        {
            Options = new FormattingOptions.FormattingOptions()
        };
    
    public static ICustomStringFormatter DefaultAsciiEscapeFormatter { get; set; } =
        new DefaultStringFormatter
        {
            Options = new FormattingOptions.FormattingOptions()
        };
    
    public static ICustomStringFormatter DefaultJsonEncodingFormatter { get; set; } =
        new DefaultStringFormatter
        {
            Options = new FormattingOptions.FormattingOptions()
        };
}

public static class CustomStringFormatterExtensions
{
    public static int ReturnCharCount(this IStringBuilder sb, int charsAdded) => charsAdded;
    public static int ReturnInt<T>(this T _,  int charsAdded) => charsAdded;
    
    
}
