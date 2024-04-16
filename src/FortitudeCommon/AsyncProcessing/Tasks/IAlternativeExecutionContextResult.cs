#region

using FortitudeCommon.DataStructures.Memory;

#endregion

namespace FortitudeCommon.AsyncProcessing.Tasks;

public class BasicCancellationToken : RecyclableObject
{
    public bool ShouldContinue { get; private set; } = true;

    public void Cancel()
    {
        ShouldContinue = false;
    }

    public override void StateReset()
    {
        ShouldContinue = true;
    }
}

public interface IAlternativeExecutionContextAction<TP>
{
    ValueTask Execute(Action<TP> methodToExecute, TP firstParam);
    ValueTask Execute(Action<TP, BasicCancellationToken?> methodToExecute, TP firstParam, BasicCancellationToken? secondParam);
}

public interface IAlternativeExecutionContextResult<TR> : IRecyclableObject
{
    ValueTask<TR> Execute(Func<TR> methodToExecute);
    ValueTask<TR> Execute(Func<ValueTask<TR>> methodToExecute);
    ValueTask<TR> Execute(Func<BasicCancellationToken?, TR> methodToExecute, BasicCancellationToken? firstParam);
}

public interface IAlternativeExecutionContextResult<TR, TP> : IRecyclableObject
{
    ValueTask<TR> Execute(Func<TP, TR> methodToExecute, TP firstParameter);
    ValueTask<TR> Execute(Func<TP, ValueTask<TR>> methodToExecute, TP firstParameter);
    ValueTask<TR> Execute(Func<TP, BasicCancellationToken?, TR> methodToExecute, TP firstParameter, BasicCancellationToken? secondParameter);
}
