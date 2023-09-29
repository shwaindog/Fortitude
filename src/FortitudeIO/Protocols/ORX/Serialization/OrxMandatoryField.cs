using System;
using System.Collections.Generic;
using System.Reflection;

namespace FortitudeIO.Protocols.ORX.Serialization
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OrxMandatoryField : Attribute
    {
        public readonly ushort Index;
        public readonly Dictionary<ushort, Type> Mapping;

        public OrxMandatoryField(ushort index, ushort[] mappingKey = null, Type[] mappingType = null)
        {
            Index = index;
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
            foreach (PropertyInfo pi in type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance
                                                           | BindingFlags.Public | BindingFlags.NonPublic))
            {
                object[] att = pi.GetCustomAttributes(typeof(OrxMandatoryField), true);
                if (att.Length == 1 && pi.CanRead && pi.CanWrite)
                {
                    props.Add((att[0] as OrxMandatoryField).Index, pi);
                }
            }
            return props;
        }
    }
}