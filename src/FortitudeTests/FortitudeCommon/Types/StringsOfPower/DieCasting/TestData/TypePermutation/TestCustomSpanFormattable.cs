using FortitudeCommon.Extensions;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

public class TestCustomSpanFormattable(string data) : ISpanFormattable
{
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (data.Length > destination.Length)
        {
            charsWritten = 0;
            return false;
        }

        destination.OverWriteAt(0, data);
        charsWritten = data.Length;
        return true;
    }

    public override string ToString() => data;

    public string ToString(string? format, IFormatProvider? formatProvider) => data;
}
