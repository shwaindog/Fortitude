// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Types.Mutable;

public interface IUpdatable
{
    uint UpdateCount { get; }
    void UpdateComplete();
}
