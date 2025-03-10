// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;

#endregion

namespace FortitudeCommon.Types;

public interface ITracksChanges<T> : IStoreState<T> where T : class
{
    [JsonIgnore] bool HasUpdates { get; set; }
}
