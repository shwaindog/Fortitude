// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public record struct VisitResult
(
    int RegistryId = VisitResult.NoVisitCheckPerformedRegistryId
  , int CurrentVisitIndex = -1
  , int InstanceId = 0
  , int FirstVisitIndex = -1
  , int FirstInstanceMatchVisitIndex = -1  
  , int LastNodeVisitIndex = -1
  , int LastRevisitCount = -1
  , bool IsBaseOfInitial = false )
{

    public const int NoVisitCheckPerformedRegistryId = -3;
    public const int NoVisitCheckRequiredRegistryId = -2;
    
    public bool HasRegisteredVisit => RegistryId >= -1 && CurrentVisitIndex >= 0;
    
    public bool NoVisitCheckDone => RegistryId <= NoVisitCheckPerformedRegistryId;

    public bool NoRegistrationRequired => RegistryId == NoVisitCheckRequiredRegistryId || CurrentVisitIndex < 0;

    public bool IsARevisit { get; set; } = InstanceId > 0 && !IsBaseOfInitial && FirstInstanceMatchVisitIndex >= 0;

    public static readonly VisitResult VisitNotChecked = new (NoVisitCheckPerformedRegistryId, -1);
    

    public VisitResult WithIsARevisitSetTo(bool countAsRevisit)
    {
        return this with { IsARevisit = countAsRevisit };
    }
    
    public VisitResult WitCurrentVisitIndexSetTo(int updatedIndex)
    {
        return this with { CurrentVisitIndex = updatedIndex };
    }
}
