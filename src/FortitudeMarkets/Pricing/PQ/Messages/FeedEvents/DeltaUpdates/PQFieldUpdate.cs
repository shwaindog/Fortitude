// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.DataStructures.Memory;
using static FortitudeCommon.Extensions.NumberExtensions;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

public struct PQFieldUpdate
{
    public PQFieldFlags Flag;
    public PQFeedFields Id;
    public byte         SubIdByte;
    public PQDepthKey   DepthId;

    public ushort AuxiliaryPayload;
    public uint   Payload;

    public PQTickerDefSubFieldKeys DefinitionSubId => (PQTickerDefSubFieldKeys)SubIdByte;

    public PQPricingSubFieldKeys   PricingSubId  => (PQPricingSubFieldKeys)SubIdByte;
    public PQOrdersSubFieldKeys    OrdersSubId   => (PQOrdersSubFieldKeys)SubIdByte;
    public PQDecisionsSubFieldKeys DecisionSubId => (PQDecisionsSubFieldKeys)SubIdByte;
    public PQTradingSubFieldKeys   TradingSubId  => (PQTradingSubFieldKeys)SubIdByte;

    public PQFieldUpdate(PQFeedFields id, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)0, (ushort)0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)0, (ushort)0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)0, (ushort)0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)0, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQPricingSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQPricingSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTradingSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTradingSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, (uint)payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQOrdersSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQOrdersSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTickerDefSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTickerDefSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDecisionsSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDecisionsSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQPricingSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQPricingSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, (uint)payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQTradingSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQTradingSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, (uint)payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQOrdersSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQOrdersSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQTickerDefSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQTickerDefSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQDecisionsSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQDecisionsSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)0, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)0, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)0, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)0, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQPricingSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQPricingSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQTradingSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQTradingSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQOrdersSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQOrdersSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQTickerDefSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQTickerDefSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDecisionsSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDecisionsSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQPricingSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQPricingSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQTradingSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQTradingSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQOrdersSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQOrdersSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQTickerDefSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQTickerDefSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQDecisionsSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, PQDecisionsSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, byte subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
    {
        Id             = id;
        DepthId        = depthId;
        this.SubIdByte = subId;

        AuxiliaryPayload = auxiliaryPayload;

        var isNegativeFlag = payload < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;

        var additionalFlags = (this.SubIdByte > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);

        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags | isNegativeFlag;
        Payload = (uint)payload;
    }

    public PQFieldUpdate
        (PQFeedFields id, PQDepthKey depthId, byte subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
    {
        Id               = id;
        DepthId          = depthId;
        this.SubIdByte   = subId;
        AuxiliaryPayload = auxiliaryPayload;
        var additionalFlags = (this.SubIdByte > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);

        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags;
        Payload = payload;
    }

    public PQFieldUpdate(PQFeedFields id, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQPricingSubFieldKeys subId, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTradingSubFieldKeys subId, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQOrdersSubFieldKeys subId, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTickerDefSubFieldKeys subId, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDecisionsSubFieldKeys subId, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, byte subId, long value, PQFieldFlags flag = 0)
    {
        Id             = id;
        DepthId        = depthId;
        this.SubIdByte = subId;

        var scaledDownLong = PQScaling.ScaleDownLongTo48Bits(value);
        AuxiliaryPayload = (ushort)((scaledDownLong >> 32) & 0xFF_FF);

        var additionalFlags = (this.SubIdByte > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);
        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var scaleFactor    = PQScaling.FindVolumeScaleFactor48Bits(value);
        var depthIdFlag    = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags | isNegativeFlag | scaleFactor;
        Payload = (uint)(scaledDownLong & 0xFF_FF_FF_FF);
    }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, depthId, 0, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, 0, auxiliaryPayload, value, flag) { }


    public PQFieldUpdate(PQFeedFields id, PQPricingSubFieldKeys subId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTradingSubFieldKeys subId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQOrdersSubFieldKeys subId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTickerDefSubFieldKeys subId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDecisionsSubFieldKeys subId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, byte subId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
    {
        Id             = id;
        DepthId        = depthId;
        this.SubIdByte = subId;

        AuxiliaryPayload = auxiliaryPayload;

        var additionalFlags = (this.SubIdByte > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);
        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var scaleFactor    = PQScaling.FindVolumeScaleFactor32Bits(value);
        var depthIdFlag    = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag = flag | depthIdFlag | additionalFlags | isNegativeFlag | scaleFactor;
        var scaledDownLong = PQScaling.ScaleDownLongTo32Bits(value);
        Payload = (uint)(scaledDownLong & 0xFF_FF_FF_FF);
    }

    public PQFieldUpdate(PQFeedFields id, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, 0, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQPricingSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTradingSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQOrdersSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTickerDefSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDecisionsSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQPricingSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQTradingSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQOrdersSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQTickerDefSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, PQDecisionsSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, depthId, (byte)subId, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, 0, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQPricingSubFieldKeys subId, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTradingSubFieldKeys subId, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQOrdersSubFieldKeys subId, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQTickerDefSubFieldKeys subId, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDecisionsSubFieldKeys subId, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, (byte)subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, decimal value, PQFieldFlags flag = 0)
        : this(id, depthId, 0, 0, value, flag) { }

    public PQFieldUpdate(PQFeedFields id, PQDepthKey depthId, byte subId, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
    {
        Id             = id;
        DepthId        = depthId;
        this.SubIdByte = subId;

        AuxiliaryPayload = auxiliaryPayload;

        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var additionalFlags = (this.SubIdByte > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);
        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags | isNegativeFlag;
        Payload = PQScaling.Scale(value, Flag & PQFieldFlags.NegativeAndScaleMask);
    }

    public bool Equals(PQFieldUpdate other) =>
        Flag == other.Flag && Id == other.Id && DepthId == other.DepthId && SubIdByte == other.SubIdByte
     && AuxiliaryPayload == other.AuxiliaryPayload && Payload == other.Payload;

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is PQFieldUpdate && Equals((PQFieldUpdate)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Id.GetHashCode();
            hashCode = (hashCode * 397) ^ Flag.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)Payload;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"{nameof(PQFieldUpdate)}{{{nameof(Id)}: {Id}, " +
        $"{nameof(DepthId)}: ({(PQDepthKey)((ushort)DepthId & PQDepthKeyExtensions.TwoByteFlagsMask)}, {(ushort)(DepthId & PQDepthKey.DepthMask)}), " +
        $"{(Id.GetSubKeyType())}: {SubIdByte.ToSubIdString(Id)}, " +
        $"{nameof(AuxiliaryPayload)}: 0x{AuxiliaryPayload.ToHex2()}, {nameof(Payload)}: 0x{Payload.ToHex2()}, {nameof(Flag)}: {Flag}}}";
}

public static class PQFieldUpdateExtensions
{
    public static bool IsAsk(this PQFieldUpdate fieldUpdate) => fieldUpdate.DepthId.IsAsk();

    public static bool IsBid(this PQFieldUpdate fieldUpdate) => fieldUpdate.DepthId.IsBid();

    public static ushort DepthIndex(this PQFieldUpdate fieldUpdate) => (ushort)(fieldUpdate.DepthId & PQDepthKey.DepthMask);

    public static int RequiredBytes(this PQFieldUpdate fieldUpdate)
    {
        var fieldSize = sizeof(PQFieldFlags) + sizeof(PQFeedFields) + sizeof(uint);
        fieldSize += fieldUpdate.Flag.HasDepthKeyFlag()
            ? fieldUpdate.DepthId.IsTwoByteDepth()
                ? sizeof(ushort)
                : sizeof(byte)
            : 0;
        if (fieldUpdate.Flag.HasBothExtendedPayloadBytesFlags()) return fieldSize + sizeof(byte) + sizeof(ushort);
        fieldSize += fieldUpdate.Flag.HasSubIdFlag() ? sizeof(byte) : 0;
        fieldSize += fieldUpdate.Flag.HasAuxiliaryPayloadFlag() ? sizeof(ushort) : 0;
        return fieldSize;
    }

    public static PQFieldUpdate AtDepth(this PQFieldUpdate fieldUpdate, ushort atDepth) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId | atDepth.DepthToDepthKey(),
            fieldUpdate.SubIdByte, fieldUpdate.AuxiliaryPayload, fieldUpdate.Payload, fieldUpdate.Flag);

    public static PQFieldUpdate WithDepth(this PQFieldUpdate fieldUpdate, PQDepthKey atDepth) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId | atDepth,
            fieldUpdate.SubIdByte, fieldUpdate.AuxiliaryPayload, fieldUpdate.Payload, fieldUpdate.Flag);

    public static PQFieldUpdate WithNewPayload(this PQFieldUpdate fieldUpdate, uint newPayload) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId,
            fieldUpdate.SubIdByte, fieldUpdate.AuxiliaryPayload, newPayload, fieldUpdate.Flag);

    public static PQFieldUpdate MergePayloadFlagsWith(this PQFieldUpdate fieldUpdate, uint toBitMergeWithExisting) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId,
            fieldUpdate.SubIdByte, fieldUpdate.AuxiliaryPayload, fieldUpdate.Payload | toBitMergeWithExisting, fieldUpdate.Flag);

    public static PQFieldUpdate WithFieldId(this PQFieldUpdate fieldUpdate, PQFeedFields newField) =>
        new(newField, fieldUpdate.DepthId,
            fieldUpdate.SubIdByte, fieldUpdate.AuxiliaryPayload, fieldUpdate.Payload, fieldUpdate.Flag);

    public static PQFieldUpdate WithAuxiliary
        (this PQFieldUpdate fieldUpdate, ushort auxiliaryValue) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId, fieldUpdate.SubIdByte, auxiliaryValue, fieldUpdate.Payload, fieldUpdate.Flag);

    public static PQFieldUpdate SetIsAsk(this PQFieldUpdate fieldUpdate) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId | PQDepthKey.AskSide,
            fieldUpdate.SubIdByte, fieldUpdate.AuxiliaryPayload, fieldUpdate.Payload, fieldUpdate.Flag | PQFieldFlags.IncludesDepth);

    public static long ReadPayloadAndAuxiliaryAs48BitScaledLong(this PQFieldUpdate fieldUpdate) =>
        PQScaling.UnscaleLong(((long)fieldUpdate.AuxiliaryPayload << 32) | fieldUpdate.Payload, fieldUpdate.Flag);

    public static long ReadPayloadAs32BitScaledLong(this PQFieldUpdate fieldUpdate) => PQScaling.UnscaleLong(fieldUpdate.Payload, fieldUpdate.Flag);
}

public struct PQFieldStringUpdate
{
    public PQFieldUpdate  Field;
    public PQStringUpdate StringUpdate;

    public bool Equals(PQFieldStringUpdate other) => Field.Equals(other.Field) && StringUpdate.Equals(other.StringUpdate);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is PQFieldStringUpdate && Equals((PQFieldStringUpdate)obj);
    }


    public override int GetHashCode()
    {
        unchecked
        {
            return (Field.GetHashCode() * 397) ^ StringUpdate.GetHashCode();
        }
    }

    public override string ToString() => $"PQFieldStringUpdate {{ {nameof(Field)}: {Field}, {nameof(StringUpdate)}: {StringUpdate} }}";
}

public static class PQFieldStringUpdateExtensions
{
    public static PQFieldStringUpdate WithDepth(this PQFieldStringUpdate original, PQDepthKey depthKey)
    {
        var updated = original;
        updated.Field = original.Field.WithDepth(depthKey);
        return updated;
    }

    public static int RequiredBytes(this PQFieldStringUpdate fieldUpdate)
    {
        var fieldSize   = fieldUpdate.Field.RequiredBytes();
        var stringBytes = fieldUpdate.StringUpdate.RequiredBytes();
        return fieldSize + stringBytes;
    }

    public static PQFieldStringUpdate WithAuxiliary(this PQFieldStringUpdate original, ushort auxiliaryPayload)
    {
        var updated = original;
        updated.Field = original.Field.WithAuxiliary(auxiliaryPayload);
        return updated;
    }

    public static PQFieldStringUpdate SetIsAsk(this PQFieldStringUpdate original)
    {
        var updated = original;
        updated.Field = original.Field.SetIsAsk();
        return updated;
    }

    public static PQFieldStringUpdate WithDictionaryId(this PQFieldStringUpdate original, int dictionaryId)
    {
        var updated = original;
        updated.StringUpdate = original.StringUpdate.WithDictionaryId(dictionaryId);
        return updated;
    }

    public static bool IsBid(this PQFieldStringUpdate original) => original.Field.IsBid();

    public static bool IsAsk(this PQFieldStringUpdate original) => original.Field.IsAsk();
}

public struct PQStringUpdate
{
    public int         DictionaryId;
    public CrudCommand Command;
    public string      Value;

    public bool Equals(PQStringUpdate other) => DictionaryId == other.DictionaryId && Command == other.Command && string.Equals(Value, other.Value);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is PQStringUpdate && Equals((PQStringUpdate)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = DictionaryId;
            hashCode = (hashCode * 397) ^ (int)Command;
            hashCode = (hashCode * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            return hashCode;
        }
    }

    public override string ToString() =>
        $"PQStringUpdate {{ {nameof(DictionaryId)}: {DictionaryId}, {nameof(Command)}: {Command}, " +
        $"{nameof(Value)}: {Value} }}";
}

public static class PQStringUpdateExtensions
{
    public static int RequiredBytes(this PQStringUpdate stringUpdate)
    {
        var fixedSize   = sizeof(int) + sizeof(int);
        var stringBytes = StreamByteOps.RequiredBytes(stringUpdate.Value);
        return fixedSize + stringBytes;
    }

    public static PQStringUpdate WithDictionaryId(this PQStringUpdate original, int dictionaryId)
    {
        var updated = original;
        updated.DictionaryId = dictionaryId;
        return updated;
    }
}

public enum CrudCommand : byte
{
    None
  , Insert               = 1 // Create
  , Read                 = 2 // Read
  , Update               = 3 // Update
  , Delete               = 4 // Delete
  , Upsert               = 5 // Insert or Update
  , CommandReset         = 6 // Clear/Wipe keep position
  , CommandElementsShift = 7 // Similar to bit shift but for list elements
}

public static class CrudCommandExtensions
{
    public static PQPricingSubFieldKeys ToPQSubFieldId(this CrudCommand crudCommand) => (PQPricingSubFieldKeys)crudCommand;
    public static CrudCommand           ToCrudCommand(this ushort ushortCrudCommand) => (CrudCommand)ushortCrudCommand;
}
