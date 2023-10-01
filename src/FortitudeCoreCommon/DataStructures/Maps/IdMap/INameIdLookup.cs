namespace FortitudeCommon.DataStructures.Maps.IdMap;

public interface INameIdLookup : IIdLookup<string>
{
    string? GetName(int id);
    new INameIdLookup Clone();
}
