using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FortitudeCommon.Extensions;

public static class CharArrayExtensions
{
    public static bool Contains(this char[] search, char subject)
    {
        for (int i = 0; i < search.Length; i++)
        {
            var checkChar = search[i];
            if (checkChar == subject) return true;
        }
        return false;
    }

    public static bool StartWith(this char[] search, string checkStartsWith)
    {
        var cappedEnd = Math.Min(search.Length, checkStartsWith.Length);
        if(cappedEnd == 0) return false;
        for (int i = 0; i < cappedEnd; i++)
        {
            var checkChar   = search[i];
            var compareChar = checkStartsWith[i];
            if (checkChar != compareChar) return false;
        }
        return true;
    }
    
    public static bool IsEquivalentTo(this char[] search, string checkIsSame, int fromIndex = 0, int count = int.MaxValue)
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