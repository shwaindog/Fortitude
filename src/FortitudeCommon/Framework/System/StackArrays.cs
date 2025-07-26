using System.Runtime.CompilerServices;

namespace FortitudeCommon.Framework.System;

// Taken from Microsoft Framework
// System.TwoObjects
// System.ThreeObjects

[InlineArray(2)]
public struct TwoObjects
{
    internal object? Arg0;

    public TwoObjects(object? arg0, object? arg1)
    {
        this[0] = arg0;
        this[1] = arg1;
    }
}

[InlineArray(3)]
public struct ThreeObjects
{
    internal object? Arg0;

    public ThreeObjects(object? arg0, object? arg1, object? arg2)
    {
        this[0] = arg0;
        this[1] = arg1;
        this[2] = arg2;
    }
}