// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using static FortitudeCommon.Extensions.NumberFormattingExtensions;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

public struct PQFieldUpdate
{
    public PQFieldFlags    Flag;
    public PQQuoteFields   Id;
    public PQSubFieldKeys SubId;
    public PQDepthKey      DepthId;

    public ushort AuxiliaryPayload;
    public uint   Payload;

    public PQFieldUpdate(PQQuoteFields id, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, PQSubFieldKeys.None, 0, payload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, PQSubFieldKeys.None, 0, payload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, PQSubFieldKeys.None, 0, payload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, PQSubFieldKeys.None, 0, (uint)payload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, 0, payload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, (uint)payload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, PQSubFieldKeys subId, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, subId, 0, payload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, PQSubFieldKeys subId, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, subId, 0, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQQuoteFields id, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, PQSubFieldKeys.None, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQQuoteFields id, PQDepthKey depthId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, depthId, PQSubFieldKeys.None, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQQuoteFields id, PQSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, auxiliaryPayload, payload, flag) { }

    public PQFieldUpdate
        (PQQuoteFields id, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, PQSubFieldKeys.None, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQQuoteFields id, PQSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
        : this(id, subId, auxiliaryPayload, (uint)payload, flag) { }

    public PQFieldUpdate
        (PQQuoteFields id, PQDepthKey depthId, PQSubFieldKeys subId, ushort auxiliaryPayload, int payload, PQFieldFlags flag = 0)
    {
        Id               = id;
        DepthId          = depthId;
        SubId            = subId;

        AuxiliaryPayload = auxiliaryPayload;
        
        var isNegativeFlag = payload < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;

        var additionalFlags = (SubId > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);

        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags | isNegativeFlag;
        Payload = (uint)payload;
    }

    public PQFieldUpdate
        (PQQuoteFields id, PQDepthKey depthId, PQSubFieldKeys subId, ushort auxiliaryPayload, uint payload, PQFieldFlags flag = 0)
    {
        Id               = id;
        DepthId          = depthId;
        SubId            = subId;
        AuxiliaryPayload = auxiliaryPayload;
        var additionalFlags = (SubId > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);

        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags;
        Payload = payload;
    }

    public PQFieldUpdate(PQQuoteFields id, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, PQSubFieldKeys.None, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, PQSubFieldKeys subId, long value, PQFieldFlags flag = 0)
    {
        Id      = id;
        DepthId = depthId;
        SubId   = subId;

        var scaledDownLong = PQScaling.ScaleDownLongTo48Bits(value);
        AuxiliaryPayload = (ushort)((scaledDownLong >> 32) & 0xFF_FF);

        var additionalFlags = (SubId > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);
        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var scaleFactor    = PQScaling.FindVolumeScaleFactor48Bits(value);
        var depthIdFlag    = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags | isNegativeFlag | scaleFactor;
        Payload = (uint)(scaledDownLong & 0xFF_FF_FF_FF);
    }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, depthId, PQSubFieldKeys.None, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, PQSubFieldKeys.None, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQSubFieldKeys subId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, PQSubFieldKeys subId, ushort auxiliaryPayload, long value, PQFieldFlags flag = 0)
    {
        Id      = id;
        DepthId = depthId;
        SubId   = subId;

        AuxiliaryPayload = auxiliaryPayload;

        var additionalFlags = (SubId > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);
        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var scaleFactor    = PQScaling.FindVolumeScaleFactor32Bits(value);
        var depthIdFlag    = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag = flag | depthIdFlag | additionalFlags | isNegativeFlag | scaleFactor;
        var scaledDownLong = PQScaling.ScaleDownLongTo32Bits(value);
        Payload = (uint)(scaledDownLong & 0xFF_FF_FF_FF);
    }

    public PQFieldUpdate(PQQuoteFields id, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, PQSubFieldKeys.None, 0, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, 0, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, PQSubFieldKeys subId, decimal value, PQFieldFlags flag = 0)
        : this(id, depthId, subId, 0, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, PQSubFieldKeys.None, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQSubFieldKeys subId, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
        : this(id, PQDepthKey.None, subId, auxiliaryPayload, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, decimal value, PQFieldFlags flag = 0)
        : this(id, depthId, PQSubFieldKeys.None, 0, value, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, PQSubFieldKeys subId, ushort auxiliaryPayload, decimal value, PQFieldFlags flag = 0)
    {
        Id      = id;
        DepthId = depthId;
        SubId   = subId;

        AuxiliaryPayload = auxiliaryPayload;

        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var additionalFlags = (SubId > 0 ? PQFieldFlags.IncludesSubId : PQFieldFlags.None) |
                              (AuxiliaryPayload > 0 ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);
        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags | isNegativeFlag;
        Payload = PQScaling.Scale(value, Flag & PQFieldFlags.NegativeAndScaleMask);
    }

    public bool Equals(PQFieldUpdate other) =>
        Flag == other.Flag && Id == other.Id && DepthId == other.DepthId && SubId == other.SubId
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
        $"{nameof(PQFieldUpdate)}{{{nameof(Id)}: {Id}, {nameof(DepthId)}: ({(PQDepthKey)((ushort)DepthId & PQDepthKeyExtensions.TwoByteFlagsMask)}," +
        $"{(ushort)(DepthId & PQDepthKey.DepthMask)}), {nameof(SubId)}: {SubId}, {nameof(AuxiliaryPayload)}: 0x{AuxiliaryPayload.ToHex2()}, " +
        $"{nameof(Payload)}: 0x{Payload.ToHex2()}, {nameof(Flag)}: {Flag}}}";
}

public static class PQFieldUpdateExtensions
{
    public static bool IsAsk(this PQFieldUpdate fieldUpdate) => fieldUpdate.DepthId.IsAsk();

    public static bool IsBid(this PQFieldUpdate fieldUpdate) => fieldUpdate.DepthId.IsBid();

    public static ushort DepthIndex(this PQFieldUpdate fieldUpdate) => (ushort)(fieldUpdate.DepthId & PQDepthKey.DepthMask);

    public static int RequiredBytes(this PQFieldUpdate fieldUpdate)
    {
        var fieldSize = sizeof(PQFieldFlags) + sizeof(PQQuoteFields) + sizeof(uint);
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
            fieldUpdate.SubId, fieldUpdate.AuxiliaryPayload, fieldUpdate.Payload, fieldUpdate.Flag);

    public static PQFieldUpdate WithDepth(this PQFieldUpdate fieldUpdate, PQDepthKey atDepth) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId | atDepth,
            fieldUpdate.SubId, fieldUpdate.AuxiliaryPayload, fieldUpdate.Payload, fieldUpdate.Flag);

    public static PQFieldUpdate WithAuxiliary
        (this PQFieldUpdate fieldUpdate, ushort auxiliaryValue) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId, fieldUpdate.SubId, auxiliaryValue, fieldUpdate.Payload, fieldUpdate.Flag);

    public static PQFieldUpdate SetIsAsk(this PQFieldUpdate fieldUpdate) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId | PQDepthKey.AskSide,
            fieldUpdate.SubId, fieldUpdate.AuxiliaryPayload, fieldUpdate.Payload, fieldUpdate.Flag | PQFieldFlags.IncludesDepth);

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

public static class PQFieldStringUpdateExtensions
{
    public static PQFieldStringUpdate WithDepth(this PQFieldStringUpdate original, PQDepthKey depthKey)
    {
        var updated = original;
        updated.Field = original.Field.WithDepth(depthKey);
        return updated;
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

    public static bool IsBid(this PQFieldStringUpdate original) => original.Field.IsBid();

    public static bool IsAsk(this PQFieldStringUpdate original) => original.Field.IsAsk();
}

public enum CrudCommand : byte
{
    None
  , Insert = 1 // Create
  , Read   = 2 // Read
  , Update = 3 // Update
  , Delete = 4 // Delete
  , Upsert = 5 // Insert or Update
}

public static class CrudCommandExtensions
{
    public static PQSubFieldKeys      ToPQSubFieldId(this CrudCommand crudCommand)       => (PQSubFieldKeys)crudCommand;
    public static CrudCommand ToCrudCommand(this ushort ushortCrudCommand) => (CrudCommand)ushortCrudCommand;
}
