namespace FortitudeCommon.Types
{
    public interface IInterfacesComparable<T>
    {
        bool AreEquivalent(T other, bool exactTypes = false);
    }
}
