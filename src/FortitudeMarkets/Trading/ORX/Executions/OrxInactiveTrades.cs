// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols.ORX.Serdes;

#endregion

namespace FortitudeMarkets.Trading.ORX.Executions;

public class OrxInactiveTrades : ReusableObject<OrxInactiveTrades>
{
    public OrxInactiveTrades() { }

    public OrxInactiveTrades(bool getInactiveOrders) => GetInactiveOrders = getInactiveOrders;

    private OrxInactiveTrades(OrxInactiveTrades toClone)
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        CopyFrom(toClone);
    }

    [OrxMandatoryField(0)] public bool GetInactiveOrders { get; set; }

    public override OrxInactiveTrades CopyFrom
    (OrxInactiveTrades source
      , CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        GetInactiveOrders = source.GetInactiveOrders;
        return this;
    }

    public override OrxInactiveTrades Clone() => Recycler?.Borrow<OrxInactiveTrades>().CopyFrom(this) ?? new OrxInactiveTrades(this);
}
