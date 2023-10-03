using System;

namespace FortitudeCommon.Types
{
    public interface ICloneable<out T> : ICloneable
    {
        new T Clone();
    }
}
