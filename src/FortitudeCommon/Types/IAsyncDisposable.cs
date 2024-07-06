// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Types;

public interface IAsyncValueTaskDisposable : IDisposable, IAsyncDisposable
{
    ValueTask DisposeAwaitValueTask { get; set; }

    void IDisposable.Dispose()
    {
        DisposeAwaitValueTask = Dispose();
    }

    new ValueTask Dispose();
}

public interface IAsyncTaskDisposable : IDisposable, IAsyncDisposable
{
    Task DisposeResult { get; set; }

    void IDisposable.Dispose()
    {
        DisposeResult = Dispose();
    }

    new Task Dispose();
}
