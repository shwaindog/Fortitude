// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using static FortitudeCommon.Extensions.NumberFormattingExtensions;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

public struct PQFieldUpdate
{
    public PQFieldFlags  Flag;
    public PQQuoteFields Id;
    public PQDepthKey    DepthId;
    public ushort        AuxiliaryPayload;
    public ushort        ExtendedPayload;
    public uint          Payload;


    public PQFieldUpdate(PQQuoteFields id, uint payload, PQFieldFlags flag = 0)
    {
        Id      = id;
        Flag    = flag;
        Payload = payload;
    }

    public PQFieldUpdate(PQQuoteFields id, int payload, PQFieldFlags flag = 0)
        : this(id, (uint)payload, flag) { }


    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, uint payload, PQFieldFlags flag = 0)
    {
        Id      = id;
        DepthId = depthId;
        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag;
        Payload = payload;
    }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, int payload, PQFieldFlags flag = 0)
        : this(id, depthId, (uint)payload, flag) { }


    public PQFieldUpdate(PQQuoteFields id, uint payload, ushort extendedPayload, PQFieldFlags flag = 0)
    {
        Id              = id;
        ExtendedPayload = extendedPayload;

        var hasExtended     = ExtendedPayload > 0;
        var additionalFlags = hasExtended ? PQFieldFlags.IncludesExtendedPayLoad : PQFieldFlags.None;

        Flag    = flag | additionalFlags;
        Payload = payload;
    }

    public PQFieldUpdate(PQQuoteFields id, int payload, ushort extendedPayload, PQFieldFlags flag = 0)
        : this(id, (uint)payload, extendedPayload, flag) { }


    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, uint payload, ushort extendedPayload, PQFieldFlags flag = 0)
    {
        Id      = id;
        DepthId = depthId;

        ExtendedPayload = extendedPayload;
        var hasExtended     = ExtendedPayload > 0;
        var additionalFlags = hasExtended ? PQFieldFlags.IncludesExtendedPayLoad : PQFieldFlags.None;

        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags;
        Payload = payload;
    }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, int payload, ushort extendedPayload, PQFieldFlags flag = 0)
        : this(id, depthId, (uint)payload, extendedPayload, flag) { }


    public PQFieldUpdate(PQQuoteFields id, uint payload, uint extendedPayload, PQFieldFlags flag = 0)
    {
        Id               = id;
        ExtendedPayload  = (ushort)(extendedPayload & 0xFF_FF);
        AuxiliaryPayload = (ushort)((extendedPayload >> 16) & 0xFF_FF);
        var hasExtended         = ExtendedPayload > 0;
        var hasOptionalExtended = AuxiliaryPayload > 0;
        var additionalFlags = (hasExtended ? PQFieldFlags.IncludesExtendedPayLoad : PQFieldFlags.None) |
                              (hasOptionalExtended ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);

        Flag    = flag | additionalFlags;
        Payload = payload;
    }

    public PQFieldUpdate(PQQuoteFields id, int payload, uint extendedPayload, PQFieldFlags flag = 0)
        : this(id, (uint)payload, extendedPayload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, int payload, int extendedPayload, PQFieldFlags flag = 0)
        : this(id, (uint)payload, (uint)extendedPayload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, uint payload, uint extendedPayload, PQFieldFlags flag = 0)
    {
        Id               = id;
        DepthId          = depthId;
        ExtendedPayload  = (ushort)(extendedPayload & 0xFF_FF);
        AuxiliaryPayload = (ushort)((extendedPayload >> 16) & 0xFF_FF);
        var hasExtended         = ExtendedPayload > 0;
        var hasOptionalExtended = AuxiliaryPayload > 0;
        var additionalFlags = (hasExtended ? PQFieldFlags.IncludesExtendedPayLoad : PQFieldFlags.None) |
                              (hasOptionalExtended ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);

        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags;
        Payload = payload;
    }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, int payload, uint extendedPayload, PQFieldFlags flag = 0)
        : this(id, (uint)payload, extendedPayload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, int payload, int extendedPayload, PQFieldFlags flag = 0)
        : this(id, (uint)payload, (uint)extendedPayload, flag) { }


    public PQFieldUpdate
        (PQQuoteFields id, uint payload, ushort extendedPayload, ushort auxiliaryPayload, PQFieldFlags flag = 0)
    {
        Id               = id;
        ExtendedPayload  = extendedPayload;
        AuxiliaryPayload = auxiliaryPayload;
        var hasExtended         = ExtendedPayload > 0;
        var hasOptionalExtended = AuxiliaryPayload > 0;
        var additionalFlags = (hasExtended ? PQFieldFlags.IncludesExtendedPayLoad : PQFieldFlags.None) |
                              (hasOptionalExtended ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);

        Flag    = flag | additionalFlags;
        Payload = payload;
    }

    public PQFieldUpdate
        (PQQuoteFields id, int payload, ushort extendedPayload, ushort auxiliaryPayload, PQFieldFlags flag = 0)
        : this(id, (uint)payload, extendedPayload, auxiliaryPayload, flag) { }


    public PQFieldUpdate
        (PQQuoteFields id, PQDepthKey depthId, uint payload, ushort extendedPayload, ushort auxiliaryPayload, PQFieldFlags flag = 0)
    {
        Id               = id;
        DepthId          = depthId;
        ExtendedPayload  = extendedPayload;
        AuxiliaryPayload = auxiliaryPayload;
        var hasExtended         = ExtendedPayload > 0;
        var hasOptionalExtended = AuxiliaryPayload > 0;
        var additionalFlags = (hasExtended ? PQFieldFlags.IncludesExtendedPayLoad : PQFieldFlags.None) |
                              (hasOptionalExtended ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);

        var depthIdFlag = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags;
        Payload = payload;
    }

    public PQFieldUpdate
        (PQQuoteFields id, PQDepthKey depthId, int payload, ushort extendedPayload, ushort auxiliaryPayload, PQFieldFlags flag = 0)
        : this(id, depthId, (uint)payload, extendedPayload, auxiliaryPayload, flag) { }

    public PQFieldUpdate(PQQuoteFields id, long value, PQFieldFlags flag = 0)
    {
        Id = id;

        var scaledDownLong = PQScaling.ScaleDownLong(value);
        ExtendedPayload  = (ushort)((scaledDownLong >> 32) & 0xFF_FF);
        AuxiliaryPayload = (ushort)((scaledDownLong >> 48) & 0xFF_FF);

        var hasExtended         = ExtendedPayload > 0;
        var hasOptionalExtended = AuxiliaryPayload > 0;
        var additionalFlags = (hasExtended ? PQFieldFlags.IncludesExtendedPayLoad : PQFieldFlags.None) |
                              (hasOptionalExtended ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);
        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var scaleFactor    = (PQFieldFlags)PQScaling.FindVolumeScaleFactor(value);

        Flag    = flag | additionalFlags | isNegativeFlag | scaleFactor;
        Payload = (uint)(scaledDownLong & 0xFF_FF_FF_FF);
    }


    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, long value, PQFieldFlags flag = 0)
    {
        Id      = id;
        DepthId = depthId;

        var scaledDownLong = PQScaling.ScaleDownLong(value);
        ExtendedPayload  = (ushort)((scaledDownLong >> 32) & 0xFF_FF);
        AuxiliaryPayload = (ushort)((scaledDownLong >> 48) & 0xFF_FF);

        var hasExtended         = ExtendedPayload > 0;
        var hasOptionalExtended = AuxiliaryPayload > 0;
        var additionalFlags = (hasExtended ? PQFieldFlags.IncludesExtendedPayLoad : PQFieldFlags.None) |
                              (hasOptionalExtended ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None);
        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var scaleFactor    = (PQFieldFlags)PQScaling.FindVolumeScaleFactor(value);
        var depthIdFlag    = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | additionalFlags | isNegativeFlag | scaleFactor;
        Payload = (uint)(scaledDownLong & 0xFF_FF_FF_FF);
    }


    public PQFieldUpdate(PQQuoteFields id, decimal value, PQFieldFlags flag = 0)
    {
        Id = id;
        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        Flag    = flag | isNegativeFlag;
        Payload = PQScaling.Scale(value, Flag & PQFieldFlags.NegativeAndScaleMask);
    }


    public PQFieldUpdate(PQQuoteFields id, decimal value, ushort auxiliaryPayload, PQFieldFlags flag = 0)
    {
        Id = id;

        AuxiliaryPayload = auxiliaryPayload;

        var isNegativeFlag      = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var hasOptionalExtended = AuxiliaryPayload > 0;
        var additionalFlags     = hasOptionalExtended ? PQFieldFlags.IncludesAuxiliaryPayload : PQFieldFlags.None;
        Flag    = flag | isNegativeFlag | additionalFlags;
        Payload = PQScaling.Scale(value, Flag & PQFieldFlags.NegativeAndScaleMask);
    }


    public PQFieldUpdate(PQQuoteFields id, PQDepthKey depthId, decimal value, PQFieldFlags flag = 0)
    {
        Id      = id;
        DepthId = depthId;

        var isNegativeFlag = value < 0 ? PQFieldFlags.NegativeBit : PQFieldFlags.None;
        var depthIdFlag    = depthId > 0 ? PQFieldFlags.IncludesDepth : PQFieldFlags.None;
        Flag    = flag | depthIdFlag | isNegativeFlag;
        Payload = PQScaling.Scale(value, Flag & PQFieldFlags.NegativeAndScaleMask);
    }


    public bool Equals(PQFieldUpdate other) =>
        Flag == other.Flag && Id == other.Id && DepthId == other.DepthId && ExtendedPayload == other.ExtendedPayload
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
        $"{(ushort)(DepthId & PQDepthKey.DepthMask)}), {nameof(Payload)}: 0x{Payload.ToHex2()}, " +
        $"{nameof(ExtendedPayload)}: 0x{ExtendedPayload.ToHex2()} , {nameof(AuxiliaryPayload)}: 0x{AuxiliaryPayload.ToHex2()}, {nameof(Flag)}: {Flag}}}";
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
        if (fieldUpdate.Flag.HasBothExtendedPayloadBytesFlags()) return fieldSize + sizeof(ushort) + sizeof(ushort);
        fieldSize += fieldUpdate.Flag.HasExtendedPayloadFlag() ? sizeof(ushort) : 0;
        fieldSize += fieldUpdate.Flag.HasAuxiliaryPayloadFlag() ? sizeof(ushort) : 0;
        return fieldSize;
    }

    public static PQFieldUpdate AtDepth(this PQFieldUpdate fieldUpdate, ushort atDepth) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId | atDepth.DepthToDepthKey(), fieldUpdate.Payload,
            fieldUpdate.ExtendedPayload, fieldUpdate.AuxiliaryPayload, fieldUpdate.Flag);

    public static PQFieldUpdate WithDepth(this PQFieldUpdate fieldUpdate, PQDepthKey atDepth) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId | atDepth, fieldUpdate.Payload,
            fieldUpdate.ExtendedPayload, fieldUpdate.AuxiliaryPayload, fieldUpdate.Flag);

    public static PQFieldUpdate WithAuxiliary
        (this PQFieldUpdate fieldUpdate, ushort auxiliaryValue) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId, fieldUpdate.Payload, fieldUpdate.ExtendedPayload,
            auxiliaryValue, fieldUpdate.Flag);

    public static PQFieldUpdate SetIsAsk(this PQFieldUpdate fieldUpdate) =>
        new(fieldUpdate.Id, fieldUpdate.DepthId | PQDepthKey.AskSide, fieldUpdate.Payload,
            fieldUpdate.ExtendedPayload, fieldUpdate.AuxiliaryPayload, fieldUpdate.Flag | PQFieldFlags.IncludesDepth);

    public static long ReadAsScaledLong(this PQFieldUpdate fieldUpdate) =>
        PQScaling.UnscaleLong(((long)fieldUpdate.AuxiliaryPayload << 48) | ((long)fieldUpdate.ExtendedPayload << 32) | fieldUpdate.Payload
                            , fieldUpdate.Flag);
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
  , Upsert // Insert or update
  , Delete
}

public static class CrudCommandExtensions
{
    public static ushort      ToUShort(this CrudCommand crudCommand)       => (ushort)crudCommand;
    public static CrudCommand ToCrudCommand(this ushort ushortCrudCommand) => (CrudCommand)ushortCrudCommand;
}
