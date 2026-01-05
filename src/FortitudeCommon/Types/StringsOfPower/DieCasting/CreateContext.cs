// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;


public struct CreateContext
{
    public CreateContext() { }

    public CreateContext(string? nameOverride = null, FormatFlags formatFlags = DieCasting.FormatFlags.DefaultCallerTypeFlags
      , string? formatString = null, ushort initiatorDefaultDepth = ushort.MaxValue, byte cappedMinRemainingDepth = 0)
    {
        NameOverride = nameOverride;
        FormatFlags  = formatFlags;
        FormatString = formatString;
        InitiatorDefaultDepth = initiatorDefaultDepth;
        CappedMinRemainingDepth = cappedMinRemainingDepth;
    }

    public string? NameOverride { get; set; }

    public FormatFlags FormatFlags { get; set; }

    public string? FormatString { get; set; }

    public ushort InitiatorDefaultDepth { get; set; } = ushort.MaxValue;
    
    public byte CappedMinRemainingDepth { get; set; }


    public static implicit operator CreateContext(string? formatString) => new() { FormatString = formatString };

    public static implicit operator CreateContext(FormatFlags formatFlags) => new() { FormatFlags = formatFlags };

    public static implicit operator CreateContext((string?, FormatFlags) formatStringAndFlags) =>
        new()
        {
            FormatString = formatStringAndFlags.Item1
          , FormatFlags  = formatStringAndFlags.Item2
        };
}
