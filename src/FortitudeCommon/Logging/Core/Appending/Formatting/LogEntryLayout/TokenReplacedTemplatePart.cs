using System.Globalization;
using System.Text;
using FortitudeCommon.Extensions;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;

public interface ITokenReplacedTemplatePart : IStyledToStringObject
{
    string TokenName { get; }
    string FormatString { get; }
}

public class TokenFormatting : ITokenReplacedTemplatePart
{
    [ThreadStatic] protected static StringBuilder? scratchBuffer;

    protected readonly string formatString;

    public readonly int MaxLength = int.MaxValue;
    
    public TokenFormatting(string tokenName, int paddingLength, int maxLength, string formattingString)
    {
        TokenName       = tokenName;
        MaxLength = maxLength;
        formatString = 0.BuildStringBuilderFormatting(paddingLength, formattingString);
    }

    public TokenFormatting(string tokenFormatting, ITokenFormattingValidator? tokenFormattingValidator)
    {
        var tokenSpan = tokenFormatting.AsSpan();
        tokenSpan.ExtractStringFormatStages(out var identifier, out var layout, out var formatting);
        Span<char> upperParam = stackalloc char[identifier.Length];
        identifier.ToUpper(upperParam, CultureInfo.InvariantCulture);
        TokenName = new String(upperParam);

        if (layout.Length > 0)
        {
            var foundCapStringLength = layout.IndexOf("[..");
            if (foundCapStringLength >= 0)
            {
                var digitsSlice = layout.ExtractDigitsSlice(foundCapStringLength + 3);
                if (digitsSlice.Length > 0)
                {
                    MaxLength = int.TryParse(digitsSlice, out var attempt) ? attempt : int.MaxValue;
                }
                layout = layout.Slice(0, foundCapStringLength);
            }
        }
        if (tokenFormattingValidator != null)
        {
            formatting = tokenFormattingValidator.ValidateFormattingToken(TokenName, formatting);
        }
        var zeroPosParam = "0".AsSpan();
        formatString = zeroPosParam.BuildStringBuilderFormatting(layout, formatting);
    }

    public string TokenName { get; private set; }

    public string FormatString => formatString;
    
    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender sbc)
    {
        return
            sbc.StartComplexType(nameof(TokenFormatting))
               .Field.AlwaysAdd(nameof(TokenName), TokenName)
               .Field.AlwaysAdd(nameof(formatString), formatString)
               .Field.WhenNonDefaultAdd(nameof(MaxLength), MaxLength, int.MaxValue)
               .Complete();
    }

    public override string ToString() => this.DefaultToString();
}
