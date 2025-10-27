using System.Text;
using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Extensions;

public static class CharSequenceExtensions
{
    public static bool SequenceMatches(this ICharSequence search, string checkIsSame, int fromIndex = 0, int count = int.MaxValue)
    {
        fromIndex = Math.Clamp(fromIndex, 0, search.Length);
        var cappedLength = Math.Min(count, search.Length - fromIndex);
        if (checkIsSame.Length != cappedLength) return false;
        for (int i = 0; i < cappedLength; i++)
        {
            var checkChar   = search[fromIndex + i];
            var compareChar = checkIsSame[i];
            if (checkChar != compareChar) return false;
        }
        return true;
    }

    public static bool SequenceMatches(this ICharSequence search, ICharSequence checkIsSame)
    {
        var fromIndex    = search.Length;
        var cappedLength = Math.Min(fromIndex, checkIsSame.Length);
        if (checkIsSame.Length != cappedLength) return false;
        for (int i = 0; i < cappedLength; i++)
        {
            var checkChar   = search[fromIndex + i];
            var compareChar = checkIsSame[i];
            if (checkChar != compareChar) return false;
        }
        return true;
    }

    public static bool UnknownSequenceMatches<T>(this T? lhs, T? rhs)
    {
        switch (lhs)
        {
            case char[] lhsCharArray:            return lhsCharArray.SequenceMatches(((char[])(object)rhs!));
            case string lhsString:               return lhsString.SequenceMatches(((string)(object)rhs!));
            case ICharSequence lhsCharSequence:  return lhsCharSequence.SequenceMatches(((ICharSequence)rhs!));
            case StringBuilder lhsStringBuilder: return lhsStringBuilder.SequenceMatches(((StringBuilder)(object)rhs!));
            default:                             return false;
        }
    }

    public static char LastNonWhiteChar(this ICharSequence sb)
    {
        if (sb.Length == 0) return '\0';
        var i = sb.Length - 1;
        for (; i >= 0 && sb[i].IsWhiteSpace(); i--) ; // no op
        return sb[Math.Max(0, i)];
    }
}
