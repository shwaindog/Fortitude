// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public record struct VisitResult
(
    int RegistryId = -1
  , int CurrentVisitIndex = -1
  , int InstanceId = 0
  , int FirstVisitIndex = -1
  , int FirstInstanceMatchVisitIndex = -1  
  , int LastNodeVisitIndex = -1
  , int LastRevisitCount = -1
  , bool IsBaseOfInitial = false )
{
    
    public bool NoVisitCheckDone => RegistryId < -1;

    public bool IsARevisit { get; set; } = InstanceId > 0 && !IsBaseOfInitial && FirstInstanceMatchVisitIndex >= 0;

    public static readonly VisitResult VisitNotChecked = new (-2, -1);
    public static readonly VisitResult NoVisitCheckRequired = new (-1, -1);

    public VisitResult WithIsARevisitSetTo(bool countAsRevisit)
    {
        return this with { IsARevisit = countAsRevisit };
    }
}
