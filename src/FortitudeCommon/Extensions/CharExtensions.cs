using System.Globalization;
using System.Runtime.CompilerServices;

namespace FortitudeCommon.Extensions;

public static class CharExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char BottomNibbleToUpperChar(this int bottomNibble)
    {
        bottomNibble = ((0x0F) & bottomNibble);
        switch (bottomNibble)
        {
            case 0: return '0';
            case 1: return '1';
            case 2: return '2';
            case 3: return '3';
            case 4: return '4';
            case 5: return '5';
            case 6: return '6';
            case 7: return '7';
            case 8: return '8';
            case 9: return '9';
            case 10: return 'A';
            case 11: return 'B';
            case 12: return 'C';
            case 13: return 'D';
            case 14: return 'E';
            default: return 'F';
        }
    } 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char BottomNibbleToLowerChar(this int bottomNibble)
    {
        bottomNibble = ((0x0F) & bottomNibble);
        switch (bottomNibble)
        {
            case 0: return '0';
            case 1: return '1';
            case 2: return '2';
            case 3: return '3';
            case 4: return '4';
            case 5: return '5';
            case 6: return '6';
            case 7: return '7';
            case 8: return '8';
            case 9: return '9';
            case 10: return 'a';
            case 11: return 'b';
            case 12: return 'c';
            case 13: return 'd';
            case 14: return 'e';
            default: return 'f';
        }
    } 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char BottomNibbleToUpperChar(this uint bottomNibble)
    {
        bottomNibble = ((0x0F) & bottomNibble);
        switch (bottomNibble)
        {
            case 0: return '0';
            case 1: return '1';
            case 2: return '2';
            case 3: return '3';
            case 4: return '4';
            case 5: return '5';
            case 6: return '6';
            case 7: return '7';
            case 8: return '8';
            case 9: return '9';
            case 10: return 'A';
            case 11: return 'B';
            case 12: return 'C';
            case 13: return 'D';
            case 14: return 'E';
            default: return 'F';
        }
    } 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char BottomNibbleToLowerChar(this uint bottomNibble)
    {
        bottomNibble = ((0x0F) & bottomNibble);
        switch (bottomNibble)
        {
            case 0: return '0';
            case 1: return '1';
            case 2: return '2';
            case 3: return '3';
            case 4: return '4';
            case 5: return '5';
            case 6: return '6';
            case 7: return '7';
            case 8: return '8';
            case 9: return '9';
            case 10: return 'a';
            case 11: return 'b';
            case 12: return 'c';
            case 13: return 'd';
            case 14: return 'e';
            default: return 'f';
        }
    } 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char BottomNibbleToUpperChar(this long bottomNibble)
    {
        bottomNibble = ((0x0F) & bottomNibble);
        switch (bottomNibble)
        {
            case 0: return '0';
            case 1: return '1';
            case 2: return '2';
            case 3: return '3';
            case 4: return '4';
            case 5: return '5';
            case 6: return '6';
            case 7: return '7';
            case 8: return '8';
            case 9: return '9';
            case 10: return 'A';
            case 11: return 'B';
            case 12: return 'C';
            case 13: return 'D';
            case 14: return 'E';
            default: return 'F';
        }
    } 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char BottomNibbleToLowerChar(this long bottomNibble)
    {
        bottomNibble = ((0x0F) & bottomNibble);
        switch (bottomNibble)
        {
            case 0: return '0';
            case 1: return '1';
            case 2: return '2';
            case 3: return '3';
            case 4: return '4';
            case 5: return '5';
            case 6: return '6';
            case 7: return '7';
            case 8: return '8';
            case 9: return '9';
            case 10: return 'a';
            case 11: return 'b';
            case 12: return 'c';
            case 13: return 'd';
            case 14: return 'e';
            default: return 'f';
        }
    } 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char BottomNibbleToUpperChar(this ulong bottomNibble)
    {
        bottomNibble = ((0x0F) & bottomNibble);
        switch (bottomNibble)
        {
            case 0: return '0';
            case 1: return '1';
            case 2: return '2';
            case 3: return '3';
            case 4: return '4';
            case 5: return '5';
            case 6: return '6';
            case 7: return '7';
            case 8: return '8';
            case 9: return '9';
            case 10: return 'A';
            case 11: return 'B';
            case 12: return 'C';
            case 13: return 'D';
            case 14: return 'E';
            default: return 'F';
        }
    } 
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char BottomNibbleToLowerChar(this ulong bottomNibble)
    {
        bottomNibble = ((0x0F) & bottomNibble);
        switch (bottomNibble)
        {
            case 0: return '0';
            case 1: return '1';
            case 2: return '2';
            case 3: return '3';
            case 4: return '4';
            case 5: return '5';
            case 6: return '6';
            case 7: return '7';
            case 8: return '8';
            case 9: return '9';
            case 10: return 'a';
            case 11: return 'b';
            case 12: return 'c';
            case 13: return 'd';
            case 14: return 'e';
            default: return 'f';
        }
    } 
    
    
    public static readonly char ThousandsSeparatorChar = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator[0];

    public static readonly char[] WhiteSpace = [' ', '\t', '\n', '\r'];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDigit(this char check) => check is >= '0' and <= '9';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCarat(this char check) => check is '^';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOpenSquareBracket(this char check) => check is '[';

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCloseSquareBracket(this char check) => check is ']';


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