using FortitudeCommon.Types.StringsOfPower.Forge;

namespace FortitudeCommon.Extensions;

public static class CharSequenceExtensions
{
    
    public static bool SequenceMatches(this ICharSequence search, string checkIsSame, int fromIndex = 0, int count = int.MaxValue)
    {
        var cappedLength = Math.Min(count, checkIsSame.Length - fromIndex);
        if(checkIsSame.Length == cappedLength) return false;
        for (int i = 0; i < search.Length; i++)
        {
            var checkChar   = search[fromIndex + i];
            var compareChar = checkIsSame[i];
            if (checkChar != compareChar) return false;
        }
        return true;
    }
}
