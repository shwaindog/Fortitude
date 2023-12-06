#region

using System.Reflection;
using FortitudeCommon.Monitoring.Logging;

#endregion

namespace FortitudeIO.Protocols.ORX.Serialization;

[AttributeUsage(AttributeTargets.Property)]
public class OrxMandatoryField : Attribute
{
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(OrxMandatoryField));
    public readonly ushort Index;
    public readonly Dictionary<ushort, Type> Mapping;

    public OrxMandatoryField(ushort index, ushort[]? mappingKey = null, Type[]? mappingType = null)
    {
        Index = index;
        Mapping = new Dictionary<ushort, Type>(mappingKey?.Length ?? 0);
        if (mappingKey != null && mappingType != null)
            for (var i = 0; i < mappingKey.Length; i++)
                Mapping.Add(mappingKey[i], mappingType[i]);
    }

    public static SortedList<ushort, PropertyInfo> FindAll(Type type)
    {
        var props = new SortedList<ushort, PropertyInfo>();
        foreach (var pi in type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance
                                                                            | BindingFlags.Public |
                                                                            BindingFlags.NonPublic))
        {
            var att = pi.GetCustomAttributes(typeof(OrxMandatoryField), true);
            if (att.Length == 1 && pi.CanRead && pi.CanWrite)
            {
                var indexToAdd = ((OrxMandatoryField)att[0]).Index;
                if (props.ContainsKey(indexToAdd))
                {
                    var message = $"Type {type.FullName} already contains MandatoryField attribute index {indexToAdd}";
                    logger.Error(message);
                    throw new ArgumentException(message);
                }

                props.Add(indexToAdd, pi);
            }
        }

        return props;
    }
}
