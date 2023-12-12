#region

using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Types.Mutable;

[SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
public sealed class MutableString : ReusableObject<IMutableString>, IMutableString, IStoreState<MutableString>
{
    private static readonly IPooledFactory<StringBuilderEnumerator> EnumeratorPool =
        new GarbageAndLockFreePooledFactory<StringBuilderEnumerator>(pool => new StringBuilderEnumerator(pool));

    private static readonly char[] WhiteSpaceChars = { ' ', '\t', '\r', '\n' };
    private readonly StringBuilder sb;
    public MutableString() => sb = new StringBuilder();

    public MutableString(IMutableString initialString)
    {
        sb = new StringBuilder();
        Append(initialString);
    }

    public MutableString(string? initialString)
    {
        sb = new StringBuilder();
        sb.Append(initialString);
    }

    public MutableString(int initialCapacity) => sb = new StringBuilder(initialCapacity);

    public MutableString(StringBuilder initializedBuilder) => sb = initializedBuilder;

    IMutableString IMutableStringBuilder<IMutableString>.Append(IMutableString value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(bool value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(byte value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(char value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(char[] value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(decimal value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(double value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(short value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(int value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(long value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(object value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(sbyte value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(float value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(string value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(string value, int startIndex, int length) =>
        Append(value, startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Append(ushort value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(uint value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.Append(ulong value) => Append(value);

    IMutableString IMutableStringBuilder<IMutableString>.AppendLine() => AppendLine();

    IMutableString IMutableStringBuilder<IMutableString>.AppendLine(string value) => AppendLine(value);

    IMutableString IMutableStringBuilder<IMutableString>.AppendLine(IMutableString value) => AppendLine(value);

    IMutableString IMutableStringBuilder<IMutableString>.Clear() => Clear();

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, bool value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, byte value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, char value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, char[] value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, decimal value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, double value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, short value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, int value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, long value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, object value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, sbyte value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, float value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, string value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, char[] value,
        int startIndex, int length) =>
        Insert(atIndex, value, startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, ushort value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, uint value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, ulong value) => Insert(atIndex, value);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(char find, char replace) => Replace(find, replace);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(char find, char replace,
        int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(string find, string replace) => Replace(find, replace);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(string find, string replace,
        int startIndex, int length) =>
        Replace(find, replace, startIndex, length);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(IMutableString find, IMutableString replace) =>
        Replace(find, replace);

    IMutableString IMutableStringBuilder<IMutableString>.Replace(IMutableString find, IMutableString replace,
        int startIndex, int length) =>
        Replace(find, replace, startIndex, Length);

    public void CopyFrom(IMutableString source)
    {
        Clear();
        Append(source);
    }

    public void CopyFrom(string source)
    {
        Clear();
        Append(source);
    }

    public int Length
    {
        get => sb.Length;
        set => sb.Length = value;
    }

    public char this[int index]
    {
        get => sb[index];
        set => sb[index] = value;
    }

    public bool Contains(string subStr) => IndexOf(subStr) >= 0;

    public bool Contains(IMutableString subStr) => IndexOf(subStr) >= 0;

    public int IndexOf(string subStr) => IndexOf(subStr, 0);

    public int IndexOf(IMutableString subStr) => IndexOf(subStr, 0);

    public int IndexOf(string subStr, int fromThisPos)
    {
        var thisLen = Length;
        var otherLen = subStr.Length;
        if (fromThisPos > thisLen) return -1;
        var startPos = fromThisPos < 0 ? 0 : fromThisPos;
        var firstChar = subStr[0];
        var max = thisLen - otherLen;
        for (var i = startPos; i <= max; i++)
        {
            if (this[i] != firstChar)
                // ReSharper disable once EmptyEmbeddedStatement
                while (++i <= max && this[i] != firstChar)
                    ;

            if (i <= max)
            {
                var j = i + 1;
                var end = i + otherLen;
                // ReSharper disable once EmptyEmbeddedStatement
                for (var k = 1; j < end && this[j] == subStr[k]; j++, k++) ;
                if (j == end) return i;
            }
        }

        return -1;
    }

    public int IndexOf(IMutableString subStr, int fromThisPos)
    {
        var thisLen = Length;
        var otherLen = subStr.Length;
        if (fromThisPos > thisLen) return -1;
        var startPos = fromThisPos < 0 ? 0 : fromThisPos;
        var firstChar = subStr[0];
        var max = thisLen - otherLen;
        for (var i = startPos; i <= max; i++)
        {
            if (this[i] != firstChar)
                // ReSharper disable once EmptyEmbeddedStatement
                while (++i <= max && this[i] != firstChar)
                    ;

            if (i <= max)
            {
                var j = i + 1;
                var end = i + otherLen;
                // ReSharper disable once EmptyEmbeddedStatement
                for (var k = 1; j < end && this[j] == subStr[k]; j++, k++) ;
                if (j == end) return i;
            }
        }

        return -1;
    }

    public int LastIndexOf(string subStr) => LastIndexOf(subStr, subStr.Length);

    public int LastIndexOf(IMutableString subStr) => LastIndexOf(subStr, subStr.Length);

    public int LastIndexOf(string subStr, int fromThisPos)
    {
        var thisLen = Length;
        var otherLen = subStr.Length;
        if (fromThisPos > thisLen) return -1;
        var firstChar = subStr[0];
        var max = thisLen - otherLen;
        var startPos = fromThisPos > max ? max : fromThisPos;
        for (var i = startPos; i >= 0; i--)
        {
            if (this[i] != firstChar)
                // ReSharper disable once EmptyEmbeddedStatement
                while (--i <= max && this[i] != firstChar)
                    ;

            if (i <= max)
            {
                var j = i + 1;
                var end = i + otherLen;
                // ReSharper disable once EmptyEmbeddedStatement
                for (var k = 1; j < end && this[j] == subStr[k]; j++, k++) ;
                if (j == end) return i;
            }
        }

        return -1;
    }

    public int LastIndexOf(IMutableString subStr, int fromThisPos)
    {
        var thisLen = Length;
        var otherLen = subStr.Length;
        if (fromThisPos > thisLen) return -1;
        var firstChar = subStr[0];
        var max = thisLen - otherLen;
        var startPos = fromThisPos > max ? max : fromThisPos;
        for (var i = startPos; i >= 0; i--)
        {
            if (this[i] != firstChar)
                // ReSharper disable once EmptyEmbeddedStatement
                while (--i <= max && this[i] != firstChar)
                    ;

            if (i <= max)
            {
                var j = i + 1;
                var end = i + otherLen;
                // ReSharper disable once EmptyEmbeddedStatement
                for (var k = 1; j < end && this[j] == subStr[k]; j++, k++) ;
                if (j == end) return i;
            }
        }

        return -1;
    }

    IMutableString IMutableString.Remove(int startIndex) => Remove(startIndex);

    IMutableString IMutableString.ToUpper() => ToUpper();

    public void ToUpper(IMutableString destMutableString)
    {
        destMutableString.Clear();
        for (var i = 0; i < sb.Length; i++)
        {
            var oldChar = sb[i];
            var upperChar = char.ToUpperInvariant(oldChar);
            destMutableString[i] = upperChar;
        }
    }

    IMutableString IMutableString.ToLower() => ToLower();

    public void ToLower(IMutableString destMutableString)
    {
        destMutableString.Clear();
        for (var i = 0; i < sb.Length; i++)
        {
            var oldChar = sb[i];
            var upperChar = char.ToLowerInvariant(oldChar);
            destMutableString[i] = upperChar;
        }
    }

    IMutableString IMutableString.Trim() => Trim();

    public void Trim(IMutableString destMutableString)
    {
        destMutableString.CopyFrom(destMutableString);
        destMutableString.Trim();
    }

    IMutableString IMutableString.Substring(int startIndex) => Substring(startIndex);

    public void Substring(int startIndex, IMutableString destMutableString)
    {
        destMutableString.Clear();
        for (var i = startIndex; i < Length; i++) destMutableString.Append(this[i]);
    }

    IMutableString IMutableString.Substring(int startIndex, int length) => Substring(startIndex, length);

    public void Substring(int startIndex, int length, IMutableString destMutableString)
    {
        destMutableString.Clear();
        for (var i = startIndex; i < Length; i++) destMutableString.Append(this[i]);
    }

    public void Replace(string find, string replace, IMutableString destMutableString)
    {
        destMutableString.Clear();
        destMutableString.CopyFrom(this);
        destMutableString.Replace(find, replace);
    }

    public string[] Split(char[] delimiters)
    {
        var countDelimitersFound = 0;
        for (var i = 0; i < Length; i++)
        {
            var checkChar = sb[i];
            if (Array.IndexOf(delimiters, checkChar) > 0) countDelimitersFound++;
        }

        var result = new string[countDelimitersFound + 1];
        var nextStringInsertIndex = 0;
        var nextString = new char[Length];
        var nextStringCharIndex = 0;
        for (var i = 0; i < Length; i++)
        {
            var checkChar = sb[i];
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

    public void Split(char[] delimiters, IList<IMutableString> results, Func<IMutableString> mutableStringSupplier)
    {
        var nextString = mutableStringSupplier();
        for (var i = 0; i < Length; i++)
        {
            var checkChar = sb[i];
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
    }

    public StringBuilder GetBackingStringBuilder() => sb;

    public int CompareTo(string other)
    {
        var thisLen = Length;
        var otherLen = other.Length;
        var minLen = Math.Min(thisLen, otherLen);
        for (var i = 0; i < minLen; i++)
        {
            var cmp = sb[i] - other[i];
            if (cmp != 0) return cmp;
        }

        if (thisLen < otherLen) return -1;
        if (thisLen > otherLen) return 1;

        return 0;
    }

    public int CompareTo(IMutableString other)
    {
        var thisLen = Length;
        var otherLen = other.Length;
        if (thisLen < otherLen) return -1;
        if (thisLen > otherLen) return 1;
        var minLen = Math.Min(thisLen, otherLen);
        for (var i = 0; i < minLen; i++)
        {
            var cmp = sb[i] - other[i];
            if (cmp != 0) return cmp;
        }

        return 0;
    }

    public bool EquivalentTo(string other)
    {
        if (ReferenceEquals(null, other)) return false;
        return 0 == CompareTo(other);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<char> GetEnumerator()
    {
        var stringBuilderEnumerator = EnumeratorPool.Borrow();
        stringBuilderEnumerator.StringBuilder = sb;
        return stringBuilderEnumerator;
    }


    public override void StateReset()
    {
        sb.Clear();
        base.StateReset();
    }

    object ICloneable.Clone() => Clone();

    IMutableString ICloneable<IMutableString>.Clone() => Clone();

    public override IMutableString CopyFrom(IMutableString source, CopyMergeFlags copyMergeFlags)
    {
        if (source is MutableString mutableStringSource)
        {
            sb.Clear();
            sb.Append(mutableStringSource.sb);
        }

        return this;
    }

    public MutableString CopyFrom(MutableString source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        Clear();
        Append(source);
        return this;
    }

    public MutableString Append(IMutableString value)
    {
        for (var i = 0; i < value.Length; i++) Append(value[i]);
        return this;
    }

    public MutableString Append(bool value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(byte value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(char value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(char[] value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(decimal value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(double value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(short value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(int value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(long value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(object value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(sbyte value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(float value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(string value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(string value, int startIndex, int length)
    {
        sb.Append(value, startIndex, length);
        return this;
    }

    public MutableString Append(ushort value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(uint value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString Append(ulong value)
    {
        sb.Append(value);
        return this;
    }

    public MutableString AppendLine()
    {
        sb.AppendLine();
        return this;
    }

    public MutableString AppendLine(string value)
    {
        sb.AppendLine(value);
        return this;
    }

    public MutableString AppendLine(IMutableString value)
    {
        sb.Append(value).AppendLine();
        return this;
    }

    public MutableString Clear()
    {
        sb.Clear();
        return this;
    }

    public MutableString Insert(int atIndex, bool value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, byte value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, char value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, char[] value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, decimal value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, double value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, short value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, int value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, long value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, object value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, sbyte value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, float value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, string value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, char[] value, int startIndex, int length)
    {
        sb.Insert(atIndex, value, startIndex, length);
        return this;
    }

    public MutableString Insert(int atIndex, ushort value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, uint value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Insert(int atIndex, ulong value)
    {
        sb.Insert(atIndex, value);
        return this;
    }

    public MutableString Replace(char find, char replace)
    {
        sb.Replace(find, replace);
        return this;
    }

    public MutableString Replace(char find, char replace, int startIndex, int length)
    {
        sb.Replace(find, replace, startIndex, length);
        return this;
    }

    public MutableString Replace(string find, string replace)
    {
        sb.Replace(find, replace);
        return this;
    }

    public MutableString Replace(string find, string replace, int startIndex, int length)
    {
        sb.Replace(find, replace, startIndex, length);
        return this;
    }

    public MutableString Replace(IMutableString find, IMutableString replace) => Replace(find, replace, 0, Length);

    public MutableString Replace(IMutableString find, IMutableString replace, int startIndex, int length)
    {
        var fromIndex = startIndex;
        int indexOfFind;
        while ((indexOfFind = IndexOf(find, fromIndex)) >= 0
               && fromIndex < startIndex + length)
        {
            var remainingIndexOf = 0;
            var highestMatchIndex = indexOfFind;
            for (fromIndex = 0; fromIndex < indexOfFind + find.Length; fromIndex++)
                if (remainingIndexOf < replace.Length)
                {
                    highestMatchIndex = remainingIndexOf++;
                    sb[fromIndex] = replace[highestMatchIndex];
                }
                else
                {
                    Remove(highestMatchIndex + 1);
                }

            for (; remainingIndexOf < replace.Length; fromIndex++)
                sb.Insert(remainingIndexOf++, replace[highestMatchIndex]);
        }

        return this;
    }

    public override MutableString Clone() => Recycler?.Borrow<MutableString>() ?? new MutableString(this);

    public MutableString Remove(int startIndex)
    {
        sb.Remove(startIndex, 1);
        return this;
    }

    public MutableString ToUpper()
    {
        for (var i = 0; i < sb.Length; i++)
        {
            var oldChar = sb[i];
            var upperChar = char.ToUpperInvariant(oldChar);
            if (upperChar != oldChar) sb[i] = upperChar;
        }

        return this;
    }

    public MutableString ToLower()
    {
        for (var i = 0; i < sb.Length; i++)
        {
            var oldChar = sb[i];
            var upperChar = char.ToLowerInvariant(oldChar);
            if (upperChar != oldChar) sb[i] = upperChar;
        }

        return this;
    }

    public MutableString Trim()
    {
        for (var i = 0; i < sb.Length; i++)
        {
            var checkChar = sb[i];
            if (Array.IndexOf(WhiteSpaceChars, checkChar) >= 0) sb.Remove(i--, 1);
        }

        for (var i = sb.Length - 1; i >= 0; i--)
        {
            var checkChar = sb[i];
            if (Array.IndexOf(WhiteSpaceChars, checkChar) >= 0) sb.Remove(i++, 1);
        }

        return this;
    }

    public MutableString Substring(int startIndex)
    {
        sb.Remove(0, startIndex);
        return this;
    }

    public MutableString Substring(int startIndex, int length)
    {
        sb.Remove(0, startIndex);
        sb.Remove(length, Length - length);
        return this;
    }


    private bool Equals(MutableString? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return 0 == CompareTo(other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is MutableString s && Equals(s);
    }

    public override int GetHashCode()
    {
        var len = sb.Length;
        var hash = 0;
        for (var i = 0; i < len; i++) hash = (31 * hash) ^ sb[i];
        return hash;
    }

    public override string ToString() => sb.ToString();

    public static bool IsNullOrEmpty(IMutableString? test)
    {
        if (test == null) return true;
        for (var i = 0; i < test.Length; i++)
            if (Array.IndexOf(WhiteSpaceChars, test[i]) < 0)
                return false;
        return true;
    }

    public static implicit operator MutableString(string? initial) => new(initial);

    public static bool operator ==(string lhs, MutableString rhs)
    {
        if (ReferenceEquals(rhs, null)) return ReferenceEquals(lhs, null);
        return rhs.Equals(lhs);
    }

    public static bool operator !=(string lhs, MutableString rhs) => !(lhs == rhs);

    public static bool operator ==(MutableString? lhs, string? rhs)
    {
        if (ReferenceEquals(lhs, null)) return ReferenceEquals(rhs, null);
        if (ReferenceEquals(rhs, null)) return ReferenceEquals(lhs, null);
        return lhs.Equals(rhs);
    }

    public static bool operator !=(MutableString? lhs, string? rhs) => !(lhs == rhs);

    public static bool operator ==(MutableString? lhs, MutableString? rhs)
    {
        if (ReferenceEquals(lhs, null)) return ReferenceEquals(rhs, null);
        return lhs.Equals(rhs);
    }

    public static bool operator !=(MutableString? lhs, MutableString? rhs) => !(lhs == rhs);

    private class StringBuilderEnumerator : IEnumerator<char>
    {
        private readonly IPooledFactory<StringBuilderEnumerator> enumeratorPool;
        private int currentPosition = -1;
        private StringBuilder? sb;

        public StringBuilderEnumerator(IPooledFactory<StringBuilderEnumerator> enumeratorPool) =>
            this.enumeratorPool = enumeratorPool;

        public StringBuilder StringBuilder
        {
            set => sb = value;
        }

        public void Dispose()
        {
            Reset();
            sb = null;
            enumeratorPool.ReturnBorrowed(this);
        }

        public bool MoveNext() => ++currentPosition < sb!.Length;

        public void Reset()
        {
            currentPosition = -1;
        }

        public char Current => sb![currentPosition];

        object IEnumerator.Current => Current;
    }
}
