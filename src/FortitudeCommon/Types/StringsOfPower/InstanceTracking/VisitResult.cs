// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public record struct VisitResult
(
    VisitId VisitId
  , VisitId RequesterVisitId  
  , int InstanceId = 0
  , int FirstVisitIndex = -1
  , int FirstInstanceMatchVisitIndex = -1  
  , int LastNodeVisitIndex = -1
  , int LastRevisitCount = -1
  , bool IsBaseOfInitial = false )
{

    
    public bool HasRegisteredVisit => VisitId.RegistryId >= -1 && VisitId.VisitIndex >= 0;
    
    public bool NoVisitCheckDone => VisitId.RegistryId <= VisitId.NoVisitCheckPerformedRegistryId;

    public bool NoRegistrationRequired => VisitId.RegistryId == VisitId.NoVisitCheckRequiredRegistryId || VisitId.VisitIndex < 0;

    public bool IsARevisit { get; set; } = InstanceId > 0 && !IsBaseOfInitial && FirstInstanceMatchVisitIndex >= 0;

    public static readonly VisitResult VisitCheckNotRequired = new (VisitId.NoVisitRequiredId, VisitId.NoVisitRequiredId);
    
    public static readonly VisitResult VisitNotChecked = new (VisitId.NoVisitCheckPerformedId, VisitId.NoVisitCheckPerformedId);

    public override string ToString() => 
        $"VisitResult {{{nameof(VisitId)}: {VisitId.ToString()}, {nameof(RequesterVisitId)}: {RequesterVisitId.ToString()}," +
        $"{nameof(InstanceId)}: {InstanceId}, {nameof(FirstInstanceMatchVisitIndex)}: {FirstInstanceMatchVisitIndex} " +
        $"{nameof(IsBaseOfInitial)}: {IsBaseOfInitial}}}";

    public VisitResult WithIsARevisitSetTo(bool countAsRevisit)
    {
        return this with { IsARevisit = countAsRevisit };
    }
    
    public VisitResult WitCurrentVisitIndexSetTo(VisitId updatedVisitId)
    {
        return this with { VisitId = updatedVisitId };
    }
}
