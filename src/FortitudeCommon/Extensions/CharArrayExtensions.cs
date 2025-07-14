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
}