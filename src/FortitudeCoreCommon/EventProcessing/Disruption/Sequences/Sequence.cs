namespace FortitudeCommon.EventProcessing.Disruption.Sequences
{
    public class Sequence
    {
        public const long InitialValue = -1L;
        private PaddedVolatileLong sequence;

        public Sequence(long initialValue)
        {
            sequence = new PaddedVolatileLong(initialValue);
        }

        public long Value
        {
            get { return sequence.Value; }
            set { sequence.Value = value; }
        }
    }

    public static class SequenceHelpers
    {
        public static long Min(this Sequence[] sequences)
        {
            var min = long.MaxValue;
            for (var i = 0; i < sequences.Length; i++)
            {
                var sequence = sequences[i].Value;
                if (sequence < min)
                {
                    min = sequence;
                }
            }
            return min;
        }
    }
}