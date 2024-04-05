#region

using System.Reflection;

#endregion

namespace FortitudeIO.Protocols.ORX.Serdes;

[AttributeUsage(AttributeTargets.Property)]
public class OrxOptionalField : Attribute
{
    public readonly ushort Id;
    public readonly Dictionary<ushort, Type> Mapping;

    public OrxOptionalField(ushort id, ushort[]? mappingKey = null, Type[]? mappingType = null)
    {
        Id = id;
        Mapping = new Dictionary<ushort, Type>(mappingKey?.Length ?? 0);
        if (mappingKey != null && mappingType != null)
            for (var i = 0; i < mappingKey.Length; i++)
                Mapping.Add(mappingKey[i], mappingType[i]);
    }

    public static SortedList<ushort, PropertyInfo> FindAll(Type type)
    {
        var props = new SortedList<ushort, PropertyInfo>();
        foreach (var pi in type.GetProperties(BindingFlags.FlattenHierarchy
                                              | BindingFlags.Instance
                                              | BindingFlags.Public
                                              | BindingFlags.NonPublic))
        {
            var att = pi.GetCustomAttributes(typeof(OrxOptionalField), true);
            if (att.Length == 1 && pi.CanRead && pi.CanWrite) props.Add(((OrxOptionalField)att[0]).Id, pi);
        }

        return props;
    }
}
