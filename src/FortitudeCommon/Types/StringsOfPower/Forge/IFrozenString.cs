// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.DataStructures.MemoryPools.Buffers;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Types.StringsOfPower.Forge;

public interface ICharSequence : IRecyclableObject, IEnumerable<char>
{
    int Length { get; }
    char this[int index] { get; }

    void CopyTo(char[] array, int arrayIndex, int myLength = int.MaxValue, int fromMyIndex = 0);
    void CopyTo(RecyclingCharArray array, int? arrayIndex = null, int myLength = int.MaxValue, int fromMyIndex = 0);
    void CopyTo(Span<char> charSpan, int spanIndex, int myLength = int.MaxValue, int fromMyIndex = 0);

    bool EquivalentTo(string other);
    int  CompareTo(string other);
    int  CompareTo(ICharSequence other);
    bool Contains(string subStr);
    bool Contains(ICharSequence subStr);
    int  IndexOf(string subStr);
    int  IndexOf(ICharSequence subStr);
    int  IndexOf(string subStr, int fromThisPos);
    int  IndexOf(ICharSequence subStr, int fromThisPos);
    int  LastIndexOf(string subStr);
    int  LastIndexOf(ICharSequence subStr);
    int  LastIndexOf(string subStr, int fromThisPos);
    int  LastIndexOf(ICharSequence subStr, int fromThisPos);

    string ToString();

    bool Equals(string? toCompare);
}

public interface IFrozenString : ICharSequence, ICanSourceThawed<IMutableString>
{
}


public static class FrozenStringExtensions
{
    public static bool IsNullOrEmpty(this IFrozenString? test)
    {
        if (test == null) return true;
        for (var i = 0; i < test.Length; i++)
            if (Array.IndexOf(MutableString.WhiteSpaceChars, test[i]) < 0)
                return false;
        return true;
    }

    public static bool IsNotNullOrEmpty(this IFrozenString? test) => !test.IsNullOrEmpty();

    public static IMutableString Replace(this IFrozenString source, string find, string replace, IMutableString destMutableString)
    {
        destMutableString.Clear();
        destMutableString.CopyFrom(source);
        destMutableString.Replace(find, replace);
        return destMutableString;
    }

    public static IMutableString Replace(this IFrozenString source, StringBuilder find, StringBuilder replace, IMutableString destMutableString)
    {
        destMutableString.Clear();
        destMutableString.CopyFrom(source);
        destMutableString.Replace(find, replace);
        return destMutableString;
    }

    public static IMutableString Replace(this IFrozenString source, MutableString find, MutableString replace, IMutableString destMutableString)
    {
        destMutableString.Clear();
        destMutableString.CopyFrom(source);
        destMutableString.Replace(find, replace);
        return destMutableString;
    }

    public static IMutableString ToUpper(this IFrozenString source, IMutableString destMutableString)
    {
        destMutableString.Clear();
        for (var i = 0; i < source.Length; i++)
        {
            var oldChar   = source[i];
            var upperChar = char.ToUpperInvariant(oldChar);
            destMutableString[i] = upperChar;
        }
        return destMutableString;
    }

    public static IMutableString ToLower(this IFrozenString source, IMutableString destMutableString)
    {
        destMutableString.Clear();
        for (var i = 0; i < source.Length; i++)
        {
            var oldChar   = source[i];
            var upperChar = char.ToLowerInvariant(oldChar);
            destMutableString[i] = upperChar;
        }
        return destMutableString;
    }

    public static IMutableString Trim(this IFrozenString source, IMutableString destMutableString)
    {
        destMutableString.Clear();
        destMutableString.CopyFrom(source);
        destMutableString.Trim();
        return destMutableString;
    }

    public static IMutableString Substring(this IFrozenString source, int startIndex, IMutableString destMutableString)
    {
        destMutableString.Clear();
        for (var i = startIndex; i < source.Length; i++) destMutableString.Append(source[i]);
        return destMutableString;
    }

    public static IMutableString Substring(this IFrozenString source, int startIndex, int length, IMutableString destMutableString)
    {
        destMutableString.Clear();
        for (var i = startIndex; i < source.Length; i++) destMutableString.Append(source[i]);
        return destMutableString;
    }

    public static string[] Split(this IFrozenString source, char[] delimiters)
    {
        var countDelimitersFound = 0;
        for (var i = 0; i < source.Length; i++)
        {
            var checkChar = source[i];
            if (Array.IndexOf(delimiters, checkChar) > 0) countDelimitersFound++;
        }

        var result = new string[countDelimitersFound + 1];

        var nextStringInsertIndex = 0;

        var nextString = new char[source.Length];

        var nextStringCharIndex = 0;
        for (var i = 0; i < source.Length; i++)
        {
            var checkChar = source[i];
            if (Array.IndexOf(delimiters, checkChar) > 0)
            {
                result[nextStringInsertIndex++] = new string(nextString, 0, nextStringCharIndex);

                nextStringCharIndex = 0;
            }
            else
            {
                nextString[nextStringCharIndex++] = checkChar;
            }
        }

        return result;
    }
    

    public static IList<IMutableString> Split(this IFrozenString source, char[] delimiters, IList<IMutableString> results, Func<IMutableString> mutableStringSupplier)
    {
        var nextString = mutableStringSupplier();
        for (var i = 0; i < source.Length; i++)
        {
            var checkChar = source[i];
            if (Array.IndexOf(delimiters, checkChar) > 0)
            {
                results.Add(nextString);
                nextString = mutableStringSupplier();
            }
            else
            {
                nextString.Append(checkChar);
            }
        }
        return results;
    }



    public static string SplitFirstAsString(this IFrozenString source, char[] delimiters)
    {
        var includedChars = stackalloc char[source.Length].ResetMemory();
        for (var i = 0; i < source.Length; i++)
        {
            var checkChar = source[i];
            if (Array.IndexOf(delimiters, checkChar) >= 0)
            {
                var subStringSlice = includedChars.Slice(0, i);
                return new String(subStringSlice);
            }
            else
            {
                includedChars[i] = checkChar;
            }
        }
        return source.ToString();
    }
}
