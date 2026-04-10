// Licensed under the MIT license.
// Copyright Alexis Sawenko 2026 all rights reserved

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.StringsOfPower.InstanceTracking;

namespace FortitudeCommon.Types.StringsOfPower.DieCasting;

public struct PreviousCompleted : IEquatable<PreviousCompleted>
{
    private object? instanceOrContainer;
    
    public VisitResult VisitResult { get; set; }
    
    public int? ItemCount { get; set; }

    public object? InstanceOrContainer
    {
        readonly get => instanceOrContainer;
        set
        {
            if (value is IRecyclableStructContainer valueRecyclableStructContainer)
            {
                valueRecyclableStructContainer.IncrementRefCount();
            }
            if (instanceOrContainer is IRecyclableStructContainer existingRecyclableStructContainer)
            {
                existingRecyclableStructContainer.DecrementRefCount();
            }
            instanceOrContainer = value;
        }
    }

    public Type TypeVisited { get; set; }
    
    public Type MoldType { get; set; }
    
    public CallerContext? CallerContext { get; set; }
    
    public static readonly PreviousCompleted Empty = new ();

    public bool Equals(PreviousCompleted other)
    {
        var instanceSame      = Equals(instanceOrContainer, other.instanceOrContainer);
        var visitSame         = VisitResult.Equals(other.VisitResult);
        var itemCountSame     = ItemCount == other.ItemCount;
        var typeVisitedSame   = TypeVisited == other.TypeVisited;
        var moldTypeSame      = MoldType == other.MoldType;
        var callerContextSame = Nullable.Equals(CallerContext, other.CallerContext);
        
        return instanceSame && visitSame && itemCountSame && typeVisitedSame && moldTypeSame && callerContextSame;
    }

    public override bool Equals(object? obj) => obj is PreviousCompleted other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(instanceOrContainer, VisitResult, ItemCount, TypeVisited, MoldType, CallerContext);
    
    public static bool operator  ==(PreviousCompleted lhs, PreviousCompleted rhs) => lhs.Equals(rhs);
    
    public static bool operator !=(PreviousCompleted lhs, PreviousCompleted rhs)  => !(lhs == rhs);
}
