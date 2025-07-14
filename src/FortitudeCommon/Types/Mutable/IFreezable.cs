namespace FortitudeCommon.Types.Mutable;


public class ModifyFrozenObjectAttempt : Exception
{
    public ModifyFrozenObjectAttempt() { }
    public ModifyFrozenObjectAttempt(string? message) : base(message) { }
    public ModifyFrozenObjectAttempt(string? message, Exception? innerException) : base(message, innerException) { }
}

public interface IMaybeFrozen 
{
    bool IsFrozen { get; }
}

public interface ICanSourceThawed<out T> : IMaybeFrozen
where T : IFreezable
{
    T SourceThawed { get; }
}

public interface IFreezable : IMaybeFrozen
{
    bool ThrowOnMutateAttempt { get; set; }

    IMaybeFrozen Freeze { get; }
}

public interface IFreezable<out T> : IFreezable
    where T : IMaybeFrozen
{
    new T   Freeze            { get; }
}