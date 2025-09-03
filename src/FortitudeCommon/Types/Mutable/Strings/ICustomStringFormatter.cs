// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.DataStructures.Memory.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString;

namespace FortitudeCommon.Types.Mutable.Strings;

public interface ICustomStringFormatter
{
    const int UnknownNumber = -1;


    public string ItemSeparator { get; set; }

    int AddItemSeparator(IStringBuilder sb);

    int AddItemSeparator(Span<char> charSpan, int atIndex);

    bool IgnoreNullableValues { get; set; }

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


    int Format(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, string formatString, int maxTransferCount = int.MaxValue);
    int Format(char[] source, int sourceFrom, IStringBuilder sb, string formatString, int maxTransferCount = int.MaxValue);
    int Format(StringBuilder source, int sourceFrom, IStringBuilder sb, string formatString, int maxTransferCount = int.MaxValue);
    
    int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, string formatString, int maxTransferCount = int.MaxValue);

    int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, string formatString, int destStartIndex, int maxTransferCount = int.MaxValue);
    int Format(char[] source, int sourceFrom, Span<char> destCharSpan, string formatString, int destStartIndex, int maxTransferCount = int.MaxValue);
    int Format(StringBuilder source, int sourceFrom, Span<char> destCharSpan, string formatString, int destStartIndex, int maxTransferCount = int.MaxValue);
    int Format(ICharSequence source, int sourceFrom, Span<char> destCharSpan, string formatString, int destStartIndex, int maxTransferCount = int.MaxValue);
    
    int Format<TFmt>(TFmt? source, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;
    int Format<TFmt>(TFmt? source, Span<char> destination, int destStartIndex, string? formatString = null) where TFmt : ISpanFormattable;

    int Format<TFmt>(TFmt? source, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;

    int Format<TFmt>(TFmt? source, Span<char> destination, int destStartIndex, string? formatString = null) where TFmt : struct, ISpanFormattable;


    int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable;

    int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable;

    int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable;

    int FormatCollectionStart<TFmt>(IStringBuilder sb, bool hasItems) where TFmt : ISpanFormattable;
    int FormatCollectionStart<TFmt>(Span<char> destination, int destStartIndex, bool hasItems) where TFmt : ISpanFormattable;

    int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString) where TFmt : ISpanFormattable;

    int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString)
        where TFmt : ISpanFormattable;

    int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString)
        where TFmt : struct, ISpanFormattable;

    int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex, string formatString)
        where TFmt : struct, ISpanFormattable;

    int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb) where TFmt : ISpanFormattable;
    int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex) where TFmt : ISpanFormattable;

    int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb) where TFmt : struct, ISpanFormattable;

    int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex)
        where TFmt : struct, ISpanFormattable;


    int FormatCollectionEnd<TFmt>(IStringBuilder sb, int totalItemCount) where TFmt : ISpanFormattable;
    int FormatCollectionEnd<TFmt>(Span<char> destination, int index, int totalItemCount) where TFmt : ISpanFormattable;


    public static ICustomStringFormatter DefaultBufferFormatter { get; set; } = new DefaultStringFormatter();
}

public class CustomStringFormatter
{
    public const string DefaultNullString = "null";
    
    protected static readonly ConcurrentDictionary<Type, ICustomFormattableProvider> GlobalCustomSpanFormattableProviders = new();

    protected static readonly ConcurrentDictionary<Type, ICustomFormattableProvider> GlobalCustomStyledToStringFormattableProviders = new();

    protected static readonly string[] RawByteHex;
    protected static readonly string[] DotNetStringByteHex;

    static CustomStringFormatter()
    {
        RawByteHex          = new string[256];
        DotNetStringByteHex = new string[256];
        for (int i = 0; i < 256; i++)
        {
            RawByteHex[i]          = $"{i:X2}";
            DotNetStringByteHex[i] = $"\\0x{i:X2}";
        }
    }

    public string ItemSeparator { get; set; } = ", ";

    public bool IgnoreNullableValues { get; set; } = false;


    public string NullString { get; set; } = DefaultNullString;


    public virtual int AddItemSeparator(IStringBuilder sb) => sb.Append(ItemSeparator).ReturnCharCount(ItemSeparator.Length);

    public virtual int AddItemSeparator(Span<char> charSpan, int atIndex ) => 
        charSpan.OverWriteAt(atIndex, ItemSeparator);

    protected virtual bool TryGetCachedCustomSpanFormatter<T>([NotNullWhen(true)] out CustomSpanFormattable<T>? maybeFormatter)
    {
        maybeFormatter = null;
        if (GlobalCustomSpanFormattableProviders.TryGetValue(typeof(T), out var formattableProvider))
        {
            if (formattableProvider.SupportSpanFormattable && formattableProvider is ICustomSpanFormattableProvider<T> spanFormattableProvider)
            {
                maybeFormatter = spanFormattableProvider.CustomSpanFormattable;
                return true;
            }
        }
        return false;
    }

    protected virtual bool TryGetCachedCustomStyledToStringFormatter<T>([NotNullWhen(true)] out CustomTypeStyler<T>? maybeFormatter)
    {
        maybeFormatter = null;
        if (GlobalCustomStyledToStringFormattableProviders.TryGetValue(typeof(T), out var formattableProvider))
        {
            if (formattableProvider.SupportStyleToString && formattableProvider is ICustomTypeStylerProvider<T> spanFormattableProvider)
            {
                maybeFormatter = spanFormattableProvider.CustomTypeStyler;
                return true;
            }
        }
        return false;
    }

    protected virtual bool TryAddCustomSpanFormattableProvider(Type registeredType, ICustomFormattableProvider toRegister) =>
        GlobalCustomSpanFormattableProviders.TryAdd(registeredType, toRegister);

    protected virtual bool TryAddCustomStyledToStringProvider(Type registeredType, ICustomFormattableProvider toRegister) =>
        GlobalCustomStyledToStringFormattableProviders.TryAdd(registeredType, toRegister);
}

public class DefaultStringFormatter : CustomStringFormatter, ICustomStringFormatter
{
    public int Transfer(char[] source, IStringBuilder sb)
    {
        var preAppendLength = sb.Length;
        sb.Append(source);
        return sb.Length - preAppendLength;
    }


    public int Transfer(char[] source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length, destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++)
        {
            destination[i + destStartIndex] = source[i];
        }
        return cappedLength;
    }

    public int Transfer(char[] source, int sourceFrom, IStringBuilder sb, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public int Transfer(char[] source, int sourceFrom, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++)
        {
            destination[i + destStartIndex] = source[i];
        }
        return cappedLength;
    }

    public int Transfer(ReadOnlySpan<char> source, IStringBuilder sb)
    {
        var preAppendLength = sb.Length;
        sb.Append(source);
        return sb.Length - preAppendLength;
    }


    public int Transfer(ReadOnlySpan<char> source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(maxTransferCount, Math.Min(source.Length, destination.Length - destStartIndex));
        for (var i = 0; i < cappedLength; i++)
        {
            destination[i + destStartIndex] = source[i];
        }
        return cappedLength;
    }

    public int Transfer(ReadOnlySpan<char> source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++)
        {
            destination[i + destStartIndex] = source[i];
        }
        return cappedLength;
    }

    public int Transfer(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public int Transfer(StringBuilder source, IStringBuilder sb)
    {
        var preAppendLength = sb.Length;
        sb.Append(source);
        return sb.Length - preAppendLength;
    }

    public int Transfer(StringBuilder source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length, destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++)
        {
            destination[i + destStartIndex] = source[i];
        }
        return cappedLength;
    }

    public int Transfer(StringBuilder source, int sourceFrom, IStringBuilder sb
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public int Transfer(StringBuilder source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++)
        {
            destination[i + destStartIndex] = source[i];
        }
        return cappedLength;
    }

    public int Transfer(ICharSequence source, IStringBuilder sb)
    {
        var preAppendLength = sb.Length;
        sb.Append(source);
        return sb.Length - preAppendLength;
    }

    public int Transfer(ICharSequence source, Span<char> destination, int destStartIndex = 0, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length, destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++)
        {
            destination[i + destStartIndex] = source[i];
        }
        return cappedLength;
    }

    public int Transfer(ICharSequence source, int sourceFrom, IStringBuilder sb
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sb.Append(source, sourceFrom, cappedLength);
        return cappedLength;
    }

    public int Transfer(ICharSequence source, int sourceFrom, Span<char> destination, int destStartIndex = 0
      , int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(Math.Min(source.Length - sourceFrom, maxTransferCount), destination.Length - destStartIndex);
        for (var i = 0; i < cappedLength; i++)
        {
            destination[i + destStartIndex] = source[i];
        }
        return cappedLength;
    }

    public int Format(ReadOnlySpan<char> source, int sourceFrom, IStringBuilder sb, string? formatString = null, int maxTransferCount = int.MaxValue)
    {
        if (source.Length == 0) return 0;
        if(formatString.IsNullOrEmpty() || formatString == "{0}") return Transfer(source, sb);
        
        var formatSpan         = formatString.AsSpan();
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf('{');
        if (indexOfOpenBracket > 0)
        {
            sb.Append(formatString[..indexOfOpenBracket]);
            formatSpan = formatSpan[indexOfOpenBracket..];
            charsAdded = indexOfOpenBracket;
        }
        formatSpan.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatSpan.IndexOf( '}') + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sb);
            if (afterClosingBracket < formatSpan.Length)
            {
                sb.Append(formatSpan[afterClosingBracket..]);
                charsAdded += formatSpan.Length - afterClosingBracket;
            }
            return charsAdded;
        }

        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        var end = sourceFrom+cappedLength;
        if (source.Length < 4096)
        {
            var padSpan = stackalloc char[source.Length + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(source[sourceFrom..end], layout);
            sb.Append(padSpan[..padSize]);
            if (afterClosingBracket < formatSpan.Length)
            {
                sb.Append(formatSpan[afterClosingBracket..]);
                charsAdded += formatSpan.Length - afterClosingBracket;
            }
            return charsAdded;
        }
        var charBuffer = (source.Length + 256).SourceRecyclingCharArray();
        var padBufferSpan    = charBuffer.RemainingAsSpan();
        var paBufferSize    = padBufferSpan.PadAndAlign(source[sourceFrom..end], layout);
        sb.Append(padBufferSpan[..paBufferSize]);
        charBuffer.DecrementRefCount();
        charsAdded += paBufferSize;
        if (afterClosingBracket < formatSpan.Length)
        {
            sb.Append(formatSpan[afterClosingBracket..]);
            charsAdded += formatSpan.Length - afterClosingBracket;
        }
        return charsAdded;
    }

    public int Format(char[] source, int sourceFrom, IStringBuilder sb, string formatString, int maxTransferCount = int.MaxValue)
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if(formatString.IsNullOrEmpty() || formatString == "{0}") return Transfer(source, sb);
        var formatSpan         = formatString.AsSpan();
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf('{');
        if (indexOfOpenBracket > 0)
        {
            sb.Append(formatString[..indexOfOpenBracket]);
            formatSpan = formatSpan[indexOfOpenBracket..];
            charsAdded = indexOfOpenBracket;
        }
        formatSpan.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatSpan.IndexOf( '}') + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sb);
            if (afterClosingBracket < formatSpan.Length)
            {
                sb.Append(formatSpan[afterClosingBracket..]);
                charsAdded += formatSpan.Length - afterClosingBracket;
            }
            return charsAdded;
        }

        if (source.Length < 4096)
        {
            var padSpan = stackalloc char[source.Length + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(source, layout);
            sb.Append(padSpan[..padSize]);
            charsAdded += padSize;
            if (afterClosingBracket < formatSpan.Length)
            {
                sb.Append(formatSpan[afterClosingBracket..]);
                charsAdded += formatSpan.Length - afterClosingBracket;
            }
            return charsAdded;
        }
        var charBuffer    = (source.Length + 256).SourceRecyclingCharArray();
        var padBufferSpan = charBuffer.RemainingAsSpan();
        var paBufferSize  = padBufferSpan.PadAndAlign(source, layout);
        sb.Append(padBufferSpan[..paBufferSize]);
        charBuffer.DecrementRefCount();
        charsAdded += paBufferSize;
        if (afterClosingBracket < formatSpan.Length)
        {
            sb.Append(formatSpan[afterClosingBracket..]);
            charsAdded += formatSpan.Length - afterClosingBracket;
        }
        return charsAdded;
    }

    public int Format(StringBuilder source, int sourceFrom, IStringBuilder sb
      , string formatString, int maxTransferCount = int.MaxValue) 
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if(formatString.IsNullOrEmpty() || formatString == "{0}") return Transfer(source, sb);
        var formatSpan         = formatString.AsSpan();
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf('{');
        if (indexOfOpenBracket > 0)
        {
            sb.Append(formatString[..indexOfOpenBracket]);
            formatSpan = formatSpan[indexOfOpenBracket..];
            charsAdded = indexOfOpenBracket;
        }
        formatSpan.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatSpan.IndexOf( '}') + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sb);
            if (afterClosingBracket < formatSpan.Length)
            {
                sb.Append(formatSpan[afterClosingBracket..]);
                charsAdded += formatSpan.Length - afterClosingBracket;
            }
            return charsAdded;
        }

        if (source.Length < 4096)
        {
            var sourceInSpan = stackalloc char[source.Length].ResetMemory();
            sourceInSpan.Append(source);
            var padSpan = stackalloc char[source.Length + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(sourceInSpan, layout);
            sb.Append(padSpan[..padSize]);
            charsAdded += padSize;
            if (afterClosingBracket < formatSpan.Length)
            {
                sb.Append(formatSpan[afterClosingBracket..]);
                charsAdded += formatSpan.Length - afterClosingBracket;
            }
            return charsAdded;
        }
        var sourceBuffer    = (source.Length).SourceRecyclingCharArray();
        sourceBuffer.Add(source);
        var sourceBufferSpan = sourceBuffer.WrittenAsSpan();
        var charBuffer       = (source.Length + 256).SourceRecyclingCharArray();
        var padBufferSpan    = charBuffer.RemainingAsSpan();
        var paBufferSize     = padBufferSpan.PadAndAlign(sourceBufferSpan, layout);
        sb.Append(padBufferSpan[..paBufferSize]);
        charBuffer.DecrementRefCount();
        sourceBuffer.DecrementRefCount();
        charsAdded += paBufferSize;
        if (afterClosingBracket < formatSpan.Length)
        {
            sb.Append(formatSpan[afterClosingBracket..]);
            charsAdded += formatSpan.Length - afterClosingBracket;
        }
        return charsAdded;
    }

    public int Format(ICharSequence source, int sourceFrom, IStringBuilder sb, string formatString, int maxTransferCount = int.MaxValue) 
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if(formatString.IsNullOrEmpty() || formatString == "{0}") return Transfer(source, sb);
        var formatSpan         = formatString.AsSpan();
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf('{');
        if (indexOfOpenBracket > 0)
        {
            sb.Append(formatString[..indexOfOpenBracket]);
            formatSpan =  formatSpan[indexOfOpenBracket..];
            charsAdded = indexOfOpenBracket;
        }
        formatSpan.ExtractStringFormatStages(out _, out var layout, out var _);
        var afterClosingBracket = formatSpan.IndexOf( '}') + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, sb);
            if (afterClosingBracket < formatSpan.Length)
            {
                sb.Append(formatSpan[afterClosingBracket..]);
                charsAdded += formatSpan.Length - afterClosingBracket;
            }
            return charsAdded;
        }

        if (source.Length < 4096)
        {
            var sourceInSpan = stackalloc char[source.Length].ResetMemory();
            sourceInSpan.Append(source);
            var padSpan = stackalloc char[source.Length + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(sourceInSpan, layout);
            sb.Append(padSpan[..padSize]);
            charsAdded += padSize;
            if (afterClosingBracket < formatSpan.Length)
            {
                sb.Append(formatSpan[afterClosingBracket..]);
                charsAdded += formatSpan.Length - afterClosingBracket;
            }
            return charsAdded;
        }
        var sourceBuffer    = (source.Length).SourceRecyclingCharArray();
        sourceBuffer.Add(source);
        var sourceBufferSpan = sourceBuffer.WrittenAsSpan();
        var charBuffer       = (source.Length + 256).SourceRecyclingCharArray();
        var padBufferSpan    = charBuffer.RemainingAsSpan();
        var paBufferSize     = padBufferSpan.PadAndAlign(sourceBufferSpan, layout);
        sb.Append(padBufferSpan[..paBufferSize]);
        charBuffer.DecrementRefCount();
        sourceBuffer.DecrementRefCount();
        charsAdded += paBufferSize;
        if (afterClosingBracket < formatSpan.Length)
        {
            sb.Append(formatSpan[afterClosingBracket..]);
            charsAdded = formatSpan.Length - afterClosingBracket;
        }
        return charsAdded;
    }

    public int Format(ReadOnlySpan<char> source, int sourceFrom, Span<char> destCharSpan, string formatString, int destStartIndex = 0
      , int maxTransferCount = Int32.MaxValue) 
    {
        if (source.Length == 0) return 0;
        if(formatString.IsNullOrEmpty() || formatString == "{0}") return Transfer(source, destCharSpan, destStartIndex);
        
        var formatSpan         = formatString.AsSpan();
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf('{');
        if (indexOfOpenBracket > 0)
        {
            charsAdded += destCharSpan.OverWriteAt(destStartIndex, formatSpan[..indexOfOpenBracket]);
            formatSpan =  formatSpan[indexOfOpenBracket..];
        }
        formatSpan.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatSpan.IndexOf( '}') + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, destCharSpan, destStartIndex + charsAdded);
            if (afterClosingBracket < formatSpan.Length)
            {
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
            }
            return charsAdded;
        }
        if (layout.Length == 0)  return Transfer(source, destCharSpan, destStartIndex + charsAdded);

        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        sourceFrom = Math.Clamp(sourceFrom, 0, source.Length);
        var end = sourceFrom+cappedLength;
        if (source.Length < 4096)
        {
            var padSpan = stackalloc char[source.Length + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(source[sourceFrom..end], layout);
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, padSpan[..padSize]);
            if (afterClosingBracket < formatSpan.Length)
            {
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
            }
            return charsAdded;
        }
        var charBuffer    = (source.Length + 256).SourceRecyclingCharArray();
        var padBufferSpan = charBuffer.RemainingAsSpan();
        var paBufferSize  = padBufferSpan.PadAndAlign(source[sourceFrom..end], layout);
        charsAdded      += destCharSpan.OverWriteAt(destStartIndex + charsAdded, padBufferSpan[..paBufferSize]);
        if (afterClosingBracket < formatSpan.Length)
        {
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
        }
        charBuffer.DecrementRefCount();
        return charsAdded;
    }

    public int Format(char[] source, int sourceFrom, Span<char> destCharSpan, string formatString, int destStartIndex = 0
      , int maxTransferCount = Int32.MaxValue) 
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if(formatString.IsNullOrEmpty() || formatString == "{0}") return Transfer(source, destCharSpan, destStartIndex);
        
        var formatSpan         = formatString.AsSpan();
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf('{');
        if (indexOfOpenBracket > 0)
        {
            charsAdded += destCharSpan.OverWriteAt(destStartIndex, formatSpan[..indexOfOpenBracket]);
            formatSpan =  formatSpan[indexOfOpenBracket..];
        }
        formatSpan.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatSpan.IndexOf( '}') + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, destCharSpan, destStartIndex + charsAdded);
            if (afterClosingBracket < formatSpan.Length)
            {
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
            }
            return charsAdded;
        }

        if (source.Length < 4096)
        {
            var padSpan = stackalloc char[source.Length + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(source, layout);
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, padSpan[..padSize]);
            if (afterClosingBracket < formatSpan.Length)
            {
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
            }
            return charsAdded;
        }
        var charBuffer    = (source.Length + 256).SourceRecyclingCharArray();
        var padBufferSpan = charBuffer.RemainingAsSpan();
        var paBufferSize  = padBufferSpan.PadAndAlign(source, layout);
        charsAdded      += destCharSpan.OverWriteAt(destStartIndex + charsAdded, padBufferSpan[..paBufferSize]);
        if (afterClosingBracket < formatSpan.Length)
        {
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
        }
        charBuffer.DecrementRefCount();
        return charsAdded;
    }

    public int Format(StringBuilder source, int sourceFrom, Span<char> destCharSpan, string formatString, int destStartIndex = 0
      , int maxTransferCount = Int32.MaxValue) 
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if(formatString.IsNullOrEmpty() || formatString == "{0}") return Transfer(source, destCharSpan, destStartIndex);
        
        var formatSpan         = formatString.AsSpan();
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf('{');
        if (indexOfOpenBracket > 0)
        {
            charsAdded += destCharSpan.OverWriteAt(destStartIndex, formatSpan[..indexOfOpenBracket]);
            formatSpan =  formatSpan[indexOfOpenBracket..];
        }
        formatSpan.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatSpan.IndexOf( '}') + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, destCharSpan, destStartIndex + charsAdded);
            if (afterClosingBracket < formatSpan.Length)
            {
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
            }
            return charsAdded;
        }

        if (source.Length < 4096)
        {
            var sourceInSpan = stackalloc char[source.Length].ResetMemory();
            sourceInSpan.Append(source);
            var padSpan = stackalloc char[source.Length + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(sourceInSpan, layout);
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, padSpan[..padSize]);
            if (afterClosingBracket < formatSpan.Length)
            {
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
            }
            return charsAdded;
        }
        var sourceBuffer    = (source.Length).SourceRecyclingCharArray();
        sourceBuffer.Add(source);
        var sourceBufferSpan = sourceBuffer.WrittenAsSpan();
        var charBuffer       = (source.Length + 256).SourceRecyclingCharArray();
        var padBufferSpan    = charBuffer.RemainingAsSpan();
        var paBufferSize     = padBufferSpan.PadAndAlign(sourceBufferSpan, layout);
        charsAdded      += destCharSpan.OverWriteAt(destStartIndex + charsAdded, padBufferSpan[..paBufferSize]);
        if (afterClosingBracket < formatSpan.Length)
        {
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
        }
        charBuffer.DecrementRefCount();
        sourceBuffer.DecrementRefCount();
        return charsAdded;
    }

    public int Format(ICharSequence source, int sourceFrom, Span<char> destCharSpan, string formatString, int destStartIndex = 0
      , int maxTransferCount = Int32.MaxValue) 
    {
        var cappedLength = Math.Min(source.Length - sourceFrom, maxTransferCount);
        if (cappedLength == 0) return 0;
        if(formatString.IsNullOrEmpty() || formatString == "{0}") return Transfer(source, destCharSpan, destStartIndex);
        
        var formatSpan         = formatString.AsSpan();
        var charsAdded         = 0;
        var indexOfOpenBracket = formatString.IndexOf('{');
        if (indexOfOpenBracket > 0)
        {
            charsAdded += destCharSpan.OverWriteAt(destStartIndex, formatSpan[..indexOfOpenBracket]);
            formatSpan =  formatSpan[indexOfOpenBracket..];
        }
        formatSpan.ExtractStringFormatStages(out _, out var layout, out _);
        var afterClosingBracket = formatSpan.IndexOf( '}') + 1;
        if (layout.Length == 0)
        {
            charsAdded += Transfer(source, destCharSpan, destStartIndex + charsAdded);
            if (afterClosingBracket < formatSpan.Length)
            {
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
            }
            return charsAdded;
        }

        if (source.Length < 4096)
        {
            var sourceInSpan = stackalloc char[source.Length].ResetMemory();
            sourceInSpan.Append(source);
            var padSpan = stackalloc char[source.Length + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(sourceInSpan, layout);
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, padSpan[..padSize]);
            if (afterClosingBracket < formatSpan.Length)
            {
                charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
            }
            return charsAdded;
        }
        var sourceBuffer    = (source.Length).SourceRecyclingCharArray();
        sourceBuffer.Add(source);
        var sourceBufferSpan = sourceBuffer.WrittenAsSpan();
        var charBuffer       = (source.Length + 256).SourceRecyclingCharArray();
        var padBufferSpan    = charBuffer.RemainingAsSpan();
        var paBufferSize     = padBufferSpan.PadAndAlign(sourceBufferSpan, layout);
        charsAdded      += destCharSpan.OverWriteAt(destStartIndex + charsAdded, padBufferSpan[..paBufferSize]);
        if (afterClosingBracket < formatSpan.Length)
        {
            charsAdded += destCharSpan.OverWriteAt(destStartIndex + charsAdded, formatSpan[afterClosingBracket..]);
        }
        charBuffer.DecrementRefCount();
        sourceBuffer.DecrementRefCount();
        return charsAdded;
    }

    public int Format<TFmt>(TFmt? source, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable
    {
        if (source == null)
        {
            return IgnoreNullableValues ? 0 : sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        int charsWritten;
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            var charSpan = stackalloc char[256].ResetMemory();
            charsWritten = formatter(source, charSpan, formatString, null);
            sb.Append(charSpan[..charsWritten]);
            return charsWritten;
        }
        if (source is Enum)
        {
            var enumFormatProvider = EnumFormatterRegistry.GetOrCreateStructEnumFormatProvider<TFmt>();
            TryAddCustomSpanFormattableProvider(typeof(TFmt), enumFormatProvider);
            formatter = enumFormatProvider.CustomSpanFormattable;
            var charSpan = stackalloc char[1024].ResetMemory();
            charsWritten = formatter(source, charSpan, formatString, null);
            sb.Append(charSpan[..charsWritten]);
            return charsWritten;
        }
        try
        {
            var charSpan = stackalloc char[1024].ResetMemory();
            formatString.AsSpan().ExtractStringFormatStages(out var _, out var layout, out var formatting);
            if (source.TryFormat(charSpan, out charsWritten, format: formatting, provider: null))
            {
                if (layout.Length == 0)
                {
                    sb.Append(charSpan[..charsWritten]);
                    return charsWritten;
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(charSpan[..charsWritten], layout);
                sb.Append(padSpan[..padSize]);
                return padSize;
            }
        }
        catch (FormatException)
        {
            var sbPreAppendLen = sb.Length;
            sb.AppendFormat(formatString ?? "{0}", source.ToString()!);
            charsWritten = sb.Length - sbPreAppendLen;
        }
        return charsWritten;
    }

    public int Format<TFmt>(TFmt? source, Span<char> destination, int destStartIndex, string? formatString = null) where TFmt : ISpanFormattable
    {
        if (source == null)
        {
            return IgnoreNullableValues ? 0 : destination.AppendReturnAddCount(NullString);
        }
        int charsWritten;
        var charSpan = stackalloc char[1024].ResetMemory();
        if (TryGetCachedCustomSpanFormatter<TFmt>(out var formatter))
        {
            charsWritten = formatter(source, charSpan, formatString, null);
            destination.OverWriteAt(destStartIndex, charSpan[..charsWritten]);
            return charsWritten;
        }
        if (source is Enum)
        {
            var enumFormatProvider = EnumFormatterRegistry.GetOrCreateStructEnumFormatProvider<TFmt>();
            TryAddCustomSpanFormattableProvider(typeof(TFmt), enumFormatProvider);
            formatter    = enumFormatProvider.CustomSpanFormattable;
            charsWritten = formatter(source, charSpan, formatString, null);
            destination.OverWriteAt(destStartIndex, charSpan[..charsWritten]);
            return charsWritten;
        }
        formatString.AsSpan().ExtractStringFormatStages(out var _, out var layout, out var formatting);
        try
        {
            if (source.TryFormat(charSpan, out charsWritten, format: formatting, provider: null))
            {
                if (layout.Length == 0)
                {
                    destination.OverWriteAt(destStartIndex, charSpan[..charsWritten]);
                    return charsWritten;
                }
                var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
                var padSize = padSpan.PadAndAlign(charSpan[..charsWritten], layout);
                destination.OverWriteAt(destStartIndex, padSpan[..padSize]);
                return padSize;
            }
        }
        catch (FormatException) { }
        if (source.TryFormat(charSpan, out charsWritten, format: "", provider: null))
        {
            if (layout.Length == 0)
            {
                destination.OverWriteAt(destStartIndex, charSpan[..charsWritten]);
                return charsWritten;
            }
            var padSpan = stackalloc char[charsWritten + 256].ResetMemory();
            var padSize = padSpan.PadAndAlign(charSpan[..charsWritten], layout);
            destination.OverWriteAt(destStartIndex, padSpan[..padSize]);
            return padSize;
        }
        return charsWritten;
    }

    public int Format<TFmt>(TFmt? source, IStringBuilder sb, string? formatString = null) where TFmt : struct, ISpanFormattable
    {
        if (source == null)
        {
            return IgnoreNullableValues ? 0 : sb.Append(NullString).ReturnCharCount(NullString.Length);
        }
        return Format(source.Value, sb, formatString);
    }

    public int Format<TFmt>(TFmt? source, Span<char> destination, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable
    {
        if (source == null)
        {
            return IgnoreNullableValues ? 0 : destination.OverWriteAt(destStartIndex, NullString);
        }
        return Format(source.Value, destination, destStartIndex, formatString);
    }

    public int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable
    {
        if (arg0.Length == 0) return 0;
        var preAppendLen = sb.Length;
        if (formatString.IsNullOrEmpty() || formatString == "{0}")
        {
            switch (arg0[0])
            {
                case char:
                    for (var i = 0; i < arg0.Length; i++)
                    {
                        sb.Append(arg0[i]);
                    }
                    break;
                default:
                    FormatCollectionStart<TFmt>(sb, true);
                    for (var i = 0; i < arg0.Length; i++)
                    {
                        if (i > 0) AddItemSeparator(sb);
                        FormatCollectionNextItem(arg0[i], i, sb);
                    }
                    FormatCollectionEnd<TFmt>(sb, arg0.Length);
                    break;
            }
        }
        else
        {
            FormatCollectionStart<TFmt>(sb, true);
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddItemSeparator(sb);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
            FormatCollectionEnd<TFmt>(sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public int FormatCollection<TFmt>(ReadOnlySpan<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable
    {
        if (arg0.Length == 0) return 0;
        var addedChars = 0;
        if (formatString.IsNullOrEmpty() || formatString == "{0}")
        {
            switch (arg0[0])
            {
                case char:
                    addedChars += destCharSpan.OverWriteAt(destStartIndex, NullString);
                    break;
                default:
                    addedChars += FormatCollectionStart<TFmt>(destCharSpan, destStartIndex, true);
                    for (var i = 0; i < arg0.Length; i++)
                    {
                        if (i > 0) addedChars += AddItemSeparator(destCharSpan, destStartIndex+addedChars);
                        addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
                    }
                    addedChars += FormatCollectionEnd<TFmt>(destCharSpan, destStartIndex + addedChars, arg0.Length);
                    break;
            }
        }
        else
        {
            addedChars += FormatCollectionStart<TFmt>(destCharSpan, destStartIndex, true);
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddItemSeparator(destCharSpan, destStartIndex+addedChars);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
            addedChars += FormatCollectionEnd<TFmt>(destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
        where TFmt : struct, ISpanFormattable 
    {
        if (arg0.Length == 0) return 0;
        var preAppendLen = sb.Length;
        if (formatString.IsNullOrEmpty() || formatString == "{0}")
        {
            switch (arg0[0])
            {
                case char:
                    for (var i = 0; i < arg0.Length; i++)
                    {
                        sb.Append(arg0[i]);
                    }
                    break;
                default:
                    FormatCollectionStart<TFmt>(sb, true);
                    for (var i = 0; i < arg0.Length; i++)
                    {
                        if (i > 0) AddItemSeparator(sb);
                        FormatCollectionNextItem(arg0[i], i, sb);
                    }
                    FormatCollectionEnd<TFmt>(sb, arg0.Length);
                    break;
            }
        }
        else
        {
            FormatCollectionStart<TFmt>(sb, true);
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) AddItemSeparator(sb);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
            FormatCollectionEnd<TFmt>(sb, arg0.Length);
        }
        return sb.Length - preAppendLen;
    }

    public int FormatCollection<TFmt>(ReadOnlySpan<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable 
    {
        if (arg0.Length == 0) return 0;
        var addedChars = 0;
        if (formatString.IsNullOrEmpty() || formatString == "{0}")
        {
            switch (arg0[0])
            {
                case char:
                    addedChars += destCharSpan.OverWriteAt(destStartIndex, NullString);
                    break;
                default:
                    addedChars += FormatCollectionStart<TFmt>(destCharSpan, destStartIndex, true);
                    for (var i = 0; i < arg0.Length; i++)
                    {
                        if (i > 0) addedChars += AddItemSeparator(destCharSpan, destStartIndex+addedChars);
                        addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
                    }
                    addedChars += FormatCollectionEnd<TFmt>(destCharSpan, destStartIndex + addedChars, arg0.Length);
                    break;
            }
        }
        else
        {
            addedChars += FormatCollectionStart<TFmt>(destCharSpan, destStartIndex, true);
            for (var i = 0; i < arg0.Length; i++)
            {
                if (i > 0) addedChars += AddItemSeparator(destCharSpan, destStartIndex+addedChars);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
            addedChars += FormatCollectionEnd<TFmt>(destCharSpan, destStartIndex + addedChars, arg0.Length);
        }
        return addedChars;
    }

    public int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, IStringBuilder sb, string? formatString = null) where TFmt : ISpanFormattable 
    {
        if (arg0.Count == 0) return 0;
        var preAppendLen = sb.Length;
        if (formatString.IsNullOrEmpty() || formatString == "{0}")
        {
            switch (arg0[0])
            {
                case char:
                    for (var i = 0; i < arg0.Count; i++)
                    {
                        sb.Append(arg0[i]);
                    }
                    break;
                default:
                    FormatCollectionStart<TFmt>(sb, true);
                    for (var i = 0; i < arg0.Count; i++)
                    {
                        if (i > 0) AddItemSeparator(sb);
                        FormatCollectionNextItem(arg0[i], i, sb);
                    }
                    FormatCollectionEnd<TFmt>(sb, arg0.Count);
                    break;
            }
        }
        else
        {
            FormatCollectionStart<TFmt>(sb, true);
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddItemSeparator(sb);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
            FormatCollectionEnd<TFmt>(sb, arg0.Count);
        }
        return sb.Length - preAppendLen;
    }

    public int FormatCollection<TFmt>(IReadOnlyList<TFmt> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : ISpanFormattable 
    {
        if (arg0.Count == 0) return 0;
        var addedChars = 0;
        if (formatString.IsNullOrEmpty() || formatString == "{0}")
        {
            switch (arg0[0])
            {
                case char:
                    addedChars += destCharSpan.OverWriteAt(destStartIndex, NullString);
                    break;
                default:
                    addedChars += FormatCollectionStart<TFmt>(destCharSpan, destStartIndex, true);
                    for (var i = 0; i < arg0.Count; i++)
                    {
                        if (i > 0) addedChars += AddItemSeparator(destCharSpan, destStartIndex+addedChars);
                        addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
                    }
                    addedChars += FormatCollectionEnd<TFmt>(destCharSpan, destStartIndex + addedChars, arg0.Count);
                    break;
            }
        }
        else
        {
            addedChars += FormatCollectionStart<TFmt>(destCharSpan, destStartIndex, true);
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddItemSeparator(destCharSpan, destStartIndex+addedChars);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
            addedChars += FormatCollectionEnd<TFmt>(destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, IStringBuilder sb, string? formatString = null)
        where TFmt : struct, ISpanFormattable 
    {
        if (arg0.Count == 0) return 0;
        var preAppendLen = sb.Length;
        if (formatString.IsNullOrEmpty() || formatString == "{0}")
        {
            switch (arg0[0])
            {
                case char:
                    for (var i = 0; i < arg0.Count; i++)
                    {
                        sb.Append(arg0[i]);
                    }
                    break;
                default:
                    FormatCollectionStart<TFmt>(sb, true);
                    for (var i = 0; i < arg0.Count; i++)
                    {
                        if (i > 0) AddItemSeparator(sb);
                        FormatCollectionNextItem(arg0[i], i, sb);
                    }
                    FormatCollectionEnd<TFmt>(sb, arg0.Count);
                    break;
            }
        }
        else
        {
            FormatCollectionStart<TFmt>(sb, true);
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) AddItemSeparator(sb);
                FormatCollectionNextItem(arg0[i], i, sb, formatString);
            }
            FormatCollectionEnd<TFmt>(sb, arg0.Count);
        }
        return sb.Length - preAppendLen;
    }

    public int FormatCollection<TFmt>(IReadOnlyList<TFmt?> arg0, Span<char> destCharSpan, int destStartIndex, string? formatString = null)
        where TFmt : struct, ISpanFormattable 
    {
        if (arg0.Count == 0) return 0;
        var addedChars = 0;
        if (formatString.IsNullOrEmpty() || formatString == "{0}")
        {
            switch (arg0[0])
            {
                case char:
                    addedChars += destCharSpan.OverWriteAt(destStartIndex, NullString);
                    break;
                default:
                    addedChars += FormatCollectionStart<TFmt>(destCharSpan, destStartIndex, true);
                    for (var i = 0; i < arg0.Count; i++)
                    {
                        if (i > 0) addedChars += AddItemSeparator(destCharSpan, destStartIndex+addedChars);
                        addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars);
                    }
                    addedChars += FormatCollectionEnd<TFmt>(destCharSpan, destStartIndex + addedChars, arg0.Count);
                    break;
            }
        }
        else
        {
            addedChars += FormatCollectionStart<TFmt>(destCharSpan, destStartIndex, true);
            for (var i = 0; i < arg0.Count; i++)
            {
                if (i > 0) addedChars += AddItemSeparator(destCharSpan, destStartIndex+addedChars);
                addedChars += FormatCollectionNextItem(arg0[i], i, destCharSpan, destStartIndex + addedChars, formatString);
            }
            addedChars += FormatCollectionEnd<TFmt>(destCharSpan, destStartIndex + addedChars, arg0.Count);
        }
        return addedChars;
    }

    public int FormatCollectionStart<TFmt>(IStringBuilder sb, bool hasItems) where TFmt : ISpanFormattable => sb.Append("[").ReturnCharCount(1);

    public int FormatCollectionStart<TFmt>(Span<char> destination, int destStartIndex, bool hasItems) where TFmt : ISpanFormattable =>
        destination.OverWriteAt(destStartIndex, "[");

    public int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb, string formatString)
        where TFmt : ISpanFormattable =>
        Format(nextItem, sb, formatString);

    public int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex, string formatString)
        where TFmt : ISpanFormattable =>
        Format(nextItem, destCharSpan, destStartIndex, formatString);

    public int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb, string formatString)
        where TFmt : struct, ISpanFormattable =>
        Format(nextItem, sb, formatString);

    public int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destCharSpan, int destStartIndex, string formatString)
        where TFmt : struct, ISpanFormattable =>
        Format(nextItem, destCharSpan, destStartIndex, formatString);

    public int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, IStringBuilder sb)
        where TFmt : ISpanFormattable
    {
        var preAppendLen = sb.Length;
        sb.Append(nextItem);
        return sb.Length - preAppendLen;
    }

    public int FormatCollectionNextItem<TFmt>(TFmt nextItem, int retrieveCount, Span<char> destination, int destStartIndex)
        where TFmt : ISpanFormattable
    {
        destination = destStartIndex > 0 ? destination[destStartIndex..] : destination;
        if (nextItem.TryFormat(destination, out var charsWritten, "", null))
        {
            return charsWritten;
        }
        return 0;
    }

    public int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, IStringBuilder sb) where TFmt : struct, ISpanFormattable
    {
        var preAppendLen = sb.Length;
        sb.Append(nextItem);
        return sb.Length - preAppendLen;
    }

    public int FormatCollectionNextItem<TFmt>(TFmt? nextItem, int retrieveCount, Span<char> destination, int destStartIndex)
        where TFmt : struct, ISpanFormattable
    {
        if (nextItem == null)
        {
            return IgnoreNullableValues ? 0 : destination.OverWriteAt(destStartIndex, NullString);
        }
        return FormatCollectionNextItem(nextItem.Value, retrieveCount, destination, destStartIndex);
    }

    public int FormatCollectionEnd<TFmt>(IStringBuilder sb, int itemsCount) where TFmt : ISpanFormattable => sb.Append("]").ReturnCharCount(1);

    public int FormatCollectionEnd<TFmt>(Span<char> destination, int index, int itemsCount) where TFmt : ISpanFormattable =>
        destination.OverWriteAt(index, "]");
}

public static class CustomStringFormatterExtensions
{
    public static int ReturnCharCount(this IStringBuilder sb, int charsAdded) => charsAdded;

    public static int ReturnCharCount(this Span<char> charSpan, int charsAdded) => charsAdded;
}
