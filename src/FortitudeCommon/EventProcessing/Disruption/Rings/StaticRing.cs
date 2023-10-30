namespace FortitudeCommon.EventProcessing.Disruption.Rings;

public class StaticRing<T> : LoopingArray<T> where T : class, new()
{
    public StaticRing(int size, Func<T> dataFactory, bool allowOverwrite) : base(size, allowOverwrite)
    {
        for (var i = 0; i < Cells.Length; i++) Cells[i] = dataFactory();
    }

    public T Claim() => Cells[NextPublisherIndex()];
}
