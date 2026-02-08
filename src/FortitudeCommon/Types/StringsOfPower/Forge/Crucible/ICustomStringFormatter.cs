// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible.FormattingOptions;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormatSwitches;

namespace FortitudeCommon.Types.StringsOfPower.Forge.Crucible;

public interface ICustomStringFormatter : IReusableObject<ICustomStringFormatter>
{
  
    int AddCollectionElementSeparator(Type collectionType, IStringBuilder sb, int nextItemNumber
    , FormatSwitches formatSwitches = EncodeInnerContent);
    
    int AddCollectionElementPadding(Type collectionType, IStringBuilder sb, int nextItemNumber
    , FormatSwitches formatFlags = EncodeInnerContent);
    
    int AddCollectionElementSeparatorAndPadding(Type collectionType, IStringBuilder sb, int nextItemNumber
    , FormatSwitches formatFlags = EncodeInnerContent);

    int AddCollectionElementSeparator(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
    , FormatSwitches formatSwitches = EncodeInnerContent);

    int AddCollectionElementPadding(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
    , FormatSwitches formatFlags = EncodeInnerContent);

    int AddCollectionElementSeparatorAndPadding(Type collectionType, Span<char> destSpan, int atSpanOffset, int nextItemNumber
    , FormatSwitches formatFlags = EncodeInnerContent);

    FormatStyle FormattingStyle { get; }

    int AppendDelimiterStart(Type maybeDelimited, IStringBuilder sb);
    int AppendDelimiterEnd(Type maybeDelimited, IStringBuilder sb);
    int AppendDelimiterStart(Type maybeDelimited, Span<char> destSpan, int fromIndex);
    int AppendDelimiterEnd(Type maybeDelimited, Span<char> destSpan, int fromIndex);

    IFormattingOptions Options { get; set; }
    IEncodingTransfer ContentEncoder { get; set; }
    IEncodingTransfer LayoutEncoder { get; set; }
    
    FormatSwitches ResolveStringFormattingFlags<T>(char lastNonWhiteSpace, T input, FormatSwitches callerFormattingFlags
      , string formatString = "");

    int ProcessAppendedRange(IStringBuilder sb, int fromIndex, FormatSwitches formatSwitches = DefaultCallerTypeFlags);
    int ProcessAppendedRange(Span<char> destSpan, int fromIndex, int length, FormatSwitches formatSwitches = DefaultCallerTypeFlags);

    int Format(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormatSwitches formatFlags = EncodeInnerContent);
    
    int Format(char[] source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormatSwitches formatFlags = EncodeInnerContent);
    
    int Format(StringBuilder source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormatSwitches formatFlags = EncodeInnerContent);

    int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, ReadOnlySpan<char> formatString, int maxTransferCount = int.MaxValue
      , FormatSwitches formatFlags = EncodeInnerContent);

    int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue, FormatSwitches formatFlags = EncodeInnerContent);

    int Format(char[] source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue, FormatSwitches formatFlags = EncodeInnerContent);

    int Format(StringBuilder source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue, FormatSwitches formatFlags = EncodeInnerContent);

    int Format(ICharSequence source, int sourceFrom, Span<char> destCharSpan, ReadOnlySpan<char> formatString, int destStartIndex
      , int maxTransferCount = int.MaxValue, FormatSwitches formatFlags = EncodeInnerContent);

    int Format<TFmt>(TFmt source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormatSwitches formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable?;
    
    int Format<TFmt>(TFmt source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable?;

    int Format(bool source, IStringBuilder sb, ReadOnlySpan<char> formatString, FormatSwitches formatFlags = DefaultCallerTypeFlags);

    int Format(bool source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = DefaultCallerTypeFlags);

    int Format(bool? source, IStringBuilder sb, ReadOnlySpan<char> formatString, FormatSwitches formatFlags = DefaultCallerTypeFlags);

    int Format(bool? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = DefaultCallerTypeFlags);

    int FormatBoolEnumerator(IEnumerator<bool> arg0, IStringBuilder sb, string? formatString = null
    , FormatSwitches formatFlags = EncodeInnerContent);

    int FormatBoolEnumerator(IEnumerator<bool> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormatSwitches formatFlags = EncodeInnerContent);

    int FormatBoolEnumerator(IEnumerator<bool?> arg0, IStringBuilder sb, string? formatString = null
    , FormatSwitches formatFlags = EncodeInnerContent);

    int FormatBoolEnumerator(IEnumerator<bool?> arg0, Span<char> destCharSpan, int destStartIndex
    , string? formatString = null, FormatSwitches formatFlags = EncodeInnerContent);

    int Format<TFmtStruct>(TFmtStruct? source, IStringBuilder sb, ReadOnlySpan<char> formatString
      , FormatSwitches formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int Format<TFmtStruct>(TFmtStruct? source, Span<char> destCharSpan, int destStartIndex, ReadOnlySpan<char> formatString
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int TryFormat<TAny>(TAny source, IStringBuilder sb, string formatString
      , FormatSwitches formatFlags = EncodeInnerContent);

    int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatReadOnlySpan<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatFlags = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatReadOnlySpan<TFmtStruct>(ReadOnlySpan<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatFlags = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatArray<TFmt>(TFmt?[] arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatArray<TFmt>(TFmt?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, IStringBuilder sb, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatArray<TFmtStruct>(TFmtStruct?[] arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
      , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatList<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatList<TFmtStruct>(IReadOnlyList<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, IStringBuilder sb, string? formatString = null
    , FormatSwitches formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatEnumerable<TFmt>(IEnumerable<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
    , FormatSwitches formatFlags = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerable<TFmtStruct>(IEnumerable<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, IStringBuilder sb, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable;

    int FormatEnumerator<TFmt>(IEnumerator<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmt : ISpanFormattable;

    int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, IStringBuilder sb, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int FormatEnumerator<TFmtStruct>(IEnumerator<TFmtStruct?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null
    , FormatSwitches formatSwitches = EncodeInnerContent)
        where TFmtStruct : struct, ISpanFormattable;


    int CollectionStart(Type collectionType, IStringBuilder sb, bool hasItems
      , FormatSwitches formatSwitches = EncodeInnerContent);
    
    int CollectionStart(Type collectionType, Span<char> destSpan, int destStartIndex, bool hasItems
    , FormatSwitches formatSwitches = EncodeInnerContent);

    int CollectionNextItemFormat(bool nextItem, int retrieveCount, IStringBuilder sb, string formatString
    , FormatSwitches formatSwitches = DefaultCallerTypeFlags);

    int CollectionNextItemFormat(bool nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString
    , FormatSwitches formatSwitches = DefaultCallerTypeFlags);
    
    int CollectionNextItemFormat(bool? nextItem, int retrieveCount, IStringBuilder sb, string formatString
      , FormatSwitches formatSwitches = DefaultCallerTypeFlags);

    int CollectionNextItemFormat(bool? nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString
    , FormatSwitches formatSwitches = DefaultCallerTypeFlags);

    int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString
    , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable?;

    int CollectionNextItemFormat<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString
    , FormatSwitches formatSwitches = EncodeInnerContent) where TFmt : ISpanFormattable?;

    int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, IStringBuilder sb, string formatString
    , FormatSwitches formatSwitches = EncodeInnerContent) where TFmtStruct : struct, ISpanFormattable;

    int CollectionNextItemFormat<TFmtStruct>(TFmtStruct? nextItem, int retrieveCount, Span<char> destination, int destStartIndex
      , string formatString, FormatSwitches formatSwitches = EncodeInnerContent) 
        where TFmtStruct : struct, ISpanFormattable;

    int CollectionNextItem<T>(T nextItem, int retrieveCount, IStringBuilder sb, FormatSwitches formatSwitches = EncodeInnerContent);
    
    int CollectionNextItem<T>(T nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex
    , FormatSwitches formatSwitches = EncodeInnerContent);


    int CollectionEnd(Type elementType, IStringBuilder sb, int totalItemCount
    , FormatSwitches formatSwitches = EncodeInnerContent);

    int CollectionEnd(Type elementType, Span<char> destSpan, int destIndex, int totalItemCount
    , FormatSwitches formatSwitches = EncodeInnerContent);


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
