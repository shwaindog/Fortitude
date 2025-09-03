// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Globalization;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface ITokenReplacedTemplatePart : IStyledToStringObject
{
    bool IsRightAligned { get; }
    bool IsLeftAligned { get; }
    int Padding { get; }
    string Layout { get; }
    string TokenName { get; }
    string FormatString { get; }
    Range SplitRange { get; }
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
        tokenSpan.ExtractStringFormatStages(out var identifier, out var layout, out var formatting);
        Span<char> upperParam = stackalloc char[identifier.Length];
        identifier.ToUpper(upperParam, CultureInfo.InvariantCulture);
        TokenName = new string(upperParam);

        if (layout.Length > 0)
        {
            IsLeftAligned = layout[0].IsMinus();
            if (layout[0].IsDigit() || IsLeftAligned)
            {
                layout.LayoutStringRangeIndexers(out var range);
                CharsRange     = range;
                IsAllCharRange = range.IsAllRange();
            }
            else if (layout[0].IsOpenSquareBracket())
            {
                var foundAt = layout.ExtractRangeFromSliceExpression(out var nullableCharRange);
                if (foundAt >= 0)
                {
                    SplitRange      = nullableCharRange!.Value;
                    IsAllSplitRange = SplitRange.IsAllRange();
                }
                layout = "".AsSpan();
            }
        }
        if (tokenFormattingValidator != null) formatting = tokenFormattingValidator.ValidateFormattingToken(TokenName, formatting);
        Layout  = new string(layout);
        Padding = int.TryParse(Layout, out var result) ? result : 0;
        Format  = new string(formatting);
        var zeroPosParam = "0".AsSpan();
        FormatString = zeroPosParam.BuildStringBuilderFormatting(layout, formatting);
    }

    public string Format { get; }
    public bool IsAllCharRange { get; private set; } = true;
    public bool IsAllSplitRange { get; private set; } = true;

    public string TokenName { get; }

    public string Layout { get; }

    public string FormatString { get; }

    public Range CharsRange { get; }
    public Range SplitRange { get; }

    public bool IsRightAligned => !IsLeftAligned;
    public bool IsLeftAligned { get; }
    public int Padding { get; }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) =>
        stsa.StartComplexType(this)
           .Field.AlwaysAdd(nameof(TokenName), TokenName)
           .Field.AlwaysAdd(nameof(FormatString), FormatString)
           .Field.WhenNonDefaultAdd(nameof(Layout), Layout)
           .Field.AlwaysAdd(nameof(Format), Format)
           .Field.WhenNonDefaultAdd(nameof(Padding), Padding)
           .Field.WhenNonDefaultAdd(nameof(IsLeftAligned), IsLeftAligned)
           .Field.WhenNonDefaultAdd(nameof(IsRightAligned), IsRightAligned, true)
           .Field.WhenNonDefaultAddMatch(nameof(CharsRange), CharsRange, Range.All)
           .Field.WhenNonDefaultAddMatch(nameof(SplitRange), SplitRange, Range.All)
           .Complete();

    public override string ToString() => this.DefaultToString();
}
