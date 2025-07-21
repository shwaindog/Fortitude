// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.Types;

namespace FortitudeCommon.Config;

public interface IConfigCombine<T> : ICloneable<T>
{
    T Combine(T mustInclude);
}