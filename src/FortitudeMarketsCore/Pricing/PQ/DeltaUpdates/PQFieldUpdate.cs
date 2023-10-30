namespace FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

public struct PQFieldUpdate
{
    public ushort Id;
    public byte Flag;
    public uint Value;

    public PQFieldUpdate(ushort id, uint value, byte flag = 0)
    {
        Id = id;
        Flag = flag;
        Value = value;
    }

    public PQFieldUpdate(byte id, uint value, byte flag = 0)
    {
        Id = id;
        Flag = flag;
        Value = value;
    }

    public PQFieldUpdate(ushort id, long value, byte flag = 0)
    {
        Id = id;
        Flag = flag;
        Value = (uint)(int)value;
    }

    public PQFieldUpdate(byte id, long value, byte flag = 0)
    {
        Id = id;
        Flag = flag;
        Value = (uint)(int)value;
    }

    public PQFieldUpdate(ushort id, decimal value, byte factor)
    {
        Id = id;
        Flag = value < 0 ? (byte)(factor | PQScaling.NegativeMask) : factor;
        Value = PQScaling.Scale(value, (byte)(Flag & 0x1F));
    }

    public PQFieldUpdate(byte id, decimal value, byte factor)
    {
        Id = id;
        Flag = value < 0 ? (byte)(factor | PQScaling.NegativeMask) : factor;
        Value = PQScaling.Scale(value, (byte)(Flag & 0x1F));
    }

    public bool Equals(PQFieldUpdate other) => Id == other.Id && Flag == other.Flag && Value == other.Value;

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
            hashCode = (hashCode * 397) ^ (int)Value;
            return hashCode;
        }
    }

    public override string ToString() =>
        $"PQFieldUpdate{{{nameof(Id)}: {Id}, {nameof(Flag)}: 0x{Flag:x2}, {nameof(Value)}: {Value} }}";
}

public struct PQFieldStringUpdate
{
    public PQFieldUpdate Field;
    public PQStringUpdate StringUpdate;

    public bool Equals(PQFieldStringUpdate other) =>
        Field.Equals(other.Field) && StringUpdate.Equals(other.StringUpdate);

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

    public override string ToString() =>
        $"PQFieldStringUpdate {{ {nameof(Field)}: {Field}, {nameof(StringUpdate)}: {StringUpdate} }}";
}

public struct PQStringUpdate
{
    public int DictionaryId;
    public CrudCommand Command;
    public string Value;

    public bool Equals(PQStringUpdate other) =>
        DictionaryId == other.DictionaryId && Command == other.Command && string.Equals(Value, other.Value);

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

public enum CrudCommand : byte
{
    None
    , Insert
    , Update
}
