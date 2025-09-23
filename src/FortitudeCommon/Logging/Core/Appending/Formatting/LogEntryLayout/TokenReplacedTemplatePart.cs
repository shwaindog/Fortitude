// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface ITokenReplacedTemplatePart : IStringBearer
{
    bool IsRightAligned { get; }
    bool IsLeftAligned { get; }
    int Padding { get; }
    string Layout { get; }
    string TokenName { get; }
    string FormatString { get; }
    SplitJoinRange SplitRange { get; }
    Range CharsRange { get; }
}

public class TokenFormatting : ITokenReplacedTemplatePart
{
    public TokenFormatting(string tokenName, int paddingLength, int charsToLength, string formattingString)
    {
        TokenName     = tokenName;
        Padding       = paddingLength;
        IsLeftAligned = paddingLength < 0;
        CharsRange =
            new Range(
                      Math.Abs(paddingLength)
                    , charsToLength == int.MaxValue ? Index.End : Index.FromStart(charsToLength));
        FormatString = 0.BuildStringBuilderFormatting(paddingLength, formattingString);

        Layout = charsToLength == 0 ? "" : charsToLength.ToString();
        Format = formattingString;
    }

    public TokenFormatting(string tokenFormatting, ITokenFormattingValidator? tokenFormattingValidator)
    {
        var tokenSpan = tokenFormatting.AsSpan();
        tokenSpan.ExtractExtendedStringFormatStages(out _, out var identifier, out var extendedLimitSizeRange,  out var layout
                                          , out var splitOutputRange, out var formatting, out _);
        Span<char> upperParam = stackalloc char[identifier.Length];
        identifier.ToUpper(upperParam, CultureInfo.InvariantCulture);
        TokenName = new string(upperParam);
        
        CharsRange      = extendedLimitSizeRange;
        IsAllCharRange  = extendedLimitSizeRange.IsAllRange();
        
        SplitRange      = splitOutputRange;
        HasSplitJoinFormatting = !SplitRange.IsNoSplitJoin;
        
        if (tokenFormattingValidator != null) formatting = tokenFormattingValidator.ValidateFormattingToken(TokenName, formatting);
        Layout  = new string(layout);
        Padding = int.TryParse(Layout, out var result) ? result : 0;
        Format  = new string(formatting);
        var zeroPosParam = "0".AsSpan();
        FormatString = zeroPosParam.BuildStringBuilderFormatting(CharsRange, layout, SplitRange, formatting);
    }

    public string Format { get; }
    public bool IsAllCharRange { get; private set; } = true;
    public bool HasSplitJoinFormatting { get; private set; } = true;

    public string TokenName { get; }

    public string Layout { get; }

    public string FormatString { get; }

    public Range CharsRange { get; }
    public SplitJoinRange SplitRange { get; }

    public bool IsRightAligned => !IsLeftAligned;
    public bool IsLeftAligned { get; }
    public int Padding { get; }

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
           .Field.AlwaysAdd(nameof(TokenName), TokenName)
           .Field.AlwaysAdd(nameof(FormatString), FormatString)
           .Field.WhenNonDefaultAdd(nameof(Layout), Layout)
           .Field.AlwaysAdd(nameof(Format), Format)
           .Field.WhenNonDefaultAdd(nameof(Padding), Padding)
           .Field.WhenNonDefaultAdd(nameof(IsLeftAligned), IsLeftAligned)
           .Field.WhenNonDefaultAdd(nameof(IsRightAligned), IsRightAligned, true)
           .Field.WhenNonDefaultAddMatch(nameof(CharsRange), CharsRange, Range.All)
           .Field.WhenNonDefaultAddMatch(nameof(SplitRange), SplitRange, SplitJoinRange.NoSplitJoin)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
