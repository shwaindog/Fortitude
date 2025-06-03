// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeCommon.Types.Mutable;


public interface IDiscreetUpdatable
{
    void UpdateComplete(uint updateSequenceId = 0);
}

public interface ISequencedUpdates 
{
    uint UpdateSequenceId { get; }
}

public interface IScopedDiscreetUpdatable : IDiscreetUpdatable, ISequencedUpdates
{
    void UpdateStarted(uint updateSequenceId);
}

public interface IPartialSequenceUpdates : ISequencedUpdates
{
    bool IsCompleteUpdate { get; }
}

public interface IMutablePartialSequenceUpdates : IPartialSequenceUpdates
{
    new bool IsCompleteUpdate { get; set; }
}

public interface IStagedDeltaUpdatePhase : IScopedDiscreetUpdatable, IMutablePartialSequenceUpdates
{
    void UpdatesAppliedToAllDeltas(uint startSequenceId, uint latestSequenceId);
}


public interface IScopedTimedUpdatable : IDiscreetUpdatable, ISequencedUpdates
{
    void UpdateAt(DateTime atDateTime, uint previousSequenceId, uint latestSequenceId);
}