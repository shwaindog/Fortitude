// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;

#endregion

namespace FortitudeCommon.AsyncProcessing.Tasks;

public class ActionToSendOrPostRecyclable : RecyclableObject
{
    public readonly SendOrPostCallback SendOrPostCallback;

    public Action<object?> ActionToInvoke = null!;

    public ActionToSendOrPostRecyclable() => SendOrPostCallback = Invoke;

    private void Invoke(object? state)
    {
        ActionToInvoke(state);
        DecrementRefCount();
    }

    public override void StateReset()
    {
        ActionToInvoke = null!;
        base.StateReset();
    }
}
