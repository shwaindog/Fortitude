using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge.Crucible;
using static FortitudeCommon.Types.StringsOfPower.Forge.FormattingHandlingFlags;

namespace FortitudeCommon.Types.StringsOfPower.Forge;

public interface IMutableStringBuilder<out T> where T : IStringBuilder, IMutableStringBuilder<T>
{
    int Capacity    { get; set; }
    int MaxCapacity { get; }
    int Length      { get; set; }

    char this[int index] { get; set; }

    T Append(ICharSequence? value, ICustomStringFormatter? customStringFormatter = null);
    T Append(StringBuilder? value, ICustomStringFormatter? customStringFormatter = null);
    
    T Append(ICharSequence? value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(ICharSequence? value, int startIndex, int length , ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter = null, FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(StringBuilder? value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(StringBuilder? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter = null, FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(bool value, ICustomStringFormatter? customStringFormatter = null);
    T Append(byte value);
    T Append(char value);
    T Append(char value, int repeatCount);
    T Append(char[]? value);
    T Append(decimal value);
    T Append(double value);
    T Append(short value);
    T Append(int value);
    T Append(long value);
    T Append(object? value);
    T Append(sbyte value);
    T Append(float value);
    T Append(string? value);
    
    T Append(string? value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(string? value, int startIndex, int length, ReadOnlySpan<char> formatString
      , ICustomStringFormatter? customStringFormatter = null, FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append<TFmt>(TFmt arg0, ICustomStringFormatter? customStringFormatter = null) where TFmt : ISpanFormattable;
    
    T Append(Span<char> value, ICustomStringFormatter? customStringFormatter = null);
    
    T Append(Span<char> value, int startIndex, int length = int.MaxValue, string? formatString = null
      , ICustomStringFormatter? customStringFormatter = null, FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(ReadOnlySpan<char> value, ICustomStringFormatter? customStringFormatter = null);
    
    T Append(ReadOnlySpan<char> value, int startIndex, int length = int.MaxValue, string? formatString = null
    , ICustomStringFormatter? customStringFormatter = null, FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(ReadOnlyMemory<char> value, ICustomStringFormatter? customStringFormatter = null);
    
    T Append(ReadOnlyMemory<char> value, int startIndex, int length = int.MaxValue, string? formatString = null
    , ICustomStringFormatter? customStringFormatter = null , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(char[]? value, int startIndex, int length, string? formatString = null, ICustomStringFormatter? customStringFormatter = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(char[]? value, int startIndex, int length, ReadOnlySpan<char> formatString, ICustomStringFormatter? customStringFormatter = null
      , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T Append(ushort value);
    T Append(uint value);
    T Append(ulong value);

    unsafe T Append(char* value, int valueCount);

    T AppendFormat<TFmt>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, TFmt arg0) where TFmt : ISpanFormattable;
    T AppendFormat<TFmt>([StringSyntax(StringSyntaxAttribute.CompositeFormat)] ReadOnlySpan<char> format, TFmt arg0) where TFmt : ISpanFormattable;
    
    T AppendFormat<TFmt>(ICustomStringFormatter customStringFormatter
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, TFmt arg0
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;
    
    T AppendFormat<TFmt>(ICustomStringFormatter customStringFormatter
    , [StringSyntax(StringSyntaxAttribute.CompositeFormat)] ReadOnlySpan<char> format, TFmt arg0
    , FormattingHandlingFlags formatFlags = EncodeInnerContent) where TFmt : ISpanFormattable;
    
    T AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, string arg0);
    T AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] ReadOnlySpan<char> format, ReadOnlySpan<char> arg0);
    T AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, ReadOnlySpan<char> arg0);
    
    T AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0);
    
    T AppendFormat(ICustomStringFormatter customStringFormatter,[StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0
    , FormattingHandlingFlags formatFlags = EncodeInnerContent);
    
    T AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1);
    T AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2);
    T AppendFormat([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args);
    T AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0);
    T AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1);

    T AppendFormat
        (IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, object? arg0, object? arg1, object? arg2);

    T AppendFormat(IFormatProvider? provider, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string format, params object?[] args);

    T AppendFormat<TParam>(IFormatProvider? provider, CompositeFormat format, TParam arg0);
    T AppendFormat<TParam, TParam1>(IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1);
    T AppendFormat<TParam, TParam1, TParam2>(IFormatProvider? provider, CompositeFormat format, TParam arg0, TParam1 arg1, TParam2 arg2);
    T AppendFormat(IFormatProvider? provider, CompositeFormat format, params object?[] args);
    T AppendFormat(IFormatProvider? provider, CompositeFormat format, ReadOnlySpan<object?> args);

    T   AppendJoin(string separator, params object?[] values);
    T   AppendJoin<TParam>(string separator, IEnumerable<TParam> values);
    T   AppendJoin(string separator, params string?[] values);
    T   AppendJoin<TParam>(char separator, IEnumerable<TParam> values);
    T   AppendJoin(char separator, params string?[] values);
    T   AppendLine();
    T   AppendLine(string? value);
    T   AppendLine(ICharSequence value);
    T   AppendLine(StringBuilder value);
    T   Clear();
    T   CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count);
    T   CopyTo(int sourceIndex, Span<char> destination, int count);
    int EnsureCapacity(int capacity);
    T   Insert(int atIndex, bool value);
    T   Insert(int atIndex, byte value);
    T   Insert(int atIndex, char value);
    T   Insert(int atIndex, char[]? value);
    T   Insert(int atIndex, decimal value);
    T   Insert(int atIndex, double value);
    T   Insert(int atIndex, short value);
    T   Insert(int atIndex, int value);
    T   Insert(int atIndex, long value);
    T   Insert(int atIndex, object? value);
    T   Insert(int atIndex, sbyte value);
    T   Insert(int atIndex, float value);
    T   Insert(int atIndex, string? value);
    T   Insert(int atIndex, Span<char> value);
    T   Insert(int atIndex, Span<char> value, int startIndex, int length);
    T   Insert(int atIndex, ReadOnlySpan<char> value);
    T   Insert(int atIndex, ReadOnlySpan<char> value, int startIndex, int length);
    T   Insert(int atIndex, char[]? value, int startIndex, int length);
    T   Insert(int atIndex, ushort value);
    T   Insert(int atIndex, uint value);
    T   Insert(int atIndex, ulong value);
    T   Insert(int atIndex, ICharSequence? value);
    T   Insert(int atIndex, StringBuilder? value);

    T Insert(int index, string? value, int count);

    T Remove(int startIndex, int length);

    T Replace(char find, char replace);
    T Replace(char find, char replace, int startIndex, int length);
    T Replace(string find, string? replace);
    T Replace(string find, string replace, int startIndex, int length);
    T Replace(ICharSequence find, ICharSequence replace);
    T Replace(StringBuilder find, StringBuilder replace);
    T Replace(ICharSequence find, ICharSequence replace, int startIndex, int length);
    T Replace(StringBuilder find, StringBuilder replace, int startIndex, int length);

    string ToString(int startIndex, int length);

    bool Equals([NotNullWhen(true)] StringBuilder? sb);
    bool Equals(ReadOnlySpan<char> span);
}

