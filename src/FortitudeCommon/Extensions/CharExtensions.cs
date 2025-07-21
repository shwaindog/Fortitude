using System.Globalization;
using System.Runtime.CompilerServices;

namespace FortitudeCommon.Extensions;

public static class CharExtensions
{
    public static readonly char ThousandsSeparatorChar = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator[0];

    public static readonly char[] WhiteSpace = [' ', '\t', '\n', '\r'];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDigit(this char check) => check is >= '0' and <= '9';


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsWhiteSpace(this char check)
    {
        for (var i = 0; i < WhiteSpace.Length; i++)
        {
            if (check == WhiteSpace[i]) return true;
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMinus(this char check) => check == '-';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsThousandsSeparator(this char check) => check == ThousandsSeparatorChar;

}