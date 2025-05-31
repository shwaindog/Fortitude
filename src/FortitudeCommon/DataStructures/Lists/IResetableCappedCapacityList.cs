// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.DataStructures.Lists;

public interface IResetableCappedCapacityList<out T> : ICappedCapacityList<T>, IShowsEmpty
{
}

public interface IMutableResetableCappedCapacityList<T> : IResetableCappedCapacityList<T>, IMutableCappedCapacityList<T>, IEmptyable, IResetable
{
}

public interface ITracksResetCappedCapacityList<T> : IMutableResetableCappedCapacityList<T>, ITrackableReset<ITracksResetCappedCapacityList<T>>
{
}