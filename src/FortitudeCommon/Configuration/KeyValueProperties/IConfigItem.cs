using System;

namespace FortitudeCommon.Configuration.KeyValueProperties
{
    public interface IConfigItem
    {
        string Category { get; }
        CompositeKey CompositeKey { get; set; }
        string Value { get; }
        string AuxilaryValue { get; }
        string Comment { get; }
        DateTime LastUpdatedDateTime { get; }
    }
}