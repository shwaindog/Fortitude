// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public record VisitResult
(
    int CurrentVisitIndex = -1
  , int InstanceId = 0
  , bool NewlyAssigned = false
  , int FirstVisitIndex = -1
  , int LastNodeVisitIndex = -1
  , int LastRevisitCount = -1
  , bool IsBaseOfInitial = false )
{
    public bool HasExistingInstanceId => InstanceId > 0 && !IsBaseOfInitial;
    
    public bool IsEmpty => CurrentVisitIndex < 0;
    
    public static VisitResult Empty => new (); 
}
