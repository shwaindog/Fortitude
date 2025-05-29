// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Types.Mutable;


public interface IDiscreetUpdatable
{
    void UpdateComplete(uint updateSequenceId = 0);
}


public interface IScopedDiscreetUpdatable : IDiscreetUpdatable
{
    uint UpdateSequenceId { get; }
    void UpdateStarted(uint updateSequenceId);
}
