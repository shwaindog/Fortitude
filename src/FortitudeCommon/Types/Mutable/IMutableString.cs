#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.Types.Mutable;

public interface IMutableString : IMutableStringBuilder<IMutableString>, IEnumerable<char>
    , IReusableObject<IMutableString>
{
    int Length { get; }
    char this[int index] { get; set; }
    void CopyFrom(IMutableString source);
    void CopyFrom(string source);
    bool EquivalentTo(string other);
    int CompareTo(string other);
    int CompareTo(IMutableString other);
    bool Contains(string subStr);
    bool Contains(IMutableString subStr);
    int IndexOf(string subStr);
    int IndexOf(IMutableString subStr);
    int IndexOf(string subStr, int fromThisPos);
    int IndexOf(IMutableString subStr, int fromThisPos);
    int LastIndexOf(string subStr);
    int LastIndexOf(IMutableString subStr);
    int LastIndexOf(string subStr, int fromThisPos);
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


