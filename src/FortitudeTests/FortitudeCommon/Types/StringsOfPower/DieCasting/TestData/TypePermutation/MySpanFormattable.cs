using FortitudeCommon.Extensions;

namespace FortitudeTests.FortitudeCommon.Types.StringsOfPower.DieCasting.TestData.TypePermutation;

public class MySpanFormattableClass(string? data, bool sameAsToString = false) : ISpanFormattable
{
    private readonly string? data = data;
    
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (data?.Length > destination.Length)
        {
            charsWritten = 0;
            return false;
        }

        destination.OverWriteAt(0, data);
        charsWritten = data?.Length ?? 0;
        return true;
    }

    public override string ToString() => sameAsToString ? data! : "From - ToString() => " + data!;

    public string ToString(string? format, IFormatProvider? formatProvider) => data!;
    
    private bool Equals(MySpanFormattableClass other) => data == other.data;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MySpanFormattableClass)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => data?.GetHashCode() ?? 0;
}

public readonly struct MySpanFormattableStruct(string? data, bool sameAsToString = false) : ISpanFormattable
{
    private readonly string? data = data;
    
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (data?.Length > destination.Length)
        {
            charsWritten = 0;
            return false;
        }

        destination.OverWriteAt(0, data);
        charsWritten = data?.Length ?? 0;
        return true;
    }

    public override string ToString() => sameAsToString ? data! : "From - ToString() => " + data!;

    public string ToString(string? format, IFormatProvider? formatProvider) => data!;
    
    private bool Equals(MySpanFormattableStruct other) => data == other.data;

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        return Equals((MySpanFormattableStruct)obj);
    }

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    public override int GetHashCode() => data?.GetHashCode() ?? 0;
}
