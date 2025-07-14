namespace FortitudeCommon.EventProcessing.Disruption.Sequences;

public class Sequence(int initialValue, int wrapMask = int.MaxValue)
{
    public const int InitialValue = -1;

    private PaddedVolatileInt sequence = new(initialValue);

    public int Value
    {
        get => sequence.Value & wrapMask;
        set
        {
            sequence.Value = value & wrapMask;
        }
    }
}

public class SequenceLong(long initialValue)
{
    public const long InitialValue = -1L;

    private PaddedVolatileLong sequence = new(initialValue);

    public long Value
    {
        get => sequence.Value;
        set => sequence.Value = value;
    }
}

public static class SequenceHelpers
{
    public static int Min(this Sequence[] sequences)
    {
        var min = int.MaxValue;
        for (var i = 0; i < sequences.Length; i++)
        {
            var sequence            = sequences[i].Value;
            if (sequence < min) min = sequence;
        }

        return min;
    }

    public static long Min(this SequenceLong[] sequences)
    {
        var min = long.MaxValue;
        for (var i = 0; i < sequences.Length; i++)
        {
            var sequence            = sequences[i].Value;
            if (sequence < min) min = sequence;
        }

        return min;
    }
}
