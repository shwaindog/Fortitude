using System.Buffers;
using System.Text;
using System.Text.Encodings.Web;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TextJsonConverters;

public class MinimalTextJsonEncoder : JavaScriptEncoder
{
    public override int MaxOutputCharactersPerInputCharacter => JavaScriptEncoder.Default.MaxOutputCharactersPerInputCharacter;

    public override unsafe int FindFirstCharacterToEncode(char* text, int textLength)
    {
        ReadOnlySpan<char> input = new ReadOnlySpan<char>(text, textLength);
        int                idx   = 0;

        // Enumerate until we're out of data or saw invalid input
        while (Rune.DecodeFromUtf16(input.Slice(idx), out Rune result, out int charsConsumed) == OperationStatus.Done)
        {
            if (WillEncode(result.Value)) { break; } // found a char that needs to be escaped
            idx += charsConsumed;
        }

        if (idx == input.Length) { return -1; } // walked entire input without finding a char which needs escaping
        return idx;
    }

    public override bool WillEncode(int unicodeScalar)
    {
        // Allow all unicode chars above the extended (latin1) control chars 

        if (unicodeScalar > 0x00A0) { return false; } // does not require escaping
        else { return UnsafeRelaxedJsonEscaping.WillEncode(unicodeScalar); }
    }

    public override unsafe bool TryEncodeUnicodeScalar(int unicodeScalar, char* buffer, int bufferLength, out int numberOfCharactersWritten)
    {
        // For anything that needs to be escaped, defer to the default escaper.
        return JavaScriptEncoder.UnsafeRelaxedJsonEscaping.TryEncodeUnicodeScalar(unicodeScalar, buffer, bufferLength, out numberOfCharactersWritten);
    }
}