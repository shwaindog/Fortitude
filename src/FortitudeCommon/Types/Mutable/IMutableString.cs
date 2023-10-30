using System;
using System.Collections.Generic;

namespace FortitudeCommon.Types.Mutable
{
    public interface IMutableString : IMutableStringBuilder<IMutableString>, IEnumerable<char>
    {
        void CopyFrom(IMutableString source);
        void CopyFrom(string source);
        int Length { get; }
        char this[int index] { get; set; }
        bool EquivalentTo(String other); 
        IMutableString Clone();
        int CompareTo(String other);
        int CompareTo(IMutableString other);
        bool Contains(String subStr);
        bool Contains(IMutableString subStr);
        int IndexOf(String subStr);
        int IndexOf(IMutableString subStr);
        int IndexOf(String subStr, int fromThisPos);
        int IndexOf(IMutableString subStr, int fromThisPos);
        int LastIndexOf(String subStr);
        int LastIndexOf(IMutableString subStr);
        int LastIndexOf(String subStr, int fromThisPos);
        int LastIndexOf(IMutableString subStr, int fromThisPos);
        IMutableString Remove(int startIndex);
        IMutableString ToUpper();
        void ToUpper(IMutableString destMutableString);
        IMutableString ToLower();
        void ToLower(IMutableString destMutableString);
        IMutableString Trim();
        void Trim(IMutableString destMutableString);
        IMutableString Substring(int startIndex);
        void Substring(int startIndex, IMutableString destMutableString);
        IMutableString Substring(int startIndex, int length);
        void Substring(int startIndex, int length, IMutableString destMutableString);
        void Replace(string find, string replace, IMutableString destMutableString);
        string[] Split(char[] delimiters);
        void Split(char[] delimiters, IList<IMutableString> results, Func<IMutableString> mutableStringSupplier);
    }
}
