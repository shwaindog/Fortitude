// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public record struct VisitResult
(
    int CurrentVisitIndex = -1
  , int InstanceId = 0
  , bool NewlyAssigned = false
  , int FirstVisitIndex = -1
  , int FirstInstanceMatchVisitIndex = -1  
  , int LastNodeVisitIndex = -1
  , int LastRevisitCount = -1
  , bool IsBaseOfInitial = false )
{
    
    public bool IsEmpty => CurrentVisitIndex < 0;

    public bool IsARevisit { get; set; } = InstanceId > 0 && !IsBaseOfInitial && FirstInstanceMatchVisitIndex >= 0;
    
    public static VisitResult Empty => new ();

    public VisitResult WithCountAsRevisitSetTo(bool countAsRevisit)
    {
        return this with { IsARevisit = countAsRevisit };
    }
}
