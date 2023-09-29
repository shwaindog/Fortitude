using System;
using System.Collections.Generic;
using System.Reflection;

namespace FortitudeIO.Protocols.ORX.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OrxOptionalField : Attribute
    {
        public readonly ushort Id;
        public readonly Dictionary<ushort, Type> Mapping;

        public OrxOptionalField(ushort id, ushort[] mappingKey = null, Type[] mappingType = null)
        {
            Id = id;
            if (mappingKey != null && mappingType != null)
            {
                Mapping = new Dictionary<ushort, Type>(mappingKey.Length);
                for (int i = 0; i < mappingKey.Length; i++)
                {
                    Mapping.Add(mappingKey[i], mappingType[i]);
                }
            }
        }

        public static SortedList<ushort, PropertyInfo> FindAll(Type type)
        {
            SortedList<ushort, PropertyInfo> props = new SortedList<ushort, PropertyInfo>();
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.FlattenHierarchy
                | BindingFlags.Instance
                | BindingFlags.Public
                | BindingFlags.NonPublic))
            {
                object[] att = pi.GetCustomAttributes(typeof(OrxOptionalField), true);
                if (att.Length == 1 && pi.CanRead && pi.CanWrite)
                {
                    props.Add((att[0] as OrxOptionalField).Id, pi);
                }
            }
            return props;
        }
    }
}
