using System.Text;

namespace FortitudeCommon.Types.Mutable
{
    public interface IMutableStringBuilder<out T> where T : IMutableString, IMutableStringBuilder<T>
    {
        T Append(IMutableString value);
        T Append(bool value);
        T Append(byte value);
        T Append(char value);
        T Append(char[] value);
        T Append(decimal value);
        T Append(double value);
        T Append(short value);
        T Append(int value);
        T Append(long value);
        T Append(object value);
        T Append(sbyte value);
        T Append(float value);
        T Append(string value);
        T Append(string value, int startIndex, int length);
        T Append(ushort value);
        T Append(uint value);
        T Append(ulong value);
        T AppendLine();
        T AppendLine(string value);
        T AppendLine(IMutableString value);
        T Clear();
        T Insert(int atIndex, bool value);
        T Insert(int atIndex, byte value);
        T Insert(int atIndex, char value);
        T Insert(int atIndex, char[] value);
        T Insert(int atIndex, decimal value);
        T Insert(int atIndex, double value);
        T Insert(int atIndex, short value);
        T Insert(int atIndex, int value);
        T Insert(int atIndex, long value);
        T Insert(int atIndex, object value);
        T Insert(int atIndex, sbyte value);
        T Insert(int atIndex, float value);
        T Insert(int atIndex, string value);
        T Insert(int atIndex, char[] value, int startIndex, int length);
        T Insert(int atIndex, ushort value);
        T Insert(int atIndex, uint value);
        T Insert(int atIndex, ulong value);
        T Replace(char find, char replace);
        T Replace(char find, char replace, int startIndex, int length);
        T Replace(string find, string replace);
        T Replace(string find, string replace, int startIndex, int length);
        T Replace(IMutableString find, IMutableString replace);
        T Replace(IMutableString find, IMutableString replace, int startIndex, int length);
        StringBuilder GetBackingStringBuilder();
    }
}
