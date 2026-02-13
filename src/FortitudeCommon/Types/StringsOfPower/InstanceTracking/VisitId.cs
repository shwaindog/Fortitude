// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

namespace FortitudeCommon.Types.StringsOfPower.InstanceTracking;

public record struct VisitId(sbyte RegistryId, int VisitIndex)
{
    public const           int     NoVisitCheckPerformedRegistryId = -3;
    public const           int     NoVisitCheckRequiredRegistryId  = -2;
    
    public static readonly VisitId EmptyVisitId                    = new (-1, -1);
    
    public static readonly VisitId NoVisitRequiredId = new (NoVisitCheckRequiredRegistryId, -1);
    
    public static readonly VisitId NoVisitCheckPerformedId = new (NoVisitCheckPerformedRegistryId, -1);

    public readonly override string ToString() => $"VisitId {{{nameof(RegistryId)}: {RegistryId}, {nameof(VisitIndex)}: {VisitIndex} }}";
};
