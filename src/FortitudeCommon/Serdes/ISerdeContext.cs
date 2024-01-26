namespace FortitudeCommon.Serdes;

[Flags]
public enum ContextDirection
{
    Unknown = 0
    , Read = 1
    , Write = 2
    , Both = 3
}

public interface ISerdeContext
{
    MarshalType MarshalType { get; }
    ContextDirection Direction { get; }
}
