namespace FortitudeCommon.DataStructures.Maps;

public class LinkedListUintKeyMap<Tv> : LinkedListCache<uint, Tv>
{
    public override bool TryGetValue(uint key, out Tv? value)
    {
        var currentNode = Chain.Head;
        for (; currentNode != null; currentNode = currentNode.Next)
        {
            if (currentNode.Payload.Key != key) continue;
            value = currentNode.Payload.Value;
            return true;
        }

        value = default;
        return false;
    }
}
