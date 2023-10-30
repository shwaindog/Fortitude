namespace FortitudeCommon.DataStructures.Memory;

public interface IRecycleableObject
{
    bool ShouldAutoRecycle { get; set; }
    IRecycler? Recycler { get; set; }
}
