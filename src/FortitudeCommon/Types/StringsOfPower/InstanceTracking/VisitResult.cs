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
  , int ReusedCount = 0 )
{

    
    public bool HasRegisteredVisit => VisitId.RegistryId >= -1 && VisitId.VisitIndex >= 0;
    
    public bool NoVisitCheckDone => VisitId.RegistryId <= VisitId.NoVisitCheckPerformedRegistryId;

    public bool NoRegistrationRequired => VisitId.RegistryId == VisitId.NoVisitCheckRequiredRegistryId || VisitId.VisitIndex < 0;

    public bool IsARevisit { get; set; } = InstanceId > 0 && LastRevisitCount > 0 && FirstInstanceMatchVisitIndex >= 0;
    
    public bool IsBaseOfInitial => ReusedCount > 0;

    public static readonly VisitResult VisitCheckNotRequired = new (VisitId.NoVisitRequiredId, VisitId.NoVisitRequiredId);
    
    public static readonly VisitResult VisitNotChecked = new (VisitId.NoVisitCheckPerformedId, VisitId.NoVisitCheckPerformedId);

    public override string ToString() => 
        $"VisitResult {{{nameof(VisitId)}: {VisitId.ToString()}, {nameof(RequesterVisitId)}: {RequesterVisitId.ToString()}," +
        $"{nameof(InstanceId)}: {InstanceId}, {nameof(FirstInstanceMatchVisitIndex)}: {FirstInstanceMatchVisitIndex} " +
        $"{nameof(ReusedCount)}: {ReusedCount}}}";

    public VisitResult WithIsARevisitSetTo(bool countAsRevisit)
    {
        return this with { IsARevisit = countAsRevisit };
    }
    
    public VisitResult WitCurrentVisitIdSetTo(VisitId updatedVisitId)
    {
        return this with { VisitId = updatedVisitId };
    }
    
    public VisitResult WitRequesterVisitIdSetTo(VisitId updateRequesterVisitId)
    {
        return this with { RequesterVisitId = updateRequesterVisitId };
    }
    
    public VisitResult WitReusedCount(int setReusedCount)
    {
        return this with { ReusedCount = setReusedCount };
    }
    
    public VisitResult IncrementUsedCount()
    {
        return this with { ReusedCount = ReusedCount + 1 };
    }
}
