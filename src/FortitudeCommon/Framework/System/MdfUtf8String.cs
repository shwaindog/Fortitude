using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.Extensions;

namespace FortitudeCommon.Framework.System;

internal readonly unsafe partial struct MdUtf8String
{
    [LibraryImport("QCall", EntryPoint = "MdUtf8String_EqualsCaseInsensitive")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool EqualsCaseInsensitive(void* szLhs, void* szRhs, int cSz);

    private readonly byte* m_pStringHeap;        // This is the raw UTF8 string.
    private readonly int   m_StringHeapByteLength;

    internal MdUtf8String(void* pStringHeap)
    {
        byte* pStringBytes = (byte*)pStringHeap;
        if (pStringHeap != null)
        {
            m_StringHeapByteLength = Utf8StringExtensions.strlen(pStringBytes);
        }
        else
        {
            m_StringHeapByteLength = 0;
        }

        m_pStringHeap = pStringBytes;
    }

    internal MdUtf8String(byte* pUtf8String, int cUtf8String)
    {
        m_pStringHeap          = pUtf8String;
        m_StringHeapByteLength = cUtf8String;
    }

    // Very common called version of the Equals pair
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool Equals(MdUtf8String s)
    {
        if (s.m_StringHeapByteLength != m_StringHeapByteLength)
        {
            return false;
        }
        else
        {
            return Utf8StringExtensions.SequenceEqual(ref *s.m_pStringHeap, ref *m_pStringHeap, (uint)m_StringHeapByteLength);
        }
    }

    internal bool EqualsCaseInsensitive(MdUtf8String s)
    {
        if (s.m_StringHeapByteLength != m_StringHeapByteLength)
        {
            return false;
        }
        else
        {
            return (m_StringHeapByteLength == 0) || EqualsCaseInsensitive(s.m_pStringHeap, m_pStringHeap, m_StringHeapByteLength);
        }
    }

    public override string ToString()
        => Encoding.UTF8.GetString(new ReadOnlySpan<byte>(m_pStringHeap, m_StringHeapByteLength));
}