namespace FortitudeIO.Protocols.ORX.Serialization;

[AttributeUsage(AttributeTargets.Class)]
public class OrxIsPreviousVersion : Attribute
{
    public OrxIsPreviousVersion(Type targetType, byte fromVersion, byte toVersion)
    {
        FromVersion = fromVersion;
        ToVersion = toVersion;
        TargetType = targetType;
    }

    public byte FromVersion { get; }

    public byte ToVersion { get; }

    public Type TargetType { get; }
}
