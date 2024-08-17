// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.AsyncProcessing.Tasks;

public interface ICanCarryTaskCallbackPayload
{
    bool IsTaskCallbackItem { get; }
    void SetAsTaskCallbackItem(SendOrPostCallback callback, object? state);
    void InvokeTaskCallback();

    void TaskPostProcessingCleanup()
    {
        // default implementation
        // no op do nothing
    }
}
