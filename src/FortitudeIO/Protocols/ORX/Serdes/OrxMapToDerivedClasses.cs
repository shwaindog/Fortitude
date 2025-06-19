// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Monitoring.Logging;

namespace FortitudeIO.Protocols.ORX.Serdes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class OrxMapToDerivedClasses : Attribute
{
    private static  IFLogger                 logger = FLoggerFactory.Instance.GetLogger(typeof(OrxMapToDerivedClasses));
    public readonly Dictionary<ushort, Type> Mapping;

    public OrxMapToDerivedClasses(ushort[]? mappingKey = null, Type[]? mappingType = null)
    {
        Mapping = new Dictionary<ushort, Type>(mappingKey?.Length ?? 0);
        if (mappingKey != null && mappingType != null)
            for (var i = 0; i < mappingKey.Length; i++)
                Mapping.Add(mappingKey[i], mappingType[i]);
    }
}
