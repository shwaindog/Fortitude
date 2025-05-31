namespace FortitudeCommon.DataStructures.Lists;

public interface ICapacityList<out T> : IReadOnlyList<T>
{
    int Capacity { get; }
}

public interface IMutableCapacityList<T> : ICapacityList<T>, IList<T>
{
    new T this [int i] { get; set; }
    new int Count { get; set; }
    new int Capacity { get; set; }
}


public interface ICappedCapacityList<out T> : ICapacityList<T>
{
    ushort MaxAllowedSize { get; }
}

public interface IMutableCappedCapacityList<T> : ICappedCapacityList<T>, IMutableCapacityList<T>
{
    new ushort MaxAllowedSize { get; set; }
}
