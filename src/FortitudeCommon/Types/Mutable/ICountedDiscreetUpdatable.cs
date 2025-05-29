// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Types.Mutable;


public interface IDiscreetUpdatable
{
    void UpdateComplete(uint updateId = 0);
}


public interface IScopedDiscreetUpdatable : IDiscreetUpdatable
{
    void UpdateStarted(uint startUpdateId);
}


public interface ICountedDiscreetUpdatable : IDiscreetUpdatable
{
    uint UpdateCount { get; }
}
