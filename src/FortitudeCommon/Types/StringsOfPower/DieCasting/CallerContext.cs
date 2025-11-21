// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types.StringsOfPower.DieCasting.TypeFields;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public struct CallerContext
{
    private uint packedDepthDelta;
    
    public FieldContentHandling FormatFlags { get; set; }

    public string? FormatString { get; set; }
    
    public sbyte InitiatorDepthDelta
    {
        get => unchecked ((sbyte)(byte)(packedDepthDelta & 0xFF));
        set => packedDepthDelta = packedDepthDelta & 0xFFFFFF00 | unchecked((byte)value);
    }

    public sbyte InitiatorChildDepthDelta
    {
        get => unchecked ((sbyte)(byte)((packedDepthDelta >> 8) & 0xFF));
        set => packedDepthDelta = packedDepthDelta & 0xFFFF00FF | ((uint)unchecked((byte)value) << 8);
    }

    public sbyte InitiatorDescendantDepthDelta
    {
        get => unchecked ((sbyte)(byte)((packedDepthDelta >> 16) & 0xFF));
        set => packedDepthDelta = packedDepthDelta & 0xFF00FFFF | ((uint)unchecked((byte)value) << 16);
    }

    public byte CappedMinRemainingDepth
    {
        get => unchecked ((byte)((packedDepthDelta >> 24) & 0xFF));
        set => packedDepthDelta = packedDepthDelta & 0x00FFFFFF | ((uint)value << 24);
    }

    public static implicit operator CallerContext(string? formatString)             => new() { FormatString = formatString };
    public static implicit operator CallerContext(FieldContentHandling formatFlags) => new() { FormatFlags  = formatFlags };
    
    public static implicit operator CallerContext((string?, FieldContentHandling) formatStringAndFlags) => 
        new()
        {
            FormatString = formatStringAndFlags.Item1
          , FormatFlags  = formatStringAndFlags.Item2
        };
}
