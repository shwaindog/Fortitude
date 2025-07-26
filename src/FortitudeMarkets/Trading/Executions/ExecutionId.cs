// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

#endregion

namespace FortitudeMarkets.Trading.Executions;

public class ExecutionId : ReusableObject<IExecutionId>, IExecutionId
{
    public ExecutionId()
    {
        VenueExecutionId = null!;
        BookingSystemId  = null!;
    }

    public ExecutionId(IExecutionId toClone)
    {
        VenueExecutionId   = toClone.VenueExecutionId;
        AdapterExecutionId = toClone.AdapterExecutionId;
        BookingSystemId    = toClone.BookingSystemId;
    }

    public ExecutionId(string venueExecutionId, int adapterExecutionId, string bookingSystemId)
        : this((MutableString)venueExecutionId, adapterExecutionId, (MutableString)bookingSystemId) { }

    public ExecutionId(IMutableString venueExecutionId, int adapterExecutionId, IMutableString bookingSystemId)
    {
        VenueExecutionId   = venueExecutionId;
        AdapterExecutionId = adapterExecutionId;
        BookingSystemId    = bookingSystemId;
    }

    public IMutableString VenueExecutionId   { get; set; }
    public int            AdapterExecutionId { get; set; }
    public IMutableString BookingSystemId    { get; set; }


    public override IExecutionId Clone() => Recycler?.Borrow<ExecutionId>().CopyFrom(this) ?? new ExecutionId(this);

    public override IExecutionId CopyFrom(IExecutionId source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        VenueExecutionId   = source.VenueExecutionId.CopyOrClone(VenueExecutionId as MutableString)!;
        AdapterExecutionId = source.AdapterExecutionId;
        BookingSystemId    = source.BookingSystemId.CopyOrClone(BookingSystemId as MutableString)!;
        return this;
    }
}
