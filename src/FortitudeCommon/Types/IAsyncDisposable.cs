namespace FortitudeCommon.Types;

public interface IAsyncValueTaskDisposable : IDisposable
{
    ValueTask DisposeResult { get; set; }

    void IDisposable.Dispose()
    {
        DisposeResult = Dispose();
    }

    new ValueTask Dispose();
}

public interface IAsyncTaskDisposable : IDisposable
{
    Task DisposeResult { get; set; }

    void IDisposable.Dispose()
    {
        DisposeResult = Dispose();
    }

    new Task Dispose();
}
