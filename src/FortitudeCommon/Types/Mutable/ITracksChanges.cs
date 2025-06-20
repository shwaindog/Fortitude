﻿// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;

#endregion

namespace FortitudeCommon.Types.Mutable;

public interface ITracksChanges : IScopedDiscreetUpdatable
{
    [JsonIgnore] bool HasUpdates { get; set; }
}

public interface ITrackableReset<out T> where T : ITrackableReset<T>
{
    T ResetWithTracking();
}