// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.Options;

namespace FortitudeCommon.Types.StringsOfPower;

public readonly struct TemporalFormattingStateChange
(
    bool shouldSkip
  , ISecretStringOfPower? stringMaster = null
  , StyleOptions? toRestoreOnDispose = null
  , StyleFormattingState? formattingState = null)
    : IDisposable
{
    public bool ShouldSkip => shouldSkip;

    public bool HasFormatChange => toRestoreOnDispose != null;

    public void Dispose()
    {
        if (toRestoreOnDispose != null && stringMaster != null)
        {
            stringMaster.Settings.CopyFrom(toRestoreOnDispose);
            if (formattingState != null)
            {
                var savedFormatter = formattingState.Value.StyleFormatter;

                stringMaster.CurrentStyledTypeFormatter = savedFormatter;

                stringMaster.GraphBuilder.GraphEncoder                   = formattingState.Value.GraphEncoder;
                stringMaster.GraphBuilder.ParentGraphEncoder             = formattingState.Value.ParentGraphEncoder;
                stringMaster.Settings.StyledTypeFormatter.ContentEncoder = formattingState.Value.StringEncoder;
            }
            ((IRecyclableObject)toRestoreOnDispose).DecrementRefCount();
        }
    }
}
