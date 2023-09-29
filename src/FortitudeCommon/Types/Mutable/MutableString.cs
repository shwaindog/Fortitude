using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using FortitudeCommon.DataStructures.Memory;

namespace FortitudeCommon.Types.Mutable
{
    [SuppressMessage("ReSharper", "ForCanBeConvertedToForeach")]
    public sealed class MutableString : IMutableString
    {
        private static readonly IPooledFactory<StringBuilderEnumerator> EnumeratorPool = 
            new GarbageAndLockFreePooledFactory<StringBuilderEnumerator>(pool => new StringBuilderEnumerator(pool));
        private readonly StringBuilder sb;

        public MutableString()
        {
            sb = new StringBuilder();
        }

        public MutableString(IMutableString initialString)
        {
            sb = new StringBuilder();
            Append(initialString);
        }

        public MutableString(String initialString)
        {
            sb = new StringBuilder();
            sb.Append(initialString);
        }

        public MutableString(int initialCapacity)
        {
            sb = new StringBuilder(initialCapacity);
        }

        public MutableString(StringBuilder initializedBuilder)
        {
            sb = initializedBuilder;
        }

        public MutableString Append(IMutableString value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                Append(value[i]);
            }
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(IMutableString value)
        {
            return Append(value);
        }

        public MutableString Append(bool value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(bool value)
        {
            return Append(value);
        }

        public MutableString Append(byte value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(byte value)
        {
            return Append(value);
        }

        public MutableString Append(char value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(char value)
        {
            return Append(value);
        }

        public MutableString Append(char[] value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(char[] value)
        {
            return Append(value);
        }

        public MutableString Append(decimal value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(decimal value)
        {
            return Append(value);
        }

        public MutableString Append(double value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(double value)
        {
            return Append(value);
        }

        public MutableString Append(short value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(short value)
        {
            return Append(value);
        }

        public MutableString Append(int value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(int value)
        {
            return Append(value);
        }

        public MutableString Append(long value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(long value)
        {
            return Append(value);
        }

        public MutableString Append(object value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(object value)
        {
            return Append(value);
        }

        public MutableString Append(sbyte value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(sbyte value)
        {
            return Append(value);
        }

        public MutableString Append(float value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(float value)
        {
            return Append(value);
        }

        public MutableString Append(string value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(string value)
        {
            return Append(value);
        }

        public MutableString Append(string value, int startIndex, int length)
        {
            sb.Append(value, startIndex, length);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(string value, int startIndex, int length)
        {
            return Append(value, startIndex, length);
        }

        public MutableString Append(ushort value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(ushort value)
        {
            return Append(value);
        }

        public MutableString Append(uint value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(uint value)
        {
            return Append(value);
        }

        public MutableString Append(ulong value)
        {
            sb.Append(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Append(ulong value)
        {
            return Append(value);
        }

        public MutableString AppendLine()
        {
            sb.AppendLine();
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.AppendLine()
        {
            return AppendLine();
        }

        public MutableString AppendLine(string value)
        {
            sb.AppendLine(value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.AppendLine(string value)
        {
            return AppendLine(value);
        }

        public MutableString AppendLine(IMutableString value)
        {
            sb.Append(value).AppendLine();
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.AppendLine(IMutableString value)
        {
            return AppendLine(value);
        }

        public MutableString Clear()
        {
            sb.Clear();
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Clear()
        {
            return Clear();
        }

        public MutableString Insert(int atIndex, bool value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, bool value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, byte value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, byte value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, char value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, char value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, char[] value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, char[] value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, decimal value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, decimal value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, double value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, double value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, short value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, short value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, int value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, int value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, long value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, long value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, object value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, object value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, sbyte value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, sbyte value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, float value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, float value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, string value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, string value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, char[] value, int startIndex, int length)
        {
            sb.Insert(atIndex, value, startIndex, length);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, char[] value, 
            int startIndex, int length)
        {
            return Insert(atIndex, value, startIndex, length);
        }

        public MutableString Insert(int atIndex, ushort value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, ushort value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, uint value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, uint value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Insert(int atIndex, ulong value)
        {
            sb.Insert(atIndex, value);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Insert(int atIndex, ulong value)
        {
            return Insert(atIndex, value);
        }

        public MutableString Replace(char find, char replace)
        {
            sb.Replace(find, replace);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Replace(char find, char replace)
        {
            return Replace(find, replace);
        }

        public MutableString Replace(char find, char replace, int startIndex, int length)
        {
            sb.Replace(find, replace, startIndex, length);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Replace(char find, char replace, 
            int startIndex, int length)
        {
            return Replace(find, replace, startIndex, length);
        }

        public MutableString Replace(string find, string replace)
        {
            sb.Replace(find, replace);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Replace(string find, string replace)
        {
            return Replace(find, replace);
        }

        public MutableString Replace(string find, string replace, int startIndex, int length)
        {
            sb.Replace(find, replace, startIndex, length);
            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Replace(string find, string replace, 
            int startIndex, int length)
        {
            return Replace(find, replace, startIndex, length);
        }

        public MutableString Replace(IMutableString find, IMutableString replace)
        {
            return Replace(find, replace, 0, Length);
        }

        IMutableString IMutableStringBuilder<IMutableString>.Replace(IMutableString find, IMutableString replace)
        {
            return Replace(find, replace);
        }

        public MutableString Replace(IMutableString find, IMutableString replace, int startIndex, int length)
        {
            int fromIndex = startIndex;
            int indexOfFind;
            while ((indexOfFind = IndexOf(find, fromIndex)) >= 0 
                   && fromIndex < startIndex + length)
            {
                int remainingIndexOf = 0;
                int highestMatchIndex = indexOfFind;
                for (fromIndex = 0; fromIndex < indexOfFind + find.Length; fromIndex++)
                {
                    if (remainingIndexOf < replace.Length)
                    {
                        highestMatchIndex = remainingIndexOf++;
                        sb[fromIndex] = replace[highestMatchIndex];
                    }
                    else
                    {
                        Remove(highestMatchIndex + 1);
                    }
                }

                for (; remainingIndexOf < replace.Length; fromIndex++)
                {
                    sb.Insert(remainingIndexOf++, replace[highestMatchIndex]);
                }
            }

            return this;
        }

        IMutableString IMutableStringBuilder<IMutableString>.Replace(IMutableString find, IMutableString replace, 
            int startIndex, int length)
        {
            return Replace(find, replace, startIndex, Length);
        }

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

        public MutableString Clone()
        {
            return new MutableString(this);
        }

        IMutableString IMutableString.Clone()
        {
            return Clone();
        }

        public bool Contains(string subStr)
        {
            return IndexOf(subStr) >= 0;
        }

        public bool Contains(IMutableString subStr)
        {
            return IndexOf(subStr) >= 0;
        }

        public int IndexOf(string subStr)
        {
            return IndexOf(subStr, 0);
        }

        public int IndexOf(IMutableString subStr)
        {
            return IndexOf(subStr, 0);
        }

        public int IndexOf(string subStr, int fromThisPos)
        {
            int thisLen = Length;
            int otherLen = subStr.Length;
            if (fromThisPos > thisLen) return -1;
            int startPos = fromThisPos < 0 ? 0 : fromThisPos;
            char firstChar = subStr[0];
            int max = thisLen - otherLen;
            for (int i = startPos; i <= max; i++)
            {
                if (this[i] != firstChar)
                {
                    // ReSharper disable once EmptyEmbeddedStatement
                    while (++i <= max && this[i] != firstChar);
                }

                if (i <= max)
                {
                    int j = i + 1;
                    int end = i + otherLen;
                    // ReSharper disable once EmptyEmbeddedStatement
                    for (int k = 1; j < end && this[j] == subStr[k]; j++, k++);
                    if (j == end)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public int IndexOf(IMutableString subStr, int fromThisPos)
        {
            int thisLen = Length;
            int otherLen = subStr.Length;
            if (fromThisPos > thisLen) return -1;
            int startPos = fromThisPos < 0 ? 0 : fromThisPos;
            char firstChar = subStr[0];
            int max = thisLen - otherLen;
            for (int i = startPos; i <= max; i++)
            {
                if (this[i] != firstChar)
                {
                    // ReSharper disable once EmptyEmbeddedStatement
                    while (++i <= max && this[i] != firstChar);
                }

                if (i <= max)
                {
                    int j = i + 1;
                    int end = i + otherLen;
                    // ReSharper disable once EmptyEmbeddedStatement
                    for (int k = 1; j < end && this[j] == subStr[k]; j++, k++);
                    if (j == end)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public int LastIndexOf(string subStr)
        {
            return LastIndexOf(subStr, subStr.Length);
        }

        public int LastIndexOf(IMutableString subStr)
        {
            return LastIndexOf(subStr, subStr.Length);
        }

        public int LastIndexOf(string subStr, int fromThisPos)
        {
            int thisLen = Length;
            int otherLen = subStr.Length;
            if (fromThisPos > thisLen) return -1;
            char firstChar = subStr[0];
            int max = thisLen - otherLen;
            int startPos = fromThisPos > max ? max : fromThisPos;
            for (int i = startPos; i >= 0; i--)
            {
                if (this[i] != firstChar)
                {
                    // ReSharper disable once EmptyEmbeddedStatement
                    while (--i <= max && this[i] != firstChar);
                }

                if (i <= max)
                {
                    int j = i + 1;
                    int end = i + otherLen;
                    // ReSharper disable once EmptyEmbeddedStatement
                    for (int k = 1; j < end && this[j] == subStr[k]; j++, k++);
                    if (j == end)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public int LastIndexOf(IMutableString subStr, int fromThisPos)
        {
            int thisLen = Length;
            int otherLen = subStr.Length;
            if (fromThisPos > thisLen) return -1;
            char firstChar = subStr[0];
            int max = thisLen - otherLen;
            int startPos = fromThisPos > max ? max : fromThisPos;
            for (int i = startPos; i >= 0; i--)
            {
                if (this[i] != firstChar)
                {
                    // ReSharper disable once EmptyEmbeddedStatement
                    while (--i <= max && this[i] != firstChar);
                }

                if (i <= max)
                {
                    int j = i + 1;
                    int end = i + otherLen;
                    // ReSharper disable once EmptyEmbeddedStatement
                    for (int k = 1; j < end && this[j] == subStr[k]; j++, k++);
                    if (j == end)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        public MutableString Remove(int startIndex)
        {
            sb.Remove(startIndex, 1);
            return this;
        }

        IMutableString IMutableString.Remove(int startIndex)
        {
            return Remove(startIndex);
        }

        public MutableString ToUpper()
        {
            for (int i = 0; i < sb.Length; i++)
            {
                char oldChar = sb[i];
                char upperChar = char.ToUpperInvariant(oldChar);
                if (upperChar != oldChar)
                {
                    sb[i] = upperChar;
                }
            }
            return this;
        }

        IMutableString IMutableString.ToUpper()
        {
            return ToUpper();
        }

        public void ToUpper(IMutableString destMutableString)
        {
            destMutableString.Clear();
            for (int i = 0; i < sb.Length; i++)
            {
                char oldChar = sb[i];
                char upperChar = char.ToUpperInvariant(oldChar);
                destMutableString[i] = upperChar;
            }
        }

        public MutableString ToLower()
        {
            for (int i = 0; i < sb.Length; i++)
            {
                char oldChar = sb[i];
                char upperChar = char.ToLowerInvariant(oldChar);
                if (upperChar != oldChar)
                {
                    sb[i] = upperChar;
                }
            }
            return this;
        }

        IMutableString IMutableString.ToLower()
        {
            return ToLower();
        }

        public void ToLower(IMutableString destMutableString)
        {
            destMutableString.Clear();
            for (int i = 0; i < sb.Length; i++)
            {
                char oldChar = sb[i];
                char upperChar = char.ToLowerInvariant(oldChar);
                destMutableString[i] = upperChar;
            }
        }

        private static readonly char[] WhiteSpaceChars = {' ', '\t', '\r', '\n'};

        public MutableString Trim()
        {
            for (int i = 0; i < sb.Length; i++)
            {
                char checkChar = sb[i];
                if (Array.IndexOf(WhiteSpaceChars, checkChar) >= 0)
                {
                    sb.Remove(i--, 1);
                }
            }
            for (int i = sb.Length - 1; i >= 0; i--)
            {
                char checkChar = sb[i];
                if (Array.IndexOf(WhiteSpaceChars, checkChar) >= 0)
                {
                    sb.Remove(i++, 1);
                }
            }
            return this;
        }

        IMutableString IMutableString.Trim()
        {
            return Trim();
        }

        public void Trim(IMutableString destMutableString)
        {
            destMutableString.CopyFrom(destMutableString);
            destMutableString.Trim();
        }

        public MutableString Substring(int startIndex)
        {
            sb.Remove(0, startIndex);
            return this;
        }

        IMutableString IMutableString.Substring(int startIndex)
        {
            return Substring(startIndex);
        }

        public void Substring(int startIndex, IMutableString destMutableString)
        {
            destMutableString.Clear();
            for (int i = startIndex; i < Length; i++)
            {
                destMutableString.Append(this[i]);
            }
        }

        public MutableString Substring(int startIndex, int length)
        {
            sb.Remove(0, startIndex);
            sb.Remove(length, Length - length);
            return this;
        }

        IMutableString IMutableString.Substring(int startIndex, int length)
        {
            return Substring(startIndex, length);
        }

        public void Substring(int startIndex, int length, IMutableString destMutableString)
        {
            destMutableString.Clear();
            for (int i = startIndex; i < Length; i++)
            {
                destMutableString.Append(this[i]);
            }
        }

        public void Replace(string find, string replace, IMutableString destMutableString)
        {
            destMutableString.Clear();
            destMutableString.CopyFrom(this);
            destMutableString.Replace(find, replace);
        }

        public string[] Split(char[] delimiters)
        {
            int countDelimitersFound = 0;
            for (int i = 0; i < Length; i++)
            {
                var checkChar = sb[i];
                if (Array.IndexOf(delimiters, checkChar) > 0)
                {
                    countDelimitersFound++;
                }
            }

            var result = new string[countDelimitersFound + 1];
            int nextStringInsertIndex = 0;
            char[] nextString = new char[Length];
            int nextStringCharIndex = 0;
            for (int i = 0; i < Length; i++)
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
            IMutableString nextString = mutableStringSupplier();
            for (int i = 0; i < Length; i++)
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

        public StringBuilder GetBackingStringBuilder()
        {
            return sb;
        }

        public int CompareTo(string other)
        {
            int thisLen = Length;
            int otherLen = other.Length;
            int minLen = Math.Min(thisLen, otherLen);
            for (int i = 0; i < minLen; i++)
            {
                int cmp = sb[i] - other[i];
                if (cmp != 0)
                {
                    return cmp;
                }
            }
            if (thisLen < otherLen)
            {
                return -1;
            }
            if (thisLen > otherLen)
            {
                return 1;
            }

            return 0;
        }

        public int CompareTo(IMutableString other)
        {
            int thisLen = Length;
            int otherLen = other.Length;
            if (thisLen < otherLen)
            {
                return -1;
            }
            if (thisLen > otherLen)
            {
                return 1;
            }
            int minLen = Math.Min(thisLen, otherLen);
            for (int i = 0; i < minLen; i++)
            {
                int cmp = sb[i] - other[i];
                if (cmp != 0)
                {
                    return cmp;
                }
            }

            return 0;
        }

        public bool EquivalentTo(string other)
        {
            if (ReferenceEquals(null, other)) return false;
            return 0 == CompareTo(other);
        }

        private bool Equals(MutableString other)
        {
            if (ReferenceEquals(null, other)) return false;
            return 0 == CompareTo(other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is MutableString s && Equals(s);
        }

        public override int GetHashCode()
        {
            var len = sb.Length;
            var hash = 0;
            for (var i = 0; i < len; i++)
            {
                hash = 31 * hash ^ sb[i];
            }
            return hash;
        }

        public override string ToString()
        {
            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<char> GetEnumerator()
        {
            var stringBuilderEnumerator = EnumeratorPool.Borrow();
            stringBuilderEnumerator.StringBuilder = sb;
            return stringBuilderEnumerator;
        }

        private class StringBuilderEnumerator : IEnumerator<char>
        {
            private readonly IPooledFactory<StringBuilderEnumerator> enumeratorPool;
            private StringBuilder sb;
            private int currentPosition = -1;

            public StringBuilder StringBuilder
            {
                set => sb = value;
            }

            public StringBuilderEnumerator(IPooledFactory<StringBuilderEnumerator> enumeratorPool)
            {
                this.enumeratorPool = enumeratorPool;
            }

            public void Dispose()
            {
                Reset();
                sb = null;
                enumeratorPool.ReturnBorrowed(this);
            }

            public bool MoveNext()
            {
                return ++currentPosition < sb.Length;
            }

            public void Reset()
            {
                currentPosition = -1;
            }

            public char Current => sb[currentPosition];

            object IEnumerator.Current => Current;
        }

        public static bool IsNullOrEmpty(IMutableString test)
        {
            if (test == null) return true;
            for (int i = 0; i < test.Length; i++)
            {
                if (Array.IndexOf(WhiteSpaceChars, test[i]) < 0)
                {
                    return false;
                }
            }
            return true;
        }
        
        public static implicit operator MutableString(string initial)
        {
            if (initial == null) return null;
            return new MutableString(initial);
        }

        public static bool operator ==(string lhs, MutableString rhs)
        {
            if (ReferenceEquals(rhs, null))
            {
                return ReferenceEquals(lhs, null);
            }
            return rhs.Equals(lhs);
        }

        public static bool operator !=(string lhs, MutableString rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(MutableString lhs, string rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(MutableString lhs, string rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator ==(MutableString lhs, MutableString rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(MutableString lhs, MutableString rhs)
        {
            return !(lhs == rhs);
        }
    }
}